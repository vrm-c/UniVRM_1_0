using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;
using System;
using System.IO;
using VrmLib;

namespace UniVRM10
{
    public class GltfEditorConverter : EditorWindow
    {
        const string EXTENSION = ".glb";
        ObjectField m_gameObjectField;
        Button m_exportButton;

        [MenuItem("VRM/UniVRM-" + UniVRM10.VRMVersion.VERSION + "/GltfEditorExporter")]
        [MenuItem("GameObject/UniVRM-" + UniVRM10.VRMVersion.VERSION + "/Export Glb", false, 20)]
        public static void ShowEditorWindow()
        {
            GltfEditorConverter wnd = GetWindow<GltfEditorConverter>();
            wnd.titleContent = new GUIContent("GltfEditorExporter");
        }

        void OnSelectionChanged()
        {
            //Debug.Log($"active: {Selection.activeObject}, selected: [{string.Join(", ", Selection.objects.Select(x => x.name))}]");
            var go = Selection.activeObject as GameObject;
            if (m_gameObjectField != null)
            {
                m_gameObjectField.value = go;
            }
        }

        void OnInspectorUpdate()
        {
            if (m_gameObjectField != null)
            {
                Validate(m_gameObjectField.value as GameObject);
            }
        }

        private void Validate(GameObject gameObject)
        {
            if(gameObject == null)
            {
                m_exportButton.SetEnabled(false);
            }
            else
            {
                m_exportButton.SetEnabled(true);
            }
        }

        public void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UniVRM-1.0/VRMConverter/Editor/GltfEditorConverter.uxml");
            VisualElement labelFromUXML = visualTree.CloneTree();
            root.Add(labelFromUXML);

            // RootNode
            m_gameObjectField = root.Query<ObjectField>(name: "RootNode").First();
            m_gameObjectField.objectType = typeof(GameObject);
            m_gameObjectField.SetEnabled(false);

            // ExportButton
            m_exportButton = root.Query<Button>(name: "ExportButton").First();
            m_exportButton.clicked += OnExportClicked;

            OnSelectionChanged();
        }

        private void OnExportClicked()
        {
            var rootObject = m_gameObjectField.value as GameObject;

            // save dialog
            var path = EditorUtility.SaveFilePanel(
                    "Save glb",
                    null,
                    rootObject.name + EXTENSION,
                    EXTENSION.Substring(1));
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            try
            {
                var exporter = new UniVRM10.RuntimeVrmConverter();
                var model = exporter.ToGlbModel(rootObject);

                // 右手系に変換
                model.ConvertCoordinate(VrmLib.Coordinates.Gltf, ignoreVrm: false);
                var exportedBytes = GetGlb(model);

                File.WriteAllBytes(path, exportedBytes);
                Debug.Log("exportedBytes: " + exportedBytes.Length);

                var assetPath = ToAssetPath(path);
                if (!string.IsNullOrEmpty(assetPath))
                {
                    AssetDatabase.ImportAsset(assetPath);
                }
            }
            catch (Exception ex)
            {
                // rethrow
                throw;
            }
        }

        static string ToAssetPath(string path)
        {
            var assetPath = UnityPath.FromFullpath(path);
            return assetPath.Value;
        }

        static byte[] GetGlb(VrmLib.Model model)
        {
            // export vrm-1.0
            var exporter = new Vrm10.Vrm10Exporter();
            var option = new VrmLib.ExportArgs
            {
                vrm = false
            };
            var glbBytes10 = exporter.Export(model, option);
            var glb10 = VrmLib.Glb.Parse(glbBytes10);
            return glb10.ToBytes();
        }
    }
}
