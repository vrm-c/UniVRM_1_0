using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace VrmLib
{
    public class BlendShape
    {
        public readonly BlendShapePreset Preset;
        public readonly string Name;

        public bool IsBinary;

        public bool IgnoreBlink;
        public bool IgnoreLookAt;
        public bool IgnoreMouth;

        public readonly List<BlendShapeBindValue> BlendShapeValues = new List<BlendShapeBindValue>();

        public readonly List<MaterialBindValue> MaterialValues = new List<MaterialBindValue>();

        public readonly List<UVScaleOffsetValue> UVScaleOffsetValues = new List<UVScaleOffsetValue>();

        public void CleanupUVScaleOffset()
        {
            // ST_S, ST_T を統合する
            var count = UVScaleOffsetValues.Count;
            var map = new Dictionary<Material, UVScaleOffsetValue>();
            foreach (var uv in UVScaleOffsetValues.OrderBy(uv => uv.Material.Name).Distinct())
            {
                if (!map.TryGetValue(uv.Material, out UVScaleOffsetValue value))
                {
                    value = new UVScaleOffsetValue(uv.Material, Vector2.One, Vector2.Zero);
                }
                map[uv.Material] = value.Merge(uv);
            }
            UVScaleOffsetValues.Clear();
            foreach (var kv in map)
            {
                UVScaleOffsetValues.Add(new UVScaleOffsetValue(kv.Key,
                    kv.Value.Scale,
                    kv.Value.Offset));
            }
            // Console.WriteLine($"MergeUVScaleOffset: {count} => {UVScaleOffsetValues.Count}");
        }

        public BlendShape(BlendShapePreset preset, string name, bool isBinary)
        {
            Preset = preset;
            Name = name;
            IsBinary = isBinary;
        }

        public override string ToString()
        {
            return Preset.ToString();
        }
    }
}
