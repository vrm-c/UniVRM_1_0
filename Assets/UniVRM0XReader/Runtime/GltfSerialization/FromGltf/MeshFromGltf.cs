using System;
using System.Collections.Generic;
using System.Linq;
using GltfFormat;
using VrmLib;

namespace GltfSerializationAdapter
{
    public static class MeshFromGltf
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
        public static Mesh SharedBufferFromGltf(this GltfMesh x, GltfSerialization.GltfStorage storage)
        {
            // 先頭を使う
            return FromGltf(storage, x, x.primitives[0], true);
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
        public static Mesh FromGltf(this GltfPrimitive primitive, GltfSerialization.GltfStorage storage, GltfMesh x)
        {
            return FromGltf(storage, x, primitive, false);
        }

        static Mesh FromGltf(GltfSerialization.GltfStorage storage, GltfMesh x, GltfPrimitive primitive, bool isShared)
        {
            var mesh = new Mesh((TopologyType)primitive.mode)
            {
                VertexBuffer = primitive.attributes.FromGltf(storage)
            };

            if (isShared)
            {
                // create joined index buffer
                mesh.IndexBuffer = storage.IndexBufferFromGltf(x.primitives.Select(y => y.indices).ToArray());
            }
            else
            {
                mesh.IndexBuffer = storage.AccessorFromGltf(primitive.indices);
            }

            if (primitive.targets != null)
            {
                for (int i = 0; i < primitive.targets.Count; ++i)
                {
                    var gltfTarget = primitive.targets[i];
                    string targetName = null;
                    if (primitive.extras != null
                    && primitive.extras.targetNames != null
                    && i < primitive.extras.targetNames.Count
                    )
                    {
                        targetName = primitive.extras.targetNames[i];
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

        public static MeshGroup FromGltf(this GltfMesh x,
            GltfSerialization.GltfStorage storage, List<Material> materials)
        {
            var group = new MeshGroup(x.name);

            if (x.primitives.Count == 1)
            {
                var primitive = x.primitives[0];
                var mesh = primitive.FromGltf(storage, x);

                mesh.Submeshes.Add(
                    new Submesh(0, mesh.IndexBuffer.Count, materials[primitive.material]));

                group.Meshes.Add(mesh);
            }
            else if (!x.AllPrimitivesHasSameVertexBuffer())
            {
                int offset = 0;
                foreach (var primitive in x.primitives)
                {
                    var mesh = primitive.FromGltf(storage, x);

                    mesh.Submeshes.Add(
                        new Submesh(offset, mesh.IndexBuffer.Count, materials[primitive.material]));
                    offset += mesh.IndexBuffer.Count;

                    group.Meshes.Add(mesh);
                }
            }
            else
            {
                // for VRM

                var mesh = x.SharedBufferFromGltf(storage);
                int offset = 0;
                foreach (var primitive in x.primitives)
                {
                    var count = storage.Gltf.accessors[primitive.indices].count;
                    mesh.Submeshes.Add(
                        new Submesh(offset, count, materials[primitive.material]));
                    offset += count;
                }

                group.Meshes.Add(mesh);
            }

            return group;
        }
    }
}
