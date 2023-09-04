using Data;
using UnityEngine;

namespace Controllers
{
    public class LevelSelectionController : MonoBehaviour
    {
        [SerializeField] private LevelSelectionScene sceneType;

        [Header("LevelButtonValues")] [SerializeField]
        private LevelButton levelButtonPrefab;

        [SerializeField] private Transform contentParent;

        private void Awake()
        {
            var totalLevelCount = DataManager.Instance.GetLevelsCount();

            for (var i = 0; i < totalLevelCount; i++)
            {
                var levelButton = Instantiate(levelButtonPrefab, contentParent);
                levelButton.Initialize(i);
            }
        }
    }
}

public enum LevelSelectionScene
{
    Menu,
    Game
}