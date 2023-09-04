using UnityEngine.SceneManagement;
using Utils;

namespace Controllers
{
    public class SceneLoader : MonoSingletonPersistent<SceneLoader>
    {
        private void Start()
        {
            Invoke(nameof(ChangeSceneToMenu), 2);
        }

        private void ChangeSceneToMenu()
        {
            LoadScene("Menu");
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}