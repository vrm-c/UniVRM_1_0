using System;
using System.Collections.Generic;
using System.Linq;
using GltfFormat;
using ObjectNotation;


namespace GltfFormat
{
    [Serializable]
    [JsonSchema(Title = "vrm.material")]
    public class VrmMaterial : IEquatable<VrmMaterial>
    {
        public string name;
        public string shader;
        public int renderQueue = -1;

        public Dictionary<string, float> floatProperties = new Dictionary<string, float>();
        public Dictionary<string, float[]> vectorProperties = new Dictionary<string, float[]>();
        public Dictionary<string, int> textureProperties = new Dictionary<string, int>();
        public Dictionary<string, bool> keywordMap = new Dictionary<string, bool>();
        public Dictionary<string, string> tagMap = new Dictionary<string, string>();

        public static readonly string VRM_USE_GLTFSHADER = "VRM_USE_GLTFSHADER";

        static Utf8String s_floatProperties = Utf8String.From("floatProperties");
        static Utf8String s_vectorProperties = Utf8String.From("vectorProperties");
        static Utf8String s_keywordMap = Utf8String.From("keywordMap");
        static Utf8String s_tagMap = Utf8String.From("tagMap");
        static Utf8String s_textureProperties = Utf8String.From("textureProperties");

        public bool Equals(VrmMaterial other)
        {
            if (other == null)
            {
                return false;
            }

            if (name != other.name) return false;
            if (shader != other.shader) return false;
            if (renderQueue != other.renderQueue) return false;

            if (!EqualUtil.IsEqual(floatProperties, other.floatProperties)) return false;
            if (!EqualUtil.IsEqual(vectorProperties, other.vectorProperties)) return false;
            if (!EqualUtil.IsEqual(textureProperties, other.textureProperties)) return false;
            if (!EqualUtil.IsEqual(keywordMap, other.keywordMap)) return false;
            if (!EqualUtil.IsEqual(tagMap, other.tagMap)) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as VrmMaterial);
        }

        public static bool operator ==(VrmMaterial lhs, VrmMaterial rhs)
        {
            // Check for null on left side.
            if (Object.ReferenceEquals(lhs, null))
            {
                if (Object.ReferenceEquals(rhs, null))
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }

        public static bool operator !=(VrmMaterial lhs, VrmMaterial rhs)
        {
            return !(lhs == rhs);
        }
    }

}
