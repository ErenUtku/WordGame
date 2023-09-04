using Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tile
{
    public class TileMapBuilder : MonoBehaviour
    {
        [FormerlySerializedAs("wordTilePrefab")] [SerializeField] private TileController tileControllerPrefab;
        [SerializeField] private Transform tilesParent;

        private void Awake()
        {
            var levelData = DataManager.Instance.GetLevelData();
                
            if (levelData is {tiles: not null})
            {
                foreach (var tileData in levelData.tiles)
                {
                    TileController newTileController = Instantiate(tileControllerPrefab, tilesParent);
                    newTileController.Initialize(tileData);
                }
            }
        }
    
    }
}
