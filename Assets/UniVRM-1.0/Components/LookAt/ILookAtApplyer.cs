#pragma warning disable 0414, 0649


namespace UniVRM10
{
    interface ILookAtApplier
    {
        void ApplyRotations(VRMBlendShapeProxy proxy, float yaw, float pitch);
    }
}
