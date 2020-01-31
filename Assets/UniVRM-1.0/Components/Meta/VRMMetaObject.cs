using System.Collections.Generic;
using UnityEngine;


namespace UniVRM10
{
    public class VRMMetaObject : ScriptableObject
    {
        [SerializeField]
        public string ExporterVersion;

        #region Info
        [SerializeField]
        public string Title;

        [SerializeField]
        public string Version;

        [SerializeField]
        public string Author;

        [SerializeField]
        public string ContactInformation;

        [SerializeField]
        public string Reference;

        [SerializeField]
        public Texture2D Thumbnail;
        #endregion

        #region Permission
        [SerializeField, Tooltip("A person who can perform with this avatar")]
        public VrmLib.MetaAllowedUser AllowedUser;

        [SerializeField, Tooltip("Violent acts using this avatar")]
        public bool ViolentUssage;

        [SerializeField, Tooltip("Sexuality acts using this avatar")]
        public bool SexualUssage;

        [SerializeField, Tooltip("For commercial use")]
        public bool CommercialUssage;

        [SerializeField, Tooltip("Other License Url")]
        public string OtherPermissionUrl;
        #endregion

        #region Distribution License
        [SerializeField]
        public VrmLib.MetaLicenseType LicenseType;

        [SerializeField]
        public string OtherLicenseUrl;
        #endregion

        public IEnumerable<string> Validate()
        {
            if (string.IsNullOrEmpty(Title))
            {
                yield return $"require Title";
            }
            if (string.IsNullOrEmpty(Version))
            {
                yield return $"require Version";
            }
            if (string.IsNullOrEmpty(Author))
            {
                yield return $"require Author";
            }
        }
    }
}
