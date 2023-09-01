using UnityEngine;

namespace Tile
{
    public class TilesMapController : MonoBehaviour
    {
        [SerializeField] private WordTile wordTilePrefab;
        [SerializeField] private Transform tilesParent;
        [SerializeField] private string levelFileName = "level_1";
        
        
        private void Awake()
        {
            var levelJson = Resources.Load<TextAsset>("levels/" + levelFileName);
            
            if (levelJson == null) return;
            
            LevelData levelData = JsonUtility.FromJson<LevelData>(levelJson.text);
                
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
