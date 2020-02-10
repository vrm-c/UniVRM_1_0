using System;


namespace ObjectNotation
{
    public static class StringExtensions
    {
        public static JsonTreeNode ParseAsJson(this string json)
        {
            return JsonParser.Parse(json);
        }
        public static JsonTreeNode ParseAsJson(this Utf8String json)
        {
            return JsonParser.Parse(json);
        }
        public static JsonTreeNode ParseAsJson(this byte[] bytes)
        {
            return JsonParser.Parse(new Utf8String(bytes));
        }
        public static JsonTreeNode ParseAsJson(this ArraySegment<byte> bytes)
        {
            return JsonParser.Parse(new Utf8String(bytes));
        }
    }
}
