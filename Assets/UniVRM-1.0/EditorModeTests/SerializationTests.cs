using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using VrmLib.Diff;
using static UnityEditor.ShaderUtil;

namespace Vrm10
{
    public class SerializationTests
    {
        [Test]
        public void MaterialTest()
        {
            var settings = Google.Protobuf.JsonFormatter.Settings.Default.WithPreserveProtoFieldNames(true);
            var q = "\"";

            {
                var data = new VrmProtobuf.Material
                {
                    Name = "Some",
                };

                var json = new Google.Protobuf.JsonFormatter(settings).Format(data);
                Assert.AreEqual($"{{ {q}name{q}: {q}Some{q} }}", json);
            }

            {
                var data = new VrmProtobuf.glTF();
                data.Textures.Add(new VrmProtobuf.Texture
                {

                });
                var json = new Google.Protobuf.JsonFormatter(settings).Format(data);
                // Assert.Equal($"{{ {q}name{q}: {q}Some{q} }}", json);
            }

            {
                var data = new VrmProtobuf.Material
                {
                    Name = "Alicia_body",
                    PbrMetallicRoughness = new VrmProtobuf.MaterialPbrMetallicRoughness
                    {
                        // BaseColorFactor = new[] { 1, 1, 1, 1 },
                        // BaseColorTexture= { }, 
                        MetallicFactor = 0,
                        RoughnessFactor = 0.9f
                    },
                    AlphaMode = "OPAQUE",
                    AlphaCutoff = 0.5f,
                    Extensions = new VrmProtobuf.Material.Types.Extensions
                    {
                        KHRMaterialsUnlit = { }
                    }
                };

                var json = new Google.Protobuf.JsonFormatter(settings).Format(data);
                // Assert.Equal($"{{ {q}name{q}: {q}Some{q} }}", json);
            }
        }

