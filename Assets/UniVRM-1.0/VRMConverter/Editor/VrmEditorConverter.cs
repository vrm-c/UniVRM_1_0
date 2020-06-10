using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using VrmLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace UniVRM10
{

    public class VrmEditorConverter : EditorWindow
    {
        const string EXTENSION = ".vrm";

        const string AssetPath = "VRMConverter/Editor";

        static VisualTreeAsset Uxml
        {
            get
            {
                var uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"Assets/UniVRM-1.0/{AssetPath}/VrmEditorConverter.uxml");
                if (uxml)
                {
                    return uxml;
                }
                return AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"Packages/com.vrmc.univrm/{AssetPath}/VrmEditorConverter.uxml");
            }
        }

        static StyleSheet StyleSheet
        {
            get
            {
                var stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>($"Assets/UniVRM-1.0/{AssetPath}/VrmEditorConverter.uss");
                if (stylesheet)
                {
                    return stylesheet;
                }
                return AssetDatabase.LoadAssetAtPath<StyleSheet>($"Packages/com.vrmc.univrm/{AssetPath}/VrmEditorConverter.uss");
            }
        }

        #region UXML項目のバインディング
        ObjectField m_gameObjectField;
        Label m_validationLabel;
        Button m_exportButton;
        Label m_logLabel;
        #endregion

        #region UIメッセージ
        const string MSG_NO_GAMEOBJECT = "select export target.";
        const string MSG_INVALID_HUMANOID = "humanoid is not valid";
        const string MSG_NO_ANIMATOR = "target has no Animator component.";
        const string MSG_NO_AVATAR = "Animator.avatar is null.";
        const string MSG_INVALID_AVATAR = "Animator.avatar is not valid.";
        const string MSG_AVATAR_IS_NOT_HUMAN = "Animator.avatar is not humanoid. Please change model's AnimationType to humanoid on Rig tab of FBX importer.";
        const string MSG_NO_META = "no Vrm Meta. export first time.";

        const string MSG_EXPORT_OK = "Ready to VRM export.";
        const string MSG_AVATAR_HAS_JAW = "Animator.avatar has jaw bone.";
        const string MSG_REQUIRE_NORMALIZE = "Hierarchy has rotation/scaling. exporter bake hierarchy.";

        const string MSG_ROOT_ROTATION = "Root rotation must zero";

        const string MSG_ROOT_SCALING = "Root scaling must 1";
        #endregion

        [MenuItem("VRM/UniVRM-" + UniVRM10.VRMVersion.VERSION + "/VRMEditorExporter")]
        [MenuItem("GameObject/UniVRM-" + UniVRM10.VRMVersion.VERSION + "/Export VRM", false, 20)]
        public static void ShowEditorWindow()
        {
            VrmEditorConverter wnd = GetWindow<VrmEditorConverter>();
            wnd.titleContent = new GUIContent("VRMEditorExporter");
        }

        class VRMMetaObjectManager
        {
            VRMMeta m_target;

            // 新規作成用の一時的なメタ。Editorが所有する
            VRMMetaObject m_tmpMeta;
            VRMMetaObject TmpMeta
            {
                get
                {
                    if (m_tmpMeta is null)
                    {
                        m_tmpMeta = ScriptableObject.CreateInstance<VRMMetaObject>();
                    }
                    return m_tmpMeta;
                }
            }

            Editor m_metaEditor;

            public VRMMetaObject SetActiveGameObject(GameObject go)
            {
                var target = go?.GetComponent<VRMMeta>();
                if (m_target != target)
                {
                    m_target = target;
                    m_metaEditor = null;
                }
                return GetVRMMetaObject();
            }

            Editor GetOrCreateEditor(VRMMetaObject target)
            {
                if (m_metaEditor is null)
                {
                    // cache
                    //Debug.Log($"CreateEditor: {target}");
                    m_metaEditor = Editor.CreateEditor(target);
                }
                return m_metaEditor;
            }

            public VRMMetaObject GetVRMMetaObject()
            {
                if (m_target is null)
                {
                    // VRMでない。初期化用の一時的なmetaを使う
                    // Debug.Log("TmpMeta");
                    return TmpMeta;
                }
                else
                {
                    // Debug.Log("m_target");
                    return m_target.Meta;
                }
            }

            bool m_enabled = true;
            public void SetEnabled(bool enabled)
            {
                m_enabled = enabled;
            }

            void DrawMeta()
            {
                if (!m_enabled)
                {
                    return;
                }
                GetOrCreateEditor(GetVRMMetaObject())?.DrawDefaultInspector();
            }

            public VisualElement CreateVisualElement()
            {
                var element = new VisualElement();
                element.Add(new IMGUIContainer(() => DrawMeta()));
                return element;
            }
        }
        VRMMetaObjectManager m_meta = new VRMMetaObjectManager();

        class ValidationManager
        {
            List<string> m_validations = new List<string>();

            public void Clear()
            {
                m_validations.Clear();
            }

            public void Push(string msg)
            {
                m_validations.Add(msg);
            }

            public override string ToString()
            {
                return string.Join("\n", m_validations) + "\n";
            }
        }
        ValidationManager m_validation = new ValidationManager();

        void OnInspectorUpdate()
        {
            if (m_gameObjectField != null)
            {
                Validate(m_gameObjectField.value as GameObject);
            }
        }

        void OnSelectionChanged()
        {
            //Debug.Log($"active: {Selection.activeObject}, selected: [{string.Join(", ", Selection.objects.Select(x => x.name))}]");
            var go = Selection.activeObject as GameObject;
            if (m_gameObjectField != null)
            {
                m_gameObjectField.value = go;
            }
            m_meta.SetActiveGameObject(go);

            Validate(go);
        }

        void Validate(GameObject go)
        {
            // Debug.Log($"size = {position.size}");
            if (m_validationLabel != null)
            {
                m_validationLabel.text = "";
            }
            if (m_logLabel != null)
            {
                m_logLabel.text = "";
            }

            m_validation.Clear();
            var isValid = ValidateRoot(go);

            // metaのvalidation
            foreach (var msg in m_meta.GetVRMMetaObject().Validate())
            {
                isValid = false;
                m_validation.Push($"Meta: {msg}");
            }

            m_meta.SetEnabled(!(go is null));
            m_exportButton.SetEnabled(isValid);

            if (isValid)
            {
                m_validation.Push(MSG_EXPORT_OK);
            }
            m_validationLabel.text = m_validation.ToString();
        }

        bool ValidateRoot(GameObject root)
        {
            m_validation.Clear();
            if (root is null)
            {
                m_validation.Push(MSG_NO_GAMEOBJECT);
                return false;
            }

            var humanoid = root.GetComponent<VrmHumanoid>();
            if (humanoid != null)
            {
                var isError = false;
                foreach (var validation in humanoid.Validate())
                {
                    if (validation.IsError)
                    {
                        isError = true;
                    }
                    m_validation.Push(validation.Message);
                }
                if (isError)
                {
                    return false;
                }
            }
            else
            {
                var animator = root.GetComponent<Animator>();
                if (animator == null)
                {
                    m_validation.Push(MSG_NO_ANIMATOR);
                    return false;
                }

                if (animator.avatar == null)
                {
                    m_validation.Push(MSG_NO_AVATAR);
                    return false;
                }

                if (!animator.avatar.isValid)
                {
                    m_validation.Push(MSG_INVALID_AVATAR);
                    return false;
                }

                if (!animator.avatar.isHuman)
                {
                    m_validation.Push(MSG_AVATAR_IS_NOT_HUMAN);
                    return false;
                }

                if (animator.GetBoneTransform(HumanBodyBones.Jaw) != null)
                {
                    // JAWあるよ
                    m_validation.Push(MSG_AVATAR_HAS_JAW);
                }
            }

            var meta = root.GetComponent<UniVRM10.VRMMeta>();
            if (meta == null)
            {
                m_validation.Push(MSG_NO_META);
            }

            if (HasRotationOrScale(root.transform))
            {
                // 回転・スケールが含まれているので正規化するよ
                m_validation.Push(MSG_REQUIRE_NORMALIZE);
            }

            if (root.transform.rotation != Quaternion.identity)
            {
                m_validation.Push(MSG_ROOT_ROTATION);
                return false;
            }
            if (root.transform.lossyScale != Vector3.one)
            {
                m_validation.Push(MSG_ROOT_SCALING);
                return false;
            }

            return true;
        }

        static bool HasRotationOrScale(Transform t)
        {
            if (t.localRotation != Quaternion.identity)
            {
                return true;
            }
            if (t.localScale != Vector3.one)
            {
                return true;
            }

            foreach (Transform child in t)
            {
                if (HasRotationOrScale(child))
                {
                    return true;
                }
            }

            return false;
        }

        void OnDisable()
        {
            Selection.selectionChanged -= OnSelectionChanged;
            // Debug.Log("disable");
        }

        void OnExportClicked()
        {
            m_logLabel.text = "";
            var rootObject = m_gameObjectField.value as GameObject;
            if (!ValidateRoot(rootObject))
            {
                m_logLabel.text += "export aborted";
                return;
            }

            // save dialog
            var path = EditorUtility.SaveFilePanel(
                    "Save vrm",
                    null,
                    rootObject.name + EXTENSION,
                    EXTENSION.Substring(1));
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            m_logLabel.text += $"export...\n";

            try
            {
                var exporter = new UniVRM10.RuntimeVrmConverter();
                var meta = m_meta.GetVRMMetaObject();
                var model = exporter.ToModelFrom10(rootObject, meta);

                if (HasRotationOrScale(rootObject.transform))
                {
                    // 正規化
                    m_logLabel.text += $"normalize...\n";
                    var modifier = new ModelModifier(model);
                    modifier.SkinningBake();
                }

                // 右手系に変換
                m_logLabel.text += $"convert to right handed coordinate...\n";
                model.ConvertCoordinate(VrmLib.Coordinates.Gltf, ignoreVrm: false);
                var exportedBytes = GetGlb(model);

                m_logLabel.text += $"write to {path}...\n";
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
                m_logLabel.text += ex.ToString();
                // rethrow
                throw;
            }
        }

        static byte[] GetGlb(VrmLib.Model model)
        {
            // export vrm-1.0
            var exporter10 = new Vrm10.Vrm10Exporter();
            var option = new VrmLib.ExportArgs
            {
                // vrm = false
            };
            var glbBytes10 = exporter10.Export(model, option);
            var glb10 = VrmLib.Glb.Parse(glbBytes10);
            return glb10.ToBytes();
        }

        static string ToAssetPath(string path)
        {
            var assetPath = UnityPath.FromFullpath(path);
            return assetPath.Value;
        }

        public void OnEnable()
        {
            minSize = new Vector2(100, 100);
            maxSize = new Vector2(4000, 4000);

            Debug.Log("enable");
            Selection.selectionChanged += OnSelectionChanged;

            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = Uxml;
            VisualElement labelFromUXML = visualTree.CloneTree();
            root.Add(labelFromUXML);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            root.styleSheets.Add(StyleSheet);

            // RootNode
            m_gameObjectField = root.Query<ObjectField>(name: "RootNode").First();
            m_gameObjectField.objectType = typeof(GameObject);
            m_gameObjectField.SetEnabled(false);

            m_validationLabel = root.Query<Label>(name: "Validation").First();
            m_logLabel = root.Query<Label>(name: "Log").First();

            // ExportButton
            m_exportButton = root.Query<Button>(name: "ExportButton").First();
            m_exportButton.clicked += OnExportClicked;

            // Meta
            var metaContainer = root.Query<VisualElement>(name: "Meta").First();
            metaContainer.Add(m_meta.CreateVisualElement());

            OnSelectionChanged();
        }
    }
}
