
using GltfFormat;
using System;
using System.Collections.Generic;
using System.Numerics;
using ObjectNotation;

namespace GltfSerialization.Generated {

public static class GltfDeserializer
{


public static Gltf Deserialize(JsonTreeNode parsed)
{
    var value = new Gltf();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="asset"){
            value.asset = Deserialize_gltf_asset(kv.Value);
            continue;
        }

        if(key=="buffers"){
            value.buffers = Deserialize_gltf_buffers(kv.Value);
            continue;
        }

        if(key=="bufferviews"){
            value.bufferViews = Deserialize_gltf_bufferViews(kv.Value);
            continue;
        }

        if(key=="accessors"){
            value.accessors = Deserialize_gltf_accessors(kv.Value);
            continue;
        }

        if(key=="textures"){
            value.textures = Deserialize_gltf_textures(kv.Value);
            continue;
        }

        if(key=="samplers"){
            value.samplers = Deserialize_gltf_samplers(kv.Value);
            continue;
        }

        if(key=="images"){
            value.images = Deserialize_gltf_images(kv.Value);
            continue;
        }

        if(key=="materials"){
            value.materials = Deserialize_gltf_materials(kv.Value);
            continue;
        }

        if(key=="meshes"){
            value.meshes = Deserialize_gltf_meshes(kv.Value);
            continue;
        }

        if(key=="nodes"){
            value.nodes = Deserialize_gltf_nodes(kv.Value);
            continue;
        }

        if(key=="skins"){
            value.skins = Deserialize_gltf_skins(kv.Value);
            continue;
        }

        if(key=="scene"){
            value.scene = kv.Value.GetInt32();
            continue;
        }

        if(key=="scenes"){
            value.scenes = Deserialize_gltf_scenes(kv.Value);
            continue;
        }

        if(key=="animations"){
            value.animations = Deserialize_gltf_animations(kv.Value);
            continue;
        }

        if(key=="cameras"){
            value.cameras = Deserialize_gltf_cameras(kv.Value);
            continue;
        }

        if(key=="extensionsused"){
            value.extensionsUsed = Deserialize_gltf_extensionsUsed(kv.Value);
            continue;
        }

        if(key=="extensionsrequired"){
            value.extensionsRequired = Deserialize_gltf_extensionsRequired(kv.Value);
            continue;
        }

        if(key=="extensions"){
            value.extensions = Deserialize_gltf_extensions(kv.Value);
            continue;
        }

    }
    return value;
}

public static GltfAsset Deserialize_gltf_asset(JsonTreeNode parsed)
{
    var value = new GltfAsset();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="generator"){
            value.generator = kv.Value.GetString();
            continue;
        }

        if(key=="version"){
            value.version = kv.Value.GetString();
            continue;
        }

        if(key=="copyright"){
            value.copyright = kv.Value.GetString();
            continue;
        }

        if(key=="minversion"){
            value.minVersion = kv.Value.GetString();
            continue;
        }

    }
    return value;
}

public static List<GltfFormat.GltfBuffer> Deserialize_gltf_buffers(JsonTreeNode parsed)
{
    var value = new List<GltfBuffer>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_buffers_LIST(x));
    }
	return value;
}
public static GltfBuffer Deserialize_gltf_buffers_LIST(JsonTreeNode parsed)
{
    var value = new GltfBuffer();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="uri"){
            value.uri = kv.Value.GetString();
            continue;
        }

        if(key=="bytelength"){
            value.byteLength = kv.Value.GetInt32();
            continue;
        }

        if(key=="name"){
            value.name = kv.Value.GetString();
            continue;
        }

    }
    return value;
}

public static List<GltfFormat.GltfBufferView> Deserialize_gltf_bufferViews(JsonTreeNode parsed)
{
    var value = new List<GltfBufferView>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_bufferViews_LIST(x));
    }
	return value;
}
public static GltfBufferView Deserialize_gltf_bufferViews_LIST(JsonTreeNode parsed)
{
    var value = new GltfBufferView();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="buffer"){
            value.buffer = kv.Value.GetInt32();
            continue;
        }

        if(key=="byteoffset"){
            value.byteOffset = kv.Value.GetInt32();
            continue;
        }

        if(key=="bytelength"){
            value.byteLength = kv.Value.GetInt32();
            continue;
        }

        if(key=="bytestride"){
            value.byteStride = kv.Value.GetInt32();
            continue;
        }

        if(key=="target"){
            value.target = (GltfBufferTargetType)kv.Value.GetInt32();
            continue;
        }

        if(key=="name"){
            value.name = kv.Value.GetString();
            continue;
        }

    }
    return value;
}

public static List<GltfFormat.GltfAccessor> Deserialize_gltf_accessors(JsonTreeNode parsed)
{
    var value = new List<GltfAccessor>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_accessors_LIST(x));
    }
	return value;
}
public static GltfAccessor Deserialize_gltf_accessors_LIST(JsonTreeNode parsed)
{
    var value = new GltfAccessor();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="bufferview"){
            value.bufferView = kv.Value.GetInt32();
            continue;
        }

        if(key=="byteoffset"){
            value.byteOffset = kv.Value.GetInt32();
            continue;
        }

        if(key=="type"){
            value.type = (GltfAccessorType)Enum.Parse(typeof(GltfAccessorType), kv.Value.GetString(), true);
            continue;
        }

        if(key=="componenttype"){
            value.componentType = (GltfComponentType)kv.Value.GetInt32();
            continue;
        }

        if(key=="count"){
            value.count = kv.Value.GetInt32();
            continue;
        }

        if(key=="max"){
            value.max = Deserialize_gltf_accessors__max(kv.Value);
            continue;
        }

        if(key=="min"){
            value.min = Deserialize_gltf_accessors__min(kv.Value);
            continue;
        }

        if(key=="normalized"){
            value.normalized = kv.Value.GetBoolean();
            continue;
        }

        if(key=="sparse"){
            value.sparse = Deserialize_gltf_accessors__sparse(kv.Value);
            continue;
        }

        if(key=="name"){
            value.name = kv.Value.GetString();
            continue;
        }

    }
    return value;
}

public static Single[] Deserialize_gltf_accessors__max(JsonTreeNode parsed)
{
    var value = new Single[parsed.GetArrayCount()];
    int i=0;
    foreach(var x in parsed.ArrayItems())
    {
        value[i++] = x.GetSingle();
    }
	return value;
} 

public static Single[] Deserialize_gltf_accessors__min(JsonTreeNode parsed)
{
    var value = new Single[parsed.GetArrayCount()];
    int i=0;
    foreach(var x in parsed.ArrayItems())
    {
        value[i++] = x.GetSingle();
    }
	return value;
} 

public static GltfSparse Deserialize_gltf_accessors__sparse(JsonTreeNode parsed)
{
    var value = new GltfSparse();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="count"){
            value.count = kv.Value.GetInt32();
            continue;
        }

        if(key=="indices"){
            value.indices = Deserialize_gltf_accessors__sparse_indices(kv.Value);
            continue;
        }

        if(key=="values"){
            value.values = Deserialize_gltf_accessors__sparse_values(kv.Value);
            continue;
        }

    }
    return value;
}

public static GltfSparseIndices Deserialize_gltf_accessors__sparse_indices(JsonTreeNode parsed)
{
    var value = new GltfSparseIndices();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="bufferview"){
            value.bufferView = kv.Value.GetInt32();
            continue;
        }

        if(key=="byteoffset"){
            value.byteOffset = kv.Value.GetInt32();
            continue;
        }

        if(key=="componenttype"){
            value.componentType = (GltfComponentType)kv.Value.GetInt32();
            continue;
        }

    }
    return value;
}

