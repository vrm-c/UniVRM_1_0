using System;
using GltfFormat;
using ObjectNotation;

namespace GltfFormat
{
    public enum VrmAllowedUser
    {
        OnlyAuthor,
        ExplicitlyLicensedPerson,
        Everyone,
    }

    public enum VrmLicenseType
    {
        Redistribution_Prohibited,
        CC0,
        CC_BY,
        CC_BY_NC,
        CC_BY_SA,
        CC_BY_NC_SA,
        CC_BY_ND,
        CC_BY_NC_ND,
        Other
    }

    public enum VrmUssageLicense
    {
        Disallow,
        Allow,
    }

    [Serializable]
    [JsonSchema(Title = "vrm.meta")]
    public class VrmMeta : IEquatable<VrmMeta>
    {
        public override string ToString()
        {
            return $"{version}: {title}({author})";
        }

        [JsonSchema(Description = "Title of VRM model")]
        public string title;

        [JsonSchema(Description = "Version of VRM model")]
        public string version;

        [JsonSchema(Description = "Author of VRM model")]
        public string author;

        [JsonSchema(Description = "Contact Information of VRM model author")]
        public string contactInformation;

        [JsonSchema(Description = "Reference of VRM model")]
        public string reference;

        // When the value is -1, it means that texture is not specified.
        [JsonSchema(Description = "Thumbnail of VRM model", Minimum = 0, ExplicitIgnorableValue = -1)]
        public int texture = -1;

        #region Ussage Permission
        [JsonSchema(Required = true, Description = "A person who can perform with this avatar ",
        EnumSerializationType = EnumSerializationType.AsString)]
        public VrmAllowedUser allowedUserName = VrmAllowedUser.OnlyAuthor;

        [JsonSchema(Required = true, Description = "Permission to perform violent acts with this avatar",
        EnumSerializationType = EnumSerializationType.AsString)]
        public VrmUssageLicense violentUssageName = VrmUssageLicense.Disallow;

        [JsonSchema(Required = true, Description = "Permission to perform sexual acts with this avatar",
        EnumSerializationType = EnumSerializationType.AsString)]
        public VrmUssageLicense sexualUssageName = VrmUssageLicense.Disallow;

        [JsonSchema(Required = true, Description = "For commercial use",
        EnumSerializationType = EnumSerializationType.AsString)]
        public VrmUssageLicense commercialUssageName = VrmUssageLicense.Disallow;

        [JsonSchema(Description = "If there are any conditions not mentioned above, put the URL link of the license document here.")]
        public string otherPermissionUrl;
        #endregion

        #region Distribution License
        [JsonSchema(Required = true, Description = "License type",
        EnumSerializationType = EnumSerializationType.AsString)]
        public VrmLicenseType licenseName = VrmLicenseType.Redistribution_Prohibited;

        [JsonSchema(Description = "If “Other” is selected, put the URL link of the license document here.")]
        public string otherLicenseUrl;
        #endregion

        public bool Equals(VrmMeta other)
        {
            if (other is null) return false;
            if (title != other.title) return false;
            if (version != other.version) return false;
            if (author != other.author) return false;
            if (contactInformation != other.contactInformation) return false;
            if (reference != other.reference) return false;
            // if(texture!=other.texture)return false;

            if (allowedUserName != other.allowedUserName) return false;
            if (violentUssageName != other.violentUssageName) return false;
            if (sexualUssageName != other.sexualUssageName) return false;
            if (commercialUssageName != other.commercialUssageName) return false;
            if (otherPermissionUrl != other.otherPermissionUrl) return false;
            if (licenseName != other.licenseName) return false;
            if (otherLicenseUrl != other.otherLicenseUrl) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals((VrmMeta)obj);
        }

        public static bool operator ==(VrmMeta lhs, VrmMeta rhs)
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

        public static bool operator !=(VrmMeta lhs, VrmMeta rhs)
        {
            return !(lhs == rhs);
        }
    }
}
