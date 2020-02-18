using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniVRM10
{
    /// <summary>
    /// VrmLib.Model から UnityPrefab を構築する
    /// </summary>
    public partial class RuntimeUnityBuilder : IUnityBuilder
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


        public ModelAsset ToUnityAsset(VrmLib.Model model, bool showMesh = true)
        {
            var modelAsset = new ModelAsset();
            CreateTextureAsset(model, modelAsset);
            CreateMaterialAssets(model, modelAsset);
            CreateMeshAsset(model, modelAsset, Meshes);

            // node
            CreateNodes(model.Root, null, Nodes);
            modelAsset.Root = Nodes[model.Root];
            Root = modelAsset.Root;

            // renderer
            foreach (var (mesh, renderer) in CreateRendererAsset(Nodes, Meshes, Materials))
            {
                renderer.enabled = showMesh;
                Renderers.Add(mesh, renderer);
                modelAsset.Renderers.Add(renderer);
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

        public static void CreateMeshAsset(VrmLib.Model model, ModelAsset modelAsset, Dictionary<VrmLib.MeshGroup, UnityEngine.Mesh> dstMeshes)
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
                    dstMeshes.Add(src, mesh);
                    modelAsset.Meshes.Add(mesh);
                }
                else
                {
                    // 頂点バッファの連結が必用
                    throw new NotImplementedException();
                }
            }
        }

        private void CreateMaterialAssets(VrmLib.Model model, ModelAsset modelAsset)
        {
            foreach (var src in model.Materials)
            {
                // TODO: material has VertexColor
                var material = RuntimeUnityMaterialBuilder.CreateMaterialAsset(src, hasVertexColor: false, Textures);
                material.name = src.Name;
                Materials.Add(src, material);
                modelAsset.Materials.Add(material);
            }
        }

        private void CreateTextureAsset(VrmLib.Model model, ModelAsset modelAsset)
        {
            // textures
            for (int i = 0; i < model.Textures.Count; ++i)
            {
                if (model.Textures[i] is VrmLib.ImageTexture imageTexture)
                {
                    var name = !string.IsNullOrEmpty(imageTexture.Name)
                        ? imageTexture.Name
                        : string.Format("{0}_img{1}", model.Root.Name, i);

                    var texture = CreateTexture(name, imageTexture);

                    Textures.Add(imageTexture, texture);
                    modelAsset.Textures.Add(texture);
                }
                else
                {
                    Debug.LogWarning($"{i} not ImageTexture");
                }
            }
        }

        private static RenderTextureReadWrite GetRenderTextureReadWrite(VrmLib.Texture.ColorSpaceTypes type)
        {
            return (type == VrmLib.Texture.ColorSpaceTypes.Linear) ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.sRGB;
        }

        public static Texture2D CreateTexture(string name, VrmLib.ImageTexture imageTexture)
        {
            Texture2D dstTexture = null;
            Material convertMaterial = null;
            var texture = new Texture2D(2, 2, TextureFormat.ARGB32, false, imageTexture.ColorSpace == VrmLib.Texture.ColorSpaceTypes.Linear);
            texture.name = name;
            texture.LoadImage(imageTexture.Image.Bytes.ToArray());

            // Convert Texture Gltf to Unity
            if (imageTexture.TextureType == VrmLib.Texture.TextureTypes.NormalMap)
            {
                convertMaterial = TextureConvertMaterial.GetNormalMapConvertGltfToUnity();
                dstTexture = UnityTextureUtil.CopyTexture(
                    texture,
                    GetRenderTextureReadWrite(imageTexture.ColorSpace),
                    convertMaterial);
            }
            else if (imageTexture.TextureType == VrmLib.Texture.TextureTypes.MetallicRoughness)
            {
                var metallicRoughnessImage = imageTexture as VrmLib.MetallicRoughnessImageTexture;
                convertMaterial = TextureConvertMaterial.GetMetallicRoughnessGltfToUnity(metallicRoughnessImage.RoughnessFactor);
                dstTexture = UnityTextureUtil.CopyTexture(
                    texture,
                    GetRenderTextureReadWrite(imageTexture.ColorSpace),
                    convertMaterial);
            }
            else if (imageTexture.TextureType == VrmLib.Texture.TextureTypes.Occlusion)
            {
                convertMaterial = TextureConvertMaterial.GetOcclusionGltfToUnity();
                dstTexture = UnityTextureUtil.CopyTexture(
                    texture,
                    GetRenderTextureReadWrite(imageTexture.ColorSpace),
                    convertMaterial);
            }

            if (dstTexture != null)
            {
                if (texture != null)
                {
                    UnityEngine.Object.DestroyImmediate(texture);
                }
                texture = dstTexture;
            }

            if (convertMaterial != null)
            {
                UnityEngine.Object.DestroyImmediate(convertMaterial);
            }

            return texture;
        }


        public static void CreateNodes(VrmLib.Node node, GameObject parent, Dictionary<VrmLib.Node, GameObject> nodes)
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

        public static IEnumerable<(VrmLib.MeshGroup, Renderer)> CreateRendererAsset(
            Dictionary<VrmLib.Node, GameObject> Nodes,
            Dictionary<VrmLib.MeshGroup, Mesh> Meshes,
            Dictionary<VrmLib.Material, Material> Materials)
        {
            // renderer
            foreach (var (node, go) in Nodes)
            {
                if (node.MeshGroup is null)
                {
                    continue;
                }

                if (node.MeshGroup.Meshes.Count > 1)
                {
                    throw new NotImplementedException("invalid isolated vertexbuffer");
                }
                var mesh = node.MeshGroup.Meshes[0];

                Renderer renderer = null;
                var hasBlendShape = mesh.MorphTargets.Any();
                if (node.MeshGroup.Skin != null || hasBlendShape)
                {
                    var skinnedMeshRenderer = go.AddComponent<SkinnedMeshRenderer>();
                    renderer = skinnedMeshRenderer;
                    skinnedMeshRenderer.sharedMesh = Meshes[node.MeshGroup];
                    if (node.MeshGroup.Skin != null)
                    {
                        skinnedMeshRenderer.bones = node.MeshGroup.Skin.Joints.Select(x => Nodes[x].transform).ToArray();
                        if (node.MeshGroup.Skin.Root != null)
                        {
                            skinnedMeshRenderer.rootBone = Nodes[node.MeshGroup.Skin.Root].transform;
                        }
                    }
                }
                else
                {
                    var meshFilter = go.AddComponent<MeshFilter>();
                    renderer = go.AddComponent<MeshRenderer>();
                    meshFilter.sharedMesh = Meshes[node.MeshGroup];
                }
                var materials = mesh.Submeshes.Select(x => Materials[x.Material]).ToArray();
                renderer.sharedMaterials = materials;

                yield return (node.MeshGroup, renderer);
            }
        }
    }
}
