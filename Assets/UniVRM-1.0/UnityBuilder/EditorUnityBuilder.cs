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


        public ModelAsset ToUnityAsset(VrmLib.Model model, string assetPath, IExternalUnityObject scriptedImporter)
        {
            var modelAsset = new ModelAsset();
            CreateTextureAsset(model, modelAsset, scriptedImporter);
            CreateMaterialAsset(model, modelAsset, scriptedImporter);
            RuntimeUnityBuilder.CreateMeshAsset(model, modelAsset, Meshes);

            // node
            RuntimeUnityBuilder.CreateNodes(model.Root, null, Nodes);
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
            if(boneMap.Any())
            {
                modelAsset.HumanoidAvatar = HumanoidLoader.LoadHumanoidAvatar(modelAsset.Root.transform, boneMap);
                if (modelAsset.HumanoidAvatar != null)
                {
                    modelAsset.HumanoidAvatar.name = "VRM";
                }
            }

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

        private void CreateTextureAsset(VrmLib.Model model, ModelAsset modelAsset, IExternalUnityObject scriptedImporter)
        {
            var externalObjects = scriptedImporter.GetExternalUnityObjects<Texture2D>();

            // textures
            for (int i = 0; i < model.Textures.Count; ++i)
            {
                if (model.Textures[i] is VrmLib.ImageTexture imageTexture)
                {  
                    if(string.IsNullOrEmpty(model.Textures[i].Name))
                    {
                        model.Textures[i].Name = string.Format("{0}_img{1}", model.Name, i);
                    }
                    if (externalObjects.ContainsKey(model.Textures[i].Name))
                    {
                        Textures.Add(imageTexture, externalObjects[model.Textures[i].Name]);
                        modelAsset.Textures.Add(externalObjects[model.Textures[i].Name]);
                    }
                    else
                    {
                        var name = !string.IsNullOrEmpty(imageTexture.Name)
                            ? imageTexture.Name
                            : string.Format("{0}_img{1}", model.Root.Name, i);

                        var texture = RuntimeUnityBuilder.CreateTexture(name, imageTexture);

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


        private void CreateMaterialAsset(VrmLib.Model model, ModelAsset modelAsset, IExternalUnityObject scriptedImporter)
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
                    var material = RuntimeUnityMaterialBuilder.CreateMaterialAsset(src, hasVertexColor: false, Textures);
                    material.name = src.Name;
                    Materials.Add(src, material);
                    modelAsset.Materials.Add(material);
                }
            }
        }
    }
}
