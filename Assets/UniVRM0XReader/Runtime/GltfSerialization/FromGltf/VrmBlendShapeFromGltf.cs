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

        /// <summary>
        /// VRM-0.X の MaterialBindValue を VRM-1.0 仕様に変換する
        ///
        /// * Property名 => enum MaterialBindType
        /// * 特に _MainTex_ST の場合、MaterialBindType.UvScale + MaterialBindType.UvScale ２つになりうる
        ///
        /// </summary>
        static IEnumerable<MaterialBindValue> FromGltf(VrmMaterialValueBind y, List<Material> materials)
        {
            var material = materials.First(z => z.Name == y.materialName);
            var target = new Vector4(y.targetValue[0], y.targetValue[1], y.targetValue[2], y.targetValue[3]);

            // if (y.propertyName.EndsWith("_ST")
            // || y.propertyName.EndsWith("_ST_S")
            // || y.propertyName.EndsWith("_ST_T"))
            // {
            //     if (target.X == 1.0f && target.Y == 1.0f && target.Z == 0 && target.W == 0)
            //     {
            //         // 変化なし。不要
            //     }
            //     else if (target.X == 1.0f && target.Y == 1.0f)
            //     {
            //         // offset only => ZW に格納された値を XY に移動する
            //         yield return new MaterialBindValue(material, MaterialBindType.UvOffset, new Vector4(target.Z, target.W, 0, 0));
            //     }
            //     else if (target.Z == 0 && target.W == 0)
            //     {
            //         // scale only
            //         yield return new MaterialBindValue(material, MaterialBindType.UvScale, target);
            //     }
            //     else
            //     {
            //         // scale と offset ２つになる
            //         yield return new MaterialBindValue(material, MaterialBindType.UvOffset, new Vector4(target.Z, target.W, 0, 0));
            //         yield return new MaterialBindValue(material, MaterialBindType.UvScale, target);
            //     }
            // }
            // else
            {
                var bindType = material.GetBindType(y.propertyName);
                yield return new MaterialBindValue(material, bindType, target);
            }
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
                expression.MaterialValues.AddRange(
                    x.materialValues.SelectMany(y => FromGltf(y, materials)));
                return expression;
            }));
            return manager;
        }
    }
}
