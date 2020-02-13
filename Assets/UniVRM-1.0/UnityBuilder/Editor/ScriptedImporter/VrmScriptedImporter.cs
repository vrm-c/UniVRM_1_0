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
    public class VrmScriptedImporter : ScriptedImporter, IExternalUnityObject
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
                VrmLib.Model model = VrmLoader.CreateVrmModel(ctx.assetPath);
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



        public void ExtractTextures()
        {
            this.ExtractTextures(TextureDirName, (path) => { return VrmLoader.CreateVrmModel(path); });
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        public void ExtractMaterials()
        {
            this.ExtractAssets<UnityEngine.Material>(MaterialDirName, ".mat");
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        public void ExtractMaterialsAndTextures()
        {
            this.ExtractTextures(TextureDirName, (path) => { return VrmLoader.CreateVrmModel(path); }, () => { this.ExtractAssets<UnityEngine.Material>(MaterialDirName, ".mat"); });
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        public void ExtractMeta()
        {
            this.ExtractAssets<UniVRM10.VRMMetaObject>(MetaDirName, ".asset");
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
            this.ExtractAssets<UniVRM10.BlendShapeAvatar>(BlendShapeDirName, ".asset");
            this.ExtractAssets<UniVRM10.BlendShapeClip>(BlendShapeDirName, ".asset");

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