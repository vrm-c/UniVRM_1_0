using System.Numerics;

namespace VrmLib
{
    public class UVScaleOffsetValue
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
    }
}