public static GltfSparseValues Deserialize_gltf_accessors__sparse_values(JsonTreeNode parsed)
{
    var value = new GltfSparseValues();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="bufferview"){
            value.bufferView = kv.Value.GetInt32();
            continue;
        }

        if(key=="byteoffset"){
            value.byteOffset = kv.Value.GetInt32();
            continue;
        }

    }
    return value;
}

public static List<GltfFormat.GltfTexture> Deserialize_gltf_textures(JsonTreeNode parsed)
{
    var value = new List<GltfTexture>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_textures_LIST(x));
    }
	return value;
}
public static GltfTexture Deserialize_gltf_textures_LIST(JsonTreeNode parsed)
{
    var value = new GltfTexture();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="sampler"){
            value.sampler = kv.Value.GetInt32();
            continue;
        }

        if(key=="source"){
            value.source = kv.Value.GetInt32();
            continue;
        }

        if(key=="extensions"){
            value.extensions = Deserialize_gltf_textures__extensions(kv.Value);
            continue;
        }

        if(key=="name"){
            value.name = kv.Value.GetString();
            continue;
        }

    }
    return value;
}

public static GltfTextureExtensions Deserialize_gltf_textures__extensions(JsonTreeNode parsed)
{
    var value = new GltfTextureExtensions();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="ext_texture_webp"){
            value.EXT_texture_webp = Deserialize_gltf_textures__extensions_EXT_texture_webp(kv.Value);
            continue;
        }

        if(key=="msft_texture_dds"){
            value.MSFT_texture_dds = Deserialize_gltf_textures__extensions_MSFT_texture_dds(kv.Value);
            continue;
        }

    }
    return value;
}

public static EXT_texture_webp Deserialize_gltf_textures__extensions_EXT_texture_webp(JsonTreeNode parsed)
{
    var value = new EXT_texture_webp();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="source"){
            value.source = kv.Value.GetInt32();
            continue;
        }

    }
    return value;
}

public static MSFT_texture_dds Deserialize_gltf_textures__extensions_MSFT_texture_dds(JsonTreeNode parsed)
{
    var value = new MSFT_texture_dds();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="source"){
            value.source = kv.Value.GetInt32();
            continue;
        }

    }
    return value;
}

public static List<GltfFormat.GltfTextureSampler> Deserialize_gltf_samplers(JsonTreeNode parsed)
{
    var value = new List<GltfTextureSampler>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_samplers_LIST(x));
    }
	return value;
}
public static GltfTextureSampler Deserialize_gltf_samplers_LIST(JsonTreeNode parsed)
{
    var value = new GltfTextureSampler();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="magfilter"){
            value.magFilter = (GltfMagFilterType)kv.Value.GetInt32();
            continue;
        }

        if(key=="minfilter"){
            value.minFilter = (GltfMinFilterType)kv.Value.GetInt32();
            continue;
        }

        if(key=="wraps"){
            value.wrapS = (GltfWrapType)kv.Value.GetInt32();
            continue;
        }

        if(key=="wrapt"){
            value.wrapT = (GltfWrapType)kv.Value.GetInt32();
            continue;
        }

        if(key=="name"){
            value.name = kv.Value.GetString();
            continue;
        }

    }
    return value;
}

public static List<GltfFormat.GltfImage> Deserialize_gltf_images(JsonTreeNode parsed)
{
    var value = new List<GltfImage>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_images_LIST(x));
    }
	return value;
}
public static GltfImage Deserialize_gltf_images_LIST(JsonTreeNode parsed)
{
    var value = new GltfImage();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="name"){
            value.name = kv.Value.GetString();
            continue;
        }

        if(key=="uri"){
            value.uri = kv.Value.GetString();
            continue;
        }

        if(key=="bufferview"){
            value.bufferView = kv.Value.GetInt32();
            continue;
        }

        if(key=="mimetype"){
            value.mimeType = kv.Value.GetString();
            continue;
        }

    }
    return value;
}

public static List<GltfFormat.GltfMaterial> Deserialize_gltf_materials(JsonTreeNode parsed)
{
    var value = new List<GltfMaterial>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_materials_LIST(x));
    }
	return value;
}
public static GltfMaterial Deserialize_gltf_materials_LIST(JsonTreeNode parsed)
{
    var value = new GltfMaterial();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="name"){
            value.name = kv.Value.GetString();
            continue;
        }

        if(key=="pbrmetallicroughness"){
            value.pbrMetallicRoughness = Deserialize_gltf_materials__pbrMetallicRoughness(kv.Value);
            continue;
        }

        if(key=="normaltexture"){
            value.normalTexture = Deserialize_gltf_materials__normalTexture(kv.Value);
            continue;
        }

        if(key=="occlusiontexture"){
            value.occlusionTexture = Deserialize_gltf_materials__occlusionTexture(kv.Value);
            continue;
        }

        if(key=="emissivetexture"){
            value.emissiveTexture = Deserialize_gltf_materials__emissiveTexture(kv.Value);
            continue;
        }

        if(key=="emissivefactor"){
            value.emissiveFactor = Deserialize_gltf_materials__emissiveFactor(kv.Value);
            continue;
        }

        if(key=="alphamode"){
            value.alphaMode = (AlphaModeType)Enum.Parse(typeof(AlphaModeType), kv.Value.GetString(), true);
            continue;
        }

        if(key=="alphacutoff"){
            value.alphaCutoff = kv.Value.GetSingle();
            continue;
        }

        if(key=="doublesided"){
            value.doubleSided = kv.Value.GetBoolean();
            continue;
        }

        if(key=="extensions"){
            value.extensions = Deserialize_gltf_materials__extensions(kv.Value);
            continue;
        }

    }
    return value;
}

public static GltfPbrMetallicRoughness Deserialize_gltf_materials__pbrMetallicRoughness(JsonTreeNode parsed)
{
    var value = new GltfPbrMetallicRoughness();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="basecolortexture"){
            value.baseColorTexture = Deserialize_gltf_materials__pbrMetallicRoughness_baseColorTexture(kv.Value);
            continue;
        }

        if(key=="basecolorfactor"){
            value.baseColorFactor = Deserialize_gltf_materials__pbrMetallicRoughness_baseColorFactor(kv.Value);
            continue;
        }

        if(key=="metallicroughnesstexture"){
            value.metallicRoughnessTexture = Deserialize_gltf_materials__pbrMetallicRoughness_metallicRoughnessTexture(kv.Value);
            continue;
        }

        if(key=="metallicfactor"){
            value.metallicFactor = kv.Value.GetSingle();
            continue;
        }

        if(key=="roughnessfactor"){
            value.roughnessFactor = kv.Value.GetSingle();
            continue;
        }

    }
    return value;
}

public static GltfMaterialBaseColorTextureInfo Deserialize_gltf_materials__pbrMetallicRoughness_baseColorTexture(JsonTreeNode parsed)
{
    var value = new GltfMaterialBaseColorTextureInfo();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="index"){
            value.index = kv.Value.GetInt32();
            continue;
        }

        if(key=="texcoord"){
            value.texCoord = kv.Value.GetInt32();
            continue;
        }

    }
    return value;
}

public static Single[] Deserialize_gltf_materials__pbrMetallicRoughness_baseColorFactor(JsonTreeNode parsed)
{
    var value = new Single[parsed.GetArrayCount()];
    int i=0;
    foreach(var x in parsed.ArrayItems())
    {
        value[i++] = x.GetSingle();
    }
	return value;
} 

