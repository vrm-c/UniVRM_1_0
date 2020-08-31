using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using VrmLib;


namespace Vrm10
{
    public class Vrm10Storage : IVrmStorage
    {
        public ArraySegment<Byte> OriginalJson { get; private set; }
        public VrmProtobuf.glTF Gltf
        {
            get;
            private set;
        }

        public readonly List<ArrayByteBuffer> Buffers;

        public Vrm10Storage()
        {
            Gltf = new VrmProtobuf.glTF();
            Buffers = new List<ArrayByteBuffer>()
            {
                new ArrayByteBuffer()
            };
        }

        public Vrm10Storage(ArraySegment<byte> json, Memory<byte> bin)
        {
            OriginalJson = json;
            var parserSettings = Google.Protobuf.JsonParser.Settings.Default;
            var jsonString = Encoding.UTF8.GetString(json.Array, json.Offset, json.Count);
            var deserialized = new Google.Protobuf.JsonParser(parserSettings).Parse<VrmProtobuf.glTF>(jsonString);
            Gltf = deserialized;
            var array = bin.ToArray();
            Buffers = new List<ArrayByteBuffer>()
            {
                new ArrayByteBuffer(array, bin.Length)
            };
        }

        public void Reserve(int bytesLength)
        {
            Buffers[0].ExtendCapacity(bytesLength);
        }

        public int AppendToBuffer(int bufferIndex, Memory<byte> segment, int stride)
        {
            Buffers[bufferIndex].Extend(segment, stride, out int offset, out int length);
            var viewIndex = Gltf.BufferViews.Count;
            Gltf.BufferViews.Add(new VrmProtobuf.BufferView
            {
                Buffer = 0,
                ByteOffset = offset,
                ByteLength = length,
            });
            return viewIndex;
        }

        static Memory<byte> RestoreSparseAccessorUInt16<T>(Memory<byte> bytes, int accessorCount, Memory<byte> indicesBytes, Memory<byte> valuesBytes)
            where T : struct
        {
            var stride = Marshal.SizeOf(typeof(T));
            if (bytes.IsEmpty)
            {
                bytes = new byte[accessorCount * stride].AsMemory();
            }
            var dst = MemoryMarshal.Cast<byte, T>(bytes.Span);

            var indices = MemoryMarshal.Cast<byte, UInt16>(indicesBytes.Span);
            var values = MemoryMarshal.Cast<byte, T>(valuesBytes.Span);

            for (int i = 0; i < indices.Length; ++i)
            {
                var index = indices[i];
                var value = values[i];
                dst[index] = value;
            }

            return bytes;
        }

        static Memory<byte> RestoreSparseAccessorUInt32<T>(Memory<byte> bytes, int accessorCount, Memory<byte> indicesBytes, Memory<byte> valuesBytes)
            where T : struct
        {
            var stride = Marshal.SizeOf(typeof(T));
            if (bytes.IsEmpty)
            {
                bytes = new byte[accessorCount * stride].AsMemory();
            }
            var dst = MemoryMarshal.Cast<byte, T>(bytes.Span);

            var indices = MemoryMarshal.Cast<byte, Int32>(indicesBytes.Span);
            var values = MemoryMarshal.Cast<byte, T>(valuesBytes.Span);

            for (int i = 0; i < indices.Length; ++i)
            {
                var index = indices[i];
                var value = values[i];
                dst[index] = value;
            }

            return bytes;
        }

