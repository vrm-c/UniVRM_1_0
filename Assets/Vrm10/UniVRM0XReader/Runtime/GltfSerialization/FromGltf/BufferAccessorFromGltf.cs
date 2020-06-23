using System;
using System.Linq;
using System.Runtime.InteropServices;
using GltfFormat;
using VrmLib;

namespace GltfSerializationAdapter
{
    public static class BufferAccessorFromGltf
    {
        /// <summary>
        /// submeshのindexが連続した領域に格納されているかを確認する
        /// </summary>
        static bool AccessorsIsContinuous(GltfSerialization.GltfStorage storage, int[] accessorIndices)
        {
            var gltf = storage.Gltf;
            var firstAccessor = gltf.accessors[accessorIndices[0]];
            var firstView = gltf.bufferViews[firstAccessor.bufferView];
            var start = firstView.byteOffset + firstAccessor.byteOffset;
            var pos = start;
            foreach (var i in accessorIndices)
            {
                var current = gltf.accessors[i];
                if (current.type != GltfAccessorType.SCALAR)
                {
                    throw new ArgumentException($"accessor.type: {current.type}");
                }
                if (firstAccessor.componentType != current.componentType)
                {
                    return false;
                }

                var view = gltf.bufferViews[current.bufferView];
                if (pos != view.byteOffset + current.byteOffset)
                {
                    return false;
                }

                var begin = view.byteOffset + current.byteOffset;
                var byteLength = current.componentType.ByteSize() * current.type.TypeCount() * current.count;

                pos += byteLength;
            }

            return true;
        }

        /// <summary>
        /// Gltfの Primitive[] の indices をひとまとめにした
        /// IndexBuffer を返す。
        /// </summary>
        public static BufferAccessor IndexBufferFromGltf(this GltfSerialization.GltfStorage storage, int[] accessorIndices)
        {
            var gltf = storage.Gltf;
            var totalCount = accessorIndices.Sum(x => gltf.accessors[x].count);
            if (AccessorsIsContinuous(storage, accessorIndices))
            {
                // IndexBufferが連続して格納されている => Slice でいける
                var firstAccessor = gltf.accessors[accessorIndices[0]];
                var firstView = gltf.bufferViews[firstAccessor.bufferView];
                var start = firstView.byteOffset + firstAccessor.byteOffset;
                var buffer = gltf.buffers[firstView.buffer];
                var bin = storage.GetBufferBytes(buffer);
                var bytes = bin.Slice(start, totalCount * firstAccessor.GetStride());
                return new BufferAccessor(bytes,
                    (AccessorValueType)firstAccessor.componentType, (AccessorVectorType)firstAccessor.type, totalCount);
            }
            else
            {
                // IndexBufferが連続して格納されていない => Int[] を作り直す
                var indices = new byte[totalCount * Marshal.SizeOf(typeof(int))];
                var span = MemoryMarshal.Cast<byte, int>(indices.AsSpan());
                var offset = 0;
                foreach (var accessorIndex in accessorIndices)
                {
                    var accessor = gltf.accessors[accessorIndex];
                    if (accessor.type != GltfAccessorType.SCALAR)
                    {
                        throw new ArgumentException($"accessor.type: {accessor.type}");
                    }
                    var view = gltf.bufferViews[accessor.bufferView];
                    var buffer = gltf.buffers[view.buffer];
                    var bin = storage.GetBufferBytes(buffer);
                    var start = view.byteOffset + accessor.byteOffset;
                    var bytes = bin.Slice(start, accessor.count * accessor.GetStride());
                    var dst = MemoryMarshal.Cast<byte, int>(indices.AsSpan()).Slice(offset, accessor.count);
                    offset += accessor.count;
                    switch (accessor.componentType)
                    {
                        case GltfComponentType.UNSIGNED_BYTE:
                            {
                                var src = bytes.Span;
                                for (int i = 0; i < src.Length; ++i)
                                {
                                    // byte to int
                                    dst[i] = src[i];
                                }
                            }
                            break;

                        case GltfComponentType.UNSIGNED_SHORT:
                            {
                                var src = MemoryMarshal.Cast<byte, ushort>(bytes.Span);
                                for (int i = 0; i < src.Length; ++i)
                                {
                                    // ushort to int
                                    dst[i] = src[i];
                                }
                            }
                            break;

                        case GltfComponentType.UNSIGNED_INT:
                            {
                                var src = MemoryMarshal.Cast<byte, int>(bytes.Span);
                                // int to int
                                src.CopyTo(dst);
                            }
                            break;

                        default:
                            throw new NotImplementedException($"accessor.componentType: {accessor.componentType}");
                    }
                }
                return new BufferAccessor(indices, AccessorValueType.UNSIGNED_INT, AccessorVectorType.SCALAR, totalCount);
            }
        }

        public static BufferAccessor AccessorFromGltf(this GltfSerialization.GltfStorage storage, int accessorIndex)
        {
            if (accessorIndex < 0)
            {
                return null;
            }
            var gltf = storage.Gltf;
            var accessor = gltf.accessors[accessorIndex];
            var bytes = storage.GetAccessorBytes(accessorIndex);
            return new BufferAccessor(bytes,
                (AccessorValueType)accessor.componentType, (AccessorVectorType)accessor.type, accessor.count);
        }
    }
}
