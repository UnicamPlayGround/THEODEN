using System.IO;
using Models.AssetBundleLocalList;
using UnityEngine;
using UnityEngine.Networking;

namespace Startup.Local
{
    public static class AssetBundlesManager
    {
        private const string AssetBundlePath = "AssetBundles/";
        public static void LoadAllAssetBundlesFromStorage()
        {
            var fileList = Directory.GetFiles(AssetBundlePath);
            foreach (var filename in fileList)
            {
                var assetBundle = AssetBundle.LoadFromFile(filename);
                if (assetBundle == null)
                {
                    Debug.LogError("Failed to load asset bundle: " + filename);
                    continue;
                }
                Utility.LocalStorageManager.AssetBundleManager.Instance.
                    SaveAssetBundle(assetBundle.name, assetBundle);
            }
        }

        public static void LoadAllAssetBundlesFromStreamingAssets(string assetBundlesJsonList)
        {
            var tmp = Resources.Load<TextAsset>(assetBundlesJsonList);
            if (tmp == null)
            {
                Debug.LogError("Failed to load models");
                return;
            }
            var models = JsonUtility.FromJson<ModelsList>(tmp.text);
            if (models == null)
            {
                Debug.LogError("Failed to parse models");
                return;
            }
            foreach (var model in models.models)
            {
                var assetBundleWebRequest =
                    UnityWebRequestAssetBundle.GetAssetBundle(Path.Combine(Application.streamingAssetsPath,
                        model.assetBundleName));
                assetBundleWebRequest.SendWebRequest();
                while (!assetBundleWebRequest.isDone)
                {
                }
                var assetBundle = DownloadHandlerAssetBundle.GetContent(assetBundleWebRequest);
                if (assetBundle == null)
                {
                    Debug.LogError("Failed to load asset bundle: " + model.name);
                    continue;
                }
                Utility.LocalStorageManager.AssetBundleManager.Instance.
                    SaveAssetBundle(model.assetBundleName, assetBundle);
            }
        }
    }
}