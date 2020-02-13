using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectNotation;


namespace GltfFormat
{
    // "/extensions"
    [Serializable]
    public class GltfExtensions : IEquatable<GltfExtensions>
    {
        public GltfExtensionVrm VRM;

        public bool Equals(GltfExtensions other)
        {
            if (other is null)
            {
                return false;
            }

            if (VRM is null)
            {
                if (!(other.VRM is null)) return false;
            }
            else
            {
                if (!VRM.Equals(other.VRM)) return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GltfExtensions);
        }

        public static bool operator ==(GltfExtensions lhs, GltfExtensions rhs)
        {
            // Check for null on left side.
            if (lhs is null)
            {
                if (rhs is null)
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }

        public static bool operator !=(GltfExtensions lhs, GltfExtensions rhs)
        {
            return !(lhs == rhs);
        }
    }

    [Serializable]
    public class Gltf : IEquatable<Gltf>
    {
        [JsonSchema(Required = true)]
        public GltfAsset asset = new GltfAsset();

        #region Buffer
        [JsonSchema(MinItems = 1, ExplicitIgnorableItemLength = 0)]
        public List<GltfBuffer> buffers = new List<GltfBuffer>();
        [JsonSchema(MinItems = 1, ExplicitIgnorableItemLength = 0)]
        public List<GltfBufferView> bufferViews = new List<GltfBufferView>();
        [JsonSchema(MinItems = 1, ExplicitIgnorableItemLength = 0)]
        public List<GltfAccessor> accessors = new List<GltfAccessor>();
        #endregion

        [JsonSchema(MinItems = 1, ExplicitIgnorableItemLength = 0)]
        public List<GltfTexture> textures = new List<GltfTexture>();

        [JsonSchema(MinItems = 1, ExplicitIgnorableItemLength = 0)]
        public List<GltfTextureSampler> samplers = new List<GltfTextureSampler>();
        public GltfTextureSampler GetSampler(int index)
        {
            if (samplers.Count == 0)
            {
                samplers.Add(new GltfTextureSampler()); // default sampler
            }

            return samplers[index];
        }

        [JsonSchema(MinItems = 1, ExplicitIgnorableItemLength = 0)]
        public List<GltfImage> images = new List<GltfImage>();

        [JsonSchema(MinItems = 1, ExplicitIgnorableItemLength = 0)]
        public List<GltfMaterial> materials = new List<GltfMaterial>();

        [JsonSchema(MinItems = 1, ExplicitIgnorableItemLength = 0)]
        public List<GltfMesh> meshes = new List<GltfMesh>();

        [JsonSchema(MinItems = 1, ExplicitIgnorableItemLength = 0)]
        public List<GltfNode> nodes = new List<GltfNode>();

        [JsonSchema(MinItems = 1, ExplicitIgnorableItemLength = 0)]
        public List<GltfSkin> skins = new List<GltfSkin>();

        [JsonSchema(Dependencies = new string[] { "scenes" }, Minimum = 0)]
        public int scene;

        [JsonSchema(MinItems = 1, ExplicitIgnorableItemLength = 0)]
        public List<GltfScene> scenes = new List<GltfScene>();
        public int[] rootnodes
        {
            get
            {
                return scenes[scene].nodes;
            }
        }

        [JsonSchema(MinItems = 1, ExplicitIgnorableItemLength = 0)]
        public List<GltfAnimation> animations = new List<GltfAnimation>();

        [JsonSchema(MinItems = 1, ExplicitIgnorableItemLength = 0)]
        public List<GltfCamera> cameras = new List<GltfCamera>();

        [JsonSchema(MinItems = 1, ExplicitIgnorableItemLength = 0)]
        public List<string> extensionsUsed = new List<string>();

        [JsonSchema(MinItems = 1, ExplicitIgnorableItemLength = 0)]
        public List<string> extensionsRequired = new List<string>();

        public GltfExtensions extensions = new GltfExtensions();
        // public GltfExtras extras = new GltfExtras();

        public override string ToString()
        {
            return string.Format("{0}", asset);
        }

        public bool Equals(Gltf other)
        {
            // if (!textures.SequenceEqual(other.textures)) return false;
            if (textures.Count != other.textures.Count) return false;
            // if (!samplers.SequenceEqual(other.samplers)) return false;
            // if (!images.SequenceEqual(other.images)) return false;
            if (images.Count != other.images.Count) return false;

            if (!materials.SequenceEqual(other.materials)) return false;
            if (!meshes.SequenceEqual(other.meshes)) return false;
            if (!nodes.SequenceEqual(other.nodes)) return false;
            if (!skins.SequenceEqual(other.skins)) return false;
            if (!scene.Equals(other.scene)) return false;
            if (!scenes.SequenceEqual(other.scenes)) return false;
            if (!animations.SequenceEqual(other.animations)) return false;

            if (extensions is null)
            {
                if (!(other.extensions is null))
                {
                    return false;
                }
            }
            else
            {
                if (extensions != other.extensions)
                {
                    return false;
                }
            }

            return true;
        }

        bool UsedExtension(string key)
        {
            if (extensionsUsed.Contains(key))
            {
                return true;
            }

            return false;
        }

        static Utf8String s_extensions = Utf8String.From("extensions");

        void Traverse(JsonTreeNode node, JsonFormatter f, Utf8String parentKey)
        {
            if (node.IsObject())
            {
                f.BeginMap();
                foreach (var kv in node.ObjectItems())
                {
                    if (parentKey == s_extensions)
                    {
                        if (!UsedExtension(kv.Key.GetString()))
                        {
                            continue;
                        }
                    }
                    f.Key(kv.Key.GetUtf8String());
                    Traverse(kv.Value, f, kv.Key.GetUtf8String());
                }
                f.EndMap();
            }
            else if (node.IsArray())
            {
                f.BeginList();
                foreach (var x in node.ArrayItems())
                {
                    Traverse(x, f, default(Utf8String));
                }
                f.EndList();
            }
            else
            {
                f.Value(node);
            }
        }

        public string RemoveUnusedExtensions(string json)
        {
            var f = new JsonFormatter();

            Traverse(JsonParser.Parse(json), f, default(Utf8String));

            return f.ToString();
        }

        // public string ToJson()
        // {
        //     var b = new BytesStore();
        //     var f = new JsonFormatter(b);
        //     ObjectNotation.FlattenSerialization.GenericSerializer<Gltf>.GetOrCreate().Serialize(f, this);
        //     return Encoding.UTF8.GetString(b.Bytes);
        // }
    }
}