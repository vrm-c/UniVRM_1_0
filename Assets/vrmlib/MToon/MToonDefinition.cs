using System.Numerics;
using System;
using System.Collections.Generic;
using VrmLib;

namespace VrmLib.MToon
{
    public class MToonDefinition : IEquatable<MToonDefinition>
    {
        public MetaDefinition Meta;
        public RenderingDefinition Rendering;
        public ColorDefinition Color;
        public LightingDefinition Lighting;
        public EmissionDefinition Emission;
        public MatCapDefinition MatCap;
        public RimDefinition Rim;
        public OutlineDefinition Outline;
        public TextureUvCoordsDefinition TextureOption;

        public bool Equals(MToonDefinition other)
        {
            if (other is null) return false;
            if (!EqualityComparer<MetaDefinition>.Default.Equals(Meta, other.Meta)) return false;
            if (!EqualityComparer<RenderingDefinition>.Default.Equals(Rendering, other.Rendering)) return false;
            if (!EqualityComparer<ColorDefinition>.Default.Equals(Color, other.Color)) return false;
            if (!EqualityComparer<LightingDefinition>.Default.Equals(Lighting, other.Lighting)) return false;
            if (!EqualityComparer<EmissionDefinition>.Default.Equals(Emission, other.Emission)) return false;
            if (!EqualityComparer<MatCapDefinition>.Default.Equals(MatCap, other.MatCap)) return false;
            if (!EqualityComparer<RimDefinition>.Default.Equals(Rim, other.Rim)) return false;
            if (!EqualityComparer<OutlineDefinition>.Default.Equals(Outline, other.Outline)) return false;
            if (!EqualityComparer<TextureUvCoordsDefinition>.Default.Equals(TextureOption, other.TextureOption)) return false;

            return true;
        }
    }

    public class MetaDefinition : IEquatable<MetaDefinition>
    {
        public string Implementation;
        public int VersionNumber;


        public bool Equals(MetaDefinition other)
        {
            return other != null &&
                   Implementation == other.Implementation &&
                   VersionNumber == other.VersionNumber;
        }
    }

    public class RenderingDefinition : IEquatable<RenderingDefinition>
    {
        public RenderMode RenderMode;
        public CullMode CullMode;
        public int RenderQueueOffsetNumber;


        public bool Equals(RenderingDefinition other)
        {
            return other != null &&
                   RenderMode == other.RenderMode &&
                   CullMode == other.CullMode &&
                   RenderQueueOffsetNumber == other.RenderQueueOffsetNumber;
        }
    }

    public class ColorDefinition : IEquatable<ColorDefinition>
    {
        public LinearColor LitColor = LinearColor.White;
        public TextureInfo LitMultiplyTexture;
        public LinearColor ShadeColor = LinearColor.White;
        public TextureInfo ShadeMultiplyTexture;
        public float CutoutThresholdValue;

        public bool Equals(ColorDefinition other)
        {
            return other != null &&
                   LitColor.Equals(other.LitColor) &&
                   EqualityComparer<TextureInfo>.Default.Equals(LitMultiplyTexture, other.LitMultiplyTexture) &&
                   ShadeColor.Equals(other.ShadeColor) &&
                   EqualityComparer<TextureInfo>.Default.Equals(ShadeMultiplyTexture, other.ShadeMultiplyTexture) &&
                   CutoutThresholdValue == other.CutoutThresholdValue;
        }
    }

    public class LightingDefinition : IEquatable<LightingDefinition>
    {
        public LitAndShadeMixingDefinition LitAndShadeMixing;
        public LightingInfluenceDefinition LightingInfluence;
        public NormalDefinition Normal;

        public bool Equals(LightingDefinition other)
        {
            return other != null &&
                   EqualityComparer<LitAndShadeMixingDefinition>.Default.Equals(LitAndShadeMixing, other.LitAndShadeMixing) &&
                   EqualityComparer<LightingInfluenceDefinition>.Default.Equals(LightingInfluence, other.LightingInfluence) &&
                   EqualityComparer<NormalDefinition>.Default.Equals(Normal, other.Normal);
        }
    }

    public class LitAndShadeMixingDefinition : IEquatable<LitAndShadeMixingDefinition>
    {
        public float ShadingShiftValue;
        public float ShadingToonyValue;
        // ToDo: Delete at 1.0 
        //public float ShadowReceiveMultiplierValue;
        //public TextureInfo ShadowReceiveMultiplierMultiplyTexture;
        //public float LitAndShadeMixingMultiplierValue;
        //public TextureInfo LitAndShadeMixingMultiplierMultiplyTexture;

        public bool Equals(LitAndShadeMixingDefinition other)
        {
            return other != null &&
                   ShadingShiftValue == other.ShadingShiftValue &&
                   ShadingToonyValue == other.ShadingToonyValue;
                   //ShadowReceiveMultiplierValue == other.ShadowReceiveMultiplierValue &&
                   //EqualityComparer<TextureInfo>.Default.Equals(ShadowReceiveMultiplierMultiplyTexture, other.ShadowReceiveMultiplierMultiplyTexture) &&
                   //LitAndShadeMixingMultiplierValue == other.LitAndShadeMixingMultiplierValue &&
                   //EqualityComparer<TextureInfo>.Default.Equals(LitAndShadeMixingMultiplierMultiplyTexture, other.LitAndShadeMixingMultiplierMultiplyTexture);
        }
    }

