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
                human = boneMap
                    .Select(x => new HumanBone
                    {
                        boneName = x.Key.name,
                        humanName = s_humanTranitBoneNameMap[x.Value],
                        limit = new HumanLimit
                        {
                            useDefaultValues = true,
                        }
                    }).ToArray(),

                armStretch = 0.05f,
                legStretch = 0.05f,
                upperArmTwist = 0.5f,
                lowerArmTwist = 0.5f,
                upperLegTwist = 0.5f,
                lowerLegTwist = 0.5f,
                feetSpacing = 0,
                hasTranslationDoF = false,
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
