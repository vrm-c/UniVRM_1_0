using System;
using System.Collections.Generic;
using VrmLib;
using VrmLib.MToon;

namespace GltfSerializationAdapter.MToon
{
    public static partial class Utils
    {
        public static MToonDefinition FromVrm0x(GltfFormat.VrmMaterial material, List<Texture> textures)
        {
            var definition = new MToonDefinition
            {
                Meta = new MetaDefinition
                {
                    Implementation = MToonUtils.Implementation,
                    VersionNumber = material.GetInt(MToonUtils.PropVersion),
                },
                Rendering = new RenderingDefinition
                {
                    RenderMode = GetBlendMode(material),
                    CullMode = GetCullMode(material),
                    RenderQueueOffsetNumber = GetRenderQueueOffset(material, GetRenderQueueOriginMode(material)),
                },
                Color = new ColorDefinition
                {
                    LitColor = ToLinear(material, MToonUtils.PropColor),
                    LitMultiplyTexture = GetTexture(material, MToonUtils.PropMainTex, textures),
                    ShadeColor = ToLinear(material, MToonUtils.PropShadeColor),
                    ShadeMultiplyTexture = GetTexture(material, MToonUtils.PropShadeTexture, textures),
                    CutoutThresholdValue = GetValue(material, MToonUtils.PropCutoff),
                },
                Lighting = new LightingDefinition
                {
                    LitAndShadeMixing = new LitAndShadeMixingDefinition
                    {
                        ShadingShiftValue = GetValue(material, MToonUtils.PropShadeShift),
                        ShadingToonyValue = GetValue(material, MToonUtils.PropShadeToony),
                        ShadowReceiveMultiplierValue = GetValue(material, MToonUtils.PropReceiveShadowRate),
                        ShadowReceiveMultiplierMultiplyTexture = GetTexture(material, MToonUtils.PropReceiveShadowTexture, textures),
                        LitAndShadeMixingMultiplierValue = GetValue(material, MToonUtils.PropShadingGradeRate),
                        LitAndShadeMixingMultiplierMultiplyTexture = GetTexture(material, MToonUtils.PropShadingGradeTexture, textures),
                    },
                    LightingInfluence = new LightingInfluenceDefinition
                    {
                        LightColorAttenuationValue = GetValue(material, MToonUtils.PropLightColorAttenuation),
                        GiIntensityValue = GetValue(material, MToonUtils.PropIndirectLightIntensity),
                    },
                    Normal = new NormalDefinition
                    {
                        NormalTexture = GetTexture(material, MToonUtils.PropBumpMap, textures),
                        NormalScaleValue = GetValue(material, MToonUtils.PropBumpScale),
                    },
                },
                Emission = new EmissionDefinition
                {
                    EmissionColor = ToLinear(material, MToonUtils.PropEmissionColor),
                    EmissionMultiplyTexture = GetTexture(material, MToonUtils.PropEmissionMap, textures),
                },
                MatCap = new MatCapDefinition
                {
                    AdditiveTexture = GetTexture(material, MToonUtils.PropSphereAdd, textures),
                },
                Rim = new RimDefinition
                {
                    RimColor = ToLinear(material, MToonUtils.PropRimColor),
                    RimMultiplyTexture = GetTexture(material, MToonUtils.PropRimTexture, textures),
                    RimLightingMixValue = GetValue(material, MToonUtils.PropRimLightingMix),
                    RimFresnelPowerValue = GetValue(material, MToonUtils.PropRimFresnelPower),
                    RimLiftValue = GetValue(material, MToonUtils.PropRimLift),
                },
                Outline = new OutlineDefinition
                {
                    OutlineWidthMode = GetOutlineWidthMode(material),
                    OutlineWidthValue = GetValue(material, MToonUtils.PropOutlineWidth),
                    OutlineWidthMultiplyTexture = GetTexture(material, MToonUtils.PropOutlineWidthTexture, textures),
                    OutlineScaledMaxDistanceValue = GetValue(material, MToonUtils.PropOutlineScaledMaxDistance),
                    OutlineColorMode = GetOutlineColorMode(material),
                    OutlineColor = ToLinear(material, MToonUtils.PropOutlineColor),
                    OutlineLightingMixValue = GetValue(material, MToonUtils.PropOutlineLightingMix),
                },
                TextureOption = new TextureUvCoordsDefinition
                {
                    MainTextureLeftBottomOriginScale = material.GetTextureScale(MToonUtils.PropMainTex),
                    MainTextureLeftBottomOriginOffset = material.GetTextureOffset(MToonUtils.PropMainTex),
                    UvAnimationMaskTexture = GetTexture(material, MToonUtils.PropUvAnimMaskTexture, textures),
                    UvAnimationScrollXSpeedValue = GetValue(material, MToonUtils.PropUvAnimScrollX),
                    UvAnimationScrollYSpeedValue = GetValue(material, MToonUtils.PropUvAnimScrollY),
                    UvAnimationRotationSpeedValue = GetValue(material, MToonUtils.PropUvAnimRotation),
                },
            };
            return definition;
        }

