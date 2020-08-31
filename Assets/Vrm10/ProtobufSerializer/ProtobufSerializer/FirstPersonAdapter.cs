using System;
using System.Collections.Generic;
using System.Linq;
using VrmLib;

namespace Vrm10
{
    public static class FirstPersonAdapter
    {
        public static VrmLib.FirstPersonMeshType FromGltf(this VrmProtobuf.MeshAnnotation.Types.FirstPersonType src)
        {
            switch (src)
            {
                case VrmProtobuf.MeshAnnotation.Types.FirstPersonType.Auto: return FirstPersonMeshType.Auto;
                case VrmProtobuf.MeshAnnotation.Types.FirstPersonType.Both: return FirstPersonMeshType.Both;
                case VrmProtobuf.MeshAnnotation.Types.FirstPersonType.FirstPersonOnly: return FirstPersonMeshType.FirstPersonOnly;
                case VrmProtobuf.MeshAnnotation.Types.FirstPersonType.ThirdPersonOnly: return FirstPersonMeshType.ThirdPersonOnly;
            }

            throw new NotImplementedException();
        }

        public static FirstPerson FromGltf(this VrmProtobuf.FirstPerson fp, List<Node> nodes)
        {
            var self = new FirstPerson();
            // self.m_offset = fp.FirstPersonBoneOffset.ToVector3();
            self.Annotations.AddRange(fp.MeshAnnotations
                .Select(x => new FirstPersonMeshAnnotation(nodes[x.Node.Value], x.FirstPersonType.FromGltf())));
            return self;
        }
        public static VrmProtobuf.FirstPerson ToGltf(this FirstPerson self, List<Node> nodes)
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
                firstPerson.MeshAnnotations.Add(new VrmProtobuf.MeshAnnotation
                {
                    Node = nodes.IndexOfThrow(x.Node),
                    FirstPersonType = EnumUtil.Cast<VrmProtobuf.MeshAnnotation.Types.FirstPersonType>(x.FirstPersonFlag),
                });
            }
            return firstPerson;
        }
    }
}