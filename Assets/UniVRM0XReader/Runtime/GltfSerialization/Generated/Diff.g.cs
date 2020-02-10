
using System;
using System.Collections.Generic;
using System.Numerics;
using GltfFormat;
using System.Linq;

namespace GltfSerialization.Generated {

public static class GltfDiff
{

    public static List<KeyValuePair<string, string>> Diff(Gltf lhs, Gltf rhs)
    {
        var list = new List<KeyValuePair<string, string>>();
        __ROOT__(new DiffContext{List = list}, lhs, rhs);
        return list;
    }


static void __ROOT__(DiffContext diff, Gltf lhs, Gltf rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

            asset(diff.GetChild("asset"), lhs.asset, rhs.asset);

            buffers(diff.GetChild("buffers"), lhs.buffers, rhs.buffers);

            bufferViews(diff.GetChild("bufferViews"), lhs.bufferViews, rhs.bufferViews);

            accessors(diff.GetChild("accessors"), lhs.accessors, rhs.accessors);

            textures(diff.GetChild("textures"), lhs.textures, rhs.textures);

            samplers(diff.GetChild("samplers"), lhs.samplers, rhs.samplers);

            images(diff.GetChild("images"), lhs.images, rhs.images);

            materials(diff.GetChild("materials"), lhs.materials, rhs.materials);

            meshes(diff.GetChild("meshes"), lhs.meshes, rhs.meshes);

            nodes(diff.GetChild("nodes"), lhs.nodes, rhs.nodes);

            skins(diff.GetChild("skins"), lhs.skins, rhs.skins);

    if(lhs.scene != rhs.scene)
    {
        diff.Add("scene", $"{lhs.scene} != {rhs.scene}");
    }

            scenes(diff.GetChild("scenes"), lhs.scenes, rhs.scenes);

            animations(diff.GetChild("animations"), lhs.animations, rhs.animations);

            cameras(diff.GetChild("cameras"), lhs.cameras, rhs.cameras);

            extensionsUsed(diff.GetChild("extensionsUsed"), lhs.extensionsUsed, rhs.extensionsUsed);

            extensionsRequired(diff.GetChild("extensionsRequired"), lhs.extensionsRequired, rhs.extensionsRequired);

            extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

}

static void asset(DiffContext diff, GltfAsset lhs, GltfAsset rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.generator != rhs.generator)
    {
        diff.Add("generator", $"{lhs.generator} != {rhs.generator}");
    }

    if(lhs.version != rhs.version)
    {
        diff.Add("version", $"{lhs.version} != {rhs.version}");
    }

    if(lhs.copyright != rhs.copyright)
    {
        diff.Add("copyright", $"{lhs.copyright} != {rhs.copyright}");
    }

    if(lhs.minVersion != rhs.minVersion)
    {
        diff.Add("minVersion", $"{lhs.minVersion} != {rhs.minVersion}");
    }

            asset_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            asset_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

}

static void asset_extensions(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void asset_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void buffers(DiffContext diff, List<GltfBuffer> lhs, List<GltfBuffer> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                buffers_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void buffers_L(DiffContext diff, GltfBuffer lhs, GltfBuffer rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.uri != rhs.uri)
    {
        diff.Add("uri", $"{lhs.uri} != {rhs.uri}");
    }

    if(lhs.byteLength != rhs.byteLength)
    {
        diff.Add("byteLength", $"{lhs.byteLength} != {rhs.byteLength}");
    }

            buffers_L_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            buffers_L_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

    if(lhs.name != rhs.name)
    {
        diff.Add("name", $"{lhs.name} != {rhs.name}");
    }

}

static void buffers_L_extensions(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void buffers_L_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void bufferViews(DiffContext diff, List<GltfBufferView> lhs, List<GltfBufferView> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                bufferViews_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void bufferViews_L(DiffContext diff, GltfBufferView lhs, GltfBufferView rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.buffer != rhs.buffer)
    {
        diff.Add("buffer", $"{lhs.buffer} != {rhs.buffer}");
    }

    if(lhs.byteOffset != rhs.byteOffset)
    {
        diff.Add("byteOffset", $"{lhs.byteOffset} != {rhs.byteOffset}");
    }

    if(lhs.byteLength != rhs.byteLength)
    {
        diff.Add("byteLength", $"{lhs.byteLength} != {rhs.byteLength}");
    }

    if(lhs.byteStride != rhs.byteStride)
    {
        diff.Add("byteStride", $"{lhs.byteStride} != {rhs.byteStride}");
    }

    if(lhs.target != rhs.target)
    {
        diff.Add("target", $"{lhs.target} != {rhs.target}");
    }

            bufferViews_L_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            bufferViews_L_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

    if(lhs.name != rhs.name)
    {
        diff.Add("name", $"{lhs.name} != {rhs.name}");
    }

}

static void bufferViews_L_extensions(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void bufferViews_L_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void accessors(DiffContext diff, List<GltfAccessor> lhs, List<GltfAccessor> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                accessors_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void accessors_L(DiffContext diff, GltfAccessor lhs, GltfAccessor rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.bufferView != rhs.bufferView)
    {
        diff.Add("bufferView", $"{lhs.bufferView} != {rhs.bufferView}");
    }

    if(lhs.byteOffset != rhs.byteOffset)
    {
        diff.Add("byteOffset", $"{lhs.byteOffset} != {rhs.byteOffset}");
    }

    if(lhs.type != rhs.type)
    {
        diff.Add("type", $"{lhs.type} != {rhs.type}");
    }

    if(lhs.componentType != rhs.componentType)
    {
        diff.Add("componentType", $"{lhs.componentType} != {rhs.componentType}");
    }

    if(lhs.count != rhs.count)
    {
        diff.Add("count", $"{lhs.count} != {rhs.count}");
    }

            accessors_L_max(diff.GetChild("max"), lhs.max, rhs.max);

            accessors_L_min(diff.GetChild("min"), lhs.min, rhs.min);

    if(lhs.normalized != rhs.normalized)
    {
        diff.Add("normalized", $"{lhs.normalized} != {rhs.normalized}");
    }

            accessors_L_sparse(diff.GetChild("sparse"), lhs.sparse, rhs.sparse);

    if(lhs.name != rhs.name)
    {
        diff.Add("name", $"{lhs.name} != {rhs.name}");
    }

            accessors_L_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            accessors_L_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

}

static void accessors_L_max(DiffContext diff, Single[] lhs, Single[] rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
        }
        else{
            diff.Add("lhs is null");
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
        }
        else{
            if(lhs.Length!=rhs.Length)
            {
                diff.Add("rhs.Length != rhs.Length");
            }
            else{
                int i=0;
                foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
                {
                    accessors_L_max_L(diff.GetChild(i++), l, r);
                }
            }
        }
    }
}

static void accessors_L_max_L(DiffContext diff, Single lhs, Single rhs)
{
}

static void accessors_L_min(DiffContext diff, Single[] lhs, Single[] rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
        }
        else{
            diff.Add("lhs is null");
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
        }
        else{
            if(lhs.Length!=rhs.Length)
            {
                diff.Add("rhs.Length != rhs.Length");
            }
            else{
                int i=0;
                foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
                {
                    accessors_L_min_L(diff.GetChild(i++), l, r);
                }
            }
        }
    }
}

static void accessors_L_min_L(DiffContext diff, Single lhs, Single rhs)
{
}

static void accessors_L_sparse(DiffContext diff, GltfSparse lhs, GltfSparse rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.count != rhs.count)
    {
        diff.Add("count", $"{lhs.count} != {rhs.count}");
    }

            accessors_L_sparse_indices(diff.GetChild("indices"), lhs.indices, rhs.indices);

            accessors_L_sparse_values(diff.GetChild("values"), lhs.values, rhs.values);

            accessors_L_sparse_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            accessors_L_sparse_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

}

static void accessors_L_sparse_indices(DiffContext diff, GltfSparseIndices lhs, GltfSparseIndices rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.bufferView != rhs.bufferView)
    {
        diff.Add("bufferView", $"{lhs.bufferView} != {rhs.bufferView}");
    }

    if(lhs.byteOffset != rhs.byteOffset)
    {
        diff.Add("byteOffset", $"{lhs.byteOffset} != {rhs.byteOffset}");
    }

    if(lhs.componentType != rhs.componentType)
    {
        diff.Add("componentType", $"{lhs.componentType} != {rhs.componentType}");
    }

            accessors_L_sparse_indices_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            accessors_L_sparse_indices_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

}

