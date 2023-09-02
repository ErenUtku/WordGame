using System;
using UnityEngine.Serialization;

namespace Controllers.Data
{
    [Serializable]
    public class PlayerDataState
    {
        public int levelIndex = 3; //Default value => 2
    }
}

public enum PlayerDataType
{
    LevelIndex
}