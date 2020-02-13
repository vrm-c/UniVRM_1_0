using System;
using System.Linq;
using System.Runtime.InteropServices;
using GltfFormat;
using GltfSerialization.Generated;
using VrmLib;
using ObjectNotation;

namespace GltfSerialization
{
    public static class GlbExtensions
    {
        static Memory<byte> GetAccessorBytes(Gltf gltf, int accessorIndex, Memory<byte> bin)
        {
            var accessor = gltf.accessors[accessorIndex];
            return GetAccessorBytes(gltf, accessor, bin);
        }

        static Memory<byte> GetAccessorBytes(Gltf gltf, GltfAccessor accessor, Memory<byte> bin)
        {
            var view = gltf.bufferViews[accessor.bufferView];
            var byteSize = accessor.componentType.ByteSize() * accessor.type.TypeCount() * accessor.count;
            return bin.Slice(view.byteOffset, view.byteLength).Slice(accessor.byteOffset, byteSize);
        }

        static Memory<byte> GetTextureBytes(Gltf gltf, int textureIndex, Memory<byte> bin)
        {
            var texture = gltf.textures[textureIndex];
            var source = gltf.images[texture.source];
            var view = gltf.bufferViews[source.bufferView];
            return bin.Slice(view.byteOffset, view.byteLength);
        }

        static bool IsTextureEquals(
            Gltf l, GltfTextureInfo lT, Memory<byte> lB,
            Gltf r, GltfTextureInfo rT, Memory<byte> rB
        )
        {
            if (lT is null && rT is null) return true;
            var lBytes = GetTextureBytes(l, lT.index, lB);
            var rBytes = GetTextureBytes(r, rT.index, rB);
            return lBytes.Span.SequenceEqual(rBytes.Span);
        }

        static bool IsAccessorEquals(
            Gltf l, int ll, Memory<byte> lB,
            Gltf r, int rr, Memory<byte> rB)
        {
            if (ll == -1 && rr == -1) return true;
            var la = l.accessors[ll];
            var ra = r.accessors[rr];
            if (la.componentType == ra.componentType)
            {
                var lBytes = GetAccessorBytes(l, ll, lB);
                var rBytes = GetAccessorBytes(r, rr, rB);
                return lBytes.Span.SequenceEqual(rBytes.Span);
            }
            else
            {
                var lBytes = GetAccessorAsIntArray(l, la, lB);
                var rBytes = GetAccessorAsIntArray(r, ra, rB);
                return lBytes.SequenceEqual(rBytes);
            }
        }