public static GltfMaterialMetallicRoughnessTextureInfo Deserialize_gltf_materials__pbrMetallicRoughness_metallicRoughnessTexture(JsonTreeNode parsed)
{
    var value = new GltfMaterialMetallicRoughnessTextureInfo();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="index"){
            value.index = kv.Value.GetInt32();
            continue;
        }

        if(key=="texcoord"){
            value.texCoord = kv.Value.GetInt32();
            continue;
        }

    }
    return value;
}

public static GltfMaterialNormalTextureInfo Deserialize_gltf_materials__normalTexture(JsonTreeNode parsed)
{
    var value = new GltfMaterialNormalTextureInfo();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="scale"){
            value.scale = kv.Value.GetSingle();
            continue;
        }

        if(key=="index"){
            value.index = kv.Value.GetInt32();
            continue;
        }

        if(key=="texcoord"){
            value.texCoord = kv.Value.GetInt32();
            continue;
        }

    }
    return value;
}

public static GltfMaterialOcclusionTextureInfo Deserialize_gltf_materials__occlusionTexture(JsonTreeNode parsed)
{
    var value = new GltfMaterialOcclusionTextureInfo();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="strength"){
            value.strength = kv.Value.GetSingle();
            continue;
        }

        if(key=="index"){
            value.index = kv.Value.GetInt32();
            continue;
        }

        if(key=="texcoord"){
            value.texCoord = kv.Value.GetInt32();
            continue;
        }

    }
    return value;
}

public static GltfMaterialEmissiveTextureInfo Deserialize_gltf_materials__emissiveTexture(JsonTreeNode parsed)
{
    var value = new GltfMaterialEmissiveTextureInfo();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="index"){
            value.index = kv.Value.GetInt32();
            continue;
        }

        if(key=="texcoord"){
            value.texCoord = kv.Value.GetInt32();
            continue;
        }

    }
    return value;
}

public static Single[] Deserialize_gltf_materials__emissiveFactor(JsonTreeNode parsed)
{
    var value = new Single[parsed.GetArrayCount()];
    int i=0;
    foreach(var x in parsed.ArrayItems())
    {
        value[i++] = x.GetSingle();
    }
	return value;
} 

public static GltfMaterial_Extensions Deserialize_gltf_materials__extensions(JsonTreeNode parsed)
{
    var value = new GltfMaterial_Extensions();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="khr_materials_unlit"){
            value.KHR_materials_unlit = Deserialize_gltf_materials__extensions_KHR_materials_unlit(kv.Value);
            continue;
        }

    }
    return value;
}

public static GltfMaterialExtension_KHR_materials_unlit Deserialize_gltf_materials__extensions_KHR_materials_unlit(JsonTreeNode parsed)
{
    var value = new GltfMaterialExtension_KHR_materials_unlit();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

    }
    return value;
}

public static List<GltfFormat.GltfMesh> Deserialize_gltf_meshes(JsonTreeNode parsed)
{
    var value = new List<GltfMesh>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_meshes_LIST(x));
    }
	return value;
}
public static GltfMesh Deserialize_gltf_meshes_LIST(JsonTreeNode parsed)
{
    var value = new GltfMesh();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="name"){
            value.name = kv.Value.GetString();
            continue;
        }

        if(key=="primitives"){
            value.primitives = Deserialize_gltf_meshes__primitives(kv.Value);
            continue;
        }

        if(key=="weights"){
            value.weights = Deserialize_gltf_meshes__weights(kv.Value);
            continue;
        }

    }
    return value;
}

public static List<GltfFormat.GltfPrimitive> Deserialize_gltf_meshes__primitives(JsonTreeNode parsed)
{
    var value = new List<GltfPrimitive>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_meshes__primitives_LIST(x));
    }
	return value;
}
public static GltfPrimitive Deserialize_gltf_meshes__primitives_LIST(JsonTreeNode parsed)
{
    var value = new GltfPrimitive();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="mode"){
            value.mode = (GltfPrimitiveMode)kv.Value.GetInt32();
            continue;
        }

        if(key=="indices"){
            value.indices = kv.Value.GetInt32();
            continue;
        }

        if(key=="attributes"){
            value.attributes = Deserialize_gltf_meshes__primitives__attributes(kv.Value);
            continue;
        }

        if(key=="material"){
            value.material = kv.Value.GetInt32();
            continue;
        }

        if(key=="targets"){
            value.targets = Deserialize_gltf_meshes__primitives__targets(kv.Value);
            continue;
        }

        if(key=="extras"){
            value.extras = Deserialize_gltf_meshes__primitives__extras(kv.Value);
            continue;
        }

    }
    return value;
}

public static GltfAttributes Deserialize_gltf_meshes__primitives__attributes(JsonTreeNode parsed)
{
    var value = new GltfAttributes();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="position"){
            value.POSITION = kv.Value.GetInt32();
            continue;
        }

        if(key=="normal"){
            value.NORMAL = kv.Value.GetInt32();
            continue;
        }

        if(key=="tangent"){
            value.TANGENT = kv.Value.GetInt32();
            continue;
        }

        if(key=="texcoord_0"){
            value.TEXCOORD_0 = kv.Value.GetInt32();
            continue;
        }

        if(key=="color_0"){
            value.COLOR_0 = kv.Value.GetInt32();
            continue;
        }

        if(key=="joints_0"){
            value.JOINTS_0 = kv.Value.GetInt32();
            continue;
        }

        if(key=="weights_0"){
            value.WEIGHTS_0 = kv.Value.GetInt32();
            continue;
        }

    }
    return value;
}

public static List<GltfFormat.GltfMorphTarget> Deserialize_gltf_meshes__primitives__targets(JsonTreeNode parsed)
{
    var value = new List<GltfMorphTarget>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_meshes__primitives__targets_LIST(x));
    }
	return value;
}
public static GltfMorphTarget Deserialize_gltf_meshes__primitives__targets_LIST(JsonTreeNode parsed)
{
    var value = new GltfMorphTarget();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="position"){
            value.POSITION = kv.Value.GetInt32();
            continue;
        }

        if(key=="normal"){
            value.NORMAL = kv.Value.GetInt32();
            continue;
        }

        if(key=="tangent"){
            value.TANGENT = kv.Value.GetInt32();
            continue;
        }

    }
    return value;
}

public static GltfPrimitive_Extras Deserialize_gltf_meshes__primitives__extras(JsonTreeNode parsed)
{
    var value = new GltfPrimitive_Extras();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="targetnames"){
            value.targetNames = Deserialize_gltf_meshes__primitives__extras_targetNames(kv.Value);
            continue;
        }

    }
    return value;
}

public static List<System.String> Deserialize_gltf_meshes__primitives__extras_targetNames(JsonTreeNode parsed)
{
    var value = new List<String>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(x.GetString());
    }
	return value;
}
public static Single[] Deserialize_gltf_meshes__weights(JsonTreeNode parsed)
{
    var value = new Single[parsed.GetArrayCount()];
    int i=0;
    foreach(var x in parsed.ArrayItems())
    {
        value[i++] = x.GetSingle();
    }
	return value;
} 

