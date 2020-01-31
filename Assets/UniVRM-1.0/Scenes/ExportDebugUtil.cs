using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vrm10;
using VrmLib;


public static class ExportDebugUtil
{

    public static void SaveJson(VrmLib.Model model, string path)
    {
        using (var stream = new System.IO.StreamWriter(path))
        {
            stream.Write(GetJsonString(model));
        }
    }

    public static void SaveVrm(VrmLib.Model model, string path)
    {
        using (var stream = new System.IO.StreamWriter(path))
        {
            stream.Write(GetGlb(model));
        }
    }

    public static string GetJsonString(VrmLib.Model model)
    {
        // export vrm-1.0
        var exporter10 = new Vrm10.Vrm10Exporter();
        var option = new VrmLib.ExportArgs
        {
            // vrm = false
        };
        var glbBytes10 = exporter10.Export(model, option);
        var glb10 = VrmLib.Glb.Parse(glbBytes10);
        return System.Text.Encoding.UTF8.GetString(glb10.Json.Bytes.Array, glb10.Json.Bytes.Offset, glb10.Json.Bytes.Count);
    }

    public static byte[] GetGlb(VrmLib.Model model)
    {
        // export vrm-1.0
        var exporter10 = new Vrm10.Vrm10Exporter();
        var option = new VrmLib.ExportArgs
        {
            // vrm = false
        };
        var glbBytes10 = exporter10.Export(model, option);
        var glb10 = VrmLib.Glb.Parse(glbBytes10);
        return glb10.ToBytes();
    }
}
