using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniVRM10
{
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

        public void Load(Dictionary<VrmLib.Node, GameObject> nodes)
        {
            foreach (var kv in nodes)
            {
                switch (kv.Key.HumanoidBone)
                {
                    case VrmLib.HumanoidBones.hips: Hips = kv.Value.transform; break;

                    #region leg
                    case VrmLib.HumanoidBones.leftUpperLeg: LeftUpperLeg = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightUpperLeg: RightUpperLeg = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.leftLowerLeg: LeftLowerLeg = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightLowerLeg: RightLowerLeg = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.leftFoot: LeftFoot = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightFoot: RightFoot = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.leftToes: LeftToes = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightToes: RightToes = kv.Value.transform; break;
                    #endregion

                    #region spine
                    case VrmLib.HumanoidBones.spine: Spine = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.chest: Chest = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.upperChest: UpperChest = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.neck: Neck = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.head: Head = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.leftEye: LeftEye = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightEye: RightEye = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.jaw: Jaw = kv.Value.transform; break;
                    #endregion

                    #region arm
                    case VrmLib.HumanoidBones.leftShoulder: LeftShoulder = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightShoulder: RightShoulder = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.leftUpperArm: LeftUpperArm = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightUpperArm: RightUpperArm = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.leftLowerArm: LeftLowerArm = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightLowerArm: RightLowerArm = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.leftHand: LeftHand = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightHand: RightHand = kv.Value.transform; break;
                    #endregion

                    #region fingers
                    case VrmLib.HumanoidBones.leftThumbProximal: LeftThumbProximal = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.leftThumbIntermediate: LeftThumbIntermediate = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.leftThumbDistal: LeftThumbDistal = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.leftIndexProximal: LeftIndexProximal = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.leftIndexIntermediate: LeftIndexIntermediate = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.leftIndexDistal: LeftIndexDistal = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.leftMiddleProximal: LeftMiddleProximal = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.leftMiddleIntermediate: LeftMiddleIntermediate = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.leftMiddleDistal: LeftMiddleDistal = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.leftRingProximal: LeftRingProximal = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.leftRingIntermediate: LeftRingIntermediate = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.leftRingDistal: LeftRingDistal = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.leftLittleProximal: LeftLittleProximal = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.leftLittleIntermediate: LeftLittleIntermediate = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.leftLittleDistal: LeftLittleDistal = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightThumbProximal: RightThumbProximal = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightThumbIntermediate: RightThumbIntermediate = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightThumbDistal: RightThumbDistal = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightIndexProximal: RightIndexProximal = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightIndexIntermediate: RightIndexIntermediate = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightIndexDistal: RightIndexDistal = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightMiddleProximal: RightMiddleProximal = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightMiddleIntermediate: RightMiddleIntermediate = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightMiddleDistal: RightMiddleDistal = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightRingProximal: RightRingProximal = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightRingIntermediate: RightRingIntermediate = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightRingDistal: RightRingDistal = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightLittleProximal: RightLittleProximal = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightLittleIntermediate: RightLittleIntermediate = kv.Value.transform; break;
                    case VrmLib.HumanoidBones.rightLittleDistal: RightLittleDistal = kv.Value.transform; break;
                        #endregion
                }
            }
        }

        public Avatar CreateAvatar()
        {
            return HumanoidLoader.LoadHumanoidAvatar(transform, BoneMap);
        }
    }
}
