using System;
using System.Collections.Generic;
using ObjectNotation;


namespace GltfFormat
{
    [Serializable]
    [JsonSchema(Title = "vrm.firstperson.degreemap")]
    public class VrmDegreeMap : IEquatable<VrmDegreeMap>
    {
        [JsonSchema(Description = "None linear mapping params. time, value, inTangent, outTangent")]
        public float[] curve;

        [JsonSchema(Description = "Look at input clamp range degree.")]
        public float xRange = 90.0f;

        [JsonSchema(Description = "Look at map range degree from xRange.")]
        public float yRange = 10.0f;

        public bool Equals(VrmDegreeMap other)
        {
            if (other is null)
            {
                return false;
            }

            if (!EqualUtil.IsEqual(curve, other.curve)) return false;
            if (xRange != other.xRange) return false;
            if (yRange != other.yRange) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as VrmDegreeMap);
        }

        public static bool operator ==(VrmDegreeMap lhs, VrmDegreeMap rhs)
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

        public static bool operator !=(VrmDegreeMap lhs, VrmDegreeMap rhs)
        {
            return !(lhs == rhs);
        }
    }

    public enum VrmFirstPersonFlag
    {
        Auto, // Create headlessModel
        Both, // Default layer
        ThirdPersonOnly,
        FirstPersonOnly,
    }

    [Serializable]
    [JsonSchema(Title = "vrm.firstperson.meshannotation")]
    public class VrmMeshAnnotation : IEquatable<VrmMeshAnnotation>
    {
        // When the value is -1, it means that no target mesh is found.
        [JsonSchema(Minimum = 0)]
        public int mesh;

        [JsonSchema(Description = "", EnumSerializationType = EnumSerializationType.AsString)]
        public VrmFirstPersonFlag firstPersonFlag;

        public bool Equals(VrmMeshAnnotation other)
        {
            if (mesh != other.mesh) return false;
            if (firstPersonFlag != other.firstPersonFlag) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as VrmMeshAnnotation);
        }

        public static bool operator ==(VrmMeshAnnotation lhs, VrmMeshAnnotation rhs)
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

        public static bool operator !=(VrmMeshAnnotation lhs, VrmMeshAnnotation rhs)
        {
            return !(lhs == rhs);
        }
    }

    public enum VrmLookAtType
    {
        Bone,
        BlendShape,
    }

    [Serializable]
    [JsonSchema(Title = "vrm.firstperson")]
    public class VrmFirstPerson : IEquatable<VrmFirstPerson>
    {
        // When the value is -1, it means that no bone for first person is found.
        [JsonSchema(Description = "The bone whose rendering should be turned off in first-person view. Usually Head is specified.", Minimum = 0, ExplicitIgnorableValue = -1)]
        public int firstPersonBone = -1;

        [JsonSchema(Description = @"The target position of the VR headset in first-person view. It is assumed that an offset from the head bone to the VR headset is added.")]
        public System.Numerics.Vector3 firstPersonBoneOffset;

        [JsonSchema(Description = "Switch display / undisplay for each mesh in first-person view or the others.")]
        public List<VrmMeshAnnotation> meshAnnotations = new List<VrmMeshAnnotation>();

        // lookat
        [JsonSchema(Description = "Eye controller mode.", EnumSerializationType = EnumSerializationType.AsString)]
        public VrmLookAtType lookAtTypeName = VrmLookAtType.Bone;

        [JsonSchema(Description = "Eye controller setting.")]
        public VrmDegreeMap lookAtHorizontalInner = new VrmDegreeMap();

        [JsonSchema(Description = "Eye controller setting.")]
        public VrmDegreeMap lookAtHorizontalOuter = new VrmDegreeMap();

        [JsonSchema(Description = "Eye controller setting.")]
        public VrmDegreeMap lookAtVerticalDown = new VrmDegreeMap();

        [JsonSchema(Description = "Eye controller setting.")]
        public VrmDegreeMap lookAtVerticalUp = new VrmDegreeMap();

        public bool Equals(VrmFirstPerson other)
        {
            if (other is null)
            {
                return false;
            }

            if (firstPersonBone != other.firstPersonBone) return false;
            if (firstPersonBoneOffset != other.firstPersonBoneOffset) return false;
            if (!EqualUtil.IsEqual(meshAnnotations, other.meshAnnotations)) return false;
            if (lookAtTypeName != other.lookAtTypeName) return false;
            if (lookAtHorizontalInner != other.lookAtHorizontalInner) return false;
            if (lookAtHorizontalOuter != other.lookAtHorizontalOuter) return false;
            if (lookAtVerticalDown != other.lookAtVerticalDown) return false;
            if (lookAtVerticalUp != other.lookAtVerticalUp) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as VrmFirstPerson);
        }

        public static bool operator ==(VrmFirstPerson lhs, VrmFirstPerson rhs)
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

        public static bool operator !=(VrmFirstPerson lhs, VrmFirstPerson rhs)
        {
            return !(lhs == rhs);
        }
    }
}
