#pragma warning disable 0414, 0649
using UnityEngine;


namespace UniVRM10
{
    [DisallowMultipleComponent]
    public class VRMLookAtBlendShapeApplier : MonoBehaviour, ILookAtApplier
    {
        public bool DrawGizmo = true;

        [SerializeField, Header("Degree Mapping")]
        public CurveMapper HorizontalOuter = new CurveMapper(90.0f, 1.0f);

        [SerializeField]
        public CurveMapper VerticalDown = new CurveMapper(90.0f, 1.0f);

        [SerializeField]
        public CurveMapper VerticalUp = new CurveMapper(90.0f, 1.0f);


        void ILookAtApplier.ApplyRotations(VRMBlendShapeProxy proxy, float yaw, float pitch)
        {
#pragma warning disable 0618
            if (yaw < 0)
            {
                // Left
                proxy.SetValue(VrmLib.BlendShapePreset.LookRight, 0); // clear first
                proxy.SetValue(VrmLib.BlendShapePreset.LookLeft, Mathf.Clamp(HorizontalOuter.Map(-yaw), 0, 1.0f));
            }
            else
            {
                // Right
                proxy.SetValue(VrmLib.BlendShapePreset.LookLeft, 0); // clear first
                proxy.SetValue(VrmLib.BlendShapePreset.LookRight, Mathf.Clamp(HorizontalOuter.Map(yaw), 0, 1.0f));
            }

            if (pitch < 0)
            {
                // Down
                proxy.SetValue(VrmLib.BlendShapePreset.LookUp, 0); // clear first
                proxy.SetValue(VrmLib.BlendShapePreset.LookDown, Mathf.Clamp(VerticalDown.Map(-pitch), 0, 1.0f));
            }
            else
            {
                // Up
                proxy.SetValue(VrmLib.BlendShapePreset.LookDown, 0); // clear first
                proxy.SetValue(VrmLib.BlendShapePreset.LookUp, Mathf.Clamp(VerticalUp.Map(pitch), 0, 1.0f));
            }
#pragma warning restore 0618
        }
    }
}
