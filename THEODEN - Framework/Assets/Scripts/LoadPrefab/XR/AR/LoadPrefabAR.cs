using Models.ModelConfigurations;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Utility;
using Utility.LocalStorageManager;

namespace LoadPrefab.XR.AR
{
    public class LoadPrefabAR: LoadPrefab
    {
        public ARTrackedImageManager trackedImageManager;

        private void Awake()
        {
            initialiseModel = false;
        }
        
        protected override void OnAfterSetupPrefab(GameObject prefab, ModelConfigs configs = null)
        {
            Debug.Log("Starting");
            trackedImageManager.requestedMaxNumberOfMovingImages = 0;
            trackedImageManager.trackedImagePrefab = prefab;
            Debug.Log("Creating library");
            var assetBundle = AssetBundleManager.Instance.LoadAssetBundle(CommonVariables.PrefabName);
            var referenceImageLibrary = assetBundle.LoadAllAssets<XRReferenceImageLibrary>()[0];
            Debug.Log("Created library");
            trackedImageManager.referenceLibrary = referenceImageLibrary;
            Debug.Log("Finished");
            trackedImageManager.enabled = true;
        }
    }
}