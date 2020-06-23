using System;
using ObjectNotation;

namespace GltfFormat
{
    [Serializable]
    public class GltfSparseIndices
    {
        [JsonSchema(Required = true, Minimum = 0)]
        public int bufferView = -1;

        [JsonSchema(Minimum = 0)]
        public int byteOffset;

        [JsonSchema(Required = true)]
        public GltfComponentType componentType;

        // empty schemas
        public object extensions;
        public object extras;
    }

    [Serializable]
    public class GltfSparseValues
    {
        [JsonSchema(Required = true, Minimum = 0)]
        public int bufferView = -1;

        [JsonSchema(Minimum = 0)]
        public int byteOffset;

        // empty schemas
        public object extensions;
        public object extras;
    }

    [Serializable]
    public class GltfSparse
    {
        [JsonSchema(Required = true, Minimum = 1)]
        public int count;

        [JsonSchema(Required = true)]
        public GltfSparseIndices indices;

        [JsonSchema(Required = true)]
        public GltfSparseValues values;

        // empty schemas
        public object extensions;
        public object extras;
    }
}