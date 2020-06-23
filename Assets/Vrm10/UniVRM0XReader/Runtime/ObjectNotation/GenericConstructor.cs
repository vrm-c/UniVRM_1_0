using System;
using System.Reflection;


namespace ObjectNotation
{
    struct GenericConstructor<U>
    {
        static V[] ArrayCreator<V>(JsonTreeNode src)
        {
            if (!src.IsArray())
            {
                throw new ArgumentException("value is not array");
            }
            var count = src.GetArrayCount();
            return new V[count];
        }

        static Func<JsonTreeNode, U> GetCreator()
        {
            var t = typeof(U);
            if (t.IsArray)
            {
                var mi = typeof(GenericConstructor<U>).GetMethod("ArrayCreator",
                    BindingFlags.NonPublic | BindingFlags.Static);
                var g = mi.MakeGenericMethod(t.GetElementType());
                return GenericInvokeCallFactory.StaticFunc<JsonTreeNode, U>(g);
            }

            {
                return _s =>
                {
                    return Activator.CreateInstance<U>();
                };
            }
        }

        delegate U Creator(JsonTreeNode src);

        static Creator s_creator;

        public U Create(JsonTreeNode src)
        {
            if (s_creator == null)
            {
                var d = GetCreator();
                s_creator = new Creator(d);
            }
            return s_creator(src);
        }
    }
}
