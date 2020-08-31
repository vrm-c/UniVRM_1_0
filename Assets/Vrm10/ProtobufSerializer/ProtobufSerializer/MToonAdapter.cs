using System;
using System.Collections.Generic;
using System.Numerics;
using VrmLib.MToon;
using VrmLib;

namespace Vrm10
{
    public static class MToonAdapter
    {
        // for debug
        static readonly Vector4 Nan = new Vector4(float.NaN, float.NaN, float.NaN, float.NaN);

        static RenderMode GetRenderMode(VrmProtobuf.Material.Types.alphaModeType alphaMode, bool isTransparentWithZWrite)
        {
            switch (alphaMode)
            {
                case VrmProtobuf.Material.Types.alphaModeType.Opaque: return RenderMode.Opaque;
                case VrmProtobuf.Material.Types.alphaModeType.Mask: return RenderMode.Cutout;
                case VrmProtobuf.Material.Types.alphaModeType.Blend:
                    {
                        if (isTransparentWithZWrite)
                        {
                            return RenderMode.TransparentWithZWrite;
                        }
                        else
                        {
                            return RenderMode.Transparent;
                        }
                    }
            }

            throw new NotImplementedException();
        }

        public static MToonMaterial MToonFromGltf(VrmProtobuf.Material material, List<Texture> textures)
        {
            var mtoon = new MToonMaterial(material.Name);
            var extension = material.Extensions.VRMCMaterialsMtoon;

            mtoon.Definition = new MToonDefinition
            {
                Meta = new MetaDefinition
                {
                    Implementation = "Santarh/MToon",
                },
                Color = new ColorDefinition
                {
                    LitColor = material.PbrMetallicRoughness.BaseColorFactor.ToLinearColor(Nan),
                    ShadeColor = extension.ShadeFactor.ToLinearColor(Nan),
                    ShadeMultiplyTexture = extension.ShadeMultiplyTexture.GetTexture(textures),
                    CutoutThresholdValue = material.AlphaCutoff.Value,
                },
                Outline = new OutlineDefinition
                {
                    OutlineColorMode = (OutlineColorMode)extension.OutlineColorMode,
                    OutlineColor = extension.OutlineFactor.ToLinearColor(Nan),
                    OutlineLightingMixValue = extension.OutlineLightingMixFactor.Value,
                    OutlineScaledMaxDistanceValue = extension.OutlineScaledMaxDistanceFactor.Value,
                    OutlineWidthMode = (OutlineWidthMode)extension.OutlineWidthMode,
                    OutlineWidthValue = extension.OutlineWidthFactor.Value,
                    OutlineWidthMultiplyTexture = extension.OutlineWidthMultiplyTexture.GetTexture(textures),
                },
                Emission = new EmissionDefinition
                {
                    EmissionColor = material.EmissiveFactor.ToLinearColor(Nan),
                },
                Lighting = new LightingDefinition
                {
                    LightingInfluence = new LightingInfluenceDefinition
                    {
                        GiIntensityValue = extension.GiIntensityFactor.Value,
                        LightColorAttenuationValue = extension.LightColorAttenuationFactor.Value,
                    },
                    LitAndShadeMixing = new LitAndShadeMixingDefinition
                    {
                        ShadingShiftValue = extension.ShadingShiftFactor.Value,
                        ShadingToonyValue = extension.ShadingToonyFactor.Value,
                    },
                    Normal = new NormalDefinition
                    {
                    },
                },
                MatCap = new MatCapDefinition
                {
                    AdditiveTexture = extension.AdditiveTexture.GetTexture(textures)
                },
                Rendering = new RenderingDefinition
                {
                    CullMode = material.DoubleSided.Value ? CullMode.Off : CullMode.Back,
                    RenderMode = GetRenderMode(material.AlphaMode, extension.TransparentWithZWrite.Value),
                    RenderQueueOffsetNumber = extension.RenderQueueOffsetNumber.Value,
                },
                Rim = new RimDefinition
                {
                    RimColor = extension.RimFactor.ToLinearColor(Nan),
                    RimMultiplyTexture = extension.RimMultiplyTexture.GetTexture(textures),
                    RimLiftValue = extension.RimLiftFactor.Value,
                    RimFresnelPowerValue = extension.RimFresnelPowerFactor.Value,
                    RimLightingMixValue = extension.RimLightingMixFactor.Value,
                },
                TextureOption = new TextureUvCoordsDefinition
                {
                    UvAnimationMaskTexture = extension.UvAnimationMaskTexture.GetTexture(textures),
                    UvAnimationRotationSpeedValue = extension.UvAnimationRotationSpeedFactor.Value,
                    UvAnimationScrollXSpeedValue = extension.UvAnimationScrollXSpeedFactor.Value,
                    UvAnimationScrollYSpeedValue = extension.UvAnimationScrollYSpeedFactor.Value,
                },
            };

            if (material.PbrMetallicRoughness.BaseColorTexture != null)
            {
                mtoon.Definition.Color.LitMultiplyTexture = material.PbrMetallicRoughness.BaseColorTexture.Index.GetTexture(textures);
                if (material.PbrMetallicRoughness.BaseColorTexture.Extensions != null &&
                material.PbrMetallicRoughness.BaseColorTexture.Extensions.KHRTextureTransform != null)
                {
                    mtoon.Definition.TextureOption.MainTextureLeftBottomOriginOffset = material.PbrMetallicRoughness.BaseColorTexture.Extensions.KHRTextureTransform.Offset.ToVector2();
                    mtoon.Definition.TextureOption.MainTextureLeftBottomOriginScale = material.PbrMetallicRoughness.BaseColorTexture.Extensions.KHRTextureTransform.Scale.ToVector2();
                }
            }
            if (material.EmissiveTexture != null)
            {
                mtoon.Definition.Emission.EmissionMultiplyTexture = material.EmissiveTexture.Index.GetTexture(textures);
            }
            if (material.NormalTexture != null)
            {
                mtoon.Definition.Lighting.Normal.NormalScaleValue = material.NormalTexture.Scale.Value;
                mtoon.Definition.Lighting.Normal.NormalTexture = material.NormalTexture.Index.GetTexture(textures);
            }

            return mtoon;
        }

