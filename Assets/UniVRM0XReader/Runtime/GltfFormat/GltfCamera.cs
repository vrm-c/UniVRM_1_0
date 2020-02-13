using System;
using ObjectNotation;

namespace GltfFormat
{
    public enum GltfProjectionType
    {
        Perspective,
        Orthographic
    }

    [Serializable]
    public class GltfOrthographic
    {
        [JsonSchema(Required = true)]
        public float xmag;
        [JsonSchema(Required = true)]
        public float ymag;
        [JsonSchema(Required = true, Minimum = 0.0f, ExclusiveMinimum = true)]
        public float zfar;
        [JsonSchema(Required = true, Minimum = 0.0f)]
        public float znear;

        // [JsonSchema(MinProperties = 1)]
        // public GltfExtensionsCameraOrthographic extensions;
        // [JsonSchema(MinProperties = 1)]
        // public glTFOrthographic_extras extras;
    }

    [Serializable]
    public class GltfPerspective
    {
        [JsonSchema(Minimum = 0.0f, ExclusiveMinimum = true)]
        public float aspectRatio;
        [JsonSchema(Required = true, Minimum = 0.0f, ExclusiveMinimum = true)]
        public float yfov;
        [JsonSchema(Minimum = 0.0f, ExclusiveMinimum = true)]
        public float zfar;
        [JsonSchema(Required = true, Minimum = 0.0f, ExclusiveMinimum = true)]
        public float znear;

        // public glTFPerspective_extensions extensions;
        // public glTFPerspective_extras extras;
    }

    [Serializable]
    public class GltfCamera
    {
        public GltfOrthographic orthographic;
        public GltfPerspective perspective;

        [JsonSchema(Required = true, EnumSerializationType = EnumSerializationType.AsLowerString)]
        public GltfProjectionType type;

        public string name;

        // public glTFCamera_extensions extensions;
        // public glTFCamera_extras extras;
    }
}
