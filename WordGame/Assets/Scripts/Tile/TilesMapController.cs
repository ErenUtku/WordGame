using UnityEngine;

namespace Tile
{
    public class TilesMapController : MonoBehaviour
    {
        [SerializeField] private WordTile wordTilePrefab;
        [SerializeField] private Transform tilesParent;

        private void Awake()
        {
            var levelData = DataManager.Instance.GetLevelData();
                
            if (levelData is {tiles: not null})
            {
                foreach (var tileData in levelData.tiles)
                {
                    WordTile newTile = Instantiate(wordTilePrefab, tilesParent);
                    newTile.SetUpTileFromData(tileData);
                }
            }
        }
    
    }
}
