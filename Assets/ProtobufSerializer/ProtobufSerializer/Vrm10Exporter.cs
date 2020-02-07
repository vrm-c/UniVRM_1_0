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
            var material = pbr.PBRToGltf(src.Name, textures);
            Gltf.Materials.Add(material);
        }

        public void ExportMaterialUnlit(Material src, UnlitMaterial unlit, List<Texture> textures)
        {
            var material = unlit.UnlitToGltf(src.Name, textures);
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

            var material = mtoon.MToonToGltf(src.Name, textures);
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
                Gltf.Extensions = new VrmProtobuf.Extensions();
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
                var humanoidBone = new VrmProtobuf.Humanoid.Types.humanBone
                {
                    Node = nodes.IndexOfThrow(kv.Value),
                };
                Gltf.Extensions.VRMCVrm.Humanoid.HumanBones.Add(kv.Key.ToString(), humanoidBone);
            }
        }

        public void ExportVrmBlendShape(BlendShapeManager src, List<MeshGroup> meshes, List<Material> materials)
        {
            Gltf.Extensions.VRMCVrm.BlendShape = src.ToGltf(meshes, materials);
        }

        public void ExportVrmSpringBone(SpringBoneManager springBone, List<Node> nodes)
        {
            Gltf.Extensions.VRMCVrm.SpringBone = springBone.ToGltf(nodes);
        }

        public void ExportVrmFirstPersonAndLookAt(FirstPerson firstPerson, LookAt lookat, List<MeshGroup> meshes, List<Node> nodes)
        {
            Gltf.Extensions.VRMCVrm.FirstPerson = firstPerson.ToGltf(nodes, meshes);
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
