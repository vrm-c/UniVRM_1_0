using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using VrmLib;

namespace Vrm10
{
    public static class MeshAdapter
    {
        /// <summary>
        /// VertexBufferはひとつでIndexBufferの参照が異なる
        ///
        ///  VertexBuffer
        ///  +----------------------------------+
        ///  |                                  |
        ///  +----------------------------------+
        ///       A         A        A
        ///       |         |        |
        ///  +---------+--------+--------+
        ///  | submesh0|submesh1|submesh2|
        ///  +---------+--------+--------+
        ///  IndexBuffer
        /// </summary>
        public static Mesh SharedBufferFromGltf(this VrmProtobuf.Mesh x, Vrm10Storage storage)
        {
            // 先頭を使う
            return FromGltf(storage, x, x.Primitives[0], true);
        }

        /// <summary>
        /// IndexBuffer毎に異なるVertexBufferを参照する
        ///
        ///  VertexBuffer
        ///  +--------+ +--------+ +--------+
        ///  |0       | |1       | |2       |
        ///  +--------+ +--------+ +--------+
        ///       A         A        A
        ///       |         |        |
        ///  +---------+--------+--------+
        ///  | submesh0|submesh1|submesh2|
        ///  +---------+--------+--------+
        ///  IndexBuffer
        /// </summary>
        public static Mesh FromGltf(this VrmProtobuf.MeshPrimitive primitive, Vrm10Storage storage, VrmProtobuf.Mesh x)
        {
            return FromGltf(storage, x, primitive, false);
        }

        static Mesh FromGltf(Vrm10Storage storage, VrmProtobuf.Mesh x, VrmProtobuf.MeshPrimitive primitive, bool isShared)
        {
            var mesh = new Mesh((TopologyType)primitive.Mode.GetValueOrDefault())
            {
                VertexBuffer = primitive.Attributes.FromGltf(storage)
            };

            if (isShared)
            {
                // create joined index buffer
                mesh.IndexBuffer = storage.CreateAccessor(x.Primitives.Select(y => y.Indices.Value).ToArray());
            }
            else
            {
                mesh.IndexBuffer = storage.CreateAccessor(primitive.Indices.Value);
            }

            {
                for (int i = 0; i < primitive.Targets.Count; ++i)
                {
                    var gltfTarget = primitive.Targets[i];
                    string targetName = null;
                    if (primitive.Extras != null
                    && primitive.Extras.TargetNames != null
                    && i < primitive.Extras.TargetNames.Count
                    )
                    {
                        targetName = primitive.Extras.TargetNames[i];
                    }
                    var target = new MorphTarget(targetName)
                    {
                        VertexBuffer = gltfTarget.FromGltf(storage)
                    };

                    // validate count
                    foreach (var kv in target.VertexBuffer)
                    {
                        if (kv.Value.Count != mesh.VertexBuffer.Count)
                        {
                            throw new Exception();
                        }
                    }

                    mesh.MorphTargets.Add(target);
                }
            }

            return mesh;
        }

        public static VertexBuffer FromGltf(this global::Google.Protobuf.Collections.MapField<string, int> attributes,
            Vrm10Storage storage)
        {
            var b = new VertexBuffer();
            foreach (var kv in attributes)
            {
                var accessor = storage.CreateAccessor(kv.Value);
                b.Add(kv.Key, accessor);
            }
            b.ValidateLength();
            return b;
        }

        public static VertexBuffer FromGltf(this VrmProtobuf.MeshPrimitive.Types.Target target, Vrm10Storage storage)
        {
            var b = new VertexBuffer();
            storage.CreateBufferAccessorAndAdd(target.POSITION, b, VertexBuffer.PositionKey);
            storage.CreateBufferAccessorAndAdd(target.NORMAL, b, VertexBuffer.NormalKey);
            storage.CreateBufferAccessorAndAdd(target.TANGENT, b, VertexBuffer.TangentKey);
            b.ValidateLength();
            return b;
        }

        public static bool HasSameVertexBuffer(this VrmProtobuf.MeshPrimitive lhs, VrmProtobuf.MeshPrimitive rhs)
        {
            return lhs.Attributes.Keys.OrderBy(x => x).SequenceEqual(rhs.Attributes.Keys.OrderBy(x => x));
        }

