using System;
using System.Collections.Generic;
using GltfFormat;
using VrmLib.MToon;
using Color = VrmLib.LinearColor;
using Texture2D = VrmLib.TextureInfo;
using Material = GltfFormat.VrmMaterial;

namespace GltfSerializationAdapter.MToon
{
    public partial class Utils : VrmLib.MToon.Utils
    {
        public static MToonDefinition FromVrm0x(Material material, List<VrmLib.Texture> textures)
        {
            return new MToonDefinition
            {
                Meta = new MetaDefinition
                {
                    Implementation = Implementation,
                    VersionNumber = material.GetInt(PropVersion),
                },
                Rendering = new RenderingDefinition
                {
                    RenderMode = GetBlendMode(material),
                    CullMode = GetCullMode(material),
                    RenderQueueOffsetNumber = GetRenderQueueOffset(material, GetRenderQueueOriginMode(material)),
                },
                Color = new ColorDefinition
                {
                    LitColor = GetColor(material, PropColor),
                    LitMultiplyTexture = GetTexture(material, PropMainTex, textures),
                    ShadeColor = GetColor(material, PropShadeColor),
                    ShadeMultiplyTexture = GetTexture(material, PropShadeTexture, textures),
                    CutoutThresholdValue = GetValue(material, PropCutoff),
                },
                Lighting = new LightingDefinition
                {
                    LitAndShadeMixing = new LitAndShadeMixingDefinition
                    {
                        ShadingShiftValue = GetValue(material, PropShadeShift),
                        ShadingToonyValue = GetValue(material, PropShadeToony),
                        ShadowReceiveMultiplierValue = GetValue(material, PropReceiveShadowRate),
                        ShadowReceiveMultiplierMultiplyTexture = GetTexture(material, PropReceiveShadowTexture, textures),
                        LitAndShadeMixingMultiplierValue = GetValue(material, PropShadingGradeRate),
                        LitAndShadeMixingMultiplierMultiplyTexture = GetTexture(material, PropShadingGradeTexture, textures),
                    },
                    LightingInfluence = new LightingInfluenceDefinition
                    {
                        LightColorAttenuationValue = GetValue(material, PropLightColorAttenuation),
                        GiIntensityValue = GetValue(material, PropIndirectLightIntensity),
                    },
                    Normal = new NormalDefinition
                    {
                        NormalTexture = GetTexture(material, PropBumpMap, textures),
                        NormalScaleValue = GetValue(material, PropBumpScale),
                    },
                },
                Emission = new EmissionDefinition
                {
                    EmissionColor = GetColor(material, PropEmissionColor),
                    EmissionMultiplyTexture = GetTexture(material, PropEmissionMap, textures),
                },
                MatCap = new MatCapDefinition
                {
                    AdditiveTexture = GetTexture(material, PropSphereAdd, textures),
                },
                Rim = new RimDefinition
                {
                    RimColor = GetColor(material, PropRimColor),
                    RimMultiplyTexture = GetTexture(material, PropRimTexture, textures),
                    RimLightingMixValue = GetValue(material, PropRimLightingMix),
                    RimFresnelPowerValue = GetValue(material, PropRimFresnelPower),
                    RimLiftValue = GetValue(material, PropRimLift),
                },
                Outline = new OutlineDefinition
                {
                    OutlineWidthMode = GetOutlineWidthMode(material),
                    OutlineWidthValue = GetValue(material, PropOutlineWidth),
                    OutlineWidthMultiplyTexture = GetTexture(material, PropOutlineWidthTexture, textures),
                    OutlineScaledMaxDistanceValue = GetValue(material, PropOutlineScaledMaxDistance),
                    OutlineColorMode = GetOutlineColorMode(material),
                    OutlineColor = GetColor(material, PropOutlineColor),
                    OutlineLightingMixValue = GetValue(material, PropOutlineLightingMix),
                },
                TextureOption = new TextureUvCoordsDefinition
                {
                    MainTextureLeftBottomOriginScale = material.GetTextureScale(PropMainTex),
                    MainTextureLeftBottomOriginOffset = material.GetTextureOffset(PropMainTex),
                    UvAnimationMaskTexture = GetTexture(material, PropUvAnimMaskTexture, textures),
                    UvAnimationScrollXSpeedValue = GetValue(material, PropUvAnimScrollX),
                    UvAnimationScrollYSpeedValue = GetValue(material, PropUvAnimScrollY),
                    UvAnimationRotationSpeedValue = GetValue(material, PropUvAnimRotation),
                },
            };
        }

        private static float GetValue(VrmMaterial material, string propertyName)
        {
            return material.GetFloat(propertyName);
        }

        private static Color GetColor(VrmMaterial material, string propertyName)
        {
            // TODO
            var color = material.GetColor(propertyName);
            return VrmLib.LinearColor.FromLiner(color.X, color.Y, color.Z, color.W);
        }

        private static Texture2D GetTexture(VrmMaterial material, string propertyName, List<VrmLib.Texture> textures)
        {
            return (Texture2D)material.GetTexture(propertyName, textures);
        }

        private static RenderMode GetBlendMode(VrmMaterial material)
        {
            if (material.IsKeywordEnabled(KeyAlphaTestOn))
            {
                return RenderMode.Cutout;
            }
            else if (material.IsKeywordEnabled(KeyAlphaBlendOn))
            {
                switch (material.GetInt(PropZWrite))
                {
                    case EnabledIntValue:
                        return RenderMode.TransparentWithZWrite;
                    case DisabledIntValue:
                        return RenderMode.Transparent;
                    default:
                        Console.WriteLine("[GetBlendMode] Invalid ZWrite Int Value.");
                        return RenderMode.Transparent;
                }
            }
            else
            {
                return RenderMode.Opaque;
            }
        }

        private static CullMode GetCullMode(VrmMaterial material)
        {
            switch ((CullMode)material.GetInt(PropCullMode))
            {
                case CullMode.Off:
                    return CullMode.Off;
                case CullMode.Front:
                    return CullMode.Front;
                case CullMode.Back:
                    return CullMode.Back;
                default:
                    Console.WriteLine("[GetCullMode]: Invalid CullMode.");
                    return CullMode.Back;
            }
        }

        private static OutlineWidthMode GetOutlineWidthMode(VrmMaterial material)
        {
            if (material.IsKeywordEnabled(KeyOutlineWidthWorld)) return OutlineWidthMode.WorldCoordinates;
            if (material.IsKeywordEnabled(KeyOutlineWidthScreen)) return OutlineWidthMode.ScreenCoordinates;

            return OutlineWidthMode.None;
        }

        private static OutlineColorMode GetOutlineColorMode(VrmMaterial material)
        {
            if (material.IsKeywordEnabled(KeyOutlineColorFixed)) return OutlineColorMode.FixedColor;
            if (material.IsKeywordEnabled(KeyOutlineColorMixed)) return OutlineColorMode.MixedLighting;

            return OutlineColorMode.FixedColor;
        }

        private static RenderMode GetRenderQueueOriginMode(VrmMaterial material)
        {
            return GetBlendMode(material);
        }

        private static int GetRenderQueueOffset(VrmMaterial material, RenderMode originMode)
        {
            var rawValue = material.renderQueue;
            var requirement = GetRenderQueueRequirement(originMode);
            if (rawValue < requirement.MinValue || rawValue > requirement.MaxValue)
            {
                return 0;
            }
            return rawValue - requirement.DefaultValue;
        }
    }
}