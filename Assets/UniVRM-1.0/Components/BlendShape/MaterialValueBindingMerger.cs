using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniVRM10
{
    ///
    /// Base + (A.Target - Base) * A.Weight + (B.Target - Base) * B.Weight ...
    ///
    class MaterialValueBindingMerger
    {
        public MaterialValueBindingMerger(Dictionary<BlendShapeKey, BlendShapeClip> clipMap, Transform root)
        {
        }

        public void Apply()
        {
        }

        public void AccumulateValue(BlendShapeClip clip, float value)
        {
        }

        public void RestoreMaterialInitialValues(IEnumerable<BlendShapeClip> clips)
        {
        }
    }
}
