using UnityEngine;
using UnityEngine.Networking;

namespace Startup.Download
{
    public static class AssetBundlesManager
    {
        public static AssetBundle Download(string url, uint version)
        {
            var request = UnityWebRequestAssetBundle.GetAssetBundle(url, version, version);
            request.SendWebRequest();
            while (!request.isDone)
            {
                // Waiting
            }

            if (request.result == UnityWebRequest.Result.Success) 
                return DownloadHandlerAssetBundle.GetContent(request);
            Debug.LogError(request.error);
            return null;
        }
    }
}