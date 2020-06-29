using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace System
{
    /// <summary>
    /// .NETCore-3.1 とのギャップを埋める
    /// </summary>
    public static class Extensions
    {
        public static void Deconstruct<T, U>(this KeyValuePair<T, U> pair, out T key, out U value)
        {
            key = pair.Key;
            value = pair.Value;
        }

        public static void Write(this Stream s, byte[] bytes)
        {
            s.Write(bytes, 0, bytes.Length);
        }

        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue addValue)
        {
            bool canAdd = !dict.ContainsKey(key);

            if (canAdd)
                dict.Add(key, addValue);

            return canAdd;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this System.Collections.Generic.IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary.TryGetValue(key, out TValue value))
            {
                return value;
            }
            else
            {
                return default;
            }
        }

        public static ArraySegment<T> Slice<T>(this ArraySegment<T> src, int offset, int count)
        {
            return new ArraySegment<T>(src.Array, src.Offset + offset, count);
        }
    }
}
