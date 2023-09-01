using Data;
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

        private Transform _previousParent;

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

            _previousParent = this.gameObject.transform.parent;

        }

        public void SetUpTileFromData(TileData newTileData)
        {
            tileData = newTileData;
            tileId = tileData.id;
            tileCharacter = tileData.character;
            _tileObjectController.SetPosition(tileData.position.x,tileData.position.y,tileData.position.z);
            _tileObjectController.SetChildrenList(tileData.children);
        }

        public void ReturnPreviousPosition()
        {
            this.gameObject.transform.localPosition = new Vector3(tileData.position.x,tileData.position.y,tileData.position.z);
            //transform.SetParent(_previousParent);
        }
    }
}
