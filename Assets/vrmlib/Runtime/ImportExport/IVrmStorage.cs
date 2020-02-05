using System.Collections.Generic;

namespace VrmLib
{
    public interface IVrmStorage
    {
        #region glTF import
        string AssetVersion { get; }
        string AssetMinVersion { get; }
        string AssetGenerator { get; }
        string AssetCopyright { get; }
        int NodeCount { get; }
        Node CreateNode(int index);
        IEnumerable<int> GetChildNodeIndices(int i);
        int ImageCount { get; }
        Image CreateImage(int index);
        int TextureCount { get; }
        Texture CreateTexture(int index, List<Image> images);
        int MaterialCount { get; }
        Material CreateMaterial(int index, List<Texture> textures);
        int SkinCount { get; }
        Skin CreateSkin(int index, List<Node> nodes);
        int MeshCount { get; }
        MeshGroup CreateMesh(int index, List<Material> materials);
        (int, int) GetNodeMeshSkin(int index);
        int AnimationCount { get; }
        Animation CreateAnimation(int index, List<Node> nodes);
        #endregion

        #region VRM
        bool HasVrm { get; }
        Meta CreateVrmMeta(List<Texture> textures);
        string VrmExporterVersion { get; }
        string VrmSpecVersion { get; }
        void LoadVrmHumanoid(List<Node> nodes);
        BlendShapeManager CreateVrmBlendShape(List<MeshGroup> meshGroups, List<Material> materials);
        SpringBoneManager CreateVrmSpringBone(List<Node> nodes);
        FirstPerson CreateVrmFirstPerson(List<Node> nodes, List<MeshGroup> meshGroups);
        LookAt CreateVrmLookAt();
        #endregion
    }
}
