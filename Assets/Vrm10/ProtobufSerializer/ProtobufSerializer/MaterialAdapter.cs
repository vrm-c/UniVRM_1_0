using VrmLib;
using System.Collections.Generic;
using System.Numerics;

namespace Vrm10
{
    public static class MaterialAdapter
    {
        public static Material FromGltf(this VrmProtobuf.Material x, List<Texture> textures)
        {
            if (x.Extensions != null && x.Extensions.VRMCMaterialsMtoon != null)
            {
                // MToon
                return MToonAdapter.MToonFromGltf(x, textures);
            }
            else if (x.Extensions != null && x.Extensions.KHRMaterialsUnlit != null)
            {
                // unlit
                return UnlitFromGltf(x, textures);
            }
            else
            {
                // PBR
                return PBRFromGltf(x, textures);
            }
        }

        public static void LoadCommonParams(this Material self, VrmProtobuf.Material material, List<Texture> textures)
        {
            var pbr = material.PbrMetallicRoughness;
            if (pbr.BaseColorFactor.Count > 0)
            {
                self.BaseColorFactor = LinearColor.FromLiner(
                    pbr.BaseColorFactor[0],
                    pbr.BaseColorFactor[1],
                    pbr.BaseColorFactor[2],
                    pbr.BaseColorFactor[3]);
            }
            var baseColorTexture = pbr.BaseColorTexture;
            if (baseColorTexture != null && baseColorTexture.Index.TryGetValidIndex(textures.Count, out int index))
            {
                self.BaseColorTexture = new TextureInfo(textures[index]);
            }

            self.AlphaMode = EnumUtil.Cast<VrmLib.AlphaModeType>(material.AlphaMode);
            self.AlphaCutoff = material.AlphaCutoff.HasValue
                ? material.AlphaCutoff.Value
                : 0.5f // gltf default
                ;
            self.DoubleSided = material.DoubleSided.HasValue
                ? material.DoubleSided.Value
                : false // gltf default
                ;
        }

        public static PBRMaterial PBRFromGltf(VrmProtobuf.Material material, List<Texture> textures)
        {
            var self = new PBRMaterial(material.Name);

            self.LoadCommonParams(material, textures);

            //
            // pbr
            //
            var pbr = material.PbrMetallicRoughness;

            // metallic roughness
            self.MetallicFactor = pbr.MetallicFactor.HasValue
                ? pbr.MetallicFactor.Value
                : 1.0f // gltf default
                ;
            self.RoughnessFactor = pbr.RoughnessFactor.HasValue
                ? pbr.RoughnessFactor.Value
                : 1.0f // gltf default
                ;
            var metallicRoughnessTexture = pbr.MetallicRoughnessTexture;
            if (metallicRoughnessTexture != null
            && metallicRoughnessTexture.Index.TryGetValidIndex(textures.Count, out int metallicRoughnessTextureIndex))
            {
                self.MetallicRoughnessTexture = textures[metallicRoughnessTextureIndex];
            }
            //
            // emissive
            //
            if (material.EmissiveFactor.Count > 0)
            {
                self.EmissiveFactor = new Vector3(
                    material.EmissiveFactor[0],
                    material.EmissiveFactor[1],
                    material.EmissiveFactor[2]);
            }
            var emissiveTexture = material.EmissiveTexture;
            if (emissiveTexture != null
            && emissiveTexture.Index.TryGetValidIndex(textures.Count, out int emissiveTextureIndex))
            {
                self.EmissiveTexture = textures[emissiveTextureIndex];
            }
            //
            // normal
            //
            var normalTexture = material.NormalTexture;
            if (normalTexture != null
            && normalTexture.Index.TryGetValidIndex(textures.Count, out int normalTextureIndex))
            {
                self.NormalTexture = textures[normalTextureIndex];
            }
            //
            // occlusion
            //
            var occlusionTexture = material.OcclusionTexture;
            if (occlusionTexture != null
            && occlusionTexture.Index.TryGetValidIndex(textures.Count, out int occlusionTextureIndex))
            {
                self.OcclusionTexture = textures[occlusionTextureIndex];
            }

            return self;
        }

        public static UnlitMaterial UnlitFromGltf(VrmProtobuf.Material material, List<Texture> textures)
        {
            var unlit = new UnlitMaterial(material.Name);
            unlit.LoadCommonParams(material, textures);
            return unlit;
        }

