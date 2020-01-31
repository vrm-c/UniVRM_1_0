using VrmLib;
using System.Collections.Generic;
using System.Numerics;

namespace Vrm10
{
    public static class MaterialAdapter
    {
        public static Material FromGltf(this VrmProtobuf.Material x, List<Texture> textures)
        {
            if (x.Extensions.VRMCMaterialsMtoon != null)
            {
                // MToon
                return MToonAdapter.MToonFromGltf(x, textures);
            }
            else if (x.Extensions.KHRMaterialsUnlit != null)
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

            self.AlphaMode = EnumUtil.Parse<VrmLib.AlphaModeType>(material.AlphaMode);
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

        static VrmProtobuf.Material ToGltf(this VrmLib.Material src, string name, List<Texture> textures)
        {
            var material = new VrmProtobuf.Material
            {
                Name = name,
                Extensions = new VrmProtobuf.Material.Types.Extensions
                {
                    KHRMaterialsUnlit = new VrmProtobuf.KHR_materials_unlit(),
                },
                PbrMetallicRoughness = new VrmProtobuf.MaterialPbrMetallicRoughness
                {

                },
                AlphaMode = src.AlphaMode.ToString(),
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

        public static VrmProtobuf.Material PBRToGltf(this PBRMaterial pbr, string name, List<Texture> textures)
        {
            var material = pbr.ToGltf(name, textures);
            // TODO: PBR params
            return material;
        }

        public static VrmProtobuf.Material UnlitToGltf(this UnlitMaterial unlit, string name, List<Texture> textures)
        {
            var material = unlit.ToGltf(name, textures);
            material.PbrMetallicRoughness.RoughnessFactor = 0.9f;
            material.PbrMetallicRoughness.MetallicFactor = 0.0f;

            return material;
        }
    }
}
