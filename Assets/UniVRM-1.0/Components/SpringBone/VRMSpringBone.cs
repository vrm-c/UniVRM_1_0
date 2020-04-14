using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniVRM10
{
    /// <summary>
    /// The base algorithm is http://rocketjump.skr.jp/unity3d/109/ of @ricopin416
    /// DefaultExecutionOrder(11000) means calculate springbone after FinalIK( VRIK )
    /// </summary>
    #if UNITY_5_5_OR_NEWER
    [DefaultExecutionOrder(11000)]
    #endif
    public class VRMSpringBone : MonoBehaviour
    {
        [SerializeField]
        public string m_comment;

        [SerializeField, Header("Gizmo")]
        bool m_drawGizmo;

        [SerializeField]
        Color m_gizmoColor = Color.yellow;

        [SerializeField, Range(0, 4), Header("Settings")]
        public float m_stiffnessForce = 1.0f;

        [SerializeField, Range(0, 2)]
        public float m_gravityPower = 0;

        [SerializeField]
        public Vector3 m_gravityDir = new Vector3(0, -1.0f, 0);

        [SerializeField, Range(0, 1)]
        public float m_dragForce = 0.4f;

        [SerializeField]
        public Transform m_center;

        [SerializeField]
        public List<Transform> RootBones = new List<Transform>();
        Dictionary<Transform, Quaternion> m_initialLocalRotationMap;

        [SerializeField, Range(0, 0.5f), Header("Collider")]
        public float m_hitRadius = 0.02f;

        [SerializeField]
        public VRMSpringBoneColliderGroup[] ColliderGroups;

        List<VRMSpringBoneLogic> m_verlet = new List<VRMSpringBoneLogic>();

        void Awake()
        {
            Setup();
        }

        [ContextMenu("Reset bones")]
        public void Setup(bool force=false)
        {
            if (RootBones != null)
            {
                if (force || m_initialLocalRotationMap == null)
                {
                    m_initialLocalRotationMap = new Dictionary<Transform, Quaternion>();
                }
                else
                {
                    foreach(var kv in m_initialLocalRotationMap)
                    {
                        kv.Key.localRotation = kv.Value;
                    }
                    m_initialLocalRotationMap.Clear();
                }
                m_verlet.Clear();

                foreach (var go in RootBones)
                {
                    if (go != null)
                    {
                        foreach(var x in go.transform.Traverse())
                        {
                            m_initialLocalRotationMap[x] = x.localRotation;
                        }

                        SetupRecursive(m_center, go);
                    }
                }
            }
        }

        public void SetLocalRotationsIdentity()
        {
            foreach (var verlet in m_verlet)
            {
                verlet.Head.localRotation = Quaternion.identity;
            }
        }

        void SetupRecursive(Transform center, Transform parent)
        {
            if (parent.childCount == 0)
            {
                var delta = parent.position - parent.parent.position;
                var childPosition = parent.position + delta.normalized * 0.07f;
                m_verlet.Add(new VRMSpringBoneLogic(center, parent, parent.worldToLocalMatrix.MultiplyPoint(childPosition)));
            }
            else
            {
                var firstChild = parent.GetChild(0);
                var localPosition = firstChild.localPosition;
                var scale = firstChild.lossyScale;
                m_verlet.Add(new VRMSpringBoneLogic(center, parent,
                    new Vector3(
                        localPosition.x * scale.x,
                        localPosition.y * scale.y,
                        localPosition.z * scale.z
                        )))
                    ;
            }

            foreach (Transform child in parent)
            {
                SetupRecursive(center, child);
            }
        }

        List<SphereCollider> m_colliderList = new List<SphereCollider>();
        void LateUpdate()
        {
            if (m_verlet == null || m_verlet.Count == 0)
            {
                if (RootBones == null)
                {
                    return;
                }

                Setup();
            }

            m_colliderList.Clear();
            if (ColliderGroups != null)
            {
                foreach (var group in ColliderGroups)
                {
                    if (group != null)
                    {
                        foreach (var collider in group.Colliders)
                        {
                            m_colliderList.Add(new SphereCollider
                            {
                                Position = group.transform.TransformPoint(collider.Offset),
                                Radius = collider.Radius,
                            });
                        }
                    }
                }
            }

            var stiffness = m_stiffnessForce * Time.deltaTime;
            var external = m_gravityDir * (m_gravityPower * Time.deltaTime);

            foreach (var verlet in m_verlet)
            {
                verlet.Radius = m_hitRadius;
                verlet.Update(m_center,
                    stiffness,
                    m_dragForce,
                    external,
                    m_colliderList
                    );
            }
        }

        private void OnDrawGizmos()
        {
            if (m_drawGizmo)
            {
                foreach (var verlet in m_verlet)
                {
                    verlet.DrawGizmo(m_center, m_hitRadius, m_gizmoColor);
                }
            }
        }
    }
}
