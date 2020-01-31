using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using System.IO;
using static UnityEditor.AssetImporter;

namespace UniVRM10
{
    public class EditorUnityBuilder : IUnityBuilder
    {
        const string TextureDirName = "Textures";
        const string MaterialDirName = "Materials";
        const string MetaDirName = "Meta";
        const string BlendShapeDirName = "BlendShapes";

        private readonly Dictionary<VrmLib.Texture, Texture2D> Textures = new Dictionary<VrmLib.Texture, Texture2D>();
        private readonly Dictionary<VrmLib.Material, Material> Materials = new Dictionary<VrmLib.Material, Material>();
        private readonly Dictionary<VrmLib.MeshGroup, Mesh> Meshes = new Dictionary<VrmLib.MeshGroup, Mesh>();
        private readonly Dictionary<VrmLib.Node, GameObject> Nodes = new Dictionary<VrmLib.Node, GameObject>();
        private readonly Dictionary<VrmLib.MeshGroup, Renderer> Renderers = new Dictionary<VrmLib.MeshGroup, Renderer>();

        private GameObject Root;

        Dictionary<VrmLib.Texture, Texture2D> IUnityBuilder.Textures => Textures;
        Dictionary<VrmLib.Material, UnityEngine.Material> IUnityBuilder.Materials => Materials;
        Dictionary<VrmLib.MeshGroup, UnityEngine.Mesh> IUnityBuilder.Meshes => Meshes;
        Dictionary<VrmLib.Node, GameObject> IUnityBuilder.Nodes => Nodes;
        Dictionary<VrmLib.MeshGroup, Renderer> IUnityBuilder.Renderers => Renderers;
        GameObject IUnityBuilder.Root => Root;


        public ModelAsset ToUnityAsset(VrmLib.Model model, string assetPath, VrmScriptedImporter scriptedImporter)
        {
            var modelAsset = new ModelAsset();
            CreateTextureAsset(model, modelAsset, scriptedImporter);
            CreateMaterialAsset(model, modelAsset, scriptedImporter);
            CreateMeshAsset(model, modelAsset);

            // node
            CreateNodes(model.Root, null, Nodes);
            modelAsset.Root = Nodes[model.Root];
            Root = modelAsset.Root;

            // renderer
            foreach (var (mesh, renderer) in RuntimeUnityBuilder.CreateRendererAsset(Nodes, Meshes, Materials))
            {
                Renderers.Add(mesh, renderer);
            }

            // humanoid
            var boneMap = Nodes
            .Where(x => x.Key.HumanoidBone.GetValueOrDefault() != VrmLib.HumanoidBones.unknown)
            .Select(x => (x.Value.transform, x.Key.HumanoidBone.Value))
            .ToDictionary(x => x.transform, x => x.Item2);
            modelAsset.HumanoidAvatar = HumanoidLoader.LoadHumanoidAvatar(modelAsset.Root.transform, boneMap);
            modelAsset.HumanoidAvatar.name = "VRM";

            var animator = modelAsset.Root.AddComponent<Animator>();
            animator.avatar = modelAsset.HumanoidAvatar;

            modelAsset.Map = new ModelMap
            {
                Nodes = Nodes,
                Textures = Textures,
                Materials = Materials,
                Meshes = Meshes,
                Renderers = Renderers,
            };

            return modelAsset;
        }

        private string GetDirectoryPath(string assetPath, string dirName)
        {
            return string.Format("{0}/{1}.{2}",
                Path.GetDirectoryName(assetPath),
                Path.GetFileNameWithoutExtension(assetPath),
                dirName
                );
        }