static void accessors_L_sparse_indices_extensions(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void accessors_L_sparse_indices_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void accessors_L_sparse_values(DiffContext diff, GltfSparseValues lhs, GltfSparseValues rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.bufferView != rhs.bufferView)
    {
        diff.Add("bufferView", $"{lhs.bufferView} != {rhs.bufferView}");
    }

    if(lhs.byteOffset != rhs.byteOffset)
    {
        diff.Add("byteOffset", $"{lhs.byteOffset} != {rhs.byteOffset}");
    }

            accessors_L_sparse_values_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            accessors_L_sparse_values_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

}

static void accessors_L_sparse_values_extensions(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void accessors_L_sparse_values_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void accessors_L_sparse_extensions(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void accessors_L_sparse_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void accessors_L_extensions(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void accessors_L_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void textures(DiffContext diff, List<GltfTexture> lhs, List<GltfTexture> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                textures_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void textures_L(DiffContext diff, GltfTexture lhs, GltfTexture rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.sampler != rhs.sampler)
    {
        diff.Add("sampler", $"{lhs.sampler} != {rhs.sampler}");
    }

    if(lhs.source != rhs.source)
    {
        diff.Add("source", $"{lhs.source} != {rhs.source}");
    }

            textures_L_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            textures_L_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

    if(lhs.name != rhs.name)
    {
        diff.Add("name", $"{lhs.name} != {rhs.name}");
    }

}

static void textures_L_extensions(DiffContext diff, GltfTextureExtensions lhs, GltfTextureExtensions rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

            textures_L_extensions_EXT_texture_webp(diff.GetChild("EXT_texture_webp"), lhs.EXT_texture_webp, rhs.EXT_texture_webp);

            textures_L_extensions_MSFT_texture_dds(diff.GetChild("MSFT_texture_dds"), lhs.MSFT_texture_dds, rhs.MSFT_texture_dds);

}

static void textures_L_extensions_EXT_texture_webp(DiffContext diff, EXT_texture_webp lhs, EXT_texture_webp rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.source != rhs.source)
    {
        diff.Add("source", $"{lhs.source} != {rhs.source}");
    }

}

static void textures_L_extensions_MSFT_texture_dds(DiffContext diff, MSFT_texture_dds lhs, MSFT_texture_dds rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.source != rhs.source)
    {
        diff.Add("source", $"{lhs.source} != {rhs.source}");
    }

}

static void textures_L_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void samplers(DiffContext diff, List<GltfTextureSampler> lhs, List<GltfTextureSampler> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                samplers_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void samplers_L(DiffContext diff, GltfTextureSampler lhs, GltfTextureSampler rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.magFilter != rhs.magFilter)
    {
        diff.Add("magFilter", $"{lhs.magFilter} != {rhs.magFilter}");
    }

    if(lhs.minFilter != rhs.minFilter)
    {
        diff.Add("minFilter", $"{lhs.minFilter} != {rhs.minFilter}");
    }

    if(lhs.wrapS != rhs.wrapS)
    {
        diff.Add("wrapS", $"{lhs.wrapS} != {rhs.wrapS}");
    }

    if(lhs.wrapT != rhs.wrapT)
    {
        diff.Add("wrapT", $"{lhs.wrapT} != {rhs.wrapT}");
    }

            samplers_L_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            samplers_L_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

    if(lhs.name != rhs.name)
    {
        diff.Add("name", $"{lhs.name} != {rhs.name}");
    }

}

static void samplers_L_extensions(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void samplers_L_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void images(DiffContext diff, List<GltfImage> lhs, List<GltfImage> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                images_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void images_L(DiffContext diff, GltfImage lhs, GltfImage rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.name != rhs.name)
    {
        diff.Add("name", $"{lhs.name} != {rhs.name}");
    }

    if(lhs.uri != rhs.uri)
    {
        diff.Add("uri", $"{lhs.uri} != {rhs.uri}");
    }

    if(lhs.bufferView != rhs.bufferView)
    {
        diff.Add("bufferView", $"{lhs.bufferView} != {rhs.bufferView}");
    }

    if(lhs.mimeType != rhs.mimeType)
    {
        diff.Add("mimeType", $"{lhs.mimeType} != {rhs.mimeType}");
    }

            images_L_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            images_L_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

}

static void images_L_extensions(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void images_L_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void materials(DiffContext diff, List<GltfMaterial> lhs, List<GltfMaterial> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                materials_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void materials_L(DiffContext diff, GltfMaterial lhs, GltfMaterial rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.name != rhs.name)
    {
        diff.Add("name", $"{lhs.name} != {rhs.name}");
    }

            materials_L_pbrMetallicRoughness(diff.GetChild("pbrMetallicRoughness"), lhs.pbrMetallicRoughness, rhs.pbrMetallicRoughness);

            materials_L_normalTexture(diff.GetChild("normalTexture"), lhs.normalTexture, rhs.normalTexture);

            materials_L_occlusionTexture(diff.GetChild("occlusionTexture"), lhs.occlusionTexture, rhs.occlusionTexture);

            materials_L_emissiveTexture(diff.GetChild("emissiveTexture"), lhs.emissiveTexture, rhs.emissiveTexture);

            materials_L_emissiveFactor(diff.GetChild("emissiveFactor"), lhs.emissiveFactor, rhs.emissiveFactor);

    if(lhs.alphaMode != rhs.alphaMode)
    {
        diff.Add("alphaMode", $"{lhs.alphaMode} != {rhs.alphaMode}");
    }

    if(lhs.alphaCutoff != rhs.alphaCutoff)
    {
        diff.Add("alphaCutoff", $"{lhs.alphaCutoff} != {rhs.alphaCutoff}");
    }

    if(lhs.doubleSided != rhs.doubleSided)
    {
        diff.Add("doubleSided", $"{lhs.doubleSided} != {rhs.doubleSided}");
    }

            materials_L_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            materials_L_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

}

static void materials_L_pbrMetallicRoughness(DiffContext diff, GltfPbrMetallicRoughness lhs, GltfPbrMetallicRoughness rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

            materials_L_pbrMetallicRoughness_baseColorTexture(diff.GetChild("baseColorTexture"), lhs.baseColorTexture, rhs.baseColorTexture);

            materials_L_pbrMetallicRoughness_baseColorFactor(diff.GetChild("baseColorFactor"), lhs.baseColorFactor, rhs.baseColorFactor);

            materials_L_pbrMetallicRoughness_metallicRoughnessTexture(diff.GetChild("metallicRoughnessTexture"), lhs.metallicRoughnessTexture, rhs.metallicRoughnessTexture);

    if(lhs.metallicFactor != rhs.metallicFactor)
    {
        diff.Add("metallicFactor", $"{lhs.metallicFactor} != {rhs.metallicFactor}");
    }

    if(lhs.roughnessFactor != rhs.roughnessFactor)
    {
        diff.Add("roughnessFactor", $"{lhs.roughnessFactor} != {rhs.roughnessFactor}");
    }

            materials_L_pbrMetallicRoughness_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            materials_L_pbrMetallicRoughness_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

}

static void materials_L_pbrMetallicRoughness_baseColorTexture(DiffContext diff, GltfMaterialBaseColorTextureInfo lhs, GltfMaterialBaseColorTextureInfo rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.index != rhs.index)
    {
        diff.Add("index", $"{lhs.index} != {rhs.index}");
    }

    if(lhs.texCoord != rhs.texCoord)
    {
        diff.Add("texCoord", $"{lhs.texCoord} != {rhs.texCoord}");
    }

            materials_L_pbrMetallicRoughness_baseColorTexture_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            materials_L_pbrMetallicRoughness_baseColorTexture_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

}

static void materials_L_pbrMetallicRoughness_baseColorTexture_extensions(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void materials_L_pbrMetallicRoughness_baseColorTexture_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void materials_L_pbrMetallicRoughness_baseColorFactor(DiffContext diff, Single[] lhs, Single[] rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
        }
        else{
            diff.Add("lhs is null");
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
        }
        else{
            if(lhs.Length!=rhs.Length)
            {
                diff.Add("rhs.Length != rhs.Length");
            }
            else{
                int i=0;
                foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
                {
                    materials_L_pbrMetallicRoughness_baseColorFactor_L(diff.GetChild(i++), l, r);
                }
            }
        }
    }
}

static void materials_L_pbrMetallicRoughness_baseColorFactor_L(DiffContext diff, Single lhs, Single rhs)
{
}

static void materials_L_pbrMetallicRoughness_metallicRoughnessTexture(DiffContext diff, GltfMaterialMetallicRoughnessTextureInfo lhs, GltfMaterialMetallicRoughnessTextureInfo rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.index != rhs.index)
    {
        diff.Add("index", $"{lhs.index} != {rhs.index}");
    }

    if(lhs.texCoord != rhs.texCoord)
    {
        diff.Add("texCoord", $"{lhs.texCoord} != {rhs.texCoord}");
    }

            materials_L_pbrMetallicRoughness_metallicRoughnessTexture_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            materials_L_pbrMetallicRoughness_metallicRoughnessTexture_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

}

