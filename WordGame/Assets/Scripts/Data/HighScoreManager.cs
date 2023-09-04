using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class HighScoreManager
    {
        private Dictionary<int, int> highScores = new Dictionary<int, int>();
        private int totalLevels;

        public HighScoreManager(int totalLevels)
        {
            this.totalLevels = totalLevels;
            LoadHighScores();
        }

        public void SetHighScore(int level, int score)
        {
            if (level < 0 || level >= totalLevels)
            {
                Debug.LogError("Invalid level index.");
                return;
            }

            if (highScores.ContainsKey(level))
            {
                if (score > highScores[level])
                {
                    highScores[level] = score;
                    SaveHighScores();
                }
            }
            else
            {
                highScores[level] = score;
                SaveHighScores();
            }
        }

        public int GetHighScore(int level)
        {
            if (level < 0 || level >= totalLevels)
            {
                Debug.LogError("Invalid level index.");
                return 0;
            }

            return highScores.ContainsKey(level) ? highScores[level] : 0;
        }

        private void SaveHighScores()
        {
            foreach (var kvp in highScores)
            {
                PlayerPrefs.SetInt("HighScoreLevel" + kvp.Key, kvp.Value);
            }
            PlayerPrefs.Save();
        }
        
        private void LoadHighScores()
        {
            highScores = new Dictionary<int, int>();

            for (int level = 0; level < totalLevels; level++) 
            {
                int score = PlayerPrefs.GetInt("HighScoreLevel" + level, 0); 
                highScores[level] = score;
            }
        }
    }
}