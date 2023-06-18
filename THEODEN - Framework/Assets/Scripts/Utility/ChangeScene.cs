using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    public class ChangeScene: MonoBehaviour
    {
        public static void ChangeToScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}