static void materials_L_pbrMetallicRoughness_metallicRoughnessTexture_extensions(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void materials_L_pbrMetallicRoughness_metallicRoughnessTexture_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void materials_L_pbrMetallicRoughness_extensions(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void materials_L_pbrMetallicRoughness_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void materials_L_normalTexture(DiffContext diff, GltfMaterialNormalTextureInfo lhs, GltfMaterialNormalTextureInfo rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.scale != rhs.scale)
    {
        diff.Add("scale", $"{lhs.scale} != {rhs.scale}");
    }

    if(lhs.index != rhs.index)
    {
        diff.Add("index", $"{lhs.index} != {rhs.index}");
    }

    if(lhs.texCoord != rhs.texCoord)
    {
        diff.Add("texCoord", $"{lhs.texCoord} != {rhs.texCoord}");
    }

            materials_L_normalTexture_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            materials_L_normalTexture_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

}

static void materials_L_normalTexture_extensions(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void materials_L_normalTexture_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void materials_L_occlusionTexture(DiffContext diff, GltfMaterialOcclusionTextureInfo lhs, GltfMaterialOcclusionTextureInfo rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.strength != rhs.strength)
    {
        diff.Add("strength", $"{lhs.strength} != {rhs.strength}");
    }

    if(lhs.index != rhs.index)
    {
        diff.Add("index", $"{lhs.index} != {rhs.index}");
    }

    if(lhs.texCoord != rhs.texCoord)
    {
        diff.Add("texCoord", $"{lhs.texCoord} != {rhs.texCoord}");
    }

            materials_L_occlusionTexture_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            materials_L_occlusionTexture_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

}

static void materials_L_occlusionTexture_extensions(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void materials_L_occlusionTexture_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void materials_L_emissiveTexture(DiffContext diff, GltfMaterialEmissiveTextureInfo lhs, GltfMaterialEmissiveTextureInfo rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.index != rhs.index)
    {
        diff.Add("index", $"{lhs.index} != {rhs.index}");
    }

    if(lhs.texCoord != rhs.texCoord)
    {
        diff.Add("texCoord", $"{lhs.texCoord} != {rhs.texCoord}");
    }

            materials_L_emissiveTexture_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            materials_L_emissiveTexture_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

}

static void materials_L_emissiveTexture_extensions(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void materials_L_emissiveTexture_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void materials_L_emissiveFactor(DiffContext diff, Single[] lhs, Single[] rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
        }
        else{
            diff.Add("lhs is null");
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
        }
        else{
            if(lhs.Length!=rhs.Length)
            {
                diff.Add("rhs.Length != rhs.Length");
            }
            else{
                int i=0;
                foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
                {
                    materials_L_emissiveFactor_L(diff.GetChild(i++), l, r);
                }
            }
        }
    }
}

static void materials_L_emissiveFactor_L(DiffContext diff, Single lhs, Single rhs)
{
}

static void materials_L_extensions(DiffContext diff, GltfMaterial_Extensions lhs, GltfMaterial_Extensions rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

            materials_L_extensions_KHR_materials_unlit(diff.GetChild("KHR_materials_unlit"), lhs.KHR_materials_unlit, rhs.KHR_materials_unlit);

}

static void materials_L_extensions_KHR_materials_unlit(DiffContext diff, GltfMaterialExtension_KHR_materials_unlit lhs, GltfMaterialExtension_KHR_materials_unlit rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void materials_L_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void meshes(DiffContext diff, List<GltfMesh> lhs, List<GltfMesh> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                meshes_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void meshes_L(DiffContext diff, GltfMesh lhs, GltfMesh rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.name != rhs.name)
    {
        diff.Add("name", $"{lhs.name} != {rhs.name}");
    }

            meshes_L_primitives(diff.GetChild("primitives"), lhs.primitives, rhs.primitives);

            meshes_L_weights(diff.GetChild("weights"), lhs.weights, rhs.weights);

            meshes_L_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            meshes_L_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

}

static void meshes_L_primitives(DiffContext diff, List<GltfPrimitive> lhs, List<GltfPrimitive> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                meshes_L_primitives_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void meshes_L_primitives_L(DiffContext diff, GltfPrimitive lhs, GltfPrimitive rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.mode != rhs.mode)
    {
        diff.Add("mode", $"{lhs.mode} != {rhs.mode}");
    }

    if(lhs.indices != rhs.indices)
    {
        diff.Add("indices", $"{lhs.indices} != {rhs.indices}");
    }

            meshes_L_primitives_L_attributes(diff.GetChild("attributes"), lhs.attributes, rhs.attributes);

    if(lhs.material != rhs.material)
    {
        diff.Add("material", $"{lhs.material} != {rhs.material}");
    }

            meshes_L_primitives_L_targets(diff.GetChild("targets"), lhs.targets, rhs.targets);

            meshes_L_primitives_L_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

}

static void meshes_L_primitives_L_attributes(DiffContext diff, GltfAttributes lhs, GltfAttributes rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.POSITION != rhs.POSITION)
    {
        diff.Add("POSITION", $"{lhs.POSITION} != {rhs.POSITION}");
    }

    if(lhs.NORMAL != rhs.NORMAL)
    {
        diff.Add("NORMAL", $"{lhs.NORMAL} != {rhs.NORMAL}");
    }

    if(lhs.TANGENT != rhs.TANGENT)
    {
        diff.Add("TANGENT", $"{lhs.TANGENT} != {rhs.TANGENT}");
    }

    if(lhs.TEXCOORD_0 != rhs.TEXCOORD_0)
    {
        diff.Add("TEXCOORD_0", $"{lhs.TEXCOORD_0} != {rhs.TEXCOORD_0}");
    }

    if(lhs.COLOR_0 != rhs.COLOR_0)
    {
        diff.Add("COLOR_0", $"{lhs.COLOR_0} != {rhs.COLOR_0}");
    }

    if(lhs.JOINTS_0 != rhs.JOINTS_0)
    {
        diff.Add("JOINTS_0", $"{lhs.JOINTS_0} != {rhs.JOINTS_0}");
    }

    if(lhs.WEIGHTS_0 != rhs.WEIGHTS_0)
    {
        diff.Add("WEIGHTS_0", $"{lhs.WEIGHTS_0} != {rhs.WEIGHTS_0}");
    }

}

static void meshes_L_primitives_L_targets(DiffContext diff, List<GltfMorphTarget> lhs, List<GltfMorphTarget> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                meshes_L_primitives_L_targets_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void meshes_L_primitives_L_targets_L(DiffContext diff, GltfMorphTarget lhs, GltfMorphTarget rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.POSITION != rhs.POSITION)
    {
        diff.Add("POSITION", $"{lhs.POSITION} != {rhs.POSITION}");
    }

    if(lhs.NORMAL != rhs.NORMAL)
    {
        diff.Add("NORMAL", $"{lhs.NORMAL} != {rhs.NORMAL}");
    }

    if(lhs.TANGENT != rhs.TANGENT)
    {
        diff.Add("TANGENT", $"{lhs.TANGENT} != {rhs.TANGENT}");
    }

}

static void meshes_L_primitives_L_extras(DiffContext diff, GltfPrimitive_Extras lhs, GltfPrimitive_Extras rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

            meshes_L_primitives_L_extras_targetNames(diff.GetChild("targetNames"), lhs.targetNames, rhs.targetNames);

}

static void meshes_L_primitives_L_extras_targetNames(DiffContext diff, List<String> lhs, List<String> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                meshes_L_primitives_L_extras_targetNames_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void meshes_L_primitives_L_extras_targetNames_L(DiffContext diff, String lhs, String rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void meshes_L_weights(DiffContext diff, Single[] lhs, Single[] rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
        }
        else{
            diff.Add("lhs is null");
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
        }
        else{
            if(lhs.Length!=rhs.Length)
            {
                diff.Add("rhs.Length != rhs.Length");
            }
            else{
                int i=0;
                foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
                {
                    meshes_L_weights_L(diff.GetChild(i++), l, r);
                }
            }
        }
    }
}

static void meshes_L_weights_L(DiffContext diff, Single lhs, Single rhs)
{
}

