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
                    bytes = bin.Slice(view.ByteOffset.GetValueOrDefault(), view.ByteLength).Slice(accessor.ByteOffset.GetValueOrDefault(), byteSize);
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
                    .Slice(sparseIndexView.ByteOffset.GetValueOrDefault(), sparseIndexView.ByteLength)
                    .Slice(sparse.Indices.ByteOffset.GetValueOrDefault(), ((AccessorValueType)sparse.Indices.ComponentType).ByteSize() * sparse.Count)
                    ;

                if (!sparse.Values.BufferView.TryGetValidIndex(Gltf.BufferViews.Count, out int sparseValulesBufferViewIndex))
                {
                    throw new Exception();
                }
                var sparseValueView = Gltf.BufferViews[sparseValulesBufferViewIndex];
                var sparseValueBin = GetBufferBytes(sparseValueView);
                var sparseValueBytes = sparseValueBin
                    .Slice(sparseValueView.ByteOffset.GetValueOrDefault(), sparseValueView.ByteLength)
                    .Slice(sparse.Values.ByteOffset.GetValueOrDefault(), accessor.GetStride() * sparse.Count);
                ;

                if (sparse.Indices.ComponentType == (int)AccessorValueType.UNSIGNED_SHORT
                    && accessor.ComponentType == (int)AccessorValueType.FLOAT
                    && accessor.Type == "VEC3")
                {
                    return RestoreSparseAccessorUInt16<Vector3>(bytes, accessor.Count, sparseIndexBytes, sparseValueBytes);
                }
                if (sparse.Indices.ComponentType == (int)AccessorValueType.UNSIGNED_INT
                    && accessor.ComponentType == (int)AccessorValueType.FLOAT
                    && accessor.Type == "VEC3")
                {
                    return RestoreSparseAccessorUInt32<Vector3>(bytes, accessor.Count, sparseIndexBytes, sparseValueBytes);
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
                    return new byte[accessor.GetStride() * accessor.Count].AsMemory();
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
                if (current.Type != "SCALAR")
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
                var bytes = bin.Slice(start, totalCount * firstAccessor.GetStride());
                return new BufferAccessor(bytes,
                    (AccessorValueType)firstAccessor.ComponentType,
                    EnumUtil.Parse<AccessorVectorType>(firstAccessor.Type),
                    totalCount);
            }
            else
            {
                // IndexBufferが連続して格納されていない => Int[] を作り直す
                var indices = new byte[totalCount * Marshal.SizeOf(typeof(int))];
                var span = MemoryMarshal.Cast<byte, int>(indices.AsSpan());
                var offset = 0;
                foreach (var accessorIndex in accessorIndices)
                {
                    var accessor = Gltf.Accessors[accessorIndex];
                    if (accessor.Type != "SCALAR")
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
                    var bytes = bin.Slice(start, accessor.Count * accessor.GetStride());
                    var dst = MemoryMarshal.Cast<byte, int>(indices.AsSpan()).Slice(offset, accessor.Count);
                    offset += accessor.Count;
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
                return new BufferAccessor(indices, AccessorValueType.UNSIGNED_INT, AccessorVectorType.SCALAR, totalCount);
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
            var vectorType = EnumUtil.Parse<AccessorVectorType>(accessor.Type);
            return new BufferAccessor(bytes,
                (AccessorValueType)accessor.ComponentType, vectorType, accessor.Count);
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
                    var mtoon = material.Extensions.VRMCMaterialsMtoon;
                    if (mtoon.NormalTexture == textureIndex) return (Texture.TextureTypes.NormalMap, material);
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
                    if (mtoon.LitMultiplyTexture == textureIndex) return Texture.ColorSpaceTypes.Srgb;
                    if (mtoon.ShadeMultiplyTexture == textureIndex) return Texture.ColorSpaceTypes.Srgb;
                    if (mtoon.EmissionMultiplyTexture == textureIndex) return Texture.ColorSpaceTypes.Srgb;
                    if (mtoon.RimMultiplyTexture == textureIndex) return Texture.ColorSpaceTypes.Srgb;
                    if (mtoon.AdditiveTexture == textureIndex) return Texture.ColorSpaceTypes.Srgb;

                    if (mtoon.OutlineWidthMultiplyTexture == textureIndex) return Texture.ColorSpaceTypes.Linear;
                    if (mtoon.UvAnimationMaskTexture == textureIndex) return Texture.ColorSpaceTypes.Linear;

                    if (mtoon.NormalTexture == textureIndex) return Texture.ColorSpaceTypes.Linear;
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

        public void LoadVrmHumanoid(List<Node> nodes)
        {
            var gltfVrm = Gltf.Extensions.VRMCVrm;
            if (gltfVrm.Humanoid != null)
            {
                foreach (var kv in gltfVrm.Humanoid.HumanBones)
                {
                    var bone = EnumUtil.Parse<VrmLib.HumanoidBones>(kv.Key);
                    if (bone != VrmLib.HumanoidBones.unknown)
                    {
                        nodes[kv.Value.Node].HumanoidBone = bone;
                    }
                }
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

        public SpringBoneManager CreateVrmSpringBone(List<Node> nodes)
        {
            var gltfVrm = Gltf.Extensions.VRMCSpringBone;
            if ((gltfVrm is null))
            {
                return null;
            }

            var springBone = new SpringBoneManager();

            // colliders
            springBone.Colliders.AddRange(
                gltfVrm.ColliderGroups.Select(y =>
                new SpringBoneColliderGroup(
                    nodes[y.Node],
                    y.Colliders.Select(z => new VrmSpringBoneColliderSphere(z.Offset.ToVector3(), z.Radius))
                )
            ));

            // springs
            springBone.Springs.AddRange(gltfVrm.BoneGroups.Select(x =>
            {
                var sb = new SpringBone();
                sb.Bones.AddRange(x.Bones.Select(y => nodes[y]));
                if (x.Center.TryGetValidIndex(nodes.Count, out int centerIndex))
                {
                    sb.Origin = nodes[centerIndex];
                }
                sb.Colliders.AddRange(x.ColliderGroups.Select(y => springBone.Colliders[y]));
                sb.Comment = x.Name;
                sb.DragForce = x.DragForce;
                sb.GravityDir = x.GravityDir.ToVector3();
                sb.GravityPower = x.GravityPower;
                sb.HitRadius = x.HitRadius;
                sb.Stiffness = x.Stiffness;
                return sb;
            }));

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