using UnityEngine;


namespace UniVRM10
{
    [CreateAssetMenu(menuName = "VRM/BlendShapeClip")]
    public class BlendShapeClip : ScriptableObject
    {
#if UNITY_EDITOR
        /// <summary>
        /// Preview 用のObject参照
        /// </summary>
        [SerializeField]
        GameObject m_prefab;
        public GameObject Prefab
        {
            set { m_prefab = value; }
            get
            {
                if (m_prefab == null)
                {
                    var assetPath = UnityEditor.AssetDatabase.GetAssetPath(this);
                    if (!string.IsNullOrEmpty(assetPath))
                    {
                        // if asset is subasset of prefab
                        m_prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                        if (m_prefab != null) return m_prefab;

                        var parent = UnityPath.FromUnityPath(assetPath).Parent;
                        var prefabPath = parent.Parent.Child(parent.FileNameWithoutExtension + ".prefab");
                        m_prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath.Value);
                        if (m_prefab != null) return m_prefab;

                        var parentParent = UnityPath.FromUnityPath(assetPath).Parent.Parent;
                        var vrmPath = parent.Parent.Child(parent.FileNameWithoutExtension + ".vrm");
                        m_prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(vrmPath.Value);
                        if (m_prefab != null) return m_prefab;
                    }
                }
                return m_prefab;
            }
        }
#endif

        /// <summary>
        /// BlendShapePresetがUnknown場合の識別子
        /// </summary>
        [SerializeField]
        public string BlendShapeName = "";

        /// <summary>
        /// BlendShapePresetを識別する。Unknownの場合は、BlendShapeNameで識別する
        /// </summary>
        [SerializeField]
        public VrmLib.BlendShapePreset Preset;

        /// <summary>
        /// BlendShapeに対する参照(index ベース)
        /// </summary>
        /// <value></value>
        [SerializeField]
        public BlendShapeBinding[] Values = new BlendShapeBinding[] { };

        /// <summary>
        /// マテリアルに対する参照(名前ベース)
        /// </summary>
        /// <value></value>
        [SerializeField]
        public MaterialValueBinding[] MaterialValues = new MaterialValueBinding[] { };

        /// <summary>
        /// UniVRM-0.45: trueの場合、このBlendShapeClipは0と1の間の中間値を取らない。四捨五入する
        /// </summary>
        [SerializeField]
        public bool IsBinary;

        [SerializeField]
        public bool IgnoreBlink;

        [SerializeField]
        public bool IgnoreLookAt;

        [SerializeField]
        public bool IgnoreMouth;
    }
}
