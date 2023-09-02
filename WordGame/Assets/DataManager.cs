using System;
using System.Collections.Generic;
using Data;
using UnityEngine;
using Utils;

public class DataManager : MonoSingletonPersistent<DataManager>
{
    public Dictionary<string, bool> Dictionary { get; } = new Dictionary<string, bool>();
    
    private LevelData[] _levels; // Store levels as an array
    
    private LevelData _currentLevelData;
    private int _currentLevelIndex;

    void Start()
    {
        //Dictionary
        LoadDictionary();
        
        //Levels
        LoadLevels();
    }

    #region Level Methods

    private void LoadLevels()
    {
        TextAsset[] levelFiles = Resources.LoadAll<TextAsset>("levels");

        _levels = new LevelData[levelFiles.Length];

        for (int i = 0; i < levelFiles.Length; i++)
        {
            _levels[i] = JsonUtility.FromJson<LevelData>(levelFiles[i].text);
        }
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
    
    #endregion
}
