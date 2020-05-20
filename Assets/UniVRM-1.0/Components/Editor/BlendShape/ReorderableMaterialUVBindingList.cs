using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


namespace UniVRM10
{
    public class ReorderableMaterialUVBindingList
    {
        const int ItemHeight = 90;
        ReorderableList m_list;
        SerializedProperty m_serializedProperty;
        bool m_changed;

        public ReorderableMaterialUVBindingList(SerializedObject serializedObject, PreviewSceneManager previewSceneManager)
        {
            m_serializedProperty = serializedObject.FindProperty(nameof(BlendShapeClip.MaterialUVBindings));
            m_list = new ReorderableList(serializedObject, m_serializedProperty);
            m_list.elementHeight = ItemHeight;
            m_list.drawElementCallback =
              (rect, index, isActive, isFocused) =>
              {
                  var element = m_serializedProperty.GetArrayElementAtIndex(index);
                  rect.height -= 4;
                  rect.y += 2;
                  if (BlendShapeClipEditorHelper.DrawMaterialValueBinding(rect, element, previewSceneManager))
                  {
                      m_changed = true;
                  }
              };

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
