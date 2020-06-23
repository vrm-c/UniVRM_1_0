using UnityEngine;
using System.Linq;
using System;

namespace UniVRM10
{
    [Obsolete("Use VRMBlendShapeProxy")]
    public class BlendShapeClipHandler
    {
        BlendShapeClip m_clip;
        [Obsolete("Use Clip")]
        public BlendShapeClip Cilp
        {
            get { return Clip; }
        }
        public BlendShapeClip Clip
        {
            get { return m_clip; }
        }
        SkinnedMeshRenderer[] m_renderers;

        public BlendShapeClipHandler(BlendShapeClip clip, Transform transform)
        {
            m_clip = clip;

            if (m_clip != null && m_clip.BlendShapeBindings != null && transform != null)
            {
                m_renderers = m_clip.BlendShapeBindings.Select(x =>
                {
                    var target = transform.GetFromPath(x.RelativePath);
                    return target.GetComponent<SkinnedMeshRenderer>();
                })
                 .ToArray();
            }
        }

        public float LastValue
        {
            get;
            private set;
        }

        public void Apply(float value)
        {
            LastValue = value;

            if (m_clip == null) return;
            if (m_renderers == null) return;

            for (int i = 0; i < m_clip.BlendShapeBindings.Length; ++i)
            {
                var binding = m_clip.BlendShapeBindings[i];
                var target = m_renderers[i];
                if (binding.Index >= 0 && binding.Index < target.sharedMesh.blendShapeCount)
                {
                    target.SetBlendShapeWeight(binding.Index, binding.Weight * value);
                }
            }
        }
    }
}
