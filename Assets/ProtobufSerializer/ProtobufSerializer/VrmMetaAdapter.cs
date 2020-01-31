using System.Collections.Generic;
using VrmLib;

namespace Vrm10
{
    public static class VrmMetaAdapter
    {
        public static AvatarPermission ToAvaterPermission(this VrmProtobuf.Meta self)
        {
            return new AvatarPermission
            {
                AvatarUsage = (AvatarUsageType)self.AvatarPermission,
                IsAllowedViolentUsage = self.ViolentUsage,
                IsAllowedSexualUsage = self.SexualUsage,
                CommercialUsage = (CommercialUsageType)self.CommercialUsage,
                OtherPermissionUrl = self.OtherPermissionUrl,
                IsAllowedGameUsage = self.GameUsage,
                IsAllowedPoliticalOrReligiousUsage = self.PoliticalOrReligiousUsage,
            };
        }

        public static RedistributionLicense ToRedistributionLicense(this VrmProtobuf.Meta self)
        {
            return new RedistributionLicense
            {
                CreditNotation = (CreditNotationType)self.CreditNotation,
                IsAllowRedistribution = self.AllowRedistribution,
                ModificationLicense = (ModificationLicenseType)self.Modify,
                OtherLicenseUrl = self.OtherLicenseUrl,
            };
        }

        public static Meta FromGltf(this VrmProtobuf.Meta self, List<Texture> textures)
        {
            var meta = new Meta
            {
                Name = self.Name,
                Version = self.Version,
                ContactInformation = self.ContactInformation,
                Reference = self.Reference,

                AvatarPermission = ToAvaterPermission(self),
                RedistributionLicense = ToRedistributionLicense(self),
            };
            meta.Authors.AddRange(self.Authors);
            if (self.ThumbnailImage.HasValue)
            {
                var texture = textures[self.ThumbnailImage.Value] as ImageTexture;
                if (texture != null)
                {
                    meta.Thumbnail = texture.Image;
                }
            }

            return meta;
        }

        public static VrmProtobuf.Meta ToGltf(this Meta self, List<Texture> textures)
        {
            var meta = new VrmProtobuf.Meta
            {
                Name = self.Name,
                Version = self.Version,
                ContactInformation = self.ContactInformation,
                Copyrights = self.Copyrights,
                Reference = self.Reference,
                // AvatarPermission
                AvatarPermission = (VrmProtobuf.Meta.Types.AvatarPermissionType)self.AvatarPermission.AvatarUsage,
                ViolentUsage = self.AvatarPermission.IsAllowedViolentUsage,
                SexualUsage = self.AvatarPermission.IsAllowedSexualUsage,
                CommercialUsage = (VrmProtobuf.Meta.Types.CommercialUsageType)self.AvatarPermission.CommercialUsage,
                GameUsage = self.AvatarPermission.IsAllowedGameUsage,
                PoliticalOrReligiousUsage = self.AvatarPermission.IsAllowedPoliticalOrReligiousUsage,
                OtherPermissionUrl = self.AvatarPermission.OtherPermissionUrl,
                // RedistributionLicense
                CreditNotation = (VrmProtobuf.Meta.Types.CreditNotationType)self.RedistributionLicense.CreditNotation,
                AllowRedistribution = self.RedistributionLicense.IsAllowRedistribution,
                Modify = (VrmProtobuf.Meta.Types.ModifyType)self.RedistributionLicense.ModificationLicense,
                OtherLicenseUrl = self.RedistributionLicense.OtherLicenseUrl,
            };
            meta.Authors.AddRange(self.Authors);
            if (self.Thumbnail != null)
            {
                for (int i = 0; i < textures.Count; ++i)
                {
                    var texture = textures[i] as ImageTexture;
                    if (texture.Image == self.Thumbnail)
                    {
                        meta.ThumbnailImage = i;
                        break;
                    }
                }
            }
            return meta;
        }
    }
}