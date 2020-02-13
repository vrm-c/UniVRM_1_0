using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GltfFormat;
using ObjectNotation;


namespace GltfFormat
{


    [Serializable]
    [JsonSchema(Title = "vrm.humanoid.bone")]
    public class VrmHumanoidBone : IEquatable<VrmHumanoidBone>
    {
        public override string ToString()
        {
            return $"VrmHumanoidBone: {bone} => {node}";
        }

        [JsonSchema(Description = "Human bone name.", EnumSerializationType = EnumSerializationType.AsString)]
        public HumanoidBones bone;

        // When the value is -1, it means that no node is found.
        [JsonSchema(Description = "Reference node index", Minimum = 0)]
        public int node = -1;

        [JsonSchema(Description = "Unity's HumanLimit.useDefaultValues")]
        public bool useDefaultValues = true;

        [JsonSchema(Description = "Unity's HumanLimit.min")]
        public Vector3 min;

        [JsonSchema(Description = "Unity's HumanLimit.max")]
        public Vector3 max;

        [JsonSchema(Description = "Unity's HumanLimit.center")]
        public Vector3 center;

        [JsonSchema(Description = "Unity's HumanLimit.axisLength")]
        public float axisLength;

        public bool Equals(VrmHumanoidBone other)
        {
            if (other is null) return false;
            if (node != other.node) return false;
            if (bone != other.bone) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals((VrmHumanoidBone)obj);
        }

        public static bool operator ==(VrmHumanoidBone lhs, VrmHumanoidBone rhs)
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

        public static bool operator !=(VrmHumanoidBone lhs, VrmHumanoidBone rhs)
        {
            return !(lhs == rhs);
        }
    }

    [Serializable]
    [JsonSchema(Title = "vrm.humanoid")]
    public class VrmHumanoid : IEquatable<VrmHumanoid>
    {
        public List<VrmHumanoidBone> humanBones = new List<VrmHumanoidBone>();

        [JsonSchema(Description = "Unity's HumanDescription.armStretch")]
        public float armStretch = 0.05f;

        [JsonSchema(Description = "Unity's HumanDescription.legStretch")]
        public float legStretch = 0.05f;

        [JsonSchema(Description = "Unity's HumanDescription.upperArmTwist")]
        public float upperArmTwist = 0.5f;

        [JsonSchema(Description = "Unity's HumanDescription.lowerArmTwist")]
        public float lowerArmTwist = 0.5f;

        [JsonSchema(Description = "Unity's HumanDescription.upperLegTwist")]
        public float upperLegTwist = 0.5f;

        [JsonSchema(Description = "Unity's HumanDescription.lowerLegTwist")]
        public float lowerLegTwist = 0.5f;

        [JsonSchema(Description = "Unity's HumanDescription.feetSpacing")]
        public float feetSpacing = 0;

        [JsonSchema(Description = "Unity's HumanDescription.hasTranslationDoF")]
        public bool hasTranslationDoF = false;

        public bool Equals(VrmHumanoid other)
        {
            if (other is null) return false;

            if (!humanBones.OrderBy(x => x.bone).SequenceEqual(other.humanBones.OrderBy(x => x.bone))) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals((VrmHumanoid)obj);
        }

        public static bool operator ==(VrmHumanoid lhs, VrmHumanoid rhs)
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

        public static bool operator !=(VrmHumanoid lhs, VrmHumanoid rhs)
        {
            return !(lhs == rhs);
        }
    }
}
