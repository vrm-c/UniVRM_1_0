using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;
using VrmLib;

namespace UniVRM10
{

    [ScriptedImporter(1, "glb")]
    public class GltfScriptedImporter : ScriptedImporter, IExternalUnityObject
    {
        const string TextureDirName = "Textures";
        const string MaterialDirName = "Materials";

        public override void OnImportAsset(AssetImportContext ctx)
        {
            Debug.Log("OnImportAsset to " + ctx.assetPath);

            try
            {
                // Create model
                VrmLib.Model model = CreateGlbModel(ctx.assetPath);
                Debug.Log($"ModelLoader.Load: {model}");

                // Build Unity Model
                var builder = new UniVRM10.EditorUnityBuilder();
                var assets = builder.ToUnityAsset(model, assetPath, this);

                // Texture
                var externalTextures = this.GetExternalUnityObjects<UnityEngine.Texture2D>();
                foreach (var texture in assets.Textures)
                {
                    if (texture == null)
                        continue;

                    if (externalTextures.ContainsValue(texture))
                    {
                    }
                    else
                    {
                        ctx.AddObjectToAsset(texture.name, texture);
                    }
                }

                // Material
                var externalMaterials = this.GetExternalUnityObjects<UnityEngine.Material>();
                foreach (var material in assets.Materials)
                {
                    if (material == null)
                        continue;

                    if (externalMaterials.ContainsValue(material))
                    {

                    }
                    else
                    {
                        ctx.AddObjectToAsset(material.name, material);
                    }
                }

                // Mesh
                foreach (var mesh in assets.Meshes)
                {
                    ctx.AddObjectToAsset(mesh.name, mesh);
                }

                // Root
                ctx.AddObjectToAsset(assets.Root.name, assets.Root);
                ctx.SetMainObject(assets.Root);

            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        private Model CreateGlbModel(string path)
        {
            var fileInfo = new FileInfo(path);
            var bytes = File.ReadAllBytes(path);
            if (!VrmLib.Glb.TryParse(bytes, out VrmLib.Glb glb, out Exception ex))
            {
                throw ex;
            }

            VrmLib.Model model = null;
            VrmLib.IVrmStorage storage;
            storage = new Vrm10.Vrm10Storage(fileInfo, glb.Json.Bytes, glb.Binary.Bytes);
            model = VrmLib.ModelLoader.Load(storage);
            model.ConvertCoordinate(VrmLib.Coordinates.Unity, ignoreVrm: true);

            return model;
        }

        public void ExtractTextures()
        {
            ExtractTextures(TextureDirName);
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        public void ExtractMaterials()
        {
            this.ExtractAssets<UnityEngine.Material>(MaterialDirName, ".mat");
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        public void ExtractMaterialsAndTextures()
        {
            ExtractTextures(TextureDirName, () => { this.ExtractAssets<UnityEngine.Material>(MaterialDirName, ".mat"); });
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        private void ExtractTextures(string dirName, Action onComplited = null)
        {
            if (string.IsNullOrEmpty(assetPath))
                return;

            var subAssets = this.GetSubAssets<UnityEngine.Texture2D>(assetPath);

            var path = string.Format("{0}/{1}.{2}",
                Path.GetDirectoryName(assetPath),
                Path.GetFileNameWithoutExtension(assetPath),
                dirName
                );

            this.SafeCreateDirectory(path);

            Dictionary<VrmLib.ImageTexture, string> targetPaths = new Dictionary<VrmLib.ImageTexture, string>();

            // Reload Model
            var model = CreateGlbModel(assetPath);
            var mimeTypeReg = new System.Text.RegularExpressions.Regex("image/(?<mime>.*)$");
            int count = 0;
            foreach (var texture in model.Textures)
            {
                var imageTexture = texture as VrmLib.ImageTexture;
                if (imageTexture == null) continue;

                var mimeType = mimeTypeReg.Match(imageTexture.Image.MimeType);
                var targetPath = string.Format("{0}/{1}.{2}", 
                    path, 
                    !string.IsNullOrEmpty(imageTexture.Name)? imageTexture.Name:string.Format("{0}_img{1}", model.Name, count) , 
                    mimeType.Groups["mime"].Value);
                File.WriteAllBytes(targetPath, imageTexture.Image.Bytes.ToArray());
                AssetDatabase.ImportAsset(targetPath);
                targetPaths.Add(imageTexture, targetPath);

                count++;
            }

            EditorApplication.delayCall += () =>
            {
                foreach (var targetPath in targetPaths)
                {
                    var imageTexture = targetPath.Key;
                    if(string.IsNullOrEmpty(imageTexture.Name))
                    {
                        imageTexture.Name = Path.GetFileNameWithoutExtension(targetPath.Value);
                    }
                    var targetTextureImporter = AssetImporter.GetAtPath(targetPath.Value) as TextureImporter;
                    targetTextureImporter.sRGBTexture = (imageTexture.ColorSpace == VrmLib.Texture.ColorSpaceTypes.Srgb);
                    if (imageTexture.TextureType == VrmLib.Texture.TextureTypes.NormalMap)
                    {
                        targetTextureImporter.textureType = TextureImporterType.NormalMap;
                    }
                    targetTextureImporter.SaveAndReimport();

                    var externalObject = AssetDatabase.LoadAssetAtPath(targetPath.Value, typeof(UnityEngine.Texture2D));
                    AddRemap(new AssetImporter.SourceAssetIdentifier(typeof(UnityEngine.Texture2D), imageTexture.Name), externalObject);
                }

                //AssetDatabase.WriteImportSettingsIfDirty(assetPath);
                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);

                if (onComplited != null)
                {
                    onComplited();
                }
            };
        }


        public Dictionary<string, T> GetExternalUnityObjects<T>() where T : UnityEngine.Object
        {
            return this.GetExternalObjectMap().Where(x => x.Key.type == typeof(T)).ToDictionary(x => x.Key.name, x => (T)x.Value);
        }

        public void SetExternalUnityObject<T>(UnityEditor.AssetImporter.SourceAssetIdentifier sourceAssetIdentifier, T obj) where T : UnityEngine.Object
        {
            this.AddRemap(sourceAssetIdentifier, obj);
            AssetDatabase.WriteImportSettingsIfDirty(this.assetPath);
            AssetDatabase.ImportAsset(this.assetPath, ImportAssetOptions.ForceUpdate);
        }
    }
}


