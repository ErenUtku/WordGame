using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using Utils;

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

    public void SetLevelScore(List<string> claimedWords , int parentChildCount)
    {
        var totalScore = _scoringSystem.CalculateTotalScore(claimedWords, parentChildCount);

        _dataManager.HighScoreManager.SetHighScore(_dataManager.GetLevelIndex(), totalScore);
    }
}
