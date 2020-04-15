using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using GltfFormat;
using ObjectNotation;
using GltfSerialization.Generated;
using System.Text;
using VrmLib;
using GltfSerializationAdapter;

namespace GltfSerialization
{
    public class GltfStorage : IVrmStorage
    {
        public ArraySegment<byte> OriginalJson { get; private set; }

        public FileInfo Path;

        public int FileByteSize;

        public String Name
        {
            get
            {
                if (Path != null)
                {
                    return Path.Name;
                }
                else
                {
                    return "__memory__";
                }
            }
        }

        public Utf8String Json;

        public Gltf Gltf;

        public List<IBuffer> Buffers = new List<IBuffer>();

        #region Read
        public GltfStorage()
        {
            Gltf = new Gltf();
            Buffers.Add(new ArrayByteBuffer());

            // buffer 0
            Gltf.buffers.Add(new GltfBuffer
            {
                byteLength = Buffers[0].Bytes.Count
            });
        }

        /// Glb interface. bin is binary chunk
        public GltfStorage(FileInfo path, ArraySegment<byte> json, Memory<byte> bin)
        {
            OriginalJson = json;
            Gltf = Generated.GltfDeserializer.Deserialize(json.ParseAsJson());

            Path = path;
            Json = new Utf8String(json);

            if (!bin.IsEmpty)
            {
                // glb
                MemoryMarshal.TryGetArray(bin, out ArraySegment<byte> segment);
                Buffers.Add(new SimpleBuffer(segment));
            }
            else
            {
                // gltf
                var baseDir = path.Directory.FullName;
                Buffers.AddRange(Gltf.buffers.Select(x => new UriByteBuffer(baseDir, x.uri)));
            }

            if (Gltf.extensions != null && Gltf.extensions.VRM != null)
            {
                // VRM
                if (Gltf.extensions.VRM.humanoid != null)
                {
                    Gltf.extensions.VRM.humanoid.humanBones =
                    Gltf.extensions.VRM.humanoid.humanBones.OrderBy(x => x.bone).ToList();
                }
            }

            if (!Gltf.materials.Any())
            {
                // default material
                Gltf.materials.Add(GltfMaterial.CreateDefault("__default__"));
            }
        }

        public Memory<byte> GetBufferBytes(GltfBufferView view)
        {
            return GetBufferBytes(Gltf.buffers[view.buffer]);
        }

        public Memory<byte> GetBufferBytes(GltfBuffer buffer)
        {
            int index = Gltf.buffers.IndexOf(buffer);
            return Buffers[index].Bytes;
        }

        public static GltfStorage Parse(byte[] bytes, string path)
        {
            return Parse(bytes, new FileInfo(path));
        }

        public static GltfStorage Parse(byte[] bytes, FileInfo fi = null)
        {
            if (Glb.TryParse(bytes, out Glb glb, out Exception ex))
            {
                var s = new GltfStorage(fi, glb.Json.Bytes, glb.Binary.Bytes);
                s.FileByteSize = bytes.Length;
                return s;
            }
            else
            {
                var s = new GltfStorage(fi, new ArraySegment<byte>(bytes), default(Memory<byte>));
                s.FileByteSize = bytes.Length;
                return s;
            }
        }

        public static GltfStorage FromStream(Stream s, FileInfo path)
        {
            // var task = ReadAsync(s, new StreamLoader(path));
            // task.Wait();
            // return task.Result;
            var bytes = new List<byte>();
            try
            {
                while (true)
                {
                    var b = s.ReadByte();
                    if (b < 0)
                    {
                        break;
                    }
                    bytes.Add((byte)b);
                }
            }
            catch (ObjectDisposedException)
            {
            }
            return GltfStorage.Parse(bytes.ToArray(), path);
        }

        public static GltfStorage FromPath(string path)
        {
            return Parse(File.ReadAllBytes(path), new FileInfo(path));
        }
        #endregion

        #region Equals
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


        public Memory<byte> GetAccessorBytes(GltfAccessor a)
        {
            return GetAccessorBytes(Gltf.accessors.IndexOf(a));
        }

