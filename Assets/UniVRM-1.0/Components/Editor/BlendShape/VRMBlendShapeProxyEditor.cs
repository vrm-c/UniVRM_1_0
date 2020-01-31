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
                m_blendShapeKeyWeights = m_target.BlendShapeAvatar.Clips.ToDictionary(x => BlendShapeKey.CreateFrom(x), x => 0.0f);
                m_sliders = m_target.BlendShapeAvatar.Clips
                    .Where(x => x != null)
                    .Select(x => new BlendShapeSlider(m_blendShapeKeyWeights, BlendShapeKey.CreateFrom(x)))
                    .ToList()
                    ;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

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
                foreach(var slider in sliders)
                {
                    m_blendShapeKeyWeights[slider.Key] = slider.Value;
                }
                m_target.SetValues(m_blendShapeKeyWeights.Select(x => new KeyValuePair<BlendShapeKey, float>(x.Key, x.Value)));
            }
        }
    }
}
