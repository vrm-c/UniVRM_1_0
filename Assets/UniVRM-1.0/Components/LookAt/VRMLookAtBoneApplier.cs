using UnityEngine;


namespace UniVRM10
{
    [DisallowMultipleComponent]
    public class VRMLookAtBoneApplier : MonoBehaviour, ILookAtApplier
    {
        public bool DrawGizmo = false;

        [SerializeField]
        public OffsetOnTransform LeftEye;

        [SerializeField]
        public OffsetOnTransform RightEye;

        [SerializeField, Header("Degree Mapping")]
        public CurveMapper HorizontalOuter = new CurveMapper(90.0f, 10.0f);

        [SerializeField]
        public CurveMapper HorizontalInner = new CurveMapper(90.0f, 10.0f);

        [SerializeField]
        public CurveMapper VerticalDown = new CurveMapper(90.0f, 10.0f);

        [SerializeField]
        public CurveMapper VerticalUp = new CurveMapper(90.0f, 10.0f);


        private void OnValidate()
        {
            HorizontalInner.OnValidate();
            HorizontalOuter.OnValidate();
            VerticalUp.OnValidate();
            VerticalDown.OnValidate();
        }

        void Start()
        {
            LeftEye.Setup();
            RightEye.Setup();
        }

        #region Gizmo
        static void DrawMatrix(Matrix4x4 m, float size)
        {
            Gizmos.matrix = m;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Vector3.zero, Vector3.right * size);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Vector3.zero, Vector3.up * size);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Vector3.zero, Vector3.forward * size);
        }

        const float SIZE = 0.5f;

        private void OnDrawGizmos()
        {
            if (DrawGizmo)
            {
                if (LeftEye.Transform != null & RightEye.Transform != null)
                {
                    DrawMatrix(LeftEye.WorldMatrix, SIZE);
                    DrawMatrix(RightEye.WorldMatrix, SIZE);
                }
            }
        }
        #endregion

        void ILookAtApplier.ApplyRotations(VRMBlendShapeProxy proxy, float yaw, float pitch)
        {
            // horizontal
            float leftYaw, rightYaw;
            if (yaw < 0)
            {
                leftYaw = -HorizontalOuter.Map(-yaw);
                rightYaw = -HorizontalInner.Map(-yaw);
            }
            else
            {
                rightYaw = HorizontalOuter.Map(yaw);
                leftYaw = HorizontalInner.Map(yaw);
            }

            // vertical
            if (pitch < 0)
            {
                pitch = -VerticalDown.Map(-pitch);
            }
            else
            {
                pitch = VerticalUp.Map(pitch);
            }

            // Apply
            if (LeftEye.Transform != null && RightEye.Transform != null)
            {
                // 目に値を適用する
                LeftEye.Transform.rotation = LeftEye.InitialWorldMatrix.ExtractRotation() * Matrix4x4.identity.YawPitchRotation(leftYaw, pitch);
                RightEye.Transform.rotation = RightEye.InitialWorldMatrix.ExtractRotation() * Matrix4x4.identity.YawPitchRotation(rightYaw, pitch);
            }
        }
    }
}
