using UnityEditor;

namespace UniVRM10
{
    [CustomEditor(typeof(BlendShapeAvatar))]
    public class BlendShapeAvatarEditor : BlendShapeClipEditorBase
    {
        /// <summary>
        /// BlendShapeAvatar から 編集対象の BlendShapeClip を選択する
        /// </summary>
        BlendShapeClipSelector m_selector;

        /// <summary>
        /// 選択中の BlendShapeClip のエディタ
        /// </summary>
        SerializedBlendShapeEditor m_serializedEditor;

        protected override BlendShapeClip GetBakeValue()
        {
            return m_selector.GetSelected();
        }

        void OnSelected(BlendShapeClip clip)
        {
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
            base.OnEnable();

            m_selector = new BlendShapeClipSelector((BlendShapeAvatar)target, serializedObject);
            m_selector.Selected += OnSelected;
            OnSelected(m_selector.GetSelected());
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            // selector
            m_selector.GUI();

            // editor
            if (m_serializedEditor != null)
            {
                Separator();
                m_serializedEditor.Draw(out BlendShapeClip bakeValue);
                PreviewSceneManager.Bake(bakeValue, 1.0f);
            }
            // serializedObject.ApplyModifiedProperties();
        }
    }
}
