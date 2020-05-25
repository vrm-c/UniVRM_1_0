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
        BlendShapeClipSelector Selector
        {
            get
            {
                if (m_selector == null)
                {
                    m_selector = new BlendShapeClipSelector((BlendShapeAvatar)target, serializedObject);
                    m_selector.Selected += OnSelected;
                    OnSelected(m_selector.GetSelected());
                }
                return m_selector;
            }
        }

        /// <summary>
        /// 選択中の BlendShapeClip のエディタ
        /// </summary>
        SerializedBlendShapeEditor m_serializedEditor;

        protected override BlendShapeClip CurrentBlendShapeClip()
        {
            return Selector.GetSelected();
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
                var status = SerializedBlendShapeEditor.EditorStatus.Default;
                if (m_serializedEditor != null)
                {
                    status = m_serializedEditor.Status;
                }
                m_serializedEditor = new SerializedBlendShapeEditor(clip, PreviewSceneManager, status);
            }
            else
            {
                // clear selection
                m_serializedEditor = null;
                PreviewSceneManager.Bake(default, 1.0f);
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // selector
            Selector.GUI();

            // editor
            if (m_serializedEditor != null)
            {
                Separator();
                m_serializedEditor.Draw(out BlendShapeClip bakeValue);
                PreviewSceneManager.Bake(bakeValue, 1.0f);
            }
        }
    }
}
