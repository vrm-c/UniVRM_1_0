using System;
using System.Numerics;
using ObjectNotation;

namespace GltfFormat
{
    public enum GltfTextureTypes
    {
        BaseColor,
        Metallic,
        Normal,
        Occlusion,
        Emissive,
        Unknown
    }

    public interface IGltfTextureinfo
    {
        GltfTextureTypes TextreType { get; }
    }

    [Serializable]
    public abstract class GltfTextureInfo : IGltfTextureinfo
    {
        [JsonSchema(Required = true, Minimum = 0)]
        public int index = -1;

        [JsonSchema(Minimum = 0)]
        public int texCoord;

        // empty schemas
        public object extensions;
        public object extras;

        public abstract GltfTextureTypes TextreType { get; }
    }


    [Serializable]
    public class GltfMaterialBaseColorTextureInfo : GltfTextureInfo
    {
        public override GltfTextureTypes TextreType
        {
            get { return GltfTextureTypes.BaseColor; }
        }
    }

    [Serializable]
    public class GltfMaterialMetallicRoughnessTextureInfo : GltfTextureInfo
    {
        public override GltfTextureTypes TextreType
        {
            get { return GltfTextureTypes.Metallic; }
        }
    }

    [Serializable]
    public class GltfMaterialNormalTextureInfo : GltfTextureInfo
    {
        public float scale = 1.0f;

        public override GltfTextureTypes TextreType
        {
            get { return GltfTextureTypes.Normal; }
        }
    }

    [Serializable]
    public class GltfMaterialOcclusionTextureInfo : GltfTextureInfo
    {
        [JsonSchema(Minimum = 0.0, Maximum = 1.0)]
        public float strength = 1.0f;

        public override GltfTextureTypes TextreType
        {
            get { return GltfTextureTypes.Occlusion; }
        }
    }

    [Serializable]
    public class GltfMaterialEmissiveTextureInfo : GltfTextureInfo
    {
        public override GltfTextureTypes TextreType
        {
            get { return GltfTextureTypes.Emissive; }
        }
    }

    public struct PBR
    {
    }

    [Serializable]
    public class GltfPbrMetallicRoughness : IEquatable<GltfPbrMetallicRoughness>
    {
        public GltfMaterialBaseColorTextureInfo baseColorTexture = null;

        [JsonSchema(MinItems = 4, MaxItems = 4)]
        [ItemJsonSchema(Minimum = 0.0, Maximum = 1.0)]
        public float[] baseColorFactor;

        public GltfMaterialMetallicRoughnessTextureInfo metallicRoughnessTexture = null;

        [JsonSchema(Minimum = 0.0, Maximum = 1.0)]
        public float metallicFactor = 1.0f;

        [JsonSchema(Minimum = 0.0, Maximum = 1.0)]
        public float roughnessFactor = 1.0f;

        // empty schemas
        public object extensions;
        public object extras;

        public bool Equals(GltfPbrMetallicRoughness other)
        {
            if (other is null)
            {
                return false;
            }

            var lBaseColorFactor = baseColorFactor != null
            ? new Vector4(baseColorFactor[0], baseColorFactor[1], baseColorFactor[2], baseColorFactor[3])
            : Vector4.One
            ;
            var rBaseColorFactor = other.baseColorFactor != null
            ? new Vector4(other.baseColorFactor[0], other.baseColorFactor[1], other.baseColorFactor[2], other.baseColorFactor[3])
            : Vector4.One
            ;
            if (lBaseColorFactor != rBaseColorFactor)
            {
                return false;
            }

            //if (baseColorTexture != other.baseColorTexture) return false;
            //if (metallicRoughnessTexture != other.metallicRoughnessTexture) return false;
            if (metallicFactor != other.metallicFactor) return false;
            if (roughnessFactor != other.roughnessFactor) return false;

            if (extensions != other.extensions) return false;
            if (extras != other.extras) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GltfPbrMetallicRoughness);
        }

        public static bool operator ==(GltfPbrMetallicRoughness lhs, GltfPbrMetallicRoughness rhs)
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

