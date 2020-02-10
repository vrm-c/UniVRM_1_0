using System;
using ObjectNotation;

namespace GltfFormat
{
    [Serializable]
    public class GltfScene : IEquatable<GltfScene>
    {
        [JsonSchema(MinItems = 1)]
        [ItemJsonSchema(Minimum = 0)]
        public int[] nodes;

        public object extensions;
        public object extras;
        public string name;

        public bool Equals(GltfScene other)
        {
            if (!EqualUtil.IsEqual(nodes, other.nodes)) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GltfScene);
        }

        public static bool operator ==(GltfScene lhs, GltfScene rhs)
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

        public static bool operator !=(GltfScene lhs, GltfScene rhs)
        {
            return !(lhs == rhs);
        }

    }
}