static void meshes_L_extensions(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void meshes_L_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void nodes(DiffContext diff, List<GltfNode> lhs, List<GltfNode> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                nodes_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void nodes_L(DiffContext diff, GltfNode lhs, GltfNode rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.name != rhs.name)
    {
        diff.Add("name", $"{lhs.name} != {rhs.name}");
    }

            nodes_L_children(diff.GetChild("children"), lhs.children, rhs.children);

            nodes_L_matrix(diff.GetChild("matrix"), lhs.matrix, rhs.matrix);

            nodes_L_translation(diff.GetChild("translation"), lhs.translation, rhs.translation);

            nodes_L_rotation(diff.GetChild("rotation"), lhs.rotation, rhs.rotation);

            nodes_L_scale(diff.GetChild("scale"), lhs.scale, rhs.scale);

    if(lhs.mesh != rhs.mesh)
    {
        diff.Add("mesh", $"{lhs.mesh} != {rhs.mesh}");
    }

    if(lhs.skin != rhs.skin)
    {
        diff.Add("skin", $"{lhs.skin} != {rhs.skin}");
    }

            nodes_L_weights(diff.GetChild("weights"), lhs.weights, rhs.weights);

    if(lhs.camera != rhs.camera)
    {
        diff.Add("camera", $"{lhs.camera} != {rhs.camera}");
    }

}

static void nodes_L_children(DiffContext diff, Int32[] lhs, Int32[] rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
        }
        else{
            diff.Add("lhs is null");
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
        }
        else{
            if(lhs.Length!=rhs.Length)
            {
                diff.Add("rhs.Length != rhs.Length");
            }
            else{
                int i=0;
                foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
                {
                    nodes_L_children_L(diff.GetChild(i++), l, r);
                }
            }
        }
    }
}

static void nodes_L_children_L(DiffContext diff, Int32 lhs, Int32 rhs)
{
}

static void nodes_L_matrix(DiffContext diff, Single[] lhs, Single[] rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
        }
        else{
            diff.Add("lhs is null");
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
        }
        else{
            if(lhs.Length!=rhs.Length)
            {
                diff.Add("rhs.Length != rhs.Length");
            }
            else{
                int i=0;
                foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
                {
                    nodes_L_matrix_L(diff.GetChild(i++), l, r);
                }
            }
        }
    }
}

static void nodes_L_matrix_L(DiffContext diff, Single lhs, Single rhs)
{
}

static void nodes_L_translation(DiffContext diff, Single[] lhs, Single[] rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
        }
        else{
            diff.Add("lhs is null");
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
        }
        else{
            if(lhs.Length!=rhs.Length)
            {
                diff.Add("rhs.Length != rhs.Length");
            }
            else{
                int i=0;
                foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
                {
                    nodes_L_translation_L(diff.GetChild(i++), l, r);
                }
            }
        }
    }
}

static void nodes_L_translation_L(DiffContext diff, Single lhs, Single rhs)
{
}

static void nodes_L_rotation(DiffContext diff, Single[] lhs, Single[] rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
        }
        else{
            diff.Add("lhs is null");
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
        }
        else{
            if(lhs.Length!=rhs.Length)
            {
                diff.Add("rhs.Length != rhs.Length");
            }
            else{
                int i=0;
                foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
                {
                    nodes_L_rotation_L(diff.GetChild(i++), l, r);
                }
            }
        }
    }
}

static void nodes_L_rotation_L(DiffContext diff, Single lhs, Single rhs)
{
}

static void nodes_L_scale(DiffContext diff, Single[] lhs, Single[] rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
        }
        else{
            diff.Add("lhs is null");
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
        }
        else{
            if(lhs.Length!=rhs.Length)
            {
                diff.Add("rhs.Length != rhs.Length");
            }
            else{
                int i=0;
                foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
                {
                    nodes_L_scale_L(diff.GetChild(i++), l, r);
                }
            }
        }
    }
}

static void nodes_L_scale_L(DiffContext diff, Single lhs, Single rhs)
{
}

static void nodes_L_weights(DiffContext diff, Single[] lhs, Single[] rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
        }
        else{
            diff.Add("lhs is null");
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
        }
        else{
            if(lhs.Length!=rhs.Length)
            {
                diff.Add("rhs.Length != rhs.Length");
            }
            else{
                int i=0;
                foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
                {
                    nodes_L_weights_L(diff.GetChild(i++), l, r);
                }
            }
        }
    }
}

static void nodes_L_weights_L(DiffContext diff, Single lhs, Single rhs)
{
}

static void skins(DiffContext diff, List<GltfSkin> lhs, List<GltfSkin> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                skins_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void skins_L(DiffContext diff, GltfSkin lhs, GltfSkin rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.inverseBindMatrices != rhs.inverseBindMatrices)
    {
        diff.Add("inverseBindMatrices", $"{lhs.inverseBindMatrices} != {rhs.inverseBindMatrices}");
    }

            skins_L_joints(diff.GetChild("joints"), lhs.joints, rhs.joints);

    if(lhs.skeleton != rhs.skeleton)
    {
        diff.Add("skeleton", $"{lhs.skeleton} != {rhs.skeleton}");
    }

            skins_L_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            skins_L_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

    if(lhs.name != rhs.name)
    {
        diff.Add("name", $"{lhs.name} != {rhs.name}");
    }

}

static void skins_L_joints(DiffContext diff, Int32[] lhs, Int32[] rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
        }
        else{
            diff.Add("lhs is null");
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
        }
        else{
            if(lhs.Length!=rhs.Length)
            {
                diff.Add("rhs.Length != rhs.Length");
            }
            else{
                int i=0;
                foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
                {
                    skins_L_joints_L(diff.GetChild(i++), l, r);
                }
            }
        }
    }
}

static void skins_L_joints_L(DiffContext diff, Int32 lhs, Int32 rhs)
{
}

static void skins_L_extensions(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void skins_L_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void scenes(DiffContext diff, List<GltfScene> lhs, List<GltfScene> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                scenes_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void scenes_L(DiffContext diff, GltfScene lhs, GltfScene rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

            scenes_L_nodes(diff.GetChild("nodes"), lhs.nodes, rhs.nodes);

            scenes_L_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            scenes_L_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

    if(lhs.name != rhs.name)
    {
        diff.Add("name", $"{lhs.name} != {rhs.name}");
    }

}

static void scenes_L_nodes(DiffContext diff, Int32[] lhs, Int32[] rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
        }
        else{
            diff.Add("lhs is null");
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
        }
        else{
            if(lhs.Length!=rhs.Length)
            {
                diff.Add("rhs.Length != rhs.Length");
            }
            else{
                int i=0;
                foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
                {
                    scenes_L_nodes_L(diff.GetChild(i++), l, r);
                }
            }
        }
    }
}

static void scenes_L_nodes_L(DiffContext diff, Int32 lhs, Int32 rhs)
{
}

static void scenes_L_extensions(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void scenes_L_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void animations(DiffContext diff, List<GltfAnimation> lhs, List<GltfAnimation> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                animations_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void animations_L(DiffContext diff, GltfAnimation lhs, GltfAnimation rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.name != rhs.name)
    {
        diff.Add("name", $"{lhs.name} != {rhs.name}");
    }

            animations_L_channels(diff.GetChild("channels"), lhs.channels, rhs.channels);

            animations_L_samplers(diff.GetChild("samplers"), lhs.samplers, rhs.samplers);

            animations_L_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            animations_L_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

}

static void animations_L_channels(DiffContext diff, List<GltfAnimationChannel> lhs, List<GltfAnimationChannel> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                animations_L_channels_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void animations_L_channels_L(DiffContext diff, GltfAnimationChannel lhs, GltfAnimationChannel rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.sampler != rhs.sampler)
    {
        diff.Add("sampler", $"{lhs.sampler} != {rhs.sampler}");
    }

            animations_L_channels_L_target(diff.GetChild("target"), lhs.target, rhs.target);

            animations_L_channels_L_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            animations_L_channels_L_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

}

static void animations_L_channels_L_target(DiffContext diff, GltfAnimationTarget lhs, GltfAnimationTarget rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.node != rhs.node)
    {
        diff.Add("node", $"{lhs.node} != {rhs.node}");
    }

    if(lhs.path != rhs.path)
    {
        diff.Add("path", $"{lhs.path} != {rhs.path}");
    }

            animations_L_channels_L_target_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            animations_L_channels_L_target_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

}

