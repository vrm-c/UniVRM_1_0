using System;
using System.Collections.Generic;
using System.Numerics;
using ObjectNotation;


namespace GltfFormat
{
    [Serializable]
    public class VrmSecondaryAnimationCollider : IEquatable<VrmSecondaryAnimationCollider>
    {
        [JsonSchema(Description = "The local coordinate from the node of the collider group.")]
        public Vector3 offset;

        [JsonSchema(Description = "The radius of the collider.")]
        public float radius;

        public bool Equals(VrmSecondaryAnimationCollider other)
        {
            if (offset != other.offset) return false;
            if (radius != other.radius) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as VrmSecondaryAnimationCollider);
        }

        public static bool operator ==(VrmSecondaryAnimationCollider lhs, VrmSecondaryAnimationCollider rhs)
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

        public static bool operator !=(VrmSecondaryAnimationCollider lhs, VrmSecondaryAnimationCollider rhs)
        {
            return !(lhs == rhs);
        }
    }


    [Serializable]
    [JsonSchema(Title = "vrm.secondaryanimation.collidergroup", Description = @"Set sphere balls for colliders used for collision detections with swaying objects.")]
    public class VrmSecondaryAnimationColliderGroup : IEquatable<VrmSecondaryAnimationColliderGroup>
    {
        [JsonSchema(Description = "The node of the collider group for setting up collision detections.", Minimum = 0)]
        public int node;

        public List<VrmSecondaryAnimationCollider> colliders = new List<VrmSecondaryAnimationCollider>();

        public bool Equals(VrmSecondaryAnimationColliderGroup other)
        {
            if (node != other.node) return false;
            if (!EqualUtil.IsEqual(colliders, other.colliders)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as VrmSecondaryAnimationColliderGroup);
        }

        public static bool operator ==(VrmSecondaryAnimationColliderGroup lhs, VrmSecondaryAnimationColliderGroup rhs)
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

        public static bool operator !=(VrmSecondaryAnimationColliderGroup lhs, VrmSecondaryAnimationColliderGroup rhs)
        {
            return !(lhs == rhs);
        }
    }


    [Serializable]
    [JsonSchema(Title = "vrm.secondaryanimation.spring")]
    public class VrmSecondaryAnimationGroup : IEquatable<VrmSecondaryAnimationGroup>
    {
        [JsonSchema(Description = "Annotation comment")]
        public string comment;

        [JsonSchema(Description = "The resilience of the swaying object (the power of returning to the initial pose).")]
        public float stiffiness;

        [JsonSchema(Description = "The strength of gravity.")]
        public float gravityPower;

        [JsonSchema(Description = "The direction of gravity. Set (0, -1, 0) for simulating the gravity. Set (1, 0, 0) for simulating the wind.")]
        public Vector3 gravityDir;

        [JsonSchema(Description = "The resistance (deceleration) of automatic animation.")]
        public float dragForce;

        // NOTE: This value denotes index but may contain -1 as a value.
        // When the value is -1, it means that center node is not specified.
        // This is a historical issue and a compromise for forward compatibility.
        [JsonSchema(Description = @"The reference point of a swaying object can be set at any location except the origin. When implementing UI moving with warp, the parent node to move with warp can be specified if you don't want to make the object swaying with warp movement.")]
        public int center;

        [JsonSchema(Description = "The radius of the sphere used for the collision detection with colliders.")]
        public float hitRadius;

        [JsonSchema(Description = "Specify the node index of the root bone of the swaying object.")]
        [ItemJsonSchema(Minimum = 0)]
        public int[] bones = new int[] { };

        [JsonSchema(Description = "Specify the index of the collider group for collisions with swaying objects.")]
        [ItemJsonSchema(Minimum = 0)]
        public int[] colliderGroups = new int[] { };

        public bool Equals(VrmSecondaryAnimationGroup other)
        {
            if (comment != other.comment) return false;
            if (stiffiness != other.stiffiness) return false;
            if (gravityPower != other.gravityPower) return false;
            if (gravityDir != other.gravityDir) return false;
            if (dragForce != other.dragForce) return false;
            if (center != other.center) return false;
            if (hitRadius != other.hitRadius) return false;
            if (!EqualUtil.IsEqual(bones, other.bones)) return false;
            if (!EqualUtil.IsEqual(colliderGroups, other.colliderGroups)) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as VrmSecondaryAnimationGroup);
        }

        public static bool operator ==(VrmSecondaryAnimationGroup lhs, VrmSecondaryAnimationGroup rhs)
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

        public static bool operator !=(VrmSecondaryAnimationGroup lhs, VrmSecondaryAnimationGroup rhs)
        {
            return !(lhs == rhs);
        }
    }

    [Serializable]
    [JsonSchema(Title = "vrm.secondaryanimation", Description = "The setting of automatic animation of string-like objects such as tails and hairs.")]
    public class VrmSecondaryAnimation : IEquatable<VrmSecondaryAnimation>
    {
        [JsonSchema(ExplicitIgnorableItemLength = 0)]
        public List<VrmSecondaryAnimationGroup> boneGroups = new List<VrmSecondaryAnimationGroup>();

        [JsonSchema(ExplicitIgnorableItemLength = 0)]
        public List<VrmSecondaryAnimationColliderGroup> colliderGroups = new List<VrmSecondaryAnimationColliderGroup>();

        public bool Equals(VrmSecondaryAnimation other)
        {
            if(other is null){
                return false;
            }

            if (!EqualUtil.IsEqual(colliderGroups, other.colliderGroups)) return false;
            if (!EqualUtil.IsEqual(boneGroups, other.boneGroups)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as VrmSecondaryAnimation);
        }

        public static bool operator ==(VrmSecondaryAnimation lhs, VrmSecondaryAnimation rhs)
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

        public static bool operator !=(VrmSecondaryAnimation lhs, VrmSecondaryAnimation rhs)
        {
            return !(lhs == rhs);
        }
    }
}
