using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniVRM10
{
    /// <summary>
    /// Bone割り当てを保持する。
    /// ヒエラルキーのルート(おそらくHipsの親)にアタッチする
    /// </summary>
    [DisallowMultipleComponent]
    public class VrmHumanoid : MonoBehaviour
    {
        public Transform Hips;

        #region leg
        public Transform LeftUpperLeg;
        public Transform RightUpperLeg;
        public Transform LeftLowerLeg;
        public Transform RightLowerLeg;
        public Transform LeftFoot;
        public Transform RightFoot;
        public Transform LeftToes;
        public Transform RightToes;
        #endregion

        #region spine
        public Transform Spine;
        public Transform Chest;
        public Transform UpperChest;
        public Transform Neck;
        public Transform Head;
        public Transform LeftEye;
        public Transform RightEye;
        public Transform Jaw;
        #endregion

        #region arm
        public Transform LeftShoulder;
        public Transform RightShoulder;
        public Transform LeftUpperArm;
        public Transform RightUpperArm;
        public Transform LeftLowerArm;
        public Transform RightLowerArm;
        public Transform LeftHand;
        public Transform RightHand;
        #endregion

        #region fingers
        public Transform LeftThumbProximal;
        public Transform LeftThumbIntermediate;
        public Transform LeftThumbDistal;
        public Transform LeftIndexProximal;
        public Transform LeftIndexIntermediate;
        public Transform LeftIndexDistal;
        public Transform LeftMiddleProximal;
        public Transform LeftMiddleIntermediate;
        public Transform LeftMiddleDistal;
        public Transform LeftRingProximal;
        public Transform LeftRingIntermediate;
        public Transform LeftRingDistal;
        public Transform LeftLittleProximal;
        public Transform LeftLittleIntermediate;
        public Transform LeftLittleDistal;
        public Transform RightThumbProximal;
        public Transform RightThumbIntermediate;
        public Transform RightThumbDistal;
        public Transform RightIndexProximal;
        public Transform RightIndexIntermediate;
        public Transform RightIndexDistal;
        public Transform RightMiddleProximal;
        public Transform RightMiddleIntermediate;
        public Transform RightMiddleDistal;
        public Transform RightRingProximal;
        public Transform RightRingIntermediate;
        public Transform RightRingDistal;
        public Transform RightLittleProximal;
        public Transform RightLittleIntermediate;
        public Transform RightLittleDistal;
        #endregion

        #region Unity
        /// <summary>
        /// ボーン割り当てから UnityEngine.Avatar を生成する
        /// </summary>
        /// <returns></returns>
        public Avatar CreateAvatar()
        {
            return HumanoidLoader.LoadHumanoidAvatar(transform, BoneMap);
        }

        public Transform GetBoneTransform(HumanBodyBones bone)
        {
            switch (bone)
            {
                case HumanBodyBones.Hips: return Hips;

                #region leg
                case HumanBodyBones.LeftUpperLeg: return LeftUpperLeg;
                case HumanBodyBones.RightUpperLeg: return RightUpperLeg;
                case HumanBodyBones.LeftLowerLeg: return LeftLowerLeg;
                case HumanBodyBones.RightLowerLeg: return RightLowerLeg;
                case HumanBodyBones.LeftFoot: return LeftFoot;
                case HumanBodyBones.RightFoot: return RightFoot;
                case HumanBodyBones.LeftToes: return LeftToes;
                case HumanBodyBones.RightToes: return RightToes;
                #endregion

                #region spine
                case HumanBodyBones.Spine: return Spine;
                case HumanBodyBones.Chest: return Chest;
                case HumanBodyBones.UpperChest: return UpperChest;
                case HumanBodyBones.Neck: return Neck;
                case HumanBodyBones.Head: return Head;
                case HumanBodyBones.LeftEye: return LeftEye;
                case HumanBodyBones.RightEye: return RightEye;
                case HumanBodyBones.Jaw: return Jaw;
                #endregion

                #region arm
                case HumanBodyBones.LeftShoulder: return LeftShoulder;
                case HumanBodyBones.RightShoulder: return RightShoulder;
                case HumanBodyBones.LeftUpperArm: return LeftUpperArm;
                case HumanBodyBones.RightUpperArm: return RightUpperArm;
                case HumanBodyBones.LeftLowerArm: return LeftLowerArm;
                case HumanBodyBones.RightLowerArm: return RightLowerArm;
                case HumanBodyBones.LeftHand: return LeftHand;
                case HumanBodyBones.RightHand: return RightHand;
                #endregion

                #region fingers
                case HumanBodyBones.LeftThumbProximal: return LeftThumbProximal;
                case HumanBodyBones.LeftThumbIntermediate: return LeftThumbIntermediate;
                case HumanBodyBones.LeftThumbDistal: return LeftThumbDistal;
                case HumanBodyBones.LeftIndexProximal: return LeftIndexProximal;
                case HumanBodyBones.LeftIndexIntermediate: return LeftIndexIntermediate;
                case HumanBodyBones.LeftIndexDistal: return LeftIndexDistal;
                case HumanBodyBones.LeftMiddleProximal: return LeftMiddleProximal;
                case HumanBodyBones.LeftMiddleIntermediate: return LeftMiddleIntermediate;
                case HumanBodyBones.LeftMiddleDistal: return LeftMiddleDistal;
                case HumanBodyBones.LeftRingProximal: return LeftRingProximal;
                case HumanBodyBones.LeftRingIntermediate: return LeftRingIntermediate;
                case HumanBodyBones.LeftRingDistal: return LeftRingDistal;
                case HumanBodyBones.LeftLittleProximal: return LeftLittleProximal;
                case HumanBodyBones.LeftLittleIntermediate: return LeftLittleIntermediate;
                case HumanBodyBones.LeftLittleDistal: return LeftLittleDistal;
                case HumanBodyBones.RightThumbProximal: return RightThumbProximal;
                case HumanBodyBones.RightThumbIntermediate: return RightThumbIntermediate;
                case HumanBodyBones.RightThumbDistal: return RightThumbDistal;
                case HumanBodyBones.RightIndexProximal: return RightIndexProximal;
                case HumanBodyBones.RightIndexIntermediate: return RightIndexIntermediate;
                case HumanBodyBones.RightIndexDistal: return RightIndexDistal;
                case HumanBodyBones.RightMiddleProximal: return RightMiddleProximal;
                case HumanBodyBones.RightMiddleIntermediate: return RightMiddleIntermediate;
                case HumanBodyBones.RightMiddleDistal: return RightMiddleDistal;
                case HumanBodyBones.RightRingProximal: return RightRingProximal;
                case HumanBodyBones.RightRingIntermediate: return RightRingIntermediate;
                case HumanBodyBones.RightRingDistal: return RightRingDistal;
                case HumanBodyBones.RightLittleProximal: return RightLittleProximal;
                case HumanBodyBones.RightLittleIntermediate: return RightLittleIntermediate;
                case HumanBodyBones.RightLittleDistal: return RightLittleDistal;
                    #endregion

            }

            return null;
        }
        #endregion

        IEnumerable<(Transform, VrmLib.HumanoidBones)> BoneMap
        {
            get
            {
                if (Hips != null) { yield return (Hips, VrmLib.HumanoidBones.hips); }

                #region leg
                if (LeftUpperLeg != null) { yield return (LeftUpperLeg, VrmLib.HumanoidBones.leftUpperLeg); }
                if (RightUpperLeg != null) { yield return (RightUpperLeg, VrmLib.HumanoidBones.rightUpperLeg); }
                if (LeftLowerLeg != null) { yield return (LeftLowerLeg, VrmLib.HumanoidBones.leftLowerLeg); }
                if (RightLowerLeg != null) { yield return (RightLowerLeg, VrmLib.HumanoidBones.rightLowerLeg); }
                if (LeftFoot != null) { yield return (LeftFoot, VrmLib.HumanoidBones.leftFoot); }
                if (RightFoot != null) { yield return (RightFoot, VrmLib.HumanoidBones.rightFoot); }
                if (LeftToes != null) { yield return (LeftToes, VrmLib.HumanoidBones.leftToes); }
                if (RightToes != null) { yield return (RightToes, VrmLib.HumanoidBones.rightToes); }
                #endregion

                #region spine
                if (Spine != null) { yield return (Spine, VrmLib.HumanoidBones.spine); }
                if (Chest != null) { yield return (Chest, VrmLib.HumanoidBones.chest); }
                if (UpperChest != null) { yield return (UpperChest, VrmLib.HumanoidBones.upperChest); }
                if (Neck != null) { yield return (Neck, VrmLib.HumanoidBones.neck); }
                if (Head != null) { yield return (Head, VrmLib.HumanoidBones.head); }
                if (LeftEye != null) { yield return (LeftEye, VrmLib.HumanoidBones.leftEye); }
                if (RightEye != null) { yield return (RightEye, VrmLib.HumanoidBones.rightEye); }
                if (Jaw != null) { yield return (Jaw, VrmLib.HumanoidBones.jaw); }
                #endregion

                #region arm
                if (LeftShoulder != null) { yield return (LeftShoulder, VrmLib.HumanoidBones.leftShoulder); }
                if (RightShoulder != null) { yield return (RightShoulder, VrmLib.HumanoidBones.rightShoulder); }
                if (LeftUpperArm != null) { yield return (LeftUpperArm, VrmLib.HumanoidBones.leftUpperArm); }
                if (RightUpperArm != null) { yield return (RightUpperArm, VrmLib.HumanoidBones.rightUpperArm); }
                if (LeftLowerArm != null) { yield return (LeftLowerArm, VrmLib.HumanoidBones.leftLowerArm); }
                if (RightLowerArm != null) { yield return (RightLowerArm, VrmLib.HumanoidBones.rightLowerArm); }
                if (LeftHand != null) { yield return (LeftHand, VrmLib.HumanoidBones.leftHand); }
                if (RightHand != null) { yield return (RightHand, VrmLib.HumanoidBones.rightHand); }
                #endregion

                #region fingers
                if (LeftThumbProximal != null) { yield return (LeftThumbProximal, VrmLib.HumanoidBones.leftThumbProximal); }
                if (LeftThumbIntermediate != null) { yield return (LeftThumbIntermediate, VrmLib.HumanoidBones.leftThumbIntermediate); }
                if (LeftThumbDistal != null) { yield return (LeftThumbDistal, VrmLib.HumanoidBones.leftThumbDistal); }
                if (LeftIndexProximal != null) { yield return (LeftIndexProximal, VrmLib.HumanoidBones.leftIndexProximal); }
                if (LeftIndexIntermediate != null) { yield return (LeftIndexIntermediate, VrmLib.HumanoidBones.leftIndexIntermediate); }
                if (LeftIndexDistal != null) { yield return (LeftIndexDistal, VrmLib.HumanoidBones.leftIndexDistal); }
                if (LeftMiddleProximal != null) { yield return (LeftMiddleProximal, VrmLib.HumanoidBones.leftMiddleProximal); }
                if (LeftMiddleIntermediate != null) { yield return (LeftMiddleIntermediate, VrmLib.HumanoidBones.leftMiddleIntermediate); }
                if (LeftMiddleDistal != null) { yield return (LeftMiddleDistal, VrmLib.HumanoidBones.leftMiddleDistal); }
                if (LeftRingProximal != null) { yield return (LeftRingProximal, VrmLib.HumanoidBones.leftRingProximal); }
                if (LeftRingIntermediate != null) { yield return (LeftRingIntermediate, VrmLib.HumanoidBones.leftRingIntermediate); }
                if (LeftRingDistal != null) { yield return (LeftRingDistal, VrmLib.HumanoidBones.leftRingDistal); }
                if (LeftLittleProximal != null) { yield return (LeftLittleProximal, VrmLib.HumanoidBones.leftLittleProximal); }
                if (LeftLittleIntermediate != null) { yield return (LeftLittleIntermediate, VrmLib.HumanoidBones.leftLittleIntermediate); }
                if (LeftLittleDistal != null) { yield return (LeftLittleDistal, VrmLib.HumanoidBones.leftLittleDistal); }
                if (RightThumbProximal != null) { yield return (RightThumbProximal, VrmLib.HumanoidBones.rightThumbProximal); }
                if (RightThumbIntermediate != null) { yield return (RightThumbIntermediate, VrmLib.HumanoidBones.rightThumbIntermediate); }
                if (RightThumbDistal != null) { yield return (RightThumbDistal, VrmLib.HumanoidBones.rightThumbDistal); }
                if (RightIndexProximal != null) { yield return (RightIndexProximal, VrmLib.HumanoidBones.rightIndexProximal); }
                if (RightIndexIntermediate != null) { yield return (RightIndexIntermediate, VrmLib.HumanoidBones.rightIndexIntermediate); }
                if (RightIndexDistal != null) { yield return (RightIndexDistal, VrmLib.HumanoidBones.rightIndexDistal); }
                if (RightMiddleProximal != null) { yield return (RightMiddleProximal, VrmLib.HumanoidBones.rightMiddleProximal); }
                if (RightMiddleIntermediate != null) { yield return (RightMiddleIntermediate, VrmLib.HumanoidBones.rightMiddleIntermediate); }
                if (RightMiddleDistal != null) { yield return (RightMiddleDistal, VrmLib.HumanoidBones.rightMiddleDistal); }
                if (RightRingProximal != null) { yield return (RightRingProximal, VrmLib.HumanoidBones.rightRingProximal); }
                if (RightRingIntermediate != null) { yield return (RightRingIntermediate, VrmLib.HumanoidBones.rightRingIntermediate); }
                if (RightRingDistal != null) { yield return (RightRingDistal, VrmLib.HumanoidBones.rightRingDistal); }
                if (RightLittleProximal != null) { yield return (RightLittleProximal, VrmLib.HumanoidBones.rightLittleProximal); }
                if (RightLittleIntermediate != null) { yield return (RightLittleIntermediate, VrmLib.HumanoidBones.rightLittleIntermediate); }
                if (RightLittleDistal != null) { yield return (RightLittleDistal, VrmLib.HumanoidBones.rightLittleDistal); }
                #endregion
            }
        }

        /// <summary>
        /// nodes からボーンを割り当てる
        /// </summary>
        /// <param name="nodes"></param>
        public void AssignBones(IEnumerable<(VrmLib.HumanoidBones, Transform)> nodes)
        {
            foreach (var (key, value) in nodes)
            {
                if (key == VrmLib.HumanoidBones.unknown)
                {
                    continue;
                }
                if (value is null)
                {
                    continue;
                }

                switch (key)
                {
                    case VrmLib.HumanoidBones.hips: Hips = value; break;

                    #region leg
                    case VrmLib.HumanoidBones.leftUpperLeg: LeftUpperLeg = value; break;
                    case VrmLib.HumanoidBones.rightUpperLeg: RightUpperLeg = value; break;
                    case VrmLib.HumanoidBones.leftLowerLeg: LeftLowerLeg = value; break;
                    case VrmLib.HumanoidBones.rightLowerLeg: RightLowerLeg = value; break;
                    case VrmLib.HumanoidBones.leftFoot: LeftFoot = value; break;
                    case VrmLib.HumanoidBones.rightFoot: RightFoot = value; break;
                    case VrmLib.HumanoidBones.leftToes: LeftToes = value; break;
                    case VrmLib.HumanoidBones.rightToes: RightToes = value; break;
                    #endregion

                    #region spine
                    case VrmLib.HumanoidBones.spine: Spine = value; break;
                    case VrmLib.HumanoidBones.chest: Chest = value; break;
                    case VrmLib.HumanoidBones.upperChest: UpperChest = value; break;
                    case VrmLib.HumanoidBones.neck: Neck = value; break;
                    case VrmLib.HumanoidBones.head: Head = value; break;
                    case VrmLib.HumanoidBones.leftEye: LeftEye = value; break;
                    case VrmLib.HumanoidBones.rightEye: RightEye = value; break;
                    case VrmLib.HumanoidBones.jaw: Jaw = value; break;
                    #endregion

                    #region arm
                    case VrmLib.HumanoidBones.leftShoulder: LeftShoulder = value; break;
                    case VrmLib.HumanoidBones.rightShoulder: RightShoulder = value; break;
                    case VrmLib.HumanoidBones.leftUpperArm: LeftUpperArm = value; break;
                    case VrmLib.HumanoidBones.rightUpperArm: RightUpperArm = value; break;
                    case VrmLib.HumanoidBones.leftLowerArm: LeftLowerArm = value; break;
                    case VrmLib.HumanoidBones.rightLowerArm: RightLowerArm = value; break;
                    case VrmLib.HumanoidBones.leftHand: LeftHand = value; break;
                    case VrmLib.HumanoidBones.rightHand: RightHand = value; break;
                    #endregion

                    #region fingers
                    case VrmLib.HumanoidBones.leftThumbProximal: LeftThumbProximal = value; break;
                    case VrmLib.HumanoidBones.leftThumbIntermediate: LeftThumbIntermediate = value; break;
                    case VrmLib.HumanoidBones.leftThumbDistal: LeftThumbDistal = value; break;
                    case VrmLib.HumanoidBones.leftIndexProximal: LeftIndexProximal = value; break;
                    case VrmLib.HumanoidBones.leftIndexIntermediate: LeftIndexIntermediate = value; break;
                    case VrmLib.HumanoidBones.leftIndexDistal: LeftIndexDistal = value; break;
                    case VrmLib.HumanoidBones.leftMiddleProximal: LeftMiddleProximal = value; break;
                    case VrmLib.HumanoidBones.leftMiddleIntermediate: LeftMiddleIntermediate = value; break;
                    case VrmLib.HumanoidBones.leftMiddleDistal: LeftMiddleDistal = value; break;
                    case VrmLib.HumanoidBones.leftRingProximal: LeftRingProximal = value; break;
                    case VrmLib.HumanoidBones.leftRingIntermediate: LeftRingIntermediate = value; break;
                    case VrmLib.HumanoidBones.leftRingDistal: LeftRingDistal = value; break;
                    case VrmLib.HumanoidBones.leftLittleProximal: LeftLittleProximal = value; break;
                    case VrmLib.HumanoidBones.leftLittleIntermediate: LeftLittleIntermediate = value; break;
                    case VrmLib.HumanoidBones.leftLittleDistal: LeftLittleDistal = value; break;
                    case VrmLib.HumanoidBones.rightThumbProximal: RightThumbProximal = value; break;
                    case VrmLib.HumanoidBones.rightThumbIntermediate: RightThumbIntermediate = value; break;
                    case VrmLib.HumanoidBones.rightThumbDistal: RightThumbDistal = value; break;
                    case VrmLib.HumanoidBones.rightIndexProximal: RightIndexProximal = value; break;
                    case VrmLib.HumanoidBones.rightIndexIntermediate: RightIndexIntermediate = value; break;
                    case VrmLib.HumanoidBones.rightIndexDistal: RightIndexDistal = value; break;
                    case VrmLib.HumanoidBones.rightMiddleProximal: RightMiddleProximal = value; break;
                    case VrmLib.HumanoidBones.rightMiddleIntermediate: RightMiddleIntermediate = value; break;
                    case VrmLib.HumanoidBones.rightMiddleDistal: RightMiddleDistal = value; break;
                    case VrmLib.HumanoidBones.rightRingProximal: RightRingProximal = value; break;
                    case VrmLib.HumanoidBones.rightRingIntermediate: RightRingIntermediate = value; break;
                    case VrmLib.HumanoidBones.rightRingDistal: RightRingDistal = value; break;
                    case VrmLib.HumanoidBones.rightLittleProximal: RightLittleProximal = value; break;
                    case VrmLib.HumanoidBones.rightLittleIntermediate: RightLittleIntermediate = value; break;
                    case VrmLib.HumanoidBones.rightLittleDistal: RightLittleDistal = value; break;
                        #endregion
                }
            }
        }

        /// <summary>
        /// Animator から Bone を割り当てる
        /// </summary>
        /// <returns></returns>
        public bool AssignBonesFromAnimator()
        {
            var animator = GetComponent<Animator>();
            if (animator is null)
            {
                return false;
            }
            var avatar = animator.avatar;
            if (avatar is null)
            {
                return false;
            }
            if (!avatar.isValid)
            {
                return false;
            }
            if (!avatar.isHuman)
            {
                return false;
            }

            var keys = (UnityEngine.HumanBodyBones[])Enum.GetValues(typeof(UnityEngine.HumanBodyBones));

            AssignBones(keys.Select(x =>
            {
                if (x == HumanBodyBones.LastBone)
                {
                    return (default(VrmLib.HumanoidBones), default(Transform));
                }
                return ((VrmLib.HumanoidBones)Enum.Parse(typeof(VrmLib.HumanoidBones), x.ToString(), true), animator.GetBoneTransform(x));
            }));

            return true;
        }
    }
}
