using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace UniVRM10
{
    public static class VrmScriptedImporterExtension
    {
        public static Dictionary<string, T> GetExternalUnityObjects<T>(this VrmScriptedImporter importer) where T : UnityEngine.Object
        {
            return importer.GetExternalObjectMap().Where(x => x.Key.type == typeof(T)).ToDictionary(x => x.Key.name , x => (T)x.Value);
        }

        public static void SetExternalUnityObject<T>(this VrmScriptedImporter importer, UnityEditor.AssetImporter.SourceAssetIdentifier sourceAssetIdentifier, T obj) where T : UnityEngine.Object
        {
            importer.AddRemap(sourceAssetIdentifier, obj);
            AssetDatabase.WriteImportSettingsIfDirty(importer.assetPath);
            AssetDatabase.ImportAsset(importer.assetPath, ImportAssetOptions.ForceUpdate);
        }
    }
}