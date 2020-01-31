using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniVRM10
{
    public class ModelMap : IDisposable
    {
        public Dictionary<VrmLib.Node, GameObject> Nodes = new Dictionary<VrmLib.Node, GameObject>();
        public Dictionary<VrmLib.Texture, Texture2D> Textures = new Dictionary<VrmLib.Texture, Texture2D>();
        public Dictionary<VrmLib.Material, Material> Materials = new Dictionary<VrmLib.Material, Material>();
        public Dictionary<VrmLib.MeshGroup, Mesh> Meshes = new Dictionary<VrmLib.MeshGroup, Mesh>();
        public Dictionary<VrmLib.MeshGroup, Renderer> Renderers = new Dictionary<VrmLib.MeshGroup, Renderer>();

        public void Dispose()
        {
        }
    }
}