public static List<GltfFormat.GltfNode> Deserialize_gltf_nodes(JsonTreeNode parsed)
{
    var value = new List<GltfNode>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_nodes_LIST(x));
    }
	return value;
}
public static GltfNode Deserialize_gltf_nodes_LIST(JsonTreeNode parsed)
{
    var value = new GltfNode();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="name"){
            value.name = kv.Value.GetString();
            continue;
        }

        if(key=="children"){
            value.children = Deserialize_gltf_nodes__children(kv.Value);
            continue;
        }

        if(key=="matrix"){
            value.matrix = Deserialize_gltf_nodes__matrix(kv.Value);
            continue;
        }

        if(key=="translation"){
            value.translation = Deserialize_gltf_nodes__translation(kv.Value);
            continue;
        }

        if(key=="rotation"){
            value.rotation = Deserialize_gltf_nodes__rotation(kv.Value);
            continue;
        }

        if(key=="scale"){
            value.scale = Deserialize_gltf_nodes__scale(kv.Value);
            continue;
        }

        if(key=="mesh"){
            value.mesh = kv.Value.GetInt32();
            continue;
        }

        if(key=="skin"){
            value.skin = kv.Value.GetInt32();
            continue;
        }

        if(key=="weights"){
            value.weights = Deserialize_gltf_nodes__weights(kv.Value);
            continue;
        }

        if(key=="camera"){
            value.camera = kv.Value.GetInt32();
            continue;
        }

    }
    return value;
}

public static Int32[] Deserialize_gltf_nodes__children(JsonTreeNode parsed)
{
    var value = new Int32[parsed.GetArrayCount()];
    int i=0;
    foreach(var x in parsed.ArrayItems())
    {
        value[i++] = x.GetInt32();
    }
	return value;
} 

public static Single[] Deserialize_gltf_nodes__matrix(JsonTreeNode parsed)
{
    var value = new Single[parsed.GetArrayCount()];
    int i=0;
    foreach(var x in parsed.ArrayItems())
    {
        value[i++] = x.GetSingle();
    }
	return value;
} 

public static Single[] Deserialize_gltf_nodes__translation(JsonTreeNode parsed)
{
    var value = new Single[parsed.GetArrayCount()];
    int i=0;
    foreach(var x in parsed.ArrayItems())
    {
        value[i++] = x.GetSingle();
    }
	return value;
} 

public static Single[] Deserialize_gltf_nodes__rotation(JsonTreeNode parsed)
{
    var value = new Single[parsed.GetArrayCount()];
    int i=0;
    foreach(var x in parsed.ArrayItems())
    {
        value[i++] = x.GetSingle();
    }
	return value;
} 

public static Single[] Deserialize_gltf_nodes__scale(JsonTreeNode parsed)
{
    var value = new Single[parsed.GetArrayCount()];
    int i=0;
    foreach(var x in parsed.ArrayItems())
    {
        value[i++] = x.GetSingle();
    }
	return value;
} 

public static Single[] Deserialize_gltf_nodes__weights(JsonTreeNode parsed)
{
    var value = new Single[parsed.GetArrayCount()];
    int i=0;
    foreach(var x in parsed.ArrayItems())
    {
        value[i++] = x.GetSingle();
    }
	return value;
} 

public static List<GltfFormat.GltfSkin> Deserialize_gltf_skins(JsonTreeNode parsed)
{
    var value = new List<GltfSkin>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_skins_LIST(x));
    }
	return value;
}
public static GltfSkin Deserialize_gltf_skins_LIST(JsonTreeNode parsed)
{
    var value = new GltfSkin();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="inversebindmatrices"){
            value.inverseBindMatrices = kv.Value.GetInt32();
            continue;
        }

        if(key=="joints"){
            value.joints = Deserialize_gltf_skins__joints(kv.Value);
            continue;
        }

        if(key=="skeleton"){
            value.skeleton = kv.Value.GetInt32();
            continue;
        }

        if(key=="name"){
            value.name = kv.Value.GetString();
            continue;
        }

    }
    return value;
}

public static Int32[] Deserialize_gltf_skins__joints(JsonTreeNode parsed)
{
    var value = new Int32[parsed.GetArrayCount()];
    int i=0;
    foreach(var x in parsed.ArrayItems())
    {
        value[i++] = x.GetInt32();
    }
	return value;
} 

public static List<GltfFormat.GltfScene> Deserialize_gltf_scenes(JsonTreeNode parsed)
{
    var value = new List<GltfScene>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_scenes_LIST(x));
    }
	return value;
}
public static GltfScene Deserialize_gltf_scenes_LIST(JsonTreeNode parsed)
{
    var value = new GltfScene();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="nodes"){
            value.nodes = Deserialize_gltf_scenes__nodes(kv.Value);
            continue;
        }

        if(key=="name"){
            value.name = kv.Value.GetString();
            continue;
        }

    }
    return value;
}

public static Int32[] Deserialize_gltf_scenes__nodes(JsonTreeNode parsed)
{
    var value = new Int32[parsed.GetArrayCount()];
    int i=0;
    foreach(var x in parsed.ArrayItems())
    {
        value[i++] = x.GetInt32();
    }
	return value;
} 

public static List<GltfFormat.GltfAnimation> Deserialize_gltf_animations(JsonTreeNode parsed)
{
    var value = new List<GltfAnimation>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_animations_LIST(x));
    }
	return value;
}
public static GltfAnimation Deserialize_gltf_animations_LIST(JsonTreeNode parsed)
{
    var value = new GltfAnimation();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="name"){
            value.name = kv.Value.GetString();
            continue;
        }

        if(key=="channels"){
            value.channels = Deserialize_gltf_animations__channels(kv.Value);
            continue;
        }

        if(key=="samplers"){
            value.samplers = Deserialize_gltf_animations__samplers(kv.Value);
            continue;
        }

    }
    return value;
}

public static List<GltfFormat.GltfAnimationChannel> Deserialize_gltf_animations__channels(JsonTreeNode parsed)
{
    var value = new List<GltfAnimationChannel>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_animations__channels_LIST(x));
    }
	return value;
}
public static GltfAnimationChannel Deserialize_gltf_animations__channels_LIST(JsonTreeNode parsed)
{
    var value = new GltfAnimationChannel();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="sampler"){
            value.sampler = kv.Value.GetInt32();
            continue;
        }

        if(key=="target"){
            value.target = Deserialize_gltf_animations__channels__target(kv.Value);
            continue;
        }

    }
    return value;
}

public static GltfAnimationTarget Deserialize_gltf_animations__channels__target(JsonTreeNode parsed)
{
    var value = new GltfAnimationTarget();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="node"){
            value.node = kv.Value.GetInt32();
            continue;
        }

        if(key=="path"){
            value.path = (GltfAnimationPathType)Enum.Parse(typeof(GltfAnimationPathType), kv.Value.GetString(), true);
            continue;
        }

    }
    return value;
}

public static List<GltfFormat.GltfAnimationSampler> Deserialize_gltf_animations__samplers(JsonTreeNode parsed)
{
    var value = new List<GltfAnimationSampler>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_animations__samplers_LIST(x));
    }
	return value;
}
public static GltfAnimationSampler Deserialize_gltf_animations__samplers_LIST(JsonTreeNode parsed)
{
    var value = new GltfAnimationSampler();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="input"){
            value.input = kv.Value.GetInt32();
            continue;
        }

        if(key=="interpolation"){
            value.interpolation = (GltfInterpolationType)Enum.Parse(typeof(GltfInterpolationType), kv.Value.GetString(), true);
            continue;
        }

        if(key=="output"){
            value.output = kv.Value.GetInt32();
            continue;
        }

    }
    return value;
}

