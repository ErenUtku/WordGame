using System;
using UnityEngine.Serialization;

namespace Controllers.Data
{
    [Serializable]
    public class PlayerDataState
    {
        public int levelIndex = 1; //Default value => 1
    }
}

public enum PlayerDataType
{
    LevelIndex
}