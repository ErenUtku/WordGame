using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [Header("Level Value")]
    [SerializeField] private int levelIndex;

    [Space] 
    [SerializeField] private GameObject highScoreObject;
    [SerializeField] private TextMeshProUGUI highScoreValueText;

    private Button _button;
    

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SelectLevel);
    }

    private void Start()
    {
        var totalScore = DataManager.Instance.GetHighScore(levelIndex);

        if (totalScore <= 0) return;
        
        highScoreObject.SetActive(true);
        highScoreValueText.text = totalScore.ToString();
    }

    private void SelectLevel()
    {
        if (!DataManager.Instance.IsLevelValid(levelIndex)) return;
        
        DataManager.Instance.SetLevel(levelIndex);
        SceneLoader.Instance.LoadScene("Game");

    }
}