public static List<GltfFormat.GltfCamera> Deserialize_gltf_cameras(JsonTreeNode parsed)
{
    var value = new List<GltfCamera>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_cameras_LIST(x));
    }
	return value;
}
public static GltfCamera Deserialize_gltf_cameras_LIST(JsonTreeNode parsed)
{
    var value = new GltfCamera();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="orthographic"){
            value.orthographic = Deserialize_gltf_cameras__orthographic(kv.Value);
            continue;
        }

        if(key=="perspective"){
            value.perspective = Deserialize_gltf_cameras__perspective(kv.Value);
            continue;
        }

        if(key=="type"){
            value.type = (GltfProjectionType)Enum.Parse(typeof(GltfProjectionType), kv.Value.GetString(), true);
            continue;
        }

        if(key=="name"){
            value.name = kv.Value.GetString();
            continue;
        }

    }
    return value;
}

public static GltfOrthographic Deserialize_gltf_cameras__orthographic(JsonTreeNode parsed)
{
    var value = new GltfOrthographic();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="xmag"){
            value.xmag = kv.Value.GetSingle();
            continue;
        }

        if(key=="ymag"){
            value.ymag = kv.Value.GetSingle();
            continue;
        }

        if(key=="zfar"){
            value.zfar = kv.Value.GetSingle();
            continue;
        }

        if(key=="znear"){
            value.znear = kv.Value.GetSingle();
            continue;
        }

    }
    return value;
}

public static GltfPerspective Deserialize_gltf_cameras__perspective(JsonTreeNode parsed)
{
    var value = new GltfPerspective();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="aspectratio"){
            value.aspectRatio = kv.Value.GetSingle();
            continue;
        }

        if(key=="yfov"){
            value.yfov = kv.Value.GetSingle();
            continue;
        }

        if(key=="zfar"){
            value.zfar = kv.Value.GetSingle();
            continue;
        }

        if(key=="znear"){
            value.znear = kv.Value.GetSingle();
            continue;
        }

    }
    return value;
}

public static List<System.String> Deserialize_gltf_extensionsUsed(JsonTreeNode parsed)
{
    var value = new List<String>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(x.GetString());
    }
	return value;
}
public static List<System.String> Deserialize_gltf_extensionsRequired(JsonTreeNode parsed)
{
    var value = new List<String>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(x.GetString());
    }
	return value;
}
public static GltfExtensions Deserialize_gltf_extensions(JsonTreeNode parsed)
{
    var value = new GltfExtensions();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="vrm"){
            value.VRM = Deserialize_gltf_extensions_VRM(kv.Value);
            continue;
        }

    }
    return value;
}

public static GltfExtensionVrm Deserialize_gltf_extensions_VRM(JsonTreeNode parsed)
{
    var value = new GltfExtensionVrm();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="exporterversion"){
            value.exporterVersion = kv.Value.GetString();
            continue;
        }

        if(key=="specversion"){
            value.specVersion = kv.Value.GetString();
            continue;
        }

        if(key=="meta"){
            value.meta = Deserialize_gltf_extensions_VRM_meta(kv.Value);
            continue;
        }

        if(key=="humanoid"){
            value.humanoid = Deserialize_gltf_extensions_VRM_humanoid(kv.Value);
            continue;
        }

        if(key=="firstperson"){
            value.firstPerson = Deserialize_gltf_extensions_VRM_firstPerson(kv.Value);
            continue;
        }

        if(key=="blendshapemaster"){
            value.blendShapeMaster = Deserialize_gltf_extensions_VRM_blendShapeMaster(kv.Value);
            continue;
        }

        if(key=="secondaryanimation"){
            value.secondaryAnimation = Deserialize_gltf_extensions_VRM_secondaryAnimation(kv.Value);
            continue;
        }

        if(key=="materialproperties"){
            value.materialProperties = Deserialize_gltf_extensions_VRM_materialProperties(kv.Value);
            continue;
        }

    }
    return value;
}

public static VrmMeta Deserialize_gltf_extensions_VRM_meta(JsonTreeNode parsed)
{
    var value = new VrmMeta();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="title"){
            value.title = kv.Value.GetString();
            continue;
        }

        if(key=="version"){
            value.version = kv.Value.GetString();
            continue;
        }

        if(key=="author"){
            value.author = kv.Value.GetString();
            continue;
        }

        if(key=="contactinformation"){
            value.contactInformation = kv.Value.GetString();
            continue;
        }

        if(key=="reference"){
            value.reference = kv.Value.GetString();
            continue;
        }

        if(key=="texture"){
            value.texture = kv.Value.GetInt32();
            continue;
        }

        if(key=="allowedusername"){
            value.allowedUserName = (VrmAllowedUser)Enum.Parse(typeof(VrmAllowedUser), kv.Value.GetString(), true);
            continue;
        }

        if(key=="violentussagename"){
            value.violentUssageName = (VrmUssageLicense)Enum.Parse(typeof(VrmUssageLicense), kv.Value.GetString(), true);
            continue;
        }

        if(key=="sexualussagename"){
            value.sexualUssageName = (VrmUssageLicense)Enum.Parse(typeof(VrmUssageLicense), kv.Value.GetString(), true);
            continue;
        }

        if(key=="commercialussagename"){
            value.commercialUssageName = (VrmUssageLicense)Enum.Parse(typeof(VrmUssageLicense), kv.Value.GetString(), true);
            continue;
        }

        if(key=="otherpermissionurl"){
            value.otherPermissionUrl = kv.Value.GetString();
            continue;
        }

        if(key=="licensename"){
            value.licenseName = (VrmLicenseType)Enum.Parse(typeof(VrmLicenseType), kv.Value.GetString(), true);
            continue;
        }

        if(key=="otherlicenseurl"){
            value.otherLicenseUrl = kv.Value.GetString();
            continue;
        }

    }
    return value;
}

public static VrmHumanoid Deserialize_gltf_extensions_VRM_humanoid(JsonTreeNode parsed)
{
    var value = new VrmHumanoid();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="humanbones"){
            value.humanBones = Deserialize_gltf_extensions_VRM_humanoid_humanBones(kv.Value);
            continue;
        }

        if(key=="armstretch"){
            value.armStretch = kv.Value.GetSingle();
            continue;
        }

        if(key=="legstretch"){
            value.legStretch = kv.Value.GetSingle();
            continue;
        }

        if(key=="upperarmtwist"){
            value.upperArmTwist = kv.Value.GetSingle();
            continue;
        }

        if(key=="lowerarmtwist"){
            value.lowerArmTwist = kv.Value.GetSingle();
            continue;
        }

        if(key=="upperlegtwist"){
            value.upperLegTwist = kv.Value.GetSingle();
            continue;
        }

        if(key=="lowerlegtwist"){
            value.lowerLegTwist = kv.Value.GetSingle();
            continue;
        }

        if(key=="feetspacing"){
            value.feetSpacing = kv.Value.GetSingle();
            continue;
        }

        if(key=="hastranslationdof"){
            value.hasTranslationDoF = kv.Value.GetBoolean();
            continue;
        }

    }
    return value;
}

