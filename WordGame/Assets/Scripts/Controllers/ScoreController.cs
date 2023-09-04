using System.Collections.Generic;
using Data;
using UnityEngine;
using Utils;

namespace Controllers
{
    public class ScoreController : MonoBehaviour
    {
        public static ScoreController Instance;

        private ScoringSystem _scoringSystem;
        private DataManager _dataManager;
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _scoringSystem = new ScoringSystem();
            _dataManager = DataManager.Instance;
        }

        public int GetLevelScore(List<string> claimedWords , int parentChildCount)
        {
            return _scoringSystem.CalculateTotalScore(claimedWords, parentChildCount);
        }

        public void SetLevelScore(int totalScore)
        {
            _dataManager.HighScoreManager.SetHighScore(_dataManager.GetLevelIndex(), totalScore);
        }
    }
}
