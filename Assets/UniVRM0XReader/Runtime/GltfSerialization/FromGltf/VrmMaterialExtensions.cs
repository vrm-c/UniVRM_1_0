using System;
using System.Collections.Generic;
using System.Numerics;
using GltfFormat;
using VrmLib;

namespace VrmLib.MToon
{
    public static class VrmMaterialExtensions
    {
        public static bool IsKeywordEnabled(this VrmMaterial m, string key)
        {
            bool enable;
            if (m.keywordMap.TryGetValue(key, out enable))
            {
                return enable;
            }
            return false;
        }

        public static void EnableKeyword(this VrmMaterial m, string key)
        {
            m.keywordMap[key] = true;
        }

        public static void DisableKeyword(this VrmMaterial m, string key)
        {
            m.keywordMap[key] = false;
        }

        public static void SetOverrideTag(this VrmMaterial m, string key, string value)
        {
            m.tagMap[key] = value;
        }

        public static int GetInt(this VrmMaterial m, string key)
        {
            float value;
            if (m.floatProperties.TryGetValue(key, out value))
            {
                return (int)value;
            }
            return 0;
        }

        public static void SetInt(this VrmMaterial m, string key, int value)
        {
            m.floatProperties[key] = value;
        }

        public static float GetFloat(this VrmMaterial m, string key)
        {
            float value;
            if (m.floatProperties.TryGetValue(key, out value))
            {
                return value;
            }
            return 0;
        }

        public static void SetFloat(this VrmMaterial m, string key, float value)
        {
            m.floatProperties[key] = value;
        }

        public static Vector2 GetTextureScale(this VrmMaterial m, string key)
        {
            float[] value;
            if (m.vectorProperties.TryGetValue(key, out value))
            {
                return new Vector2(value[2], value[3]);
            }
            return Vector2.One;
        }

        public static void SetTextureScale(this VrmMaterial m, string key, Vector2 value)
        {
            float[] old;
            if (!m.vectorProperties.TryGetValue(key, out old))
            {
                old = new float[] { 0, 0, 1, 1 };
            }
            m.vectorProperties[key] = new float[] { old[0], old[1], value.X, value.Y };
        }

        public static Vector2 GetTextureOffset(this VrmMaterial m, string key)
        {
            float[] value;
            if (m.vectorProperties.TryGetValue(key, out value))
            {
                return new Vector2(value[0], value[1]);
            }
            return Vector2.Zero;
        }

        public static void SetTextureOffset(this VrmMaterial m, string key, Vector2 value)
        {
            float[] old;
            if (!m.vectorProperties.TryGetValue(key, out old))
            {
                old = new float[] { 0, 0, 1, 1 };
            }
            m.vectorProperties[key] = new float[] { value.X, value.Y, old[2], old[3] };
        }

        public static Vector4 GetColor(this VrmMaterial m, string key)
        {
            float[] value;
            if (m.vectorProperties.TryGetValue(key, out value))
            {
                return new Vector4(value[0], value[1], value[2], value[3]);
            }
            return Vector4.Zero; // black ?
        }

        public static void SetColor(this VrmMaterial m, string key, Vector4 value)
        {
            m.vectorProperties[key] = new float[] { value.X, value.Y, value.Z, value.W };
        }

        public static RenderMode GetRenderMode(this VrmMaterial m)
        {
            throw new NotImplementedException();
        }

        public static BlendMode GetBlendMode(this VrmMaterial m)
        {
            throw new NotImplementedException();
        }

        public static TextureInfo GetTexture(this VrmMaterial m, string key, List<Texture> textures)
        {
            if (m.textureProperties.TryGetValue(key, out int value))
            {
                var texture = new TextureInfo(textures[value]);
                if (m.vectorProperties.TryGetValue(key, out float[] offsetScaling))
                {
                    texture.OffsetScaling = offsetScaling;
                }
                return texture;
            }

            return null;
        }

        public static void SetTexture(this VrmMaterial m, string key, TextureInfo value, List<Texture> textures)
        {
            if (value is null)
            {
                return;
            }
            var index = textures.IndexOf(value.Texture);
            if (index != -1)
            {
                m.textureProperties[key] = index;
                m.vectorProperties[key] = value.OffsetScaling;
            }
        }
    }
}