using System;
using VrmLib;

namespace Vrm10
{
    public static class LookAtAdapter
    {
        public static LookAtRangeMap FromGltf(this VrmProtobuf.LookAtRangeMap map)
        {
            return new LookAtRangeMap
            {
                InputMaxValue = map.InputMaxValue.Value,
                OutputScaling = map.OutputScale.Value,
            };
        }

        public static LookAtType FromGltf(this VrmProtobuf.LookAt.Types.LookAtType src)
        {
            switch (src)
            {
                case VrmProtobuf.LookAt.Types.LookAtType.Bone: return LookAtType.Bone;
                case VrmProtobuf.LookAt.Types.LookAtType.BlendShape: return LookAtType.BlendShape;
            }

            throw new NotImplementedException();
        }

        public static LookAt FromGltf(this VrmProtobuf.LookAt src)
        {
            return new LookAt
            {
                OffsetFromHeadBone = src.OffsetFromHeadBone.ToVector3(),
                LookAtType = src.LookAtType.FromGltf(),
                HorizontalInner = src.LookAtHorizontalInner.FromGltf(),
                HorizontalOuter = src.LookAtHorizontalOuter.FromGltf(),
                VerticalUp = src.LookAtVerticalUp.FromGltf(),
                VerticalDown = src.LookAtVerticalDown.FromGltf(),
            };
        }

        public static VrmProtobuf.LookAtRangeMap ToGltf(this LookAtRangeMap map)
        {
            return new VrmProtobuf.LookAtRangeMap
            {
                InputMaxValue = map.InputMaxValue,
                OutputScale = map.OutputScaling,
            };
        }

        public static VrmProtobuf.LookAt ToGltf(this LookAt lookAt)
        {
            var dst = new VrmProtobuf.LookAt
            {
                LookAtType = (VrmProtobuf.LookAt.Types.LookAtType)lookAt.LookAtType,
                LookAtHorizontalInner = lookAt.HorizontalInner.ToGltf(),
                LookAtHorizontalOuter = lookAt.HorizontalOuter.ToGltf(),
                LookAtVerticalUp = lookAt.VerticalUp.ToGltf(),
                LookAtVerticalDown = lookAt.VerticalDown.ToGltf(),
            };
            dst.OffsetFromHeadBone.Add(lookAt.OffsetFromHeadBone.X);
            dst.OffsetFromHeadBone.Add(lookAt.OffsetFromHeadBone.Y);
            dst.OffsetFromHeadBone.Add(lookAt.OffsetFromHeadBone.Z);
            return dst;
        }
    }
}