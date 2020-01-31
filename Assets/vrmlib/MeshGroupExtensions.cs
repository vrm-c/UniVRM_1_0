using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace VrmLib
{
    public static class MeshGroupExtensions
    {
        /// <summary>
        /// MorphTarget が有る Mesh と無い Mesh に分ける
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static ValueTuple<MeshGroup, MeshGroup> SepareteByMorphTarget(this MeshGroup g)
        {
            if (g.Meshes.Count > 1)
            {
                throw new NotImplementedException("MeshGroup.Meshes.Count must 1");
            }

            var src = g.Meshes[0];
            if (src.Topology != TopologyType.Triangles)
            {
                throw new InvalidOperationException("not GltfPrimitiveMode.Triangles");
            }

            var (withTriangles, withoutTriangles) = MeshMorphTargetSplitter.SplitTriangles(src);

            MeshGroup with = default(MeshGroup);
            if (withTriangles.Any())
            {
                var mesh = MeshMorphTargetSplitter.SeparateMesh(src, withTriangles, true);
                with = new MeshGroup(g.Name + ".blendshape")
                {
                    Skin = g.Skin,
                };
                with.Meshes.Add(mesh);
            }

            MeshGroup without = default(MeshGroup);
            if (withoutTriangles.Any())
            {
                var mesh = MeshMorphTargetSplitter.SeparateMesh(src, withoutTriangles);
                without = new MeshGroup(g.Name)
                {
                    Skin = g.Skin,
                };
                without.Meshes.Add(mesh);
            }

            return (with, without);
        }

        public static MeshGroup Clone(this MeshGroup src)
        {
            throw new NotImplementedException();

            // var dst = new MeshGroup(src.Name);

            // return dst;
        }
    }
}