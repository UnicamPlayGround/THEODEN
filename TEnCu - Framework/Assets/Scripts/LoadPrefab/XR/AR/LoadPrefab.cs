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
        public GameObject sessionOrigin;
        public float tagSize = 0.1F;

        private void Awake()
        {
            initialiseModel = false;
        }

        protected override void OnAfterSetupPrefab(GameObject prefab, ModelConfigs configs = null)
        {
            var trackedImageManager = sessionOrigin.AddComponent<ARTrackedImageManager>();
            trackedImageManager.requestedMaxNumberOfMovingImages = 0;
            trackedImageManager.trackedImagePrefab = prefab;
            var referenceImageLibrary = trackedImageManager.CreateRuntimeLibrary();
            if (referenceImageLibrary is MutableRuntimeReferenceImageLibrary library)
            {
                var assetBundle = AssetBundleManager.Instance.LoadAssetBundle(CommonVariables.PrefabName);
                var images = assetBundle.LoadAllAssets<Texture2D>();
                foreach (var image in images)
                {
                    library.ScheduleAddImageWithValidationJob(image, image.name, tagSize);
                }
            }
            trackedImageManager.referenceLibrary = referenceImageLibrary;
        }
    }
}