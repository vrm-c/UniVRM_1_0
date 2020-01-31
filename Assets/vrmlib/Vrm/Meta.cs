namespace VrmLib
{
    public enum MetaAllowedUser
    {
        OnlyAuthor,
        ExplicitlyLicensedPerson,
        Everyone,
    }

    public enum MetaLicenseType
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

    public class Meta
    {
        public string Title = "";

        public string Version = "";

        public string Author = "";

        public string ContactInformation = "";

        public string Reference = "";

        public Image Thumbnail;

        #region Ussage Permission
        public MetaAllowedUser AllowedUser;

        public bool IsAllowedViolentUsage;

        public bool IsAllowedSexualUsage;

        public bool IsAllowedCommercialUsage;

        public string OtherPermissionUrl = "";
        #endregion

        #region Distribution License
        public MetaLicenseType License;

        public string OtherLicenseUrl = "";
        #endregion
    }
}
