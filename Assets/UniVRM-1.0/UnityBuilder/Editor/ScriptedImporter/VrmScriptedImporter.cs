using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor.Experimental.AssetImporters;
using UnityEditor;
using VrmLib;

namespace UniVRM10
{
    [ScriptedImporter(1, "vrm")]
    public class VrmScriptedImporter : ScriptedImporter
    {
        const string TextureDirName = "Textures";
        const string MaterialDirName = "Materials";
        const string MetaDirName = "MetaObjects";
        const string BlendShapeDirName = "BlendShapes";

        public override void OnImportAsset(AssetImportContext ctx)
        {
            Debug.Log("OnImportAsset to " + ctx.assetPath);

            try
            {
                // Create Vrm Model
                VrmLib.Model model = CreateVrmModel(ctx.assetPath);
                Debug.Log($"ModelLoader.Load: {model}");

                // Build Unity Model
                var builder = new UniVRM10.EditorUnityBuilder();
                var assets = builder.ToUnityAsset(model, assetPath, this);
                ComponentBuilder.Build10(model, builder, assets);

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

                //// ScriptableObject
                // avatar
                ctx.AddObjectToAsset("avatar", assets.HumanoidAvatar);

                // meta
                {
                    var external = this.GetExternalUnityObjects<UniVRM10.VRMMetaObject>().FirstOrDefault();
                    if (external.Value != null)
                    {
                        var metaComponent = assets.Root.GetComponent<VRMMeta>();
                        if (metaComponent != null)
                        {
                            metaComponent.Meta = external.Value;
                        }
                    }
                    else
                    {
                        var meta = assets.ScriptableObjects
                            .FirstOrDefault(x => x.GetType() == typeof(UniVRM10.VRMMetaObject)) as UniVRM10.VRMMetaObject;
                        if (meta != null)
                        {
                            meta.name = "meta";
                            ctx.AddObjectToAsset(meta.name, meta);
                        }
                    }
                }

                // blendShape
                {
                    var external = this.GetExternalUnityObjects<UniVRM10.BlendShapeClip>();
                    if (external.Any())
                    {
                    }
                    else
                    {
                        var blendShapeClips = assets.ScriptableObjects
                            .Where(x => x.GetType() == typeof(UniVRM10.BlendShapeClip))
                            .Select(x => x as UniVRM10.BlendShapeClip);
                        foreach (var clip in blendShapeClips)
                        {
                            clip.name = clip.BlendShapeName;
                            ctx.AddObjectToAsset(clip.BlendShapeName, clip);
                        }
                    }
                }
                {
                    var external = this.GetExternalUnityObjects<UniVRM10.BlendShapeAvatar>().FirstOrDefault();
                    if (external.Value != null)
                    {
                        var blendShapeComponent = assets.Root.GetComponent<VRMBlendShapeProxy>();
                        if (blendShapeComponent != null)
                        {
                            blendShapeComponent.BlendShapeAvatar = external.Value;
                        }
                    }
                    else
                    {
                        var blendShapeAvatar = assets.ScriptableObjects
                            .FirstOrDefault(x => x.GetType() == typeof(UniVRM10.BlendShapeAvatar)) as UniVRM10.BlendShapeAvatar;
                        if (blendShapeAvatar != null)
                        {
                            blendShapeAvatar.name = "blendShapeAvatar";
                            ctx.AddObjectToAsset(blendShapeAvatar.name, blendShapeAvatar);
                        }
                    }
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

        private VrmLib.Model CreateVrmModel(string path)
        {
            var bytes = File.ReadAllBytes(path);
            if (!VrmLib.Glb.TryParse(bytes, out VrmLib.Glb glb, out Exception ex))
            {
                throw ex;
            }

            // version check
            VrmLib.Model model = null;
            VrmLib.IVrmStorage storage;
            if (VRMVersionCheck.IsVrm10(glb.Json.Bytes.ToArray()))
            {
                storage = new Vrm10.Vrm10Storage(glb.Json.Bytes, glb.Binary.Bytes);
                model = VrmLib.ModelLoader.Load(storage, Path.GetFileName(path));
                model.ConvertCoordinate(VrmLib.Coordinates.Unity, ignoreVrm: true);
            }
            else
            {
                throw new NotImplementedException();
            }

            return model;
        }

        public static void ExtractFromAsset(UnityEngine.Object subAsset, string destinationPath, bool isForceUpdate)
        {
            string assetPath = AssetDatabase.GetAssetPath(subAsset);

            var clone = UnityEngine.Object.Instantiate(subAsset);
            AssetDatabase.CreateAsset(clone, destinationPath);

            var assetImporter = AssetImporter.GetAtPath(assetPath);
            assetImporter.AddRemap(new AssetImporter.SourceAssetIdentifier(subAsset), clone);

            if (isForceUpdate)
            {
                AssetDatabase.WriteImportSettingsIfDirty(assetPath);
                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            }
        }

        public void ExtractTextures()
        {
            ExtractTextures(TextureDirName);
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        public void ExtractMaterials()
        {
            ExtractAssets<UnityEngine.Material>(MaterialDirName, ".mat");
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        public void ExtractMaterialsAndTextures()
        {
            ExtractTextures(TextureDirName, () => { ExtractAssets<UnityEngine.Material>(MaterialDirName, ".mat"); });
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        public void ExtractMeta()
        {
            ExtractAssets<UniVRM10.VRMMetaObject>(MetaDirName, ".asset");
            var metaObject = this.GetExternalUnityObjects<UniVRM10.VRMMetaObject>().FirstOrDefault();
            var metaObjectPath = AssetDatabase.GetAssetPath(metaObject.Value);
            if (!string.IsNullOrEmpty(metaObjectPath))
            {
                EditorUtility.SetDirty(metaObject.Value);
                AssetDatabase.WriteImportSettingsIfDirty(metaObjectPath);
            }
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        public void ExtractBlendShapes()
        {
            ExtractAssets<UniVRM10.BlendShapeAvatar>(BlendShapeDirName, ".asset");
            ExtractAssets<UniVRM10.BlendShapeClip>(BlendShapeDirName, ".asset");

            var blendShapeAvatar = this.GetExternalUnityObjects<UniVRM10.BlendShapeAvatar>().FirstOrDefault();
            var blendShapeClips = this.GetExternalUnityObjects<UniVRM10.BlendShapeClip>();

            blendShapeAvatar.Value.Clips = blendShapeClips.Select(x => x.Value).ToList();
            var avatarPath = AssetDatabase.GetAssetPath(blendShapeAvatar.Value);
            if (!string.IsNullOrEmpty(avatarPath))
            {
                EditorUtility.SetDirty(blendShapeAvatar.Value);
                AssetDatabase.WriteImportSettingsIfDirty(avatarPath);
            }

            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        public void ClearExtarnalObjects<T>() where T : UnityEngine.Object
        {
            foreach (var extarnalObject in this.GetExternalObjectMap().Where(x => x.Key.type == typeof(T)))
            {
                RemoveRemap(extarnalObject.Key);
            }

            AssetDatabase.WriteImportSettingsIfDirty(assetPath);
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        public void ClearExtarnalObjects()
        {
            foreach (var extarnalObject in GetExternalObjectMap())
            {
                RemoveRemap(extarnalObject.Key);
            }

            AssetDatabase.WriteImportSettingsIfDirty(assetPath);
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        private T GetSubAsset<T>() where T : UnityEngine.Object
        {
            return GetSubAssets<T>()
                .FirstOrDefault();
        }

        private IEnumerable<T> GetSubAssets<T>() where T : UnityEngine.Object
        {
            return AssetDatabase
                .LoadAllAssetsAtPath(assetPath)
                .Where(x => AssetDatabase.IsSubAsset(x))
                .Where(x => x is T)
                .Select(x => x as T);
        }

        private void ExtractAssets<T>(string dirName, string extension) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(assetPath))
                return;

            var subAssets = GetSubAssets<T>();

            var path = string.Format("{0}/{1}.{2}",
                Path.GetDirectoryName(assetPath),
                Path.GetFileNameWithoutExtension(assetPath),
                dirName
                );

            var info = SafeCreateDirectory(path);

            foreach (var asset in subAssets)
            {
                ExtractFromAsset(asset, string.Format("{0}/{1}{2}", path, asset.name, extension), false);
            }
        }

        private void ExtractTextures(string dirName, Action onComplited = null)
        {
            if (string.IsNullOrEmpty(assetPath))
                return;

            var subAssets = GetSubAssets<UnityEngine.Texture2D>();

            var path = string.Format("{0}/{1}.{2}",
                Path.GetDirectoryName(assetPath),
                Path.GetFileNameWithoutExtension(assetPath),
                dirName
                );

            SafeCreateDirectory(path);

            Dictionary<VrmLib.ImageTexture, string> targetPaths = new Dictionary<VrmLib.ImageTexture, string>();

            // Reload Model
            var model = CreateVrmModel(assetPath);
            var mimeTypeReg = new System.Text.RegularExpressions.Regex("image/(?<mime>.*)$");
            foreach (var texture in model.Textures)
            {
                var imageTexture = texture as VrmLib.ImageTexture;
                if (imageTexture == null) continue;

                var mimeType = mimeTypeReg.Match(imageTexture.Image.MimeType);
                var targetPath = string.Format("{0}/{1}.{2}", path, imageTexture.Name, mimeType.Groups["mime"].Value);
                File.WriteAllBytes(targetPath, imageTexture.Image.Bytes.ToArray());
                AssetDatabase.ImportAsset(targetPath);
                targetPaths.Add(imageTexture, targetPath);
            }

            EditorApplication.delayCall += () =>
            {
                foreach (var targetPath in targetPaths)
                {
                    var imageTexture = targetPath.Key;
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

        private static DirectoryInfo SafeCreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return null;
            }
            return Directory.CreateDirectory(path);
        }
    }
}