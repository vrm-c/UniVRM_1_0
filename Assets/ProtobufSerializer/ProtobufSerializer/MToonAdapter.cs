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
                    LitColor = extension.LitFactor.ToLinearColor(Nan),
                    LitMultiplyTexture = extension.LitMultiplyTexture.GetTexture(textures),
                    ShadeColor = extension.ShadeFactor.ToLinearColor(Nan),
                    ShadeMultiplyTexture = extension.ShadeMultiplyTexture.GetTexture(textures),
                    CutoutThresholdValue = extension.CutoutThresholdFactor,
                },
                Outline = new OutlineDefinition
                {
                    OutlineColorMode = (OutlineColorMode)extension.OutlineColorMode,
                    OutlineColor = extension.OutlineFactor.ToLinearColor(Nan),
                    OutlineLightingMixValue = extension.OutlineLightingMixFactor,
                    OutlineScaledMaxDistanceValue = extension.OutlineScaledMaxDistanceFactor,
                    OutlineWidthMode = (OutlineWidthMode)extension.OutlineWidthMode,
                    OutlineWidthValue = extension.OutlineWidthFactor,
                    OutlineWidthMultiplyTexture = extension.OutlineWidthMultiplyTexture.GetTexture(textures),
                },
                Emission = new EmissionDefinition
                {
                    EmissionColor = extension.EmissionFactor.ToLinearColor(Nan),
                    EmissionMultiplyTexture = extension.EmissionMultiplyTexture.GetTexture(textures),
                },
                Lighting = new LightingDefinition
                {
                    LightingInfluence = new LightingInfluenceDefinition
                    {
                        GiIntensityValue = extension.GiIntensityFactor,
                        LightColorAttenuationValue = extension.LightColorAttenuationFactor,
                    },
                    LitAndShadeMixing = new LitAndShadeMixingDefinition
                    {
                        ShadingShiftValue = extension.ShadingShiftFactor,
                        ShadingToonyValue = extension.ShadingToonyFactor,
                    },
                    Normal = new NormalDefinition
                    {
                        NormalScaleValue = extension.NormalScaleFactor,
                        NormalTexture = extension.NormalTexture.GetTexture(textures),
                    },
                },
                MatCap = new MatCapDefinition
                {
                    AdditiveTexture = extension.AdditiveTexture.GetTexture(textures)
                },
                Rendering = new RenderingDefinition
                {
                    CullMode = (CullMode)extension.CullMode,
                    RenderMode = (RenderMode)extension.RenderMode,
                    RenderQueueOffsetNumber = extension.RenderQueueOffsetNumber,
                },
                Rim = new RimDefinition
                {
                    RimColor = extension.RimFactor.ToLinearColor(Nan),
                    RimMultiplyTexture = extension.RimMultiplyTexture.GetTexture(textures),
                    RimLiftValue = extension.RimLiftFactor,
                    RimFresnelPowerValue = extension.RimFresnelPowerFactor,
                    RimLightingMixValue = extension.RimLightingMixFactor,
                },
                TextureOption = new TextureUvCoordsDefinition
                {
                    UvAnimationMaskTexture = extension.UvAnimationMaskTexture.GetTexture(textures),
                    UvAnimationRotationSpeedValue = extension.UvAnimationRotationSpeedFactor,
                    UvAnimationScrollXSpeedValue = extension.UvAnimationScrollXSpeedFactor,
                    UvAnimationScrollYSpeedValue = extension.UvAnimationScrollYSpeedFactor,
                    MainTextureLeftBottomOriginOffset = extension.MainTextureLeftBottomOriginOffset.ToVector2(),
                    MainTextureLeftBottomOriginScale = extension.MainTextureLeftBottomOriginScale.ToVector2(),
                },
            };

            return mtoon;
        }

        public static VrmProtobuf.Material MToonToGltf(this MToonMaterial mtoon, List<Texture> textures)
        {
            var material = mtoon.UnlitToGltf(textures);

            // Unlitも有効にする
            // material.Extensions.KHRMaterialsUnlit = null;

            var dst = new VrmProtobuf.VRMC_materials_mtoon();
            material.Extensions.VRMCMaterialsMtoon = dst;

            // Color
            dst.LitFactor.Assign(mtoon.Definition.Color.LitColor);
            dst.LitMultiplyTexture = mtoon.Definition.Color.LitMultiplyTexture.ToIndex(textures);
            dst.ShadeFactor.Assign(mtoon.Definition.Color.ShadeColor);
            dst.ShadeMultiplyTexture = mtoon.Definition.Color.ShadeMultiplyTexture.ToIndex(textures);
            dst.CutoutThresholdFactor = mtoon.Definition.Color.CutoutThresholdValue;

            // Outline
            dst.OutlineColorMode = (VrmProtobuf.VRMC_materials_mtoon.Types.OutlineColorMode)mtoon.Definition.Outline.OutlineColorMode;
            dst.OutlineFactor.Assign(mtoon.Definition.Outline.OutlineColor);
            dst.OutlineLightingMixFactor = mtoon.Definition.Outline.OutlineLightingMixValue;
            dst.OutlineScaledMaxDistanceFactor = mtoon.Definition.Outline.OutlineScaledMaxDistanceValue;
            dst.OutlineWidthMode = (VrmProtobuf.VRMC_materials_mtoon.Types.OutlineWidthMode)mtoon.Definition.Outline.OutlineWidthMode;
            dst.OutlineWidthFactor = mtoon.Definition.Outline.OutlineWidthValue;
            dst.OutlineWidthMultiplyTexture = mtoon.Definition.Outline.OutlineWidthMultiplyTexture.ToIndex(textures);

            // Emission
            dst.EmissionFactor.Assign(mtoon.Definition.Emission.EmissionColor);
            dst.EmissionMultiplyTexture = mtoon.Definition.Emission.EmissionMultiplyTexture.ToIndex(textures);
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
            dst.NormalScaleFactor = mtoon.Definition.Lighting.Normal.NormalScaleValue;
            dst.NormalTexture = mtoon.Definition.Lighting.Normal.NormalTexture.ToIndex(textures);
            if (mtoon.Definition.Lighting.Normal.NormalTexture != null)
            {
                material.NormalTexture = new VrmProtobuf.MaterialNormalTextureInfo
                {
                    Index = textures.IndexOfNullable(mtoon.Definition.Lighting.Normal.NormalTexture.Texture)
                };
            }

            // matcap
            dst.AdditiveTexture = mtoon.Definition.MatCap.AdditiveTexture.ToIndex(textures);

            // rendering
            dst.CullMode = (VrmProtobuf.VRMC_materials_mtoon.Types.CullMode)mtoon.Definition.Rendering.CullMode;
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
            dst.RenderMode = (VrmProtobuf.VRMC_materials_mtoon.Types.RenderMode)mtoon.Definition.Rendering.RenderMode;
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
            dst.MainTextureLeftBottomOriginOffset.Assign(mtoon.Definition.TextureOption.MainTextureLeftBottomOriginOffset);
            dst.MainTextureLeftBottomOriginScale.Assign(mtoon.Definition.TextureOption.MainTextureLeftBottomOriginScale);

            return material;
        }
    }
}
