using GltfFormat;
using VrmLib;

namespace GltfSerializationAdapter
{
    public static class VertexBufferFromGltf
    {
        static void CreateBufferAccessorAndAdd(GltfSerialization.GltfStorage storage, int accessorIndex,
            VertexBuffer b, string key)
        {
            var a = storage.AccessorFromGltf(accessorIndex);
            if (a != null)
            {
                b.Add(key, a);
            }
        }

        public static VertexBuffer FromGltf(this GltfAttributes attributes, GltfSerialization.GltfStorage storage)
        {
            var b = new VertexBuffer();
            b.Add(VertexBuffer.PositionKey, storage.AccessorFromGltf(attributes.POSITION));
            CreateBufferAccessorAndAdd(storage, attributes.NORMAL, b, VertexBuffer.NormalKey);
            CreateBufferAccessorAndAdd(storage, attributes.TEXCOORD_0, b, VertexBuffer.TexCoordKey);
            CreateBufferAccessorAndAdd(storage, attributes.TANGENT, b, VertexBuffer.TangentKey);
            CreateBufferAccessorAndAdd(storage, attributes.COLOR_0, b, VertexBuffer.ColorKey);
            CreateBufferAccessorAndAdd(storage, attributes.JOINTS_0, b, VertexBuffer.JointKey);
            CreateBufferAccessorAndAdd(storage, attributes.WEIGHTS_0, b, VertexBuffer.WeightKey);
            return b;
        }

        public static VertexBuffer FromGltf(this GltfMorphTarget target, GltfSerialization.GltfStorage storage)
        {
            var b = new VertexBuffer();
            CreateBufferAccessorAndAdd(storage, target.POSITION, b, VertexBuffer.PositionKey);
            CreateBufferAccessorAndAdd(storage, target.NORMAL, b, VertexBuffer.NormalKey);
            CreateBufferAccessorAndAdd(storage, target.TANGENT, b, VertexBuffer.TangentKey);
            return b;
        }
    }
}