        public Memory<byte> GetAccessorBytes(int accessorIndex)
        {
            var accessor = Gltf.accessors[accessorIndex];
            var sparse = accessor.sparse;

            Memory<byte> bytes = default(Memory<byte>);

            if (accessor.bufferView >= 0)
            {
                var view = Gltf.bufferViews[accessor.bufferView];
                var buffer = Buffers[view.buffer];
                var bin = buffer.Bytes;
                var byteSize = accessor.componentType.ByteSize() * accessor.type.TypeCount() * accessor.count;
                bytes = bin.Slice(view.byteOffset, view.byteLength).Slice(accessor.byteOffset, byteSize);
            }

            if (sparse != null)
            {
                var sparseIndexView = Gltf.bufferViews[sparse.indices.bufferView];
                var sparseIndexBin = GetBufferBytes(sparseIndexView);
                var sparseIndexBytes = sparseIndexBin
                    .Slice(sparseIndexView.byteOffset, sparseIndexView.byteLength)
                    .Slice(sparse.indices.byteOffset, sparse.indices.componentType.ByteSize() * sparse.count)
                    ;

                var sparseValueView = Gltf.bufferViews[sparse.values.bufferView];
                var sparseValueBin = GetBufferBytes(sparseValueView);
                var sparseValueBytes = sparseValueBin
                    .Slice(sparseValueView.byteOffset, sparseValueView.byteLength)
                    .Slice(sparse.values.byteOffset, accessor.GetStride() * sparse.count);
                ;

                if (sparse.indices.componentType == GltfComponentType.UNSIGNED_SHORT
                    && accessor.componentType == GltfComponentType.FLOAT
                    && accessor.type == GltfAccessorType.VEC3)
                {
                    return RestoreSparseAccessorUInt16<Vector3>(bytes, accessor.count, sparseIndexBytes, sparseValueBytes);
                }
                if (sparse.indices.componentType == GltfComponentType.UNSIGNED_INT
                    && accessor.componentType == GltfComponentType.FLOAT
                    && accessor.type == GltfAccessorType.VEC3)
                {
                    return RestoreSparseAccessorUInt32<Vector3>(bytes, accessor.count, sparseIndexBytes, sparseValueBytes);
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
                    return new byte[accessor.GetStride() * accessor.count].AsMemory();
                }

                return bytes;
            }
        }

        public Memory<byte> GetTextureBytes(GltfTexture texture)
        {
            return GetTextureBytes(Gltf.textures.IndexOf(texture));
        }

        Memory<byte> GetTextureBytes(int textureIndex)
        {
            var texture = Gltf.textures[textureIndex];
            var source = Gltf.images[texture.source];
            var view = Gltf.bufferViews[source.bufferView];
            var buffer = Buffers[view.buffer];
            var bin = buffer.Bytes;
            return bin.Slice(view.byteOffset, view.byteLength);
        }

        static bool IsTextureEquals(
            GltfStorage l, GltfTextureInfo lT,
            GltfStorage r, GltfTextureInfo rT
        )
        {
            if (lT is null && rT is null) return true;
            var lBytes = l.GetTextureBytes(lT.index);
            var rBytes = r.GetTextureBytes(rT.index);
            return lBytes.Span.SequenceEqual(rBytes.Span);
        }

        static bool IsAccessorEqualsGeneric<T>(
            GltfStorage l, GltfAccessor la,
            GltfStorage r, GltfAccessor ra) where T : struct, IEquatable<T>
        {
            var lSpan = MemoryMarshal.Cast<byte, T>(l.GetAccessorBytes(la).Span);
            var rSpan = MemoryMarshal.Cast<byte, T>(r.GetAccessorBytes(ra).Span);
            return lSpan.SequenceEqual(rSpan);
        }

        static bool IsAccessorEquals(
            GltfStorage l, GltfAccessor la,
            GltfStorage r, GltfAccessor ra)
        {
            if (la.componentType != ra.componentType) return false;
            if (la.type != ra.type) return false;
            if (la.count != ra.count) return false;

            var t = la.GetValueType();
            var gmi = typeof(GltfStorage).GetMethod(nameof(IsAccessorEqualsGeneric),
                BindingFlags.Static | BindingFlags.NonPublic);
            var mi = gmi.MakeGenericMethod(t);

            return (bool)mi.Invoke(null, new object[]{
                l, la,
                r, ra,
            });
        }

        static bool IsAccessorEquals(
            GltfStorage l, int ll,
            GltfStorage r, int rr)
        {
            if (ll == -1 && rr == -1) return true;
            if (ll == -1 || rr == -1) return false;

            var la = l.Gltf.accessors[ll];
            var ra = r.Gltf.accessors[rr];
            return IsAccessorEquals(l, la, r, ra);
        }

