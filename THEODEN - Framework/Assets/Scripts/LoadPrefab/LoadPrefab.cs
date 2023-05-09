using Models.ModelConfigurations;
using UnityEngine;

namespace LoadPrefab
{
    public class LoadPrefab : MonoBehaviour
    {
        public bool initialiseCamera = true;
        public bool initialiseModel = true;
        public GameObject cameraObject;
        private void Awake()
        {
            LoadModel(Utility.CommonVariables.PrefabName);
            enabled = false;
        }

        private void LoadModel(string prefabName)
        {
            if (prefabName == string.Empty)
            {
                Utility.ChangeScene.ChangeToScene(Utility.CommonVariables.HomeScene);
                return;
            }

            var assetBundle = Utility.LocalStorageManager.AssetBundleManager.Instance.
                LoadAssetBundle(prefabName, () =>
                {
                    Utility.ChangeScene.ChangeToScene(Utility.CommonVariables.HomeScene);
                });

            var prefab = assetBundle.LoadAllAssets<GameObject>()[0];
            var prefabConfigurationsText = assetBundle.LoadAllAssets<TextAsset>()[0];
            var prefabConfigurations = JsonUtility.FromJson<ModelConfigs>(prefabConfigurationsText.text);
            SetupCamera(prefab, prefabConfigurations);
            SetupPrefab(prefab, prefabConfigurations);
        }

        private void SetupPrefab(GameObject prefab, ModelConfigs prefabConfigurations)
        {
            if (initialiseModel)
            {
                //setup prefab
                var instantiated = Instantiate(prefab);

                instantiated.transform.position = prefabConfigurations.prefab.position.GetVector3();
                instantiated.transform.rotation =
                    Quaternion.Euler(prefabConfigurations.prefab.eulerRotation.GetVector3());
                instantiated.transform.localScale = prefabConfigurations.prefab.scale.GetVector3();

                OnAfterSetupPrefab(instantiated);
            }
            else
            {
                OnAfterSetupPrefab(prefab, prefabConfigurations);
            }
        }
        protected virtual void OnAfterSetupPrefab(GameObject prefab, ModelConfigs configs = null) { }
        private void SetupCamera(GameObject prefab, ModelConfigs prefabConfigurations)
        {
            if (initialiseCamera)
            {
                //setup camera transform
                cameraObject.transform.position = prefabConfigurations.prefab.position.GetVector3();
                cameraObject.transform.rotation =
                    Quaternion.Euler(prefabConfigurations.camera.eulerRotation.GetVector3());

                //setup camera fov
                var cam = cameraObject.GetComponentInChildren<Camera>();
                cam.fieldOfView = prefabConfigurations.camera.fieldOfView.max;
                cam.transform.position = prefabConfigurations.camera.position.GetVector3();

                //setup CameraManager
                var cameraManager = cameraObject.GetComponent<Utility.CameraManager.CameraManager>();
                cameraManager.modelConfigs = prefabConfigurations;
                cameraManager.model = prefab;

                OnAfterSetupCamera(cameraObject);
            }
            else
            {
                OnAfterSetupCamera(cameraObject, prefab, prefabConfigurations);
            }
        }
        protected virtual void OnAfterSetupCamera(GameObject cameraObj, GameObject prefab = null, ModelConfigs configs = null) { }
    }
}
