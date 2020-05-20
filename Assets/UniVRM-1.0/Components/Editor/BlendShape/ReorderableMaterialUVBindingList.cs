using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


namespace UniVRM10
{
    public class ReorderableMaterialUVBindingList
    {
        ReorderableList m_list;
        SerializedProperty m_serializedProperty;
        bool m_changed;

        public ReorderableMaterialUVBindingList(SerializedObject serializedObject, PreviewSceneManager previewSceneManager, int height)
        {
            m_serializedProperty = serializedObject.FindProperty(nameof(BlendShapeClip.MaterialUVBindings));
            m_list = new ReorderableList(serializedObject, m_serializedProperty);
            m_list.elementHeight = height * 3;
            m_list.drawElementCallback =
              (rect, index, isActive, isFocused) =>
              {
                  var element = m_serializedProperty.GetArrayElementAtIndex(index);
                  rect.height -= 4;
                  rect.y += 2;
                  if (DrawMaterialUVBinding(rect, element, previewSceneManager, height))
                  {
                      m_changed = true;
                  }
              };

        }

        ///
        /// Material List のElement描画
        ///
        static bool DrawMaterialUVBinding(Rect position, SerializedProperty property,
            PreviewSceneManager scene, int height)
        {
            bool changed = false;
            if (scene != null)
            {
                // Materialを選択する
                var y = position.y;
                var rect = new Rect(position.x, y, position.width, height);
                int materialIndex;
                if (BlendShapeClipEditorHelper.StringPopup(rect, property.FindPropertyRelative(nameof(MaterialUVBinding.MaterialName)), scene.MaterialNames, out materialIndex))
                {
                    changed = true;
                }

                // offset
                y += height;
                rect = new Rect(position.x, y, position.width, height);
                if (BlendShapeClipEditorHelper.UVProp(rect, property.FindPropertyRelative(nameof(MaterialUVBinding.Offset))))
                {
                    changed = true;
                }

                // scale
                y += height;
                rect = new Rect(position.x, y, position.width, height);
                if (BlendShapeClipEditorHelper.UVProp(rect, property.FindPropertyRelative(nameof(MaterialUVBinding.Scaling))))
                {
                    changed = true;
                }
            }
            return changed;
        }

        public bool Draw()
        {
            m_changed = false;
            if (GUILayout.Button("Clear MaterialUVBindings"))
            {
                m_changed = true;
                m_serializedProperty.arraySize = 0;
            }
            m_list.DoLayoutList();
            return m_changed;
        }
    }
}
