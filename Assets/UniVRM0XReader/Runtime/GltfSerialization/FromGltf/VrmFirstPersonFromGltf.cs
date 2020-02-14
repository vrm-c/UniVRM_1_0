using GltfFormat;
using VrmLib;
using System.Collections.Generic;
using System.Linq;

namespace GltfSerializationAdapter
{
    public static class VrmFirstPersonFromGltf
    {
        public static FirstPerson FromGltf(this VrmFirstPerson fp, List<Node> nodes, List<MeshGroup> meshes)
        {
            var self = new FirstPerson();
            // if (fp.firstPersonBone >= 0)
            // {
            //     self.m_fp = nodes[fp.firstPersonBone];
            // }
            // self.m_offset = fp.firstPersonBoneOffset;
            self.Annotations.AddRange(fp.meshAnnotations
                .Select(x => new FirstPersonMeshAnnotation(meshes[x.mesh], (FirstPersonMeshType)x.firstPersonFlag)));
            return self;
        }
    }
}
