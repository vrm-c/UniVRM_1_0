using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VrmLib;

namespace Vrm10
{
    public class Vrm10Exporter : IVrmExporter
    {
        public readonly Vrm10Storage Storage = new Vrm10Storage();

        VrmProtobuf.glTF Gltf => Storage.Gltf;

        public readonly string VrmExtensionName = "VRMC_vrm";

        public Vrm10Exporter()
        {
            Gltf.ExtensionsUsed.Add(UnlitMaterial.ExtensionName);
            Gltf.ExtensionsUsed.Add(SpringBone.ExtensionName);
            Gltf.Buffers.Add(new VrmProtobuf.Buffer
            {

            });
        }

        public byte[] ToBytes()
        {
            Gltf.Buffers[0].ByteLength = Storage.Buffers[0].Bytes.Count;

            var settings = Google.Protobuf.JsonFormatter.Settings.Default.WithPreserveProtoFieldNames(true);
            var json = new Google.Protobuf.JsonFormatter(settings).Format(Gltf);
            var glb = new Glb(new GlbChunk(json), new GlbChunk(Storage.Buffers[0].Bytes));
            return glb.ToBytes();
        }

        public void ExportAsset(Model model)
        {
            Gltf.Asset = new VrmProtobuf.Asset
            {
            };
            if (!string.IsNullOrEmpty(model.AssetVersion)) Gltf.Asset.Version = model.AssetVersion;
            if (!string.IsNullOrEmpty(model.AssetMinVersion)) Gltf.Asset.MinVersion = model.AssetMinVersion;

            if (!string.IsNullOrEmpty(model.AssetGenerator)) Gltf.Asset.Generator = model.AssetGenerator;
            if (model.Vrm != null && !string.IsNullOrEmpty(model.Vrm.ExporterVersion))
            {
                Gltf.Asset.Generator = model.Vrm.ExporterVersion;
            }

            if (!string.IsNullOrEmpty(model.AssetCopyright)) Gltf.Asset.Copyright = model.AssetCopyright;
        }

        public void Reserve(int bytesLength)
        {
            Storage.Reserve(bytesLength);
        }

