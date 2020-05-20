using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


namespace UniVRM10
{
    public class ReorderableMaterialColorBindingList
    {
        ReorderableList m_MaterialValuesList;
        SerializedProperty m_materialsProp;
        bool m_changed;
        public ReorderableMaterialColorBindingList(SerializedObject serializedObject, PreviewSceneManager previewSceneManager, int height)
        {
            m_materialsProp = serializedObject.FindProperty(nameof(BlendShapeClip.MaterialColorBindings));
            m_MaterialValuesList = new ReorderableList(serializedObject, m_materialsProp);
            m_MaterialValuesList.elementHeight = height * 3;
            m_MaterialValuesList.drawElementCallback =
              (rect, index, isActive, isFocused) =>
              {
                  var element = m_materialsProp.GetArrayElementAtIndex(index);
                  rect.height -= 4;
                  rect.y += 2;
                  if (DrawMaterialValueBinding(rect, element, previewSceneManager, height))
                  {
                      m_changed = true;
                  }
              };
        }

        ///
        /// Material List のElement描画
        ///
        static bool DrawMaterialValueBinding(Rect position, SerializedProperty property,
            PreviewSceneManager scene, int height)
        {
            bool changed = false;
            if (scene != null)
            {
                // Material を選択する
                var y = position.y;
                var rect = new Rect(position.x, y, position.width, height);
                int materialIndex;
                if (BlendShapeClipEditorHelper.StringPopup(rect, property.FindPropertyRelative(nameof(MaterialColorBinding.MaterialName)), scene.MaterialNames, out materialIndex))
                {
                    changed = true;
                }

                // PreviewSceneの複製されたマテリアルを変更する
                if (materialIndex >= 0)
                {
                    var materialItem = scene.GetMaterialItem(scene.MaterialNames[materialIndex]);
                    if (materialItem != null)
                    {
                        y += height;
                        rect = new Rect(position.x, y, position.width, height);

                        // プロパティ名のポップアップ
                        var bindTypeProp = property.FindPropertyRelative("BindType");
                        var bindTypes = (VrmLib.MaterialBindType[])Enum.GetValues(typeof(VrmLib.MaterialBindType));
                        var bindType = bindTypes[bindTypeProp.enumValueIndex];
                        var newBindType = BlendShapeClipEditorHelper.EnumPopup(rect, bindType);
                        if (newBindType != bindType)
                        {
                            bindTypeProp.enumValueIndex = Array.IndexOf(bindTypes, newBindType);
                            changed = true;
                        }

                        y += height;
                        rect = new Rect(position.x, y, position.width, height);
                        if (BlendShapeClipEditorHelper.ColorProp(rect, property.FindPropertyRelative(nameof(MaterialColorBinding.TargetValue))))
                        {
                            changed = true;
                        }
                    }
                }
            }
            return changed;
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
