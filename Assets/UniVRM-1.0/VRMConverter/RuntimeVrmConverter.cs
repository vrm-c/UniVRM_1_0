using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UniVRM10
{
    public delegate VrmLib.TextureInfo GetOrCreateTextureDelegate(Material material, Texture srcTexture, VrmLib.Texture.ColorSpaceTypes colorSpace, VrmLib.Texture.TextureTypes textureType);
    public class RuntimeVrmConverter
    {
        public VrmLib.Model Model;

        public Dictionary<GameObject, VrmLib.Node> Nodes = new Dictionary<GameObject, VrmLib.Node>();
        public Dictionary<Texture2D, VrmLib.TextureInfo> Textures = new Dictionary<Texture2D, VrmLib.TextureInfo>();
        public Dictionary<Material, VrmLib.Material> Materials = new Dictionary<Material, VrmLib.Material>();
        public Dictionary<Mesh, VrmLib.MeshGroup> Meshes = new Dictionary<Mesh, VrmLib.MeshGroup>();

        static string GetSupportedMime(string path)
        {
            var ext = Path.GetExtension(path).ToLower();
            switch (ext)
            {
                case ".png": return "image/png";
                case ".jpg": return "image/jpeg";
            }

            // .tga etc
            return null;
        }

        /// <summary>
        /// return (bytes, mime string)
        /// </summary>
        static (byte[], string) GetImageEncodedBytes(Texture src, RenderTextureReadWrite renderTextureReadWrite, Material renderMaterial = null)
        {
#if false
            /// 元になるアセットがあればそれを得る(png, jpgのみ)
            var assetPath = UnityEditor.AssetDatabase.GetAssetPath(src);
            if (!string.IsNullOrEmpty(assetPath))
            {
                var mime = GetSupportedMime(assetPath);
                if (!string.IsNullOrEmpty(mime))
                {
                    return (File.ReadAllBytes(assetPath), GetSupportedMime(assetPath));
                }
            }
#endif

            var copy = UnityTextureUtil.CopyTexture(src, renderTextureReadWrite, renderMaterial);
            return (copy.EncodeToPNG(), "image/png");
        }

        public VrmLib.TextureInfo GetOrCreateTexture(Material material, Texture srcTexture, VrmLib.Texture.ColorSpaceTypes colorSpace, VrmLib.Texture.TextureTypes textureType)
        {
            var texture = srcTexture as Texture2D;
            if (texture is null)
            {
                return null;
            }

            if (!Textures.TryGetValue(texture, out VrmLib.TextureInfo info))
            {
                Material converter = null;
                if (textureType == VrmLib.Texture.TextureTypes.NormalMap)
                {
                    converter = TextureConvertMaterial.GetNormalMapConvertUnityToGltf();
                }
                else if (textureType == VrmLib.Texture.TextureTypes.MetallicRoughness)
                {
                    float smoothness = 0.0f;
                    if (material.HasProperty("_GlossMapScale"))
                    {
                        smoothness = material.GetFloat("_GlossMapScale");
                    }

                    converter = TextureConvertMaterial.GetMetallicRoughnessUnityToGltf(smoothness);
                }
                else if (textureType == VrmLib.Texture.TextureTypes.Occlusion)
                {
                    converter = TextureConvertMaterial.GetOcclusionUnityToGltf();
                }

                var (bytes, mime) = GetImageEncodedBytes(
                    texture,
                    (colorSpace == VrmLib.Texture.ColorSpaceTypes.Linear) ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.sRGB,
                    converter
                    );

                if (converter != null)
                {
                    UnityEngine.Object.DestroyImmediate(converter);
                }

                var sampler = new VrmLib.TextureSampler
                {
                    MagFilter = texture.filterMode.ToVrmLibMagFilter(),
                    MinFilter = texture.filterMode.ToVrmLibMinFilter(),
                    WrapS = texture.wrapMode.ToVrmLib(),
                    WrapT = texture.wrapMode.ToVrmLib(),
                };
                var image = new VrmLib.Image(texture.name, mime, VrmLib.ImageUsage.None, bytes);
                info = new VrmLib.TextureInfo(new VrmLib.ImageTexture(texture.name, sampler, image, colorSpace, textureType));
                Textures.Add(texture, info);

                if (Model != null)
                {
                    Model.Images.Add(image);
                    Model.Textures.Add(info.Texture);
                }
            }

            return info;
        }

        #region Export 1.0
        /// <summary>
        /// metaObject が null のときは、root から取得する
        /// </summary>
        public VrmLib.Model ToModelFrom10(GameObject root, VRMMetaObject metaObject = null)
        {
            Model = new VrmLib.Model(VrmLib.Coordinates.Unity);

            if (metaObject is null)
            {
                var vrmMeta = root.GetComponent<VRMMeta>();
                if (vrmMeta is null || vrmMeta.Meta is null)
                {
                    throw new NullReferenceException("metaObject is null");
                }
                metaObject = vrmMeta.Meta;
            }

            ToGlbModel(root);

            // meta
            var meta = new VrmLib.Meta();
            meta.Name = metaObject.Name;
            meta.Version = metaObject.Version;
            meta.Copyrights = metaObject.Copyrights;
            meta.Authors.AddRange(metaObject.Authors);
            meta.ContactInformation = metaObject.ContactInformation;
            meta.Reference = metaObject.Reference;
            meta.Thumbnail = metaObject.Thumbnail.ToPngImage(VrmLib.ImageUsage.None);

            meta.AvatarPermission = new VrmLib.AvatarPermission
            {
                AvatarUsage = metaObject.AllowedUser,
                IsAllowedViolentUsage = metaObject.ViolentUsage,
                IsAllowedSexualUsage = metaObject.SexualUsage,
                CommercialUsage = metaObject.CommercialUsage,
                IsAllowedGameUsage = metaObject.GameUsage,
                IsAllowedPoliticalOrReligiousUsage = metaObject.PoliticalOrReligiousUsage,
                OtherPermissionUrl = metaObject.OtherPermissionUrl,
            };
            meta.RedistributionLicense = new VrmLib.RedistributionLicense
            {
                CreditNotation = metaObject.CreditNotation,
                IsAllowRedistribution = metaObject.Redistribution,
                ModificationLicense = metaObject.ModificationLicense,
                OtherLicenseUrl = metaObject.OtherLicenseUrl,
            };
            Model.Vrm = new VrmLib.Vrm(meta, UniVRM10.VRMVersion.VERSION, UniVRM10.VRMSpecVersion.Version);

            // humanoid
            {
                var animator = root.GetComponent<Animator>();
                if (animator != null && animator.avatar != null)
                {
                    foreach (HumanBodyBones humanBoneType in Enum.GetValues(typeof(HumanBodyBones)))
                    {
                        if (humanBoneType == HumanBodyBones.LastBone)
                            continue;

                        var transform = animator.GetBoneTransform(humanBoneType);
                        if (transform != null && Nodes.TryGetValue(transform.gameObject, out VrmLib.Node node))
                        {
                            node.HumanoidBone = (VrmLib.HumanoidBones)Enum.Parse(typeof(VrmLib.HumanoidBones), humanBoneType.ToString(), true);
                        }
                    }
                }
            }


            // blendShape
            {
                Model.Vrm.BlendShape = new VrmLib.BlendShapeManager();
                var blendShapeProxy = root.GetComponent<UniVRM10.VRMBlendShapeProxy>();
                if (blendShapeProxy != null)
                {
                    foreach (var clip in blendShapeProxy.BlendShapeAvatar.Clips)
                    {
                        var blendShape = new VrmLib.BlendShape(clip.Preset, clip.BlendShapeName, clip.IsBinary);
                        blendShape.IgnoreBlink = clip.IgnoreBlink;
                        blendShape.IgnoreLookAt = clip.IgnoreLookAt;
                        blendShape.IgnoreMouth = clip.IgnoreMouth;

                        foreach (var value in clip.Values)
                        {
                            var transform = GetTransformFromRelativePath(root.transform, value.RelativePath);
                            if (transform == null)
                                continue;
                            var renderer = transform.gameObject.GetComponent<SkinnedMeshRenderer>();
                            if (renderer == null)
                                continue;
                            var mesh = renderer.sharedMesh;
                            if (mesh == null)
                                continue;

                            var names = new List<string>();
                            for (int i = 0; i < mesh.blendShapeCount; ++i)
                            {
                                names.Add(mesh.GetBlendShapeName(i));
                            }

                            var node = Nodes[transform.gameObject];
                            var blendShapeValue = new VrmLib.BlendShapeBindValue(
                                node,
                                names[value.Index],
                                value.Weight
                                );
                            blendShape.BlendShapeValues.Add(blendShapeValue);
                        }

                        foreach (var value in clip.MaterialValues)
                        {
                            var materialPair = Materials.FirstOrDefault(x => x.Key.name == value.MaterialName);
                            if (materialPair.Equals(default(KeyValuePair<Material, VrmLib.Material>)))
                                continue;

                            var bind = new VrmLib.MaterialBindValue(
                                materialPair.Value,
                                value.ValueName,
                                value.TargetValue.ToNumericsVector4()
                                );
                            blendShape.MaterialValues.Add(bind);
                        }

                        Model.Vrm.BlendShape.BlendShapeList.Add(blendShape);
                    }
                }
            }

            // firstPerson
            {
                var firstPerson = new VrmLib.FirstPerson();
                var firstPersonComponent = root.GetComponent<UniVRM10.VRMFirstPerson>();
                if (firstPersonComponent != null)
                {
                    foreach (var annotation in firstPersonComponent.Renderers)
                    {
                        firstPerson.Annotations.Add(
                            new VrmLib.FirstPersonMeshAnnotation(Nodes[annotation.Renderer.gameObject],
                            annotation.FirstPersonFlag)
                            );
                    }
                    Model.Vrm.FirstPerson = firstPerson;
                }
            }

            // lookAt
            {
                var blendShapeProxy = root.GetComponent<UniVRM10.VRMBlendShapeProxy>();
                var lookAt = new VrmLib.LookAt();
                var lookAtBlendShape = blendShapeProxy;
                var lookAtBone = blendShapeProxy;
                if (lookAtBlendShape != null)
                {
                    if (lookAtBlendShape.LookAtType == VRMBlendShapeProxy.LookAtTypes.BlendShape)
                    {
                        lookAt.HorizontalInner = new VrmLib.LookAtRangeMap();
                        lookAt.HorizontalOuter = new VrmLib.LookAtRangeMap()
                        {
                            InputMaxValue = lookAtBlendShape.HorizontalOuter.CurveXRangeDegree,
                            OutputScaling = lookAtBlendShape.HorizontalOuter.CurveYRangeDegree
                        };
                        lookAt.VerticalUp = new VrmLib.LookAtRangeMap()
                        {
                            InputMaxValue = lookAtBlendShape.VerticalUp.CurveXRangeDegree,
                            OutputScaling = lookAtBlendShape.VerticalUp.CurveYRangeDegree,
                        };
                        lookAt.VerticalDown = new VrmLib.LookAtRangeMap()
                        {
                            InputMaxValue = lookAtBlendShape.VerticalDown.CurveXRangeDegree,
                            OutputScaling = lookAtBlendShape.VerticalDown.CurveYRangeDegree,
                        };
                    }
                    else if (lookAtBlendShape.LookAtType == VRMBlendShapeProxy.LookAtTypes.Bone)
                    {
                        lookAt.HorizontalInner = new VrmLib.LookAtRangeMap()
                        {
                            InputMaxValue = lookAtBone.HorizontalInner.CurveXRangeDegree,
                            OutputScaling = lookAtBone.HorizontalInner.CurveYRangeDegree
                        };
                        lookAt.HorizontalOuter = new VrmLib.LookAtRangeMap()
                        {
                            InputMaxValue = lookAtBone.HorizontalOuter.CurveXRangeDegree,
                            OutputScaling = lookAtBone.HorizontalOuter.CurveYRangeDegree
                        };
                        lookAt.VerticalUp = new VrmLib.LookAtRangeMap()
                        {
                            InputMaxValue = lookAtBone.VerticalUp.CurveXRangeDegree,
                            OutputScaling = lookAtBone.VerticalUp.CurveYRangeDegree,
                        };
                        lookAt.VerticalDown = new VrmLib.LookAtRangeMap()
                        {
                            InputMaxValue = lookAtBone.VerticalDown.CurveXRangeDegree,
                            OutputScaling = lookAtBone.VerticalDown.CurveYRangeDegree,
                        };
                    }
                    lookAt.OffsetFromHeadBone = blendShapeProxy.OffsetFromHead.ToNumericsVector3();
                }
                Model.Vrm.LookAt = lookAt;
            }

            // springBone
            {
                var springBoneColliderGroups = root.GetComponentsInChildren<UniVRM10.VRMSpringBoneColliderGroup>();
                if (springBoneColliderGroups != null)
                {
                    Model.Vrm.SpringBone = new VrmLib.SpringBoneManager();
                    var colliders = new Dictionary<UniVRM10.VRMSpringBoneColliderGroup, VrmLib.SpringBoneColliderGroup>();
                    foreach (var colliderGroup in springBoneColliderGroups)
                    {
                        var colliderGroups = colliderGroup.Colliders.Select(x =>
                            new VrmLib.VrmSpringBoneColliderSphere(x.Offset.ToNumericsVector3(), x.Radius));
                        var vrmColliderGroup = new VrmLib.SpringBoneColliderGroup(Nodes[colliderGroup.gameObject], colliderGroups);
                        Model.Vrm.SpringBone.Colliders.Add(vrmColliderGroup);

                        colliders.Add(colliderGroup, vrmColliderGroup);
                    }

                    var springBones = root.GetComponentsInChildren<UniVRM10.VRMSpringBone>();
                    foreach (var springBone in springBones)
                    {
                        var vrmSpringBone = new VrmLib.SpringBone()
                        {
                            Comment = springBone.m_comment,
                            Stiffness = springBone.m_stiffnessForce,
                            GravityPower = springBone.m_gravityPower,
                            GravityDir = springBone.m_gravityDir.ToNumericsVector3(),
                            DragForce = springBone.m_dragForce,
                            Origin = (springBone.m_center != null) ? Nodes[springBone.m_center.gameObject] : null,
                            HitRadius = springBone.m_hitRadius,
                        };

                        foreach (var rootBone in springBone.RootBones)
                        {
                            vrmSpringBone.Bones.Add(Nodes[rootBone.gameObject]);
                        }

                        foreach (var collider in springBone.ColliderGroups)
                        {
                            vrmSpringBone.Colliders.Add(colliders[collider]);
                        }

                        Model.Vrm.SpringBone.Springs.Add(vrmSpringBone);
                    }
                }
            }

            return Model;
        }

        public VrmLib.Model ToGlbModel(GameObject root)
        {
            if (Model == null)
            {
                Model = new VrmLib.Model(VrmLib.Coordinates.Unity);
            }

            // node
            {
                Model.Root.Name = root.name;
                CreateNodes(root.transform, Model.Root, Nodes);
                Model.Nodes = Nodes
                .Where(x => x.Value != Model.Root)
                            .Select(x => x.Value).ToList();
            }

            // material and textures
            var rendererComponents = root.GetComponentsInChildren<Renderer>();
            {
                foreach (var renderer in rendererComponents)
                {
                    var materials = renderer.sharedMaterials; // avoid copy
                    foreach (var material in materials)
                    {
                        if (Materials.ContainsKey(material))
                        {
                            continue;
                        }

                        var vrmMaterial = Export10(material, GetOrCreateTexture);
                        Model.Materials.Add(vrmMaterial);
                        Materials.Add(material, vrmMaterial);
                    }
                }
            }

            // mesh
            {
                foreach (var renderer in rendererComponents)
                {
                    if (renderer is SkinnedMeshRenderer skinnedMeshRenderer)
                    {
                        if (skinnedMeshRenderer.sharedMesh != null)
                        {
                            var mesh = CreateMesh(skinnedMeshRenderer.sharedMesh, skinnedMeshRenderer, Materials);
                            var skin = CreateSkin(skinnedMeshRenderer, Nodes, root);
                            if (skin != null)
                            {
                                // blendshape only で skinning が無いやつがある
                                mesh.Skin = skin;
                                Model.Skins.Add(mesh.Skin);
                            }
                            Model.MeshGroups.Add(mesh);
                            Nodes[renderer.gameObject].MeshGroup = mesh;
                            Meshes.Add(skinnedMeshRenderer.sharedMesh, mesh);
                        }
                    }
                    else if (renderer is MeshRenderer meshRenderer)
                    {
                        var filter = meshRenderer.gameObject.GetComponent<MeshFilter>();
                        if (filter != null && filter.sharedMesh != null)
                        {
                            var mesh = CreateMesh(filter.sharedMesh, meshRenderer, Materials);
                            Model.MeshGroups.Add(mesh);
                            Nodes[renderer.gameObject].MeshGroup = mesh;
                            Meshes.Add(filter.sharedMesh, mesh);
                        }
                    }
                }
            }

            return Model;
        }
        #endregion

        public VrmLib.Material Export10(Material src, GetOrCreateTextureDelegate map)
        {
            switch (src.shader.name)
            {
                case "VRM/MToon":
                    {
                        var def = MToon.Utils.GetMToonParametersFromMaterial(src);
                        return new VrmLib.MToonMaterial(src.name)
                        {
                            Definition = def.ToVrmLib(src, map),
                        };
                    }

                case "Unlit/Color":
                    return new VrmLib.UnlitMaterial(src.name)
                    {
                        BaseColorFactor = src.color.FromUnitySrgbToLinear(),
                    };

                case "Unlit/Texture":
                    return new VrmLib.UnlitMaterial(src.name)
                    {
                        BaseColorTexture = map(src, src.mainTexture as Texture2D, VrmLib.Texture.ColorSpaceTypes.Srgb, VrmLib.Texture.TextureTypes.Default),
                    };

                case "Unlit/Transparent":
                    return new VrmLib.UnlitMaterial(src.name)
                    {
                        BaseColorTexture = map(src, src.mainTexture as Texture2D, VrmLib.Texture.ColorSpaceTypes.Srgb, VrmLib.Texture.TextureTypes.Default),
                        AlphaMode = VrmLib.AlphaModeType.BLEND,
                    };

                case "Unlit/Transparent Cutout":
                    return new VrmLib.UnlitMaterial(src.name)
                    {
                        BaseColorTexture = map(src, src.mainTexture as Texture2D, VrmLib.Texture.ColorSpaceTypes.Srgb, VrmLib.Texture.TextureTypes.Default),
                        AlphaMode = VrmLib.AlphaModeType.MASK,
                        AlphaCutoff = src.GetFloat("_Cutoff"),
                    };

                case "UniGLTF/UniUnlit":
                case "VRM/UniUnlit":
                    {
                        var material = new VrmLib.UnlitMaterial(src.name)
                        {
                            BaseColorFactor = src.color.FromUnitySrgbToLinear(),
                            BaseColorTexture = map(src, src.mainTexture as Texture2D, VrmLib.Texture.ColorSpaceTypes.Srgb, VrmLib.Texture.TextureTypes.Default),
                            AlphaMode = GetAlphaMode(src),
                            DoubleSided = UniGLTF.UniUnlit.Utils.GetCullMode(src) == UniGLTF.UniUnlit.UniUnlitCullMode.Off,
                        };
                        if (material.AlphaMode == VrmLib.AlphaModeType.MASK)
                        {
                            material.AlphaCutoff = src.GetFloat("_Cutoff");
                        }
                        // TODO: VertexColorMode
                        return material;
                    }

                default:
                    return ExportStandard(src, map);
            }
        }

        static VrmLib.AlphaModeType GetAlphaMode(Material m)
        {
            switch (UniGLTF.UniUnlit.Utils.GetRenderMode(m))
            {
                case UniGLTF.UniUnlit.UniUnlitRenderMode.Opaque: return VrmLib.AlphaModeType.OPAQUE;
                case UniGLTF.UniUnlit.UniUnlitRenderMode.Cutout: return VrmLib.AlphaModeType.MASK;
                case UniGLTF.UniUnlit.UniUnlitRenderMode.Transparent: return VrmLib.AlphaModeType.BLEND;
            }
            throw new NotImplementedException();
        }

        static VrmLib.PBRMaterial ExportStandard(Material src, GetOrCreateTextureDelegate map)
        {
            var material = new VrmLib.PBRMaterial(src.name)
            {
            };

            switch (src.GetTag("RenderType", true))
            {
                case "Transparent":
                    material.AlphaMode = VrmLib.AlphaModeType.BLEND;
                    break;

                case "TransparentCutout":
                    material.AlphaMode = VrmLib.AlphaModeType.MASK;
                    material.AlphaCutoff = src.GetFloat("_Cutoff");
                    break;

                default:
                    material.AlphaMode = VrmLib.AlphaModeType.OPAQUE;
                    break;
            }

            if (src.HasProperty("_Color"))
            {
                material.BaseColorFactor = src.color.linear.FromUnitySrgbToLinear();
            }

            if (src.HasProperty("_MainTex"))
            {
                material.BaseColorTexture = map(src, src.GetTexture("_MainTex"), VrmLib.Texture.ColorSpaceTypes.Srgb, VrmLib.Texture.TextureTypes.Default);
            }

            if (src.HasProperty("_MetallicGlossMap"))
            {
                // float smoothness = 0.0f;
                // if (m.HasProperty("_GlossMapScale"))
                // {
                //     smoothness = m.GetFloat("_GlossMapScale");
                // }

                material.MetallicRoughnessTexture = map(
                    src,
                    src.GetTexture("_MetallicGlossMap"),
                    VrmLib.Texture.ColorSpaceTypes.Linear,
                    VrmLib.Texture.TextureTypes.MetallicRoughness)?.Texture;
                if (material.MetallicRoughnessTexture != null)
                {
                    material.MetallicFactor = 1.0f;
                    // Set 1.0f as hard-coded. See: https://github.com/vrm-c/UniVRM/issues/212.
                    material.RoughnessFactor = 1.0f;
                }
            }

            if (material.MetallicRoughnessTexture == null)
            {
                if (src.HasProperty("_Metallic"))
                {
                    material.MetallicFactor = src.GetFloat("_Metallic");
                }

                if (src.HasProperty("_Glossiness"))
                {
                    material.RoughnessFactor = 1.0f - src.GetFloat("_Glossiness");
                }
            }

            if (src.HasProperty("_BumpMap"))
            {
                material.NormalTexture = map(src, src.GetTexture("_BumpMap"), VrmLib.Texture.ColorSpaceTypes.Linear, VrmLib.Texture.TextureTypes.NormalMap)?.Texture;

                if (src.HasProperty("_BumpScale"))
                {
                    material.NormalTextureScale = src.GetFloat("_BumpScale");
                }
            }

            if (src.HasProperty("_OcclusionMap"))
            {
                material.OcclusionTexture = map(src, src.GetTexture("_OcclusionMap"), VrmLib.Texture.ColorSpaceTypes.Linear, VrmLib.Texture.TextureTypes.Occlusion)?.Texture;

                if (src.HasProperty("_OcclusionStrength"))
                {
                    material.OcclusionTextureStrength = src.GetFloat("_OcclusionStrength");
                }
            }

            if (src.IsKeywordEnabled("_EMISSION"))
            {
                if (src.HasProperty("_EmissionColor"))
                {
                    var color = src.GetColor("_EmissionColor");
                    if (color.maxColorComponent > 1)
                    {
                        color /= color.maxColorComponent;
                    }
                    material.EmissiveFactor = new System.Numerics.Vector3(color.r, color.g, color.b);
                }

                if (src.HasProperty("_EmissionMap"))
                {
                    material.EmissiveTexture = map(src, src.GetTexture("_EmissionMap"), VrmLib.Texture.ColorSpaceTypes.Srgb, VrmLib.Texture.TextureTypes.Emissive)?.Texture;
                }
            }

            return material;
        }

        private static void CreateNodes(
            Transform parentTransform,
            VrmLib.Node parentNode,
            Dictionary<GameObject, VrmLib.Node> nodes)
        {
            // parentNode.SetMatrix(parentTransform.localToWorldMatrix.ToNumericsMatrix4x4(), false);
            parentNode.LocalTranslation = parentTransform.localPosition.ToNumericsVector3();
            parentNode.LocalRotation = parentTransform.localRotation.ToNumericsQuaternion();
            parentNode.LocalScaling = parentTransform.localScale.ToNumericsVector3();
            nodes.Add(parentTransform.gameObject, parentNode);

            foreach (Transform child in parentTransform)
            {
                var childNode = new VrmLib.Node(child.gameObject.name);
                CreateNodes(child, childNode, nodes);
                parentNode.Add(childNode);
            }
        }

        private static Transform GetTransformFromRelativePath(Transform root, string relativePath)
        {
            var paths = new Queue<string>(relativePath.Split('/'));
            return GetTransformFromRelativePath(root, paths);
        }

        private static Transform GetTransformFromRelativePath(Transform root, Queue<string> relativePath)
        {
            var name = relativePath.Dequeue();
            foreach (Transform node in root)
            {
                if (node.gameObject.name == name)
                {
                    if (relativePath.Count == 0)
                    {
                        return node;
                    }
                    else
                    {
                        return GetTransformFromRelativePath(node, relativePath);
                    }
                }
            }

            return null;
        }

        private static VrmLib.MeshGroup CreateMesh(Mesh mesh, Renderer renderer, Dictionary<Material, VrmLib.Material> materials)
        {
            var meshGroup = new VrmLib.MeshGroup(mesh.name);
            var vrmMesh = new VrmLib.Mesh();
            vrmMesh.VertexBuffer = new VrmLib.VertexBuffer();
            vrmMesh.VertexBuffer.Add(VrmLib.VertexBuffer.PositionKey, ToBufferAccessor(mesh.vertices));

            if (mesh.boneWeights.Length == mesh.vertexCount)
            {
                vrmMesh.VertexBuffer.Add(
                    VrmLib.VertexBuffer.WeightKey,
                    ToBufferAccessor(mesh.boneWeights.Select(x =>
                    new Vector4(x.weight0, x.weight1, x.weight2, x.weight3)).ToArray()
                    ));
                vrmMesh.VertexBuffer.Add(
                    VrmLib.VertexBuffer.JointKey,
                    ToBufferAccessor(mesh.boneWeights.Select(x =>
                    new VrmLib.SkinJoints((ushort)x.boneIndex0, (ushort)x.boneIndex1, (ushort)x.boneIndex2, (ushort)x.boneIndex3)).ToArray()
                    ));
            }
            if (mesh.uv.Length == mesh.vertexCount) vrmMesh.VertexBuffer.Add(VrmLib.VertexBuffer.TexCoordKey, ToBufferAccessor(mesh.uv));
            if (mesh.normals.Length == mesh.vertexCount) vrmMesh.VertexBuffer.Add(VrmLib.VertexBuffer.NormalKey, ToBufferAccessor(mesh.normals));
            if (mesh.colors.Length == mesh.vertexCount) vrmMesh.VertexBuffer.Add(VrmLib.VertexBuffer.ColorKey, ToBufferAccessor(mesh.colors));
            vrmMesh.IndexBuffer = ToBufferAccessor(mesh.triangles);

            int offset = 0;
            for (int i = 0; i < mesh.subMeshCount; i++)
            {
                var subMesh = mesh.GetSubMesh(i);
                try
                {
                    vrmMesh.Submeshes.Add(new VrmLib.Submesh(offset, subMesh.indexCount, materials[renderer.sharedMaterials[i]]));
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }

                offset += subMesh.indexCount;
            }

            for (int i = 0; i < mesh.blendShapeCount; i++)
            {
                var blendShapeVertices = mesh.vertices;
                var usePosition = blendShapeVertices != null && blendShapeVertices.Length > 0;

                var blendShapeNormals = mesh.normals;
                var useNormal = usePosition && blendShapeNormals != null && blendShapeNormals.Length == blendShapeVertices.Length;
                // var useNormal = usePosition && blendShapeNormals != null && blendShapeNormals.Length == blendShapeVertices.Length && !exportOnlyBlendShapePosition;

                var blendShapeTangents = mesh.tangents.Select(y => (Vector3)y).ToArray();
                //var useTangent = usePosition && blendShapeTangents != null && blendShapeTangents.Length == blendShapeVertices.Length;
                // var useTangent = false;

                var frameCount = mesh.GetBlendShapeFrameCount(i);
                mesh.GetBlendShapeFrameVertices(i, frameCount - 1, blendShapeVertices, blendShapeNormals, null);

                if (usePosition)
                {
                    var morphTarget = new VrmLib.MorphTarget(mesh.GetBlendShapeName(i));
                    morphTarget.VertexBuffer = new VrmLib.VertexBuffer();
                    morphTarget.VertexBuffer.Add(VrmLib.VertexBuffer.PositionKey, ToBufferAccessor(blendShapeVertices));
                    vrmMesh.MorphTargets.Add(morphTarget);
                }
            }

            meshGroup.Meshes.Add(vrmMesh);
            return meshGroup;
        }

        private static VrmLib.Skin CreateSkin(
            SkinnedMeshRenderer skinnedMeshRenderer,
            Dictionary<GameObject, VrmLib.Node> nodes,
            GameObject root)
        {
            if (skinnedMeshRenderer.bones == null || skinnedMeshRenderer.bones.Length == 0)
            {
                return null;
            }

            var skin = new VrmLib.Skin();
            skin.InverseMatrices = ToBufferAccessor(skinnedMeshRenderer.sharedMesh.bindposes);
            if (skinnedMeshRenderer.rootBone != null)
            {
                skin.Root = nodes[skinnedMeshRenderer.rootBone.gameObject];
            }

            skin.Joints = skinnedMeshRenderer.bones.Select(x => nodes[x.gameObject]).ToList();
            return skin;
        }

        private static VrmLib.BufferAccessor ToBufferAccessor(VrmLib.SkinJoints[] values)
        {
            return ToBufferAccessor(values, VrmLib.AccessorValueType.UNSIGNED_SHORT, VrmLib.AccessorVectorType.VEC4);
        }

        private static VrmLib.BufferAccessor ToBufferAccessor(Color[] colors)
        {
            return ToBufferAccessor(colors, VrmLib.AccessorValueType.FLOAT, VrmLib.AccessorVectorType.VEC4);
        }

        private static VrmLib.BufferAccessor ToBufferAccessor(Vector4[] vectors)
        {
            return ToBufferAccessor(vectors, VrmLib.AccessorValueType.FLOAT, VrmLib.AccessorVectorType.VEC4);
        }

        private static VrmLib.BufferAccessor ToBufferAccessor(Vector3[] vectors)
        {
            return ToBufferAccessor(vectors, VrmLib.AccessorValueType.FLOAT, VrmLib.AccessorVectorType.VEC3);
        }

        private static VrmLib.BufferAccessor ToBufferAccessor(Vector2[] vectors)
        {
            return ToBufferAccessor(vectors, VrmLib.AccessorValueType.FLOAT, VrmLib.AccessorVectorType.VEC2);
        }

        private static VrmLib.BufferAccessor ToBufferAccessor(int[] scalars)
        {
            return ToBufferAccessor(scalars, VrmLib.AccessorValueType.UNSIGNED_INT, VrmLib.AccessorVectorType.SCALAR);
        }

        private static VrmLib.BufferAccessor ToBufferAccessor(Matrix4x4[] matrixes)
        {
            return ToBufferAccessor(matrixes, VrmLib.AccessorValueType.FLOAT, VrmLib.AccessorVectorType.MAT4);
        }

        private static VrmLib.BufferAccessor ToBufferAccessor<T>(T[] value, VrmLib.AccessorValueType valueType, VrmLib.AccessorVectorType vectorType) where T : struct
        {
            var bytes = MemoryMarshal.Cast<T, byte>(value);
            return new VrmLib.BufferAccessor(
                bytes.ToArray(),
                valueType,
                vectorType,
                value.Length
                );
        }
    }
}