using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace UniVRM10
{
    public static class ComponentBuilder
    {
        #region Util
        static (Transform, Mesh) GetTransformAndMesh(Transform t)
        {
            var skinnedMeshRenderer = t.GetComponent<SkinnedMeshRenderer>();
            if (skinnedMeshRenderer != null)
            {
                return (t, skinnedMeshRenderer.sharedMesh);
            }

            var filter = t.GetComponent<MeshFilter>();
            if (filter != null)
            {
                return (t, filter.sharedMesh);
            }

            return default;
        }
        #endregion

        #region Build10

        static UniVRM10.BlendShapeBinding Build10(this VrmLib.BlendShapeBindValue bind, GameObject root, ModelMap loader)
        {
            var node = loader.Nodes[bind.Node].transform;
            var mesh = loader.Meshes[bind.Node.MeshGroup];
            // var transformMeshTable = loader.Root.transform.Traverse()
            //     .Select(GetTransformAndMesh)
            //     .Where(x => x.Item2 != null)
            //     .ToDictionary(x => x.Item2, x => x.Item1);
            // var node = transformMeshTable[mesh];
            // var transform = loader.Nodes[node].transform;
            var relativePath = node.RelativePathFrom(root.transform);

            var names = new List<string>();
            for (int i = 0; i < mesh.blendShapeCount; ++i)
            {
                names.Add(mesh.GetBlendShapeName(i));
            }

            return new UniVRM10.BlendShapeBinding
            {
                RelativePath = relativePath,
                Index = names.IndexOf(bind.Name),
                Weight = bind.Value,
            };
        }

        static UniVRM10.MaterialColorBinding? Build10(this VrmLib.MaterialBindValue bind, ModelMap loader)
        {
            var kv = bind.Property;
            var value = kv.Value.ToUnityVector4();
            var material = loader.Materials[bind.Material];

            var binding = default(UniVRM10.MaterialColorBinding?);
            if (material != null)
            {
                try
                {
                    binding = new UniVRM10.MaterialColorBinding
                    {
                        MaterialName = bind.Material.Name, // UniVRM-0Xの実装は名前で持っている
                        BindType = bind.BindType,
                        TargetValue = value,
                        // BaseValue = material.GetColor(kv.Key),
                    };
                }
                catch (Exception)
                {
                    // do nothing
                }
            }
            return binding;
        }

        public static void Build10(VrmLib.Model model, ModelAsset asset)
        {
            // meta
            {
                var meta = model.Vrm.Meta;
                var metaComponent = asset.Root.AddComponent<UniVRM10.VRMMeta>();
                metaComponent.Meta = ScriptableObject.CreateInstance<UniVRM10.VRMMetaObject>();
                metaComponent.Meta.Name = meta.Name;
                metaComponent.Meta.Version = meta.Version;
                metaComponent.Meta.Copyrights = meta.Copyrights;
                metaComponent.Meta.Authors = meta.Authors.ToArray();
                metaComponent.Meta.ContactInformation = meta.ContactInformation;
                metaComponent.Meta.Reference = meta.Reference;
                var thumbnailImages = asset.Map.Textures.Where(x => ((VrmLib.ImageTexture)x.Key).Image == meta.Thumbnail);
                if (meta.Thumbnail != null && thumbnailImages.Count() > 0)
                {
                    metaComponent.Meta.Thumbnail = thumbnailImages.First().Value;
                }
                else if (meta.Thumbnail != null && !meta.Thumbnail.Bytes.IsEmpty)
                {
                    var thumbnail = new Texture2D(2, 2, TextureFormat.ARGB32, false, false);
                    thumbnail.name = "Thumbnail";
                    thumbnail.LoadImage(meta.Thumbnail.Bytes.ToArray());
                    metaComponent.Meta.Thumbnail = thumbnail;
                    asset.Textures.Add(thumbnail);
                }
                // avatar permission
                metaComponent.Meta.AllowedUser = meta.AvatarPermission.AvatarUsage;
                metaComponent.Meta.ViolentUsage = meta.AvatarPermission.IsAllowedViolentUsage;
                metaComponent.Meta.SexualUsage = meta.AvatarPermission.IsAllowedSexualUsage;
                metaComponent.Meta.CommercialUsage = meta.AvatarPermission.CommercialUsage;
                metaComponent.Meta.GameUsage = meta.AvatarPermission.IsAllowedGameUsage;
                metaComponent.Meta.PoliticalOrReligiousUsage = meta.AvatarPermission.IsAllowedPoliticalOrReligiousUsage;
                metaComponent.Meta.OtherPermissionUrl = meta.AvatarPermission.OtherPermissionUrl;

                // redistribution license
                metaComponent.Meta.CreditNotation = meta.RedistributionLicense.CreditNotation;
                metaComponent.Meta.ModificationLicense = meta.RedistributionLicense.ModificationLicense;
                metaComponent.Meta.Redistribution = meta.RedistributionLicense.IsAllowRedistribution;
                metaComponent.Meta.OtherLicenseUrl = meta.RedistributionLicense.OtherLicenseUrl;

                asset.ScriptableObjects.Add(metaComponent.Meta);
            }

            // blendShape
            var blendShapeProxy = asset.Root.AddComponent<UniVRM10.VRMBlendShapeProxy>();
            {
                blendShapeProxy.BlendShapeAvatar = ScriptableObject.CreateInstance<UniVRM10.BlendShapeAvatar>();
                asset.ScriptableObjects.Add(blendShapeProxy.BlendShapeAvatar);
                if (model.Vrm.BlendShape != null)
                {
                    foreach (var blendShape in model.Vrm.BlendShape.BlendShapeList)
                    {
                        var clip = ScriptableObject.CreateInstance<UniVRM10.BlendShapeClip>();
                        clip.Preset = blendShape.Preset;
                        clip.BlendShapeName = blendShape.Name;
                        clip.IsBinary = blendShape.IsBinary;
                        clip.IgnoreBlink = blendShape.IgnoreBlink;
                        clip.IgnoreLookAt = blendShape.IgnoreLookAt;
                        clip.IgnoreMouth = blendShape.IgnoreMouth;

                        clip.BlendShapeBindings = blendShape.BlendShapeValues.Select(x => x.Build10(asset.Root, asset.Map))
                            .ToArray();
                        clip.MaterialColorBindings = blendShape.MaterialValues.Select(x => x.Build10(asset.Map))
                            .Where(x => x.HasValue)
                            .Select(x => x.Value)
                            .ToArray();
                        blendShapeProxy.BlendShapeAvatar.Clips.Add(clip);
                        asset.ScriptableObjects.Add(clip);
                    }
                }
            }

            // firstPerson
            {
                // VRMFirstPerson
                var firstPerson = asset.Root.AddComponent<UniVRM10.VRMFirstPerson>();
                firstPerson.Renderers = model.Vrm.FirstPerson.Annotations.Select(x =>
                    new UniVRM10.VRMFirstPerson.RendererFirstPersonFlags()
                    {
                        Renderer = asset.Map.Renderers[x.Node],
                        FirstPersonFlag = x.FirstPersonFlag
                    }
                    ).ToList();

                // VRMLookAtApplyer
                blendShapeProxy.OffsetFromHead = model.Vrm.LookAt.OffsetFromHeadBone.ToUnityVector3();
                if (model.Vrm.LookAt.LookAtType == VrmLib.LookAtType.BlendShape)
                {
                    var lookAtApplyer = blendShapeProxy;
                    lookAtApplyer.LookAtType = VRMBlendShapeProxy.LookAtTypes.BlendShape;
                    lookAtApplyer.HorizontalOuter = new UniVRM10.CurveMapper(
                        model.Vrm.LookAt.HorizontalOuter.InputMaxValue,
                        model.Vrm.LookAt.HorizontalOuter.OutputScaling);
                    lookAtApplyer.VerticalUp = new UniVRM10.CurveMapper(
                        model.Vrm.LookAt.VerticalUp.InputMaxValue,
                        model.Vrm.LookAt.VerticalUp.OutputScaling);
                    lookAtApplyer.VerticalDown = new UniVRM10.CurveMapper(
                        model.Vrm.LookAt.VerticalDown.InputMaxValue,
                        model.Vrm.LookAt.VerticalDown.OutputScaling);
                }
                else if (model.Vrm.LookAt.LookAtType == VrmLib.LookAtType.Bone)
                {
                    var lookAtBoneApplyer = blendShapeProxy;
                    lookAtBoneApplyer.HorizontalInner = new UniVRM10.CurveMapper(
                         model.Vrm.LookAt.HorizontalInner.InputMaxValue,
                         model.Vrm.LookAt.HorizontalInner.OutputScaling);
                    lookAtBoneApplyer.HorizontalOuter = new UniVRM10.CurveMapper(
                        model.Vrm.LookAt.HorizontalOuter.InputMaxValue,
                        model.Vrm.LookAt.HorizontalOuter.OutputScaling);
                    lookAtBoneApplyer.VerticalUp = new UniVRM10.CurveMapper(
                        model.Vrm.LookAt.VerticalUp.InputMaxValue,
                        model.Vrm.LookAt.VerticalUp.OutputScaling);
                    lookAtBoneApplyer.VerticalDown = new UniVRM10.CurveMapper(
                        model.Vrm.LookAt.VerticalDown.InputMaxValue,
                        model.Vrm.LookAt.VerticalDown.OutputScaling);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            // springBone
            {
                var colliders = new Dictionary<VrmLib.SpringBoneColliderGroup, UniVRM10.VRMSpringBoneColliderGroup>();
                foreach (var colliderGroup in model.Vrm.SpringBone.Colliders)
                {
                    var go = asset.Map.Nodes[colliderGroup.Node];
                    var springBoneColliderGroup = go.AddComponent<UniVRM10.VRMSpringBoneColliderGroup>();

                    springBoneColliderGroup.Colliders = colliderGroup.Colliders.Select(x =>
                    {
                        switch (x.ColliderType)
                        {
                            case VrmLib.VrmSpringBoneColliderTypes.Sphere:
                                return new UniVRM10.SpringBoneCollider()
                                {
                                    ColliderType = SpringBoneColliderTypes.Sphere,
                                    Offset = x.Offset.ToUnityVector3(),
                                    Radius = x.Radius
                                };

                            case VrmLib.VrmSpringBoneColliderTypes.Capsule:
                                return new UniVRM10.SpringBoneCollider()
                                {
                                    ColliderType = SpringBoneColliderTypes.Capsule,
                                    Offset = x.Offset.ToUnityVector3(),
                                    Radius = x.Radius,
                                    Tail = x.CapsuleTail.ToUnityVector3(),
                                };

                            default:
                                throw new NotImplementedException();
                        }
                    }).ToArray(); ;

                    colliders.Add(colliderGroup, springBoneColliderGroup);
                }

                GameObject springBoneObject = null;
                var springBoneTransform = asset.Root.transform.GetChildren().FirstOrDefault(x => x.name == "SpringBone");
                if (springBoneTransform == null)
                {
                    springBoneObject = new GameObject("SpringBone");
                }
                else
                {
                    springBoneObject = springBoneTransform.gameObject;
                }

                springBoneObject.transform.SetParent(asset.Root.transform);
                foreach (var spring in model.Vrm.SpringBone.Springs)
                {
                    var springBoneComponent = springBoneObject.AddComponent<UniVRM10.VRMSpringBone>();
                    springBoneComponent.m_comment = spring.Comment;
                    springBoneComponent.m_stiffnessForce = spring.Stiffness;
                    springBoneComponent.m_gravityPower = spring.GravityPower;
                    springBoneComponent.m_gravityDir = spring.GravityDir.ToUnityVector3();
                    springBoneComponent.m_dragForce = spring.DragForce;
                    if (spring.Origin != null && asset.Map.Nodes.TryGetValue(spring.Origin, out GameObject origin))
                    {
                        springBoneComponent.m_center = origin.transform;
                    }
                    springBoneComponent.RootBones = spring.Bones.Select(x => asset.Map.Nodes[x].transform).ToList();
                    springBoneComponent.m_hitRadius = spring.HitRadius;
                    springBoneComponent.ColliderGroups = spring.Colliders.Select(x => colliders[x]).ToArray();
                }
            }


            // Assets
            {
                var assetsContainer = asset.Root.AddComponent<UniVRM10.AssetsContainer>();
                assetsContainer.SetAsset(asset);
            }
        }
        #endregion
    }
}
