using System;
using System.IO;
using UnityEngine;
using VrmLib;
using Vrm10;

public class Sample : MonoBehaviour
{
    [SerializeField]
    string m_vrmPath = "Tests/Models/Alicia_vrm-0.51/AliciaSolid_vrm-0.51.vrm";

    ///
    /// UniVRM-0.XX のバイト列からロードする
    ///
    static UniVRM10.ModelAsset Import0X(string path)
    {
        var bytes = File.ReadAllBytes(path);
        if (!Glb.TryParse(bytes, out Glb glb, out Exception ex))
        {
            throw ex;
        }

        var storage = new GltfSerialization.GltfStorage(new FileInfo(path), glb.Json.Bytes, glb.Binary.Bytes);
        var model = ModelLoader.Load(storage, Path.GetFileName(path));
        Debug.Log($"ModelLoader.Load: {model}");
        Debug.Log(model);

        // 左手系に変換
        model.ConvertCoordinate(VrmLib.Coordinates.Unity, ignoreVrm: true);

        // UniVRM-0.XXのコンポーネントを構築する
        var importer = new UniVRM10.RuntimeUnityBuilder();
        var assets = importer.ToUnityAsset(model);
        UniVRM10.ComponentBuilder.Build10(model, importer, assets);

        return assets;
    }

    UniVRM10.ModelAsset Import10(byte[] bytes, string rootName)
    {
        if (!Glb.TryParse(bytes, out Glb glb, out Exception ex))
        {
            throw ex;
        }

        // Import
        var storage = new Vrm10.Vrm10Storage(glb.Json.Bytes, glb.Binary.Bytes);
        var model = ModelLoader.Load(storage, rootName);
        Debug.Log($"ModelLoader.Load: {model}");
        Debug.Log(model);

        // 左手系に変換
        model.ConvertCoordinate(VrmLib.Coordinates.Unity);

        // UniVRM-0.XXのコンポーネントを構築する
        var importer = new UniVRM10.RuntimeUnityBuilder();
        var assets = importer.ToUnityAsset(model);
        UniVRM10.ComponentBuilder.Build10(model, importer, assets);

        return assets;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        var vrm0x = Import0X(m_vrmPath);

        // Export 1.0
        var exporter = new UniVRM10.RuntimeVrmConverter();
        var model = exporter.ToModelFrom10(vrm0x.Root);
        // 右手系に変換
        model.ConvertCoordinate(VrmLib.Coordinates.Gltf);
        var exportedBytes = model.ToGlb();

        // Import 1.0
        var vrm10 = Import10(exportedBytes, vrm0x.Root.name);
        var pos = vrm10.Root.transform.position;
        pos.x += 1.5f;
        vrm10.Root.transform.position = pos;
        vrm10.Root.name = vrm10.Root.name + "_Imported_v1_0";

        // write
        var path = Path.GetFullPath("vrm10.vrm");
        Debug.Log($"write : {path}");
        File.WriteAllBytes(path, exportedBytes);
    }

    static void Printmatrices(Model model)
    {
        var matrices = model.Skins[0].InverseMatrices.GetSpan<System.Numerics.Matrix4x4>();
        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < matrices.Length; ++i)
        {
            var m = matrices[i];
            sb.AppendLine($"#{i:00}[{m.M11:.00}, {m.M12:.00}, {m.M13:.00}, {m.M14:.00}][{m.M21:.00}, {m.M22:.00}, {m.M23:.00}, {m.M24:.00}][{m.M31:.00}, {m.M32:.00}, {m.M33:.00}, {m.M34:.00}][{m.M41:.00}, {m.M42:.00}, {m.M43:.00}, {m.M44:.00}]");
        }
        Debug.Log(sb.ToString());
    }
}
