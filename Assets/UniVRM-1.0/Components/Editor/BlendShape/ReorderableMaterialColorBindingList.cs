﻿using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


namespace UniVRM10
{
    public class ReorderableMaterialColorBindingList
    {
        const int MaterialValueBindingHeight = 90;
        ReorderableList m_MaterialValuesList;
        SerializedProperty m_materialsProp;
        bool m_changed;
        public ReorderableMaterialColorBindingList(SerializedObject serializedObject, PreviewSceneManager previewSceneManager)
        {
            m_materialsProp = serializedObject.FindProperty(nameof(BlendShapeClip.MaterialColorBindings));
            m_MaterialValuesList = new ReorderableList(serializedObject, m_materialsProp);
            m_MaterialValuesList.elementHeight = MaterialValueBindingHeight;
            m_MaterialValuesList.drawElementCallback =
              (rect, index, isActive, isFocused) =>
              {
                  var element = m_materialsProp.GetArrayElementAtIndex(index);
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
            if (GUILayout.Button("Clear MaterialColor"))
            {
                m_changed = true;
                m_materialsProp.arraySize = 0;
            }
            m_MaterialValuesList.DoLayoutList();
            return m_changed;
        }
    }
}
