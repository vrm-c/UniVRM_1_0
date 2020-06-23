using System;
using System.IO;
using GltfFormat;

namespace GltfSerialization
{
    // GltfBuffer backend
    public interface IBuffer
    {
        /// Get buffer bytes
        ArraySegment<Byte> Bytes { get; }

        /// Extend buffer
        GltfBufferView Extend(Memory<byte> array, int stride, GltfBufferTargetType target);

        /// extend internal buffer
        void ExtendCapacity(int byteLength);
    }
}
