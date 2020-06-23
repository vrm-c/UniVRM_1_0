using System.Collections.Generic;
using VrmLib;

namespace Vrm10
{
    public static class SpringBoneAdapter
    {
        public static VrmProtobuf.VRMCSpringBone.Types.ColliderGroup ToGltf(this SpringBoneColliderGroup x, List<Node> nodes)
        {
            var node = nodes.IndexOfThrow(x.Node);
            var colliderGroup = new VrmProtobuf.VRMCSpringBone.Types.ColliderGroup
            {
                Node = node,
            };
            foreach (var y in x.Colliders)
            {
                var collider = new VrmProtobuf.VRMCSpringBone.Types.ColliderGroup.Types.Collider
                {
                    Type = (VrmProtobuf.VRMCSpringBone.Types.ColliderGroup.Types.ColliderTypes)y.ColliderType,
                    Radius = y.Radius,
                };
                collider.Offset.Add(y.Offset.X);
                collider.Offset.Add(y.Offset.Y);
                collider.Offset.Add(y.Offset.Z);
                if (y.ColliderType == VrmSpringBoneColliderTypes.Capsule)
                {
                    collider.Tail.Add(y.CapsuleTail.X);
                    collider.Tail.Add(y.CapsuleTail.Y);
                    collider.Tail.Add(y.CapsuleTail.Z);
                }
                colliderGroup.Colliders.Add(collider);
            }
            return colliderGroup;
        }

        // boneGroups = springBone.Springs.Select(x => x.ToGltf(nodes, springBone.Colliders)).ToList(),

        public static VrmProtobuf.VRMCSpringBone.Types.BoneGroup ToGltf(this SpringBone self, List<Node> nodes, List<SpringBoneColliderGroup> colliders)
        {
            var boneGroup = new VrmProtobuf.VRMCSpringBone.Types.BoneGroup
            {
                Name = self.Comment,
                Center = nodes.IndexOfNullable(self.Origin),
                DragForce = self.DragForce,
                GravityPower = self.GravityPower,
                HitRadius = self.HitRadius,
                Stiffness = self.Stiffness,
            };
            boneGroup.GravityDir.Add(self.GravityDir.X);
            boneGroup.GravityDir.Add(self.GravityDir.Y);
            boneGroup.GravityDir.Add(self.GravityDir.Z);
            foreach (var x in self.Colliders)
            {
                boneGroup.ColliderGroups.Add(colliders.IndexOfThrow(x));
            }
            foreach (var x in self.Bones)
            {
                boneGroup.Bones.Add(nodes.IndexOfThrow(x));
            }
            return boneGroup;
        }

        public static VrmProtobuf.VRMCSpringBone ToGltf(this SpringBoneManager self, List<Node> nodes)
        {
            if (self == null)
            {
                return null;
            }

            var springBone = new VrmProtobuf.VRMCSpringBone();

            foreach (var x in self.Colliders)
            {
                springBone.ColliderGroups.Add(x.ToGltf(nodes));
            }
            foreach (var x in self.Springs)
            {
                springBone.BoneGroups.Add(x.ToGltf(nodes, self.Colliders));
            }

            return springBone;
        }
    }
}