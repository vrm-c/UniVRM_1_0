using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace UniVRM10
{
    /// <summary>
    /// BlendShapeAvatarEditorの部品
    /// </summary>
    class BlendShapeClipSelector
    {
        BlendShapeAvatar m_avatar;

        int m_mode;
        static readonly string[] MODES = new string[]{
            "Button",
            "List"
        };

        ReorderableBlendShapeClipList m_clipList;

        public BlendShapeClip GetSelected()
        {
            if (m_avatar == null || m_avatar.Clips == null)
            {
                return null;
            }
            if (m_selectedIndex < 0 || m_selectedIndex >= m_avatar.Clips.Count)
            {
                return null;
            }
            return m_avatar.Clips[m_selectedIndex];
        }

        public event Action<BlendShapeClip> Selected;
        void RaiseSelected(int index)
        {
            m_clipList.Select(index);
            var clip = GetSelected();
            var handle = Selected;
            if (handle == null)
            {
                return;
            }
            handle(clip);
        }

        int m_selectedIndex;
        int SelectedIndex
        {
            get { return m_selectedIndex; }
            set
            {
                // これで更新するべし
                if (m_selectedIndex == value) return;
                m_selectedIndex = value;
                RaiseSelected(value);
            }
        }

        public BlendShapeClipSelector(BlendShapeAvatar avatar, SerializedObject serializedObject)
        {
            avatar.RemoveNullClip();

            m_avatar = avatar;

            var prop = serializedObject.FindProperty("Clips");
            m_clipList = new ReorderableBlendShapeClipList(serializedObject, prop, avatar);
            m_clipList.Selected += (selected) =>
            {
                SelectedIndex = avatar.Clips.IndexOf(selected);
            };
        }

        public void GUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Select BlendShapeClip", EditorStyles.boldLabel);

            m_mode = GUILayout.Toolbar(m_mode, MODES);
            switch (m_mode)
            {
                case 0:
                    SelectGUI();
                    break;

                case 1:
                    m_clipList.GUI();
                    break;

                default:
                    throw new NotImplementedException();
            }

        }

        void SelectGUI()
        {
            if (m_avatar != null && m_avatar.Clips != null)
            {
                var array = m_avatar.Clips
                    .Select(x => x != null
                        ? BlendShapeKey.CreateFromClip(x).ToString()
                        : "null"
                        ).ToArray();
                SelectedIndex = GUILayout.SelectionGrid(SelectedIndex, array, 4);
            }

            if (GUILayout.Button("Add BlendShapeClip"))
            {
                var dir = Path.GetDirectoryName(AssetDatabase.GetAssetPath(m_avatar));
                var path = EditorUtility.SaveFilePanel(
                               "Create BlendShapeClip",
                               dir,
                               string.Format("BlendShapeClip#{0}.asset", m_avatar.Clips.Count),
                               "asset");
                if (!string.IsNullOrEmpty(path))
                {
                    var clip = BlendShapeAvatar.CreateBlendShapeClip(path.ToUnityRelativePath());
                    //clip.Prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GetAssetPath(target));

                    m_avatar.Clips.Add(clip);
                }
            }
        }

        public void DuplicateWarn()
        {
            var key = BlendShapeKey.CreateFromClip(GetSelected());
            if (m_avatar.Clips.Where(x => key.Match(x)).Count() > 1)
            {
                EditorGUILayout.HelpBox("duplicate clip: " + key, MessageType.Error);
            }
        }
    }
}
