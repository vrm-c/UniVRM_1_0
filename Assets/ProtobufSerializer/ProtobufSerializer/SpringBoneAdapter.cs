using System.Collections.Generic;
using VrmLib;

namespace Vrm10
{
    public static class SpringBoneAdapter
    {
        public static VrmProtobuf.SpringBone.Types.ColliderGroup ToGltf(this SpringBoneColliderGroup x, List<Node> nodes)
        {
            var node = nodes.IndexOfThrow(x.Node);
            var colliderGroup = new VrmProtobuf.SpringBone.Types.ColliderGroup
            {
                Node = node,
            };
            foreach (var y in x.Colliders)
            {
                var collider = new VrmProtobuf.SpringBone.Types.ColliderGroup.Types.Collider
                {
                    Radius = y.Radius,
                };
                collider.Offset.Add(y.Offset.X);
                collider.Offset.Add(y.Offset.Y);
                collider.Offset.Add(y.Offset.Z);
                colliderGroup.Colliders.Add(collider);
            }
            return colliderGroup;
        }

        // boneGroups = springBone.Springs.Select(x => x.ToGltf(nodes, springBone.Colliders)).ToList(),

        public static VrmProtobuf.SpringBone.Types.BoneGroup ToGltf(this SpringBone self, List<Node> nodes, List<SpringBoneColliderGroup> colliders)
        {
            var boneGroup = new VrmProtobuf.SpringBone.Types.BoneGroup
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

        public static VrmProtobuf.SpringBone ToGltf(this SpringBoneManager self, List<Node> nodes)
        {
            if (self == null)
            {
                return null;
            }

            var springBone = new VrmProtobuf.SpringBone();

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