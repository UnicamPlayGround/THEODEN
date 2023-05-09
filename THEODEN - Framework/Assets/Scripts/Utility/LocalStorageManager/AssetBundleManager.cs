using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.Events;

namespace Utility.LocalStorageManager
{
    public class AssetBundleManager
    {
        private static AssetBundleManager _instance;
        private AssetBundleManager()
        {
            _assetBundles = new ConcurrentDictionary<string, AssetBundle>();
        }
        public static AssetBundleManager Instance => _instance??=new AssetBundleManager();

        private readonly ConcurrentDictionary<string, AssetBundle> _assetBundles;

        public AssetBundle LoadAssetBundle(string assetBundleName, UnityAction onAssetBundleNotPresent = null)
        {
            if(_assetBundles.ContainsKey(assetBundleName))
                return _assetBundles[assetBundleName];
            onAssetBundleNotPresent?.Invoke();
            return null;
        }
        
        public void SaveAssetBundle(string assetBundleName, AssetBundle assetBundle)
        {
            _assetBundles.AddOrUpdate(assetBundleName, assetBundle, 
                (key, oldBundle) => assetBundle);
        }
    }
}