        private void CreateTextureAsset(VrmLib.Model model, ModelAsset modelAsset, VrmScriptedImporter scriptedImporter)
        {
            var externalObjects = scriptedImporter.GetExternalUnityObjects<Texture2D>();

            // textures
            for (int i = 0; i < model.Textures.Count; ++i)
            {
                if (model.Textures[i] is VrmLib.ImageTexture imageTexture)
                {
                    if (externalObjects.ContainsKey(model.Textures[i].Name))
                    {
                        Textures.Add(imageTexture, externalObjects[model.Textures[i].Name]);
                        modelAsset.Textures.Add(externalObjects[model.Textures[i].Name]);
                    }
                    else
                    {
                        // Linear or sRGB
                        var texture = new Texture2D(2, 2, TextureFormat.ARGB32, false, imageTexture.ColorSpace == VrmLib.Texture.ColorSpaceTypes.Linear);
                        texture.name = !string.IsNullOrEmpty(imageTexture.Name)
                            ? imageTexture.Name
                            : imageTexture.Image.Name
                            ;
                        texture.LoadImage(imageTexture.Image.Bytes.ToArray());

                        // ToDo: Can texture settings of sub-assets be changed with TextureImporter?
                        if (imageTexture.TextureType == VrmLib.Texture.TextureTypes.NormalMap)
                        {
                            var convertMaterial = RuntimeUnityBuilder.GetNormalMapConvertGltfToUnity();
                            var dstTexture = UnityTextureUtil.CopyTexture(
                                texture,
                                (imageTexture.ColorSpace == VrmLib.Texture.ColorSpaceTypes.Linear) ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.sRGB,
                                convertMaterial);
                            if (convertMaterial != null)
                            {
                                UnityEngine.Object.DestroyImmediate(convertMaterial);
                            }
                            if (texture != null)
                            {
                                UnityEngine.Object.DestroyImmediate(texture);
                            }

                            texture = dstTexture;
                        }

                        Textures.Add(imageTexture, texture);
                        modelAsset.Textures.Add(texture);
                    }
                }
                else
                {
                    Debug.LogWarning($"{i} not ImageTexture");
                }
            }
        }


        private void CreateMaterialAsset(VrmLib.Model model, ModelAsset modelAsset, VrmScriptedImporter scriptedImporter)
        {
            var externalObjects = scriptedImporter.GetExternalUnityObjects<Material>();

            foreach (var src in model.Materials)
            {
                if (externalObjects.ContainsKey(src.Name))
                {
                    Materials.Add(src, externalObjects[src.Name]);
                    modelAsset.Materials.Add(externalObjects[src.Name]);
                }
                else
                {
                    // TODO: material has VertexColor
                    var material = RuntimeUnityBuilder.CreateMaterialAsset(src, hasVertexColor: false, Textures);
                    material.name = src.Name;
                    Materials.Add(src, material);
                    modelAsset.Materials.Add(material);
                }
            }
        }

        private void CreateMeshAsset(VrmLib.Model model, ModelAsset modelAsset)
        {
            // mesh
            for (int i = 0; i < model.MeshGroups.Count; ++i)
            {
                var src = model.MeshGroups[i];
                if (src.Meshes.Count == 1)
                {
                    // submesh 方式
                    var mesh = new Mesh();
                    mesh.name = src.Name;
                    mesh.LoadMesh(src.Meshes[0], src.Skin);
                    Meshes.Add(src, mesh);
                    modelAsset.Meshes.Add(mesh);
                }
                else
                {
                    // 頂点バッファの連結が必用
                    throw new NotImplementedException();
                }
            }
        }

        static void CreateNodes(VrmLib.Node node, GameObject parent, Dictionary<VrmLib.Node, GameObject> nodes)
        {
            GameObject go = new GameObject(node.Name);
            go.transform.SetPositionAndRotation(node.Translation.ToUnityVector3(), node.Rotation.ToUnityQuaternion());
            nodes.Add(node, go);
            if (parent != null)
            {
                go.transform.SetParent(parent.transform);
            }

            if (node.Children.Count > 0)
            {
                for (int n = 0; n < node.Children.Count; n++)
                {
                    CreateNodes(node.Children[n], go, nodes);
                }
            }
        }

        // private void CreateRendererAsset()
        // {
        //     // renderer
        //     foreach (var node in Nodes)
        //     {
        //         if (node.Key.MeshGroup != null)
        //         {
        //             if (node.Key.MeshGroup.Skin != null)
        //             {
        //                 var skin = node.Value.AddComponent<SkinnedMeshRenderer>();
        //                 skin.sharedMesh = Meshes[node.Key.MeshGroup];
        //                 skin.materials = node.Key.MeshGroup.Meshes[0].Submeshes.Select(x => Materials[x.Material]).ToArray();
        //                 skin.bones = node.Key.MeshGroup.Skin.Joints.Select(x => Nodes[x].transform).ToArray();
        //                 if (node.Key.MeshGroup.Skin.Root != null)
        //                 {
        //                     skin.rootBone = Nodes[node.Key.MeshGroup.Skin.Root].transform;
        //                 }
        //                 Renderers.Add(node.Key.MeshGroup, skin);
        //             }
        //             else
        //             {
        //                 var meshFilter = node.Value.AddComponent<MeshFilter>();
        //                 var meshRenderer = node.Value.AddComponent<MeshRenderer>();
        //                 meshFilter.sharedMesh = Meshes[node.Key.MeshGroup];
        //                 Renderers.Add(node.Key.MeshGroup, meshRenderer);
        //             }
        //         }
        //     }
        // }

        private static DirectoryInfo SafeCreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return null;
            }
            return Directory.CreateDirectory(path);
        }
    }
}
