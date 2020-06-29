using System.Collections.Generic;
using System.Linq;

namespace VrmLib
{
    public class Meta
    {
        public string Name = "";

        public string Version = "";

        // 1.0 added
        public string Copyrights = "";

        // 1.0 added
        public List<string> Authors = new List<string>();

        // backward compatibility
        public string Author
        {
            get => Authors.FirstOrDefault();
            set
            {
                Authors.Clear();
                if (!string.IsNullOrEmpty(value))
                {
                    Authors.Add(value);
                }
            }
        }

        public string ContactInformation = "";

        public string Reference = "";

        public Image Thumbnail;

        public IAvatarPermission AvatarPermission;

        public IRedistributionLicense RedistributionLicense;
    }
}
