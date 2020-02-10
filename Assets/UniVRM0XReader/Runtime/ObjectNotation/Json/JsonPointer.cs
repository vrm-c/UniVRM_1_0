using System;
using System.Linq;
using System.Text;


namespace ObjectNotation
{
    public struct JsonPointer
    {
        public ArraySegment<Utf8String> Path
        {
            get;
            private set;
        }

        public int Count
        {
            get
            {
                return Path.Count;
            }
        }

        public Utf8String this[int index]
        {
            get
            {
                return Path.Array[Path.Offset + index];
            }
        }

        public JsonPointer Unshift()
        {
            return new JsonPointer
            {
                Path = new ArraySegment<Utf8String>(Path.Array, Path.Offset + 1, Path.Count - 1)
            };
        }

        public static JsonPointer Create(JsonTreeNode node)
        {
            return new JsonPointer
            {
                Path = new ArraySegment<Utf8String>(node.Path().Skip(1).Select(x => GetKeyFromParent(x)).ToArray())
            };
        }

        public JsonPointer(Utf8String pointer) : this()
        {
            int pos;
            if (!pointer.TrySearchAscii((Byte)'/', 0, out pos))
            {
                throw new ArgumentException();
            }
            if (pos != 0)
            {
                throw new ArgumentException();
            }

            var splited = pointer.Split((Byte)'/').ToArray();
            Path = new ArraySegment<Utf8String>(splited, 1, splited.Length - 1);
        }

        public override string ToString()
        {
            if (Path.Count == 0)
            {
                return "/";
            }

            var sb = new StringBuilder();
            var end = Path.Offset + Path.Count;
            for (int i = Path.Offset; i < end; ++i)
            {
                sb.Append('/');
                sb.Append(Path.Array[i]);
            }
            return sb.ToString();
        }

        static Utf8String GetKeyFromParent(JsonTreeNode json)
        {
            var parent = json.Parent;
            if (parent.IsArray())
            {
                var index = parent.IndexOf(json);
                return Utf8String.From(index);
            }
            else if (parent.IsObject())
            {
                return parent.KeyOf(json);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
