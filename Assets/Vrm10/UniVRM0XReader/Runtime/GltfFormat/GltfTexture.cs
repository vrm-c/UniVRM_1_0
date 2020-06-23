using System;
using System.IO;
using ObjectNotation;

namespace GltfFormat
{
    [Serializable]
    public class GltfImage
    {
        public string name;
        public string uri;

        [JsonSchema(Dependencies = new string[] { "mimeType" }, Minimum = 0)]
        public int bufferView;

        [JsonSchema(EnumValues = new object[] { "image/jpeg", "image/png", "image/astc" }, EnumSerializationType = EnumSerializationType.AsString)]
        public string mimeType;

        public string GetExt()
        {
            switch (mimeType)
            {
                case "image/png":
                    return ".png";

                case "image/jpeg":
                    return ".jpg";

                case "image/astc":
                    return ".astc";

                case "image/webp":
                    return ".webp";

                case "image/vnd-ms.dds":
                    return ".dds";
                default:
                    if (uri.StartsWith("data:image/jpeg;"))
                    {
                        return ".jpg";
                    }
                    else if (uri.StartsWith("data:image/png;"))
                    {
                        return ".png";
                    }
                    else if (uri.StartsWith("data:image/astc;"))
                    {
                        return ".astc";
                    }
                    else if (uri.StartsWith("data:image/webp;"))
                    {
                        return ".webp";
                    }
                    else if (uri.StartsWith("data:image/vnd-ms.dds;"))
                    {
                        return ".dds";
                    }
                    else
                    {
                        return Path.GetExtension(uri).ToLower();
                    }
            }
        }

        // empty schemas
        public object extensions;
        public object extras;
    }

    [Serializable]
    public class GltfTexture
    {
        [JsonSchema(Minimum = 0)]
        public int sampler;

        [JsonSchema(Minimum = 0)]
        public int source;

        // empty schemas
        public GltfTextureExtensions extensions;
        public object extras;
        public string name;
    }

    [Serializable]
    public class GltfTextureExtensions
    {
        public EXT_texture_astc EXT_texture_astc;

        public EXT_texture_webp EXT_texture_webp;

        public MSFT_texture_dds MSFT_texture_dds;

    }

    [Serializable]
    public class EXT_texture_astc
    {
        [JsonSchema(Minimum = 0)]
        public int source;
    }

    [Serializable]
    public class EXT_texture_webp
    {
        [JsonSchema(Minimum = 0)]
        public int source;
    }

    [Serializable]
    public class MSFT_texture_dds
    {
        [JsonSchema(Minimum = 0)]
        public int source;
    }
}
