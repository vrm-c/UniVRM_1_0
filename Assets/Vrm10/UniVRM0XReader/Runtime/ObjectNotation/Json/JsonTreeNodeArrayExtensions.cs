using System.Collections.Generic;
using System.Linq;


namespace ObjectNotation
{
    public static class JsonTreeNodeArrayExtensions
    {
        public static IEnumerable<JsonTreeNode> ArrayItems(this JsonTreeNode self)
        {
            if (!self.IsArray()) throw new DeserializationException("is not array");
            return self.Children;
        }

        public static JsonTreeNode GetArrrayItem(this JsonTreeNode self, int index)
        {
            int i = 0;
            foreach (var v in self.ArrayItems())
            {
                if (i++ == index)
                {
                    return v;
                }
            }
            throw new KeyNotFoundException();
        }

        public static int GetArrayCount(this JsonTreeNode self)
        {
            if (!self.IsArray()) throw new DeserializationException("is not array");
            return self.Children.Count();
        }

        public static int IndexOf(this JsonTreeNode self, JsonTreeNode child)
        {
            int i = 0;
            foreach (var v in self.ArrayItems())
            {
                if (v.ValueIndex == child.ValueIndex)
                {
                    return i;
                }
                ++i;
            }
            throw new KeyNotFoundException();
        }
    }
}
