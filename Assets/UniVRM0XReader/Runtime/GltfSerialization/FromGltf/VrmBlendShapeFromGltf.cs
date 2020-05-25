using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GltfFormat;
using VrmLib;


namespace GltfSerializationAdapter
{
    public static class VrmBlendShapeFromGltf
    {
        public static VrmLib.BlendShapePreset FromGltf(this GltfFormat.VrmBlendShapePreset src)
        {
            switch (src)
            {
                case VrmBlendShapePreset.Unknown: return BlendShapePreset.Custom;
                case VrmBlendShapePreset.A: return BlendShapePreset.Aa;
                case VrmBlendShapePreset.I: return BlendShapePreset.Ih;
                case VrmBlendShapePreset.U: return BlendShapePreset.Ou;
                case VrmBlendShapePreset.E: return BlendShapePreset.Ee;
                case VrmBlendShapePreset.O: return BlendShapePreset.Oh;
                case VrmBlendShapePreset.Blink: return BlendShapePreset.Blink;
                case VrmBlendShapePreset.Joy: return BlendShapePreset.Joy;
                case VrmBlendShapePreset.Angry: return BlendShapePreset.Angry;
                case VrmBlendShapePreset.Sorrow: return BlendShapePreset.Sorrow;
                case VrmBlendShapePreset.Fun: return BlendShapePreset.Fun;
                case VrmBlendShapePreset.LookUp: return BlendShapePreset.LookUp;
                case VrmBlendShapePreset.LookDown: return BlendShapePreset.LookDown;
                case VrmBlendShapePreset.LookLeft: return BlendShapePreset.LookLeft;
                case VrmBlendShapePreset.LookRight: return BlendShapePreset.LookRight;
                case VrmBlendShapePreset.Blink_L: return BlendShapePreset.BlinkLeft;
                case VrmBlendShapePreset.Blink_R: return BlendShapePreset.BlinkRight;
                case VrmBlendShapePreset.Neutral: return BlendShapePreset.Neutral;
            }

            throw new NotImplementedException();
        }

        public static BlendShapeManager FromGltf(this VrmBlendShapeMaster master, List<MeshGroup> meshes, List<Material> materials, List<Node> nodes)
        {
            var manager = new BlendShapeManager();
            manager.BlendShapeList.AddRange(master.blendShapeGroups.Select(x =>
            {
                var expression = new BlendShape(x.presetName.FromGltf(), x.name, x.isBinary);
                expression.BlendShapeValues.AddRange(
                    x.binds.Select(y =>
                    {
                        var group = meshes[y.mesh];
                        var node = nodes.First(z => z.MeshGroup == group);
                        var blendShapeName = group.Meshes[0].MorphTargets[y.index].Name;
                        var value = new BlendShapeBindValue(node, blendShapeName, y.weight);
                        return value;
                    }));

                /// VRM-0.X の MaterialBindValue を VRM-1.0 仕様に変換する
                foreach (var y in x.materialValues)
                {
                    var material = materials.First(z => z.Name == y.materialName);

                    if (y.propertyName.EndsWith("_ST"))
                    {
                        expression.UVScaleOffsetValues.Add(new UVScaleOffsetValue(material,
                            new Vector2(y.targetValue[0], y.targetValue[1]),
                            new Vector2(y.targetValue[2], y.targetValue[3])));
                    }
                    else if (y.propertyName.EndsWith("_ST_S"))
                    {
                        expression.UVScaleOffsetValues.Add(new UVScaleOffsetValue(material,
                            new Vector2(y.targetValue[0], 1),
                            new Vector2(y.targetValue[2], 0)));
                    }
                    else if (y.propertyName.EndsWith("_ST_T"))
                    {
                        expression.UVScaleOffsetValues.Add(new UVScaleOffsetValue(material,
                            new Vector2(1, y.targetValue[1]),
                            new Vector2(0, y.targetValue[3])));
                    }
                    else
                    {
                        var bindType = material.GetBindType(y.propertyName);
                        var target = new Vector4(y.targetValue[0], y.targetValue[1], y.targetValue[2], y.targetValue[3]);
                        expression.MaterialValues.Add(new MaterialBindValue(material, bindType, target));
                    }
                }
                return expression;
            }));
            return manager;
        }
    }
}
