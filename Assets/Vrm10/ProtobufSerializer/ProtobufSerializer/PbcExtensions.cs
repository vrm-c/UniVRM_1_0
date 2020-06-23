using System;
using System.Collections.Generic;
using System.Numerics;
using Google.Protobuf.Collections;
using VrmLib;
using pbc = global::Google.Protobuf.Collections;


namespace Vrm10
{
    public static class PbcExtensions
    {
        public static Vector2 ToVector2(this pbc::RepeatedField<float> src)
        {
            if (src.Count != 2) throw new System.Exception();

            var v = new Vector2();
            v.X = src[0];
            v.Y = src[1];
            return v;
        }

        public static Vector3 ToVector3(this pbc::RepeatedField<float> src)
        {
            if (src.Count != 3) throw new System.Exception();

            var v = new Vector3();
            v.X = src[0];
            v.Y = src[1];
            v.Z = src[2];
            return v;
        }

        public static TextureInfo GetTexture(this int? nullable, List<Texture> textures)
        {
            if (!nullable.TryGetValidIndex(textures.Count, out int index))
            {
                return null;
            }
            var texture = textures[index];
            return new TextureInfo(texture);
        }

        public static int? ToIndex(this TextureInfo texture, List<Texture> textures)
        {
            if (texture == null)
            {
                return default;
            }
            return textures.IndexOfThrow(texture.Texture);
        }

        public static Vector4 ToVector4(this RepeatedField<float> src, Vector4 defaultValue)
        {
            switch (src.Count)
            {
                case 4:
                    return new Vector4(src[0], src[1], src[2], src[3]);
                case 3:
                    return new Vector4(src[0], src[1], src[2], 1.0f);
                case 0:
                    return defaultValue;
                default:
                    throw new Exception();
            }
        }

        public static LinearColor ToLinearColor(this RepeatedField<float> src, Vector4 defaultValue)
        {
            switch (src.Count)
            {
                case 4:
                    return LinearColor.FromLiner(src[0], src[1], src[2], src[3]);
                case 3:
                    return LinearColor.FromLiner(src[0], src[1], src[2], 1.0f);
                case 0:
                    return LinearColor.FromLiner(defaultValue);
                default:
                    throw new Exception();
            }
        }

        public static void Assign(this RepeatedField<float> f, in Vector2 src)
        {
            f.Add(src.X);
            f.Add(src.Y);
        }

        public static void Assign(this RepeatedField<float> f, in Vector4 src)
        {
            f.Add(src.X);
            f.Add(src.Y);
            f.Add(src.Z);
            f.Add(src.W);
        }

        public static void Assign(this RepeatedField<float> f, in LinearColor src)
        {
            f.Add(src.RGBA.X);
            f.Add(src.RGBA.Y);
            f.Add(src.RGBA.Z);
            f.Add(src.RGBA.W);
        }
    }
}
