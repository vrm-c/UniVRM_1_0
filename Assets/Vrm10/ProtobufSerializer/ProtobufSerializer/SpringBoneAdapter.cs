using System;
using System.Collections.Generic;
using VrmLib;
using pbc = global::Google.Protobuf.Collections;

namespace Vrm10
{
    public static class SpringBoneAdapter
    {
        public static VrmProtobuf.SpringSetting ToGltf(this SpringBone self, List<Node> nodes)
        {
            var setting = new VrmProtobuf.SpringSetting
            {
                DragForce = self.DragForce,
                GravityPower = self.GravityPower,
                Stiffness = self.Stiffness,
            };
            setting.GravityDir.Add(self.GravityDir.X);
            setting.GravityDir.Add(self.GravityDir.Y);
            setting.GravityDir.Add(self.GravityDir.Z);

            return setting;
        }

        public static VrmProtobuf.VRMCSpringBone ToGltf(this SpringBoneManager self, List<Node> nodes,
            pbc::RepeatedField<global::VrmProtobuf.Node> protoNodes)
        {
            if (self == null)
            {
                return null;
            }

            var springBone = new VrmProtobuf.VRMCSpringBone();

            //
            // VRMC_node_collider
            //
            foreach (var x in self.Colliders)
            {
                var index = nodes.IndexOfThrow(x.Node);
                var collider = new VrmProtobuf.VRMC_node_collider();
                foreach (var y in x.Colliders)
                {
                    switch (y.ColliderType)
                    {
                        case VrmSpringBoneColliderTypes.Sphere:
                            {
                                var sphere = new VrmProtobuf.Sphere
                                {
                                    Radius = y.Radius,
                                };
                                sphere.Offset.Add(y.Offset.X);
                                sphere.Offset.Add(y.Offset.Y);
                                sphere.Offset.Add(y.Offset.Z);
                                collider.Shapes.Add(new VrmProtobuf.ColliderShape
                                {
                                    Sphere = sphere,
                                });
                                break;
                            }

                        case VrmSpringBoneColliderTypes.Capsule:
                            {
                                var capsule = new VrmProtobuf.Capsule
                                {
                                    Radius = y.Radius,
                                };
                                capsule.Offset.Add(y.Offset.X);
                                capsule.Offset.Add(y.Offset.Y);
                                capsule.Offset.Add(y.Offset.Z);
                                capsule.Tail.Add(y.CapsuleTail.X);
                                capsule.Tail.Add(y.CapsuleTail.Y);
                                capsule.Tail.Add(y.CapsuleTail.Z);
                                collider.Shapes.Add(new VrmProtobuf.ColliderShape
                                {
                                    Capsule = capsule,
                                });
                            }
                            break;

                        default:
                            throw new NotImplementedException();
                    }
                }
                protoNodes[index].Extensions.VRMCNodeCollider = collider;
            }

            //
            // VRMC_springBone
            //
            foreach (var x in self.Springs)
            {
                var settingIndex = springBone.Settings.Count;
                springBone.Settings.Add(x.ToGltf(nodes));
                foreach (var bone in x.Bones)
                {
                    var spring = new VrmProtobuf.Spring
                    {
                        Name = x.Comment,
                        HitRadius = x.HitRadius,
                        SpringRoot = nodes.IndexOfThrow(bone),
                        Setting = settingIndex,
                    };
                    foreach (var y in x.Colliders)
                    {
                        spring.Colliders.Add(nodes.IndexOfThrow(y.Node));
                    }
                    springBone.Springs.Add(spring);
                }
            }

            return springBone;
        }
    }
}
