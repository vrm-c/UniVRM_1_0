using System;
using System.Collections.Generic;
using System.Linq;
using ObjectNotation;

namespace GltfFormat
{
    [Serializable]
    public class GltfAttributes : IEquatable<GltfAttributes>
    {
        [JsonSchema(Minimum = 0, ExplicitIgnorableValue = -1)]
        public int POSITION = -1;

        [JsonSchema(Minimum = 0, ExplicitIgnorableValue = -1)]
        public int NORMAL = -1;

        [JsonSchema(Minimum = 0, ExplicitIgnorableValue = -1)]
        public int TANGENT = -1;

        [JsonSchema(Minimum = 0, ExplicitIgnorableValue = -1)]
        public int TEXCOORD_0 = -1;

        [JsonSchema(Minimum = 0, ExplicitIgnorableValue = -1)]
        public int COLOR_0 = -1;

        [JsonSchema(Minimum = 0, ExplicitIgnorableValue = -1)]
        public int JOINTS_0 = -1;

        [JsonSchema(Minimum = 0, ExplicitIgnorableValue = -1)]
        public int WEIGHTS_0 = -1;

        public bool Equals(GltfAttributes rhs)
        {
            if (rhs is null)
            {
                return false;
            }
            
            return POSITION == rhs.POSITION
                && NORMAL == rhs.NORMAL
                && TANGENT == rhs.TANGENT
                && TEXCOORD_0 == rhs.TEXCOORD_0
                && COLOR_0 == rhs.COLOR_0
                && JOINTS_0 == rhs.JOINTS_0
                && WEIGHTS_0 == rhs.WEIGHTS_0
                ;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GltfAttributes);
        }

        public static bool operator ==(GltfAttributes lhs, GltfAttributes rhs)
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

