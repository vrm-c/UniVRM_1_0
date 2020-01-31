using System;

namespace VrmLib
{
    public static class EnumUtil
    {
        public static T Parse<T>(string src, bool ignoreCase = true) where T : struct
        {
            if (string.IsNullOrEmpty(src))
            {
                return default(T);
            }

            return (T)Enum.Parse(typeof(T), src, ignoreCase);
        }
    }
}