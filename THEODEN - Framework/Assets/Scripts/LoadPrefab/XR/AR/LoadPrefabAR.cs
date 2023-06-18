using Models.ModelConfigurations;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using CameraManager = Utility.CameraManager.XR.AR.CameraManager;

namespace LoadPrefab.XR.AR
{
    public class LoadPrefabAR: LoadPrefab
    {
        public ARTrackedImageManager trackedImageManager;
        public CameraManager cameraManager;

        private void Start()
        {
            initialiseModel = false;
            initialiseCamera = false;
        }
        
        protected override void OnAfterSetupPrefab(GameObject prefab, ModelConfigs configs = null)
        {
            if (configs != null)
            {
                prefab.transform.localScale = new Vector3(configs.prefab.scale.x, prefab.transform.localScale.y, configs.prefab.scale.z);
                prefab.transform.eulerAngles = new Vector3(configs.prefab.eulerRotation.x, prefab.transform.eulerAngles.y, configs.prefab.eulerRotation.z);
                prefab.transform.position = new Vector3(configs.prefab.position.x, prefab.transform.position.y, configs.prefab.position.z);
                cameraManager.modelConfigs = configs;
            }
            trackedImageManager.enabled = true;
            cameraManager.modelPrefab = prefab;
        }
    }
}