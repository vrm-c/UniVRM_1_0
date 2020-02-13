using System;
using System.IO;
using VrmLib;

namespace UniVRM10
{
    /// <summary>
    /// utility for load VrmLib Model from byte[]
    /// </summary>
    public static class VrmLoader
    {
        /// <summary>
        /// Load VRM10 or VRM0x from path
        /// </summary>
        public static Model CreateVrmModel(string path)
        {
            var bytes = File.ReadAllBytes(path);
            return CreateVrmModel(bytes, new FileInfo(path));
        }

        public static Model CreateVrmModel(byte[] bytes, FileInfo path)
        {
            if (!Glb.TryParse(bytes, out Glb glb, out Exception ex))
            {
                throw ex;
            }

            var flag = VRMVersionCheck.GetVRMExtensionFlag(glb.Json.Bytes);

            if (flag.HasFlag(VRMExtensionFlags.Vrm10))
            {
                var storage = new Vrm10.Vrm10Storage(glb.Json.Bytes, glb.Binary.Bytes);
                var model = ModelLoader.Load(storage, path.Name);
                model.ConvertCoordinate(Coordinates.Unity);
                return model;
            }

            if (flag.HasFlag(VRMExtensionFlags.Vrm0X))
            {
                var storage = new GltfSerialization.GltfStorage(path, glb.Json.Bytes, glb.Binary.Bytes);
                var model = ModelLoader.Load(storage, path.Name);

                // TODO: conversion from VRMMeta0x to VRMMeta10 

                model.ConvertCoordinate(Coordinates.Unity, ignoreVrm: true);
                return model;
            }

            throw new NotImplementedException();
        }
    }
}
