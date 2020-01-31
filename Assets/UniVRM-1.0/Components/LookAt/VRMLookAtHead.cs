using System;
using UnityEngine;


namespace UniVRM10
{


    /// <summary>
    /// Headボーンローカルで目標物のYaw, Pitchを求めて目線に適用する
    /// 
    /// * VRMLookAtBoneApplyer
    /// * VRMLookAtBlendShapeApplyer
    /// 
    /// </summary>
    [Serializable]
    public class VRMLookAtHead
    {
        [SerializeField]
        public Transform Target;

        [SerializeField]
        public Transform Head;

        public VRMLookAtHead(Animator animator)
        {
            if (animator == null)
            {
                Debug.LogWarning("animator is not found");
                return;
            }

            var head = animator.GetBoneTransform(HumanBodyBones.Head);
            if (head == null)
            {
                Debug.LogWarning("head is not found");
                return;
            }

            Head = head;
        }

        static Matrix4x4 LookAtMatrixFromWorld(Vector3 from, Vector3 target)
        {
            var pos = new Vector4(from.x, from.y, from.z, 1);
            return LookAtMatrix(UnityExtensions.Matrix4x4FromColumns(Vector3.right, Vector3.up, Vector3.forward, pos), target);
        }

        static Matrix4x4 LookAtMatrix(Vector3 up_vector, Vector3 localPosition)
        {
            var z_axis = localPosition.normalized;
            var x_axis = Vector3.Cross(up_vector, z_axis).normalized;
            var y_axis = Vector3.Cross(z_axis, x_axis).normalized;
            return UnityExtensions.Matrix4x4FromColumns(x_axis, y_axis, z_axis, new Vector4(0, 0, 0, 1));
        }

        static Matrix4x4 LookAtMatrix(Matrix4x4 m, Vector3 target)
        {
            return LookAtMatrix(Vector3.up, m.inverse.MultiplyPoint(target));
        }

        public Matrix4x4 YawMatrix
        {
            get
            {
                var yaw = Quaternion.AngleAxis(m_yaw, Vector3.up);
                var m = default(Matrix4x4);
                m.SetTRS(Vector3.zero, yaw, Vector3.one);
                return m;
            }
        }

        [SerializeField, Header("Debug")]
        float m_yaw;
        public float Yaw
        {
            get { return m_yaw; }
        }

        [SerializeField]
        float m_pitch;
        public float Pitch
        {
            get { return m_pitch; }
        }

        public event Action<float, float> YawPitchChanged;
        public void RaiseYawPitchChanged(float yaw, float pitch)
        {
            m_yaw = yaw;
            m_pitch = pitch;
            var handle = YawPitchChanged;
            if (handle != null)
            {
                handle(yaw, pitch);
            }
        }

        public void Update()
        {
            if (Head == null)
            {
                return;
            }

            LookWorldPosition();
        }

        public void LookWorldPosition()
        {
            if (Target == null) return;
            float yaw;
            float pitch;
            LookWorldPosition(Target.position, out yaw, out pitch);
        }

        public void LookWorldPosition(Vector3 targetPosition, out float yaw, out float pitch)
        {
            var localPosition = Head.worldToLocalMatrix.MultiplyPoint(targetPosition);
            Matrix4x4.identity.CalcYawPitch(localPosition, out yaw, out pitch);
            RaiseYawPitchChanged(yaw, pitch);
        }
    }
}
