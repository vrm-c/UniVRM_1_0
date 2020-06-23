using System;
using System.Linq;
using ObjectNotation;

namespace GltfFormat
{
    [Serializable]
    public class GltfBuffer
    {
        public string uri;

        [JsonSchema(Required = true, Minimum = 1)]
        public int byteLength;

        // empty schemas
        public object extensions;
        public object extras;
        public string name;
    }
}