        public static bool operator !=(GltfPbrMetallicRoughness lhs, GltfPbrMetallicRoughness rhs)
        {
            return !(lhs == rhs);
        }
    }

    public enum AlphaModeType
    {
        OPAQUE,
        MASK,
        BLEND,
    }

    [Serializable]
    public class GltfMaterialExtension_KHR_materials_unlit
    {
        public static string ExtensionName
        {
            get
            {
                return "KHR_materials_unlit";
            }
        }

        public static GltfMaterial CreateDefault()
        {
            return new GltfMaterial
            {
                pbrMetallicRoughness = new GltfPbrMetallicRoughness
                {
                    baseColorFactor = new float[] { 1.0f, 1.0f, 1.0f, 1.0f },
                    roughnessFactor = 0.9f,
                    metallicFactor = 0.0f,
                },
                extensions = new GltfMaterial_Extensions
                {
                    KHR_materials_unlit = new GltfMaterialExtension_KHR_materials_unlit(),
                },
            };
        }
    }

    [Serializable]
    public class GltfMaterial_Extensions
    {
        [JsonSchema(Required = true)]
        public GltfMaterialExtension_KHR_materials_unlit KHR_materials_unlit;

        public bool Equals(GltfMaterial_Extensions other)
        {
            if (other is null)
            {
                return false;
            }

            if ((KHR_materials_unlit is null) != (other.KHR_materials_unlit is null)) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GltfMaterial_Extensions);
        }

        public static bool operator ==(GltfMaterial_Extensions lhs, GltfMaterial_Extensions rhs)
        {
            // Check for null on left side.
            if (lhs is null)
            {
                if (rhs is null)
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

        public static bool operator !=(GltfMaterial_Extensions lhs, GltfMaterial_Extensions rhs)
        {
            return !(lhs == rhs);
        }
    }

    [Serializable]
    public class GltfMaterial : IEquatable<GltfMaterial>
    {
        public string name;
        public GltfPbrMetallicRoughness pbrMetallicRoughness;
        public GltfMaterialNormalTextureInfo normalTexture = null;

        public GltfMaterialOcclusionTextureInfo occlusionTexture = null;

        public GltfMaterialEmissiveTextureInfo emissiveTexture = null;

        [JsonSchema(MinItems = 3, MaxItems = 3)]
        [ItemJsonSchema(Minimum = 0.0, Maximum = 1.0)]
        public float[] emissiveFactor;

        [JsonSchema(EnumSerializationType = EnumSerializationType.AsUpperString)]
        public AlphaModeType alphaMode;

        [JsonSchema(Dependencies = new string[] { "alphaMode" }, Minimum = 0.0)]
        public float alphaCutoff = 0.5f;

        public bool doubleSided;

        [JsonSchema(SkipSchemaComparison = true)]
        public GltfMaterial_Extensions extensions;
        public object extras;

        public bool Equals(GltfMaterial other)
        {
            // If parameter is null, return false.
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }

            // Optimization for a common success case.
            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != other.GetType())
            {
                return false;
            }

            if (name != other.name) return false;
            if (pbrMetallicRoughness != other.pbrMetallicRoughness) return false;
            // if (normalTexture != other.normalTexture) return false;
            // if (occlusionTexture != other.occlusionTexture) return false;

            var lEmissiveFactor = emissiveFactor != null
                ? new Vector3(emissiveFactor[0], emissiveFactor[1], emissiveFactor[2])
                : Vector3.Zero
                ;
            var rEmissiveFactor = other.emissiveFactor != null
                ? new Vector3(other.emissiveFactor[0], other.emissiveFactor[1], other.emissiveFactor[2])
                : Vector3.Zero
                ;
            if (lEmissiveFactor != rEmissiveFactor) return false;

            if (alphaMode != other.alphaMode) return false;

            if (alphaMode != AlphaModeType.OPAQUE)
            {
                if (alphaCutoff != other.alphaCutoff) return false;
            }

            if (doubleSided != other.doubleSided) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GltfMaterial);
        }

        public static bool operator ==(GltfMaterial lhs, GltfMaterial rhs)
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

        public static bool operator !=(GltfMaterial lhs, GltfMaterial rhs)
        {
            return !(lhs == rhs);
        }

        public static GltfMaterial CreateDefault(string name)
        {
            return new GltfMaterial
            {
                name = name,
                pbrMetallicRoughness = new GltfPbrMetallicRoughness
                {

                },
            };
        }
    }

    public static class GltfMaterialExtensions
    {
        public static bool IsUnlit(this GltfMaterial m)
        {
            return m.extensions != null
            && m.extensions.KHR_materials_unlit != null;
        }

        public static GltfTextureInfo[] GetTextures(this GltfMaterial m)
        {
            return new GltfTextureInfo[]
            {
                m.pbrMetallicRoughness.baseColorTexture,
                m.pbrMetallicRoughness.metallicRoughnessTexture,
                m.normalTexture,
                m.occlusionTexture,
                m.emissiveTexture
            };
        }
    }
}
