using System.Collections.Generic;
using System.Numerics;
using GltfFormat;
using VrmLib;


namespace GltfSerializationAdapter
{
    public static class MaterialFromGltf
    {
        public static Material FromGltf(this GltfMaterial x, List<Texture> textures)
        {
            return x.IsUnlit()
            ? (Material)UnlitFromGltf(x, textures)
            : (Material)PBRFromGltf(x, textures)
            ;
        }

        static void LoadCommonParams(this Material self, GltfMaterial material, List<Texture> textures)
        {
            var pbr = material.pbrMetallicRoughness;
            if (pbr != null)
            {
                if (pbr.baseColorFactor != null)
                {
                    self.BaseColorFactor = LinearColor.FromLiner(
                        pbr.baseColorFactor[0],
                        pbr.baseColorFactor[1],
                        pbr.baseColorFactor[2],
                        pbr.baseColorFactor[3]);
                }
                var baseColorTexture = pbr.baseColorTexture;
                if (baseColorTexture != null)
                {
                    self.BaseColorTexture = new TextureInfo(textures[baseColorTexture.index]);
                }
            }

            self.AlphaMode = (VrmLib.AlphaModeType)material.alphaMode;
            self.AlphaCutoff = material.alphaCutoff;
            self.DoubleSided = material.doubleSided;
        }

        public static PBRMaterial PBRFromGltf(GltfMaterial material, List<Texture> textures)
        {
            var self = new PBRMaterial(material.name);

            self.LoadCommonParams(material, textures);

            //
            // pbr
            //
            var pbr = material.pbrMetallicRoughness;

            // metallic roughness
            self.MetallicFactor = pbr.metallicFactor;
            self.RoughnessFactor = pbr.roughnessFactor;
            var metallicRoughnessTexture = pbr.metallicRoughnessTexture;
            if (metallicRoughnessTexture != null)
            {
                self.MetallicRoughnessTexture = textures[metallicRoughnessTexture.index];
            }
            //
            // emissive
            //
            if (material.emissiveFactor != null)
            {
                self.EmissiveFactor = new Vector3(
                    material.emissiveFactor[0], material.emissiveFactor[1], material.emissiveFactor[2]);
            }
            var emissiveTexture = material.emissiveTexture;
            if (emissiveTexture != null)
            {
                self.EmissiveTexture = textures[emissiveTexture.index];
            }
            //
            // normal
            //
            var normalTexture = material.normalTexture;
            if (normalTexture != null)
            {
                self.NormalTexture = textures[normalTexture.index];
            }
            //
            // occlusion
            //
            var occlusionTexture = material.occlusionTexture;
            if (occlusionTexture != null)
            {
                self.OcclusionTexture = textures[occlusionTexture.index];
            }

            return self;
        }

        public static UnlitMaterial UnlitFromGltf(GltfMaterial material, List<Texture> textures)
        {
            var unlit = new UnlitMaterial(material.name);
            unlit.LoadCommonParams(material, textures);
            return unlit;
        }
    }
}