        public static bool AllPrimitivesHasSameVertexBuffer(this VrmProtobuf.Mesh m)
        {
            if (m.Primitives.Count <= 1)
            {
                return true;
            }

            var first = m.Primitives[0];
            for (int i = 1; i < m.Primitives.Count; ++i)
            {
                if (!first.HasSameVertexBuffer(m.Primitives[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static MeshGroup FromGltf(this VrmProtobuf.Mesh x,
            Vrm10Storage storage, List<Material> materials)
        {
            var group = new MeshGroup(x.Name);

            if (x.Primitives.Count == 1)
            {
                var primitive = x.Primitives[0];
                var mesh = primitive.FromGltf(storage, x);
                var materialIndex = primitive.Material.HasValue ? primitive.Material.Value : 0;

                mesh.Submeshes.Add(
                    new Submesh(0, mesh.IndexBuffer.Count, materials[materialIndex]));

                group.Meshes.Add(mesh);
            }
            else if (!x.AllPrimitivesHasSameVertexBuffer())
            {
                int offset = 0;
                foreach (var primitive in x.Primitives)
                {
                    var mesh = primitive.FromGltf(storage, x);
                    var materialIndex = primitive.Material.HasValue ? primitive.Material.Value : 0;

                    mesh.Submeshes.Add(
                        new Submesh(offset, mesh.IndexBuffer.Count, materials[materialIndex]));
                    offset += mesh.IndexBuffer.Count;

                    group.Meshes.Add(mesh);
                }
            }
            else
            {
                // for VRM

                var mesh = x.SharedBufferFromGltf(storage);
                int offset = 0;
                foreach (var primitive in x.Primitives)
                {
                    var materialIndex = primitive.Material.HasValue ? primitive.Material.Value : 0;
                    var count = storage.Gltf.Accessors[primitive.Indices.Value].Count;
                    mesh.Submeshes.Add(
                        new Submesh(offset, count, materials[materialIndex]));
                    offset += count;
                }

                group.Meshes.Add(mesh);
            }

            return group;
        }

        static void Vec3MinMax(Memory<byte> bytes, VrmProtobuf.Accessor accessor)
        {
            var positions = MemoryMarshal.Cast<byte, Vector3>(bytes.Span);
            var min = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
            var max = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
            foreach (var p in positions)
            {
                min = Vector3.Min(min, p);
                max = Vector3.Max(max, p);
            }
            accessor.Min.Add(min.X);
            accessor.Min.Add(min.Y);
            accessor.Min.Add(min.Z);
            accessor.Max.Add(max.X);
            accessor.Max.Add(max.Y);
            accessor.Max.Add(max.Z);
        }

        static int ExportIndices(Vrm10Storage storage, BufferAccessor x, int offset, int count, ExportArgs option)
        {
            if (x.Count <= ushort.MaxValue)
            {
                if (x.ComponentType == AccessorValueType.UNSIGNED_INT)
                {
                    // ensure ushort
                    var src = x.GetSpan<UInt32>().Slice(offset, count);
                    var bytes = new byte[src.Length * 2];
                    var dst = MemoryMarshal.Cast<byte, ushort>(bytes);
                    for (int i = 0; i < src.Length; ++i)
                    {
                        dst[i] = (ushort)src[i];
                    }
                    var accessor = new BufferAccessor(bytes, AccessorValueType.UNSIGNED_SHORT, AccessorVectorType.SCALAR, count);
                    return accessor.AddAccessorTo(storage, 0, option.sparse, null, 0, count);
                }
                else
                {
                    return x.AddAccessorTo(storage, 0, option.sparse, null, offset, count);
                }
            }
            else
            {
                return x.AddAccessorTo(storage, 0, option.sparse, null, offset, count);
            }
        }

        static void ExportMesh(this Mesh mesh, List<Material> materials, Vrm10Storage storage, VrmProtobuf.Mesh gltfMesh, ExportArgs option)
        {
            //
            // primitive share vertex buffer
            //
            var attributeAccessorIndexMap = mesh.VertexBuffer
                .ToDictionary(
                    kv => kv.Key,
                    kv => kv.Value.AddAccessorTo(
                        storage, 0, option.sparse,
                        kv.Key == VertexBuffer.PositionKey ? (Action<Memory<byte>, VrmProtobuf.Accessor>)Vec3MinMax : null
                    )
                );

            List<Dictionary<string, int>> morphTargetAccessorIndexMapList = null;
            if (mesh.MorphTargets.Any())
            {
                morphTargetAccessorIndexMapList = new List<Dictionary<string, int>>();
                foreach (var morphTarget in mesh.MorphTargets)
                {
                    var dict = new Dictionary<string, int>();

                    foreach (var kv in morphTarget.VertexBuffer)
                    {
                        if (option.removeTangent && kv.Key == VertexBuffer.TangentKey)
                        {
                            // remove tangent
                            continue;
                        }
                        if (option.removeMorphNormal && kv.Key == VertexBuffer.NormalKey)
                        {
                            // normal normal
                            continue;
                        }
                        if (kv.Value.Count != mesh.VertexBuffer.Count)
                        {
                            throw new Exception("inavlid data");
                        }
                        var accessorIndex = kv.Value.AddAccessorTo(storage, 0,
                        option.sparse,
                        kv.Key == VertexBuffer.PositionKey ? (Action<Memory<byte>, VrmProtobuf.Accessor>)Vec3MinMax : null);
                        dict.Add(kv.Key, accessorIndex);
                    }

                    morphTargetAccessorIndexMapList.Add(dict);
                }
            }

            var drawCountOffset = 0;
            foreach (var y in mesh.Submeshes)
            {
                // index
                // slide index buffer accessor
                var indicesAccessorIndex = ExportIndices(storage, mesh.IndexBuffer, drawCountOffset, y.DrawCount, option);
                drawCountOffset += y.DrawCount;

                var prim = new VrmProtobuf.MeshPrimitive
                {
                    Mode = (int)mesh.Topology,
                    Material = materials.IndexOfNullable(y.Material),
                    Indices = indicesAccessorIndex,
                };
                gltfMesh.Primitives.Add(prim);

                // attribute
                foreach (var kv in mesh.VertexBuffer)
                {
                    var attributeAccessorIndex = attributeAccessorIndexMap[kv.Key];
                    prim.Attributes.Add(kv.Key, attributeAccessorIndex);
                }

                // morph target
                if (mesh.MorphTargets.Any())
                {
                    foreach (var (t, accessorIndexMap) in
                        Enumerable.Zip(mesh.MorphTargets, morphTargetAccessorIndexMapList, (t, v) => (t, v)))
                    {
                        var target = new VrmProtobuf.MeshPrimitive.Types.Target();
                        prim.Targets.Add(target);

                        foreach (var kv in t.VertexBuffer)
                        {
                            if (!accessorIndexMap.TryGetValue(kv.Key, out int targetAccessorIndex))
                            {
                                continue;
                            }
                            switch (kv.Key)
                            {
                                case VertexBuffer.PositionKey:
                                    target.POSITION = targetAccessorIndex;
                                    break;
                                case VertexBuffer.NormalKey:
                                    target.NORMAL = targetAccessorIndex;
                                    break;
                                case VertexBuffer.TangentKey:
                                    target.TANGENT = targetAccessorIndex;
                                    break;

                                default:
                                    throw new NotImplementedException();
                            }
                        }
                    }

                    if (mesh.MorphTargets.Any())
                    {
                        prim.Extras = new VrmProtobuf.MeshPrimitive.Types.Extras();
                        foreach (var name in mesh.MorphTargets.Select(z => z.Name))
                        {
                            prim.Extras.TargetNames.Add(name);
                        }
                    }
                }
            }
        }
        public static VrmProtobuf.Mesh ExportMeshGroup(this MeshGroup src, List<Material> materials, Vrm10Storage storage, ExportArgs option)
        {
            var mesh = new VrmProtobuf.Mesh
            {
                Name = src.Name
            };

            foreach (var x in src.Meshes)
            {
                // MeshとSubmeshがGltfのPrimitiveに相当する？
                x.ExportMesh(materials, storage, mesh, option);
            }

            return mesh;
        }
    }
}