        static bool IsPrimitiveEquals(
            GltfStorage l, GltfPrimitive lPrim,
            GltfStorage r, GltfPrimitive rPrim
        )
        {
            if (!IsAccessorEquals(
                l, lPrim.indices,
                r, rPrim.indices))
            {
                return false;
            }

            if (!IsAccessorEquals(
                l, lPrim.attributes.POSITION,
                r, rPrim.attributes.POSITION))
            {
                return false;
            }

            if (!IsAccessorEquals(
                l, lPrim.attributes.NORMAL,
                r, rPrim.attributes.NORMAL))
            {
                return false;
            }

            if (!IsAccessorEquals(
                l, lPrim.attributes.TEXCOORD_0,
                r, rPrim.attributes.TEXCOORD_0))
            {
                return false;
            }

            // ignore tangent
            // if (!IsAccessorEquals(
            //     l, lPrim.attributes.TANGENT,
            //     r, rPrim.attributes.TANGENT))
            // {
            //     return false;
            // }

            if (!IsAccessorEquals(
                l, lPrim.attributes.COLOR_0,
                r, rPrim.attributes.COLOR_0))
            {
                return false;
            }
            if (!IsAccessorEquals(
                l, lPrim.attributes.JOINTS_0,
                r, rPrim.attributes.JOINTS_0))
            {
                return false;
            }
            if (!IsAccessorEquals(
                l, lPrim.attributes.WEIGHTS_0,
                r, rPrim.attributes.WEIGHTS_0))
            {
                return false;
            }

            if (lPrim.targets is null)
            {
                if (!(rPrim.targets is null))
                {
                    return false;
                }
            }
            else
            {
                if (rPrim.targets is null)
                {
                    return false;
                }

                foreach (var (ll, rr) in Enumerable.Zip(lPrim.targets, rPrim.targets, (x, y) => (x, y)))
                {
                    if (!IsAccessorEquals(
                        l, ll.POSITION,
                        r, rr.POSITION))
                    {
                        return false;
                    }
                    if (!IsAccessorEquals(
                        l, ll.NORMAL,
                        r, rr.NORMAL))
                    {
                        return false;
                    }
                    // ignore tangent
                    // if (!IsAccessorEquals(
                    //     l, ll.TANGENT,
                    //     r, rr.TANGENT))
                    // {
                    //     return false;
                    // }
                }
            }

            return true;
        }

