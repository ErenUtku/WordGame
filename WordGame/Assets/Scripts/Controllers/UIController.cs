using System;
using Data;
using Slot;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class UIController : MonoBehaviour
    {
        [Header("Panels")] 
        [SerializeField] private GameObject levelCompletePanel;
        [SerializeField] private GameObject scorePanel;
        [SerializeField] private GameObject levelSelection;
        
        
        [SerializeField] private Button nextButton;
        
        [SerializeField] private GameObject newHighScore;
        [SerializeField] private TextMeshProUGUI highScoreText;
        

        public static UIController instance;
        private void Awake()
        {
            instance = this;

            LevelManager.OnLevelComplete += ShowLevelCompletePanel;
        }

        private void OnDestroy()
        {
            LevelManager.OnLevelComplete -= ShowLevelCompletePanel;
        }

        private void Start()
        {
            nextButton.onClick?.AddListener(SwitchPanels);
        }

        public void ShowLevelCompletePanel(LevelData levelData)
        {
            nextButton.gameObject.SetActive(false); //Delay Button Activation

            levelCompletePanel.SetActive(true);
            
            scorePanel.SetActive(true);
            
            levelSelection.SetActive(false);

            HighScoreUI();
            
            Invoke(nameof(DelayNextButton), 2f);
        }
    
        private void DelayNextButton()
        {
            nextButton.gameObject.SetActive(true);
        }

        private void SwitchPanels()
        {
            scorePanel.SetActive(false);
            levelSelection.SetActive(true);
        }

        private void HighScoreUI()
        {
            var previousHighScore = SlotController.instance.PreviousHighScore;
            var currentHighScore = SlotController.instance.CurrentHighScore;

            highScoreText.text = currentHighScore.ToString();

            if (currentHighScore > previousHighScore)
            {
                newHighScore.SetActive(true);
            }

        }
    }
}
