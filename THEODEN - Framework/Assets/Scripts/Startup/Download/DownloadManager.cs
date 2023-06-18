using System;
using System.Collections;
using System.Linq;
using Models.Version;
using UnityEngine;

namespace Startup.Download
{
    public class DownloadManager: MonoBehaviour
    {
        public string localVersionFilePath;
        public string remoteVersionFileUrl;
        public string assetBundleDownloadBaseUrl;
        public bool downloadAllAssetBundles;
        
        private void Start()
        {
            StartCoroutine(ManageAssetBundles());
        }

        private IEnumerator ManageAssetBundles()
        {
            var remoteVersionList = GetRemoteVersionList();
            if(remoteVersionList == null)
                yield break;
            
            var localVersionList = GetLocalVersionList();
            if(localVersionList == null)
                yield break;
            
            var localVersionsToUpdate = localVersionList & remoteVersionList;
            DownloadAll(localVersionsToUpdate);
            var updatedVersions = localVersionsToUpdate;
            if (downloadAllAssetBundles)
            {
                var missingVersions = remoteVersionList - localVersionList;
                DownloadAll(missingVersions);
                updatedVersions += missingVersions;
            }

            var text = JsonUtility.ToJson(updatedVersions);
            Local.TextManager.SaveTextToFile(localVersionFilePath, text);
            yield return null;
        }

        private void DownloadAll(params VersionList[] versionsToUpdate)
        {
            foreach (var versionList in versionsToUpdate)
            {
                foreach (var assetBundle in 
                         from version in versionList.versionList 
                         let versionNumber = Convert.ToUInt32(version.version) 
                         select AssetBundlesManager.
                             Download(assetBundleDownloadBaseUrl + version.name, versionNumber) 
                         into assetBundle where assetBundle != null select assetBundle)
                {
                    Utility.LocalStorageManager.AssetBundleManager.Instance.
                        SaveAssetBundle(assetBundle.name, assetBundle);
                }
            }
        }

        private VersionList GetLocalVersionList()
        {
            var localVersionText = Local.TextManager.LoadLocalTextFromFile(localVersionFilePath);
            var localVersionList = new VersionList();
            if (!(string.IsNullOrEmpty(localVersionText) || localVersionText == "{}" || localVersionText == "[]"))
                localVersionList = JsonUtility.FromJson<VersionList>(localVersionText);
            return localVersionList;
        }

        private VersionList GetRemoteVersionList()
        {
            var remoteVersionText = TextManager.LoadRemoteText(remoteVersionFileUrl);
            if (!string.IsNullOrEmpty(remoteVersionText) && remoteVersionText != "{}" && remoteVersionText != "[]")
                return JsonUtility.FromJson<VersionList>(remoteVersionText);
            Debug.LogError("remote version file is empty");
            return null;
        }
    }
}