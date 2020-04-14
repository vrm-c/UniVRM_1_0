
using System.Collections.Generic;
using UnityEngine;

namespace UniVRM10
{
    /// <summary>
    /// 
    /// original from
    /// 
    /// http://rocketjump.skr.jp/unity3d/109/
    /// 
    /// </summary>
    public class VRMSpringBoneLogic
    {
        Transform m_transform;
        public Transform Head
        {
            get { return m_transform; }
        }

        public Vector3 Tail
        {
            get { return m_transform.localToWorldMatrix.MultiplyPoint(m_boneAxis * m_length); }
        }

        float m_length;
        Vector3 m_currentTail;
        Vector3 m_prevTail;
        Vector3 m_localDir;
        Quaternion m_localRotation;
        public Quaternion LocalRotation
        {
            get { return m_localRotation; }
        }
        public Vector3 m_boneAxis;

        public float Radius { get; set; }

        public VRMSpringBoneLogic(Transform center, Transform transform, Vector3 localChildPosition)
        {
            m_transform = transform;
            var worldChildPosition = m_transform.TransformPoint(localChildPosition);
            m_currentTail = center != null
                ? center.InverseTransformPoint(worldChildPosition)
                : worldChildPosition
                ;
            m_prevTail = m_currentTail;
            m_localRotation = transform.localRotation;
            m_boneAxis = localChildPosition.normalized;
            m_length = localChildPosition.magnitude;
        }

        Quaternion ParentRotation
        {
            get
            {
                return m_transform.parent != null
                    ? m_transform.parent.rotation
                    : Quaternion.identity
                    ;
            }
        }

        public void Update(Transform center,
            float stiffnessForce, float dragForce, Vector3 external,
            List<SphereCollider> colliders)
        {
            var currentTail = center != null
                ? center.TransformPoint(m_currentTail)
                : m_currentTail
                ;
            var prevTail = center != null
                ? center.TransformPoint(m_prevTail)
                : m_prevTail
                ;

            // verlet積分で次の位置を計算
            var nextTail = currentTail
                + (currentTail - prevTail) * (1.0f - dragForce) // 前フレームの移動を継続する(減衰もあるよ)
                + ParentRotation * m_localRotation * m_boneAxis * stiffnessForce // 親の回転による子ボーンの移動目標
                + external // 外力による移動量
                ;

            // 長さをboneLengthに強制
            nextTail = m_transform.position + (nextTail - m_transform.position).normalized * m_length;

            // Collisionで移動
            nextTail = Collision(colliders, nextTail);

            m_prevTail = center != null
                ? center.InverseTransformPoint(currentTail)
                : currentTail
                ;
            m_currentTail = center != null
                ? center.InverseTransformPoint(nextTail)
                : nextTail
                ;

            //回転を適用
            Head.rotation = ApplyRotation(nextTail);
        }

        protected virtual Quaternion ApplyRotation(Vector3 nextTail)
        {
            var rotation = ParentRotation * m_localRotation;
            return Quaternion.FromToRotation(rotation * m_boneAxis,
                nextTail - m_transform.position) * rotation;
        }

        protected virtual Vector3 Collision(List<SphereCollider> colliders, Vector3 nextTail)
        {
            foreach (var collider in colliders)
            {
                var r = Radius + collider.Radius;
                if (Vector3.SqrMagnitude(nextTail - collider.Position) <= (r * r))
                {
                    // ヒット。Colliderの半径方向に押し出す
                    var normal = (nextTail - collider.Position).normalized;
                    var posFromCollider = collider.Position + normal * (Radius + collider.Radius);
                    // 長さをboneLengthに強制
                    nextTail = m_transform.position + (posFromCollider - m_transform.position).normalized * m_length;
                }
            }
            return nextTail;
        }

        public void DrawGizmo(Transform center, float radius, Color color)
        {
            var currentTail = center != null
                ? center.TransformPoint(m_currentTail)
                : m_currentTail
                ;
            var prevTail = center != null
                ? center.TransformPoint(m_prevTail)
                : m_prevTail
                ;

            Gizmos.color = Color.gray;
            Gizmos.DrawLine(currentTail, prevTail);
            Gizmos.DrawWireSphere(prevTail, radius);

            Gizmos.color = color;
            Gizmos.DrawLine(currentTail, m_transform.position);
            Gizmos.DrawWireSphere(currentTail, radius);
        }
    }
}
