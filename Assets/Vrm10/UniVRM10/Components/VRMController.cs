using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace UniVRM10
{
    /// <summary>
    /// VRM全体を制御するコンポーネント。
    /// VRMBlendShapeProxy, LookAt, SpringBone などの適用を一括して行う。
    /// 各フレームのHumanoidへのモーション適用後に任意のタイミングで
    /// Applyを呼び出してください。
    /// </summary>
    [AddComponentMenu("VRM/VRMBlendShapeProxy")]
    [DisallowMultipleComponent]
    public class VRMController : MonoBehaviour
    {
        public enum UpdateTypes
        {
            None,
            Update,
            LateUpdate,
        }

        public enum LookAtTypes
        {
            // Gaze control by bone (leftEye, rightEye)
            Bone,
            // Gaze control by blend shape (lookUp, lookDown, lookLeft, lookRight)
            BlendShape,
        }

        public enum LookAtTargetTypes
        {
            CalcYawPitchToGaze,
            SetYawPitch,
        }

        [SerializeField, Header("UpdateSetting")]
        public UpdateTypes UpdateType = UpdateTypes.LateUpdate;

        bool m_ignoreBlink;
        bool m_ignoreLookAt;
        bool m_ignoreMouth;

        #region for CustomEditor
        public bool IgnoreBlink => m_ignoreBlink;
        public bool IgnoreLookAt => m_ignoreLookAt;
        public bool IgnoreMouth => m_ignoreMouth;
        #endregion

        [SerializeField, Header("BlendShape")]
        public BlendShapeAvatar BlendShapeAvatar;

        public Dictionary<BlendShapeKey, float> BlendShapeKeyWeights = new Dictionary<BlendShapeKey, float>();

        List<BlendShapeKey> m_blinkBlendShapeKeys = new List<BlendShapeKey>();
        List<BlendShapeKey> m_lookAtBlendShapeKeys = new List<BlendShapeKey>();
        List<BlendShapeKey> m_mouthBlendShapeKeys = new List<BlendShapeKey>();

        BlendShapeMerger m_merger;

        #region LookAt
        [SerializeField, Header("LookAt")]
        public bool DrawGizmo = true;

        [SerializeField]
        public Vector3 OffsetFromHead = new Vector3(0, 0.06f, 0);

        [SerializeField]
        public LookAtTypes LookAtType;

        [SerializeField]
        public CurveMapper HorizontalOuter = new CurveMapper(90.0f, 10.0f);

        [SerializeField]
        public CurveMapper HorizontalInner = new CurveMapper(90.0f, 10.0f);

        [SerializeField]
        public CurveMapper VerticalDown = new CurveMapper(90.0f, 10.0f);

        [SerializeField]
        public CurveMapper VerticalUp = new CurveMapper(90.0f, 10.0f);

        Transform m_head;
        public Transform Head
        {
            get
            {
                if (m_head == null)
                {
                    m_head = GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Head);
                }
                return m_head;
            }
        }

        [SerializeField]
        public LookAtTargetTypes LookAtTargetType;

        OffsetOnTransform m_leftEye;
        OffsetOnTransform m_rightEye;

        #region LookAtTargetTypes.CalcYawPitchToGaze
        /// <summay>
        /// LookAtTargetTypes.CalcYawPitchToGaze時の注視点
        /// </summary>
        [SerializeField]
        public Transform Gaze;

        // 座標計算用のempty
        Transform m_lookAtOrigin;
        public Transform LookAtOrigin
        {
            get
            {
                if (!Application.isPlaying)
                {
                    return null;
                }
                if (m_lookAtOrigin == null)
                {
                    m_lookAtOrigin = new GameObject("_lookat_origin_").transform;
                    m_lookAtOrigin.SetParent(Head);
                }
                return m_lookAtOrigin;
            }
        }

        /// <summary>
        /// Headローカルの注視点からYaw, Pitch角を計算する
        /// </summary>
        (float, float) CalcLookAtYawPitch(Vector3 targetWorldPosition)
        {
            LookAtOrigin.localPosition = OffsetFromHead;

            var localPosition = m_lookAtOrigin.worldToLocalMatrix.MultiplyPoint(targetWorldPosition);
            float yaw, pitch;
            Matrix4x4.identity.CalcYawPitch(localPosition, out yaw, out pitch);
            return (yaw, pitch);
        }
        #endregion

        #region LookAtTargetTypes.SetYawPitch
        float m_yaw;
        float m_pitch;

        /// <summary>
        /// LookAtTargetTypes.SetYawPitch時の視線の角度を指定する
        /// </summary>
        /// <param name="yaw">Headボーンのforwardに対するyaw角(度)</param>
        /// <param name="pitch">Headボーンのforwardに対するpitch角(度)</param>
        public void SetLookAtYawPitch(float yaw, float pitch)
        {
            m_yaw = yaw;
            m_pitch = pitch;
        }
        #endregion

        VRMConstraint[] m_constraints;
        VRMSpringBone[] m_springs;

        /// <summary>
        /// LookAtTargetType に応じた yaw, pitch を得る
        /// </summary>
        /// <returns>Headボーンのforwardに対するyaw角(度), pitch角(度)</returns>
        public (float, float) GetLookAtYawPitch()
        {
            switch (LookAtTargetType)
            {
                case LookAtTargetTypes.CalcYawPitchToGaze:
                    // Gaze(Transform)のワールド位置に対して計算する
                    return CalcLookAtYawPitch(Gaze.position);

                case LookAtTargetTypes.SetYawPitch:
                    // 事前にSetYawPitchした値を使う
                    return (m_yaw, m_pitch);
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// LeftEyeボーンとRightEyeボーンに回転を適用する
        /// </summary>
        void LookAtBone(float yaw, float pitch)
        {
            // horizontal
            float leftYaw, rightYaw;
            if (yaw < 0)
            {
                leftYaw = -HorizontalOuter.Map(-yaw);
                rightYaw = -HorizontalInner.Map(-yaw);
            }
            else
            {
                rightYaw = HorizontalOuter.Map(yaw);
                leftYaw = HorizontalInner.Map(yaw);
            }

            // vertical
            if (pitch < 0)
            {
                pitch = -VerticalDown.Map(-pitch);
            }
            else
            {
                pitch = VerticalUp.Map(pitch);
            }

            // Apply
            if (m_leftEye.Transform != null && m_rightEye.Transform != null)
            {
                // 目に値を適用する
                m_leftEye.Transform.rotation = m_leftEye.InitialWorldMatrix.ExtractRotation() * Matrix4x4.identity.YawPitchRotation(leftYaw, pitch);
                m_rightEye.Transform.rotation = m_rightEye.InitialWorldMatrix.ExtractRotation() * Matrix4x4.identity.YawPitchRotation(rightYaw, pitch);
            }
        }

        void LookAtBlendShape(float yaw, float pitch)
        {
            if (yaw < 0)
            {
                // Left
                this.SetPresetValue(VrmLib.BlendShapePreset.LookRight, 0); // clear first
                this.SetPresetValue(VrmLib.BlendShapePreset.LookLeft, Mathf.Clamp(HorizontalOuter.Map(-yaw), 0, 1.0f));
            }
            else
            {
                // Right
                this.SetPresetValue(VrmLib.BlendShapePreset.LookLeft, 0); // clear first
                this.SetPresetValue(VrmLib.BlendShapePreset.LookRight, Mathf.Clamp(HorizontalOuter.Map(yaw), 0, 1.0f));
            }

            if (pitch < 0)
            {
                // Down
                this.SetPresetValue(VrmLib.BlendShapePreset.LookUp, 0); // clear first
                this.SetPresetValue(VrmLib.BlendShapePreset.LookDown, Mathf.Clamp(VerticalDown.Map(-pitch), 0, 1.0f));
            }
            else
            {
                // Up
                this.SetPresetValue(VrmLib.BlendShapePreset.LookDown, 0); // clear first
                this.SetPresetValue(VrmLib.BlendShapePreset.LookUp, Mathf.Clamp(VerticalUp.Map(pitch), 0, 1.0f));
            }
        }
        #endregion
        #region Gizmo
        static void DrawMatrix(Matrix4x4 m, float size)
        {
            Gizmos.matrix = m;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Vector3.zero, Vector3.right * size);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Vector3.zero, Vector3.up * size);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Vector3.zero, Vector3.forward * size);
        }

        const float LOOKAT_GIZMO_SIZE = 0.5f;

        private void OnDrawGizmos()
        {
            if (DrawGizmo)
            {
                if (m_leftEye.Transform != null & m_rightEye.Transform != null)
                {
                    DrawMatrix(m_leftEye.WorldMatrix, LOOKAT_GIZMO_SIZE);
                    DrawMatrix(m_rightEye.WorldMatrix, LOOKAT_GIZMO_SIZE);
                }
            }
        }
        #endregion

        void Reset()
        {
            var animator = GetComponent<Animator>();
            m_head = animator.GetBoneTransform(HumanBodyBones.Head);
        }

        private void OnValidate()
        {
            HorizontalInner.OnValidate();
            HorizontalOuter.OnValidate();
            VerticalUp.OnValidate();
            VerticalDown.OnValidate();
        }

        private void OnDestroy()
        {
            if (m_merger != null)
            {
                m_merger.RestoreMaterialInitialValues();
            }
        }

        private void Start()
        {
            if (BlendShapeAvatar != null)
            {
                if (m_merger == null)
                {
                    m_merger = new BlendShapeMerger(BlendShapeAvatar.Clips, transform);
                }
            }

            // get lookat origin
            var animator = GetComponent<Animator>();
            if (animator != null)
            {
                m_head = animator.GetBoneTransform(HumanBodyBones.Head);
                m_leftEye = OffsetOnTransform.Create(animator.GetBoneTransform(HumanBodyBones.LeftEye));
                m_rightEye = OffsetOnTransform.Create(animator.GetBoneTransform(HumanBodyBones.RightEye));
                if (Gaze == null)
                {
                    Gaze = new GameObject().transform;
                    Gaze.name = "__LOOKAT_GAZE__";
                    Gaze.SetParent(m_head);
                    Gaze.localPosition = Vector3.forward;
                }
            }
            BlendShapeKeyWeights = m_merger.BlendShapeKeys.ToDictionary(x => x, x => 0.0f);

            m_blinkBlendShapeKeys.Add(m_merger.BlendShapeKeys.FirstOrDefault(x => x.Preset == VrmLib.BlendShapePreset.Blink));
            m_lookAtBlendShapeKeys.Add(m_merger.BlendShapeKeys.FirstOrDefault(x => x.Preset == VrmLib.BlendShapePreset.LookUp));
            m_lookAtBlendShapeKeys.Add(m_merger.BlendShapeKeys.FirstOrDefault(x => x.Preset == VrmLib.BlendShapePreset.LookDown));
            m_lookAtBlendShapeKeys.Add(m_merger.BlendShapeKeys.FirstOrDefault(x => x.Preset == VrmLib.BlendShapePreset.LookLeft));
            m_lookAtBlendShapeKeys.Add(m_merger.BlendShapeKeys.FirstOrDefault(x => x.Preset == VrmLib.BlendShapePreset.LookRight));
            m_mouthBlendShapeKeys.Add(m_merger.BlendShapeKeys.FirstOrDefault(x => x.Preset == VrmLib.BlendShapePreset.Aa));
            m_mouthBlendShapeKeys.Add(m_merger.BlendShapeKeys.FirstOrDefault(x => x.Preset == VrmLib.BlendShapePreset.Ih));
            m_mouthBlendShapeKeys.Add(m_merger.BlendShapeKeys.FirstOrDefault(x => x.Preset == VrmLib.BlendShapePreset.Ou));
            m_mouthBlendShapeKeys.Add(m_merger.BlendShapeKeys.FirstOrDefault(x => x.Preset == VrmLib.BlendShapePreset.Ee));
            m_mouthBlendShapeKeys.Add(m_merger.BlendShapeKeys.FirstOrDefault(x => x.Preset == VrmLib.BlendShapePreset.Oh));
        }

        /// <summary>
        /// SetValue
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetValue(BlendShapeKey key, float value)
        {
            if (BlendShapeKeyWeights.ContainsKey(key))
            {
                BlendShapeKeyWeights[key] = value;
            }
        }

        /// <summary>
        /// Get a blendShape value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public float GetValue(BlendShapeKey key)
        {
            if (BlendShapeKeyWeights.ContainsKey(key))
            {
                return BlendShapeKeyWeights[key];
            }
            else
            {
                return 0.0f;
            }
        }

        public IEnumerable<KeyValuePair<BlendShapeKey, float>> GetValues()
        {
            return BlendShapeKeyWeights.Select(x => new KeyValuePair<BlendShapeKey, float>(x.Key, x.Value));
        }

        /// <summary>
        /// Set blendShape values.
        /// </summary>
        /// <param name="values"></param>
        public void SetValues(IEnumerable<KeyValuePair<BlendShapeKey, float>> values)
        {
            foreach (var keyValue in values)
            {
                if (BlendShapeKeyWeights.ContainsKey(keyValue.Key))
                {
                    BlendShapeKeyWeights[keyValue.Key] = keyValue.Value;
                }
            }
        }

        public IEnumerable<BlendShapeKey> GetKeys()
        {
            return BlendShapeKeyWeights.Keys;
        }

        /// <summary>
        /// TODO: Constraint
        /// TODO: SpringBone
        /// </summary>
        public void Apply()
        {
            // 
            // constraint
            //
            if(m_constraints==null)
            {
                m_constraints = GetComponentsInChildren<VRMConstraint>();
            }
            foreach(var constraint in m_constraints)
            {
                constraint.Process();
            }

            //
            // spring
            //
            if (m_springs == null)
            {
                m_springs = GetComponentsInChildren<VRMSpringBone>();
            }
            foreach (var spring in m_springs)
            {
                spring.Process();
            }

            //
            // get ingore settings
            //
            var validateState = GetValidateState();
            m_ignoreBlink = validateState.ignoreBlink;
            m_ignoreLookAt = validateState.ignoreLookAt;
            m_ignoreMouth = validateState.ignoreMouth;

            if (validateState.ignoreBlink)
            {
                // cancel blink
                m_blinkBlendShapeKeys.ForEach(x => BlendShapeKeyWeights[x] = 0.0f);
            }

            //
            // gaze control
            //
            var (yaw, pitch) = (validateState.ignoreLookAt)
                ? (0.0f, 0.0f)
                : GetLookAtYawPitch()
                ;
            switch (LookAtType)
            {
                case LookAtTypes.Bone:
                    LookAtBone(yaw, pitch);
                    break;

                case LookAtTypes.BlendShape:
                    LookAtBlendShape(yaw, pitch);
                    break;
            }

            //
            // blendshape
            //
            if (validateState.ignoreMouth)
            {
                m_mouthBlendShapeKeys.ForEach(x => BlendShapeKeyWeights[x] = 0.0f);
            }
            m_merger.SetValues(BlendShapeKeyWeights.Select(x => new KeyValuePair<BlendShapeKey, float>(x.Key, x.Value)));
            m_merger.Apply();
        }

        private (bool ignoreBlink, bool ignoreLookAt, bool ignoreMouth) GetValidateState()
        {
            var ignoreBlink = false;
            var ignoreLookAt = false;
            var ignoreMouth = false;

            foreach (var keyWeight in BlendShapeKeyWeights)
            {
                if (keyWeight.Value <= 0.0f)
                    continue;

                // Blink
                if (!ignoreBlink && m_merger.GetClip(keyWeight.Key).IgnoreBlink && !m_blinkBlendShapeKeys.Contains(keyWeight.Key))
                {
                    ignoreBlink = true;
                }

                // LookAt
                if (!ignoreLookAt && m_merger.GetClip(keyWeight.Key).IgnoreLookAt && !m_lookAtBlendShapeKeys.Contains(keyWeight.Key))
                {
                    ignoreLookAt = true;
                }

                // Mouth
                if (!ignoreMouth && m_merger.GetClip(keyWeight.Key).IgnoreMouth && !m_mouthBlendShapeKeys.Contains(keyWeight.Key))
                {
                    ignoreMouth = true;
                }
            }

            return (ignoreBlink, ignoreLookAt, ignoreMouth);
        }

        private void Update()
        {
            if (UpdateType == UpdateTypes.Update)
            {
                Apply();
            }
        }

        private void LateUpdate()
        {
            if (UpdateType == UpdateTypes.LateUpdate)
            {
                Apply();
            }
        }

        #region Setter and Getter
        public float GetPresetValue(VrmLib.BlendShapePreset key)
        {
            var blendShapeKey = new BlendShapeKey(key);
            if (this.BlendShapeKeyWeights.ContainsKey(blendShapeKey))
            {
                return this.BlendShapeKeyWeights[blendShapeKey];
            }
            else
            {
                return 0.0f;
            }
        }

        public float GetCustomValue(String key)
        {
            var blendShapeKey = BlendShapeKey.CreateCustom(key);
            if (this.BlendShapeKeyWeights.ContainsKey(blendShapeKey))
            {
                return this.BlendShapeKeyWeights[blendShapeKey];
            }
            else
            {
                return 0.0f;
            }
        }

        public void SetPresetValue(VrmLib.BlendShapePreset key, float value)
        {
            var blendShapeKey = new BlendShapeKey(key);
            if (this.BlendShapeKeyWeights.ContainsKey(blendShapeKey))
            {
                this.BlendShapeKeyWeights[blendShapeKey] = value;
            }
        }

        /// <parameter>key</parameter>    
        public void SetCustomValue(String key, float value)
        {
            var blendShapeKey = BlendShapeKey.CreateCustom(key);
            if (this.BlendShapeKeyWeights.ContainsKey(blendShapeKey))
            {
                this.BlendShapeKeyWeights[blendShapeKey] = value;
            }
        }
        #endregion
    }
}