static void animations_L_channels_L_target_extensions(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void animations_L_channels_L_target_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void animations_L_channels_L_extensions(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void animations_L_channels_L_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void animations_L_samplers(DiffContext diff, List<GltfAnimationSampler> lhs, List<GltfAnimationSampler> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                animations_L_samplers_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void animations_L_samplers_L(DiffContext diff, GltfAnimationSampler lhs, GltfAnimationSampler rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.input != rhs.input)
    {
        diff.Add("input", $"{lhs.input} != {rhs.input}");
    }

    if(lhs.interpolation != rhs.interpolation)
    {
        diff.Add("interpolation", $"{lhs.interpolation} != {rhs.interpolation}");
    }

    if(lhs.output != rhs.output)
    {
        diff.Add("output", $"{lhs.output} != {rhs.output}");
    }

            animations_L_samplers_L_extensions(diff.GetChild("extensions"), lhs.extensions, rhs.extensions);

            animations_L_samplers_L_extras(diff.GetChild("extras"), lhs.extras, rhs.extras);

}

static void animations_L_samplers_L_extensions(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void animations_L_samplers_L_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void animations_L_extensions(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void animations_L_extras(DiffContext diff, Object lhs, Object rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void cameras(DiffContext diff, List<GltfCamera> lhs, List<GltfCamera> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                cameras_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void cameras_L(DiffContext diff, GltfCamera lhs, GltfCamera rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

            cameras_L_orthographic(diff.GetChild("orthographic"), lhs.orthographic, rhs.orthographic);

            cameras_L_perspective(diff.GetChild("perspective"), lhs.perspective, rhs.perspective);

    if(lhs.type != rhs.type)
    {
        diff.Add("type", $"{lhs.type} != {rhs.type}");
    }

    if(lhs.name != rhs.name)
    {
        diff.Add("name", $"{lhs.name} != {rhs.name}");
    }

}

static void cameras_L_orthographic(DiffContext diff, GltfOrthographic lhs, GltfOrthographic rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.xmag != rhs.xmag)
    {
        diff.Add("xmag", $"{lhs.xmag} != {rhs.xmag}");
    }

    if(lhs.ymag != rhs.ymag)
    {
        diff.Add("ymag", $"{lhs.ymag} != {rhs.ymag}");
    }

    if(lhs.zfar != rhs.zfar)
    {
        diff.Add("zfar", $"{lhs.zfar} != {rhs.zfar}");
    }

    if(lhs.znear != rhs.znear)
    {
        diff.Add("znear", $"{lhs.znear} != {rhs.znear}");
    }

}

static void cameras_L_perspective(DiffContext diff, GltfPerspective lhs, GltfPerspective rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.aspectRatio != rhs.aspectRatio)
    {
        diff.Add("aspectRatio", $"{lhs.aspectRatio} != {rhs.aspectRatio}");
    }

    if(lhs.yfov != rhs.yfov)
    {
        diff.Add("yfov", $"{lhs.yfov} != {rhs.yfov}");
    }

    if(lhs.zfar != rhs.zfar)
    {
        diff.Add("zfar", $"{lhs.zfar} != {rhs.zfar}");
    }

    if(lhs.znear != rhs.znear)
    {
        diff.Add("znear", $"{lhs.znear} != {rhs.znear}");
    }

}

static void extensionsUsed(DiffContext diff, List<String> lhs, List<String> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                extensionsUsed_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void extensionsUsed_L(DiffContext diff, String lhs, String rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void extensionsRequired(DiffContext diff, List<String> lhs, List<String> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                extensionsRequired_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void extensionsRequired_L(DiffContext diff, String lhs, String rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

static void extensions(DiffContext diff, GltfExtensions lhs, GltfExtensions rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

            extensions_VRM(diff.GetChild("VRM"), lhs.VRM, rhs.VRM);

}

static void extensions_VRM(DiffContext diff, GltfExtensionVrm lhs, GltfExtensionVrm rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.exporterVersion != rhs.exporterVersion)
    {
        diff.Add("exporterVersion", $"{lhs.exporterVersion} != {rhs.exporterVersion}");
    }

    if(lhs.specVersion != rhs.specVersion)
    {
        diff.Add("specVersion", $"{lhs.specVersion} != {rhs.specVersion}");
    }

            extensions_VRM_meta(diff.GetChild("meta"), lhs.meta, rhs.meta);

            extensions_VRM_humanoid(diff.GetChild("humanoid"), lhs.humanoid, rhs.humanoid);

            extensions_VRM_firstPerson(diff.GetChild("firstPerson"), lhs.firstPerson, rhs.firstPerson);

            extensions_VRM_blendShapeMaster(diff.GetChild("blendShapeMaster"), lhs.blendShapeMaster, rhs.blendShapeMaster);

            extensions_VRM_secondaryAnimation(diff.GetChild("secondaryAnimation"), lhs.secondaryAnimation, rhs.secondaryAnimation);

            extensions_VRM_materialProperties(diff.GetChild("materialProperties"), lhs.materialProperties, rhs.materialProperties);

}

static void extensions_VRM_meta(DiffContext diff, VrmMeta lhs, VrmMeta rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.title != rhs.title)
    {
        diff.Add("title", $"{lhs.title} != {rhs.title}");
    }

    if(lhs.version != rhs.version)
    {
        diff.Add("version", $"{lhs.version} != {rhs.version}");
    }

    if(lhs.author != rhs.author)
    {
        diff.Add("author", $"{lhs.author} != {rhs.author}");
    }

    if(lhs.contactInformation != rhs.contactInformation)
    {
        diff.Add("contactInformation", $"{lhs.contactInformation} != {rhs.contactInformation}");
    }

    if(lhs.reference != rhs.reference)
    {
        diff.Add("reference", $"{lhs.reference} != {rhs.reference}");
    }

    if(lhs.texture != rhs.texture)
    {
        diff.Add("texture", $"{lhs.texture} != {rhs.texture}");
    }

    if(lhs.allowedUserName != rhs.allowedUserName)
    {
        diff.Add("allowedUserName", $"{lhs.allowedUserName} != {rhs.allowedUserName}");
    }

    if(lhs.violentUssageName != rhs.violentUssageName)
    {
        diff.Add("violentUssageName", $"{lhs.violentUssageName} != {rhs.violentUssageName}");
    }

    if(lhs.sexualUssageName != rhs.sexualUssageName)
    {
        diff.Add("sexualUssageName", $"{lhs.sexualUssageName} != {rhs.sexualUssageName}");
    }

    if(lhs.commercialUssageName != rhs.commercialUssageName)
    {
        diff.Add("commercialUssageName", $"{lhs.commercialUssageName} != {rhs.commercialUssageName}");
    }

    if(lhs.otherPermissionUrl != rhs.otherPermissionUrl)
    {
        diff.Add("otherPermissionUrl", $"{lhs.otherPermissionUrl} != {rhs.otherPermissionUrl}");
    }

    if(lhs.licenseName != rhs.licenseName)
    {
        diff.Add("licenseName", $"{lhs.licenseName} != {rhs.licenseName}");
    }

    if(lhs.otherLicenseUrl != rhs.otherLicenseUrl)
    {
        diff.Add("otherLicenseUrl", $"{lhs.otherLicenseUrl} != {rhs.otherLicenseUrl}");
    }

}

static void extensions_VRM_humanoid(DiffContext diff, VrmHumanoid lhs, VrmHumanoid rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

            extensions_VRM_humanoid_humanBones(diff.GetChild("humanBones"), lhs.humanBones, rhs.humanBones);

    if(lhs.armStretch != rhs.armStretch)
    {
        diff.Add("armStretch", $"{lhs.armStretch} != {rhs.armStretch}");
    }

    if(lhs.legStretch != rhs.legStretch)
    {
        diff.Add("legStretch", $"{lhs.legStretch} != {rhs.legStretch}");
    }

    if(lhs.upperArmTwist != rhs.upperArmTwist)
    {
        diff.Add("upperArmTwist", $"{lhs.upperArmTwist} != {rhs.upperArmTwist}");
    }

    if(lhs.lowerArmTwist != rhs.lowerArmTwist)
    {
        diff.Add("lowerArmTwist", $"{lhs.lowerArmTwist} != {rhs.lowerArmTwist}");
    }

    if(lhs.upperLegTwist != rhs.upperLegTwist)
    {
        diff.Add("upperLegTwist", $"{lhs.upperLegTwist} != {rhs.upperLegTwist}");
    }

    if(lhs.lowerLegTwist != rhs.lowerLegTwist)
    {
        diff.Add("lowerLegTwist", $"{lhs.lowerLegTwist} != {rhs.lowerLegTwist}");
    }

    if(lhs.feetSpacing != rhs.feetSpacing)
    {
        diff.Add("feetSpacing", $"{lhs.feetSpacing} != {rhs.feetSpacing}");
    }

    if(lhs.hasTranslationDoF != rhs.hasTranslationDoF)
    {
        diff.Add("hasTranslationDoF", $"{lhs.hasTranslationDoF} != {rhs.hasTranslationDoF}");
    }

}

static void extensions_VRM_humanoid_humanBones(DiffContext diff, List<VrmHumanoidBone> lhs, List<VrmHumanoidBone> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                extensions_VRM_humanoid_humanBones_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void extensions_VRM_humanoid_humanBones_L(DiffContext diff, VrmHumanoidBone lhs, VrmHumanoidBone rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.bone != rhs.bone)
    {
        diff.Add("bone", $"{lhs.bone} != {rhs.bone}");
    }

    if(lhs.node != rhs.node)
    {
        diff.Add("node", $"{lhs.node} != {rhs.node}");
    }

    if(lhs.useDefaultValues != rhs.useDefaultValues)
    {
        diff.Add("useDefaultValues", $"{lhs.useDefaultValues} != {rhs.useDefaultValues}");
    }

    if(lhs.min != rhs.min)
    {
        diff.Add("min", $"{lhs.min} != {rhs.min}");
    }

    if(lhs.max != rhs.max)
    {
        diff.Add("max", $"{lhs.max} != {rhs.max}");
    }

    if(lhs.center != rhs.center)
    {
        diff.Add("center", $"{lhs.center} != {rhs.center}");
    }

    if(lhs.axisLength != rhs.axisLength)
    {
        diff.Add("axisLength", $"{lhs.axisLength} != {rhs.axisLength}");
    }

}

static void extensions_VRM_firstPerson(DiffContext diff, VrmFirstPerson lhs, VrmFirstPerson rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.firstPersonBone != rhs.firstPersonBone)
    {
        diff.Add("firstPersonBone", $"{lhs.firstPersonBone} != {rhs.firstPersonBone}");
    }

    if(lhs.firstPersonBoneOffset != rhs.firstPersonBoneOffset)
    {
        diff.Add("firstPersonBoneOffset", $"{lhs.firstPersonBoneOffset} != {rhs.firstPersonBoneOffset}");
    }

            extensions_VRM_firstPerson_meshAnnotations(diff.GetChild("meshAnnotations"), lhs.meshAnnotations, rhs.meshAnnotations);

    if(lhs.lookAtTypeName != rhs.lookAtTypeName)
    {
        diff.Add("lookAtTypeName", $"{lhs.lookAtTypeName} != {rhs.lookAtTypeName}");
    }

            extensions_VRM_firstPerson_lookAtHorizontalInner(diff.GetChild("lookAtHorizontalInner"), lhs.lookAtHorizontalInner, rhs.lookAtHorizontalInner);

            extensions_VRM_firstPerson_lookAtHorizontalOuter(diff.GetChild("lookAtHorizontalOuter"), lhs.lookAtHorizontalOuter, rhs.lookAtHorizontalOuter);

            extensions_VRM_firstPerson_lookAtVerticalDown(diff.GetChild("lookAtVerticalDown"), lhs.lookAtVerticalDown, rhs.lookAtVerticalDown);

            extensions_VRM_firstPerson_lookAtVerticalUp(diff.GetChild("lookAtVerticalUp"), lhs.lookAtVerticalUp, rhs.lookAtVerticalUp);

}

static void extensions_VRM_firstPerson_meshAnnotations(DiffContext diff, List<VrmMeshAnnotation> lhs, List<VrmMeshAnnotation> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                extensions_VRM_firstPerson_meshAnnotations_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void extensions_VRM_firstPerson_meshAnnotations_L(DiffContext diff, VrmMeshAnnotation lhs, VrmMeshAnnotation rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.mesh != rhs.mesh)
    {
        diff.Add("mesh", $"{lhs.mesh} != {rhs.mesh}");
    }

    if(lhs.firstPersonFlag != rhs.firstPersonFlag)
    {
        diff.Add("firstPersonFlag", $"{lhs.firstPersonFlag} != {rhs.firstPersonFlag}");
    }

}

static void extensions_VRM_firstPerson_lookAtHorizontalInner(DiffContext diff, VrmDegreeMap lhs, VrmDegreeMap rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

            extensions_VRM_firstPerson_lookAtHorizontalInner_curve(diff.GetChild("curve"), lhs.curve, rhs.curve);

    if(lhs.xRange != rhs.xRange)
    {
        diff.Add("xRange", $"{lhs.xRange} != {rhs.xRange}");
    }

    if(lhs.yRange != rhs.yRange)
    {
        diff.Add("yRange", $"{lhs.yRange} != {rhs.yRange}");
    }

}

static void extensions_VRM_firstPerson_lookAtHorizontalInner_curve(DiffContext diff, Single[] lhs, Single[] rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
        }
        else{
            diff.Add("lhs is null");
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
        }
        else{
            if(lhs.Length!=rhs.Length)
            {
                diff.Add("rhs.Length != rhs.Length");
            }
            else{
                int i=0;
                foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
                {
                    extensions_VRM_firstPerson_lookAtHorizontalInner_curve_L(diff.GetChild(i++), l, r);
                }
            }
        }
    }
}

static void extensions_VRM_firstPerson_lookAtHorizontalInner_curve_L(DiffContext diff, Single lhs, Single rhs)
{
}

static void extensions_VRM_firstPerson_lookAtHorizontalOuter(DiffContext diff, VrmDegreeMap lhs, VrmDegreeMap rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

            extensions_VRM_firstPerson_lookAtHorizontalOuter_curve(diff.GetChild("curve"), lhs.curve, rhs.curve);

    if(lhs.xRange != rhs.xRange)
    {
        diff.Add("xRange", $"{lhs.xRange} != {rhs.xRange}");
    }

    if(lhs.yRange != rhs.yRange)
    {
        diff.Add("yRange", $"{lhs.yRange} != {rhs.yRange}");
    }

}

static void extensions_VRM_firstPerson_lookAtHorizontalOuter_curve(DiffContext diff, Single[] lhs, Single[] rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
        }
        else{
            diff.Add("lhs is null");
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
        }
        else{
            if(lhs.Length!=rhs.Length)
            {
                diff.Add("rhs.Length != rhs.Length");
            }
            else{
                int i=0;
                foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
                {
                    extensions_VRM_firstPerson_lookAtHorizontalOuter_curve_L(diff.GetChild(i++), l, r);
                }
            }
        }
    }
}

static void extensions_VRM_firstPerson_lookAtHorizontalOuter_curve_L(DiffContext diff, Single lhs, Single rhs)
{
}

static void extensions_VRM_firstPerson_lookAtVerticalDown(DiffContext diff, VrmDegreeMap lhs, VrmDegreeMap rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

            extensions_VRM_firstPerson_lookAtVerticalDown_curve(diff.GetChild("curve"), lhs.curve, rhs.curve);

    if(lhs.xRange != rhs.xRange)
    {
        diff.Add("xRange", $"{lhs.xRange} != {rhs.xRange}");
    }

    if(lhs.yRange != rhs.yRange)
    {
        diff.Add("yRange", $"{lhs.yRange} != {rhs.yRange}");
    }

}

static void extensions_VRM_firstPerson_lookAtVerticalDown_curve(DiffContext diff, Single[] lhs, Single[] rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
        }
        else{
            diff.Add("lhs is null");
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
        }
        else{
            if(lhs.Length!=rhs.Length)
            {
                diff.Add("rhs.Length != rhs.Length");
            }
            else{
                int i=0;
                foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
                {
                    extensions_VRM_firstPerson_lookAtVerticalDown_curve_L(diff.GetChild(i++), l, r);
                }
            }
        }
    }
}

