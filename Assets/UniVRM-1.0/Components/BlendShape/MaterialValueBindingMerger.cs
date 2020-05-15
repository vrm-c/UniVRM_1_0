using System.Collections.Generic;
using UnityEngine;

namespace UniVRM10
{
    ///
    /// Base + (A.Target - Base) * A.Weight + (B.Target - Base) * B.Weight ...
    ///
    class MaterialValueBindingMerger
    {
        Dictionary<string, PreviewMaterialItem> m_materialMap = new Dictionary<string, PreviewMaterialItem>();

        public MaterialValueBindingMerger(Dictionary<BlendShapeKey, BlendShapeClip> clipMap, Transform root)
        {
            Dictionary<string, Material> materialNameMap = new Dictionary<string, Material>();
            foreach (var renderer in root.GetComponentsInChildren<Renderer>())
            {
                foreach (var material in renderer.sharedMaterials)
                {
                    if (!materialNameMap.ContainsKey(material.name))
                    {
                        materialNameMap.Add(material.name, material);
                    }
                }
            }

            foreach (var kv in clipMap)
            {
                foreach (var binding in kv.Value.MaterialValues)
                {
                    PreviewMaterialItem item;
                    if (!m_materialMap.TryGetValue(binding.MaterialName, out item))
                    {
                        if (!materialNameMap.TryGetValue(binding.MaterialName, out Material material))
                        {
                            // not found skip
                            continue;
                        }
                        item = new PreviewMaterialItem(material);
                        m_materialMap.Add(binding.MaterialName, item);
                    }
                    var propName = VrmLib.MaterialBindTypeExtensions.GetProperty(binding.BindType);
                    item.PropMap.Add(binding.BindType, new PropItem
                    {
                        Name = propName,
                        DefaultValues = item.Material.GetVector(propName),
                        // PropertyType = binding.BindType,
                    });
                }
            }
        }

        public void RestoreMaterialInitialValues(IEnumerable<BlendShapeClip> clips)
        {
            foreach (var kv in m_materialMap)
            {
                foreach (var prop in kv.Value.PropMap)
                {
                    kv.Value.Material.SetColor(prop.Value.Name, prop.Value.DefaultValues);
                }
            }
        }

        #region Accumulate
        struct MaterialProp
        {
            public readonly Material Material;
            public readonly VrmLib.MaterialBindType BindType;
            public readonly string Property;

            public MaterialProp(Material material, VrmLib.MaterialBindType bindType, string property)
            {
                Material = material;
                BindType = bindType;
                Property = property;
            }
        }

        Dictionary<MaterialProp, Vector4> m_valueMap = new Dictionary<MaterialProp, Vector4>();

        public void AccumulateValue(BlendShapeClip clip, float weight)
        {
            foreach (var binding in clip.MaterialValues)
            {
                var materialItem = m_materialMap[binding.MaterialName];
                var prop = materialItem.PropMap[binding.BindType];
                var delta = (binding.TargetValue - prop.DefaultValues) * weight;
                var key = new MaterialProp(materialItem.Material, binding.BindType, prop.Name);
                if (m_valueMap.TryGetValue(key, out Vector4 acc))
                {
                    m_valueMap[key] = acc + delta;
                }
                else
                {
                    m_valueMap.Add(key, prop.DefaultValues + delta);
                }
            }
        }

        public void Apply()
        {
            foreach (var kv in m_valueMap)
            {
                kv.Key.Material.SetColor(kv.Key.Property, kv.Value);
            }
            m_valueMap.Clear();
        }
        #endregion
    }
}
