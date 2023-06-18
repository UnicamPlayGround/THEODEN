using UnityEngine;
using UnityEngine.Networking;

namespace Startup.Download
{
    public static class TextManager
    {
        public static string LoadRemoteText(string url)
        {
            var request = UnityWebRequest.Get(url);
            request.SendWebRequest();
            while (!request.isDone)
            {
                // Waiting for the request to complete
            }
            if (request.result == UnityWebRequest.Result.Success)
                return request.downloadHandler.text;
            Debug.LogError(request.error);
            return null;
        }
    }
}