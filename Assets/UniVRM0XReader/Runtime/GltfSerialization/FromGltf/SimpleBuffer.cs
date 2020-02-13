using System;
using GltfFormat;

namespace GltfSerialization
{
    public class SimpleBuffer : IBuffer
    {
        ArraySegment<Byte> m_bytes;

        public SimpleBuffer() : this(new ArraySegment<byte>())
        {
        }

        public SimpleBuffer(ArraySegment<Byte> bytes)
        {
            m_bytes = bytes;
        }

        public ArraySegment<byte> Bytes => m_bytes;

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