public static List<GltfFormat.VrmHumanoidBone> Deserialize_gltf_extensions_VRM_humanoid_humanBones(JsonTreeNode parsed)
{
    var value = new List<VrmHumanoidBone>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_extensions_VRM_humanoid_humanBones_LIST(x));
    }
	return value;
}
public static VrmHumanoidBone Deserialize_gltf_extensions_VRM_humanoid_humanBones_LIST(JsonTreeNode parsed)
{
    var value = new VrmHumanoidBone();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="bone"){
            value.bone = (HumanoidBones)Enum.Parse(typeof(HumanoidBones), kv.Value.GetString(), true);
            continue;
        }

        if(key=="node"){
            value.node = kv.Value.GetInt32();
            continue;
        }

        if(key=="usedefaultvalues"){
            value.useDefaultValues = kv.Value.GetBoolean();
            continue;
        }

        if(key=="min"){
            value.min = Deserialize_gltf_extensions_VRM_humanoid_humanBones__min(kv.Value);
            continue;
        }

        if(key=="max"){
            value.max = Deserialize_gltf_extensions_VRM_humanoid_humanBones__max(kv.Value);
            continue;
        }

        if(key=="center"){
            value.center = Deserialize_gltf_extensions_VRM_humanoid_humanBones__center(kv.Value);
            continue;
        }

        if(key=="axislength"){
            value.axisLength = kv.Value.GetSingle();
            continue;
        }

    }
    return value;
}

public static Vector3 Deserialize_gltf_extensions_VRM_humanoid_humanBones__min(JsonTreeNode parsed)
{
    var value = new Vector3();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="x"){
            value.X = kv.Value.GetSingle();
            continue;
        }

        if(key=="y"){
            value.Y = kv.Value.GetSingle();
            continue;
        }

        if(key=="z"){
            value.Z = kv.Value.GetSingle();
            continue;
        }

    }
    return value;
}

public static Vector3 Deserialize_gltf_extensions_VRM_humanoid_humanBones__max(JsonTreeNode parsed)
{
    var value = new Vector3();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="x"){
            value.X = kv.Value.GetSingle();
            continue;
        }

        if(key=="y"){
            value.Y = kv.Value.GetSingle();
            continue;
        }

        if(key=="z"){
            value.Z = kv.Value.GetSingle();
            continue;
        }

    }
    return value;
}

public static Vector3 Deserialize_gltf_extensions_VRM_humanoid_humanBones__center(JsonTreeNode parsed)
{
    var value = new Vector3();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="x"){
            value.X = kv.Value.GetSingle();
            continue;
        }

        if(key=="y"){
            value.Y = kv.Value.GetSingle();
            continue;
        }

        if(key=="z"){
            value.Z = kv.Value.GetSingle();
            continue;
        }

    }
    return value;
}

public static VrmFirstPerson Deserialize_gltf_extensions_VRM_firstPerson(JsonTreeNode parsed)
{
    var value = new VrmFirstPerson();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="firstpersonbone"){
            value.firstPersonBone = kv.Value.GetInt32();
            continue;
        }

        if(key=="firstpersonboneoffset"){
            value.firstPersonBoneOffset = Deserialize_gltf_extensions_VRM_firstPerson_firstPersonBoneOffset(kv.Value);
            continue;
        }

        if(key=="meshannotations"){
            value.meshAnnotations = Deserialize_gltf_extensions_VRM_firstPerson_meshAnnotations(kv.Value);
            continue;
        }

        if(key=="lookattypename"){
            value.lookAtTypeName = (VrmLookAtType)Enum.Parse(typeof(VrmLookAtType), kv.Value.GetString(), true);
            continue;
        }

        if(key=="lookathorizontalinner"){
            value.lookAtHorizontalInner = Deserialize_gltf_extensions_VRM_firstPerson_lookAtHorizontalInner(kv.Value);
            continue;
        }

        if(key=="lookathorizontalouter"){
            value.lookAtHorizontalOuter = Deserialize_gltf_extensions_VRM_firstPerson_lookAtHorizontalOuter(kv.Value);
            continue;
        }

        if(key=="lookatverticaldown"){
            value.lookAtVerticalDown = Deserialize_gltf_extensions_VRM_firstPerson_lookAtVerticalDown(kv.Value);
            continue;
        }

        if(key=="lookatverticalup"){
            value.lookAtVerticalUp = Deserialize_gltf_extensions_VRM_firstPerson_lookAtVerticalUp(kv.Value);
            continue;
        }

    }
    return value;
}

public static Vector3 Deserialize_gltf_extensions_VRM_firstPerson_firstPersonBoneOffset(JsonTreeNode parsed)
{
    var value = new Vector3();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="x"){
            value.X = kv.Value.GetSingle();
            continue;
        }

        if(key=="y"){
            value.Y = kv.Value.GetSingle();
            continue;
        }

        if(key=="z"){
            value.Z = kv.Value.GetSingle();
            continue;
        }

    }
    return value;
}

public static List<GltfFormat.VrmMeshAnnotation> Deserialize_gltf_extensions_VRM_firstPerson_meshAnnotations(JsonTreeNode parsed)
{
    var value = new List<VrmMeshAnnotation>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_extensions_VRM_firstPerson_meshAnnotations_LIST(x));
    }
	return value;
}
public static VrmMeshAnnotation Deserialize_gltf_extensions_VRM_firstPerson_meshAnnotations_LIST(JsonTreeNode parsed)
{
    var value = new VrmMeshAnnotation();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="mesh"){
            value.mesh = kv.Value.GetInt32();
            continue;
        }

        if(key=="firstpersonflag"){
            value.firstPersonFlag = (VrmFirstPersonFlag)Enum.Parse(typeof(VrmFirstPersonFlag), kv.Value.GetString(), true);
            continue;
        }

    }
    return value;
}

public static VrmDegreeMap Deserialize_gltf_extensions_VRM_firstPerson_lookAtHorizontalInner(JsonTreeNode parsed)
{
    var value = new VrmDegreeMap();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="curve"){
            value.curve = Deserialize_gltf_extensions_VRM_firstPerson_lookAtHorizontalInner_curve(kv.Value);
            continue;
        }

        if(key=="xrange"){
            value.xRange = kv.Value.GetSingle();
            continue;
        }

        if(key=="yrange"){
            value.yRange = kv.Value.GetSingle();
            continue;
        }

    }
    return value;
}

public static Single[] Deserialize_gltf_extensions_VRM_firstPerson_lookAtHorizontalInner_curve(JsonTreeNode parsed)
{
    var value = new Single[parsed.GetArrayCount()];
    int i=0;
    foreach(var x in parsed.ArrayItems())
    {
        value[i++] = x.GetSingle();
    }
	return value;
} 

public static VrmDegreeMap Deserialize_gltf_extensions_VRM_firstPerson_lookAtHorizontalOuter(JsonTreeNode parsed)
{
    var value = new VrmDegreeMap();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="curve"){
            value.curve = Deserialize_gltf_extensions_VRM_firstPerson_lookAtHorizontalOuter_curve(kv.Value);
            continue;
        }

        if(key=="xrange"){
            value.xRange = kv.Value.GetSingle();
            continue;
        }

        if(key=="yrange"){
            value.yRange = kv.Value.GetSingle();
            continue;
        }

    }
    return value;
}

public static Single[] Deserialize_gltf_extensions_VRM_firstPerson_lookAtHorizontalOuter_curve(JsonTreeNode parsed)
{
    var value = new Single[parsed.GetArrayCount()];
    int i=0;
    foreach(var x in parsed.ArrayItems())
    {
        value[i++] = x.GetSingle();
    }
	return value;
} 

public static VrmDegreeMap Deserialize_gltf_extensions_VRM_firstPerson_lookAtVerticalDown(JsonTreeNode parsed)
{
    var value = new VrmDegreeMap();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="curve"){
            value.curve = Deserialize_gltf_extensions_VRM_firstPerson_lookAtVerticalDown_curve(kv.Value);
            continue;
        }

        if(key=="xrange"){
            value.xRange = kv.Value.GetSingle();
            continue;
        }

        if(key=="yrange"){
            value.yRange = kv.Value.GetSingle();
            continue;
        }

    }
    return value;
}

