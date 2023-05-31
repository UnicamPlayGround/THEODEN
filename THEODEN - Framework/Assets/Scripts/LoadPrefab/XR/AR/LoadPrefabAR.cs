using Models.ModelConfigurations;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace LoadPrefab.XR.AR
{
    public class LoadPrefabAR: LoadPrefab
    {
        public ARTrackedImageManager trackedImageManager;

        private void Start()
        {
            initialiseModel = false;
        }
        
        protected override void OnAfterSetupPrefab(GameObject prefab, ModelConfigs configs = null)
        {
            trackedImageManager.enabled = false;
            trackedImageManager.trackedImagePrefab = prefab;
            trackedImageManager.enabled = true;
        }
    }
}