        static (VrmProtobuf.Material, bool) ToProtobufMaterial(VrmLib.Material vrmlibMaterial, List<VrmLib.Texture> textures)
        {
            if (vrmlibMaterial is VrmLib.MToonMaterial mtoon)
            {
                // MToon
                var protobufMaterial = Vrm10.MToonAdapter.MToonToGltf(mtoon, textures);
                return (protobufMaterial, true);
            }
            else if (vrmlibMaterial is VrmLib.UnlitMaterial unlit)
            {
                // Unlit
                var protobufMaterial = Vrm10.MaterialAdapter.UnlitToGltf(unlit, textures);
                return (protobufMaterial, true);
            }
            else if (vrmlibMaterial is VrmLib.PBRMaterial pbr)
            {
                // PBR
                var protobufMaterial = Vrm10.MaterialAdapter.PBRToGltf(pbr, textures);
                return (protobufMaterial, false);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        static void CompareUnityMaterial(Material lhs, Material rhs)
        {
            Assert.AreEqual(lhs.name, rhs.name);
            Assert.AreEqual(lhs.shader, rhs.shader);
            var sb = new StringBuilder();
            for (int i = 0; i < ShaderUtil.GetPropertyCount(lhs.shader); ++i)
            {
                var prop = ShaderUtil.GetPropertyName(lhs.shader, i);
                if (s_ignoreProps.Contains(prop))
                {
                    continue;
                }

                switch (ShaderUtil.GetPropertyType(lhs.shader, i))
                {
                    case ShaderPropertyType.Color:
                    case ShaderPropertyType.Vector:
                        {
                            var l = lhs.GetVector(prop);
                            var r = rhs.GetVector(prop);
                            if (l != r)
                            {
                                sb.AppendLine($"{prop} {l}!={r}");
                            }
                        }
                        break;

                    case ShaderPropertyType.Float:
                    case ShaderPropertyType.Range:
                        {
                            var l = lhs.GetFloat(prop);
                            var r = rhs.GetFloat(prop);
                            if (l != r)
                            {
                                sb.AppendLine($"{prop} {l}!={r}");
                            }
                        }
                        break;

                    case ShaderPropertyType.TexEnv:
                        {
                            var l = lhs.GetTextureOffset(prop);
                            var r = rhs.GetTextureOffset(prop);
                            if (l != r)
                            {
                                sb.AppendLine($"{prop} {l}!={r}");
                            }
                        }
                        break;

                    default:
                        throw new NotImplementedException(prop);
                }
            }
            if (sb.Length > 0)
            {
                Debug.LogWarning(sb.ToString());
            }
            Assert.AreEqual(0, sb.Length);
        }

        static string[] s_ignoreKeys = new string[]
        {
            "(MToonMaterial).Definition.MetaDefinition.VersionNumber",
        };

        static string[] s_ignoreProps = new string[]
        {
            "_ReceiveShadowRate",
            "_ShadingGradeRate",
            "_MToonVersion",
            "_Glossiness", // Gloss is burned into the texture and changed to the default value (1.0)
        };

        /// Unity material を export => import して元の material と一致するか
        [Test]
        [TestCase("TestMToon", typeof(VrmLib.MToonMaterial))]
        [TestCase("TestUniUnlit", typeof(VrmLib.UnlitMaterial))]
        [TestCase("TestStandard", typeof(VrmLib.PBRMaterial))]
        [TestCase("TestUnlitColor", typeof(VrmLib.UnlitMaterial), false)]
        [TestCase("TestUnlitTexture", typeof(VrmLib.UnlitMaterial), false)]
        [TestCase("TestUnlitTransparent", typeof(VrmLib.UnlitMaterial), false)]
        [TestCase("TestUnlitCutout", typeof(VrmLib.UnlitMaterial), false)]
        public void UnityMaterialTest(string materialName, Type vrmLibMaterialType, bool sameShader = true)
        {
            // cerate copy avoid modify asset
            var src = new Material(Resources.Load<Material>(materialName));

            // => vrmlib
            var converter = new UniVRM10.RuntimeVrmConverter();
            var vrmLibMaterial = converter.Export10(src, (a, b, c, d) => null);
            Assert.AreEqual(vrmLibMaterialType, vrmLibMaterial.GetType());

            // => protobuf
            var textures = new List<VrmLib.Texture>();
            var (protobufMaterial, hasKhrUnlit) = ToProtobufMaterial(vrmLibMaterial, textures);
            Assert.AreEqual(hasKhrUnlit, protobufMaterial.Extensions?.KHRMaterialsUnlit != null);

            // => json
            var settings = Google.Protobuf.JsonFormatter.Settings.Default.WithPreserveProtoFieldNames(true);
            var jsonMaterial = new Google.Protobuf.JsonFormatter(settings).Format(protobufMaterial);

            // <= json
            var parserSettings = Google.Protobuf.JsonParser.Settings.Default;
            var deserialized = new Google.Protobuf.JsonParser(parserSettings).Parse<VrmProtobuf.Material>(jsonMaterial);

            // <= protobuf
            var loaded = deserialized.FromGltf(textures);
            var context = ModelDiffContext.Create();
            ModelDiffExtensions.MaterialEquals(context, vrmLibMaterial, loaded);
            var diff = context.List
            .Where(x => !s_ignoreKeys.Contains(x.Context))
            .ToArray();
            if (diff.Length > 0)
            {
                Debug.LogWarning(string.Join("\n", diff.Select(x => $"{x.Context}: {x.Message}")));
            }
            Assert.AreEqual(0, diff.Length);

            // <= vrmlib
            var map = new Dictionary<VrmLib.Texture, Texture2D>();
            var dst = UniVRM10.RuntimeUnityMaterialBuilder.CreateMaterialAsset(loaded, hasVertexColor: false, map);
            dst.name = src.name;

            if (sameShader)
            {
                CompareUnityMaterial(src, dst);
            }
        }
    }
}