public static Single[] Deserialize_gltf_extensions_VRM_firstPerson_lookAtVerticalDown_curve(JsonTreeNode parsed)
{
    var value = new Single[parsed.GetArrayCount()];
    int i=0;
    foreach(var x in parsed.ArrayItems())
    {
        value[i++] = x.GetSingle();
    }
	return value;
} 

public static VrmDegreeMap Deserialize_gltf_extensions_VRM_firstPerson_lookAtVerticalUp(JsonTreeNode parsed)
{
    var value = new VrmDegreeMap();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="curve"){
            value.curve = Deserialize_gltf_extensions_VRM_firstPerson_lookAtVerticalUp_curve(kv.Value);
            continue;
        }

        if(key=="xrange"){
            value.xRange = kv.Value.GetSingle();
            continue;
        }

        if(key=="yrange"){
            value.yRange = kv.Value.GetSingle();
            continue;
        }

    }
    return value;
}

public static Single[] Deserialize_gltf_extensions_VRM_firstPerson_lookAtVerticalUp_curve(JsonTreeNode parsed)
{
    var value = new Single[parsed.GetArrayCount()];
    int i=0;
    foreach(var x in parsed.ArrayItems())
    {
        value[i++] = x.GetSingle();
    }
	return value;
} 

public static VrmBlendShapeMaster Deserialize_gltf_extensions_VRM_blendShapeMaster(JsonTreeNode parsed)
{
    var value = new VrmBlendShapeMaster();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="blendshapegroups"){
            value.blendShapeGroups = Deserialize_gltf_extensions_VRM_blendShapeMaster_blendShapeGroups(kv.Value);
            continue;
        }

    }
    return value;
}

public static List<GltfFormat.VrmBlendShapeGroup> Deserialize_gltf_extensions_VRM_blendShapeMaster_blendShapeGroups(JsonTreeNode parsed)
{
    var value = new List<VrmBlendShapeGroup>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_extensions_VRM_blendShapeMaster_blendShapeGroups_LIST(x));
    }
	return value;
}
public static VrmBlendShapeGroup Deserialize_gltf_extensions_VRM_blendShapeMaster_blendShapeGroups_LIST(JsonTreeNode parsed)
{
    var value = new VrmBlendShapeGroup();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="name"){
            value.name = kv.Value.GetString();
            continue;
        }

        if(key=="presetname"){
            value.presetName = (VrmBlendShapePreset)Enum.Parse(typeof(VrmBlendShapePreset), kv.Value.GetString(), true);
            continue;
        }

        if(key=="binds"){
            value.binds = Deserialize_gltf_extensions_VRM_blendShapeMaster_blendShapeGroups__binds(kv.Value);
            continue;
        }

        if(key=="materialvalues"){
            value.materialValues = Deserialize_gltf_extensions_VRM_blendShapeMaster_blendShapeGroups__materialValues(kv.Value);
            continue;
        }

        if(key=="isbinary"){
            value.isBinary = kv.Value.GetBoolean();
            continue;
        }

    }
    return value;
}

public static List<GltfFormat.VrmBlendShapeBind> Deserialize_gltf_extensions_VRM_blendShapeMaster_blendShapeGroups__binds(JsonTreeNode parsed)
{
    var value = new List<VrmBlendShapeBind>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_extensions_VRM_blendShapeMaster_blendShapeGroups__binds_LIST(x));
    }
	return value;
}
public static VrmBlendShapeBind Deserialize_gltf_extensions_VRM_blendShapeMaster_blendShapeGroups__binds_LIST(JsonTreeNode parsed)
{
    var value = new VrmBlendShapeBind();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="mesh"){
            value.mesh = kv.Value.GetInt32();
            continue;
        }

        if(key=="index"){
            value.index = kv.Value.GetInt32();
            continue;
        }

        if(key=="weight"){
            value.weight = kv.Value.GetSingle();
            continue;
        }

    }
    return value;
}

public static List<GltfFormat.VrmMaterialValueBind> Deserialize_gltf_extensions_VRM_blendShapeMaster_blendShapeGroups__materialValues(JsonTreeNode parsed)
{
    var value = new List<VrmMaterialValueBind>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_extensions_VRM_blendShapeMaster_blendShapeGroups__materialValues_LIST(x));
    }
	return value;
}
public static VrmMaterialValueBind Deserialize_gltf_extensions_VRM_blendShapeMaster_blendShapeGroups__materialValues_LIST(JsonTreeNode parsed)
{
    var value = new VrmMaterialValueBind();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="materialname"){
            value.materialName = kv.Value.GetString();
            continue;
        }

        if(key=="propertyname"){
            value.propertyName = kv.Value.GetString();
            continue;
        }

        if(key=="targetvalue"){
            value.targetValue = Deserialize_gltf_extensions_VRM_blendShapeMaster_blendShapeGroups__materialValues__targetValue(kv.Value);
            continue;
        }

    }
    return value;
}

public static Single[] Deserialize_gltf_extensions_VRM_blendShapeMaster_blendShapeGroups__materialValues__targetValue(JsonTreeNode parsed)
{
    var value = new Single[parsed.GetArrayCount()];
    int i=0;
    foreach(var x in parsed.ArrayItems())
    {
        value[i++] = x.GetSingle();
    }
	return value;
} 

public static VrmSecondaryAnimation Deserialize_gltf_extensions_VRM_secondaryAnimation(JsonTreeNode parsed)
{
    var value = new VrmSecondaryAnimation();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="bonegroups"){
            value.boneGroups = Deserialize_gltf_extensions_VRM_secondaryAnimation_boneGroups(kv.Value);
            continue;
        }

        if(key=="collidergroups"){
            value.colliderGroups = Deserialize_gltf_extensions_VRM_secondaryAnimation_colliderGroups(kv.Value);
            continue;
        }

    }
    return value;
}

public static List<GltfFormat.VrmSecondaryAnimationGroup> Deserialize_gltf_extensions_VRM_secondaryAnimation_boneGroups(JsonTreeNode parsed)
{
    var value = new List<VrmSecondaryAnimationGroup>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_extensions_VRM_secondaryAnimation_boneGroups_LIST(x));
    }
	return value;
}
public static VrmSecondaryAnimationGroup Deserialize_gltf_extensions_VRM_secondaryAnimation_boneGroups_LIST(JsonTreeNode parsed)
{
    var value = new VrmSecondaryAnimationGroup();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="comment"){
            value.comment = kv.Value.GetString();
            continue;
        }

        if(key=="stiffiness"){
            value.stiffiness = kv.Value.GetSingle();
            continue;
        }

        if(key=="gravitypower"){
            value.gravityPower = kv.Value.GetSingle();
            continue;
        }

        if(key=="gravitydir"){
            value.gravityDir = Deserialize_gltf_extensions_VRM_secondaryAnimation_boneGroups__gravityDir(kv.Value);
            continue;
        }

        if(key=="dragforce"){
            value.dragForce = kv.Value.GetSingle();
            continue;
        }

        if(key=="center"){
            value.center = kv.Value.GetInt32();
            continue;
        }

        if(key=="hitradius"){
            value.hitRadius = kv.Value.GetSingle();
            continue;
        }

        if(key=="bones"){
            value.bones = Deserialize_gltf_extensions_VRM_secondaryAnimation_boneGroups__bones(kv.Value);
            continue;
        }

        if(key=="collidergroups"){
            value.colliderGroups = Deserialize_gltf_extensions_VRM_secondaryAnimation_boneGroups__colliderGroups(kv.Value);
            continue;
        }

    }
    return value;
}

