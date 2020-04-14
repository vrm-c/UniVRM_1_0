using System;
using UnityEngine;

namespace UniVRM10
{
    public enum SpringColliderTypes
    {
        Sphere,
        Capsule,
    }

    [Serializable]
    public class SpringCollider
    {
        public SpringColliderTypes ColliderTypes;

        /// <summary>bone local position</summary>
        public Vector3 Offset;

        [Range(0, 1.0f)]
        public float Radius;

        /// <summary>bone local position</summary>
        public Vector3 Tail;
    }

    public struct SphereCollider
    {
        public Vector3 Position;
        public float Radius;
    }
}
