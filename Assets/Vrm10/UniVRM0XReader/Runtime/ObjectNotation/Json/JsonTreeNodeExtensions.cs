using System.Collections.Generic;


namespace ObjectNotation
{
    public static class JsonTreeNodeExtensions
    {
        #region IValue
        public static bool IsNull(this JsonTreeNode self)
        {
            return self.Value.ValueType == ValueNodeType.Null;
        }

        public static bool IsBoolean(this JsonTreeNode self)
        {
            return self.Value.ValueType == ValueNodeType.Boolean;
        }

        public static bool IsString(this JsonTreeNode self)
        {
            return self.Value.ValueType == ValueNodeType.String;
        }

        public static bool IsInteger(this JsonTreeNode self)
        {
            return self.Value.ValueType == ValueNodeType.Integer;
        }

        public static bool IsFloat(this JsonTreeNode self)
        {
            return self.Value.ValueType == ValueNodeType.Number
                   || self.Value.ValueType == ValueNodeType.NaN
                   || self.Value.ValueType == ValueNodeType.Infinity
                   || self.Value.ValueType == ValueNodeType.MinusInfinity;
        }

        public static bool IsArray(this JsonTreeNode self)
        {
            return self.Value.ValueType == ValueNodeType.Array;
        }

        public static bool IsObject(this JsonTreeNode self)
        {
            return self.Value.ValueType == ValueNodeType.Object;
        }

        public static bool GetBoolean(this JsonTreeNode self) { return self.Value.GetBoolean(); }
        public static string GetString(this JsonTreeNode self) { return self.Value.GetString(); }
        public static Utf8String GetUtf8String(this JsonTreeNode self) { return self.Value.GetUtf8String(); }
        public static sbyte GetSByte(this JsonTreeNode self) { return self.Value.GetSByte(); }
        public static short GetInt16(this JsonTreeNode self) { return self.Value.GetInt16(); }
        public static int GetInt32(this JsonTreeNode self) { return self.Value.GetInt32(); }
        public static long GetInt64(this JsonTreeNode self) { return self.Value.GetInt64(); }
        public static byte GetByte(this JsonTreeNode self) { return self.Value.GetByte(); }
        public static ushort GetUInt16(this JsonTreeNode self) { return self.Value.GetUInt16(); }
        public static uint GetUInt32(this JsonTreeNode self) { return self.Value.GetUInt32(); }
        public static ulong GetUInt64(this JsonTreeNode self) { return self.Value.GetUInt64(); }
        public static float GetSingle(this JsonTreeNode self) { return self.Value.GetSingle(); }
        public static double GetDouble(this JsonTreeNode self) { return self.Value.GetDouble(); }

        /// <summary>
        /// for UnitTest. Use explicit GetT() or Deserialize(ref T)
        /// </summary>
        /// <returns></returns>
        // public static object GetValue(this JsonTreeNode self)
        // {
        //     return self.Value.GetValue<object>();
        // }
        #endregion

        public static IEnumerable<JsonTreeNode> Traverse(this JsonTreeNode self)
        {
            yield return self;
            if (self.IsArray())
            {
                foreach (var x in self.ArrayItems())
                {
                    foreach (var y in x.Traverse())
                    {
                        yield return y;
                    }
                }
            }
            else if (self.IsObject())
            {
                foreach (var kv in self.ObjectItems())
                {
                    foreach (var y in kv.Value.Traverse())
                    {
                        yield return y;
                    }
                }
            }
        }
    }
}
