namespace VrmLib
{
    public class BlendShapeBindValue
    {
        /// <summary>
        /// 対象のMesh(Renderer)
        /// </summary>
        public Node Node;

        /// <summary>
        /// BlendShapeのIndex
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// BlendShapeの適用度
        /// </summary>
        public readonly float Value;

        public BlendShapeBindValue(Node node, string name, float value)
        {
            Node = node;
            Name = name;
            Value = value;
        }
    }
}
