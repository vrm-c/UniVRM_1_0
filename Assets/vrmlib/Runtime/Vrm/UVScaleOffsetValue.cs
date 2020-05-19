using System.Numerics;

namespace VrmLib
{
    public class UVScaleOffsetValue
    {
        public readonly Material Material;
        readonly Vector2 Scale; // default = [1, 1]
        readonly Vector2 Offset; // default = [0, 0]

        public UVScaleOffsetValue(Material material, Vector2 scale, Vector2 offset)
        {
            Material = material;
            Scale = scale;
            Offset = offset;
        }
    }
}
