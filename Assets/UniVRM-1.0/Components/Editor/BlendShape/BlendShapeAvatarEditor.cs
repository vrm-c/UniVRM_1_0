using System;
using UnityEditor;
using UnityEngine;


namespace UniVRM10
{
    [CustomEditor(typeof(BlendShapeAvatar))]
    public class BlendShapeAvatarEditor : BlendShapeClipEditorBase
    {
        ReorderableBlendShapeClipList m_clipList;

        BlendShapeClipSelector m_selector;

        SerializedBlendShapeEditor m_serializedEditor;

        protected override BlendShapeClip GetBakeValue()
        {
            return m_selector.Selected;
        }

        void OnSelected(BlendShapeClip clip)
        {
            if (m_selector != null)
            {
                m_selector.Selected = clip;
            }
            if (PreviewSceneManager == null)
            {
                m_serializedEditor = null;
                return;
            }

            if (clip != null)
            {
                // select clip
                int mode = 0;
                if (m_serializedEditor != null)
                {
                    mode = m_serializedEditor.Mode;
                }
                m_serializedEditor = new SerializedBlendShapeEditor(clip, PreviewSceneManager, mode);
            }
            else
            {
                // clear selection
                m_serializedEditor = null;
                PreviewSceneManager.Bake(default, 1.0f);
            }
        }

        protected override void OnEnable()
        {
            m_selector = new BlendShapeClipSelector((BlendShapeAvatar)target, OnSelected);

            var prop = serializedObject.FindProperty("Clips");
            m_clipList = new ReorderableBlendShapeClipList(serializedObject, prop, target);
            m_clipList.Selected += OnSelected;

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
                    m_clipList.GUI();
                    break;

                default:
                    throw new NotImplementedException();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