        static (VrmProtobuf.Material.Types.alphaModeType, bool) GetRenderMode(RenderMode mode)
        {
            switch (mode)
            {
                case RenderMode.Opaque: return (VrmProtobuf.Material.Types.alphaModeType.Opaque, false);
                case RenderMode.Cutout: return (VrmProtobuf.Material.Types.alphaModeType.Mask, false);
                case RenderMode.Transparent: return (VrmProtobuf.Material.Types.alphaModeType.Blend, false);
                case RenderMode.TransparentWithZWrite: return (VrmProtobuf.Material.Types.alphaModeType.Blend, true);
            }

            throw new NotImplementedException();
        }

        public static VrmProtobuf.Material MToonToGltf(this MToonMaterial mtoon, List<Texture> textures)
        {
            var material = mtoon.UnlitToGltf(textures);

            var dst = new VrmProtobuf.VRMC_materials_mtoon();
            material.Extensions.VRMCMaterialsMtoon = dst;

            // Color
            // unlit で済んでいる
            // material.PbrMetallicRoughness.BaseColorFactor.Assign(mtoon.Definition.Color.LitColor);
            if (mtoon.Definition.Color.LitMultiplyTexture != null)
            {
                // material.PbrMetallicRoughness.BaseColorTexture = new VrmProtobuf.TextureInfo
                // {
                //     Index = mtoon.Definition.Color.LitMultiplyTexture.ToIndex(textures),
                // };

                // standard, unlit, mtoon で共通処理にすべき？
                material.PbrMetallicRoughness.BaseColorTexture.Extensions = new VrmProtobuf.TextureInfo.Types.Extensions
                {
                    KHRTextureTransform = new VrmProtobuf.TextureInfo.Types.KHR_texture_transformtextureInfoextension
                    {
                    },
                };
                material.PbrMetallicRoughness.BaseColorTexture.Extensions.KHRTextureTransform.Offset.Assign(mtoon.Definition.TextureOption.MainTextureLeftBottomOriginOffset);
                material.PbrMetallicRoughness.BaseColorTexture.Extensions.KHRTextureTransform.Scale.Assign(mtoon.Definition.TextureOption.MainTextureLeftBottomOriginScale);
            }
            dst.ShadeFactor.Assign(mtoon.Definition.Color.ShadeColor);
            dst.ShadeMultiplyTexture = mtoon.Definition.Color.ShadeMultiplyTexture.ToIndex(textures);
            material.AlphaCutoff = mtoon.Definition.Color.CutoutThresholdValue;

            // Outline
            dst.OutlineColorMode = (VrmProtobuf.VRMC_materials_mtoon.Types.OutlineColorMode)mtoon.Definition.Outline.OutlineColorMode;
            dst.OutlineFactor.Assign(mtoon.Definition.Outline.OutlineColor);
            dst.OutlineLightingMixFactor = mtoon.Definition.Outline.OutlineLightingMixValue;
            dst.OutlineScaledMaxDistanceFactor = mtoon.Definition.Outline.OutlineScaledMaxDistanceValue;
            dst.OutlineWidthMode = (VrmProtobuf.VRMC_materials_mtoon.Types.OutlineWidthMode)mtoon.Definition.Outline.OutlineWidthMode;
            dst.OutlineWidthFactor = mtoon.Definition.Outline.OutlineWidthValue;
            dst.OutlineWidthMultiplyTexture = mtoon.Definition.Outline.OutlineWidthMultiplyTexture.ToIndex(textures);

            // Emission
            material.EmissiveFactor.Assign(mtoon.Definition.Emission.EmissionColor);
            if (mtoon.Definition.Emission.EmissionMultiplyTexture != null)
            {
                material.EmissiveTexture = new VrmProtobuf.TextureInfo
                {
                    Index = textures.IndexOfNullable(mtoon.Definition.Emission.EmissionMultiplyTexture.Texture)
                };
            }

            // Light
            dst.GiIntensityFactor = mtoon.Definition.Lighting.LightingInfluence.GiIntensityValue;
            dst.LightColorAttenuationFactor = mtoon.Definition.Lighting.LightingInfluence.LightColorAttenuationValue;
            dst.ShadingShiftFactor = mtoon.Definition.Lighting.LitAndShadeMixing.ShadingShiftValue;
            dst.ShadingToonyFactor = mtoon.Definition.Lighting.LitAndShadeMixing.ShadingToonyValue;
            if (mtoon.Definition.Lighting.Normal.NormalTexture != null)
            {
                material.NormalTexture = new VrmProtobuf.MaterialNormalTextureInfo
                {
                    Scale = mtoon.Definition.Lighting.Normal.NormalScaleValue,
                    Index = textures.IndexOfNullable(mtoon.Definition.Lighting.Normal.NormalTexture.Texture)
                };
            }

            // matcap
            dst.AdditiveTexture = mtoon.Definition.MatCap.AdditiveTexture.ToIndex(textures);

            // rendering
            switch (mtoon.Definition.Rendering.CullMode)
            {
                case CullMode.Back:
                    material.DoubleSided = false;
                    break;

                case CullMode.Off:
                    material.DoubleSided = true;
                    break;

                case CullMode.Front:
                    // GLTF not support
                    material.DoubleSided = false;
                    break;

                default:
                    throw new NotImplementedException();
            }
            (material.AlphaMode, dst.TransparentWithZWrite) = GetRenderMode(mtoon.Definition.Rendering.RenderMode);
            dst.RenderQueueOffsetNumber = mtoon.Definition.Rendering.RenderQueueOffsetNumber;

            // rim
            dst.RimFactor.Assign(mtoon.Definition.Rim.RimColor);
            dst.RimMultiplyTexture = mtoon.Definition.Rim.RimMultiplyTexture.ToIndex(textures);
            dst.RimLiftFactor = mtoon.Definition.Rim.RimLiftValue;
            dst.RimFresnelPowerFactor = mtoon.Definition.Rim.RimFresnelPowerValue;
            dst.RimLightingMixFactor = mtoon.Definition.Rim.RimLightingMixValue;

            // texture option
            dst.UvAnimationMaskTexture = mtoon.Definition.TextureOption.UvAnimationMaskTexture.ToIndex(textures);
            dst.UvAnimationRotationSpeedFactor = mtoon.Definition.TextureOption.UvAnimationRotationSpeedValue;
            dst.UvAnimationScrollXSpeedFactor = mtoon.Definition.TextureOption.UvAnimationScrollXSpeedValue;
            dst.UvAnimationScrollYSpeedFactor = mtoon.Definition.TextureOption.UvAnimationScrollYSpeedValue;

            return material;
        }
    }
}
