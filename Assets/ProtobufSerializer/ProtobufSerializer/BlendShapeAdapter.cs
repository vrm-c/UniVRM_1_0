using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using VrmLib;

namespace Vrm10
{
    public static class BlendShapeAdapter
    {
        public static BlendShapeManager FromGltf(this VrmProtobuf.BlendShape master, List<MeshGroup> meshes, List<Material> materials)
        {
            var manager = new BlendShapeManager();
            manager.BlendShapeList.AddRange(master.BlendShapeGroups.Select(x =>
            {
                var expression = new BlendShape((VrmLib.BlendShapePreset)x.Preset,
                    x.Name,
                    x.IsBinary.HasValue && x.IsBinary.Value);
                expression.BlendShapeValues.AddRange(
                    x.Binds.Select(y =>
                    {
                        var group = meshes[y.Mesh];
                        var blendShapeName = group.Meshes[0].MorphTargets[y.Index].Name;
                        var value = new BlendShapeBindValue(group, blendShapeName, y.Weight);
                        return value;
                    }));
                expression.MaterialValues.AddRange(
                    x.MaterialValues.Select(y =>
                    {
                        var material = materials[y.Material];
                        Vector4 target = default;
                        if (y.TargetValue.Count > 0) target.X = y.TargetValue[0];
                        if (y.TargetValue.Count > 1) target.Y = y.TargetValue[1];
                        if (y.TargetValue.Count > 2) target.Z = y.TargetValue[2];
                        if (y.TargetValue.Count > 3) target.W = y.TargetValue[3];
                        var value = new MaterialBindValue(material, EnumUtil.Parse<MaterialBindType>(y.Type.ToString()), target);
                        return value;
                    }));
                return expression;
            }));
            return manager;
        }

        public static VrmProtobuf.BlendShapeGroup.Types.BlendShapeBind ToGltf(this BlendShapeBindValue self, List<MeshGroup> meshes)
        {
            var name = self.Name;
            var value = self.Value;
            var index = self.Mesh.Meshes[0].MorphTargets.FindIndex(x => x.Name == name);
            if (index < 0)
            {
                throw new IndexOutOfRangeException(string.Format("MorphTargetName {0} is not found", name));
            }

            return new VrmProtobuf.BlendShapeGroup.Types.BlendShapeBind
            {
                Mesh = meshes.IndexOfThrow(self.Mesh),
                Index = self.Mesh.Meshes[0].MorphTargets.FindIndex(x => x.Name == name),
                Weight = value,
            };
        }

        public static VrmProtobuf.BlendShapeGroup.Types.MaterialValue ToGltf(this MaterialBindValue self, List<Material> materials)
        {
            var m = new VrmProtobuf.BlendShapeGroup.Types.MaterialValue
            {
                Material = materials.IndexOfThrow(self.Material),
            };

            switch (self.Property)
            {
                // Type = self.Property,
                default:
                    throw new Exception($"unknown exception: {self.Property}");
            }

            m.TargetValue.Add(self.Value.X);
            m.TargetValue.Add(self.Value.Y);
            m.TargetValue.Add(self.Value.Z);
            m.TargetValue.Add(self.Value.W);

            return m;
        }

        public static VrmProtobuf.BlendShapeGroup ToGltf(this BlendShape x, List<MeshGroup> meshes, List<Material> materials)
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
            foreach (var y in x.BlendShapeValues)
            {
                g.Binds.Add(y.ToGltf(meshes));
            }
            foreach (var y in x.MaterialValues)
            {
                g.MaterialValues.Add(y.ToGltf(materials));
            }
            return g;
        }

        public static VrmProtobuf.BlendShape ToGltf(this BlendShapeManager src, List<MeshGroup> meshes, List<Material> materials)
        {
            var blendShape = new VrmProtobuf.BlendShape
            {
            };
            if (src != null)
            {
                foreach (var x in src.BlendShapeList)
                {
                    blendShape.BlendShapeGroups.Add(x.ToGltf(meshes, materials));
                }
            }
            return blendShape;
        }
    }
}
