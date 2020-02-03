using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace VrmLib
{
    public struct VrmSpringBoneColliderSphere
    {
        public readonly Vector3 Offset;
        public readonly float Radius;

        public VrmSpringBoneColliderSphere(Vector3 offset, float radius)
        {
            Offset = offset;
            Radius = radius;
        }
    }

    public class SpringBoneColliderGroup
    {
        public readonly Node Node;

        public readonly List<VrmSpringBoneColliderSphere> Colliders;

        public SpringBoneColliderGroup(Node node, IEnumerable<VrmSpringBoneColliderSphere> colliders)
        {
            Node = node;
            Colliders = colliders.ToList();
        }
    }

    public class SpringBone
    {
        public readonly List<Node> Bones = new List<Node>();
        public Node Origin;

        public readonly List<SpringBoneColliderGroup> Colliders = new List<SpringBoneColliderGroup>();

        public string Comment;

        public float DragForce;

        public Vector3 GravityDir;

        public float GravityPower;

        public float HitRadius;

        public float Stiffness;
    }

    public class SpringBoneManager
    {
        public readonly List<SpringBone> Springs = new List<SpringBone>();
        public readonly List<SpringBoneColliderGroup> Colliders = new List<SpringBoneColliderGroup>();
    }
}