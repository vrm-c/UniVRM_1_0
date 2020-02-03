using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace UniVRM10
{
    public class VRMMetaObject : ScriptableObject
    {
        [SerializeField]
        public string ExporterVersion;

        #region Info
        [SerializeField]
        public string Name;

        [SerializeField]
        public string Version;

        [SerializeField]
        public string Copyrights;

        [SerializeField]
        public string[] Authors;

        [SerializeField]
        public string ContactInformation;

        [SerializeField]
        public string Reference;

        [SerializeField]
        public Texture2D Thumbnail;
        #endregion

        #region AvatarPermission
        [SerializeField, Tooltip("A person who can perform with this avatar")]
        public VrmLib.AvatarUsageType AllowedUser;

        [SerializeField, Tooltip("Violent acts using this avatar")]
        public bool ViolentUsage;

        [SerializeField, Tooltip("Sexuality acts using this avatar")]
        public bool SexualUsage;

        [SerializeField, Tooltip("For commercial use")]
        public VrmLib.CommercialUsageType CommercialUsage;

        [SerializeField]
        public bool GameUsage;

        [SerializeField]
        public bool PoliticalOrReligiousUsage;

        [SerializeField, Tooltip("Other License Url")]
        public string OtherPermissionUrl;
        #endregion

        #region Distribution License
        [SerializeField]
        public VrmLib.CreditNotationType CreditNotation;

        [SerializeField]
        public bool Redistribution;

        [SerializeField]
        public VrmLib.ModificationLicenseType ModificationLicense;

        [SerializeField]
        public string OtherLicenseUrl;
        #endregion

        public IEnumerable<string> Validate()
        {
            if (string.IsNullOrEmpty(Name))
            {
                yield return $"require Name";
            }
            if (string.IsNullOrEmpty(Version))
            {
                yield return $"require Version";
            }
            if (Authors == null || Authors.All(x => string.IsNullOrEmpty(x)))
            {
                yield return $"require Authors";
            }
        }
    }
}
