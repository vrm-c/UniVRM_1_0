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
        [SerializeField] private Transform m_Hips; public Transform Hips => m_Hips;

        #region leg
        [SerializeField] private Transform m_LeftUpperLeg; public Transform LeftUpperLeg => m_LeftUpperLeg;
        [SerializeField] private Transform m_RightUpperLeg; public Transform RightUpperLeg => m_RightUpperLeg;
        [SerializeField] private Transform m_LeftLowerLeg; public Transform LeftLowerLeg => m_LeftLowerLeg;
        [SerializeField] private Transform m_RightLowerLeg; public Transform RightLowerLeg => m_RightLowerLeg;
        [SerializeField] private Transform m_LeftFoot; public Transform LeftFoot => m_LeftFoot;
        [SerializeField] private Transform m_RightFoot; public Transform RightFoot => m_RightFoot;
        [SerializeField] private Transform m_LeftToes; public Transform LeftToes => m_LeftToes;
        [SerializeField] private Transform m_RightToes; public Transform RightToes => m_RightToes;
        #endregion

        #region spine
        [SerializeField] private Transform m_Spine; public Transform Spine => m_Spine;
        [SerializeField] private Transform m_Chest; public Transform Chest => m_Chest;
        [SerializeField] private Transform m_UpperChest; public Transform UpperChest => m_UpperChest;
        [SerializeField] private Transform m_Neck; public Transform Neck => m_Neck;
        [SerializeField] private Transform m_Head; public Transform Head => m_Head;
        [SerializeField] private Transform m_LeftEye; public Transform LeftEye => m_LeftEye;
        [SerializeField] private Transform m_RightEye; public Transform RightEye => m_RightEye;
        [SerializeField] private Transform m_Jaw; public Transform Jaw => m_Jaw;
        #endregion

        #region arm
        [SerializeField] private Transform m_LeftShoulder; public Transform LeftShoulder => m_LeftShoulder;
        [SerializeField] private Transform m_RightShoulder; public Transform RightShoulder => m_RightShoulder;
        [SerializeField] private Transform m_LeftUpperArm; public Transform LeftUpperArm => m_LeftUpperArm;
        [SerializeField] private Transform m_RightUpperArm; public Transform RightUpperArm => m_RightUpperArm;
        [SerializeField] private Transform m_LeftLowerArm; public Transform LeftLowerArm => m_LeftLowerArm;
        [SerializeField] private Transform m_RightLowerArm; public Transform RightLowerArm => m_RightLowerArm;
        [SerializeField] private Transform m_LeftHand; public Transform LeftHand => m_LeftHand;
        [SerializeField] private Transform m_RightHand; public Transform RightHand => m_RightHand;
        #endregion

        #region fingers
        [SerializeField] private Transform m_LeftThumbProximal; public Transform LeftThumbProximal => m_LeftThumbProximal;
        [SerializeField] private Transform m_LeftThumbIntermediate; public Transform LeftThumbIntermediate => m_LeftThumbIntermediate;
        [SerializeField] private Transform m_LeftThumbDistal; public Transform LeftThumbDistal => m_LeftThumbDistal;
        [SerializeField] private Transform m_LeftIndexProximal; public Transform LeftIndexProximal => m_LeftIndexProximal;
        [SerializeField] private Transform m_LeftIndexIntermediate; public Transform LeftIndexIntermediate => m_LeftIndexIntermediate;
        [SerializeField] private Transform m_LeftIndexDistal; public Transform LeftIndexDistal => m_LeftIndexDistal;
        [SerializeField] private Transform m_LeftMiddleProximal; public Transform LeftMiddleProximal => m_LeftMiddleProximal;
        [SerializeField] private Transform m_LeftMiddleIntermediate; public Transform LeftMiddleIntermediate => m_LeftMiddleIntermediate;
        [SerializeField] private Transform m_LeftMiddleDistal; public Transform LeftMiddleDistal => m_LeftMiddleDistal;
        [SerializeField] private Transform m_LeftRingProximal; public Transform LeftRingProximal => m_LeftRingProximal;
        [SerializeField] private Transform m_LeftRingIntermediate; public Transform LeftRingIntermediate => m_LeftRingIntermediate;
        [SerializeField] private Transform m_LeftRingDistal; public Transform LeftRingDistal => m_LeftRingDistal;
        [SerializeField] private Transform m_LeftLittleProximal; public Transform LeftLittleProximal => m_LeftLittleProximal;
        [SerializeField] private Transform m_LeftLittleIntermediate; public Transform LeftLittleIntermediate => m_LeftLittleIntermediate;
        [SerializeField] private Transform m_LeftLittleDistal; public Transform LeftLittleDistal => m_LeftLittleDistal;
        [SerializeField] private Transform m_RightThumbProximal; public Transform RightThumbProximal => m_RightThumbProximal;
        [SerializeField] private Transform m_RightThumbIntermediate; public Transform RightThumbIntermediate => m_RightThumbIntermediate;
        [SerializeField] private Transform m_RightThumbDistal; public Transform RightThumbDistal => m_RightThumbDistal;
        [SerializeField] private Transform m_RightIndexProximal; public Transform RightIndexProximal => m_RightIndexProximal;
        [SerializeField] private Transform m_RightIndexIntermediate; public Transform RightIndexIntermediate => m_RightIndexIntermediate;
        [SerializeField] private Transform m_RightIndexDistal; public Transform RightIndexDistal => m_RightIndexDistal;
        [SerializeField] private Transform m_RightMiddleProximal; public Transform RightMiddleProximal => m_RightMiddleProximal;
        [SerializeField] private Transform m_RightMiddleIntermediate; public Transform RightMiddleIntermediate => m_RightMiddleIntermediate;
        [SerializeField] private Transform m_RightMiddleDistal; public Transform RightMiddleDistal => m_RightMiddleDistal;
        [SerializeField] private Transform m_RightRingProximal; public Transform RightRingProximal => m_RightRingProximal;
        [SerializeField] private Transform m_RightRingIntermediate; public Transform RightRingIntermediate => m_RightRingIntermediate;
        [SerializeField] private Transform m_RightRingDistal; public Transform RightRingDistal => m_RightRingDistal;
        [SerializeField] private Transform m_RightLittleProximal; public Transform RightLittleProximal => m_RightLittleProximal;
        [SerializeField] private Transform m_RightLittleIntermediate; public Transform RightLittleIntermediate => m_RightLittleIntermediate;
        [SerializeField] private Transform m_RightLittleDistal; public Transform RightLittleDistal => m_RightLittleDistal;
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
                    case VrmLib.HumanoidBones.hips: m_Hips = value; break;

                    #region leg
                    case VrmLib.HumanoidBones.leftUpperLeg: m_LeftUpperLeg = value; break;
                    case VrmLib.HumanoidBones.rightUpperLeg: m_RightUpperLeg = value; break;
                    case VrmLib.HumanoidBones.leftLowerLeg: m_LeftLowerLeg = value; break;
                    case VrmLib.HumanoidBones.rightLowerLeg: m_RightLowerLeg = value; break;
                    case VrmLib.HumanoidBones.leftFoot: m_LeftFoot = value; break;
                    case VrmLib.HumanoidBones.rightFoot: m_RightFoot = value; break;
                    case VrmLib.HumanoidBones.leftToes: m_LeftToes = value; break;
                    case VrmLib.HumanoidBones.rightToes: m_RightToes = value; break;
                    #endregion

                    #region spine
                    case VrmLib.HumanoidBones.spine: m_Spine = value; break;
                    case VrmLib.HumanoidBones.chest: m_Chest = value; break;
                    case VrmLib.HumanoidBones.upperChest: m_UpperChest = value; break;
                    case VrmLib.HumanoidBones.neck: m_Neck = value; break;
                    case VrmLib.HumanoidBones.head: m_Head = value; break;
                    case VrmLib.HumanoidBones.leftEye: m_LeftEye = value; break;
                    case VrmLib.HumanoidBones.rightEye: m_RightEye = value; break;
                    case VrmLib.HumanoidBones.jaw: m_Jaw = value; break;
                    #endregion

                    #region arm
                    case VrmLib.HumanoidBones.leftShoulder: m_LeftShoulder = value; break;
                    case VrmLib.HumanoidBones.rightShoulder: m_RightShoulder = value; break;
                    case VrmLib.HumanoidBones.leftUpperArm: m_LeftUpperArm = value; break;
                    case VrmLib.HumanoidBones.rightUpperArm: m_RightUpperArm = value; break;
                    case VrmLib.HumanoidBones.leftLowerArm: m_LeftLowerArm = value; break;
                    case VrmLib.HumanoidBones.rightLowerArm: m_RightLowerArm = value; break;
                    case VrmLib.HumanoidBones.leftHand: m_LeftHand = value; break;
                    case VrmLib.HumanoidBones.rightHand: m_RightHand = value; break;
                    #endregion

                    #region fingers
                    case VrmLib.HumanoidBones.leftThumbProximal: m_LeftThumbProximal = value; break;
                    case VrmLib.HumanoidBones.leftThumbIntermediate: m_LeftThumbIntermediate = value; break;
                    case VrmLib.HumanoidBones.leftThumbDistal: m_LeftThumbDistal = value; break;
                    case VrmLib.HumanoidBones.leftIndexProximal: m_LeftIndexProximal = value; break;
                    case VrmLib.HumanoidBones.leftIndexIntermediate: m_LeftIndexIntermediate = value; break;
                    case VrmLib.HumanoidBones.leftIndexDistal: m_LeftIndexDistal = value; break;
                    case VrmLib.HumanoidBones.leftMiddleProximal: m_LeftMiddleProximal = value; break;
                    case VrmLib.HumanoidBones.leftMiddleIntermediate: m_LeftMiddleIntermediate = value; break;
                    case VrmLib.HumanoidBones.leftMiddleDistal: m_LeftMiddleDistal = value; break;
                    case VrmLib.HumanoidBones.leftRingProximal: m_LeftRingProximal = value; break;
                    case VrmLib.HumanoidBones.leftRingIntermediate: m_LeftRingIntermediate = value; break;
                    case VrmLib.HumanoidBones.leftRingDistal: m_LeftRingDistal = value; break;
                    case VrmLib.HumanoidBones.leftLittleProximal: m_LeftLittleProximal = value; break;
                    case VrmLib.HumanoidBones.leftLittleIntermediate: m_LeftLittleIntermediate = value; break;
                    case VrmLib.HumanoidBones.leftLittleDistal: m_LeftLittleDistal = value; break;
                    case VrmLib.HumanoidBones.rightThumbProximal: m_RightThumbProximal = value; break;
                    case VrmLib.HumanoidBones.rightThumbIntermediate: m_RightThumbIntermediate = value; break;
                    case VrmLib.HumanoidBones.rightThumbDistal: m_RightThumbDistal = value; break;
                    case VrmLib.HumanoidBones.rightIndexProximal: m_RightIndexProximal = value; break;
                    case VrmLib.HumanoidBones.rightIndexIntermediate: m_RightIndexIntermediate = value; break;
                    case VrmLib.HumanoidBones.rightIndexDistal: m_RightIndexDistal = value; break;
                    case VrmLib.HumanoidBones.rightMiddleProximal: m_RightMiddleProximal = value; break;
                    case VrmLib.HumanoidBones.rightMiddleIntermediate: m_RightMiddleIntermediate = value; break;
                    case VrmLib.HumanoidBones.rightMiddleDistal: m_RightMiddleDistal = value; break;
                    case VrmLib.HumanoidBones.rightRingProximal: m_RightRingProximal = value; break;
                    case VrmLib.HumanoidBones.rightRingIntermediate: m_RightRingIntermediate = value; break;
                    case VrmLib.HumanoidBones.rightRingDistal: m_RightRingDistal = value; break;
                    case VrmLib.HumanoidBones.rightLittleProximal: m_RightLittleProximal = value; break;
                    case VrmLib.HumanoidBones.rightLittleIntermediate: m_RightLittleIntermediate = value; break;
                    case VrmLib.HumanoidBones.rightLittleDistal: m_RightLittleDistal = value; break;
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
