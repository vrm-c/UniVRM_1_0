using System;

namespace UniVRM10
{
    public class VRMSpecVersion
    {
        public const int Major = 1;
        public const int Minor = 0;

        public static string Version
        {
            get
            {
                return String.Format("{0}.{1}", Major, Minor);
            }
        }

        public const string VERSION = "1.0";
    }
}
