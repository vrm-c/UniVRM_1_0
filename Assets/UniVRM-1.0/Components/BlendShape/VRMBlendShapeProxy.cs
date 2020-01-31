using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace UniVRM10
{
    [DisallowMultipleComponent]
    public class VRMBlendShapeProxy : MonoBehaviour
    {
        public enum UpdateTypes
        {
            None,
            Update,
            LateUpdate,
        }

        [SerializeField, Header("UpdateSetting")]
        private UpdateTypes m_updateType = UpdateTypes.Update;
        public UpdateTypes UpdateType => m_updateType;

        [SerializeField, ReadOnlyAttribute, Header("IgnoreStatus")]
        public bool m_ignoreBlink;
        [SerializeField, ReadOnlyAttribute]
        public bool m_ignoreLookAt;
        [SerializeField, ReadOnlyAttribute]
        public bool m_ignoreMouth;

        public bool IgnoreBlink => m_ignoreBlink;
        public bool IgnoreLookAt => m_ignoreLookAt;
        public bool IgnoreMouth => m_ignoreMouth;

        [SerializeField, Header("LookAt")]
        private VRMLookAtHead LookAtHead;

        [SerializeField, Header("BlendShape")]
        public BlendShapeAvatar BlendShapeAvatar;

        public Dictionary<BlendShapeKey, float> BlendShapeKeyWeights = new Dictionary<BlendShapeKey, float>();

        List<BlendShapeKey> m_blinkBlendShapeKeys = new List<BlendShapeKey>();
        List<BlendShapeKey> m_lookAtBlendShapeKeys = new List<BlendShapeKey>();
        List<BlendShapeKey> m_mouthBlendShapeKeys = new List<BlendShapeKey>();

        BlendShapeMerger m_merger;


        private void OnDestroy()
        {
            if (m_merger != null)
            {
                m_merger.RestoreMaterialInitialValues(BlendShapeAvatar.Clips);
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

            var animator = GetComponent<Animator>();
            if(animator != null)
            {
                LookAtHead = new VRMLookAtHead(animator);
                var lookAtApplier = GetComponent<ILookAtApplier>();
                if (lookAtApplier != null)
                {
                    LookAtHead.YawPitchChanged += (float y, float p) => { lookAtApplier.ApplyRotations(this, y, p); };
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
            foreach(var keyValue in values)
            {
                if(BlendShapeKeyWeights.ContainsKey(keyValue.Key))
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
        /// Apply blendShape values that use SetValue apply=false
        /// </summary>
        public void Apply()
        {
            var validateState = GetValidateState();
            m_ignoreBlink = validateState.ignoreBlink;
            m_ignoreLookAt = validateState.ignoreLookAt;
            m_ignoreMouth = validateState.ignoreMouth;
            if (validateState.ignoreBlink)
            {
                m_blinkBlendShapeKeys.ForEach(x => BlendShapeKeyWeights[x] = 0.0f);
            }

            if(validateState.ignoreLookAt)
            {
                LookAtHead.RaiseYawPitchChanged(0.0f, 0.0f);
            }
            else
            {
                LookAtHead.Update();
            }

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
            if(m_updateType == UpdateTypes.Update)
            {
                Apply();
            }
        }

        private void LateUpdate()
        {
            if (m_updateType == UpdateTypes.LateUpdate)
            {
                Apply();
            }
        }
    }

    public static class VRMBlendShapeProxyExtensions
    {
        public static float GetValue(this VRMBlendShapeProxy proxy, VrmLib.BlendShapePreset key)
        {
            var blendShapeKey = new BlendShapeKey(key);
            if (proxy.BlendShapeKeyWeights.ContainsKey(blendShapeKey))
            {
                return proxy.BlendShapeKeyWeights[blendShapeKey];
            }
            else
            {
                return 0.0f;
            }
        }

        public static float GetValue(this VRMBlendShapeProxy proxy, String key)
        {
            var blendShapeKey = new BlendShapeKey(key);
            if (proxy.BlendShapeKeyWeights.ContainsKey(blendShapeKey))
            {
                return proxy.BlendShapeKeyWeights[blendShapeKey];
            }
            else
            {
                return 0.0f;
            }
        }

        public static void SetValue(this VRMBlendShapeProxy proxy, VrmLib.BlendShapePreset key, float value)
        {
            var blendShapeKey = new BlendShapeKey(key);
            if (proxy.BlendShapeKeyWeights.ContainsKey(blendShapeKey))
            {
                proxy.BlendShapeKeyWeights[blendShapeKey] = value;
            }
        }

        public static void SetValue(this VRMBlendShapeProxy proxy, String key, float value)
        {
            var blendShapeKey = new BlendShapeKey(key);
            if (proxy.BlendShapeKeyWeights.ContainsKey(blendShapeKey))
            {
                proxy.BlendShapeKeyWeights[blendShapeKey] = value;
            }
        }
    }
}
