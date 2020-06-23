using System;
using System.Linq;
using System.Numerics;
using ObjectNotation;

namespace GltfFormat
{
    public struct TRS : IEquatable<TRS>
    {
        Vector3 T;
        Quaternion R;
        Vector3 S;
        public TRS(float[] m, float[] t, float[] r, float[] s)
        {
            if (m != null)
            {
                if (!Matrix4x4.Decompose(new Matrix4x4(
                    m[0], m[1], m[2], m[3],
                    m[4], m[5], m[6], m[7],
                    m[8], m[9], m[10], m[11],
                    m[12], m[13], m[14], m[15]
                    ), out S, out R, out T))
                {
                    throw new Exception("fail to Decompose");
                }
            }
            else
            {
                T = (t != null) ? new Vector3(t[0], t[1], t[2]) : Vector3.Zero;
                R = (r != null) ? new Quaternion(r[0], r[1], r[2], r[3]) : Quaternion.Identity;
                S = (s != null) ? new Vector3(s[0], s[1], s[2]) : Vector3.One;
            }
        }

        public bool Equals(TRS other)
        {
            if (T != other.T) return false;
            if (R != other.R) return false;
            if (S != other.S) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals((TRS)obj);
        }

        public static bool operator ==(TRS lhs, TRS rhs)
        {
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }

        public static bool operator !=(TRS lhs, TRS rhs)
        {
            return !(lhs == rhs);
        }
    }

    [Serializable]
    public class GltfNode : IEquatable<GltfNode>
    {
        // TODO: need an empty string?
        public string name;

        public override string ToString()
        {
            return name;
        }

        [JsonSchema(MinItems = 1)]
        [ItemJsonSchema(Minimum = 0)]
        public int[] children;

        [JsonSchema(MinItems = 16, MaxItems = 16)]
        public float[] matrix;

        [JsonSchema(MinItems = 3, MaxItems = 3)]
        public float[] translation;

        [JsonSchema(MinItems = 4, MaxItems = 4)]
        [ItemJsonSchema(Minimum = -1.0, Maximum = 1.0)]
        public float[] rotation;

        [JsonSchema(MinItems = 3, MaxItems = 3)]
        public float[] scale;

        [JsonSchema(Minimum = 0, ExplicitIgnorableValue = -1)]
        public int mesh = -1;

        [JsonSchema(Dependencies = new string[] { "mesh" }, Minimum = 0, ExplicitIgnorableValue = -1)]
        public int skin = -1;

        [JsonSchema(Dependencies = new string[] { "mesh" }, MinItems = 1)]
        public float[] weights;

        [JsonSchema(Minimum = 0, ExplicitIgnorableValue = -1)]
        public int camera = -1;

        // empty schemas
        // public GltfNodeExtensions extensions;

        // public GltfExtraNode extras = new GltfExtraNode();

        public bool Equals(GltfNode other)
        {
            if (string.IsNullOrEmpty(name))
            {
                // 処理中に名前を自動で付与している場合あり
                // OK
            }
            else if (name != other.name)
            {
                return false;
            }

            if (
                new TRS(matrix, translation, rotation, scale)
                != new TRS(other.matrix, other.translation, other.rotation, other.scale))
            {
                return false;
            }

            if (mesh != other.mesh) return false;
            if (skin != other.skin) return false;
            if (!EqualUtil.IsEqual(weights, other.weights)) return false;
            if (camera != other.camera) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GltfNode);
        }

        public static bool operator ==(GltfNode lhs, GltfNode rhs)
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

        public static bool operator !=(GltfNode lhs, GltfNode rhs)
        {
            return !(lhs == rhs);
        }
    }
}
