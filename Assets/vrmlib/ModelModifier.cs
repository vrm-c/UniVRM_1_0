using System;
using System.Linq;

namespace VrmLib
{
    /// <summary>
    /// Modelを安全に変更できるようにラップする
    /// </summary>
    public class ModelModifier
    {
        /// <summary>
        /// 直接データを変更する場合は整合性に注意
        /// </summary>
        public Model Model;

        public ModelModifier(Model model)
        {
            Model = model;
        }

        /// <summary>
        /// Meshを置き換える。
        ///
        /// src=null, dst!=null で追加。
        /// src!=null, dst=null で削除。
        /// </summary>
        /// <param name = "src">置き換え元</param>
        /// <param name = "dst">置き換え先</param>
        public void MeshReplace(MeshGroup src, MeshGroup dst)
        {
            // replace: Meshes
            if (src != null)
            {
                Model.MeshGroups.RemoveAll(x => x == src);
            }
            if (dst != null && !Model.MeshGroups.Contains(dst))
            {
                Model.MeshGroups.Add(dst);
            }

            // replace: Node.Mesh
            foreach (var node in Model.Nodes)
            {
                if (src != null && src == node.MeshGroup)
                {
                    node.MeshGroup = dst;
                }
            }

            // fix VRM
            if (Model.Vrm != null)
            {
                // replace: VrmBlendShape.Mesh
                if (Model.Vrm.BlendShape != null)
                {
                    foreach (var x in Model.Vrm.BlendShape.BlendShapeList)
                    {
                        for (int i = 0; i < x.BlendShapeValues.Count; ++i)
                        {
                            var v = x.BlendShapeValues[i];
                            if (src != null && src == v.Mesh)
                            {
                                v.Mesh = dst;
                            }
                        }
                    }
                }

                // replace: VrmFirstPerson.MeshAnnotations
                if (src != null)
                {
                    Model.Vrm.FirstPerson.Annotations.RemoveAll(x => x.Mesh == src);
                }
                if (dst != null && !Model.Vrm.FirstPerson.Annotations.Any(x => x.Mesh == dst))
                {
                    Model.Vrm.FirstPerson.Annotations.Add(
                        new FirstPersonMeshAnnotation(dst, FirstPersonMeshType.Auto));
                }
            }
        }

        /// <summary>
        /// NodeのMesh参照を削除する
        /// </summary>
        public void NodeMeshClear()
        {
            foreach (var node in Model.Nodes)
            {
                node.MeshGroup = null;
            }
        }

        /// <summary>
        /// Nodeを置き換える。参照を置換する。
        /// </summary>
        public void NodeReplace(Node src, Node dst)
        {
            if (src == null)
            {
                // new node. add to root
                Model.Root.Add(dst);
            }
            else
            {
                // add dst same parent
                if (dst != null)
                {
                    src.Parent.Add(dst, ChildMatrixMode.KeepWorld);
                }

                // remove all child
                foreach (var child in src.Children.ToArray())
                {
                    if (dst != null)
                    {
                        dst.Add(child, ChildMatrixMode.KeepWorld);
                    }
                    else
                    {
                        src.Parent.Add(child, ChildMatrixMode.KeepWorld);
                    }
                }

                // remove from parent
                src.Parent.Remove(src);
                Model.Nodes.Remove(src);

                // remove from skinning
                foreach (var skin in Model.Skins)
                {
                    skin.Replace(src, dst);
                }

                // fix animation reference
                foreach (var animation in Model.Animations)
                {
                    if (animation.NodeMap.TryGetValue(src, out NodeAnimation nodeAnimation))
                    {
                        animation.NodeMap.Remove(src);
                        animation.NodeMap.Add(dst, nodeAnimation);
                    }
                }
            }

            if (dst != null)
            {
                if (Model.Nodes.Contains(dst))
                {
                    throw new Exception("already exists");
                }
                Model.Nodes.Add(dst);
            }

            // TODO: fix VRM
            if (Model.Vrm != null)
            {

                // spring

            }
        }

        public void MaterialReplace(Material src, Material dst)
        {
            // replace material of submesh
            foreach (var group in Model.MeshGroups)
            {
                foreach (var mesh in group.Meshes)
                {
                    foreach (var submesh in mesh.Submeshes)
                    {
                        if (submesh.Material == src)
                        {
                            submesh.Material = dst;
                        }
                    }
                }
            }
        }
    }
}