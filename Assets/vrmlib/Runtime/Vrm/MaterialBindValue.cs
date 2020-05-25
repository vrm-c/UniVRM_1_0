using System;
using System.Collections.Generic;
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
        /// Unity仕様の Property名 + Vector4
        /// </summary>       
        public KeyValuePair<string, Vector4> Property
        {
            get
            {
                // switch (BindType)
                // {
                //     case MaterialBindType.UvScale:
                //         return new KeyValuePair<string, Vector4>(
                //             MaterialBindTypeExtensions.UV_PROPERTY,
                //             new Vector4(m_value.X, m_value.Y, 0, 0)
                //             );

                //     case MaterialBindType.UvOffset:
                //         return new KeyValuePair<string, Vector4>(
                //             MaterialBindTypeExtensions.UV_PROPERTY,
                //             new Vector4(1, 1, m_value.X, m_value.Y)
                //             );
                // }

                return new KeyValuePair<string, Vector4>(BindType.GetProperty(Material), m_value);
            }
        }

        readonly Vector4 m_value;

        // public MaterialBindValue(Material material, String property, Vector4 value)
        // {
        //     Material = material;
        //     BindType = material.GetBindType(property);
        //     m_value = value;
        // }

        public MaterialBindValue(Material material, MaterialBindType bindType, Vector4 value)
        {
            Material = material;
            BindType = bindType;
            m_value = value;
        }
    }
}