static void extensions_VRM_firstPerson_lookAtVerticalDown_curve_L(DiffContext diff, Single lhs, Single rhs)
{
}

static void extensions_VRM_firstPerson_lookAtVerticalUp(DiffContext diff, VrmDegreeMap lhs, VrmDegreeMap rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

            extensions_VRM_firstPerson_lookAtVerticalUp_curve(diff.GetChild("curve"), lhs.curve, rhs.curve);

    if(lhs.xRange != rhs.xRange)
    {
        diff.Add("xRange", $"{lhs.xRange} != {rhs.xRange}");
    }

    if(lhs.yRange != rhs.yRange)
    {
        diff.Add("yRange", $"{lhs.yRange} != {rhs.yRange}");
    }

}

static void extensions_VRM_firstPerson_lookAtVerticalUp_curve(DiffContext diff, Single[] lhs, Single[] rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
        }
        else{
            diff.Add("lhs is null");
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
        }
        else{
            if(lhs.Length!=rhs.Length)
            {
                diff.Add("rhs.Length != rhs.Length");
            }
            else{
                int i=0;
                foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
                {
                    extensions_VRM_firstPerson_lookAtVerticalUp_curve_L(diff.GetChild(i++), l, r);
                }
            }
        }
    }
}

static void extensions_VRM_firstPerson_lookAtVerticalUp_curve_L(DiffContext diff, Single lhs, Single rhs)
{
}

