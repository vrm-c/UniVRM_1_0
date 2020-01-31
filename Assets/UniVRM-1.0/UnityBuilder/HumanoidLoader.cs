using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniVRM10
{
    public static class HumanoidLoader
    {
        public static Avatar LoadHumanoidAvatar(Transform root, Dictionary<Transform, VrmLib.HumanoidBones> boneMap)
        {
            var description = new HumanDescription
            {
                skeleton = root.Traverse()
                    .Select(x => x.ToSkeletonBone()).ToArray(),
                human = root.Traverse()
                    .Where(x => boneMap.ContainsKey(x))
                    .Select(x => new HumanBone
                    {
                        boneName = x.name,
                        humanName = s_humanTranitBoneNameMap[boneMap[x]],
                    }).ToArray(),
            };

            return AvatarBuilder.BuildHumanAvatar(root.gameObject, description);
        }

        static SkeletonBone ToSkeletonBone(this Transform t)
        {
            var sb = new SkeletonBone();
            sb.name = t.name;
            sb.position = t.localPosition;
            sb.rotation = t.localRotation;
            sb.scale = t.localScale;
            return sb;
        }

        static VrmLib.HumanoidBones TraitToHumanBone(string x)
        {
            return (VrmLib.HumanoidBones)Enum.Parse(typeof(VrmLib.HumanoidBones), x.Replace(" ", ""), true);
        }

        static readonly Dictionary<VrmLib.HumanoidBones, string> s_humanTranitBoneNameMap =
        HumanTrait.BoneName.ToDictionary(
            x => TraitToHumanBone(x),
            x => x);
    }
}
