using System.Data.Common;
using Data;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    [RequireComponent(typeof(LevelButtonUI))] 
    public class LevelButton : MonoBehaviour
    {
        private LevelButtonUI _levelButtonUI;
        private int _levelIndex;
        
        public void Initialize(int value)
        {
            this._levelIndex = value;
            
            this.gameObject.name = $"Level_{_levelIndex}";
        }
        
        private void Awake()
        {
            _levelButtonUI = GetComponent<LevelButtonUI>();
            
            _levelButtonUI.LevelButtonBehavior(LoadLevelFromIndex);
        }

        private void Start()
        {
            _levelButtonUI.SetLevelText(_levelIndex);
            
            var totalScore = DataManager.Instance.HighScoreManager.GetHighScore(_levelIndex);

            if (DataManager.Instance.LevelIndex <= _levelIndex) return;
            
            _levelButtonUI.UnlockLevel();
                
            if (totalScore <= 0) return;
        
            _levelButtonUI.ActivateHighScore(totalScore);

        }

        private void LoadLevelFromIndex()
        {
            if (!DataManager.Instance.IsLevelValid(_levelIndex)) return;
        
            DataManager.Instance.SetLevel(_levelIndex);
            SceneLoader.Instance.LoadScene("Game");
        }
        
    }
}
