using System;
using ObjectNotation;

namespace GltfFormat
{
    public enum GltfBufferTargetType : int
    {
        NONE = 0,
        ARRAY_BUFFER = 34962,
        ELEMENT_ARRAY_BUFFER = 34963,
    }

    [Serializable]
    public class GltfBufferView
    {
        [JsonSchema(Required = true, Minimum = 0)]
        public int buffer;

        [JsonSchema(Minimum = 0)]
        public int byteOffset;

        [JsonSchema(Required = true, Minimum = 1)]
        public int byteLength;

        [JsonSchema(Minimum = 4, Maximum = 252, MultipleOf = 4)]
        public int byteStride;

        [JsonSchema(EnumSerializationType = EnumSerializationType.AsInt, EnumExcludes = new object[] { GltfBufferTargetType.NONE })]
        public GltfBufferTargetType target;

        // empty schemas
        public object extensions;
        public object extras;
        public string name;
    }
}