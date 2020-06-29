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

        public static T Cast<T>(object src, bool ignoreCase = true) where T : struct
        {
            if (src is null)
            {
                throw new ArgumentNullException();
            }

            return (T)Enum.Parse(typeof(T), src.ToString(), ignoreCase);
        }
    }
}