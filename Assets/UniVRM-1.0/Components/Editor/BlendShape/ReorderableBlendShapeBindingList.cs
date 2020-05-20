using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


namespace UniVRM10
{
    public class ReorderableBlendShapeBindingList
    {
        ReorderableList m_ValuesList;
        SerializedProperty m_valuesProp;
        bool m_changed;

        public ReorderableBlendShapeBindingList(SerializedObject serializedObject, PreviewSceneManager previewSceneManager, int height)
        {
            m_valuesProp = serializedObject.FindProperty(nameof(BlendShapeClip.BlendShapeBindings));
            m_ValuesList = new ReorderableList(serializedObject, m_valuesProp);
            m_ValuesList.elementHeight = height * 3;
            m_ValuesList.drawElementCallback =
              (rect, index, isActive, isFocused) =>
              {
                  var element = m_valuesProp.GetArrayElementAtIndex(index);
                  rect.height -= 4;
                  rect.y += 2;
                  if (DrawBlendShapeBinding(rect, element, previewSceneManager, height))
                  {
                      m_changed = true;
                  }
              };
        }

        ///
        /// BlendShape List のElement描画
        ///
        static bool DrawBlendShapeBinding(Rect position, SerializedProperty property,
            PreviewSceneManager scene, int height)
        {
            bool changed = false;
            if (scene != null)
            {
                var y = position.y;
                var rect = new Rect(position.x, y, position.width, height);
                int pathIndex;
                if (BlendShapeClipEditorHelper.StringPopup(rect, property.FindPropertyRelative(nameof(BlendShapeBinding.RelativePath)), scene.SkinnedMeshRendererPathList, out pathIndex))
                {
                    changed = true;
                }

                y += height;
                rect = new Rect(position.x, y, position.width, height);
                int blendShapeIndex;
                if (BlendShapeClipEditorHelper.IntPopup(rect, property.FindPropertyRelative(nameof(BlendShapeBinding.Index)), scene.GetBlendShapeNames(pathIndex), out blendShapeIndex))
                {
                    changed = true;
                }

                y += height;
                rect = new Rect(position.x, y, position.width, height);
                if (BlendShapeClipEditorHelper.FloatSlider(rect, property.FindPropertyRelative(nameof(BlendShapeBinding.Weight)), 100))
                {
                    changed = true;
                }
            }
            return changed;
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
                        case nameof(BlendShapeBinding.RelativePath):
                            item.stringValue = bindings[i].RelativePath;
                            break;

                        case nameof(BlendShapeBinding.Index):
                            item.intValue = bindings[i].Index;
                            break;

                        case nameof(BlendShapeBinding.Weight):
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
            m_ValuesList.DoLayoutList();
            if (GUILayout.Button("Clear BlendShape"))
            {
                m_changed = true;
                m_valuesProp.arraySize = 0;
            }
            return m_changed;
        }
    }
}
