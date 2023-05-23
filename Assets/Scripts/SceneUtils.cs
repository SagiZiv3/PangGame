using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pang
{
    internal sealed class SceneUtils : MonoBehaviour
    {
        public void RestartScene()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}