using NUnit.Framework;

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
    }
}
