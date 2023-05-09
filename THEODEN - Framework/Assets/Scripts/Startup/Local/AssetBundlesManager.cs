using System.IO;
using Test_Scenes.models;
using UnityEngine;
using UnityEngine.Networking;

namespace Startup.Local
{
    public static class AssetBundlesManager
    {
        private const string AssetBundlePath = "AssetBundles/";
        public static void LoadAllAssetBundlesFromStorage()
        {
            var tmp = Resources.Load<TextAsset>("models");
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
                        model.assetBundleName+".unity3d"));
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
                    SaveAssetBundle(model.name, assetBundle);
            }
            /*
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
            */
        }
    }
}