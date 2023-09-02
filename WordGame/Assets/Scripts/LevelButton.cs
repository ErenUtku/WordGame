using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int levelIndex;

    private Button _button;
    

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SelectLevel);
    }

    private void SelectLevel()
    {
        if (!DataManager.Instance.IsLevelValid(levelIndex)) return;
        
        DataManager.Instance.SetLevel(levelIndex);
        SceneLoader.Instance.LoadScene("Game");

    }
}
