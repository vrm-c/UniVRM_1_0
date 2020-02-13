using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

namespace UniVRM10
{
    [Flags]
    public enum VRMExtensionFlags
    {
        None = 0,
        Vrm0X = 0x1,
        Vrm10 = 0x2,
    }

    public class VRMVersionCheck
    {
        [DataContract]
        internal class VrmVersionCheck
        {
            [DataContract]
            public class VRM
            {
                [DataMember]
                public string specVersion;
            }

            [DataContract]
            public class VRMC_vrm
            {
                [DataMember]
                public string specVersion;
            }

            [DataContract]
            public class Extensions
            {
                [DataMember]
                public VRMC_vrm VRMC_vrm;
                [DataMember]
                public VRM VRM;
            }

            [DataMember(Name = "extensions")]
            public Extensions extensions;
        }

        public static VRMExtensionFlags GetVRMExtensionFlag(byte[] jsonBytes)
        {
            using (var ms = new MemoryStream(jsonBytes))
            {
                var serializer = new DataContractJsonSerializer(typeof(VrmVersionCheck));
                var deserialized = (VrmVersionCheck)serializer.ReadObject(ms);
                var flag = VRMExtensionFlags.None;
                if (deserialized.extensions.VRMC_vrm != null)
                {
                    Debug.Log("specVersion " + deserialized.extensions.VRMC_vrm.specVersion);
                    flag |= VRMExtensionFlags.Vrm10;
                }

                if (deserialized.extensions.VRM != null)
                {
                    Debug.Log("specVersion " + deserialized.extensions.VRM.specVersion);
                    flag |= VRMExtensionFlags.Vrm0X;
                }

                return flag;
            }
        }
    }
}
