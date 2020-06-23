using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ObjectNotation;

namespace GltfFormat
{
    [Serializable]
    [JsonSchema(Title = "vrm", Description = @"
VRM extension is for 3d humanoid avatars (and models) in VR applications.
")]
    public class GltfExtensionVrm : IEquatable<GltfExtensionVrm>
    {
        public static string ExtensionName
        {
            get
            {
                return "VRM";
            }
        }

        [JsonSchema(Description = @"Version of exporter that vrm created. ")]
        public string exporterVersion;

        [JsonSchema(Description = @"Version of VRM specification. ")]
        public string specVersion;

        public VrmMeta meta = new VrmMeta();
        public VrmHumanoid humanoid = new VrmHumanoid();
        public VrmFirstPerson firstPerson = new VrmFirstPerson();
        public VrmBlendShapeMaster blendShapeMaster = new VrmBlendShapeMaster();
        public VrmSecondaryAnimation secondaryAnimation = new VrmSecondaryAnimation();
        public List<VrmMaterial> materialProperties = new List<VrmMaterial>();

        public bool Equals(GltfExtensionVrm other)
        {
            if (other is null) return false;

            if (exporterVersion != other.exporterVersion) return false;
            if (specVersion != other.specVersion) return false;

            if (meta != other.meta) return false;
            if (humanoid != other.humanoid) return false;
            if (!materialProperties.SequenceEqual(other.materialProperties)) return false;
            if (blendShapeMaster != other.blendShapeMaster) return false;
            if (secondaryAnimation != other.secondaryAnimation) return false;
            if (firstPerson != other.firstPerson) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals((GltfExtensionVrm)obj);
        }

        public static bool operator ==(GltfExtensionVrm lhs, GltfExtensionVrm rhs)
        {
            // Check for null on left side.
            if (lhs is null)
            {
                if (rhs is null)
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }

        public static bool operator !=(GltfExtensionVrm lhs, GltfExtensionVrm rhs)
        {
            return !(lhs == rhs);
        }
    }
}
