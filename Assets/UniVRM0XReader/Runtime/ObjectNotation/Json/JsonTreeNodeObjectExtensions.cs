using System;
using System.Collections.Generic;
using System.Linq;


namespace ObjectNotation
{
    public static class IValueNodeObjectExtensions
    {
        public static IEnumerable<KeyValuePair<JsonTreeNode, JsonTreeNode>> ObjectItems(this JsonTreeNode self)
        {
            if (!self.IsObject()) throw new DeserializationException("is not object");
            var it = self.Children.GetEnumerator();
            while (it.MoveNext())
            {
                var key = it.Current;

                it.MoveNext();
                yield return new KeyValuePair<JsonTreeNode, JsonTreeNode>(key, it.Current);
            }
        }

        public static int GetObjectCount(this JsonTreeNode self)
        {
            if (!self.IsObject()) throw new DeserializationException("is not object");
            return self.Children.Count() / 2;
        }

        public static JsonTreeNode GetObjectItem(this JsonTreeNode self, String key)
        {
            return self.GetObjectItem(Utf8String.From(key));
        }

        public static JsonTreeNode GetObjectItem(this JsonTreeNode self, Utf8String key)

        {
            foreach (var kv in self.ObjectItems())
            {
                if (kv.Key.GetUtf8String() == key)
                {
                    return kv.Value;
                }
            }
            throw new KeyNotFoundException();
        }

        public static bool ContainsKey(this JsonTreeNode self, Utf8String key)
        {
            return self.ObjectItems().Any(x => x.Key.GetUtf8String() == key);
        }

        public static bool ContainsKey(this JsonTreeNode self, String key)
        {
            var ukey = Utf8String.From(key);
            return self.ContainsKey(ukey);
        }

        public static Utf8String KeyOf(this JsonTreeNode self, JsonTreeNode node)
        {
            foreach (var kv in self.ObjectItems())
            {
                if (node.ValueIndex == kv.Value.ValueIndex)
                {
                    return kv.Key.GetUtf8String();
                }
            }
            throw new KeyNotFoundException();
        }
    }
}