        private static float GetValue(GltfFormat.VrmMaterial material, string propertyName)
        {
            return material.GetFloat(propertyName);
        }

        private static LinearColor ToLinear(GltfFormat.VrmMaterial material, string propertyName)
        {
            // TODO
            var color = material.GetColor(propertyName);
            return LinearColor.FromLiner(color.X, color.Y, color.Z, color.W);
        }

        private static TextureInfo GetTexture(GltfFormat.VrmMaterial material, string propertyName, List<Texture> textures)
        {
            return (TextureInfo)material.GetTexture(propertyName, textures);
        }

        private static RenderMode GetBlendMode(GltfFormat.VrmMaterial material)
        {
            if (material.IsKeywordEnabled(MToonUtils.KeyAlphaTestOn))
            {
                return RenderMode.Cutout;
            }
            else if (material.IsKeywordEnabled(MToonUtils.KeyAlphaBlendOn))
            {
                switch (material.GetInt(MToonUtils.PropZWrite))
                {
                    case MToonUtils.EnabledIntValue:
                        return RenderMode.TransparentWithZWrite;
                    case MToonUtils.DisabledIntValue:
                        return RenderMode.Transparent;
                    default:
                        throw new ArgumentException("Invalid ZWrite Int Value.");
                }
            }
            else
            {
                return RenderMode.Opaque;
            }
        }

        private static CullMode GetCullMode(GltfFormat.VrmMaterial material)
        {
            switch ((CullMode)material.GetInt(MToonUtils.PropCullMode))
            {
                case CullMode.Off:
                    return CullMode.Off;
                case CullMode.Front:
                    return CullMode.Front;
                case CullMode.Back:
                    return CullMode.Back;
                default:
                    throw new ArgumentException("Invalid CullMode.");
            }
        }

        private static OutlineWidthMode GetOutlineWidthMode(GltfFormat.VrmMaterial material)
        {
            if (material.IsKeywordEnabled(MToonUtils.KeyOutlineWidthWorld)) return OutlineWidthMode.WorldCoordinates;
            if (material.IsKeywordEnabled(MToonUtils.KeyOutlineWidthScreen)) return OutlineWidthMode.ScreenCoordinates;

            return OutlineWidthMode.None;
        }

        private static OutlineColorMode GetOutlineColorMode(GltfFormat.VrmMaterial material)
        {
            if (material.IsKeywordEnabled(MToonUtils.KeyOutlineColorFixed)) return OutlineColorMode.FixedColor;
            if (material.IsKeywordEnabled(MToonUtils.KeyOutlineColorMixed)) return OutlineColorMode.MixedLighting;

            return OutlineColorMode.FixedColor;
        }

        private static RenderMode GetRenderQueueOriginMode(GltfFormat.VrmMaterial material)
        {
            return GetBlendMode(material);
        }

        private static int GetRenderQueueOffset(GltfFormat.VrmMaterial material, RenderMode originMode)
        {
            var rawValue = material.renderQueue;
            var requirement = MToonUtils.GetRenderQueueRequirement(originMode);
            if (rawValue < requirement.MinValue || rawValue > requirement.MaxValue)
            {
                return 0;
            }
            return rawValue - requirement.DefaultValue;
        }
    }
}