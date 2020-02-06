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

        static VrmProtobuf.Material ToProtobuf(VrmLib.Material vrmLibMaterial, List<VrmLib.Texture> textures)
        {
            if (vrmLibMaterial is VrmLib.PBRMaterial pbr)
            {
                return Vrm10.MaterialAdapter.PBRToGltf(pbr, pbr.Name, textures);
            }
            else if (vrmLibMaterial is VrmLib.UnlitMaterial unlit)
            {
                return Vrm10.MaterialAdapter.UnlitToGltf(unlit, unlit.Name, textures);
            }
            else if (vrmLibMaterial is VrmLib.MToonMaterial mtoon)
            {
                return Vrm10.MToonAdapter.MToonToGltf(mtoon, mtoon.Name, textures);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// Unity material を export => import して元の material と一致するか
        [Test]
        [TestCase("TestMToon")]
        [TestCase("TestUniUnlit")]
        [TestCase("TestStandard")]
        [TestCase("TestUnlitColor")]
        [TestCase("TestUnlitTexture")]
        [TestCase("TestUnlitTransparent")]
        [TestCase("TestUnlitCutout")]
        public void UnityMaterialTest(string materialName)
        {
            var src = Resources.Load<Material>(materialName);
            var converter = new UniVRM10.RuntimeVrmConverter();
            var vrmLibMaterial = converter.Export10(src, (a, b, c) => null);
            Debug.Log($"{src} => {vrmLibMaterial}");
            var textures = new List<VrmLib.Texture>();

            var protobufMaterial = ToProtobuf(vrmLibMaterial, textures);
            Assert.Null(protobufMaterial?.Extensions?.KHRMaterialsUnlit);
            var settings = Google.Protobuf.JsonFormatter.Settings.Default.WithPreserveProtoFieldNames(true);
            var jsonMaterial = new Google.Protobuf.JsonFormatter(settings).Format(protobufMaterial);

            Debug.Log(jsonMaterial);
        }
    }
}