public static Vector3 Deserialize_gltf_extensions_VRM_secondaryAnimation_boneGroups__gravityDir(JsonTreeNode parsed)
{
    var value = new Vector3();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="x"){
            value.X = kv.Value.GetSingle();
            continue;
        }

        if(key=="y"){
            value.Y = kv.Value.GetSingle();
            continue;
        }

        if(key=="z"){
            value.Z = kv.Value.GetSingle();
            continue;
        }

    }
    return value;
}

public static Int32[] Deserialize_gltf_extensions_VRM_secondaryAnimation_boneGroups__bones(JsonTreeNode parsed)
{
    var value = new Int32[parsed.GetArrayCount()];
    int i=0;
    foreach(var x in parsed.ArrayItems())
    {
        value[i++] = x.GetInt32();
    }
	return value;
} 

public static Int32[] Deserialize_gltf_extensions_VRM_secondaryAnimation_boneGroups__colliderGroups(JsonTreeNode parsed)
{
    var value = new Int32[parsed.GetArrayCount()];
    int i=0;
    foreach(var x in parsed.ArrayItems())
    {
        value[i++] = x.GetInt32();
    }
	return value;
} 

public static List<GltfFormat.VrmSecondaryAnimationColliderGroup> Deserialize_gltf_extensions_VRM_secondaryAnimation_colliderGroups(JsonTreeNode parsed)
{
    var value = new List<VrmSecondaryAnimationColliderGroup>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_extensions_VRM_secondaryAnimation_colliderGroups_LIST(x));
    }
	return value;
}
public static VrmSecondaryAnimationColliderGroup Deserialize_gltf_extensions_VRM_secondaryAnimation_colliderGroups_LIST(JsonTreeNode parsed)
{
    var value = new VrmSecondaryAnimationColliderGroup();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="node"){
            value.node = kv.Value.GetInt32();
            continue;
        }

        if(key=="colliders"){
            value.colliders = Deserialize_gltf_extensions_VRM_secondaryAnimation_colliderGroups__colliders(kv.Value);
            continue;
        }

    }
    return value;
}

public static List<GltfFormat.VrmSecondaryAnimationCollider> Deserialize_gltf_extensions_VRM_secondaryAnimation_colliderGroups__colliders(JsonTreeNode parsed)
{
    var value = new List<VrmSecondaryAnimationCollider>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_extensions_VRM_secondaryAnimation_colliderGroups__colliders_LIST(x));
    }
	return value;
}
public static VrmSecondaryAnimationCollider Deserialize_gltf_extensions_VRM_secondaryAnimation_colliderGroups__colliders_LIST(JsonTreeNode parsed)
{
    var value = new VrmSecondaryAnimationCollider();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="offset"){
            value.offset = Deserialize_gltf_extensions_VRM_secondaryAnimation_colliderGroups__colliders__offset(kv.Value);
            continue;
        }

        if(key=="radius"){
            value.radius = kv.Value.GetSingle();
            continue;
        }

    }
    return value;
}

public static Vector3 Deserialize_gltf_extensions_VRM_secondaryAnimation_colliderGroups__colliders__offset(JsonTreeNode parsed)
{
    var value = new Vector3();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="x"){
            value.X = kv.Value.GetSingle();
            continue;
        }

        if(key=="y"){
            value.Y = kv.Value.GetSingle();
            continue;
        }

        if(key=="z"){
            value.Z = kv.Value.GetSingle();
            continue;
        }

    }
    return value;
}

public static List<GltfFormat.VrmMaterial> Deserialize_gltf_extensions_VRM_materialProperties(JsonTreeNode parsed)
{
    var value = new List<VrmMaterial>();
    foreach(var x in parsed.ArrayItems())
    {
        value.Add(Deserialize_gltf_extensions_VRM_materialProperties_LIST(x));
    }
	return value;
}
public static VrmMaterial Deserialize_gltf_extensions_VRM_materialProperties_LIST(JsonTreeNode parsed)
{
    var value = new VrmMaterial();

    foreach(var kv in parsed.ObjectItems())
    {
        var key = kv.Key.GetString().ToLower();

        if(key=="name"){
            value.name = kv.Value.GetString();
            continue;
        }

        if(key=="shader"){
            value.shader = kv.Value.GetString();
            continue;
        }

        if(key=="renderqueue"){
            value.renderQueue = kv.Value.GetInt32();
            continue;
        }

        if(key=="floatproperties"){
            value.floatProperties = Deserialize_gltf_extensions_VRM_materialProperties__floatProperties(kv.Value);
            continue;
        }

        if(key=="vectorproperties"){
            value.vectorProperties = Deserialize_gltf_extensions_VRM_materialProperties__vectorProperties(kv.Value);
            continue;
        }

        if(key=="textureproperties"){
            value.textureProperties = Deserialize_gltf_extensions_VRM_materialProperties__textureProperties(kv.Value);
            continue;
        }

        if(key=="keywordmap"){
            value.keywordMap = Deserialize_gltf_extensions_VRM_materialProperties__keywordMap(kv.Value);
            continue;
        }

        if(key=="tagmap"){
            value.tagMap = Deserialize_gltf_extensions_VRM_materialProperties__tagMap(kv.Value);
            continue;
        }

    }
    return value;
}

 
public static Dictionary<String, Single> Deserialize_gltf_extensions_VRM_materialProperties__floatProperties(JsonTreeNode parsed)
{
    var value = new Dictionary<string, Single>();
    foreach(var kv in parsed.ObjectItems())
    {
        value.Add(kv.Key.GetString(), kv.Value.GetSingle());
    }
	return value;
}

 
public static Dictionary<String, Single[]> Deserialize_gltf_extensions_VRM_materialProperties__vectorProperties(JsonTreeNode parsed)
{
    var value = new Dictionary<string, Single[]>();
    foreach(var kv in parsed.ObjectItems())
    {
        value.Add(kv.Key.GetString(), Deserialize_gltf_extensions_VRM_materialProperties__vectorProperties_DICT(kv.Value));
    }
	return value;
}

public static Single[] Deserialize_gltf_extensions_VRM_materialProperties__vectorProperties_DICT(JsonTreeNode parsed)
{
    var value = new Single[parsed.GetArrayCount()];
    int i=0;
    foreach(var x in parsed.ArrayItems())
    {
        value[i++] = x.GetSingle();
    }
	return value;
} 

 
public static Dictionary<String, Int32> Deserialize_gltf_extensions_VRM_materialProperties__textureProperties(JsonTreeNode parsed)
{
    var value = new Dictionary<string, Int32>();
    foreach(var kv in parsed.ObjectItems())
    {
        value.Add(kv.Key.GetString(), kv.Value.GetInt32());
    }
	return value;
}

 
public static Dictionary<String, Boolean> Deserialize_gltf_extensions_VRM_materialProperties__keywordMap(JsonTreeNode parsed)
{
    var value = new Dictionary<string, Boolean>();
    foreach(var kv in parsed.ObjectItems())
    {
        value.Add(kv.Key.GetString(), kv.Value.GetBoolean());
    }
	return value;
}

 
public static Dictionary<String, String> Deserialize_gltf_extensions_VRM_materialProperties__tagMap(JsonTreeNode parsed)
{
    var value = new Dictionary<string, String>();
    foreach(var kv in parsed.ObjectItems())
    {
        value.Add(kv.Key.GetString(), kv.Value.GetString());
    }
	return value;
}

} // GltfDeserializer
} // ObjectNotation 
