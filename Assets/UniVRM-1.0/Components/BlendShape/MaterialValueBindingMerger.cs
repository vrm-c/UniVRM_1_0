using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniVRM10
{
    ///
    /// Base + (A.Target - Base) * A.Weight + (B.Target - Base) * B.Weight ...
    ///
    class MaterialValueBindingMerger
    {
        #region MaterialMap
        /// <summary>
        /// MaterialValueBinding の対象になるマテリアルの情報を記録する
        /// </summary>
        Dictionary<string, PreviewMaterialItem> m_materialMap = new Dictionary<string, PreviewMaterialItem>();

        /// <summary>
        /// m_materialMap に記録した値に Material を復旧する
        /// </summary>
        public void RestoreMaterialInitialValues()
        {
            foreach (var kv in m_materialMap)
            {
                kv.Value.RestoreInitialValues();
            }
        }
        #endregion

        #region Accumulate
        struct DictionaryKeyMaterialValueBindingComparer : IEqualityComparer<MaterialColorBinding>
        {
            public bool Equals(MaterialColorBinding x, MaterialColorBinding y)
            {
                return x.TargetValue == y.TargetValue && x.MaterialName == y.MaterialName && x.BindType == y.BindType;
            }

            public int GetHashCode(MaterialColorBinding obj)
            {
                return obj.GetHashCode();
            }
        }

        static DictionaryKeyMaterialValueBindingComparer comparer = new DictionaryKeyMaterialValueBindingComparer();

        /// <summary>
        /// MaterialValueの適用値を蓄積する
        /// </summary>
        /// <typeparam name="MaterialValueBinding"></typeparam>
        /// <typeparam name="float"></typeparam>
        /// <returns></returns>
        Dictionary<MaterialColorBinding, float> m_materialValueMap = new Dictionary<MaterialColorBinding, float>(comparer);

        // Dictionary<MaterialValueBinding, Setter> m_materialSetterMap = new Dictionary<MaterialValueBinding, Setter>(comparer);
        public void AccumulateValue(BlendShapeClip clip, float value)
        {
            foreach (var binding in clip.MaterialValues)
            {
                // 積算
                float acc;
                if (m_materialValueMap.TryGetValue(binding, out acc))
                {
                    m_materialValueMap[binding] = acc + value;
                }
                else
                {
                    m_materialValueMap[binding] = value;
                }
            }
        }

        struct MaterialTarget : IEquatable<MaterialTarget>
        {
            public string MaterialName;
            public string ValueName;

            public bool Equals(MaterialTarget other)
            {
                return MaterialName == other.MaterialName
                    && ValueName == other.ValueName;
            }

            public override bool Equals(object obj)
            {
                if (obj is MaterialTarget)
                {
                    return Equals((MaterialTarget)obj);
                }
                else
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                if (MaterialName == null || ValueName == null)
                {
                    return 0;
                }
                return MaterialName.GetHashCode() + ValueName.GetHashCode();
            }

            public static MaterialTarget Create(MaterialColorBinding binding)
            {
                return new MaterialTarget
                {
                    MaterialName = binding.MaterialName,
                    ValueName = VrmLib.MaterialBindTypeExtensions.GetProperty(binding.BindType),
                };
            }
        }

        HashSet<MaterialTarget> m_used = new HashSet<MaterialTarget>();
        public void Apply()
        {
            m_used.Clear();
            foreach (var kv in m_materialValueMap)
            {
                var key = MaterialTarget.Create(kv.Key);
                PreviewMaterialItem item;
                if (m_materialMap.TryGetValue(key.MaterialName, out item))
                {
                    // 初期値(コンストラクタで記録)
                    var initial = item.PropMap[kv.Key.BindType].DefaultValues;
                    if (!m_used.Contains(key))
                    {
                        //
                        // m_used に入っていない場合は、このフレームで初回の呼び出しになる。
                        // (Apply はフレームに一回呼ばれる想定)
                        // 初回は、値を初期値に戻す。
                        //
                        item.Material.SetColor(key.ValueName, initial);
                        m_used.Add(key);
                    }

                    // 現在値
                    var current = item.Material.GetVector(key.ValueName);
                    // 変化量
                    var value = (kv.Key.TargetValue - initial) * kv.Value;
                    // 適用
                    item.Material.SetColor(key.ValueName, current + value);
                }
                else
                {
                    // エラー？
                }
            }
            m_materialValueMap.Clear();
        }
        #endregion

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
    }
}