        static VrmProtobuf.Material ToGltf(this VrmLib.Material src, List<Texture> textures)
        {
            var material = new VrmProtobuf.Material
            {
                Name = src.Name,
                PbrMetallicRoughness = new VrmProtobuf.MaterialPBRMetallicRoughness
                {

                },
                AlphaMode = EnumUtil.Cast<VrmProtobuf.Material.Types.alphaModeType>(src.AlphaMode),
                AlphaCutoff = src.AlphaCutoff,
                DoubleSided = src.DoubleSided,
            };
            src.BaseColorFactor.ToProtobuf(material.PbrMetallicRoughness.BaseColorFactor.Add, true);
            if (src.BaseColorTexture != null)
            {
                material.PbrMetallicRoughness.BaseColorTexture = new VrmProtobuf.TextureInfo
                {
                    Index = textures.IndexOfNullable(src.BaseColorTexture.Texture),
                };
            }
            return material;
        }

        public static VrmProtobuf.Material PBRToGltf(this PBRMaterial pbr, List<Texture> textures)
        {
            var material = pbr.ToGltf(textures);

            // MetallicRoughness
            material.PbrMetallicRoughness.BaseColorFactor.Add(pbr.BaseColorFactor.ToFloat4());
            if (pbr.BaseColorTexture != null)
            {
                material.PbrMetallicRoughness.BaseColorTexture = new VrmProtobuf.TextureInfo
                {
                    Index = textures.IndexOfNullable(pbr.BaseColorTexture.Texture),
                };
            }
            material.PbrMetallicRoughness.MetallicFactor = pbr.MetallicFactor;
            material.PbrMetallicRoughness.RoughnessFactor = pbr.RoughnessFactor;
            if (pbr.MetallicRoughnessTexture != null)
            {
                material.PbrMetallicRoughness.MetallicRoughnessTexture = new VrmProtobuf.TextureInfo
                {
                    Index = textures.IndexOfNullable(pbr.MetallicRoughnessTexture),
                };
            }

            // Normal
            if (pbr.NormalTexture != null)
            {
                material.NormalTexture = new VrmProtobuf.MaterialNormalTextureInfo
                {
                    Index = textures.IndexOfNullable(pbr.NormalTexture),
                    Scale = pbr.NormalTextureScale
                };
            }

            // Occlusion
            if (pbr.OcclusionTexture != null)
            {
                material.OcclusionTexture = new VrmProtobuf.MaterialOcclusionTextureInfo
                {
                    Index = textures.IndexOfNullable(pbr.OcclusionTexture),
                    Strength = pbr.OcclusionTextureStrength,
                };
            }

            // Emissive
            if (pbr.EmissiveTexture != null)
            {
                material.EmissiveTexture = new VrmProtobuf.TextureInfo
                {
                    Index = textures.IndexOfNullable(pbr.EmissiveTexture),
                };
            }
            material.EmissiveFactor.Add(pbr.EmissiveFactor.X);
            material.EmissiveFactor.Add(pbr.EmissiveFactor.Y);
            material.EmissiveFactor.Add(pbr.EmissiveFactor.Z);

            // AlphaMode
            var alphaMode = (pbr.AlphaMode == AlphaModeType.BLEND_ZWRITE)?AlphaModeType.BLEND: pbr.AlphaMode;
            material.AlphaMode = EnumUtil.Cast<VrmProtobuf.Material.Types.alphaModeType>(alphaMode);

            // AlphaCutoff
            material.AlphaCutoff = pbr.AlphaCutoff;

            // DoubleSided
            material.DoubleSided = pbr.DoubleSided;

            return material;
        }

        public static VrmProtobuf.Material UnlitToGltf(this UnlitMaterial unlit, List<Texture> textures)
        {
            var material = unlit.ToGltf(textures);
            material.Extensions = new VrmProtobuf.Material.Types.Extensions
            {
                KHRMaterialsUnlit = new VrmProtobuf.Material.Types.KHR_materials_unlitglTFextension(),
            };
            material.PbrMetallicRoughness.RoughnessFactor = 0.9f;
            material.PbrMetallicRoughness.MetallicFactor = 0.0f;

            return material;
        }
    }
}
