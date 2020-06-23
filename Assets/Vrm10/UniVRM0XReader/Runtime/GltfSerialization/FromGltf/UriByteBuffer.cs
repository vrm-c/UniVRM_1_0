using System;
using System.IO;
using GltfFormat;

namespace GltfSerialization
{
    /// <summary>
    /// for buffer with uri read
    /// </summary>
    public class UriByteBuffer : IBuffer
    {
        string m_baseDir;
        public string Uri
        {
            get;
            private set;
        }

        Byte[] m_bytes;

        public ArraySegment<byte> Bytes
        {
            get
            {
                if (m_bytes == null)
                {
                    m_bytes = ReadFromUri(m_baseDir, Uri);
                }
                return new ArraySegment<byte>(m_bytes);
            }
        }

        Byte[] ReadFromUri(string baseDir, string uri)
        {
            if (TryReadEmbedded(uri, out Byte[] bytes))
            {
                return bytes;
            }
            else
            {
                // as local file path
                return File.ReadAllBytes(Path.Combine(baseDir, uri));
            }
        }

        public UriByteBuffer(string baseDir, string uri)
        {
            m_baseDir = baseDir;
            Uri = uri;
        }

        const string DataPrefix = "data:application/octet-stream;base64,";

        const string DataPrefix2 = "data:application/gltf-buffer;base64,";

        const string DataPrefix3 = "data:image/jpeg;base64,";

        public static bool TryReadEmbedded(string uri, out Byte[] bytes)
        {
            var pos = uri.IndexOf(";base64,");
            if (pos < 0)
            {
                bytes = null;
                return false;
            }
            else
            {
                bytes = Convert.FromBase64String(uri.Substring(pos + 8));
                return true;
            }
        }

        public GltfBufferView Extend(Memory<byte> array, int stride, GltfBufferTargetType target)
        {
            throw new NotImplementedException();
        }

        public void ExtendCapacity(int byteLength)
        {
            throw new NotImplementedException();
        }
    }
}