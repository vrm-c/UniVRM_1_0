using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

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

        /// Unity material を export => import して元の material と一致するか
        [Test]
        [TestCase("TestMToon", typeof(VrmLib.MToonMaterial))]
        [TestCase("TestUniUnlit", typeof(VrmLib.UnlitMaterial))]
        [TestCase("TestStandard", typeof(VrmLib.PBRMaterial))]
        [TestCase("TestUnlitColor", typeof(VrmLib.UnlitMaterial))]
        [TestCase("TestUnlitTexture", typeof(VrmLib.UnlitMaterial))]
        [TestCase("TestUnlitTransparent", typeof(VrmLib.UnlitMaterial))]
        [TestCase("TestUnlitCutout", typeof(VrmLib.UnlitMaterial))]
        public void UnityMaterialTest(string materialName, Type vrmLibMaterialType)
        {
            var src = Resources.Load<Material>(materialName);
            var converter = new UniVRM10.RuntimeVrmConverter();
            var vrmLibMaterial = converter.Export10(src, (a, b, c) => null);
            Debug.Log($"{src.shader.name} => {vrmLibMaterial}");
            Assert.AreEqual(vrmLibMaterialType, vrmLibMaterial.GetType());

            var textures = new List<VrmLib.Texture>();
            if (vrmLibMaterial is VrmLib.MToonMaterial mtoon)
            {
                // MToon
                Vrm10.MToonAdapter.MToonToGltf(mtoon, mtoon.Name, textures);
            }
            else if (vrmLibMaterial is VrmLib.UnlitMaterial unlit)
            {
                // Unlit
                Vrm10.MaterialAdapter.UnlitToGltf(unlit, unlit.Name, textures);
            }
            else if (vrmLibMaterial is VrmLib.PBRMaterial pbr)
            {
                // PBR
                Vrm10.MaterialAdapter.PBRToGltf(pbr, pbr.Name, textures);
            }
            else
            {
                throw new NotImplementedException();
            }

            // var protobufMaterial = ToProtobuf(vrmLibMaterial, textures);
            // var settings = Google.Protobuf.JsonFormatter.Settings.Default.WithPreserveProtoFieldNames(true);
            // var jsonMaterial = new Google.Protobuf.JsonFormatter(settings).Format(protobufMaterial);
        }
    }
}
