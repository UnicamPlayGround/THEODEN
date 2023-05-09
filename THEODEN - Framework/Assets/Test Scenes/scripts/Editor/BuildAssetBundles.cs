using System.IO;
using UnityEditor;
using UnityEngine;

namespace Test_Scenes.scripts.Editor
{
    public class BuildAssetBundles : MonoBehaviour
    {
        [MenuItem("Asset Bundles/Build AssetBundles")]
        private static void BuildAllAssetBundles()
        {
            const string assetBundleDirectory = "Assets/Editor/AssetBundles/";
            if (!Directory.Exists(assetBundleDirectory)) Directory.CreateDirectory(assetBundleDirectory);

            BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                BuildAssetBundleOptions.None,
                BuildTarget.Android);
        }
    }
}
