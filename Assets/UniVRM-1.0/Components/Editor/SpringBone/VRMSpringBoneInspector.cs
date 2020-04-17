using System;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace UniVRM10
{
    struct VRMSpringBoneInspector : IDisposable
    {
        SerializedObject serializedObject;
        int m_depth;

        public VRMSpringBoneInspector(SerializedObject so, int depth = 0)
        {
            m_depth = depth;
            serializedObject = so;
            serializedObject.Update();
        }

        class PropStack
        {
            StringBuilder m_sb = new StringBuilder();

            public PropStack()
            {
            }

            public void Push(SerializedProperty it, bool isCollapsed)
            {
                m_sb.Append($"{it.depth}: {it.propertyPath}({it.propertyType})");
                if (isCollapsed)
                {
                    m_sb.Append($" => skip");
                }
                m_sb.AppendLine();
            }

            public override string ToString()
            {
                return m_sb.ToString();
            }
        }

        public void OnInspectorGUI()
        {
            var stack = new PropStack();
            int currentDepth = 0;
            for (var iterator = serializedObject.GetIterator(); iterator.NextVisible(true);)
            {
                var isCollapsed = currentDepth < iterator.depth;
                stack.Push(iterator, isCollapsed);
                if (isCollapsed)
                {
                    continue;
                }

                {
                    EditorGUI.indentLevel = iterator.depth + m_depth;
                    EditorGUILayout.PropertyField(iterator, false);
                }
                // if (iterator.propertyPath.StartsWith("ColliderGroups.Array.data[")
                // && iterator.propertyType == SerializedPropertyType.ObjectReference)
                // {
                //     // custom editor
                //     var colliderSO = new SerializedObject(iterator.objectReferenceValue);
                //     // CustomEditor(iterator.depth);
                //     using (var nested = new VRMSpringBoneInspector(colliderSO, iterator.depth))
                //     {
                //         nested.OnInspectorGUI();
                //     }
                //     EditorGUILayout.Separator();
                // }

                if (iterator.isExpanded)
                {
                    currentDepth = iterator.depth + 1;
                }
                else
                {
                    currentDepth = iterator.depth;
                }
            }

            // Debug.Log(stack);
        }

        public void Dispose()
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
