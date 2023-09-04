using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelButtonUI : MonoBehaviour
    {
        [Header("LevelValue")]
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Button levelButton;
        [Space]
            
        [Header("HighScore UI")]
        [SerializeField] private GameObject highScoreObject;
        [SerializeField] private TextMeshProUGUI highScoreValueText;

        [Header("LockedObject")] 
        [SerializeField] private GameObject lockedObject;
        
        public void ActivateHighScore(int totalScore)
        {
            highScoreObject.SetActive(true);
            highScoreValueText.text = totalScore.ToString();
        }

        public void SetLevelText(int value)
        {
            levelText.text = $"Level {value + 1}";
        }

        public void UnlockLevel()
        {
            lockedObject.SetActive(false);
        }

        public void LevelButtonBehavior(Action method)
        {
            levelButton.onClick.AddListener(method.Invoke);
        }
    }
}
