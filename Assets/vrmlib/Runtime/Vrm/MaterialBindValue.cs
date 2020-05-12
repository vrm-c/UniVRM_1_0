using System;
using System.Numerics;

namespace VrmLib
{
    public class MaterialBindValue
    {
        public readonly Material Material;

        /// <summary>
        /// BlendShapeClip の Material変化の対象
        /// </summary>
        public readonly MaterialBindType BindType;

        /// <summary>
        /// VRM-0.x で使っていた。
        /// VRM-1.0 では廃止して、MaterialBindType を使う
        /// </summary>       
        public string Property => BindType.GetProperty(Material);

        public readonly Vector4 Value;

        public MaterialBindValue(Material material, String property, Vector4 value)
        {
            Material = material;
            BindType = material.GetBindType(property);
            Value = value;
        }

        public MaterialBindValue(Material material, MaterialBindType bindType, Vector4 value)
        {
            Material = material;
            BindType = bindType;
            Value = value;
        }
    }
}