        public static bool operator !=(GltfAttributes lhs, GltfAttributes rhs)
        {
            return !(lhs == rhs);
        }
    }

    [Serializable]
    public class GltfMorphTarget
    {
        [JsonSchema(Minimum = 0, ExplicitIgnorableValue = -1)]
        public int POSITION = -1;

        [JsonSchema(Minimum = 0, ExplicitIgnorableValue = -1)]
        public int NORMAL = -1;

        [JsonSchema(Minimum = 0, ExplicitIgnorableValue = -1)]
        public int TANGENT = -1;
    }

    public enum GltfPrimitiveMode : int
    {
        Points = 0,
        Lines = 1,
        LineLoop = 2,
        LineStrip = 3,
        Triangles = 4,
        TriangleStrip = 5,
        TriangleFan = 6,
    }

    /// <summary>
    /// https://github.com/KhronosGroup/glTF/blob/master/specification/2.0/schema/mesh.primitive.schema.json
    /// </summary>
    [Serializable]
    public class GltfPrimitive : IEquatable<GltfPrimitive>
    {
        [JsonSchema(EnumSerializationType = EnumSerializationType.AsInt)]
        public GltfPrimitiveMode mode = GltfPrimitiveMode.Triangles;

        [JsonSchema(Minimum = 0, ExplicitIgnorableValue = -1)]
        public int indices = -1;

        [JsonSchema(Required = true, SkipSchemaComparison = true)]
        public GltfAttributes attributes = new GltfAttributes();

        [JsonSchema(Minimum = 0)]
        public int material;

        [JsonSchema(MinItems = 1, ExplicitIgnorableItemLength = 0)]
        [ItemJsonSchema(SkipSchemaComparison = true)]
        public List<GltfMorphTarget> targets = new List<GltfMorphTarget>();

        public GltfPrimitive_Extras extras = new GltfPrimitive_Extras();

        // [JsonSchema(SkipSchemaComparison = true)]
        // public glTFPrimitives_extensions extensions = null;

        public bool Equals(GltfPrimitive other)
        {
            if (other == null)
            {
                return false;
            }

            if (mode != other.mode) return false;
            //if (indices != other.indices) return false;
            //if (attributes != other.attributes) return false;
            if (material != other.material) return false;
            if (targets == null)
            {
                if (other.targets != null) return false;
            }
            else
            {
                //if (!targets.SequenceEqual(other.targets)) return false;
                if (targets.Count != other.targets.Count) return false;
            }

            // if (extensions != other.extensions) return false;
            if (extras != other.extras) return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GltfMesh);
        }

        public static bool operator ==(GltfPrimitive lhs, GltfPrimitive rhs)
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

        public static bool operator !=(GltfPrimitive lhs, GltfPrimitive rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class GltfPrimitivesExtensions
    {
        public static bool HasVertexColor(this GltfPrimitive p)
        {
            return p.attributes != null && p.attributes.COLOR_0 != -1;
        }

        public static bool HasSameVertexBuffer(this GltfPrimitive lhs, GltfPrimitive rhs)
        {
            return lhs.attributes == rhs.attributes;
        }

        public static void SetAttribute(this GltfPrimitive prim, string semantic, int accessorIndex)
        {
            switch (semantic)
            {
                case "POSITION": prim.attributes.POSITION = accessorIndex; break;
                case "NORMAL": prim.attributes.NORMAL = accessorIndex; break;
                case "TANGENT": prim.attributes.TANGENT = accessorIndex; break;
                case "TEXCOORD_0": prim.attributes.TEXCOORD_0 = accessorIndex; break;
                case "COLOR_0": prim.attributes.COLOR_0 = accessorIndex; break;
                case "JOINTS_0": prim.attributes.JOINTS_0 = accessorIndex; break;
                case "WEIGHTS_0": prim.attributes.WEIGHTS_0 = accessorIndex; break;
                default: throw new KeyNotFoundException($"unknown key: {semantic}");
            }
        }
    }

    /// <summary>
    /// https://github.com/KhronosGroup/glTF/issues/1036
    /// </summary>
    [Serializable]
    public class GltfPrimitive_Extras : IEquatable<GltfPrimitive_Extras>
    {
        [JsonSchema(Required = true, MinItems = 1)]
        public List<string> targetNames = new List<string>();

        public bool Equals(GltfPrimitive_Extras other)
        {
            if (other == null)
            {
                return false;
            }

            if (targetNames == null)
            {
                if (other.targetNames != null) return false;
            }
            else
            {
                if (!targetNames.SequenceEqual(other.targetNames)) return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GltfPrimitive_Extras);
        }

        public static bool operator ==(GltfPrimitive_Extras lhs, GltfPrimitive_Extras rhs)
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

        public static bool operator !=(GltfPrimitive_Extras lhs, GltfPrimitive_Extras rhs)
        {
            return !(lhs == rhs);
        }

    }

    [Serializable]
    public class GltfMesh : IEquatable<GltfMesh>
    {
        public string name;

        [JsonSchema(Required = true, MinItems = 1)]
        public List<GltfPrimitive> primitives = new List<GltfPrimitive>();

        // A Morph Target may also define an optional mesh.weights property that stores the default targets weights. 
        // In the absence of a node.weights property, the primitives attributes are resolved using these weights. 
        // When this property is missing, the default targets weights are assumed to be zero.
        [JsonSchema(MinItems = 1)]
        public float[] weights;

        // empty schemas
        public object extensions;
        public object extras;

        public bool Equals(GltfMesh other)
        {
            if (name != other.name) return false;
            if (!primitives.SequenceEqual(other.primitives)) return false;
            // if (weights == null)
            // {
            //     if (other.weights != null) return false;
            // }
            // else
            // {
            //     if (!ArrayUtil.IsEqual(weights, other.weights)) return false;
            // }

            if (extensions != other.extensions) return false;
            if (extras != other.extras) return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GltfMesh);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(GltfMesh lhs, GltfMesh rhs)
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

        public static bool operator !=(GltfMesh lhs, GltfMesh rhs)
        {
            return !(lhs == rhs);
        }
    }

    public static class GltfMeshExtensions
    {
        public static bool AllPrimitivesHasSameVertexBuffer(this GltfMesh m)
        {
            if (m.primitives.Count <= 1)
            {
                return true;
            }

            var first = m.primitives[0];
            for (int i = 1; i < m.primitives.Count; ++i)
            {
                if (!first.HasSameVertexBuffer(m.primitives[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
