using System;

namespace Vrm10
{
    /// <summary>
    /// for exporter
    /// </summary>
    public class ArrayByteBuffer
    {
        public ArraySegment<byte> Bytes
        {
            get
            {
                if (m_bytes == null)
                {
                    return new ArraySegment<byte>();
                }

                return new ArraySegment<byte>(m_bytes, 0, m_used);
            }
        }

        Byte[] m_bytes;
        int m_used = 0;

        public ArrayByteBuffer(Byte[] bytes = null)
        {
            m_bytes = bytes ?? (new byte[] { });
        }

        public ArrayByteBuffer(Byte[] bytes, int used)
        {
            m_bytes = bytes ?? (new byte[] { });
            m_used = used;
        }


        public void ExtendCapacity(int byteLength)
        {
            var backup = m_bytes;
            m_bytes = new byte[backup.Length + byteLength];
            backup.CopyTo(m_bytes, backup.Length);
        }

        public void Extend(Memory<byte> array, int stride, out int offset, out int length)
        {
            var tmp = m_bytes;
            // alignment
            var padding = m_used % stride == 0 ? 0 : stride - m_used % stride;

            if (m_bytes == null || m_used + padding + array.Length > m_bytes.Length)
            {
                // recreate buffer
                var newSize = Math.Max(m_used + padding + array.Length, m_bytes.Length * 2);
                m_bytes = new Byte[newSize];
                if (m_used > 0)
                {
                    // Buffer.BlockCopy(tmp, 0, m_bytes, 0, m_used);
                    tmp.AsSpan().Slice(0, m_used).CopyTo(m_bytes);
                }
            }
            if (m_used + padding + array.Length > m_bytes.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            array.CopyTo(m_bytes.AsMemory().Slice(m_used + padding, array.Length));

            length = array.Length;
            offset = m_used + padding;

            // var result = new GltfBufferView
            // {
            //     buffer = 0,
            //     byteLength = array.Length,
            //     byteOffset = m_used + padding,
            //     target = target,
            // };
            // if (target == GltfBufferTargetType.ARRAY_BUFFER)
            // {
            //     result.byteStride = stride;
            // }

            m_used = m_used + padding + array.Length;
            // return result;
        }
    }
}