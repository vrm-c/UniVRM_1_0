using System;
using System.Numerics;

namespace VrmLib
{
    public class UVScaleOffsetValue : IEquatable<UVScaleOffsetValue>
    {
        public readonly Material Material;
        public readonly Vector2 Scale; // default = [1, 1]
        public readonly Vector2 Offset; // default = [0, 0]

        public UVScaleOffsetValue(Material material, Vector2 scale, Vector2 offset)
        {
            Material = material;
            Scale = scale;
            Offset = offset;
        }

        public override int GetHashCode()
        {
            return Material.GetHashCode();
        }

        public bool Equals(UVScaleOffsetValue other)
        {
            return Material == other.Material && Scale == other.Scale && Offset == other.Offset;
        }

        /// <summary>
        /// Scaleは平均。Offsetは足す
        /// </summary>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public UVScaleOffsetValue Merge(UVScaleOffsetValue rhs)
        {
            if (Material != rhs.Material)
            {
                throw new System.Exception();
            }
            return new UVScaleOffsetValue(Material, (Scale + rhs.Scale) / 2, Offset + rhs.Offset);
        }
    }
}