        // Unitテスト向け
        //
        // 緩めに等値比較する
        public bool Equals(GltfStorage rhs)
        {
            if (rhs is null)
            {
                return false;
            }

            var lAsset = Gltf.asset;
            var rAsset = rhs.Gltf.asset;
            if (!lAsset.Equals(rAsset))
            {
                return false;
            }

            if (Gltf.buffers.Count != Gltf.buffers.Count)
            {
                return false;
            }
            var lBuffer = Gltf.buffers[0];
            var rBuffer = rhs.Gltf.buffers[0];
            if (!lBuffer.Equals(rBuffer))
            {
                //return false;
            }

            // comapre json level
            if (!Gltf.Equals(rhs.Gltf))
            {
                return false;
            }

            // compare buffer
            if (Gltf.meshes.Count != rhs.Gltf.meshes.Count)
            {
                return false;
            }

            for (var x = 0; x < Gltf.meshes.Count; ++x)
            {
                var lMesh = Gltf.meshes[x];
                var rMesh = rhs.Gltf.meshes[x];

                if (lMesh.primitives.Count != rMesh.primitives.Count)
                {
                    return false;
                }
                for (var y = 0; y < lMesh.primitives.Count; ++y)
                {
                    var lPrim = lMesh.primitives[y];
                    var rPrim = rMesh.primitives[y];

                    if (!IsPrimitiveEquals(this, lPrim, rhs, rPrim))
                    {
                        return false;
                    }

                    // マテリアルで使っているテクスチャを比較する
                    var lMaterial = Gltf.materials[lPrim.material];
                    var rMaterial = rhs.Gltf.materials[rPrim.material];
                    // baseColor
                    if (!IsTextureEquals(
                        this, lMaterial.pbrMetallicRoughness?.baseColorTexture,
                        rhs, rMaterial.pbrMetallicRoughness?.baseColorTexture
                    ))
                    {
                        return false;
                    }
                    if (!IsTextureEquals(
                        this, lMaterial.pbrMetallicRoughness?.metallicRoughnessTexture,
                        rhs, rMaterial.pbrMetallicRoughness?.metallicRoughnessTexture
                    ))
                    {
                        return false;
                    }
                    if (!IsTextureEquals(
                        this, lMaterial.emissiveTexture,
                        rhs, rMaterial.emissiveTexture
                    ))
                    {
                        return false;
                    }
                    if (!IsTextureEquals(
                        this, lMaterial.normalTexture,
                        rhs, rMaterial.normalTexture
                    ))
                    {
                        return false;
                    }
                    if (!IsTextureEquals(
                        this, lMaterial.occlusionTexture,
                        rhs, rMaterial.occlusionTexture
                    ))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals((GltfStorage)obj);
        }

        public static bool operator ==(GltfStorage lhs, GltfStorage rhs)
        {
            if (lhs is null)
            {
                return rhs is null;
            }
            else
            {
                if (rhs is null)
                {
                    return false;
                }
                else
                {
                    // Equals handles case of null on right side.
                    return lhs.Equals(rhs);
                }
            }
        }

        public static bool operator !=(GltfStorage lhs, GltfStorage rhs)
        {
            return !(lhs == rhs);
        }
        #endregion

        public string AssetVersion => Gltf.asset.version;

        public string AssetMinVersion => Gltf.asset.minVersion;

        public string AssetGenerator => Gltf.asset.generator;

        public string AssetCopyright => Gltf.asset.copyright;

        public int NodeCount => Gltf.nodes.Count;

        public Node CreateNode(int index)
        {
            var x = Gltf.nodes[index];
            var node = new Node(x.name)
            {
                GltfIndex = index,
            };
            if (x.matrix != null)
            {
                if (x.translation != null) throw new Exception("matrix with translation");
                if (x.rotation != null) throw new Exception("matrix with rotation");
                if (x.scale != null) throw new Exception("matrix with scale");
                var m = new Matrix4x4(
                    x.matrix[0], x.matrix[1], x.matrix[2], x.matrix[3],
                    x.matrix[4], x.matrix[5], x.matrix[6], x.matrix[7],
                    x.matrix[8], x.matrix[9], x.matrix[10], x.matrix[11],
                    x.matrix[12], x.matrix[13], x.matrix[14], x.matrix[15]
                    );

                node.SetLocalMatrix(m, true);
            }
            else
            {
                if (x.translation != null) node.LocalTranslation = new Vector3(x.translation[0], x.translation[1], x.translation[2]);
                if (x.rotation != null) node.LocalRotation = new Quaternion(x.rotation[0], x.rotation[1], x.rotation[2], x.rotation[3]);
                if (x.scale != null) node.LocalScaling = new Vector3(x.scale[0], x.scale[1], x.scale[2]);
            }
            return node;
        }

        public IEnumerable<int> GetChildNodeIndices(int i)
        {
            var gltfNode = Gltf.nodes[i];
            if (gltfNode.children != null)
            {
                foreach (var j in gltfNode.children)
                {
                    yield return j;
                }
            }
        }

        public int ImageCount => Gltf.images.Count;
        public Image CreateImage(int index)
        {
            var image = Gltf.images[index].FromGltf(this);
            image.GltfIndex = index;
            return image;
        }

        public int TextureCount => Gltf.textures.Count;
        public Texture CreateTexture(int index, List<Image> images)
        {
            var x = Gltf.textures[index];
            var sampler = new GltfTextureSampler();
            if (x.sampler >= 0 && x.sampler < Gltf.samplers.Count)
            {
                sampler = Gltf.samplers[x.sampler];
            }
            return new ImageTexture(x.name ?? "", sampler.FromGltf(), images[x.source], Texture.ColorSpaceTypes.Srgb)
            {
                GltfIndex = index,
            };
        }

        public int MaterialCount => Gltf.materials.Count;

        public Material CreateMaterial(int index, List<Texture> textures)
        {
            var gltfVrm = Gltf.extensions.VRM;
            if (gltfVrm != null
                && gltfVrm.materialProperties != null
                && gltfVrm.materialProperties[index].shader == "VRM/MToon")
            {
                // is mtoon
                var mp = gltfVrm.materialProperties[index];
                var mtoon = MToonMaterialFromGltf.Load(mp, textures);
                mtoon.GltfIndex = index;
                return mtoon;
            }
            else
            {
                // not mtoon, as gltf
                if (gltfVrm.materialProperties[index].tagMap.Keys.Contains("RenderType"))
                {
                    // get transparent property
                    if (gltfVrm.materialProperties[index].tagMap["RenderType"] == "Opaque")
                        Gltf.materials[index].alphaMode = GltfFormat.AlphaModeType.OPAQUE;
                    else if (gltfVrm.materialProperties[index].tagMap["RenderType"] == "TransparentCutout")
                        Gltf.materials[index].alphaMode = GltfFormat.AlphaModeType.MASK;
                    else if (gltfVrm.materialProperties[index].tagMap["RenderType"] == "Transparent")
                        Gltf.materials[index].alphaMode = GltfFormat.AlphaModeType.BLEND;
                    else
                        throw new Exception("Not enough information");
                }
                var x = Gltf.materials[index];
                var material = x.FromGltf(textures);
                material.GltfIndex = index;
                return material;
            }
        }

        public int SkinCount => Gltf.skins.Count;

        public Skin CreateSkin(int index, List<Node> nodes)
        {
            var x = Gltf.skins[index];
            var skin = new Skin
            {
                GltfIndex = index,
                InverseMatrices = this.AccessorFromGltf(x.inverseBindMatrices),
                Joints = x.joints.Select(y => nodes[y]).ToList(),
            };
            if (x.skeleton != -1)
            {
                skin.Root = nodes[x.skeleton];
            }
            return skin;
        }

        public int MeshCount => Gltf.meshes.Count;

        public MeshGroup CreateMesh(int index, List<Material> materials)
        {
            var x = Gltf.meshes[index];
            var group = x.FromGltf(this, materials);
            group.GltfIndex = index;
            return group;
        }

        public (int, int) GetNodeMeshSkin(int index)
        {
            var x = Gltf.nodes[index];
            return (x.mesh, x.skin);
        }

        public int AnimationCount => Gltf.animations.Count;

        public Animation CreateAnimation(int index, List<Node> nodes)
        {
            var x = Gltf.animations[index];
            return x.FromGltf(this, nodes);
        }

        public bool HasVrm => Gltf.extensions != null && Gltf.extensions.VRM != null;
        public Meta CreateVrmMeta(List<Texture> textures)
        {
            return Gltf.extensions.VRM.meta.FromGltf(textures);
        }
        public string VrmExporterVersion => Gltf.extensions.VRM.exporterVersion;
        public string VrmSpecVersion => Gltf.extensions.VRM.specVersion;
        public void LoadVrmHumanoid(List<Node> nodes)
        {
            var gltfVrm = Gltf.extensions.VRM;
            if (gltfVrm.humanoid != null)
            {
                foreach (var humanBone in gltfVrm.humanoid.humanBones)
                {
                    if (humanBone.bone != GltfFormat.HumanoidBones.unknown)
                    {
                        nodes[humanBone.node].HumanoidBone = (VrmLib.HumanoidBones)humanBone.bone;
                    }
                }
            }
        }

        public BlendShapeManager CreateVrmBlendShape(List<MeshGroup> meshGroups, List<Material> materials, List<Node> nodes)
        {
            var gltfVrm = Gltf.extensions.VRM;
            if (gltfVrm.blendShapeMaster != null
                && gltfVrm.blendShapeMaster.blendShapeGroups != null
                && gltfVrm.blendShapeMaster.blendShapeGroups.Any())
            {
                return gltfVrm.blendShapeMaster.FromGltf(meshGroups, materials, nodes);
            }

            return null;
        }

        public SpringBoneManager CreateVrmSpringBone(List<Node> nodes)
        {
            var gltfVrm = Gltf.extensions.VRM;
            if ((gltfVrm.secondaryAnimation is null))
            {
                return null;
            }

            var springBone = new SpringBoneManager();

            // colliders
            springBone.Colliders.AddRange(
                gltfVrm.secondaryAnimation.colliderGroups.Select(y =>
                new SpringBoneColliderGroup(
                    nodes[y.node],
                    y.colliders.Select(z => VrmSpringBoneCollider.CreateSphere(z.offset, z.radius))
                )
            ));

            // springs
            springBone.Springs.AddRange(gltfVrm.secondaryAnimation.boneGroups.Select(x =>
            {
                var sb = new SpringBone();
                sb.Bones.AddRange(x.bones.Select(y => nodes[y]));
                if (x.center >= 0) sb.Origin = nodes[x.center];
                sb.Colliders.AddRange(x.colliderGroups.Select(y => springBone.Colliders[y]));
                sb.Comment = x.comment;
                sb.DragForce = x.dragForce;
                sb.GravityDir = x.gravityDir;
                sb.GravityPower = x.gravityPower;
                sb.HitRadius = x.hitRadius;
                sb.Stiffness = x.stiffiness;
                return sb;
            }));

            return springBone;
        }

        public FirstPerson CreateVrmFirstPerson(List<Node> nodes, List<MeshGroup> meshGroups)
        {
            var gltfVrm = Gltf.extensions.VRM;
            if (gltfVrm.firstPerson == null)
            {
                return null;
            }
            return gltfVrm.firstPerson.FromGltf(nodes, meshGroups);
        }

        public LookAt CreateVrmLookAt()
        {
            var gltfVrm = Gltf.extensions.VRM;
            if (gltfVrm.firstPerson == null)
            {
                return null;
            }
            return gltfVrm.firstPerson.LookAtFromGltf();
        }
    }
}