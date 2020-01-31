using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniVRM10
{
    /// <summary>
    /// VrmLib.Model から UnityPrefab を構築する
    /// </summary>
    public class RuntimeUnityBuilder : IUnityBuilder
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

        // GLTF data to Unity texture
        // ConvertToNormalValueFromRawColorWhenCompressionIsRequired
        public static Material GetNormalMapConvertGltfToUnity()
        {
            return new Material(Shader.Find("UniVRM/NormalMapEncoder"));
        }

        public ModelAsset ToUnityAsset(VrmLib.Model model, string assetPath = "")
        {
            var modelAsset = new ModelAsset();
            CreateTextureAsset(model, modelAsset);
            CreateMaterialAssets(model, modelAsset);
            CreateMeshAsset(model, modelAsset);

            // node
            CreateNodes(model.Root, null, Nodes);
            modelAsset.Root = Nodes[model.Root];
            Root = modelAsset.Root;

            // renderer
            foreach (var (mesh, renderer) in CreateRendererAsset(Nodes, Meshes, Materials))
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

        static UnityEngine.Material CreateUnlitMaterial(VrmLib.UnlitMaterial src, bool hasVertexColor, Dictionary<VrmLib.Texture, Texture2D> textures)
        {
            var material = new Material(Shader.Find(UniUnlit.Utils.ShaderName));

            // texture
            if (src.BaseColorTexture != null)
            {
                material.mainTexture = textures[src.BaseColorTexture.Texture];
            }

            // color
            material.color = src.BaseColorFactor.ToUnitySRGB();

            //renderMode
            switch (src.AlphaMode)
            {
                case VrmLib.AlphaModeType.OPAQUE:
                    UniUnlit.Utils.SetRenderMode(material, UniUnlit.UniUnlitRenderMode.Opaque);
                    break;

                case VrmLib.AlphaModeType.BLEND:
                    UniUnlit.Utils.SetRenderMode(material, UniUnlit.UniUnlitRenderMode.Transparent);
                    break;

                case VrmLib.AlphaModeType.MASK:
                    UniUnlit.Utils.SetRenderMode(material, UniUnlit.UniUnlitRenderMode.Cutout);
                    break;

                default:
                    UniUnlit.Utils.SetRenderMode(material, UniUnlit.UniUnlitRenderMode.Opaque);
                    break;
            }

            // culling
            if (src.DoubleSided)
            {
                UniUnlit.Utils.SetCullMode(material, UniUnlit.UniUnlitCullMode.Off);
            }
            else
            {
                UniUnlit.Utils.SetCullMode(material, UniUnlit.UniUnlitCullMode.Back);
            }

            // VColor
            if (hasVertexColor)
            {
                UniUnlit.Utils.SetVColBlendMode(material, UniUnlit.UniUnlitVertexColorBlendOp.Multiply);
            }

            UniUnlit.Utils.ValidateProperties(material, true);

            return material;
        }

        // https://forum.unity.com/threads/standard-material-shader-ignoring-setfloat-property-_mode.344557/#post-2229980
        internal enum BlendMode
        {
            Opaque,
            Cutout,
            Fade,        // Old school alpha-blending mode, fresnel does not affect amount of transparency
            Transparent // Physically plausible transparency mode, implemented as alpha pre-multiply
        }

        static UnityEngine.Material CreateStandardMaterial(VrmLib.PBRMaterial x, Dictionary<VrmLib.Texture, Texture2D> textures)
        {
            var material = new Material(Shader.Find("Standard"));

            material.color = x.BaseColorFactor.ToUnitySRGB();

            if (x.BaseColorTexture != null)
            {
                material.mainTexture = textures[x.BaseColorTexture.Texture];
            }

            if (x.MetallicRoughnessTexture != null)
            {
                material.EnableKeyword("_METALLICGLOSSMAP");
                var texture = textures[x.MetallicRoughnessTexture];
                if (texture != null)
                {
                    var prop = "_MetallicGlossMap";
                    // TODO: Bake roughnessFactor values into a texture.
                    // material.SetTexture(prop, texture.ConvertTexture(prop, x.pbrMetallicRoughness.roughnessFactor));
                }

                material.SetFloat("_Metallic", 1.0f);
                // Set 1.0f as hard-coded. See: https://github.com/dwango/UniVRM/issues/212.
                material.SetFloat("_GlossMapScale", 1.0f);
            }
            else
            {
                material.SetFloat("_Metallic", x.MetallicFactor);
                material.SetFloat("_Glossiness", 1.0f - x.RoughnessFactor);
            }

            if (x.NormalTexture != null)
            {
                material.EnableKeyword("_NORMALMAP");
                var texture = textures[x.NormalTexture];
                if (texture != null)
                {
                    var prop = "_BumpMap";
                    // TODO
                    // material.SetTexture(prop, texture.ConvertTexture(prop));
                    material.SetFloat("_BumpScale", x.NormalTextureScale);
                }
            }

            if (x.OcclusionTexture != null)
            {
                var texture = textures[x.OcclusionTexture];
                if (texture != null)
                {
                    var prop = "_OcclusionMap";
                    // TODO
                    // material.SetTexture(prop, texture.ConvertTexture(prop));
                    material.SetFloat("_OcclusionStrength", x.OcclusionTextureStrength);
                }
            }

            if (x.EmissiveFactor != System.Numerics.Vector3.Zero || x.EmissiveTexture != null)
            {
                material.EnableKeyword("_EMISSION");
                material.globalIlluminationFlags &= ~MaterialGlobalIlluminationFlags.EmissiveIsBlack;

                material.SetColor("_EmissionColor", x.EmissiveFactor.ToUnityColor());

                if (x.EmissiveTexture != null)
                {
                    var texture = textures[x.EmissiveTexture];
                    if (texture != null)
                    {
                        material.SetTexture("_EmissionMap", texture);
                    }
                }
            }

            BlendMode blendMode = BlendMode.Opaque;
            // https://forum.unity.com/threads/standard-material-shader-ignoring-setfloat-property-_mode.344557/#post-2229980
            switch (x.AlphaMode)
            {
                case VrmLib.AlphaModeType.BLEND:
                    blendMode = BlendMode.Fade;
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;
                    break;

                case VrmLib.AlphaModeType.MASK:
                    blendMode = BlendMode.Cutout;
                    material.SetOverrideTag("RenderType", "TransparentCutout");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.SetFloat("_Cutoff", x.AlphaCutoff);
                    material.EnableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 2450;

                    break;

                default: // OPAQUE
                    blendMode = BlendMode.Opaque;
                    material.SetOverrideTag("RenderType", "");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = -1;
                    break;
            }

            material.SetFloat("_Mode", (float)blendMode);
            return material;
        }

        public static UnityEngine.Material CreateMaterialAsset(VrmLib.Material src, bool hasVertexColor, Dictionary<VrmLib.Texture, Texture2D> textures)
        {
            if (src is VrmLib.MToonMaterial mtoonSrc)
            {
                // MTOON
                var material = new Material(Shader.Find(MToon.Utils.ShaderName));
                MToon.Utils.SetMToonParametersToMaterial(material, mtoonSrc.Definition.ToUnity(textures));
                return material;
            }

            if (src is VrmLib.UnlitMaterial unlitSrc)
            {
                return CreateUnlitMaterial(unlitSrc, hasVertexColor, textures);
            }

            if (src is VrmLib.PBRMaterial pbrSrc)
            {
                return CreateStandardMaterial(pbrSrc, textures);
            }

            throw new NotImplementedException($"unknown material: {src}");
        }

        private void CreateMaterialAssets(VrmLib.Model model, ModelAsset modelAsset)
        {
            foreach (var src in model.Materials)
            {
                // TODO: material has VertexColor
                var material = CreateMaterialAsset(src, hasVertexColor: false, Textures);
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
                    var texture = new Texture2D(2, 2, TextureFormat.ARGB32, false, imageTexture.ColorSpace == VrmLib.Texture.ColorSpaceTypes.Linear);
                    texture.name = !string.IsNullOrEmpty(imageTexture.Name)
                        ? imageTexture.Name
                        : imageTexture.Image.Name
                        ;
                    texture.LoadImage(imageTexture.Image.Bytes.ToArray());
                    if(imageTexture.TextureType == VrmLib.Texture.TextureTypes.NormalMap)
                    {
                        var convertMaterial = GetNormalMapConvertGltfToUnity();
                        var dstTexture = UnityTextureUtil.CopyTexture(
                            texture, 
                            (imageTexture.ColorSpace == VrmLib.Texture.ColorSpaceTypes.Linear)? RenderTextureReadWrite.Linear:RenderTextureReadWrite.sRGB,
                            convertMaterial);
                        if(convertMaterial != null)
                        {
                            UnityEngine.Object.DestroyImmediate(convertMaterial);
                        }
                        if(texture != null)
                        {
                            UnityEngine.Object.DestroyImmediate(texture);
                        }
                        
                        texture = dstTexture;
                    }

                    Textures.Add(imageTexture, texture);
                    modelAsset.Textures.Add(texture);
                }
                else
                {
                    Debug.LogWarning($"{i} not ImageTexture");
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