        public Memory<byte> GetAccessorBytes(int accessorIndex)
        {
            var accessor = Gltf.Accessors[accessorIndex];
            var sparse = accessor.Sparse;

            Memory<byte> bytes = default(Memory<byte>);

            if (accessor.BufferView.TryGetValidIndex(Gltf.BufferViews.Count, out int bufferViewIndex))
            {
                var view = Gltf.BufferViews[bufferViewIndex];
                if (view.Buffer.TryGetValidIndex(Gltf.Buffers.Count, out int bufferIndex))
                {
                    var buffer = Buffers[bufferIndex];
                    var bin = buffer.Bytes;
                    var byteSize = ((AccessorValueType)accessor.ComponentType).ByteSize() * accessor.Type.TypeCount() * accessor.Count;
                    bytes = bin.Slice(view.ByteOffset.GetValueOrDefault(), view.ByteLength.Value).Slice(accessor.ByteOffset.GetValueOrDefault(), byteSize.Value);
                }
            }

            if (sparse != null)
            {
                if (!sparse.Indices.BufferView.TryGetValidIndex(Gltf.BufferViews.Count, out int sparseIndicesBufferViewIndex))
                {
                    throw new Exception();
                }
                var sparseIndexView = Gltf.BufferViews[sparseIndicesBufferViewIndex];
                var sparseIndexBin = GetBufferBytes(sparseIndexView);
                var sparseIndexBytes = sparseIndexBin
                    .Slice(sparseIndexView.ByteOffset.GetValueOrDefault(), sparseIndexView.ByteLength.Value)
                    .Slice(sparse.Indices.ByteOffset.GetValueOrDefault(), ((AccessorValueType)sparse.Indices.ComponentType).ByteSize() * sparse.Count.Value)
                    ;

                if (!sparse.Values.BufferView.TryGetValidIndex(Gltf.BufferViews.Count, out int sparseValulesBufferViewIndex))
                {
                    throw new Exception();
                }
                var sparseValueView = Gltf.BufferViews[sparseValulesBufferViewIndex];
                var sparseValueBin = GetBufferBytes(sparseValueView);
                var sparseValueBytes = sparseValueBin
                    .Slice(sparseValueView.ByteOffset.GetValueOrDefault(), sparseValueView.ByteLength.Value)
                    .Slice(sparse.Values.ByteOffset.GetValueOrDefault(), accessor.GetStride() * sparse.Count.Value);
                ;

                if (sparse.Indices.ComponentType == (int)AccessorValueType.UNSIGNED_SHORT
                    && accessor.ComponentType == (int)AccessorValueType.FLOAT
                    && accessor.Type == VrmProtobuf.Accessor.Types.accessorType.Vec3)
                {
                    return RestoreSparseAccessorUInt16<Vector3>(bytes, accessor.Count.Value, sparseIndexBytes, sparseValueBytes);
                }
                if (sparse.Indices.ComponentType == (int)AccessorValueType.UNSIGNED_INT
                    && accessor.ComponentType == (int)AccessorValueType.FLOAT
                    && accessor.Type == VrmProtobuf.Accessor.Types.accessorType.Vec3)
                {
                    return RestoreSparseAccessorUInt32<Vector3>(bytes, accessor.Count.Value, sparseIndexBytes, sparseValueBytes);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                if (bytes.IsEmpty)
                {
                    // sparse and all value is zero
                    return new byte[accessor.GetStride() * accessor.Count.Value].AsMemory();
                }

                return bytes;
            }
        }

        /// <summary>
        /// submeshのindexが連続した領域に格納されているかを確認する
        /// </summary>
        bool AccessorsIsContinuous(int[] accessorIndices)
        {
            var firstAccessor = Gltf.Accessors[accessorIndices[0]];
            var firstView = Gltf.BufferViews[firstAccessor.BufferView.Value];
            var start = firstView.ByteOffset + firstAccessor.ByteOffset;
            var pos = start;
            foreach (var i in accessorIndices)
            {
                var current = Gltf.Accessors[i];
                if (current.Type != VrmProtobuf.Accessor.Types.accessorType.Scalar)
                {
                    throw new ArgumentException($"accessor.type: {current.Type}");
                }
                if (firstAccessor.ComponentType != current.ComponentType)
                {
                    return false;
                }

                var view = Gltf.BufferViews[current.BufferView.Value];
                if (pos != view.ByteOffset + current.ByteOffset)
                {
                    return false;
                }

                var begin = view.ByteOffset + current.ByteOffset;
                var byteLength = ((AccessorValueType)current.ComponentType).ByteSize() * current.Type.TypeCount() * current.Count;

                pos += byteLength;
            }

            return true;
        }

        /// <summary>
        /// Gltfの Primitive[] の indices をひとまとめにした
        /// IndexBuffer を返す。
        /// </summary>
        public BufferAccessor CreateAccessor(int[] accessorIndices)
        {
            var totalCount = accessorIndices.Sum(x => Gltf.Accessors[x].Count);
            if (AccessorsIsContinuous(accessorIndices))
            {
                // IndexBufferが連続して格納されている => Slice でいける
                var firstAccessor = Gltf.Accessors[accessorIndices[0]];
                var firstView = Gltf.BufferViews[firstAccessor.BufferView.Value];
                var start = firstView.ByteOffset.GetValueOrDefault() + firstAccessor.ByteOffset.Value;
                if (!firstView.Buffer.TryGetValidIndex(Gltf.Buffers.Count, out int firstViewBufferIndex))
                {
                    throw new Exception();
                }
                var buffer = Gltf.Buffers[firstViewBufferIndex];
                var bin = GetBufferBytes(buffer);
                var bytes = bin.Slice(start, totalCount.Value * firstAccessor.GetStride());
                return new BufferAccessor(bytes,
                    (AccessorValueType)firstAccessor.ComponentType,
                    EnumUtil.Cast<AccessorVectorType>(firstAccessor.Type),
                    totalCount.Value);
            }
            else
            {
                // IndexBufferが連続して格納されていない => Int[] を作り直す
                var indices = new byte[totalCount.Value * Marshal.SizeOf(typeof(int))];
                var span = MemoryMarshal.Cast<byte, int>(indices.AsSpan());
                var offset = 0;
                foreach (var accessorIndex in accessorIndices)
                {
                    var accessor = Gltf.Accessors[accessorIndex];
                    if (accessor.Type != VrmProtobuf.Accessor.Types.accessorType.Scalar)
                    {
                        throw new ArgumentException($"accessor.type: {accessor.Type}");
                    }
                    var view = Gltf.BufferViews[accessor.BufferView.Value];
                    if (!view.Buffer.TryGetValidIndex(Gltf.Buffers.Count, out int viewBufferIndex))
                    {
                        throw new Exception();
                    }
                    var buffer = Gltf.Buffers[viewBufferIndex];
                    var bin = GetBufferBytes(buffer);
                    var start = view.ByteOffset.GetValueOrDefault() + accessor.ByteOffset.Value;
                    var bytes = bin.Slice(start, accessor.Count.Value * accessor.GetStride());
                    var dst = MemoryMarshal.Cast<byte, int>(indices.AsSpan()).Slice(offset, accessor.Count.Value);
                    offset += accessor.Count.Value;
                    switch ((AccessorValueType)accessor.ComponentType)
                    {
                        case AccessorValueType.UNSIGNED_BYTE:
                            {
                                var src = bytes.Span;
                                for (int i = 0; i < src.Length; ++i)
                                {
                                    // byte to int
                                    dst[i] = src[i];
                                }
                            }
                            break;

                        case AccessorValueType.UNSIGNED_SHORT:
                            {
                                var src = MemoryMarshal.Cast<byte, ushort>(bytes.Span);
                                for (int i = 0; i < src.Length; ++i)
                                {
                                    // ushort to int
                                    dst[i] = src[i];
                                }
                            }
                            break;

                        case AccessorValueType.UNSIGNED_INT:
                            {
                                var src = MemoryMarshal.Cast<byte, int>(bytes.Span);
                                // int to int
                                src.CopyTo(dst);
                            }
                            break;

                        default:
                            throw new NotImplementedException($"accessor.componentType: {accessor.ComponentType}");
                    }
                }
                return new BufferAccessor(indices, AccessorValueType.UNSIGNED_INT, AccessorVectorType.SCALAR, totalCount.Value);
            }
        }

        public void CreateBufferAccessorAndAdd(int? accessorIndex, VertexBuffer b, string key)
        {
            if (accessorIndex.HasValue)
            {
                CreateBufferAccessorAndAdd(accessorIndex.Value, b, key);
            }
        }

        public void CreateBufferAccessorAndAdd(int accessorIndex, VertexBuffer b, string key)
        {
            var a = CreateAccessor(accessorIndex);
            if (a != null)
            {
                b.Add(key, a);
            }
        }

        public BufferAccessor CreateAccessor(int accessorIndex)
        {
            if (accessorIndex < 0)
            {
                return null;
            }
            var accessor = Gltf.Accessors[accessorIndex];
            var bytes = GetAccessorBytes(accessorIndex);
            var vectorType = EnumUtil.Cast<AccessorVectorType>(accessor.Type);
            return new BufferAccessor(bytes,
                (AccessorValueType)accessor.ComponentType, vectorType, accessor.Count.Value);
        }

        public string AssetVersion => Gltf.Asset.Version;

        public string AssetMinVersion => Gltf.Asset.MinVersion;

        public string AssetGenerator => Gltf.Asset.Generator;

        public string AssetCopyright => Gltf.Asset.Copyright;

        public int NodeCount => Gltf.Nodes.Count;

        public int ImageCount => Gltf.Images.Count;

        public int TextureCount => Gltf.Textures.Count;

        public int MaterialCount => Gltf.Materials.Count;

        public int SkinCount => Gltf.Skins.Count;

        public int MeshCount => Gltf.Meshes.Count;

        // TODO:
        public int AnimationCount => 0;

        public bool HasVrm => Gltf.Extensions != null && Gltf.Extensions.VRMCVrm != null;

        public string VrmExporterVersion => Gltf.Asset.Generator;

        public string VrmSpecVersion => Gltf.Extensions.VRMCVrm.SpecVersion;



        public Node CreateNode(int index)
        {
            var x = Gltf.Nodes[index];
            var node = new Node(x.Name);
            if (x.Matrix.Count > 0)
            {
                if (x.Translation.Count > 0) throw new Exception("matrix with translation");
                if (x.Rotation.Count > 0) throw new Exception("matrix with rotation");
                if (x.Scale.Count > 0) throw new Exception("matrix with scale");
                var m = new Matrix4x4(
                    x.Matrix[0], x.Matrix[1], x.Matrix[2], x.Matrix[3],
                    x.Matrix[4], x.Matrix[5], x.Matrix[6], x.Matrix[7],
                    x.Matrix[8], x.Matrix[9], x.Matrix[10], x.Matrix[11],
                    x.Matrix[12], x.Matrix[13], x.Matrix[14], x.Matrix[15]
                    );

                node.SetLocalMatrix(m, true);
            }
            else
            {
                if (x.Translation.Count > 0) node.LocalTranslation = new Vector3(x.Translation[0], x.Translation[1], x.Translation[2]);
                if (x.Rotation.Count > 0) node.LocalRotation = new Quaternion(x.Rotation[0], x.Rotation[1], x.Rotation[2], x.Rotation[3]);
                if (x.Scale.Count > 0) node.LocalScaling = new Vector3(x.Scale[0], x.Scale[1], x.Scale[2]);
            }
            return node;
        }

        public IEnumerable<int> GetChildNodeIndices(int i)
        {
            var gltfNode = Gltf.Nodes[i];
            // if (gltfNode.Children.Count)
            {
                foreach (var j in gltfNode.Children)
                {
                    yield return j;
                }
            }
        }

        public Image CreateImage(int index)
        {
            return Gltf.Images[index].FromGltf(this);
        }
        private (Texture.TextureTypes, VrmProtobuf.Material) GetTextureType(int textureIndex)
        {
            foreach (var material in Gltf.Materials)
            {
                if (material.Extensions != null && material.Extensions.VRMCMaterialsMtoon != null)
                {
                    // var mtoon = material.Extensions.VRMCMaterialsMtoon;
                    if (material.NormalTexture.Index == textureIndex) return (Texture.TextureTypes.NormalMap, material);
                }
                else if (material.Extensions != null && material.Extensions.KHRMaterialsUnlit != null)
                {
                }
                else
                {
                    if (material.PbrMetallicRoughness?.BaseColorTexture?.Index == textureIndex) return (Texture.TextureTypes.Default, material);
                    if (material.PbrMetallicRoughness?.MetallicRoughnessTexture?.Index == textureIndex) return (Texture.TextureTypes.MetallicRoughness, material);
                    if (material.OcclusionTexture?.Index == textureIndex) return (Texture.TextureTypes.Occlusion, material);
                    if (material.EmissiveTexture?.Index == textureIndex) return (Texture.TextureTypes.Emissive, material);
                    if (material.NormalTexture?.Index == textureIndex) return (Texture.TextureTypes.NormalMap, material);
                }
            }

            return (Texture.TextureTypes.Default, null);
        }

        private Texture.ColorSpaceTypes GetTextureColorSpaceType(int textureIndex)
        {
            foreach (var material in Gltf.Materials)
            {
                // mtoon
                if (material.Extensions != null && material.Extensions.VRMCMaterialsMtoon != null)
                {
                    var mtoon = material.Extensions.VRMCMaterialsMtoon;
                    if (material.PbrMetallicRoughness.BaseColorTexture.Index == textureIndex) return Texture.ColorSpaceTypes.Srgb;
                    if (mtoon.ShadeMultiplyTexture == textureIndex) return Texture.ColorSpaceTypes.Srgb;
                    if (material.EmissiveTexture.Index == textureIndex) return Texture.ColorSpaceTypes.Srgb;
                    if (mtoon.RimMultiplyTexture == textureIndex) return Texture.ColorSpaceTypes.Srgb;
                    if (mtoon.AdditiveTexture == textureIndex) return Texture.ColorSpaceTypes.Srgb;

                    if (mtoon.OutlineWidthMultiplyTexture == textureIndex) return Texture.ColorSpaceTypes.Linear;
                    if (mtoon.UvAnimationMaskTexture == textureIndex) return Texture.ColorSpaceTypes.Linear;

                    if (material.NormalTexture.Index == textureIndex) return Texture.ColorSpaceTypes.Linear;
                }
                // unlit
                else if (material.Extensions != null && material.Extensions.KHRMaterialsUnlit != null)
                {
                    if (material.PbrMetallicRoughness.BaseColorTexture.Index == textureIndex) return Texture.ColorSpaceTypes.Srgb;
                }
                // Pbr
                else
                {
                    if (material.PbrMetallicRoughness?.BaseColorTexture?.Index == textureIndex) return Texture.ColorSpaceTypes.Srgb;
                    if (material.PbrMetallicRoughness?.MetallicRoughnessTexture?.Index == textureIndex) return Texture.ColorSpaceTypes.Linear;
                    if (material.OcclusionTexture?.Index == textureIndex) return Texture.ColorSpaceTypes.Linear;
                    if (material.EmissiveTexture?.Index == textureIndex) return Texture.ColorSpaceTypes.Srgb;
                    if (material.NormalTexture?.Index == textureIndex) return Texture.ColorSpaceTypes.Linear;
                }
            }

            return Texture.ColorSpaceTypes.Srgb;
        }

        public Texture CreateTexture(int index, List<Image> images)
        {
            var texture = Gltf.Textures[index];
            var textureType = GetTextureType(index);
            var colorSpace = GetTextureColorSpaceType(index);

            var sampler = (texture.Sampler.HasValue)
            ? Gltf.Samplers[texture.Sampler.Value]
            : new VrmProtobuf.Sampler()
            ;

            if (textureType.Item1 == Texture.TextureTypes.MetallicRoughness && textureType.Item2.PbrMetallicRoughness != null)
            {
                var roughnessFactor = textureType.Item2.PbrMetallicRoughness.RoughnessFactor;
                var name = !string.IsNullOrEmpty(texture.Name) ? texture.Name : images[texture.Source.Value].Name;
                return new MetallicRoughnessImageTexture(
                    name,
                    sampler.FromGltf(),
                    images[texture.Source.Value],
                    roughnessFactor.GetValueOrDefault(1.0f),
                    colorSpace,
                    textureType.Item1);
            }
            else
            {
                return texture.FromGltf(sampler, images, colorSpace, textureType.Item1);
            }
        }

        public Material CreateMaterial(int index, List<Texture> textures)
        {
            var x = Gltf.Materials[index];
            return x.FromGltf(textures);
        }

        public Skin CreateSkin(int index, List<Node> nodes)
        {
            var x = Gltf.Skins[index];
            BufferAccessor inverseMatrices = null;
            if (x.InverseBindMatrices.HasValue)
            {
                inverseMatrices = CreateAccessor(x.InverseBindMatrices.Value);
            }
            var skin = new Skin
            {
                InverseMatrices = inverseMatrices,
                Joints = x.Joints.Select(y => nodes[y]).ToList(),
            };
            if (x.Skeleton.HasValue) // TODO: proto to int
            {
                skin.Root = nodes[x.Skeleton.Value];
            }
            return skin;
        }

        public MeshGroup CreateMesh(int index, List<Material> materials)
        {
            var x = Gltf.Meshes[index];
            var group = x.FromGltf(this, materials);
            return group;
        }

        public (int, int) GetNodeMeshSkin(int index)
        {
            var x = Gltf.Nodes[index];

            int meshIndex = -1;
            if (x.Mesh.TryGetValidIndex(Gltf.Meshes.Count, out int mi))
            {
                meshIndex = mi;
            }

            int skinIndex = -1;
            if (x.Skin.TryGetValidIndex(Gltf.Skins.Count, out int si))
            {
                skinIndex = si;
            }

            return (meshIndex, skinIndex);
        }

        public Animation CreateAnimation(int index, List<Node> nodes)
        {
            throw new NotImplementedException();
        }

        public Meta CreateVrmMeta(List<Texture> textures)
        {
            return Gltf.Extensions.VRMCVrm.Meta.FromGltf(textures);
        }

        static void AssignHumanoid(List<Node> nodes, VrmProtobuf.HumanBone humanBone, VrmLib.HumanoidBones key)
        {
            if (humanBone.Node.HasValue)
            {
                nodes[humanBone.Node.Value].HumanoidBone = key;
            }
        }

        public void LoadVrmHumanoid(List<Node> nodes)
        {
            var gltfVrm = Gltf.Extensions.VRMCVrm;
            if (gltfVrm.Humanoid != null)
            {
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.Hips, HumanoidBones.hips);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftUpperLeg, HumanoidBones.leftUpperLeg);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightUpperLeg, HumanoidBones.rightUpperLeg);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftLowerLeg, HumanoidBones.leftLowerLeg);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightLowerLeg, HumanoidBones.rightLowerLeg);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftFoot, HumanoidBones.leftFoot);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightFoot, HumanoidBones.rightFoot);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.Spine, HumanoidBones.spine);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.Chest, HumanoidBones.chest);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.Neck, HumanoidBones.neck);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.Head, HumanoidBones.head);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftShoulder, HumanoidBones.leftShoulder);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightShoulder, HumanoidBones.rightShoulder);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftUpperArm, HumanoidBones.leftUpperArm);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightUpperArm, HumanoidBones.rightUpperArm);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftLowerArm, HumanoidBones.leftLowerArm);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightLowerArm, HumanoidBones.rightLowerArm);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftHand, HumanoidBones.leftHand);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightHand, HumanoidBones.rightHand);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftToes, HumanoidBones.leftToes);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightToes, HumanoidBones.rightToes);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftEye, HumanoidBones.leftEye);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightEye, HumanoidBones.rightEye);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.Jaw, HumanoidBones.jaw);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftThumbProximal, HumanoidBones.leftThumbProximal);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftThumbIntermediate, HumanoidBones.leftThumbIntermediate);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftThumbDistal, HumanoidBones.leftThumbDistal);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftIndexProximal, HumanoidBones.leftIndexProximal);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftIndexIntermediate, HumanoidBones.leftIndexIntermediate);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftIndexDistal, HumanoidBones.leftIndexDistal);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftMiddleProximal, HumanoidBones.leftMiddleProximal);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftMiddleIntermediate, HumanoidBones.leftMiddleIntermediate);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftMiddleDistal, HumanoidBones.leftMiddleDistal);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftRingProximal, HumanoidBones.leftRingProximal);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftRingIntermediate, HumanoidBones.leftRingIntermediate);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftRingDistal, HumanoidBones.leftRingDistal);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftLittleProximal, HumanoidBones.leftLittleProximal);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftLittleIntermediate, HumanoidBones.leftLittleIntermediate);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.LeftLittleDistal, HumanoidBones.leftLittleDistal);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightThumbProximal, HumanoidBones.rightThumbProximal);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightThumbIntermediate, HumanoidBones.rightThumbIntermediate);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightThumbDistal, HumanoidBones.rightThumbDistal);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightIndexProximal, HumanoidBones.rightIndexProximal);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightIndexIntermediate, HumanoidBones.rightIndexIntermediate);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightIndexDistal, HumanoidBones.rightIndexDistal);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightMiddleProximal, HumanoidBones.rightMiddleProximal);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightMiddleIntermediate, HumanoidBones.rightMiddleIntermediate);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightMiddleDistal, HumanoidBones.rightMiddleDistal);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightRingProximal, HumanoidBones.rightRingProximal);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightRingIntermediate, HumanoidBones.rightRingIntermediate);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightRingDistal, HumanoidBones.rightRingDistal);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightLittleProximal, HumanoidBones.rightLittleProximal);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightLittleIntermediate, HumanoidBones.rightLittleIntermediate);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.RightLittleDistal, HumanoidBones.rightLittleDistal);
                AssignHumanoid(nodes, gltfVrm.Humanoid.HumanBones.UpperChest, HumanoidBones.upperChest);
            }
        }

        public BlendShapeManager CreateVrmBlendShape(List<MeshGroup> _, List<Material> materials, List<Node> nodes)
        {
            var gltfVrm = Gltf.Extensions.VRMCVrm;
            if (gltfVrm.BlendShape != null
                && gltfVrm.BlendShape.BlendShapeGroups != null
                && gltfVrm.BlendShape.BlendShapeGroups.Any())
            {
                return gltfVrm.BlendShape.FromGltf(nodes, materials);
            }

            return null;
        }

        static VrmSpringBoneCollider CreateCollider(VrmProtobuf.ColliderShape z)
        {
            if (z.Sphere != null)
            {
                return VrmSpringBoneCollider.CreateSphere(z.Sphere.Offset.ToVector3(), z.Sphere.Radius.Value);
            }
            if (z.Capsule != null)
            {
                return VrmSpringBoneCollider.CreateCapsule(z.Capsule.Offset.ToVector3(), z.Capsule.Radius.Value, z.Capsule.Tail.ToVector3());
            }
            throw new NotImplementedException();
        }

        public SpringBoneManager CreateVrmSpringBone(List<Node> nodes)
        {
            var gltfVrm = Gltf.Extensions.VRMCSpringBone;
            if ((gltfVrm is null))
            {
                return null;
            }

            var springBone = new SpringBoneManager();

            // springs
            foreach (var group in gltfVrm.Springs.GroupBy(x => x.Setting.Value))
            {
                var sb = new SpringBone();
                sb.Comment = group.First().Name;
                sb.HitRadius = group.First().HitRadius.Value;
                var setting = gltfVrm.Settings[group.Key];
                sb.DragForce = setting.DragForce.Value;
                sb.GravityDir = setting.GravityDir.ToVector3();
                sb.GravityPower = setting.GravityPower.Value;
                sb.Stiffness = setting.Stiffness.Value;

                foreach (var spring in group)
                {
                    // root
                    sb.Bones.Add(nodes[spring.SpringRoot.Value]);
                    // collider
                    foreach (var colliderNode in spring.Colliders)
                    {
                        var collider = springBone.Colliders.FirstOrDefault(x => x.Node == nodes[colliderNode]);
                        if (collider == null)
                        {
                            collider = new SpringBoneColliderGroup(nodes[colliderNode], Gltf.Nodes[colliderNode].Extensions.VRMCNodeCollider.Shapes.Select(x =>
                            {
                                if (x.Sphere != null)
                                {
                                    return VrmSpringBoneCollider.CreateSphere(x.Sphere.Offset.ToVector3(), x.Sphere.Radius.Value);
                                }
                                else if (x.Capsule != null)
                                {
                                    return VrmSpringBoneCollider.CreateCapsule(x.Capsule.Offset.ToVector3(), x.Capsule.Radius.Value, x.Capsule.Tail.ToVector3());
                                }
                                else
                                {
                                    throw new NotImplementedException();
                                }
                            }));
                            springBone.Colliders.Add(collider);
                        }
                        sb.Colliders.Add(collider);
                    }
                }

                springBone.Springs.Add(sb);
            }

            return springBone;
        }

        public FirstPerson CreateVrmFirstPerson(List<Node> nodes, List<MeshGroup> meshGroups)
        {
            var gltfVrm = Gltf.Extensions.VRMCVrm;
            if (gltfVrm.FirstPerson == null)
            {
                return null;
            }
            return gltfVrm.FirstPerson.FromGltf(nodes);
        }

        public LookAt CreateVrmLookAt()
        {
            var gltfVrm = Gltf.Extensions.VRMCVrm;
            if (gltfVrm.LookAt == null)
            {
                return null;
            }
            return gltfVrm.LookAt.FromGltf();
        }

        public Memory<byte> GetBufferBytes(VrmProtobuf.BufferView bufferView)
        {
            if (!bufferView.Buffer.TryGetValidIndex(Gltf.Buffers.Count, out int bufferViewBufferIndex))
            {
                throw new Exception();
            }
            return GetBufferBytes(Gltf.Buffers[bufferViewBufferIndex]);
        }

        public Memory<byte> GetBufferBytes(VrmProtobuf.Buffer buffer)
        {
            int index = Gltf.Buffers.IndexOf(buffer);
            return Buffers[index].Bytes;
        }
    }
}