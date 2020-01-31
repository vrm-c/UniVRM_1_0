using VrmLib;
using System;
using System.Collections.Generic;

namespace Vrm10
{
    public static class TextureAdapter
    {
        public static ImageTexture FromGltf(this VrmProtobuf.Texture x, VrmProtobuf.Sampler sampler, List<Image> images, Texture.ColorSpaceTypes colorSpace, Texture.TextureTypes textureType)
        {
            var image = images[x.Source.Value];
            var name = !string.IsNullOrEmpty(x.Name) ? x.Name : image.Name;
            return new ImageTexture(x.Name, sampler.FromGltf(), image, colorSpace, textureType);
        }

        public static TextureSampler FromGltf(this VrmProtobuf.Sampler sampler)
        {
            return new TextureSampler
            {
                WrapS = (TextureWrapType)sampler.WrapS,
                WrapT = (TextureWrapType)sampler.WrapT,
                MinFilter = (TextureMinFilterType)sampler.MinFilter,
                MagFilter = (TextureMagFilterType)sampler.MagFilter,
            };
        }

        public static VrmProtobuf.Sampler ToGltf(this TextureSampler src)
        {
            return new VrmProtobuf.Sampler
            {
                WrapS = (int)src.WrapS,
                WrapT = (int)src.WrapT,
                MinFilter = (int)src.MinFilter,
                MagFilter = (int)src.MagFilter,
            };
        }
    }
}