using System.Collections.Generic;
using GltfFormat;
using VrmLib.MToon;
using VrmLib;

namespace GltfSerializationAdapter
{
    public static class MToonMaterialFromGltf
    {
        public static MToonMaterial Load(VrmMaterial mp, List<Texture> textures)
        {
            var mtoon = new MToonMaterial(mp.name);
            if (mp.floatProperties.TryGetValue(Utils.PropDebugMode, out float value))
            {
                mtoon._DebugMode = value;
            }
            mtoon.Definition = MToon.Utils.FromVrm0x(mp, textures);
            return mtoon;
        }
    }
}
