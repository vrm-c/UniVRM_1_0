using System;
using UnityEngine;

namespace UniVRM10
{
    [Serializable]
    public struct MaterialValueBinding : IEquatable<MaterialValueBinding>
    {
        public String MaterialName;
        public String ValueName;
        public Vector4 TargetValue;
        public Vector4 BaseValue;

        public bool Equals(MaterialValueBinding other)
        {
            return string.Equals(MaterialName, other.MaterialName) && string.Equals(ValueName, other.ValueName) && TargetValue.Equals(other.TargetValue) && BaseValue.Equals(other.BaseValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is MaterialValueBinding && Equals((MaterialValueBinding)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (MaterialName != null ? MaterialName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ValueName != null ? ValueName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ TargetValue.GetHashCode();
                hashCode = (hashCode * 397) ^ BaseValue.GetHashCode();
                return hashCode;
            }
        }
    }
}