static void extensions_VRM_blendShapeMaster(DiffContext diff, VrmBlendShapeMaster lhs, VrmBlendShapeMaster rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

            extensions_VRM_blendShapeMaster_blendShapeGroups(diff.GetChild("blendShapeGroups"), lhs.blendShapeGroups, rhs.blendShapeGroups);

}

static void extensions_VRM_blendShapeMaster_blendShapeGroups(DiffContext diff, List<VrmBlendShapeGroup> lhs, List<VrmBlendShapeGroup> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                extensions_VRM_blendShapeMaster_blendShapeGroups_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void extensions_VRM_blendShapeMaster_blendShapeGroups_L(DiffContext diff, VrmBlendShapeGroup lhs, VrmBlendShapeGroup rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.name != rhs.name)
    {
        diff.Add("name", $"{lhs.name} != {rhs.name}");
    }

    if(lhs.presetName != rhs.presetName)
    {
        diff.Add("presetName", $"{lhs.presetName} != {rhs.presetName}");
    }

            extensions_VRM_blendShapeMaster_blendShapeGroups_L_binds(diff.GetChild("binds"), lhs.binds, rhs.binds);

            extensions_VRM_blendShapeMaster_blendShapeGroups_L_materialValues(diff.GetChild("materialValues"), lhs.materialValues, rhs.materialValues);

    if(lhs.isBinary != rhs.isBinary)
    {
        diff.Add("isBinary", $"{lhs.isBinary} != {rhs.isBinary}");
    }

}

static void extensions_VRM_blendShapeMaster_blendShapeGroups_L_binds(DiffContext diff, List<VrmBlendShapeBind> lhs, List<VrmBlendShapeBind> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                extensions_VRM_blendShapeMaster_blendShapeGroups_L_binds_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void extensions_VRM_blendShapeMaster_blendShapeGroups_L_binds_L(DiffContext diff, VrmBlendShapeBind lhs, VrmBlendShapeBind rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.mesh != rhs.mesh)
    {
        diff.Add("mesh", $"{lhs.mesh} != {rhs.mesh}");
    }

    if(lhs.index != rhs.index)
    {
        diff.Add("index", $"{lhs.index} != {rhs.index}");
    }

    if(lhs.weight != rhs.weight)
    {
        diff.Add("weight", $"{lhs.weight} != {rhs.weight}");
    }

}

static void extensions_VRM_blendShapeMaster_blendShapeGroups_L_materialValues(DiffContext diff, List<VrmMaterialValueBind> lhs, List<VrmMaterialValueBind> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                extensions_VRM_blendShapeMaster_blendShapeGroups_L_materialValues_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void extensions_VRM_blendShapeMaster_blendShapeGroups_L_materialValues_L(DiffContext diff, VrmMaterialValueBind lhs, VrmMaterialValueBind rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.materialName != rhs.materialName)
    {
        diff.Add("materialName", $"{lhs.materialName} != {rhs.materialName}");
    }

    if(lhs.propertyName != rhs.propertyName)
    {
        diff.Add("propertyName", $"{lhs.propertyName} != {rhs.propertyName}");
    }

            extensions_VRM_blendShapeMaster_blendShapeGroups_L_materialValues_L_targetValue(diff.GetChild("targetValue"), lhs.targetValue, rhs.targetValue);

}

static void extensions_VRM_blendShapeMaster_blendShapeGroups_L_materialValues_L_targetValue(DiffContext diff, Single[] lhs, Single[] rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
        }
        else{
            diff.Add("lhs is null");
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
        }
        else{
            if(lhs.Length!=rhs.Length)
            {
                diff.Add("rhs.Length != rhs.Length");
            }
            else{
                int i=0;
                foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
                {
                    extensions_VRM_blendShapeMaster_blendShapeGroups_L_materialValues_L_targetValue_L(diff.GetChild(i++), l, r);
                }
            }
        }
    }
}

static void extensions_VRM_blendShapeMaster_blendShapeGroups_L_materialValues_L_targetValue_L(DiffContext diff, Single lhs, Single rhs)
{
}

static void extensions_VRM_secondaryAnimation(DiffContext diff, VrmSecondaryAnimation lhs, VrmSecondaryAnimation rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

            extensions_VRM_secondaryAnimation_boneGroups(diff.GetChild("boneGroups"), lhs.boneGroups, rhs.boneGroups);

            extensions_VRM_secondaryAnimation_colliderGroups(diff.GetChild("colliderGroups"), lhs.colliderGroups, rhs.colliderGroups);

}

static void extensions_VRM_secondaryAnimation_boneGroups(DiffContext diff, List<VrmSecondaryAnimationGroup> lhs, List<VrmSecondaryAnimationGroup> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                extensions_VRM_secondaryAnimation_boneGroups_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void extensions_VRM_secondaryAnimation_boneGroups_L(DiffContext diff, VrmSecondaryAnimationGroup lhs, VrmSecondaryAnimationGroup rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.comment != rhs.comment)
    {
        diff.Add("comment", $"{lhs.comment} != {rhs.comment}");
    }

    if(lhs.stiffiness != rhs.stiffiness)
    {
        diff.Add("stiffiness", $"{lhs.stiffiness} != {rhs.stiffiness}");
    }

    if(lhs.gravityPower != rhs.gravityPower)
    {
        diff.Add("gravityPower", $"{lhs.gravityPower} != {rhs.gravityPower}");
    }

    if(lhs.gravityDir != rhs.gravityDir)
    {
        diff.Add("gravityDir", $"{lhs.gravityDir} != {rhs.gravityDir}");
    }

    if(lhs.dragForce != rhs.dragForce)
    {
        diff.Add("dragForce", $"{lhs.dragForce} != {rhs.dragForce}");
    }

    if(lhs.center != rhs.center)
    {
        diff.Add("center", $"{lhs.center} != {rhs.center}");
    }

    if(lhs.hitRadius != rhs.hitRadius)
    {
        diff.Add("hitRadius", $"{lhs.hitRadius} != {rhs.hitRadius}");
    }

            extensions_VRM_secondaryAnimation_boneGroups_L_bones(diff.GetChild("bones"), lhs.bones, rhs.bones);

            extensions_VRM_secondaryAnimation_boneGroups_L_colliderGroups(diff.GetChild("colliderGroups"), lhs.colliderGroups, rhs.colliderGroups);

}

static void extensions_VRM_secondaryAnimation_boneGroups_L_bones(DiffContext diff, Int32[] lhs, Int32[] rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
        }
        else{
            diff.Add("lhs is null");
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
        }
        else{
            if(lhs.Length!=rhs.Length)
            {
                diff.Add("rhs.Length != rhs.Length");
            }
            else{
                int i=0;
                foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
                {
                    extensions_VRM_secondaryAnimation_boneGroups_L_bones_L(diff.GetChild(i++), l, r);
                }
            }
        }
    }
}

static void extensions_VRM_secondaryAnimation_boneGroups_L_bones_L(DiffContext diff, Int32 lhs, Int32 rhs)
{
}

static void extensions_VRM_secondaryAnimation_boneGroups_L_colliderGroups(DiffContext diff, Int32[] lhs, Int32[] rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
        }
        else{
            diff.Add("lhs is null");
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
        }
        else{
            if(lhs.Length!=rhs.Length)
            {
                diff.Add("rhs.Length != rhs.Length");
            }
            else{
                int i=0;
                foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
                {
                    extensions_VRM_secondaryAnimation_boneGroups_L_colliderGroups_L(diff.GetChild(i++), l, r);
                }
            }
        }
    }
}

static void extensions_VRM_secondaryAnimation_boneGroups_L_colliderGroups_L(DiffContext diff, Int32 lhs, Int32 rhs)
{
}

static void extensions_VRM_secondaryAnimation_colliderGroups(DiffContext diff, List<VrmSecondaryAnimationColliderGroup> lhs, List<VrmSecondaryAnimationColliderGroup> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                extensions_VRM_secondaryAnimation_colliderGroups_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void extensions_VRM_secondaryAnimation_colliderGroups_L(DiffContext diff, VrmSecondaryAnimationColliderGroup lhs, VrmSecondaryAnimationColliderGroup rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.node != rhs.node)
    {
        diff.Add("node", $"{lhs.node} != {rhs.node}");
    }

            extensions_VRM_secondaryAnimation_colliderGroups_L_colliders(diff.GetChild("colliders"), lhs.colliders, rhs.colliders);

}

