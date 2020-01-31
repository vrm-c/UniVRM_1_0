﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace VrmLib
{
    struct BoneWeight
    {
        public int boneIndex0;
        public int boneIndex1;
        public int boneIndex2;
        public int boneIndex3;

        public float weight0;
        public float weight1;
        public float weight2;
        public float weight3;
    }

    class MeshBuilder
    {
        public static BoneHeadTail[] Bones = new BoneHeadTail[]
        {
            new BoneHeadTail(HumanoidBones.hips, HumanoidBones.spine, 0.1f, 0.06f),
            new BoneHeadTail(HumanoidBones.spine, HumanoidBones.chest),
            new BoneHeadTail(HumanoidBones.chest, HumanoidBones.neck, 0.1f, 0.06f),
            new BoneHeadTail(HumanoidBones.neck, HumanoidBones.head, 0.03f, 0.03f),
            new BoneHeadTail(HumanoidBones.head, new Vector3(0, 0.1f, 0), 0.1f, 0.1f),

            new BoneHeadTail(HumanoidBones.leftShoulder, HumanoidBones.leftUpperArm),
            new BoneHeadTail(HumanoidBones.leftUpperArm, HumanoidBones.leftLowerArm),
            new BoneHeadTail(HumanoidBones.leftLowerArm, HumanoidBones.leftHand),
            new BoneHeadTail(HumanoidBones.leftHand, new Vector3(-0.1f, 0, 0)),

            new BoneHeadTail(HumanoidBones.leftUpperLeg, HumanoidBones.leftLowerLeg),
            new BoneHeadTail(HumanoidBones.leftLowerLeg, HumanoidBones.leftFoot),
            new BoneHeadTail(HumanoidBones.leftFoot, HumanoidBones.leftToes),
            new BoneHeadTail(HumanoidBones.leftToes, new Vector3(0, 0, 0.1f)),

            new BoneHeadTail(HumanoidBones.rightShoulder, HumanoidBones.rightUpperArm),
            new BoneHeadTail(HumanoidBones.rightUpperArm, HumanoidBones.rightLowerArm),
            new BoneHeadTail(HumanoidBones.rightLowerArm, HumanoidBones.rightHand),
            new BoneHeadTail(HumanoidBones.rightHand, new Vector3(0.1f, 0, 0)),

            new BoneHeadTail(HumanoidBones.rightUpperLeg, HumanoidBones.rightLowerLeg),
            new BoneHeadTail(HumanoidBones.rightLowerLeg, HumanoidBones.rightFoot),
            new BoneHeadTail(HumanoidBones.rightFoot, HumanoidBones.rightToes),
            new BoneHeadTail(HumanoidBones.rightToes, new Vector3(0, 0, 0.1f)),
        };

        public void Build(List<Node> bones)
        {
            foreach (var headTail in Bones)
            {
                var head = bones.FirstOrDefault(x => x.HumanoidBone == headTail.Head);
                if (head != null)
                {
                    Node tail = default(Node);
                    if (headTail.Tail != HumanoidBones.unknown)
                    {
                        tail = bones.FirstOrDefault(x => x.HumanoidBone == headTail.Tail);
                    }

                    if (tail != null)
                    {
                        AddBone(head.SkeletonLocalPosition, tail.SkeletonLocalPosition, bones.IndexOf(head), headTail.XWidth, headTail.ZWidth);
                    }
                    else if(headTail.TailOffset!=Vector3.Zero)
                    {
                        AddBone(head.SkeletonLocalPosition, head.SkeletonLocalPosition + headTail.TailOffset, bones.IndexOf(head), headTail.XWidth, headTail.ZWidth);
                    }
                }
                else
                {
                    Console.Error.WriteLine($"{headTail.Head} not found");
                }
            }
        }

        List<Vector3> m_positioins = new List<Vector3>();
        List<int> m_indices = new List<int>();
        List<BoneWeight> m_boneWeights = new List<BoneWeight>();

        void AddBone(Vector3 head, Vector3 tail, int boneIndex, float xWidth, float zWidth)
        {
            var yaxis = Vector3.Normalize(tail - head);
            Vector3 xaxis;
            Vector3 zaxis;
            if (Vector3.Dot(yaxis, Vector3.UnitZ) >= 1.0f - float.Epsilon)
            {
                // ほぼZ軸
                xaxis = Vector3.UnitX;
                zaxis = -Vector3.UnitY;
            }
            else
            {
                xaxis = Vector3.Normalize(Vector3.Cross(yaxis, Vector3.UnitZ));
                zaxis = Vector3.UnitZ;
            }
            AddBox((head + tail) * 0.5f,
                xaxis * xWidth,
                (tail - head) * 0.5f,
                zaxis * zWidth,
                boneIndex);
        }

        void AddBox(Vector3 center, Vector3 xaxis, Vector3 yaxis, Vector3 zaxis, int boneIndex)
        {
            AddQuad(
                center - yaxis - xaxis - zaxis,
                center - yaxis + xaxis - zaxis,
                center - yaxis + xaxis + zaxis,
                center - yaxis - xaxis + zaxis,
                boneIndex);
            AddQuad(
                center + yaxis - xaxis - zaxis,
                center + yaxis + xaxis - zaxis,
                center + yaxis + xaxis + zaxis,
                center + yaxis - xaxis + zaxis,
                boneIndex, true);
            AddQuad(
                center - xaxis - yaxis - zaxis,
                center - xaxis + yaxis - zaxis,
                center - xaxis + yaxis + zaxis,
                center - xaxis - yaxis + zaxis,
                boneIndex, true);
            AddQuad(
                center + xaxis - yaxis - zaxis,
                center + xaxis + yaxis - zaxis,
                center + xaxis + yaxis + zaxis,
                center + xaxis - yaxis + zaxis,
                boneIndex);
            AddQuad(
                center - zaxis - xaxis - yaxis,
                center - zaxis + xaxis - yaxis,
                center - zaxis + xaxis + yaxis,
                center - zaxis - xaxis + yaxis,
                boneIndex, true);
            AddQuad(
                center + zaxis - xaxis - yaxis,
                center + zaxis + xaxis - yaxis,
                center + zaxis + xaxis + yaxis,
                center + zaxis - xaxis + yaxis,
                boneIndex);
        }

        void AddQuad(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, int boneIndex, bool reverse = false)
        {
            var i = m_positioins.Count;
            if(float.IsNaN(v0.X) || float.IsNaN(v0.Y) || float.IsNaN(v0.Z))
            {
                throw new Exception();
            }
            m_positioins.Add(v0);

            if(float.IsNaN(v1.X) || float.IsNaN(v1.Y) || float.IsNaN(v1.Z))
            {
                throw new Exception();
            }
            m_positioins.Add(v1);

            if(float.IsNaN(v2.X) || float.IsNaN(v2.Y) || float.IsNaN(v2.Z))
            {
                throw new Exception();
            }
            m_positioins.Add(v2);

            if(float.IsNaN(v3.X) || float.IsNaN(v3.Y) || float.IsNaN(v3.Z))
            {
                throw new Exception();
            }
            m_positioins.Add(v3);

            var bw = new BoneWeight
            {
                boneIndex0 = boneIndex,
                weight0 = 1.0f,
            };
            m_boneWeights.Add(bw);
            m_boneWeights.Add(bw);
            m_boneWeights.Add(bw);
            m_boneWeights.Add(bw);

            if (reverse)
            {
                m_indices.Add(i + 3);
                m_indices.Add(i + 2);
                m_indices.Add(i + 1);

                m_indices.Add(i + 1);
                m_indices.Add(i);
                m_indices.Add(i + 3);
            }
            else
            {
                m_indices.Add(i);
                m_indices.Add(i + 1);
                m_indices.Add(i + 2);

                m_indices.Add(i + 2);
                m_indices.Add(i + 3);
                m_indices.Add(i);
            }
        }

        public Mesh CreateMesh()
        {
            if(m_positioins.Any(x => float.IsNaN(x.X) || float.IsNaN(x.Y) || float.IsNaN(x.Z)))
            {
                throw new Exception();
            }

            var mesh = new Mesh();
            mesh.VertexBuffer = new VertexBuffer();
            mesh.VertexBuffer.Add(VertexBuffer.PositionKey,
                m_positioins.ToArray());

            mesh.VertexBuffer.Add(VertexBuffer.JointKey,
                m_boneWeights.Select(x => new SkinJoints(
                    (ushort)x.boneIndex0,
                    (ushort)x.boneIndex1,
                    (ushort)x.boneIndex2,
                    (ushort)x.boneIndex3)).ToArray());

            mesh.VertexBuffer.Add(VertexBuffer.WeightKey,
                m_boneWeights.Select(x => new Vector4(
                    x.weight0,
                    x.weight1,
                    x.weight2,
                    x.weight3
                )).ToArray());

            mesh.IndexBuffer = BufferAccessor.Create(m_indices.ToArray());

            mesh.Submeshes.Add(new Submesh(0, mesh.IndexBuffer.Count, null));

            return mesh;
        }
    }

    struct BoneHeadTail
    {
        public HumanoidBones Head;
        public HumanoidBones Tail;
        public Vector3 TailOffset;
        public float XWidth;
        public float ZWidth;

        public BoneHeadTail(HumanoidBones head, HumanoidBones tail, float xWidth = 0.05f, float zWidth = 0.05f)
        {
            Head = head;
            Tail = tail;
            TailOffset = Vector3.Zero;
            XWidth = xWidth;
            ZWidth = zWidth;
        }

        public BoneHeadTail(HumanoidBones head, Vector3 tailOffset, float xWidth = 0.05f, float zWidth = 0.05f)
        {
            Head = head;
            Tail = HumanoidBones.unknown;
            TailOffset = tailOffset;
            XWidth = xWidth;
            ZWidth = zWidth;
        }
    }
}
