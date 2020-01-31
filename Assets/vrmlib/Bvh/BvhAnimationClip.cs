using System;
using System.Collections.Generic;
using System.Numerics;

namespace VrmLib.Bvh
{
    public static class BvhAnimation
    {
        class CurveSet
        {
            BvhNode Node;
            Func<float, float, float, Quaternion> EulerToRotation;
            public CurveSet(BvhNode node)
            {
                Node = node;
            }

            public ChannelCurve PositionX;
            public ChannelCurve PositionY;
            public ChannelCurve PositionZ;
            public Vector3 GetPosition(int i)
            {
                return new Vector3(
                    PositionX.Keys[i],
                    PositionY.Keys[i],
                    PositionZ.Keys[i]);
            }

            public ChannelCurve RotationX;
            public ChannelCurve RotationY;
            public ChannelCurve RotationZ;
            public Quaternion GetRotation(int i)
            {
                if (EulerToRotation == null)
                {
                    EulerToRotation = Node.GetEulerToRotation();
                }
                return EulerToRotation(
                    RotationX.Keys[i],
                    RotationY.Keys[i],
                    RotationZ.Keys[i]
                    );
            }
        }
    }
}
