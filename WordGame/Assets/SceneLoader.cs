using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

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