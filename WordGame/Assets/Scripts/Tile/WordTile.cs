using UnityEngine;

namespace Tile
{
    public class WordTile : MonoBehaviour
    {
        public int tileId;
        public TileData tileData;
        public string tileCharacter;

        [SerializeField] private bool used;

        private TileUIController _tileUIController;
        private TileObjectController _tileObjectController;

        private void Awake()
        {
            _tileUIController = GetComponent<TileUIController>();
            _tileObjectController = GetComponent<TileObjectController>();
        }

        void Start()
        {
            //Set UI Text And GameObject Name
            _tileUIController.SetTileText(tileCharacter);

            //test
            var childrenExist = _tileObjectController.ChildrenExist();

            _tileObjectController.BlockerActivation(childrenExist);

        }

        public void SetUpTileFromData(TileData newTileData)
        {
            tileData = newTileData;
            tileId = tileData.id;
            tileCharacter = tileData.character;
            _tileObjectController.SetPosition(tileData.position.x,tileData.position.y,tileData.position.z);
            _tileObjectController.SetChildrenList(tileData.children);

        }
    }
}
