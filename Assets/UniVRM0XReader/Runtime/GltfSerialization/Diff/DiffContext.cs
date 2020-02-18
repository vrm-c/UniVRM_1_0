using System.Collections.Generic;
using System.Linq;

namespace GltfSerialization
{
    public struct DiffContext
    {
        public static bool IsNullOrEmpty<T>(Dictionary<string, T> array)
        {
            if (array is null)
            {
                return true;
            }
            if (array.Count == 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsNullOrEmpty<T>(List<T> array)
        {
            if (array is null)
            {
                return true;
            }
            if (array.Count == 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsNullOrEmpty<T>(T[] array)
        {
            if (array is null)
            {
                return true;
            }
            if (array.Length == 0)
            {
                return true;
            }
            return false;
        }

        public List<KeyValuePair<string, string>> List;
        public object[] Context;

        string GetPath()
        {
            return "/" + string.Join("/", Context.Select(x => x.ToString()));
        }

        public void Add(string diff)
        {
            List.Add(new KeyValuePair<string, string>(GetPath(), diff));
        }

        public void Add(string key, string diff)
        {
            List.Add(new KeyValuePair<string, string>($"{GetPath()}/{key}", diff));
        }

        public DiffContext GetChild(object o)
        {
            return new DiffContext
            {
                List = List,
                Context = (Context == null)
                ? new object[] { o }
                : Context.Concat(new object[] { o }).ToArray()
            };
        }
    }
}