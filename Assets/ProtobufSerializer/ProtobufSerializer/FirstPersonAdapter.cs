using System;
using System.Collections.Generic;
using System.Linq;
using VrmLib;

namespace Vrm10
{
    public static class FirstPersonAdapter
    {
        public static VrmLib.FirstPersonMeshType FromGltf(this VrmProtobuf.FirstPerson.Types.MeshAnnotation.Types.FirstPersonType src)
        {
            switch (src)
            {
                case VrmProtobuf.FirstPerson.Types.MeshAnnotation.Types.FirstPersonType.Auto: return FirstPersonMeshType.Auto;
                case VrmProtobuf.FirstPerson.Types.MeshAnnotation.Types.FirstPersonType.Both: return FirstPersonMeshType.Both;
                case VrmProtobuf.FirstPerson.Types.MeshAnnotation.Types.FirstPersonType.FirstPersonOnly: return FirstPersonMeshType.FirstPersonOnly;
                case VrmProtobuf.FirstPerson.Types.MeshAnnotation.Types.FirstPersonType.ThirdPersonOnly: return FirstPersonMeshType.ThirdPersonOnly;
            }

            throw new NotImplementedException();
        }

        public static FirstPerson FromGltf(this VrmProtobuf.FirstPerson fp, List<Node> nodes, List<MeshGroup> meshes)
        {
            var self = new FirstPerson();
            // self.m_offset = fp.FirstPersonBoneOffset.ToVector3();
            self.Annotations.AddRange(fp.MeshAnnotations
                .Select(x => new FirstPersonMeshAnnotation(meshes[x.Mesh], x.FirstPersonType.FromGltf())));
            return self;
        }
        public static VrmProtobuf.FirstPerson ToGltf(this FirstPerson self, List<Node> nodes, List<MeshGroup> meshes)
        {
            if (self == null)
            {
                return null;
            }

            var firstPerson = new VrmProtobuf.FirstPerson
            {

            };

            foreach (var x in self.Annotations)
            {
                firstPerson.MeshAnnotations.Add(new VrmProtobuf.FirstPerson.Types.MeshAnnotation
                {
                    Mesh = meshes.IndexOfThrow(x.Mesh),
                    FirstPersonType = (VrmProtobuf.FirstPerson.Types.MeshAnnotation.Types.FirstPersonType)x.FirstPersonFlag,
                });
            }
            return firstPerson;
        }
    }
}