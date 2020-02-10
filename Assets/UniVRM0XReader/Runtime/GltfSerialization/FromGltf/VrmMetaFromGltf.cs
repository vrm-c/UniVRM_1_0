using System;
using System.Collections.Generic;
using GltfFormat;
using VrmLib;

namespace GltfSerializationAdapter
{
    public static class VrmMetaFromGltf
    {
        public class AvatarPermission0x : IAvatarPermission
        {
            public AvatarUsageType AvatarUsage { get; set; }

            public bool IsAllowedViolentUsage { get; set; }

            public bool IsAllowedSexualUsage { get; set; }

            public bool IsAllowedCommercialUsage { get; set; }

            public CommercialUsageType CommercialUsage
            {
                get
                {
                    if (IsAllowedCommercialUsage)
                    {
                        return CommercialUsageType.Corporation;
                    }
                    else
                    {
                        return CommercialUsageType.PersonalNonCommercialNonProfit;
                    }
                }
            }

            public bool IsAllowedPoliticalOrReligiousUsage => throw new NotImplementedException();

            public bool IsAllowedGameUsage => throw new NotImplementedException();

            public string OtherPermissionUrl { get; set; }

            public static AvatarPermission0x FromGltf(VrmMeta self)
            {
                return new AvatarPermission0x
                {
                    AvatarUsage = (AvatarUsageType)self.allowedUserName,
                    IsAllowedViolentUsage = self.violentUssageName == VrmUssageLicense.Allow ? true : false,
                    IsAllowedSexualUsage = self.sexualUssageName == VrmUssageLicense.Allow ? true : false,
                    OtherPermissionUrl = self.otherPermissionUrl,
                    IsAllowedCommercialUsage = self.commercialUssageName == VrmUssageLicense.Allow ? true : false,
                };
            }
        }

        public class RedistributionLicense0x : IRedistributionLicense
        {
            public DistributionLicenseType License { get; set; }

            public CreditNotationType CreditNotation => throw new NotImplementedException();

            public bool IsAllowRedistribution => throw new NotImplementedException();

            public ModificationLicenseType ModificationLicense => throw new NotImplementedException();

            public string OtherLicenseUrl { get; set; }

            public static RedistributionLicense0x FromGltf(VrmMeta self)
            {
                return new RedistributionLicense0x
                {
                    License = (DistributionLicenseType)self.licenseName,
                    OtherLicenseUrl = self.otherLicenseUrl,
                };
            }
        }

        public static Meta FromGltf(this VrmMeta self, List<Texture> textures)
        {
            var meta = new Meta
            {
                Name = self.title,
                Version = self.version,
                Author = self.author,
                ContactInformation = self.contactInformation,
                Reference = self.reference,
                AvatarPermission = AvatarPermission0x.FromGltf(self),
                RedistributionLicense = RedistributionLicense0x.FromGltf(self),
            };

            if (self.texture >= 0 && self.texture < textures.Count)
            {
                var texture = textures[self.texture] as ImageTexture;
                if (texture != null)
                {
                    meta.Thumbnail = (textures[self.texture] as ImageTexture).Image;
                }
            }

            return meta;
        }
    }
}
