using VrmLib;
using System;

namespace Vrm10
{
    public static class ImageAdapter
    {
        public static Image FromGltf(this VrmProtobuf.Image x, Vrm10Storage storage)
        {
            if (!x.BufferView.HasValue)
            {
                // 外部参照？
                throw new Exception();
            }

            var view = storage.Gltf.BufferViews[x.BufferView.Value];

            var buffer = storage.Gltf.Buffers[view.Buffer.Value];

            // テクスチャの用途を調べる
            var usage = default(ImageUsage);
            foreach (var material in storage.Gltf.Materials)
            {
                var colorImage = GetColorImage(storage, material);
                if (colorImage == x)
                {
                    usage |= ImageUsage.Color;
                }

                var normalImage = GetNormalImage(storage, material);
                if (normalImage == x)
                {
                    usage |= ImageUsage.Normal;
                }
            }

            var memory = storage.GetBufferBytes(buffer);
            return new Image(x.Name,
                x.MimeType,
                usage,
                memory.Slice(view.ByteOffset.GetValueOrDefault(), view.ByteLength.Value));
        }

        static VrmProtobuf.Image GetTexture(Vrm10Storage storage, int index)
        {
            if (index < 0 || index >= storage.Gltf.Textures.Count)
            {
                return null;
            }
            var texture = storage.Gltf.Textures[index];
            if (texture.Source.HasValue && texture.Source < 0 || texture.Source >= storage.Gltf.Images.Count)
            {
                return null;
            }
            return storage.Gltf.Images[texture.Source.Value];
        }

        static VrmProtobuf.Image GetColorImage(Vrm10Storage storage, VrmProtobuf.Material m)
        {
            if (m.PbrMetallicRoughness == null)
            {
                return null;
            }
            if (m.PbrMetallicRoughness.BaseColorTexture == null)
            {
                return null;
            }
            if(!m.PbrMetallicRoughness.BaseColorTexture.Index.TryGetValidIndex(storage.TextureCount, out int index))
            {
                return null;
            }
            return GetTexture(storage, index);
        }

        static VrmProtobuf.Image GetNormalImage(Vrm10Storage storage, VrmProtobuf.Material m)
        {
            if (m.NormalTexture == null)
            {
                return null;
            }
            if(!m.NormalTexture.Index.TryGetValidIndex(storage.TextureCount, out int index))
            {
                return null;
            }
            return GetTexture(storage, index);
        }

        public static VrmProtobuf.Image ToGltf(this Image src, Vrm10Storage storage)
        {
            var viewIndex = storage.AppendToBuffer(0, src.Bytes, 1);
            var gltf = storage.Gltf;
            return new VrmProtobuf.Image
            {
                Name = src.Name,
                MimeType = src.MimeType,
                BufferView = viewIndex,
            };
        }
    }
}