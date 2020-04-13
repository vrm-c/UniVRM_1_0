using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;


namespace UniVRM10
{
    [CustomEditor(typeof(VRMBlendShapeProxy))]
    public class VRMBlendShapeProxyEditor : Editor
    {
        VRMBlendShapeProxy m_target;
        SkinnedMeshRenderer[] m_renderers;
        Dictionary<BlendShapeKey, float> m_blendShapeKeyWeights = new Dictionary<BlendShapeKey, float>();

        public class BlendShapeSlider
        {
            Dictionary<BlendShapeKey, float> m_blendShapeKeys;
            BlendShapeKey m_key;

            public BlendShapeSlider(Dictionary<BlendShapeKey, float> blendShapeKeys, BlendShapeKey key)
            {
                m_blendShapeKeys = blendShapeKeys;
                m_key = key;
            }

            public KeyValuePair<BlendShapeKey, float> Slider()
            {
                var oldValue = m_blendShapeKeys[m_key];
                var enable = GUI.enabled;
                GUI.enabled = Application.isPlaying;
                var newValue = EditorGUILayout.Slider(m_key.ToString(), oldValue, 0, 1.0f);
                GUI.enabled = enable;
                return new KeyValuePair<BlendShapeKey, float>(m_key, newValue);
            }
        }
        List<BlendShapeSlider> m_sliders;

        void OnEnable()
        {
            m_target = (VRMBlendShapeProxy)target;
            if (m_target.BlendShapeAvatar != null && m_target.BlendShapeAvatar.Clips != null)
            {
                m_blendShapeKeyWeights = m_target.BlendShapeAvatar.Clips.ToDictionary(x => BlendShapeKey.CreateFromClip(x), x => 0.0f);
                m_sliders = m_target.BlendShapeAvatar.Clips
                    .Where(x => x != null)
                    .Select(x => new BlendShapeSlider(m_blendShapeKeyWeights, BlendShapeKey.CreateFromClip(x)))
                    .ToList()
                    ;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("IgnoreStatus", EditorStyles.boldLabel);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Toggle("Ignore Blink", m_target.IgnoreBlink);
            EditorGUILayout.Toggle("Ignore Look At", m_target.IgnoreLookAt);
            EditorGUILayout.Toggle("Ignore Mouth", m_target.IgnoreMouth);
            EditorGUI.EndDisabledGroup();

            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Enable when playing", MessageType.Info);
            }

            if (m_target.BlendShapeAvatar == null)
            {
                return;
            }

            if (m_sliders != null)
            {
                var sliders = m_sliders.Select(x => x.Slider());
                foreach (var slider in sliders)
                {
                    m_blendShapeKeyWeights[slider.Key] = slider.Value;
                }
                m_target.SetValues(m_blendShapeKeyWeights.Select(x => new KeyValuePair<BlendShapeKey, float>(x.Key, x.Value)));
            }
        }

        const string GUI_LABEL = "OffsetFromHead";
        void OnSceneGUI()
        {
            var component = target as VRMBlendShapeProxy;
            if (!component.DrawGizmo)
            {
                return;
            }

            var head = component.Head;
            if (head == null)
            {
                return;
            }

            EditorGUI.BeginChangeCheck();

            var worldOffset = head.localToWorldMatrix.MultiplyPoint(component.OffsetFromHead);
            worldOffset = Handles.PositionHandle(worldOffset, head.rotation);

            Handles.DrawDottedLine(head.position, worldOffset, 5);

            Handles.Label(worldOffset, GUI_LABEL);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(component, "Changed FirstPerson");

                component.OffsetFromHead = head.worldToLocalMatrix.MultiplyPoint(worldOffset);
            }
        }
    }
}
