using System;
using System.Collections.Generic;
using System.Numerics;
using VrmLib;

namespace Vrm10
{
    public static class BlendShapeAdapter
    {
        public static BlendShapeManager FromGltf(this VrmProtobuf.BlendShape master, List<Node> nodes, List<Material> materials)
        {
            var manager = new BlendShapeManager();
            foreach (var x in master.BlendShapeGroups)
            {
                var expression = new BlendShape((VrmLib.BlendShapePreset)x.Preset,
                    x.Name,
                    x.IsBinary.HasValue && x.IsBinary.Value);

                foreach (var y in x.Binds)
                {
                    var node = nodes[y.Node];
                    var blendShapeName = node.Mesh.MorphTargets[y.Index].Name;
                    var blendShapeBind = new BlendShapeBindValue(node, blendShapeName, y.Weight);
                    expression.BlendShapeValues.Add(blendShapeBind);
                }

                foreach (var y in x.MaterialValues)
                {
                    var material = materials[y.Material];
                    Vector4 target = default;
                    if (y.TargetValue.Count > 0) target.X = y.TargetValue[0];
                    if (y.TargetValue.Count > 1) target.Y = y.TargetValue[1];
                    if (y.TargetValue.Count > 2) target.Z = y.TargetValue[2];
                    if (y.TargetValue.Count > 3) target.W = y.TargetValue[3];
                    var materialColorBind = new MaterialBindValue(material, EnumUtil.Cast<MaterialBindType>(y.Type), target);
                    expression.MaterialValues.Add(materialColorBind);
                }

                foreach (var y in x.MaterialUVBinds)
                {
                    var material = materials[y.Material];
                    var scaling = Vector2.One;
                    if (y.Scaling.Count > 0) scaling.X = y.Scaling[0];
                    if (y.Scaling.Count > 1) scaling.Y = y.Scaling[1];
                    var offset = Vector2.Zero;
                    if (y.Offset.Count > 0) offset.X = y.Offset[0];
                    if (y.Offset.Count > 1) offset.Y = y.Offset[1];
                    var materialUVBind = new UVScaleOffsetValue(material, scaling, offset);
                    expression.UVScaleOffsetValues.Add(materialUVBind);
                }

                manager.BlendShapeList.Add(expression);
            };
            return manager;
        }

        public static VrmProtobuf.BlendShapeGroup.Types.BlendShapeBind ToGltf(this BlendShapeBindValue self, List<Node> nodes)
        {
            var name = self.Name;
            var value = self.Value;
            var index = self.Node.Mesh.MorphTargets.FindIndex(x => x.Name == name);
            if (index < 0)
            {
                throw new IndexOutOfRangeException(string.Format("MorphTargetName {0} is not found", name));
            }

            return new VrmProtobuf.BlendShapeGroup.Types.BlendShapeBind
            {
                Node = nodes.IndexOfThrow(self.Node),
                Index = self.Node.Mesh.MorphTargets.FindIndex(x => x.Name == name),
                Weight = value,
            };
        }

        public static VrmProtobuf.BlendShapeGroup.Types.MaterialValue ToGltf(this MaterialBindValue self, List<Material> materials)
        {
            var m = new VrmProtobuf.BlendShapeGroup.Types.MaterialValue
            {
                Material = materials.IndexOfThrow(self.Material),
                Type = EnumUtil.Cast<VrmProtobuf.BlendShapeGroup.Types.MaterialValueType>(self.BindType),
            };
            var kv = self.Property;
            m.TargetValue.Add(kv.Value.X);
            m.TargetValue.Add(kv.Value.Y);
            m.TargetValue.Add(kv.Value.Z);
            m.TargetValue.Add(kv.Value.W);
            return m;
        }

        public static VrmProtobuf.BlendShapeGroup.Types.MaterialUVBind ToGltf(this UVScaleOffsetValue self, List<Material> materials)
        {
            var m = new VrmProtobuf.BlendShapeGroup.Types.MaterialUVBind
            {
                Material = materials.IndexOfThrow(self.Material),
            };
            m.Scaling.Add(self.Scale.X);
            m.Scaling.Add(self.Scale.Y);
            m.Offset.Add(self.Offset.X);
            m.Offset.Add(self.Offset.Y);
            return m;
        }

        public static VrmProtobuf.BlendShapeGroup ToGltf(this BlendShape x, List<Node> nodes, List<Material> materials)
        {
            var g = new VrmProtobuf.BlendShapeGroup
            {
                Preset = (VrmProtobuf.BlendShapeGroup.Types.BlendShapePreset)x.Preset,
                Name = x.Name,
                IsBinary = x.IsBinary,
                IgnoreBlink = x.IgnoreBlink,
                IgnoreLookAt = x.IgnoreLookAt,
                IgnoreMouth = x.IgnoreMouth,
            };
            foreach (var blendShapeBind in x.BlendShapeValues)
            {
                g.Binds.Add(blendShapeBind.ToGltf(nodes));
            }
            foreach (var materialColorBind in x.MaterialValues)
            {
                g.MaterialValues.Add(materialColorBind.ToGltf(materials));
            }
            foreach (var materialUVBind in x.UVScaleOffsetValues)
            {
                g.MaterialUVBinds.Add(materialUVBind.ToGltf(materials));
            }
            return g;
        }

        public static VrmProtobuf.BlendShape ToGltf(this BlendShapeManager src, List<Node> nodes, List<Material> materials)
        {
            var blendShape = new VrmProtobuf.BlendShape
            {
            };
            if (src != null)
            {
                foreach (var x in src.BlendShapeList)
                {
                    blendShape.BlendShapeGroups.Add(x.ToGltf(nodes, materials));
                }
            }
            return blendShape;
        }
    }
}
