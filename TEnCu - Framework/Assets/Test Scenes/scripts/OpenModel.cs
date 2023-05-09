using UnityEngine.SceneManagement;
using Utility;

namespace Test_Scenes.scripts
{
    public class OpenModel
    {
        public static void OpenModelScene(string assetBundleName)
        {
            CommonVariables.PrefabName = assetBundleName;
            SceneManager.LoadScene("AR Sample Scene", LoadSceneMode.Single);
        }
    }
}
