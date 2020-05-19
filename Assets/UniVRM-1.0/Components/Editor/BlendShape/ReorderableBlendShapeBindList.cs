using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


namespace UniVRM10
{
    public class ReorderableBlendShapeBindList
    {
        public const int BlendShapeBindingHeight = 60;
        ReorderableList m_ValuesList;
        SerializedProperty m_valuesProp;
        bool m_changed;

        public ReorderableBlendShapeBindList(SerializedObject serializedObject, PreviewSceneManager previewSceneManager)
        {
            m_valuesProp = serializedObject.FindProperty("Values");

            m_ValuesList = new ReorderableList(serializedObject, m_valuesProp);
            m_ValuesList.elementHeight = BlendShapeBindingHeight;
            m_ValuesList.drawElementCallback =
              (rect, index, isActive, isFocused) =>
              {
                  var element = m_valuesProp.GetArrayElementAtIndex(index);
                  rect.height -= 4;
                  rect.y += 2;
                  if (BlendShapeClipEditorHelper.DrawBlendShapeBinding(rect, element, previewSceneManager))
                  {
                      m_changed = true;
                  }
              };
        }

        public void SetValues(BlendShapeBinding[] bindings)
        {
            m_valuesProp.ClearArray();
            m_valuesProp.arraySize = bindings.Length;
            for (int i = 0; i < bindings.Length; ++i)
            {
                var item = m_valuesProp.GetArrayElementAtIndex(i);

                var endProperty = item.GetEndProperty();
                while (item.NextVisible(true))
                {
                    if (SerializedProperty.EqualContents(item, endProperty))
                    {
                        break;
                    }

                    switch (item.name)
                    {
                        case "RelativePath":
                            item.stringValue = bindings[i].RelativePath;
                            break;

                        case "Index":
                            item.intValue = bindings[i].Index;
                            break;

                        case "Weight":
                            item.floatValue = bindings[i].Weight;
                            break;

                        default:
                            throw new Exception();
                    }
                }
            }

        }

        public bool Draw()
        {
            m_changed = false;
            if (GUILayout.Button("Clear BlendShape"))
            {
                m_changed = true;
                m_valuesProp.arraySize = 0;
            }
            m_ValuesList.DoLayoutList();
            return true;
        }
    }
}
