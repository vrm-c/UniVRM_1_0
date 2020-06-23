using System;
using System.Collections.Generic;
using ObjectNotation;


namespace GltfFormat
{
    [Serializable]
    [JsonSchema(Title = "vrm.blendshape.materialbind")]
    public class VrmMaterialValueBind: IEquatable<VrmMaterialValueBind>
    {
        public string materialName;
        public string propertyName;
        public float[] targetValue;

        public bool Equals(VrmMaterialValueBind other)
        {
            if(materialName!=other.materialName){
                return false;
            }

            if(propertyName!=other.propertyName){
                return false;
            }

            if(!EqualUtil.IsEqual(targetValue, other.targetValue)){
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as VrmMaterialValueBind);
        }

        public static bool operator ==(VrmMaterialValueBind lhs, VrmMaterialValueBind rhs)
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

        public static bool operator !=(VrmMaterialValueBind lhs, VrmMaterialValueBind rhs)
        {
            return !(lhs == rhs);
        }
    }

    [Serializable]
    [JsonSchema(Title = "vrm.blendshape.bind")]
    public class VrmBlendShapeBind: IEquatable<VrmBlendShapeBind>
    {
        [JsonSchema(Required = true, Minimum = 0)]
        public int mesh = -1;

        [JsonSchema(Required = true, Minimum = 0)]
        public int index = -1;

        [JsonSchema(Required = true, Minimum = 0, Maximum = 100, Description = @"SkinnedMeshRenderer.SetBlendShapeWeight")]
        public float weight = 0;

        public bool Equals(VrmBlendShapeBind other)
        {
            if(mesh!=other.mesh)return false;
            if(index!=other.index)return false;
            if(weight!=other.weight)return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as VrmBlendShapeBind);
        }

        public static bool operator ==(VrmBlendShapeBind lhs, VrmBlendShapeBind rhs)
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

        public static bool operator !=(VrmBlendShapeBind lhs, VrmBlendShapeBind rhs)
        {
            return !(lhs == rhs);
        }
    }

    public enum VrmBlendShapePreset
    {
        Unknown,

        Neutral,

        A,
        I,
        U,
        E,
        O,

        Blink,

        // 喜怒哀楽
        Joy,
        Angry,
        Sorrow,
        Fun,

        // LookAt
        LookUp,
        LookDown,
        LookLeft,
        LookRight,

        Blink_L,
        Blink_R,
    }

    [Serializable]
    [JsonSchema(Title = "vrm.blendshape.group", Description = "BlendShapeClip of UniVRM")]
    public class VrmBlendShapeGroup : IEquatable<VrmBlendShapeGroup>
    {
        [JsonSchema(Description = "Expression name")]
        public string name;

        [JsonSchema(Description = "Predefined Expression name", EnumSerializationType = EnumSerializationType.AsLowerString)]
        public VrmBlendShapePreset presetName;

        [JsonSchema(Description = "Low level blendshape references. ")]
        public List<VrmBlendShapeBind> binds = new List<VrmBlendShapeBind>();

        [JsonSchema(Description = "Material animation references.")]
        public List<VrmMaterialValueBind> materialValues = new List<VrmMaterialValueBind>();

        [JsonSchema(Description = "0 or 1. Do not allow an intermediate value. Value should rounded")]
        public bool isBinary;

        public bool Equals(VrmBlendShapeGroup other)
        {
            if(name!=other.name){
                return false;
            }

            if(presetName!=other.presetName){
                return false;
            }

            if (!EqualUtil.IsEqual(binds, other.binds))
            {
                return false;
            }
            if (!EqualUtil.IsEqual(materialValues, other.materialValues))
            {
                return false;
            }

            if (isBinary != other.isBinary)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as VrmBlendShapeGroup);
        }

        public static bool operator ==(VrmBlendShapeGroup lhs, VrmBlendShapeGroup rhs)
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

        public static bool operator !=(VrmBlendShapeGroup lhs, VrmBlendShapeGroup rhs)
        {
            return !(lhs == rhs);
        }
    }

    [Serializable]
    [JsonSchema(Title = "vrm.blendshape", Description = "BlendShapeAvatar of UniVRM")]
    public class VrmBlendShapeMaster : IEquatable<VrmBlendShapeMaster>
    {
        public List<VrmBlendShapeGroup> blendShapeGroups = new List<VrmBlendShapeGroup>();

        public bool Equals(VrmBlendShapeMaster other)
        {
            if (other is null)
            {
                return false;
            }

            if (!EqualUtil.IsEqual(blendShapeGroups, other.blendShapeGroups))
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as VrmBlendShapeMaster);
        }

        public static bool operator ==(VrmBlendShapeMaster lhs, VrmBlendShapeMaster rhs)
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

        public static bool operator !=(VrmBlendShapeMaster lhs, VrmBlendShapeMaster rhs)
        {
            return !(lhs == rhs);
        }
    }
}
