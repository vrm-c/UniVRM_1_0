using System;
using ObjectNotation;


namespace GltfFormat
{
    public enum GltfMagFilterType : int
    {
        NEAREST = 9728,
        LINEAR = 9729
    }
    public enum GltfMinFilterType : int
    {
        NEAREST = 9728,
        LINEAR = 9729,

        NEAREST_MIPMAP_NEAREST = 9984,
        LINEAR_MIPMAP_NEAREST = 9985,
        NEAREST_MIPMAP_LINEAR = 9986,
        LINEAR_MIPMAP_LINEAR = 9987,
    }

    public enum GltfWrapType : int
    {
        REPEAT = 10497,
        CLAMP_TO_EDGE = 33071,
        MIRRORED_REPEAT = 33648,
    }

    /// https://github.com/KhronosGroup/glTF/blob/master/specification/2.0/schema/sampler.schema.json
    [Serializable]
    public class GltfTextureSampler
    {
        [JsonSchema(EnumSerializationType = EnumSerializationType.AsInt)]
        public GltfMagFilterType magFilter = GltfMagFilterType.NEAREST;

        [JsonSchema(EnumSerializationType = EnumSerializationType.AsInt)]
        public GltfMinFilterType minFilter = GltfMinFilterType.NEAREST;

        [JsonSchema(EnumSerializationType = EnumSerializationType.AsInt)]
        public GltfWrapType wrapS = GltfWrapType.REPEAT;

        [JsonSchema(EnumSerializationType = EnumSerializationType.AsInt)]
        public GltfWrapType wrapT = GltfWrapType.REPEAT;

        // empty schemas
        public object extensions;
        public object extras;
        public string name;
    }
}