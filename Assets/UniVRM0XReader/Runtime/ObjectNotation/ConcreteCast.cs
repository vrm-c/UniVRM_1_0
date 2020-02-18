using System;
using System.Reflection;


namespace ObjectNotation
{
    public static partial class ConcreteCast
    {
        public static string GetMethodName(Type src, Type dst)
        {
            return string.Format("Cast{0}To{1}", src.Name, dst.Name);
        }

        public static MethodInfo GetMethod(Type src, Type dst)
        {
            var name = GetMethodName(src, dst);
            var mi = typeof(ConcreteCast).GetMethod(name, 
                BindingFlags.Static | BindingFlags.Public);
            return mi;
        }
    }
}
