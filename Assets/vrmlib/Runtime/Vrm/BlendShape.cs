using System.Collections.Generic;

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