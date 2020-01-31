using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace VrmLib
{
    /// <summary>
    /// 画像の用途
    /// </summary>
    [Flags]
    public enum ImageUsage
    {
        None,
        Color,
        Normal,
    }

    struct PngChunk
    {
        public readonly string Type;
        public readonly Memory<byte> Data;

        public readonly Memory<byte> CRC;

        public PngChunk(string type, Memory<byte> data, Memory<byte> crc)
        {
            Type = type;
            Data = data;
            CRC = crc;
        }

        public override string ToString()
        {
            return $"{Type}: {Data.Length} bytes";
        }
    }

    static class PngUtil
    {
        static readonly Byte[] PNG_MAGIC = new byte[]
        {
            0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A
        };

        // big endian
        public static int ToInt32BE(Memory<byte> bytes)
        {
            var endian = bytes.Slice(0, 4).ToArray();
            endian.AsSpan().Reverse(); // to LE
            // return BitConverter.ToInt32(endian);
            return endian[0] | (8 << endian[1]) | (16 << endian[2]) | (24 << endian[3]);
        }

        public static IEnumerable<PngChunk> ParseBytes(Memory<byte> bytes)
        {
            if (!bytes.Slice(0, 8).Span.SequenceEqual(PNG_MAGIC.AsSpan()))
            {
                throw new FormatException("is not png");
            }
            bytes = bytes.Slice(8);

            while (bytes.Length > 0)
            {
                var length = ToInt32BE(bytes);
                bytes = bytes.Slice(4);

                var type = bytes.Slice(0, 4);
                MemoryMarshal.TryGetArray(type, out ArraySegment<byte> segment);
                var chunkType = Encoding.ASCII.GetString(segment.Array, segment.Offset, segment.Count);
                bytes = bytes.Slice(4);

                var data = bytes.Slice(0, length);
                bytes = bytes.Slice(length);

                var crc = bytes.Slice(0, 4);
                bytes = bytes.Slice(4);

                yield return new PngChunk(chunkType, data, crc);
            }
        }
    }

    public class Image
    {
        public string Name;
        public string MimeType;
        public Memory<byte> Bytes;

        public ImageUsage Usage;

        public override string ToString()
        {
            if (MimeType == "image/png")
            {
                foreach (var chunk in PngUtil.ParseBytes(Bytes))
                {
                    if (chunk.Type == "IHDR")
                    {

                        var w = PngUtil.ToInt32BE(chunk.Data.Slice(0, 4));
                        var h = PngUtil.ToInt32BE(chunk.Data.Slice(4, 4));
                        return $"{Name}: {MimeType}: {w}x{h}";
                    }
                }
            }

            return $"{Name}: {MimeType}: {Bytes.Length} bytes";
        }

        public Image(string name, string mimeType, ImageUsage usage, Memory<byte> bytes)
        {
            Name = name;
            MimeType = mimeType;
            Bytes = bytes;
            Usage = usage;
        }
    }
}
