using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;


namespace UniVRM10
{
    [CustomEditor(typeof(VRMController))]
    public class VRMControllerEditor : Editor
    {
        VRMController m_target;
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
            m_target = (VRMController)target;
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

        void OnSceneGUI()
        {
            OnSceneGUIOffset();
            if (!Application.isPlaying)
            {
                // offset
                var p = m_target.OffsetFromHead;
                Handles.Label(m_target.Head.position, $"fromHead: [{p.x:0.00}, {p.y:0.00}, {p.z:0.00}]");
            }
            else
            {
                OnSceneGUILookAt();
            }
        }

        void OnSceneGUIOffset()
        {
            var component = target as VRMController;
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
            Handles.SphereCap(0, head.position, Quaternion.identity, 0.01f);
            Handles.SphereCap(0, worldOffset, Quaternion.identity, 0.01f);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(component, "Changed FirstPerson");

                component.OffsetFromHead = head.worldToLocalMatrix.MultiplyPoint(worldOffset);
            }
        }

        const float RADIUS = 0.5f;

        void OnSceneGUILookAt()
        {
            if (m_target.Head == null) return;
            if (!m_target.DrawGizmo) return;

            if (m_target.Gaze != null)
            {
                {
                    EditorGUI.BeginChangeCheck();
                    var newTargetPosition = Handles.PositionHandle(m_target.Gaze.position, Quaternion.identity);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(m_target.Gaze, "Change Look At Target Position");
                        m_target.Gaze.position = newTargetPosition;
                    }
                }

                Handles.color = new Color(1, 1, 1, 0.6f);
                Handles.DrawDottedLine(m_target.LookAtOrigin.position, m_target.Gaze.position, 4.0f);
            }

            var (yaw, pitch) = m_target.GetLookAtYawPitch();
            var lookAtOriginMatrix = m_target.LookAtOrigin.localToWorldMatrix;
            Handles.matrix = lookAtOriginMatrix;
            var p = m_target.OffsetFromHead;
            Handles.Label(Vector3.zero,
            $"FromHead: [{p.x:0.00}, {p.y:0.00}, {p.z:0.00}]\nYaw: {yaw:0.}degree\nPitch: {pitch:0.}degree");

            Handles.color = new Color(0, 1, 0, 0.2f);
            Handles.DrawSolidArc(Vector3.zero,
                    Matrix4x4.identity.GetColumn(1),
                    Matrix4x4.identity.GetColumn(2),
                    yaw,
                    RADIUS);


            var yawQ = Quaternion.AngleAxis(yaw, Vector3.up);
            var yawMatrix = default(Matrix4x4);
            yawMatrix.SetTRS(Vector3.zero, yawQ, Vector3.one);

            Handles.matrix = lookAtOriginMatrix * yawMatrix;
            Handles.color = new Color(1, 0, 0, 0.2f);
            Handles.DrawSolidArc(Vector3.zero,
                    Matrix4x4.identity.GetColumn(0),
                    Matrix4x4.identity.GetColumn(2),
                    -pitch,
                    RADIUS);
        }
    }
}
