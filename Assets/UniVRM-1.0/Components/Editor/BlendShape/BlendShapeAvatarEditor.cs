using System;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


namespace UniVRM10
{
    [CustomEditor(typeof(BlendShapeAvatar))]
    public class BlendShapeAvatarEditor : PreviewEditor
    {
        ReorderableList m_clipList;

        BlendShapeClipSelector m_selector;

        SerializedBlendShapeEditor m_serializedEditor;

        int m_clipEditorMode;

        protected override BlendShapeClip GetBakeValue()
        {
            return m_selector.Selected;
        }

        void OnSelected(BlendShapeClip clip)
        {
            if (PreviewSceneManager == null)
            {
                m_serializedEditor = null;
            }
            else if (clip != null)
            {
                m_serializedEditor = new SerializedBlendShapeEditor(clip, PreviewSceneManager, m_clipEditorMode);
            }
            else
            {
                m_serializedEditor = null;
                PreviewSceneManager.Bake(default, 1.0f);
            }
        }

        protected override void OnEnable()
        {
            m_selector = new BlendShapeClipSelector((BlendShapeAvatar)target, OnSelected);

            var prop = serializedObject.FindProperty("Clips");
            m_clipList = new ReorderableList(serializedObject, prop);

            m_clipList.drawHeaderCallback = (rect) =>
                                 EditorGUI.LabelField(rect, "BlendShapeClips");

            m_clipList.elementHeight = BlendShapeClipDrawer.Height;
            m_clipList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var element = prop.GetArrayElementAtIndex(index);
                rect.height -= 4;
                rect.y += 2;
                EditorGUI.PropertyField(rect, element);
            };

            m_clipList.onAddCallback += (list) =>
            {
                // Add slot
                prop.arraySize++;
                // select last item
                list.index = prop.arraySize - 1;
                // get last item
                var element = prop.GetArrayElementAtIndex(list.index);
                element.objectReferenceValue = null;

                var dir = Path.GetDirectoryName(AssetDatabase.GetAssetPath(target));
                var path = EditorUtility.SaveFilePanel(
                               "Create BlendShapeClip",
                               dir,
                               string.Format("BlendShapeClip#{0}.asset", list.count),
                               "asset");
                if (!string.IsNullOrEmpty(path))
                {
                    var clip = BlendShapeAvatar.CreateBlendShapeClip(path.ToUnityRelativePath());
                    //clip.Prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GetAssetPath(target));

                    element.objectReferenceValue = clip;
                }
            };

            m_clipList.onSelectCallback += (list) =>
            {
                var a = list.serializedProperty;
                var selected = a.GetArrayElementAtIndex(list.index);
                OnSelected((BlendShapeClip)selected.objectReferenceValue);
            };

            //m_clipList.onCanRemoveCallback += list => true;
            base.OnEnable();

            OnSelected(m_selector.Selected);
        }

        int m_mode;
        static readonly string[] MODES = new string[]{
            "Editor",
            "List"
        };

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();

            m_mode = GUILayout.Toolbar(m_mode, MODES);
            switch (m_mode)
            {
                case 0:
                    m_selector.SelectGUI();
                    if (m_serializedEditor != null)
                    {
                        Separator();
                        m_serializedEditor.Draw(out BlendShapeClip bakeValue);
                        PreviewSceneManager.Bake(bakeValue, 1.0f);
                    }
                    break;

                case 1:
                    m_clipList.DoLayoutList();
                    break;

                default:
                    throw new NotImplementedException();
            }

            serializedObject.ApplyModifiedProperties();
            m_clipEditorMode = m_serializedEditor.Mode;
        }
    }
}
