using System.Collections.Generic;
using GltfFormat;
using VrmLib;

namespace GltfSerializationAdapter
{
    public static class AnimationFromGltf
    {
        public static CurveSampler FromGltf(GltfSerialization.GltfStorage storage, int inIndex, int outIndex)
        {
            return new CurveSampler
            {
                In = storage.AccessorFromGltf(inIndex),
                Out = storage.AccessorFromGltf(outIndex),
            };
        }

        public static void AddCurve(this Animation self, GltfSerialization.GltfStorage storage, Node node, AnimationPathType path, GltfAnimationSampler sampler)
        {
            var nodeAnimation = self.GetOrCreateNodeAnimation(node);
            nodeAnimation.Curves[path] = FromGltf(storage, sampler.input, sampler.output);

            self.UpdateChannelsAndLastTime();
        }

        public static Animation FromGltf(this GltfAnimation src,
            GltfSerialization.GltfStorage storage, List<Node> nodes)
        {
            var animation = new Animation(src.name);

            foreach (var ch in src.channels)
            {
                var sampler = src.samplers[ch.sampler];
                var target = ch.target;
                var node = nodes[target.node];
                animation.AddCurve(storage, node, (AnimationPathType)target.path, sampler);
            }

            return animation;
        }
    }
}
