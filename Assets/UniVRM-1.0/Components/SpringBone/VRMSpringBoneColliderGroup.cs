using System;
using UnityEngine;


namespace UniVRM10
{
#if UNITY_5_5_OR_NEWER
    [DefaultExecutionOrder(11001)]
#endif
    public class VRMSpringBoneColliderGroup : MonoBehaviour
    {
        [SerializeField]
        public SpringBoneCollider[] Colliders = new SpringBoneCollider[]{
            new SpringBoneCollider
            {
                ColliderTypes = SpringBoneColliderTypes.Capsule,
                Radius=0.1f
            }
        };

        [SerializeField]
        Color m_gizmoColor = Color.magenta;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = m_gizmoColor;
            Matrix4x4 mat = transform.localToWorldMatrix;
            Gizmos.matrix = mat * Matrix4x4.Scale(new Vector3(
                1.0f / transform.lossyScale.x,
                1.0f / transform.lossyScale.y,
                1.0f / transform.lossyScale.z
                ));
            foreach (var y in Colliders)
            {
                switch (y.ColliderTypes)
                {
                    case SpringBoneColliderTypes.Sphere:
                        Gizmos.DrawWireSphere(y.Offset, y.Radius);
                        break;

                    case SpringBoneColliderTypes.Capsule:
                        Gizmos.DrawWireSphere(y.Offset, y.Radius);
                        Gizmos.DrawWireSphere(y.Tail, y.Radius);
                        Gizmos.DrawLine(y.Offset, y.Tail);
                        break;
                }
            }
        }
    }
}
