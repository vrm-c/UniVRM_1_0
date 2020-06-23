using System.Linq;
using GltfFormat;
using VrmLib;


namespace GltfSerializationAdapter
{
    public static class VrmLookAtFromGltf
    {
        public static LookAtRangeMap FromGltf(this VrmDegreeMap map)
        {
            var self = new LookAtRangeMap
            {
                InputMaxValue = map.xRange,
                OutputScaling = map.yRange,
            };
            if (map.curve != null)
            {
                self.Curve = map.curve.ToArray();
            }
            return self;
        }

        public static LookAt LookAtFromGltf(this VrmFirstPerson fp)
        {
            return new LookAt
            {
                OffsetFromHeadBone = fp.firstPersonBoneOffset,
                LookAtType = (LookAtType)fp.lookAtTypeName,
                HorizontalInner = fp.lookAtHorizontalInner.FromGltf(),
                HorizontalOuter = fp.lookAtHorizontalOuter.FromGltf(),
                VerticalUp = fp.lookAtVerticalUp.FromGltf(),
                VerticalDown = fp.lookAtVerticalDown.FromGltf(),
            };
        }
    }
}
