using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVRM10
{
    public class AssetsContainer : MonoBehaviour
    {
        [SerializeField]
        private ModelAsset ModelAsset;

        public ModelAsset Assets =>  ModelAsset;

        public void SetAsset(ModelAsset asset)
        {
            ModelAsset = asset;
        }

        void OnDestroy()
        {
            if(ModelAsset != null)
            {
#if UNITY_EDITOR
                ModelAsset.DisposeEditor();
#else
                ModelAsset.Dispose();
#endif
            }
        }
    }
}


