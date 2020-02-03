using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace VrmLib
{
    class MeshMorphTargetSplitter
    {
        public static ValueTuple<
            List<ValueTuple<int, Triangle>>,
            List<ValueTuple<int, Triangle>>> SplitTriangles(Mesh src)
        {
            var triangles = src.Triangles.ToArray();
            var morphUseCount = new int[triangles.Length];

            // 各モーフ
            foreach (var morph in src.MorphTargets)
            {
                // POSITIONが使っているtriangleのカウンターをアップさせる
                var positions = morph.VertexBuffer.Positions.GetSpan<Vector3>();
                for (int i = 0; i < triangles.Length; ++i)
                {
                    ref var triangle = ref triangles[i];
                    if (positions[triangle.Item2.Vertex0] != Vector3.Zero)
                    {
                        ++morphUseCount[i];
                    }
                    if (positions[triangle.Item2.Vertex1] != Vector3.Zero)
                    {
                        ++morphUseCount[i];
                    }
                    if (positions[triangle.Item2.Vertex2] != Vector3.Zero)
                    {
                        ++morphUseCount[i];
                    }
                }
            }

            var withTriangles = new List<ValueTuple<int, Triangle>>();
            var withoutTriangles = new List<ValueTuple<int, Triangle>>();
            for (int i = 0; i < triangles.Length; ++i)
            {
                if (morphUseCount[i] > 0)
                {
                    // モーフで使われている
                    withTriangles.Add(triangles[i]);
                }
                else
                {
                    // モーフで使われない
                    withoutTriangles.Add(triangles[i]);
                }
            }

            return (withTriangles, withoutTriangles);
        }

        class VertexReorderMapper
        {
            public readonly List<int> IndexMap = new List<int>();

            public BufferAccessor MapBuffer<T>(BufferAccessor srcBuffer) where T : struct
            {
                var src = srcBuffer.GetSpan<T>();
                var dstBytes = new byte[srcBuffer.Stride * IndexMap.Count];
                var dst = MemoryMarshal.Cast<byte, T>(dstBytes.AsSpan());
                for (int i = 0; i < IndexMap.Count; ++i)
                {
                    dst[i] = src[IndexMap[i]];
                }
                return new BufferAccessor(dstBytes, srcBuffer.ComponentType, srcBuffer.AccessorType, IndexMap.Count);
            }

            public VertexBuffer Map(VertexBuffer src)
            {
                var dst = new VertexBuffer();
                foreach (var (semantic, buffer) in src)
                {
                    BufferAccessor mapped = null;
                    if (buffer.ComponentType == AccessorValueType.FLOAT)
                    {
                        switch (buffer.AccessorType)
                        {
                            case AccessorVectorType.VEC2:
                                mapped = MapBuffer<Vector2>(buffer);
                                break;

                            case AccessorVectorType.VEC3:
                                mapped = MapBuffer<Vector3>(buffer);
                                break;

                            case AccessorVectorType.VEC4:
                                mapped = MapBuffer<Vector4>(buffer);
                                break;

                            default:
                                throw new NotImplementedException();
                        }
                    }
                    else if (buffer.ComponentType == AccessorValueType.UNSIGNED_SHORT)
                    {
                        if (buffer.AccessorType == AccessorVectorType.VEC4)
                        {
                            mapped = MapBuffer<SkinJoints>(buffer);
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                    dst.Add(semantic, mapped);
                }

                return dst;
            }

            public readonly List<int> Indices = new List<int>();

            public Memory<byte> IndexBytes
            {
                get
                {
                    var bytes = new byte[Indices.Count * 4];
                    var span = MemoryMarshal.Cast<byte, int>(bytes.AsMemory().Span);
                    for (int i = 0; i < span.Length; ++i)
                    {
                        span[i] = Indices[i];
                    }
                    return bytes;
                }
            }

            public BufferAccessor CreateIndexBuffer()
            {
                return new BufferAccessor(IndexBytes,
                AccessorValueType.UNSIGNED_INT, AccessorVectorType.SCALAR, Indices.Count);
            }

            void PushIndex(int src)
            {
                int index = IndexMap.IndexOf(src);
                if (index == -1)
                {
                    // index to src map
                    index = IndexMap.Count;
                    IndexMap.Add(src);
                }
                Indices.Add(index);
            }

            public VertexReorderMapper(IEnumerable<ValueTuple<int, Triangle>> submeshTriangles)
            {
                foreach (var t in submeshTriangles)
                {
                    PushIndex(t.Item2.Vertex0);
                    PushIndex(t.Item2.Vertex1);
                    PushIndex(t.Item2.Vertex2);
                }
            }
        }
        public static Mesh SeparateMesh(Mesh src, IEnumerable<ValueTuple<int, Triangle>> submeshTriangles,
            bool includeMorphTarget = false)
        {
            var mapper = new VertexReorderMapper(submeshTriangles);

            var mesh = new Mesh
            {
                Topology = TopologyType.Triangles,
                IndexBuffer = mapper.CreateIndexBuffer(),
                VertexBuffer = mapper.Map(src.VertexBuffer),
            };

            var offset = 0;
            foreach (var triangles in submeshTriangles.GroupBy(x => x.Item1, x => x).OrderBy(x => x.Key))
            {
                var count = triangles.Count() * 3;
                mesh.Submeshes.Add(new Submesh(offset, count, src.Submeshes[triangles.Key].Material));
                offset += count;
            }

            if (includeMorphTarget)
            {
                foreach (var target in src.MorphTargets)
                {
                    mesh.MorphTargets.Add(new MorphTarget(target.Name)
                    {
                        VertexBuffer = mapper.Map(target.VertexBuffer)
                    });
                }
            }

            return mesh;
        }
    }
}