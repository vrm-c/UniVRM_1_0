using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using VrmLib;

namespace Vrm10
{
    public static class BufferAccessorAdapter
    {
        public static int TypeCount(this string type)
        {
            switch (type)
            {
                case "SCALAR":
                    return 1;
                case "VEC2":
                    return 2;
                case "VEC3":
                    return 3;
                case "VEC4":
                case "MAT2":
                    return 4;
                case "MAT3":
                    return 9;
                case "MAT4":
                    return 16;
                default:
                    throw new NotImplementedException();
            }
        }

        public static int TypeCount(this VrmProtobuf.Accessor.Types.accessorType type)
        {
            switch (type)
            {
                case VrmProtobuf.Accessor.Types.accessorType.Scalar:
                    return 1;
                case VrmProtobuf.Accessor.Types.accessorType.Vec2:
                    return 2;
                case VrmProtobuf.Accessor.Types.accessorType.Vec3:
                    return 3;
                case VrmProtobuf.Accessor.Types.accessorType.Vec4:
                case VrmProtobuf.Accessor.Types.accessorType.Mat2:
                    return 4;
                case VrmProtobuf.Accessor.Types.accessorType.Mat3:
                    return 9;
                case VrmProtobuf.Accessor.Types.accessorType.Mat4:
                    return 16;
                default:
                    throw new NotImplementedException();
            }
        }

        public static int GetStride(this VrmProtobuf.Accessor accessor)
        {
            return accessor.Type.TypeCount() * ((VrmLib.AccessorValueType)accessor.ComponentType).ByteSize();
        }

        public static int AddViewTo(this BufferAccessor self,
            Vrm10Storage storage, int bufferIndex,
            int offset = 0, int count = 0)
        {
            var stride = self.Stride;
            if (count == 0)
            {
                count = self.Count;
            }
            var slice = self.Bytes.Slice(offset * stride, count * stride);
            return storage.AppendToBuffer(bufferIndex, slice, stride);
        }

        static VrmProtobuf.Accessor CreateGltfAccessor(this BufferAccessor self,
            int viewIndex, int count = 0, int byteOffset = 0)
        {
            if (count == 0)
            {
                count = self.Count;
            }
            return new VrmProtobuf.Accessor
            {
                BufferView = viewIndex,
                ByteOffset = byteOffset,
                ComponentType = (int)self.ComponentType,
                Type = EnumUtil.Cast<VrmProtobuf.Accessor.Types.accessorType>(self.AccessorType),
                Count = count,
            };
        }

        public static int AddAccessorTo(this BufferAccessor self,
            Vrm10Storage storage, int viewIndex,
            Action<Memory<byte>, VrmProtobuf.Accessor> minMax = null,
            int offset = 0, int count = 0)
        {
            var gltf = storage.Gltf;
            var accessorIndex = gltf.Accessors.Count;
            var accessor = self.CreateGltfAccessor(viewIndex, count, offset * self.Stride);
            if (minMax != null)
            {
                minMax(self.Bytes, accessor);
            }
            gltf.Accessors.Add(accessor);
            return accessorIndex;
        }

        public static int AddAccessorTo(this BufferAccessor self,
            Vrm10Storage storage, int bufferIndex,
            // GltfBufferTargetType targetType,
            bool useSparse,
            Action<Memory<byte>, VrmProtobuf.Accessor> minMax = null,
            int offset = 0, int count = 0)
        {
            if (self.ComponentType == AccessorValueType.FLOAT
            && self.AccessorType == AccessorVectorType.VEC3
            )
            {
                var values = self.GetSpan<Vector3>();
                // 巨大ポリゴンのモデル対策にValueTupleの型をushort -> uint へ
                var sparseValuesWithIndex = new List<ValueTuple<int, Vector3>>();
                for (int i = 0; i < values.Length; ++i)
                {
                    var v = values[i];
                    if (v != Vector3.Zero)
                    {
                        sparseValuesWithIndex.Add((i, v));
                    }
                }

                //var status = $"{sparseIndices.Count * 14}/{values.Length * 12}";
                if (useSparse
                && sparseValuesWithIndex.Count > 0 // avoid empty sparse
                && sparseValuesWithIndex.Count * 16 < values.Length * 12)
                {
                    // use sparse
                    var sparseIndexBin = new byte[sparseValuesWithIndex.Count * 4].AsMemory();
                    var sparseIndexSpan = MemoryMarshal.Cast<byte, int>(sparseIndexBin.Span);
                    var sparseValueBin = new byte[sparseValuesWithIndex.Count * 12].AsMemory();
                    var sparseValueSpan = MemoryMarshal.Cast<byte, Vector3>(sparseValueBin.Span);

                    for (int i = 0; i < sparseValuesWithIndex.Count; ++i)
                    {
                        var (index, value) = sparseValuesWithIndex[i];
                        sparseIndexSpan[i] = index;
                        sparseValueSpan[i] = value;
                    }

                    var sparseIndexView = storage.AppendToBuffer(bufferIndex, sparseIndexBin, 4);
                    var sparseValueView = storage.AppendToBuffer(bufferIndex, sparseValueBin, 12);

                    var accessorIndex = storage.Gltf.Accessors.Count;
                    var accessor = new VrmProtobuf.Accessor
                    {
                        ComponentType = (int)self.ComponentType,
                        Type = EnumUtil.Cast<VrmProtobuf.Accessor.Types.accessorType>(self.AccessorType),
                        Count = self.Count,
                        Sparse = new VrmProtobuf.AccessorSparse
                        {
                            Count = sparseValuesWithIndex.Count,
                            Indices = new VrmProtobuf.AccessorSparseIndices
                            {
                                ComponentType = (int)AccessorValueType.UNSIGNED_INT,
                                BufferView = sparseIndexView,
                            },
                            Values = new VrmProtobuf.AccessorSparseValues
                            {
                                BufferView = sparseValueView,
                            },
                        }
                    };
                    if (minMax != null)
                    {
                        minMax(sparseValueBin, accessor);
                    }
                    storage.Gltf.Accessors.Add(accessor);
                    return accessorIndex;
                }
            }

            var viewIndex = self.AddViewTo(storage, bufferIndex, offset, count);
            return self.AddAccessorTo(storage, viewIndex, minMax, 0, count);
        }
    }
}
