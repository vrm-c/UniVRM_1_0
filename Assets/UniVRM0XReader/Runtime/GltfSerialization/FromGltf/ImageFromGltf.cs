using GltfFormat;
using VrmLib;


namespace GltfSerializationAdapter
{
    public static class ImageFromGltf
    {
        public static Image FromGltf(this GltfImage x, GltfSerialization.GltfStorage storage)
        {
            var view = storage.Gltf.bufferViews[x.bufferView];
            var buffer = storage.Gltf.buffers[view.buffer];
            var memory = storage.GetBufferBytes(buffer);

            // テクスチャの用途を調べる
            var usage = default(ImageUsage);
            foreach (var material in storage.Gltf.materials)
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

            return new Image(x.name,
                x.mimeType,
                usage,
                memory.Slice(view.byteOffset, view.byteLength));
        }
        static GltfImage GetTexture(GltfSerialization.GltfStorage storage, int index)
        {
            if (index < 0 || index >= storage.Gltf.textures.Count)
            {
                return null;
            }
            var texture = storage.Gltf.textures[index];
            if (texture.source < 0 || texture.source >= storage.Gltf.images.Count)
            {
                return null;
            }
            return storage.Gltf.images[texture.source];
        }

        static GltfImage GetColorImage(GltfSerialization.GltfStorage storage, GltfMaterial m)
        {
            if (m.pbrMetallicRoughness == null)
            {
                return null;
            }
            if (m.pbrMetallicRoughness.baseColorTexture == null)
            {
                return null;
            }
            return GetTexture(storage, m.pbrMetallicRoughness.baseColorTexture.index);
        }

        static GltfImage GetNormalImage(GltfSerialization.GltfStorage storage, GltfMaterial m)
        {
            if (m.normalTexture == null)
            {
                return null;
            }
            return GetTexture(storage, m.normalTexture.index);
        }
    }
}
