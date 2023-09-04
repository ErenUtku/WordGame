using Data;
using UnityEngine;

namespace Controllers
{
    public class LevelManager : MonoBehaviour
    {
        private LevelData _currentLevelData;
        #region DELEGATE
        
        public delegate void LevelStartHandler(LevelData levelData);

        public delegate void LevelCompleteHandler(LevelData levelData);


        #endregion

        #region EVENTS

        public static LevelStartHandler OnLevelStart;
        public static LevelCompleteHandler OnLevelComplete;


        #endregion

        #region PUBLIC FIELDS / PROPS
        
        public static LevelManager Instance { get; private set; }

        #endregion
        
        #region PUBLIC METHODS
        
        public LevelData GetLevel()
        { 
            //return level
            return null;
        }
        public void LevelLoad()
        {
            //LoadLevel
        }
        
        public void LevelStart()
        {
            OnLevelStart?.Invoke(_currentLevelData);
        }
        
        
        public void LevelComplete()
        {
            DataManager.Instance.LevelIndex = ((DataManager.Instance.GetLevelIndex()+1) + 1); //+1 for index getting -1
            OnLevelComplete?.Invoke(_currentLevelData);
        }
        
        
        #endregion

        #region UNITY EVENT METHODS

        private void Awake()
        {
            Instance = this;
        }

        #endregion
    }
}