        public void ExportImageAndTextures(List<Image> images, List<Texture> textures)
        {
            foreach (var x in images)
            {
                Gltf.Images.Add(x.ToGltf(Storage));
            }
            foreach (var x in textures)
            {
                if (x is ImageTexture imageTexture)
                {
                    var samplerIndex = Gltf.Samplers.Count;
                    Gltf.Samplers.Add(x.Sampler.ToGltf());
                    Gltf.Textures.Add(new VrmProtobuf.Texture
                    {
                        Name = x.Name,
                        Source = images.IndexOfThrow(imageTexture.Image),
                        Sampler = samplerIndex,
                        // extensions
                        // = imageTexture.Image.MimeType.Equals("image/webp") ? new GltfTextureExtensions() { EXT_texture_webp = new EXT_texture_webp() { source = images.IndexOf(imageTexture.Image) } }
                        // : imageTexture.Image.MimeType.Equals("image/vnd-ms.dds") ? new GltfTextureExtensions() { MSFT_texture_dds = new MSFT_texture_dds() { source = images.IndexOf(imageTexture.Image) } }
                        // : null
                    });
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public void ExportMaterialPBR(Material src, PBRMaterial pbr, List<Texture> textures)
        {
            var material = pbr.PBRToGltf(textures);
            Gltf.Materials.Add(material);
        }

        public void ExportMaterialUnlit(Material src, UnlitMaterial unlit, List<Texture> textures)
        {
            var material = unlit.UnlitToGltf(textures);
            Gltf.Materials.Add(material);
            if (!Gltf.ExtensionsUsed.Contains(UnlitMaterial.ExtensionName))
            {
                Gltf.ExtensionsUsed.Add(UnlitMaterial.ExtensionName);
            }
        }

        public void ExportMaterialMToon(Material src, MToonMaterial mtoon, List<Texture> textures)
        {
            if (!Gltf.ExtensionsUsed.Contains(UnlitMaterial.ExtensionName))
            {
                Gltf.ExtensionsUsed.Add(UnlitMaterial.ExtensionName);
            }

            var material = mtoon.MToonToGltf(textures);
            Gltf.Materials.Add(material);
            if (!Gltf.ExtensionsUsed.Contains(MToonMaterial.ExtensionName))
            {
                Gltf.ExtensionsUsed.Add(MToonMaterial.ExtensionName);
            }
        }

        public void ExportMeshes(List<MeshGroup> groups, List<Material> materials, ExportArgs option)
        {
            foreach (var group in groups)
            {
                var mesh = group.ExportMeshGroup(materials, Storage, option);
                Gltf.Meshes.Add(mesh);
            }
        }

        public void ExportNodes(Node root, List<Node> nodes, List<MeshGroup> groups, ExportArgs option)
        {
            foreach (var x in nodes)
            {
                var node = new VrmProtobuf.Node
                {
                    Name = x.Name,
                };

                node.Translation.Add(x.LocalTranslation.X);
                node.Translation.Add(x.LocalTranslation.Y);
                node.Translation.Add(x.LocalTranslation.Z);

                node.Rotation.Add(x.LocalRotation.X);
                node.Rotation.Add(x.LocalRotation.Y);
                node.Rotation.Add(x.LocalRotation.Z);
                node.Rotation.Add(x.LocalRotation.W);

                node.Scale.Add(x.LocalScaling.X);
                node.Scale.Add(x.LocalScaling.Y);
                node.Scale.Add(x.LocalScaling.Z);

                if (x.MeshGroup != null)
                {
                    node.Mesh = groups.IndexOfThrow(x.MeshGroup);
                    var skin = x.MeshGroup.Skin;
                    if (skin != null)
                    {
                        var skinIndex = Gltf.Skins.Count;
                        var gltfSkin = new VrmProtobuf.Skin();
                        foreach (var joint in skin.Joints)
                        {
                            gltfSkin.Joints.Add(nodes.IndexOfThrow(joint));
                        }
                        if (skin.InverseMatrices == null)
                        {
                            skin.CalcInverseMatrices();
                        }
                        if (skin.InverseMatrices != null)
                        {
                            gltfSkin.InverseBindMatrices = skin.InverseMatrices.AddAccessorTo(Storage, 0, option.sparse);
                        }
                        if (skin.Root != null)
                        {
                            gltfSkin.Skeleton = nodes.IndexOfNullable(skin.Root);
                        }
                        Gltf.Skins.Add(gltfSkin);
                        node.Skin = skinIndex;
                    }
                }

                foreach (var child in x.Children)
                {
                    node.Children.Add(nodes.IndexOfThrow(child));
                }

                Gltf.Nodes.Add(node);
            }

            Gltf.Scenes.Add(new VrmProtobuf.Scene());
            foreach (var child in root.Children)
            {
                Gltf.Scenes[0].Nodes.Add(nodes.IndexOfThrow(child));
            }
        }

        public void ExportAnimations(List<Animation> animations, List<Node> nodes, ExportArgs option)
        {
            // throw new System.NotImplementedException();
        }

        public void ExportVrmMeta(Vrm src, List<Texture> textures)
        {
            if (!Gltf.ExtensionsUsed.Contains(VrmExtensionName))
            {
                Gltf.ExtensionsUsed.Add(VrmExtensionName);
            }
            if (Gltf.Extensions == null)
            {
                Gltf.Extensions = new VrmProtobuf.glTF.Types.Extensions();
            }
            if (Gltf.Extensions.VRMCVrm == null)
            {
                Gltf.Extensions.VRMCVrm = new VrmProtobuf.VRMCVrm();
            }
            var vrm = Gltf.Extensions.VRMCVrm;
            vrm.SpecVersion = src.SpecVersion;
            vrm.Meta = src.Meta.ToGltf(textures);
        }

        public void ExportVrmHumanoid(Dictionary<HumanoidBones, Node> map, List<Node> nodes)
        {
            Gltf.Extensions.VRMCVrm.Humanoid = new VrmProtobuf.Humanoid();
            foreach (var kv in map.OrderBy(kv => kv.Key))
            {
                var humanoidBone = new VrmProtobuf.HumanBone
                {
                    Node = nodes.IndexOfThrow(kv.Value),
                };

                switch (kv.Key)
                {
                    case HumanoidBones.hips: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.Hips = humanoidBone; break;
                    case HumanoidBones.leftUpperLeg: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftUpperLeg = humanoidBone; break;
                    case HumanoidBones.rightUpperLeg: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightUpperLeg = humanoidBone; break;
                    case HumanoidBones.leftLowerLeg: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftLowerLeg = humanoidBone; break;
                    case HumanoidBones.rightLowerLeg: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightLowerLeg = humanoidBone; break;
                    case HumanoidBones.leftFoot: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftFoot = humanoidBone; break;
                    case HumanoidBones.rightFoot: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightFoot = humanoidBone; break;
                    case HumanoidBones.spine: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.Spine = humanoidBone; break;
                    case HumanoidBones.chest: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.Chest = humanoidBone; break;
                    case HumanoidBones.neck: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.Neck = humanoidBone; break;
                    case HumanoidBones.head: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.Head = humanoidBone; break;
                    case HumanoidBones.leftShoulder: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftShoulder = humanoidBone; break;
                    case HumanoidBones.rightShoulder: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightShoulder = humanoidBone; break;
                    case HumanoidBones.leftUpperArm: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftUpperArm = humanoidBone; break;
                    case HumanoidBones.rightUpperArm: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightUpperArm = humanoidBone; break;
                    case HumanoidBones.leftLowerArm: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftLowerArm = humanoidBone; break;
                    case HumanoidBones.rightLowerArm: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightLowerArm = humanoidBone; break;
                    case HumanoidBones.leftHand: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftHand = humanoidBone; break;
                    case HumanoidBones.rightHand: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightHand = humanoidBone; break;
                    case HumanoidBones.leftToes: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftToes = humanoidBone; break;
                    case HumanoidBones.rightToes: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightToes = humanoidBone; break;
                    case HumanoidBones.leftEye: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftEye = humanoidBone; break;
                    case HumanoidBones.rightEye: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightEye = humanoidBone; break;
                    case HumanoidBones.jaw: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.Jaw = humanoidBone; break;
                    case HumanoidBones.leftThumbProximal: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftThumbProximal = humanoidBone; break;
                    case HumanoidBones.leftThumbIntermediate: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftThumbIntermediate = humanoidBone; break;
                    case HumanoidBones.leftThumbDistal: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftThumbDistal = humanoidBone; break;
                    case HumanoidBones.leftIndexProximal: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftIndexProximal = humanoidBone; break;
                    case HumanoidBones.leftIndexIntermediate: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftIndexIntermediate = humanoidBone; break;
                    case HumanoidBones.leftIndexDistal: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftIndexDistal = humanoidBone; break;
                    case HumanoidBones.leftMiddleProximal: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftMiddleProximal = humanoidBone; break;
                    case HumanoidBones.leftMiddleIntermediate: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftMiddleIntermediate = humanoidBone; break;
                    case HumanoidBones.leftMiddleDistal: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftMiddleDistal = humanoidBone; break;
                    case HumanoidBones.leftRingProximal: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftRingProximal = humanoidBone; break;
                    case HumanoidBones.leftRingIntermediate: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftRingIntermediate = humanoidBone; break;
                    case HumanoidBones.leftRingDistal: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftRingDistal = humanoidBone; break;
                    case HumanoidBones.leftLittleProximal: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftLittleProximal = humanoidBone; break;
                    case HumanoidBones.leftLittleIntermediate: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftLittleIntermediate = humanoidBone; break;
                    case HumanoidBones.leftLittleDistal: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.LeftLittleDistal = humanoidBone; break;
                    case HumanoidBones.rightThumbProximal: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightThumbProximal = humanoidBone; break;
                    case HumanoidBones.rightThumbIntermediate: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightThumbIntermediate = humanoidBone; break;
                    case HumanoidBones.rightThumbDistal: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightThumbDistal = humanoidBone; break;
                    case HumanoidBones.rightIndexProximal: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightIndexProximal = humanoidBone; break;
                    case HumanoidBones.rightIndexIntermediate: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightIndexIntermediate = humanoidBone; break;
                    case HumanoidBones.rightIndexDistal: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightIndexDistal = humanoidBone; break;
                    case HumanoidBones.rightMiddleProximal: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightMiddleProximal = humanoidBone; break;
                    case HumanoidBones.rightMiddleIntermediate: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightMiddleIntermediate = humanoidBone; break;
                    case HumanoidBones.rightMiddleDistal: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightMiddleDistal = humanoidBone; break;
                    case HumanoidBones.rightRingProximal: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightRingProximal = humanoidBone; break;
                    case HumanoidBones.rightRingIntermediate: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightRingIntermediate = humanoidBone; break;
                    case HumanoidBones.rightRingDistal: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightRingDistal = humanoidBone; break;
                    case HumanoidBones.rightLittleProximal: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightLittleProximal = humanoidBone; break;
                    case HumanoidBones.rightLittleIntermediate: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightLittleIntermediate = humanoidBone; break;
                    case HumanoidBones.rightLittleDistal: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.RightLittleDistal = humanoidBone; break;
                    case HumanoidBones.upperChest: Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.UpperChest = humanoidBone; break;
                }

                // Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.Add(kv.Key.ToString(), humanoidBone);
            }
        }

        public void ExportVrmBlendShape(BlendShapeManager src, List<MeshGroup> _, List<Material> materials, List<Node> nodes)
        {
            Gltf.Extensions.VRMCVrm.BlendShape = src.ToGltf(nodes, materials);
        }

        public void ExportVrmSpringBone(SpringBoneManager springBone, List<Node> nodes)
        {
            Gltf.Extensions.VRMCSpringBone = springBone.ToGltf(nodes, Gltf.Nodes);
        }

        public void ExportVrmFirstPersonAndLookAt(FirstPerson firstPerson, LookAt lookat, List<MeshGroup> meshes, List<Node> nodes)
        {
            Gltf.Extensions.VRMCVrm.FirstPerson = firstPerson.ToGltf(nodes);
            Gltf.Extensions.VRMCVrm.LookAt = lookat.ToGltf();
        }

        public void ExportVrmMaterialProperties(List<Material> materials, List<Texture> textures)
        {
            // Do nothing
            // see
            // ExportMaterialPBR
            // ExportMaterialUnlit
            // ExportMaterialMToon
        }
    }
}
