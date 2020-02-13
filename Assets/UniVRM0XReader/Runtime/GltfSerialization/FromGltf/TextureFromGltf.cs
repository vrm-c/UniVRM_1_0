using GltfFormat;
using VrmLib;


namespace GltfSerializationAdapter
{
    public static class TextureFromGltf
    {
        public static TextureSampler FromGltf(this GltfTextureSampler sampler)
        {
            return new TextureSampler
            {
                WrapS = (TextureWrapType)sampler.wrapS,
                WrapT = (TextureWrapType)sampler.wrapT,
                MinFilter = (TextureMinFilterType)sampler.minFilter,
                MagFilter = (TextureMagFilterType)sampler.magFilter,
            };
        }
    }
}
