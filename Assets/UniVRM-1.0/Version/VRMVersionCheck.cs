using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace UniVRM10
{
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

        public static bool IsVrm10(byte[] jsonBytes)
        {
            using (var ms = new MemoryStream(jsonBytes))
            {
                var serializer = new DataContractJsonSerializer(typeof(VrmVersionCheck));
                var deserialized = (VrmVersionCheck)serializer.ReadObject(ms);
                if (deserialized.extensions.VRMC_vrm != null)
                {
                    UnityEngine.Debug.Log("specVersion " + deserialized.extensions.VRMC_vrm.specVersion);
                    return true;
                }
                else if (deserialized.extensions.VRM != null)
                {
                    UnityEngine.Debug.Log("specVersion " + deserialized.extensions.VRM.specVersion);
                    return false;
                }
                else
                {
                    throw new Exception("vrm extensions is not found.");
                }
            }
        }
    }
}
