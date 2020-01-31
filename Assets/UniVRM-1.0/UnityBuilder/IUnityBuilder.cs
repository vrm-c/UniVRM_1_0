using System.Collections.Generic;
using UnityEngine;

namespace UniVRM10
{
    public interface IUnityBuilder
    {
        Dictionary<VrmLib.Texture, Texture2D> Textures { get; }
        Dictionary<VrmLib.Material, Material> Materials { get; }
        Dictionary<VrmLib.MeshGroup, Mesh> Meshes { get; }
        Dictionary<VrmLib.Node, GameObject> Nodes { get; }
        Dictionary<VrmLib.MeshGroup, Renderer> Renderers { get; }

        GameObject Root { get; }
    }
}
