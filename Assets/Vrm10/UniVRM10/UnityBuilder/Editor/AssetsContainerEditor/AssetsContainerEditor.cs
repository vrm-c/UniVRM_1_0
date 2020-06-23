using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Linq;
using System.Collections.Generic;
using System;

namespace UniVRM10
{
    [CustomEditor(typeof(AssetsContainer))]
    public class AssetsContainerEditor : Editor
    {
        const string AssetPath = "UnityBuilder/Editor/AssetsContainerEditor";

        static VisualTreeAsset Uxml
        {
            get
            {
                var uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"Assets/VRM10/UniVRM10/{AssetPath}/AssetsContainerEditor.uxml");
                if (uxml)
                {
                    return uxml;
                }
                return AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"Packages/com.vrmc.univrm/{AssetPath}/AssetsContainerEditor.uxml");
            }
        }

        static StyleSheet StyleSheet
        {
            get
            {
                var stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>($"Assets/VRM10/UniVRM10/{AssetPath}/AssetsContainerEditor.uss");
                if (stylesheet)
                {
                    return stylesheet;
                }
                return AssetDatabase.LoadAssetAtPath<StyleSheet>($"Packages/com.vrmc.univrm/{AssetPath}/AssetsContainerEditor.uss");
            }
        }

        Dictionary<UnityEngine.Object, Editor> m_editors = new Dictionary<UnityEngine.Object, Editor>();

        VisualElement m_rootElement;

        VisualTreeAsset m_visualTree;

        public void OnEnable()
        {
            // Hierarchy
            m_rootElement = new VisualElement();
            m_visualTree = Uxml;

            // Styles
            m_rootElement.styleSheets.Add(StyleSheet);
        }

        public void OnDisable()
        {
            foreach (var (k, v) in m_editors)
            {
                Editor.DestroyImmediate(v);
            }
            m_editors.Clear();
        }

        public override VisualElement CreateInspectorGUI()
        {
            var assetsContainer = target as AssetsContainer;
            if (assetsContainer.Assets is null)
            {
                return new Label("empty");
            }

            // Reset root element and reuse.
            var root = m_rootElement;
            root.Clear();

            BuildElements(root, assetsContainer);

            return root;
        }

        VisualElement CreateTextureVisual(int i, Texture2D asset, int height)
        {
            var horizontal = new VisualElement();
            horizontal.AddToClassList("horizontal");

            {
                var left = new VisualElement();
                left.AddToClassList("grow");
                horizontal.Add(left);
                left.Add(new Label($"[{i:00}]{asset.name}"));
                left.Add(new Label($"{asset.width} x {asset.height}"));
            }

            {
                var image = new Image();
                image.AddToClassList("image-size");
                image.image = asset;
                horizontal.Add(image);
            }

            return horizontal;
        }

        VisualElement CreateMaterialVisual(int i, Material asset, int height)
        {

            if (!m_editors.TryGetValue(asset, out Editor e))
            {
                e = Editor.CreateEditor(asset);
                m_editors.Add(asset, e);
            }

            var horizontal = new VisualElement();


            horizontal.AddToClassList("horizontal");

            {
                var left = new VisualElement();
                left.AddToClassList("grow");
                left.Add(new Label($"[{i:00}]{asset.name}"));
                left.Add(new Label(asset.shader.name));
                horizontal.Add(left);
            }
            {
                var imguiContainer = new IMGUIContainer(() =>
                {
                    e.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(height, height), null);
                });
                horizontal.Add(imguiContainer);
            }
            return horizontal;
        }

        VisualElement CreateMeshVisual(int i, Mesh asset, int height)
        {
            if (!m_editors.TryGetValue(asset, out Editor e))
            {
                e = Editor.CreateEditor(asset);
                m_editors.Add(asset, e);
            }

            var horizontal = new VisualElement();
            horizontal.AddToClassList("horizontal");

            {
                var imguiContainer = new IMGUIContainer(() =>
                {
                    e.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(height, height), null);
                });
                horizontal.Add(imguiContainer);
            }
            {
                var left = new VisualElement();
                left.AddToClassList("grow");
                left.Add(new Label($"[{i:00}]{asset.name}"));
                left.Add(new Label($"{asset.vertexCount}vertices"));
                left.Add(new Label($"{asset.triangles.Length / 3}triangles"));
                left.Add(new Label($"{asset.subMeshCount}subMeshes"));
                horizontal.Add(left);
            }
            return horizontal;

        }

        void BuildElements(VisualElement root, AssetsContainer assetsContainer)
        {
            // Turn the UXML into a VisualElement hierarchy under root.
            m_visualTree.CloneTree(root);

            // Root
            {
                var avatar = root.Query<ObjectField>(name: "RootNode").First();
                avatar.objectType = typeof(GameObject);
                avatar.value = assetsContainer.Assets.Root;
            }

            // avatar
            {
                var avatar = root.Query<ObjectField>(name: "Avatar").First();
                avatar.objectType = typeof(Avatar);
                avatar.value = assetsContainer.Assets.HumanoidAvatar;
            }

            // textures
            {
                var textureList = root.Query(name: "TextureList").First();
                var textures = assetsContainer.Assets.Textures;
                for (int i = 0; i < textures.Count; ++i)
                {
                    if (textures[i] != null) textureList.Add(CreateTextureVisual(i, textures[i], 60));
                }
            }

            // materils
            {
                var materialList = root.Query(name: "MaterialList").First();
                var materials = assetsContainer.Assets.Materials;
                for (int i = 0; i < materials.Count; ++i)
                {
                    if (materials[i] != null) materialList.Add(CreateMaterialVisual(i, materials[i], 60));
                }
            }

            // meshes
            {
                var meshList = root.Query(name: "MeshList").First();
                var meshes = assetsContainer.Assets.Meshes;
                for (int i = 0; i < meshes.Count; ++i)
                {
                    meshList.Add(CreateMeshVisual(i, meshes[i], 150));
                }
            }
        }
    }
}
