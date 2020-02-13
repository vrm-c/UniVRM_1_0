using System;
using System.Collections.Generic;
using System.Linq;


namespace ObjectNotation
{
    public static class JsonTreeNodeJsonPointerExtensions
    {
        public static void SetValue(this JsonTreeNode self, 
            Utf8String jsonPointer, ArraySegment<Byte> bytes)
        {
            foreach (var node in self.GetNodes(jsonPointer))
            {
                node.SetValue(default(JsonValue).New(
                    bytes,
                    ValueNodeType.Boolean,
                    node.Value.ParentIndex));
            }
        }

        public static void RemoveValue(this JsonTreeNode self, Utf8String jsonPointer)
        {
            foreach (var node in self.GetNodes(new JsonPointer(jsonPointer)))
            {
                if (node.Parent.IsObject())
                {
                    node.Prev.SetValue(default(JsonValue)); // remove key
                }
                node.SetValue(default(JsonValue)); // remove
            }
        }

        public static JsonPointer Pointer(this JsonTreeNode self)
        {
            return JsonPointer.Create(self);
        }

        public static IEnumerable<JsonTreeNode> Path(this JsonTreeNode self)
        {
            if (self.HasParent)
            {
                foreach (var x in self.Parent.Path())
                {
                    yield return x;
                }
            }
            yield return self;
        }

        public static IEnumerable<JsonTreeNode> GetNodes(this JsonTreeNode self, 
            JsonPointer jsonPointer)
        {
            if (jsonPointer.Path.Count == 0)
            {
                yield return self;
                yield break;
            }

            if (self.IsArray())
            {
                // array
                if (jsonPointer[0][0] == '*')
                {
                    // wildcard
                    foreach (var child in self.ArrayItems())
                    {
                        foreach (var childChild in child.GetNodes(jsonPointer.Unshift()))
                        {
                            yield return childChild;
                        }
                    }
                }
                else
                {
                    int index = jsonPointer[0].ToInt32();
                    var child = self.ArrayItems().Skip(index).First();
                    foreach (var childChild in child.GetNodes(jsonPointer.Unshift()))
                    {
                        yield return childChild;
                    }
                }
            }
            else if (self.IsObject())
            {
                // object
                if (jsonPointer[0][0] == '*')
                {
                    // wildcard
                    foreach (var kv in self.ObjectItems())
                    {
                        foreach (var childChild in kv.Value.GetNodes(jsonPointer.Unshift()))
                        {
                            yield return childChild;
                        }
                    }
                }
                else
                {
                    JsonTreeNode child;
                    try
                    {
                        child = self.ObjectItems().First(x => x.Key.GetUtf8String() == jsonPointer[0]).Value;
                    }
                    catch (Exception)
                    {
                        // key
                        self.AddKey(jsonPointer[0]);
                        // value
                        self.AddValue(default(ArraySegment<byte>), ValueNodeType.Object);

                        child = self.ObjectItems().First(x => x.Key.GetUtf8String() == jsonPointer[0]).Value;
                    }
                    foreach (var childChild in child.GetNodes(jsonPointer.Unshift()))
                    {
                        yield return childChild;
                    }
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static IEnumerable<JsonTreeNode> GetNodes(this JsonTreeNode self, 
            Utf8String jsonPointer) 
        {
            return self.GetNodes(new JsonPointer(jsonPointer));
        }
    }
}
