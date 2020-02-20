using System;
using System.Collections.Generic;
using System.Numerics;

namespace VrmLib
{
    public class BlendShapeBindValue
    {
        /// <summary>
        /// 対象のMesh(Renderer)
        /// </summary>
        public Node Node;

        /// <summary>
        /// BlendShapeのIndex
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// BlendShapeの適用度
        /// </summary>
        public readonly float Value;

        public BlendShapeBindValue(Node node, string name, float value)
        {
            Node = node;
            Name = name;
            Value = value;
        }
    }

    public enum MaterialBindType
    {
        Color,
        UvScale,
        UvOffset,
    }

    public class MaterialBindValue
    {
        public readonly Material Material;

        public readonly MaterialBindType BindType;

        [Obsolete]
        public string Property;

        public readonly Vector4 Value;

        public MaterialBindValue(Material material, String property, Vector4 value)
        {
            Material = material;
            Property = property;
            Value = value;
        }

        public MaterialBindValue(Material material, MaterialBindType bindType, Vector4 value)
        {
            Material = material;
            BindType = bindType;
            Value = value;
        }
    }

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

    public class BlendShapeManager
    {
        public readonly List<BlendShape> BlendShapeList = new List<BlendShape>();
    }
}