        static Int32[] GetAccessorAsIntArray(Gltf gltf, GltfAccessor a, Memory<byte> bytes)
        {
            if (a.componentType == GltfComponentType.UNSIGNED_INT)
            {
                return MemoryMarshal.Cast<byte, int>(GetAccessorBytes(gltf, a, bytes).Span).ToArray();
            }
            else if (a.componentType == GltfComponentType.UNSIGNED_SHORT
            || a.componentType == GltfComponentType.SHORT)
            {
                var span = MemoryMarshal.Cast<byte, ushort>(GetAccessorBytes(gltf, a, bytes).Span);
                var array = new int[span.Length];
                for (int i = 0; i < span.Length; ++i)
                {
                    array[i] = span[i];
                }
                return array;
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Unitテスト向けにバッファを比較する
        /// </summary>
        public static void AssertEqualBuffers(this Glb lhs, Glb rhs)
        {
            var lJson = lhs.Json.Bytes.ParseAsJson();
            var rJson = rhs.Json.Bytes.ParseAsJson();

            var lGltf = GltfDeserializer.Deserialize(lJson);
            var rGltf = GltfDeserializer.Deserialize(rJson);

            ////////////////////////////////////////////////////////////
            // compare buffer
            // Meshは同数で同じ順番で並んでいるであろう
            ////////////////////////////////////////////////////////////
            var lBytes = lhs.Binary.Bytes;
            var rBytes = rhs.Binary.Bytes;
            if (lGltf.meshes.Count != rGltf.meshes.Count)
            {
                throw new Exception($"differenct mesh count");
            }

            for (var x = 0; x < lGltf.meshes.Count; ++x)
            {
                var lMesh = lGltf.meshes[x];
                var rMesh = rGltf.meshes[x];

                if (lMesh.primitives.Count != rMesh.primitives.Count)
                {
                    throw new Exception($"mesh[{x}]: different primitive count");
                }
                for (var y = 0; y < lMesh.primitives.Count; ++y)
                {
                    var lPrim = lMesh.primitives[y];
                    var rPrim = rMesh.primitives[y];

                    if (!IsAccessorEquals(
                        lGltf, lPrim.indices, lBytes,
                        rGltf, rPrim.indices, rBytes))
                    {
                        throw new Exception($"mesh[{x}].primitives[{y}].indices");
                    }

                    if (!IsAccessorEquals(
                        lGltf, lPrim.attributes.POSITION, lBytes,
                        rGltf, rPrim.attributes.POSITION, rBytes))
                    {
                        throw new Exception($"mesh[{x}].primitives[{y}].attributes.POSITION");
                    }

                    if (!IsAccessorEquals(
                        lGltf, lPrim.attributes.NORMAL, lBytes,
                        rGltf, rPrim.attributes.NORMAL, rBytes))
                    {
                        throw new Exception($"mesh[{x}].primitives[{y}].attributes.NORMAL");
                    }

                    if (!IsAccessorEquals(
                        lGltf, lPrim.attributes.TEXCOORD_0, lBytes,
                        rGltf, rPrim.attributes.TEXCOORD_0, rBytes))
                    {
                        throw new Exception($"mesh[{x}].primitives[{y}].attributes.TEXCOORD_0");
                    }

                    if (!IsAccessorEquals(
                        lGltf, lPrim.attributes.TANGENT, lBytes,
                        rGltf, rPrim.attributes.TANGENT, rBytes))
                    {
                        throw new Exception($"mesh[{x}].primitives[{y}].attributes.TANGENTS");
                    }

                    if (!IsAccessorEquals(
                        lGltf, lPrim.attributes.COLOR_0, lBytes,
                        rGltf, rPrim.attributes.COLOR_0, rBytes))
                    {
                        throw new Exception($"mesh[{x}].primitives[{y}].attributes.COLOR_0");
                    }

                    if (!IsAccessorEquals(
                        lGltf, lPrim.attributes.JOINTS_0, lBytes,
                        rGltf, rPrim.attributes.JOINTS_0, rBytes))
                    {
                        throw new Exception($"mesh[{x}].primitives[{y}].attributes.JOINTS_0");
                    }

                    if (!IsAccessorEquals(
                        lGltf, lPrim.attributes.WEIGHTS_0, lBytes,
                        rGltf, rPrim.attributes.WEIGHTS_0, rBytes))
                    {
                        throw new Exception($"mesh[{x}].primitives[{y}].attributes.WEIGHTS_0");
                    }

                    // マテリアルで使っているテクスチャを比較する
                    var lMaterial = lGltf.materials[lPrim.material];
                    var rMaterial = rGltf.materials[rPrim.material];
                    // baseColor
                    if (!IsTextureEquals(
                        lGltf, lMaterial.pbrMetallicRoughness?.baseColorTexture, lBytes,
                        rGltf, rMaterial.pbrMetallicRoughness?.baseColorTexture, rBytes
                    ))
                    {
                        throw new Exception($"mesh[{x}].primitives[{y}].material.pbrMetallicRoughness.baseColorTexture");
                    }
                    if (!IsTextureEquals(
                        lGltf, lMaterial.pbrMetallicRoughness?.metallicRoughnessTexture, lBytes,
                        rGltf, rMaterial.pbrMetallicRoughness?.metallicRoughnessTexture, rBytes
                    ))
                    {
                        throw new Exception($"mesh[{x}].primitives[{y}].material.pbrMetallicRoughness.metallicRoughnessTexture");
                    }
                    if (!IsTextureEquals(
                        lGltf, lMaterial.emissiveTexture, lBytes,
                        rGltf, rMaterial.emissiveTexture, rBytes
                    ))
                    {
                        throw new Exception($"mesh[{x}].primitives[{y}].material.emissiveTexture");
                    }
                    if (!IsTextureEquals(
                        lGltf, lMaterial.normalTexture, lBytes,
                        rGltf, rMaterial.normalTexture, rBytes
                    ))
                    {
                        throw new Exception($"mesh[{x}].primitives[{y}].material.normalTexture");
                    }
                    if (!IsTextureEquals(
                        lGltf, lMaterial.occlusionTexture, lBytes,
                        rGltf, rMaterial.occlusionTexture, rBytes
                    ))
                    {
                        throw new Exception($"mesh[{x}].primitives[{y}].material.occlusionTexture");
                    }
                }
            }
        }
    }
}