    public class LightingInfluenceDefinition : IEquatable<LightingInfluenceDefinition>
    {
        public float LightColorAttenuationValue;
        public float GiIntensityValue;


        public bool Equals(LightingInfluenceDefinition other)
        {
            return other != null &&
                   LightColorAttenuationValue == other.LightColorAttenuationValue &&
                   GiIntensityValue == other.GiIntensityValue;
        }
    }

    public class EmissionDefinition : IEquatable<EmissionDefinition>
    {
        public LinearColor EmissionColor = LinearColor.Black;
        public TextureInfo EmissionMultiplyTexture;


        public bool Equals(EmissionDefinition other)
        {
            return other != null &&
                   EmissionColor.Equals(other.EmissionColor) &&
                   EqualityComparer<TextureInfo>.Default.Equals(EmissionMultiplyTexture, other.EmissionMultiplyTexture);
        }
    }

    public class MatCapDefinition : IEquatable<MatCapDefinition>
    {
        public TextureInfo AdditiveTexture;


        public bool Equals(MatCapDefinition other)
        {
            return other != null &&
                   EqualityComparer<TextureInfo>.Default.Equals(AdditiveTexture, other.AdditiveTexture);
        }
    }

    public class RimDefinition : IEquatable<RimDefinition>
    {
        public LinearColor RimColor = LinearColor.White;
        public TextureInfo RimMultiplyTexture;
        public float RimLightingMixValue;
        public float RimFresnelPowerValue;
        public float RimLiftValue;

        public bool Equals(RimDefinition other)
        {
            return other != null &&
                   RimColor.Equals(other.RimColor) &&
                   EqualityComparer<TextureInfo>.Default.Equals(RimMultiplyTexture, other.RimMultiplyTexture) &&
                   RimLightingMixValue == other.RimLightingMixValue &&
                   RimFresnelPowerValue == other.RimFresnelPowerValue &&
                   RimLiftValue == other.RimLiftValue;
        }
    }

    public class NormalDefinition : IEquatable<NormalDefinition>
    {
        public TextureInfo NormalTexture;
        public float NormalScaleValue;

        public bool Equals(NormalDefinition other)
        {
            if (other is null) return false;
            if (NormalTexture != other.NormalTexture) return false;
            if (NormalScaleValue != other.NormalScaleValue) return false;
            return true;
        }
    }

    public class OutlineDefinition : IEquatable<OutlineDefinition>
    {
        public OutlineWidthMode OutlineWidthMode;
        public float OutlineWidthValue;
        public TextureInfo OutlineWidthMultiplyTexture;
        public float OutlineScaledMaxDistanceValue;
        public OutlineColorMode OutlineColorMode;
        public LinearColor OutlineColor = LinearColor.White;
        public float OutlineLightingMixValue;

        public bool Equals(OutlineDefinition other)
        {
            if (other is null) return false;
            if (OutlineWidthMode != other.OutlineWidthMode) return false;
            if (OutlineWidthValue != other.OutlineWidthValue) return false;
            if (OutlineWidthMultiplyTexture != other.OutlineWidthMultiplyTexture) return false;
            if (OutlineScaledMaxDistanceValue != other.OutlineScaledMaxDistanceValue) return false;
            if (OutlineColorMode != other.OutlineColorMode) return false;
            if (OutlineColor != other.OutlineColor) return false;
            if (OutlineLightingMixValue != other.OutlineLightingMixValue) return false;

            return true;
        }
    }

    public class TextureUvCoordsDefinition : IEquatable<TextureUvCoordsDefinition>
    {
        public Vector2 MainTextureLeftBottomOriginScale;
        public Vector2 MainTextureLeftBottomOriginOffset;
        public TextureInfo UvAnimationMaskTexture;
        public float UvAnimationScrollXSpeedValue;
        public float UvAnimationScrollYSpeedValue;
        public float UvAnimationRotationSpeedValue;

        public bool Equals(TextureUvCoordsDefinition other)
        {
            if (other is null) return false;
            if (MainTextureLeftBottomOriginScale != other.MainTextureLeftBottomOriginScale) return false;
            if (MainTextureLeftBottomOriginOffset != other.MainTextureLeftBottomOriginOffset) return false;
            if (UvAnimationMaskTexture != other.UvAnimationMaskTexture) return false;
            if (UvAnimationScrollXSpeedValue != other.UvAnimationScrollXSpeedValue) return false;
            if (UvAnimationScrollYSpeedValue != other.UvAnimationScrollYSpeedValue) return false;
            if (UvAnimationRotationSpeedValue != other.UvAnimationRotationSpeedValue) return false;

            return true;
        }
    }
}