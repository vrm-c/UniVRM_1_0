using System.Collections.Generic;
using VrmLib;

namespace Vrm10
{
    public static class VrmMetaAdapter
    {
        public static Meta FromGltf(this VrmProtobuf.Meta self, List<Texture> textures)
        {
            var meta = new Meta
            {
                Title = self.Title,
                Version = self.Version,
                Author = self.Author,
                ContactInformation = self.ContactInformation,
                Reference = self.Reference,
                AllowedUser = EnumUtil.Parse<MetaAllowedUser>(self.AllowedUser),
                IsAllowedViolentUsage = self.ViolentUsage == "Allow" ? true : false,
                IsAllowedSexualUsage = self.SexualUsage == "ALlow" ? true : false,
                IsAllowedCommercialUsage = self.CommercialUsage == "Allow" ? true : false,
                OtherPermissionUrl = self.OtherPermissionUrl,
                License = EnumUtil.Parse<MetaLicenseType>(self.License),
                OtherLicenseUrl = self.OtherLicenseUrl,
            };

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
                Title = self.Title,
                Version = self.Version,
                Author = self.Author,
                ContactInformation = self.ContactInformation,
                Reference = self.Reference,
                AllowedUser = self.AllowedUser.ToString(),
                ViolentUsage = self.IsAllowedViolentUsage ? "Allow" : "Disallow",
                SexualUsage = self.IsAllowedSexualUsage ? "Allow" : "Disallow",
                CommercialUsage = self.IsAllowedCommercialUsage ? "Allow" : "Disallow",
                OtherPermissionUrl = self.OtherPermissionUrl,
                License = self.License.ToString(),
                OtherLicenseUrl = self.OtherLicenseUrl,
            };
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