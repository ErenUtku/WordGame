using System;
using System.Collections.Generic;
using System.Linq;
using Controllers.Data;
using UnityEngine;
using Utils;

namespace Data
{
    public class DataManager : MonoSingletonPersistent<DataManager>
    {
        public Dictionary<string, bool> Dictionary { get; } = new Dictionary<string, bool>(); 
        private PlayerDataState _playerDataState;
        private LevelData[] _levels;
        private Dictionary<int, int> _highScores = new Dictionary<int, int>();
    
        private LevelData _currentLevelData;
        private int _currentLevelIndex;
        private int _totalLevelIndex;
    
        public HighScoreManager HighScoreManager;
        public static event Action<PlayerDataType> OnDataChanged;
        private void Awake()
        {
            //Dictionary
            LoadDictionary();
        
            //Levels
            LoadLevels();

            //Player
            LoadPlayerData();
        
            //HighScores
            HighScoreManager = new HighScoreManager(_levels.Length);
        
            //First Time App Launching
            Application.targetFrameRate = 60;
        }
    
        #region PlayerMethods

        private void LoadPlayerData()
        {
            _playerDataState= SaveManager.LoadData<PlayerDataState>("SavePlayerDataState");
        }
        public int LevelIndex
        {
            get => _playerDataState.levelIndex;
            set => SetLevelIndex(value);
        }
    
        private void SetLevelIndex(int value)
        {
            _playerDataState.levelIndex = value;
            SaveManager.SaveData(_playerDataState,"SavePlayerDataState");
            OnDataChanged?.Invoke(PlayerDataType.LevelIndex);
        }
    
        #endregion

        #region Level Methods

        private void LoadLevels()
        {
            TextAsset[] levelFiles = Resources.LoadAll<TextAsset>("levels");

            _levels = new LevelData[levelFiles.Length];

            _totalLevelIndex = levelFiles.Length;

            for (int i = 0; i < levelFiles.Length; i++)
            {
                _levels[i] = JsonUtility.FromJson<LevelData>(levelFiles[i].text);
            }
        }

        public int GetLevelsCount()
        {
            return _totalLevelIndex;
        }

        public void SetLevel(int levelIndex)
        {
            if (!IsLevelValid(levelIndex)) return;
        
            _currentLevelIndex = levelIndex;
            _currentLevelData = _levels[levelIndex];

        }

        public bool IsLevelValid(int levelIndex)
        {
            if (levelIndex < 0 || levelIndex >= _levels.Length) return false;
            return true;
        }
    
        public LevelData GetLevelData()
        {
            return _currentLevelData;
        }

        public int GetLevelIndex()
        {
            return _currentLevelIndex;
        }

        #endregion
    
        #region Dictionary Methods
        private void LoadDictionary()
        {
            TextAsset dictionaryFile = Resources.Load<TextAsset>("dictionary/dictionary");

            if (dictionaryFile != null)
            {
                // Split the text file into words and create a dictionary.
                string[] words = dictionaryFile.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            
                // Initialize all words in the dictionary as false.
                foreach (string word in words)
                {
                    Dictionary[word.ToLower()] = false;
                }
            }
            else
            {
                // Handle the case where the dictionary file is not found.
                Debug.LogError("Dictionary file not found!");
            }
        }
    
        public void ClearDictionary()
        {
            foreach (var key in Dictionary.Keys.ToList())
            {
                Dictionary[key] = false;
            }
        }
    
    
    
        #endregion
    
    }
}
