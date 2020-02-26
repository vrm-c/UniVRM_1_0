using System;
using System.Linq;

namespace VrmLib
{
    public static class ModelExtensionsForValidation
    {
        public static void Validate(this Model model)
        {
            foreach (var node in model.Root.Traverse().Skip(1))
            {
                if (!model.Nodes.Contains(node))
                {
                    throw new Exception("nodes must Contains node");
                }
            }

            if (model.Vrm != null)
            {
                if (model.Vrm.BlendShape != null)
                {
                    foreach (var b in model.Vrm.BlendShape.BlendShapeList)
                    {
                        foreach (var v in b.BlendShapeValues)
                        {
                            if (v.Node is null)
                            {
                                throw new ArgumentNullException("BlendShapeBindValue.Node is null");
                            }
                        }
                    }
                }

                if (model.Vrm.FirstPerson != null)
                {
                    foreach (var a in model.Vrm.FirstPerson.Annotations)
                    {
                        if (a.Node is null)
                        {
                            throw new ArgumentNullException("FirstPersonMeshAnnotation.Node is null");
                        }
                    }
                }

                var humanDict = model.Root.Traverse()
                    .Where(x => x.HumanoidBone.HasValue)
                    .ToDictionary(x => x.HumanoidBone.Value, x => x);

                foreach (var required in new[]{
                    HumanoidBones.hips,
                })
                {
                    if (!humanDict.ContainsKey(required))
                    {
                        throw new Exception($"no {required}");
                    }
                }
            }
        }
    }
}
