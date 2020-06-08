using System.Collections;
using System.Collections.Generic;
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

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
