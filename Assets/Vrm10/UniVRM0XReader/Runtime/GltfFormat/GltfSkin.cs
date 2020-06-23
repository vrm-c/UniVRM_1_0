using System;
using System.Linq;
using ObjectNotation;

namespace GltfFormat
{
    [Serializable]
    public class GltfSkin : IEquatable<GltfSkin>
    {
        [JsonSchema(Minimum = 0, ExplicitIgnorableValue = -1)]
        public int inverseBindMatrices = -1;

        [JsonSchema(Required = true, MinItems = 1)]
        [ItemJsonSchema(Minimum = 0)]
        public int[] joints;

        [JsonSchema(Minimum = 0, ExplicitIgnorableValue = -1)]
        public int skeleton = -1;

        // empty schemas
        public object extensions;
        public object extras;
        public string name;

        public bool Equals(GltfSkin other)
        {
            if(!joints.SequenceEqual(other.joints))return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GltfSkin);
        }

        public static bool operator ==(GltfSkin lhs, GltfSkin rhs)
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

        public static bool operator !=(GltfSkin lhs, GltfSkin rhs)
        {
            return !(lhs == rhs);
        }
    }
}
