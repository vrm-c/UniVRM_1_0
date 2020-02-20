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
            self.Annotations.AddRange(fp.meshAnnotations
                 .Select(x =>
                 {
                     var meshGroup = meshes[x.mesh];
                     var node = nodes.First(y => y.MeshGroup == meshGroup);
                     return new FirstPersonMeshAnnotation(node, (FirstPersonMeshType)x.firstPersonFlag);
                 }));
            return self;
        }
    }
}