static void extensions_VRM_secondaryAnimation_colliderGroups_L_colliders(DiffContext diff, List<VrmSecondaryAnimationCollider> lhs, List<VrmSecondaryAnimationCollider> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                extensions_VRM_secondaryAnimation_colliderGroups_L_colliders_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void extensions_VRM_secondaryAnimation_colliderGroups_L_colliders_L(DiffContext diff, VrmSecondaryAnimationCollider lhs, VrmSecondaryAnimationCollider rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.offset != rhs.offset)
    {
        diff.Add("offset", $"{lhs.offset} != {rhs.offset}");
    }

    if(lhs.radius != rhs.radius)
    {
        diff.Add("radius", $"{lhs.radius} != {rhs.radius}");
    }

}

static void extensions_VRM_materialProperties(DiffContext diff, List<VrmMaterial> lhs, List<VrmMaterial> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            if(lhs.Count!=rhs.Count)
            {
                diff.Add("lhs.Count != rhs.Count");
                return;
            }
            int i=0;
            foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
            {
                extensions_VRM_materialProperties_L(diff.GetChild(i++), l, r);
            }
        }
    }
}

static void extensions_VRM_materialProperties_L(DiffContext diff, VrmMaterial lhs, VrmMaterial rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

    if(lhs.name != rhs.name)
    {
        diff.Add("name", $"{lhs.name} != {rhs.name}");
    }

    if(lhs.shader != rhs.shader)
    {
        diff.Add("shader", $"{lhs.shader} != {rhs.shader}");
    }

    if(lhs.renderQueue != rhs.renderQueue)
    {
        diff.Add("renderQueue", $"{lhs.renderQueue} != {rhs.renderQueue}");
    }

            extensions_VRM_materialProperties_L_floatProperties(diff.GetChild("floatProperties"), lhs.floatProperties, rhs.floatProperties);

            extensions_VRM_materialProperties_L_vectorProperties(diff.GetChild("vectorProperties"), lhs.vectorProperties, rhs.vectorProperties);

            extensions_VRM_materialProperties_L_textureProperties(diff.GetChild("textureProperties"), lhs.textureProperties, rhs.textureProperties);

            extensions_VRM_materialProperties_L_keywordMap(diff.GetChild("keywordMap"), lhs.keywordMap, rhs.keywordMap);

            extensions_VRM_materialProperties_L_tagMap(diff.GetChild("tagMap"), lhs.tagMap, rhs.tagMap);

}

static void extensions_VRM_materialProperties_L_floatProperties(DiffContext diff, Dictionary<string, Single> lhs, Dictionary<string, Single> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            // if(lhs.Count!=rhs.Count)
            // {
            //     diff.Add("lhs.Count != rhs.Count");
            //     return;
            // }
            foreach(var (key, l) in lhs)
            {
                if(rhs.TryGetValue(key, out Single r))
                {
                    extensions_VRM_materialProperties_L_floatProperties_D(diff.GetChild(key), l, r);
                }
                else{
                    diff.Add($"rhs.{key} not found");
                    // break;
                }
            }
            foreach(var (key, r) in rhs)
            {
                if(lhs.TryGetValue(key, out Single l))
                {
                    extensions_VRM_materialProperties_L_floatProperties_D(diff.GetChild(key), l, r);
                }
                else{
                    diff.Add($"lhs.{key} not found");
                    // break;
                }
            }
         }
    }
}

static void extensions_VRM_materialProperties_L_floatProperties_D(DiffContext diff, Single lhs, Single rhs)
{
}

static void extensions_VRM_materialProperties_L_vectorProperties(DiffContext diff, Dictionary<string, Single[]> lhs, Dictionary<string, Single[]> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            // if(lhs.Count!=rhs.Count)
            // {
            //     diff.Add("lhs.Count != rhs.Count");
            //     return;
            // }
            foreach(var (key, l) in lhs)
            {
                if(rhs.TryGetValue(key, out Single[] r))
                {
                    extensions_VRM_materialProperties_L_vectorProperties_D(diff.GetChild(key), l, r);
                }
                else{
                    diff.Add($"rhs.{key} not found");
                    // break;
                }
            }
            foreach(var (key, r) in rhs)
            {
                if(lhs.TryGetValue(key, out Single[] l))
                {
                    extensions_VRM_materialProperties_L_vectorProperties_D(diff.GetChild(key), l, r);
                }
                else{
                    diff.Add($"lhs.{key} not found");
                    // break;
                }
            }
         }
    }
}

static void extensions_VRM_materialProperties_L_vectorProperties_D(DiffContext diff, Single[] lhs, Single[] rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
        }
        else{
            diff.Add("lhs is null");
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
        }
        else{
            if(lhs.Length!=rhs.Length)
            {
                diff.Add("rhs.Length != rhs.Length");
            }
            else{
                int i=0;
                foreach(var (l, r) in Enumerable.Zip(lhs, rhs, (x, y)=>(x, y)))
                {
                    extensions_VRM_materialProperties_L_vectorProperties_D_L(diff.GetChild(i++), l, r);
                }
            }
        }
    }
}

static void extensions_VRM_materialProperties_L_vectorProperties_D_L(DiffContext diff, Single lhs, Single rhs)
{
}

static void extensions_VRM_materialProperties_L_textureProperties(DiffContext diff, Dictionary<string, Int32> lhs, Dictionary<string, Int32> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            // if(lhs.Count!=rhs.Count)
            // {
            //     diff.Add("lhs.Count != rhs.Count");
            //     return;
            // }
            foreach(var (key, l) in lhs)
            {
                if(rhs.TryGetValue(key, out Int32 r))
                {
                    extensions_VRM_materialProperties_L_textureProperties_D(diff.GetChild(key), l, r);
                }
                else{
                    diff.Add($"rhs.{key} not found");
                    // break;
                }
            }
            foreach(var (key, r) in rhs)
            {
                if(lhs.TryGetValue(key, out Int32 l))
                {
                    extensions_VRM_materialProperties_L_textureProperties_D(diff.GetChild(key), l, r);
                }
                else{
                    diff.Add($"lhs.{key} not found");
                    // break;
                }
            }
         }
    }
}

static void extensions_VRM_materialProperties_L_textureProperties_D(DiffContext diff, Int32 lhs, Int32 rhs)
{
}

static void extensions_VRM_materialProperties_L_keywordMap(DiffContext diff, Dictionary<string, Boolean> lhs, Dictionary<string, Boolean> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            // if(lhs.Count!=rhs.Count)
            // {
            //     diff.Add("lhs.Count != rhs.Count");
            //     return;
            // }
            foreach(var (key, l) in lhs)
            {
                if(rhs.TryGetValue(key, out Boolean r))
                {
                    extensions_VRM_materialProperties_L_keywordMap_D(diff.GetChild(key), l, r);
                }
                else{
                    diff.Add($"rhs.{key} not found");
                    // break;
                }
            }
            foreach(var (key, r) in rhs)
            {
                if(lhs.TryGetValue(key, out Boolean l))
                {
                    extensions_VRM_materialProperties_L_keywordMap_D(diff.GetChild(key), l, r);
                }
                else{
                    diff.Add($"lhs.{key} not found");
                    // break;
                }
            }
         }
    }
}

static void extensions_VRM_materialProperties_L_keywordMap_D(DiffContext diff, Boolean lhs, Boolean rhs)
{
}

static void extensions_VRM_materialProperties_L_tagMap(DiffContext diff, Dictionary<string, String> lhs, Dictionary<string, String> rhs)
{
    if(DiffContext.IsNullOrEmpty(lhs))
    {
        if(DiffContext.IsNullOrEmpty(rhs)){
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(DiffContext.IsNullOrEmpty(rhs)){
            diff.Add("rhs is null");
            return;
        }
        else{
            // if(lhs.Count!=rhs.Count)
            // {
            //     diff.Add("lhs.Count != rhs.Count");
            //     return;
            // }
            foreach(var (key, l) in lhs)
            {
                if(rhs.TryGetValue(key, out String r))
                {
                    extensions_VRM_materialProperties_L_tagMap_D(diff.GetChild(key), l, r);
                }
                else{
                    diff.Add($"rhs.{key} not found");
                    // break;
                }
            }
            foreach(var (key, r) in rhs)
            {
                if(lhs.TryGetValue(key, out String l))
                {
                    extensions_VRM_materialProperties_L_tagMap_D(diff.GetChild(key), l, r);
                }
                else{
                    diff.Add($"lhs.{key} not found");
                    // break;
                }
            }
         }
    }
}

static void extensions_VRM_materialProperties_L_tagMap_D(DiffContext diff, String lhs, String rhs)
{

    if(lhs is null){
        if(rhs is null){
            // equal
            return;
        }
        else{
            diff.Add("lhs is null");
            return;
        }
    }
    else{
        if(rhs is null){
            diff.Add("rhs is null");
            return;
        }
        else{
        }
    }

}

} // GltfDeserializer
} // ObjectNotation 
