using System;
using System.IO;
using VrmLib;

namespace UniVRM10
{
    /// <summary>
    /// utility for load VrmLib Model from byte[]
    /// </summary>
    public static class VrmLoader
    {
        // TODO:
        const string VRM0X_LICENSE_URL = "https://vrm-consortium.org/";

        /// <summary>
        /// Load VRM10 or VRM0x from path
        /// </summary>
        public static Model CreateVrmModel(string path)
        {
            var bytes = File.ReadAllBytes(path);
            return CreateVrmModel(bytes, new FileInfo(path));
        }

        public static Model CreateVrmModel(byte[] bytes, FileInfo path)
        {
            if (!Glb.TryParse(bytes, out Glb glb, out Exception ex))
            {
                throw ex;
            }

            var flag = VRMVersionCheck.GetVRMExtensionFlag(glb.Json.Bytes);

            if (flag.HasFlag(VRMExtensionFlags.Vrm10))
            {
                var storage = new Vrm10.Vrm10Storage(glb.Json.Bytes, glb.Binary.Bytes);
                var model = ModelLoader.Load(storage, path.Name);
                model.ConvertCoordinate(Coordinates.Unity);
                return model;
            }

            if (flag.HasFlag(VRMExtensionFlags.Vrm0X))
            {
                var storage = new GltfSerialization.GltfStorage(path, glb.Json.Bytes, glb.Binary.Bytes);
                var model = ModelLoader.Load(storage, path.Name);

                // convert meta frm 0x to 10
                var meta0x = model.Vrm.Meta.AvatarPermission;
                model.Vrm.Meta.AvatarPermission = new AvatarPermission
                {
                    AvatarUsage = meta0x.AvatarUsage,
                    CommercialUsage = meta0x.CommercialUsage,
                    IsAllowedGameUsage = meta0x.IsAllowedGameUsage,
                    IsAllowedPoliticalOrReligiousUsage = meta0x.IsAllowedPoliticalOrReligiousUsage,
                    IsAllowedSexualUsage = meta0x.IsAllowedSexualUsage,
                    IsAllowedViolentUsage = meta0x.IsAllowedViolentUsage,
                    OtherPermissionUrl = meta0x.OtherPermissionUrl,
                };
                model.Vrm.Meta.RedistributionLicense = new RedistributionLicense
                {
                    OtherLicenseUrl = VRM0X_LICENSE_URL,
                };
                UnityEngine.Debug.LogWarning($"convert {model.Vrm.ExporterVersion} to 1.0. please update meta information");

                model.ConvertCoordinate(Coordinates.Unity, ignoreVrm: true);
                return model;
            }

            throw new NotImplementedException();
        }
    }
}
