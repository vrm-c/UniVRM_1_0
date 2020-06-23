using System;
using System.Linq;
using GltfFormat;
using VrmLib;

namespace GltfSerializationAdapter
{
    public static class ModelFromGltf
    {
        public static bool LoadVrm(this Model self, Gltf gltf)
        {
            var gltfVrm = gltf.extensions?.VRM;
            if (gltfVrm == null)
            {
                return false;
            }

            var Vrm = new Vrm(gltfVrm.meta.FromGltf(self.Textures), gltfVrm.exporterVersion, gltfVrm.specVersion);
            self.Vrm = Vrm;

            if (gltfVrm.humanoid != null)
            {
                foreach (var humanBone in gltfVrm.humanoid.humanBones)
                {
                    if (humanBone.bone != GltfFormat.HumanoidBones.unknown)
                    {
                        self.Nodes[humanBone.node].HumanoidBone = (VrmLib.HumanoidBones)humanBone.bone;
                    }
                }
            }

            if (!self.CheckVrmHumanoid())
            {
                throw new Exception("duplicate human bone");
            }

            // blendshape
            if (gltfVrm.blendShapeMaster != null
                && gltfVrm.blendShapeMaster.blendShapeGroups != null
                && gltfVrm.blendShapeMaster.blendShapeGroups.Any())
            {
                Vrm.BlendShape = gltfVrm.blendShapeMaster.FromGltf(self.MeshGroups, self.Materials, self.Nodes);
            }

            // secondary
            if (!(gltfVrm.secondaryAnimation is null))
            {
                Vrm.SpringBone = new SpringBoneManager();

                // colliders
                Vrm.SpringBone.Colliders.AddRange(
                    gltfVrm.secondaryAnimation.colliderGroups.Select(y =>
                    new SpringBoneColliderGroup(
                        self.Nodes[y.node],
                        y.colliders.Select(z => VrmSpringBoneCollider.CreateSphere(z.offset, z.radius))
                    )
                ));

                // springs
                Vrm.SpringBone.Springs.AddRange(gltfVrm.secondaryAnimation.boneGroups.Select(x =>
                {
                    var sb = new SpringBone();
                    sb.Bones.AddRange(x.bones.Select(y => self.Nodes[y]));
                    if (x.center >= 0) sb.Origin = self.Nodes[x.center];
                    sb.Colliders.AddRange(x.colliderGroups.Select(y => Vrm.SpringBone.Colliders[y]));
                    sb.Comment = x.comment;
                    sb.DragForce = x.dragForce;
                    sb.GravityDir = x.gravityDir;
                    sb.GravityPower = x.gravityPower;
                    sb.HitRadius = x.hitRadius;
                    sb.Stiffness = x.stiffiness;
                    return sb;
                }));
            }

            // material(already replaced)

            if (gltfVrm.firstPerson != null)
            {
                Vrm.FirstPerson = gltfVrm.firstPerson.FromGltf(self.Nodes, self.MeshGroups);
                Vrm.LookAt = gltfVrm.firstPerson.LookAtFromGltf();
                Vrm.LookAt.OffsetFromHeadBone = gltfVrm.firstPerson.firstPersonBoneOffset;
            }

            return true;
        }
    }
}