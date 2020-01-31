using System;
using System.Collections.Generic;
using System.Numerics;

namespace VrmLib
{
    public class MToonMaterial : UnlitMaterial
    {
        public override LinearColor BaseColorFactor
       {
            get => Definition.Color.LitColor;
            set => Definition.Color.LitColor = value;
        }

        public override TextureInfo BaseColorTexture
        {
            get => Definition.Color.LitMultiplyTexture;
            set => Definition.Color.LitMultiplyTexture = value;
        }

        public override AlphaModeType AlphaMode
        {
            get
            {
                switch (Definition.Rendering.RenderMode)
                {
                    case MToon.RenderMode.Opaque: return AlphaModeType.OPAQUE;
                    case MToon.RenderMode.Cutout: return AlphaModeType.MASK;
                    case MToon.RenderMode.Transparent: return AlphaModeType.BLEND;
                    case MToon.RenderMode.TransparentWithZWrite: return AlphaModeType.BLEND_ZWRITE;
                    default: throw new NotImplementedException();
                }
            }
            set
            {
                switch (value)
                {
                    case AlphaModeType.OPAQUE: Definition.Rendering.RenderMode = MToon.RenderMode.Opaque; break;
                    case AlphaModeType.MASK: Definition.Rendering.RenderMode = MToon.RenderMode.Cutout; break;
                    case AlphaModeType.BLEND: Definition.Rendering.RenderMode = MToon.RenderMode.Transparent; break;
                    case AlphaModeType.BLEND_ZWRITE: Definition.Rendering.RenderMode = MToon.RenderMode.TransparentWithZWrite; break;
                    default: throw new NotImplementedException();
                }
            }
        }

        public override float AlphaCutoff
        {
            get => Definition.Color.CutoutThresholdValue;
            set => Definition.Color.CutoutThresholdValue = value;
        }

        public MToonMaterial(string name) : base(name)
        {
        }

        public const string ExtensionName = "VRMC_materials_mtoon";

        public override string ToString()
        {
            return "[MTOON]";
        }

        public float _DebugMode;
        public float _SrcBlend;
        public float _DstBlend;
        public float _ZWrite;

        public Dictionary<string, bool> KeyWords = new Dictionary<string, bool>();

        bool? MTOON_OUTLINE_COLORED;
        bool? MTOON_OUTLINE_COLOR_MIXED;
        bool? MTOON_OUTLINE_WIDTH_WORLD;
        bool? _ALPHABLEND_ON;

        string RenderType;

        MToon.CullMode _OutlineCullMode;

        public MToon.MToonDefinition Definition;

        public override bool CanIntegrate(Material _rhs)
        {
            var rhs = _rhs as MToonMaterial;
            if (rhs == null)
            {
                return false;
            }

            if (!Definition.Equals(rhs.Definition)) return false;

            return true;
        }

        public const string MToonShaderName = "VRM/MToon";

    }
}