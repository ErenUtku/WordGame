using System;
using Data;
using Unity.VisualScripting;
using UnityEngine;

namespace Controllers
{
    public class UIController : MonoBehaviour
    {
        [Header("Panels")] 
        [SerializeField] private GameObject levelCompletePanel;
        [SerializeField] private GameObject nextButton;

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


        public void ShowLevelCompletePanel(LevelData levelData)
        {
            nextButton.SetActive(false); //Delay Button Activation

            levelCompletePanel.SetActive(true);

            Invoke(nameof(DelayNextButton), 2f);
        }
    
        private void DelayNextButton()
        {
            nextButton.SetActive(true);
        }
    }
}
