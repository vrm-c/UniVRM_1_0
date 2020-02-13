using System;
using System.IO;
using UnityEngine;
using UnityEngine.TestTools;

namespace UniVRM10.Test
{
    public class ApiSampleTests
    {
        VrmLib.Model ReadModel(string path)
        {
            var bytes = File.ReadAllBytes(path);

            if (!VrmLib.Glb.TryParse(bytes, out VrmLib.Glb glb, out Exception ex))
            {
                Debug.LogError($"fail to Glb.TryParse: {path} => {ex}");
                return null;
            }

            var storage = new Vrm10.Vrm10Storage(glb.Json.Bytes, glb.Binary.Bytes);
            var model = VrmLib.ModelLoader.Load(storage, path);
            return model;
        }

        GameObject BuildGameObject(VrmLib.Model model)
        {
            // convert Coordinate from Gltf to Unity
            VrmLib.ModelExtensionsForCoordinates.ConvertCoordinate(model, VrmLib.Coordinates.Unity);

            var importer = new UniVRM10.RuntimeUnityBuilder();
            var assets = importer.ToUnityAsset(model);
            UniVRM10.ComponentBuilder.Build10(model, importer, assets);
            return assets.Root;
        }

        VrmLib.Model ToModel(UnityEngine.GameObject target)
        {
            var exporter = new UniVRM10.RuntimeVrmConverter();
            var model = exporter.ToModelFrom10(target);
            return model;
        }

        byte[] ToVrm10(VrmLib.Model model)
        {
            // 右手系に変換
            VrmLib.ModelExtensionsForCoordinates.ConvertCoordinate(model, VrmLib.Coordinates.Gltf);
            var bytes = Vrm10.ModelExtensions.ToGlb(model);
            return bytes;
        }

        [UnityTest]
        public bool Sample()
        {
            var path="Tests/Models/Alicia_vrm-1.00/AliciaSolid_vrm-1.00.vrm";
            Debug.Log($"load: {path}");

            var srcModel = ReadModel(path);
            Debug.Log(srcModel);

            var go = BuildGameObject(srcModel);
            Debug.Log(go);

            var dstModel = ToModel(go);
            Debug.Log(dstModel);

            var vrmBytes = ToVrm10(dstModel);
            Debug.Log($"export {vrmBytes.Length} bytes");

            return true;
        }
    }
}
