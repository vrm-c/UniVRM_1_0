using System;
using System.Numerics;
using ObjectNotation;

namespace GltfFormat
{
    public enum GltfAccessorType
    {
        SCALAR,
        VEC2,
        VEC3,
        VEC4,
        MAT2,
        MAT3,
        MAT4,
    }

    public static class GltfAccessorTypeExtensions
    {
        public static int TypeCount(this GltfAccessorType type)
        {
            switch (type)
            {
                case GltfAccessorType.SCALAR:
                    return 1;
                case GltfAccessorType.VEC2:
                    return 2;
                case GltfAccessorType.VEC3:
                    return 3;
                case GltfAccessorType.VEC4:
                case GltfAccessorType.MAT2:
                    return 4;
                case GltfAccessorType.MAT3:
                    return 9;
                case GltfAccessorType.MAT4:
                    return 16;
                default:
                    throw new NotImplementedException();
            }
        }
    }

    public enum GltfComponentType : int
    {
        BYTE = 5120, // signed ?
        UNSIGNED_BYTE = 5121,

        SHORT = 5122,
        UNSIGNED_SHORT = 5123,

        //INT = 5124,
        UNSIGNED_INT = 5125,

        FLOAT = 5126,
    }

    public static class GltfComponentTypeExtensions
    {
        public static int ByteSize(this GltfComponentType t)
        {
            switch (t)
            {
                case GltfComponentType.BYTE: return 1;
                case GltfComponentType.UNSIGNED_BYTE: return 4;
                case GltfComponentType.SHORT: return 2;
                case GltfComponentType.UNSIGNED_SHORT: return 2;
                case GltfComponentType.UNSIGNED_INT: return 4;
                case GltfComponentType.FLOAT: return 4;
                default: throw new ArgumentException();
            }
        }
    }

    [Serializable]
    public class GltfAccessor
    {
        [JsonSchema(Minimum = 0)]
        public int bufferView = -1;

        [JsonSchema(Minimum = 0, Dependencies = new string[] { "bufferView" })]
        public int byteOffset;

        [JsonSchema(Required = true, EnumSerializationType = EnumSerializationType.AsUpperString)]
        public GltfAccessorType type;

        [JsonSchema(Required = true, EnumSerializationType = EnumSerializationType.AsInt)]
        public GltfComponentType componentType;

        [JsonSchema(Required = true, Minimum = 1)]
        public int count;

        [JsonSchema(MinItems = 1, MaxItems = 16)]
        public float[] max;

        [JsonSchema(MinItems = 1, MaxItems = 16)]
        public float[] min;

        public bool normalized;
        public GltfSparse sparse;

        // empty schemas
        public string name;

        public object extensions;

        public object extras;
    }

    public static class GltfAccessorExtensions
    {
        public static int GetStride(this GltfAccessor accessor)
        {
            return accessor.type.TypeCount() * accessor.componentType.ByteSize();
        }

        public static Type GetValueType(this GltfAccessor accessor)
        {
            if (accessor.type == GltfAccessorType.SCALAR)
            {
                switch (accessor.componentType)
                {
                    case GltfComponentType.BYTE: return typeof(sbyte);
                    case GltfComponentType.UNSIGNED_BYTE: return typeof(byte);
                    case GltfComponentType.SHORT: return typeof(short);
                    case GltfComponentType.UNSIGNED_SHORT: return typeof(ushort);
                    case GltfComponentType.UNSIGNED_INT: return typeof(uint);
                    case GltfComponentType.FLOAT: return typeof(float);
                }
            }
            else if (accessor.type == GltfAccessorType.VEC2)
            {
                switch (accessor.componentType)
                {
                    case GltfComponentType.FLOAT: return typeof(Vector2);
                }
            }
            else if (accessor.type == GltfAccessorType.VEC3)
            {
                switch (accessor.componentType)
                {
                    case GltfComponentType.FLOAT: return typeof(Vector3);
                }
            }
            else if (accessor.type == GltfAccessorType.VEC4)
            {
                switch (accessor.componentType)
                {
                    case GltfComponentType.UNSIGNED_SHORT: return typeof(SkinJoints);
                    case GltfComponentType.FLOAT: return typeof(Vector4);
                }
            }
            else if (accessor.type == GltfAccessorType.MAT4)
            {
                switch (accessor.componentType)
                {
                    case GltfComponentType.FLOAT: return typeof(Matrix4x4);
                }
            }

            throw new NotImplementedException();
        }
    }
}