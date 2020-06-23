using System;
using System.Linq;
using System.Collections.Generic;
using ObjectNotation;


namespace GltfFormat
{
    public enum GltfAnimationPathType
    {
        Translation,
        Rotation,
        Scale, 
        Weights,
    }
 
    [Serializable]
    public class GltfAnimationTarget
    {
        [JsonSchema(Minimum = 0)]
        public int node;

        [JsonSchema(Required = true, EnumSerializationType = EnumSerializationType.AsLowerString)]
        public GltfAnimationPathType path;

        // empty schemas
        public object extensions;
        public object extras;
    }

    [Serializable]
    public class GltfAnimationChannel
    {
        [JsonSchema(Required = true, Minimum = 0)]
        public int sampler = -1;

        [JsonSchema(Required = true)]
        public GltfAnimationTarget target;

        // empty schemas
        public object extensions;
        public object extras;
    }

    public enum GltfInterpolationType
    {
        LINEAR, 
        STEP, 
        CUBICSPLINE,
    }

    [Serializable]
    public class GltfAnimationSampler
    {
        [JsonSchema(Required = true, Minimum = 0)]
        public int input = -1;

        [JsonSchema(EnumSerializationType = EnumSerializationType.AsUpperString)]
        public GltfInterpolationType interpolation;

        [JsonSchema(Required = true, Minimum = 0)]
        public int output = -1;

        // empty schemas
        public object extensions;
        public object extras;
    }

    [Serializable]
    public class GltfAnimation: IEquatable<GltfAnimation>
    {
        public string name = "";

        [JsonSchema(Required = true, MinItems = 1)]
        public List<GltfAnimationChannel> channels = new List<GltfAnimationChannel>();

        [JsonSchema(Required = true, MinItems = 1)]
        public List<GltfAnimationSampler> samplers = new List<GltfAnimationSampler>();

        // empty schemas
        public object extensions;
        public object extras;

        public bool Equals(GltfAnimation other)
        {
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals((GltfAnimation)obj);
        }

        public static bool operator ==(GltfAnimation lhs, GltfAnimation rhs)
        {
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }

        public static bool operator !=(GltfAnimation lhs, GltfAnimation rhs)
        {
            return !(lhs == rhs);
        }
    }
}
