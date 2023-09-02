using System;
using Data;
using UnityEngine;

namespace Tile
{
    public class WordTile : MonoBehaviour
    {
        public int tileId;
        public TileData tileData;
        public string tileCharacter;

        public bool tileBlocked;
        public bool tileUsedInSlot;

        [SerializeField] private bool childrenExist;

        private TileUIController _tileUIController;
        private TileObjectController _tileObjectController;

        private Transform _currentParent;

        private void Awake()
        {
            _tileUIController = GetComponent<TileUIController>();
            _tileObjectController = GetComponent<TileObjectController>();

            TileSelector.TileMoved += CheckContainsChildren;
        }

        private void OnDestroy()
        {
            TileSelector.TileMoved -= CheckContainsChildren;
        }

        void Start()
        {
            //Set UI Text And GameObject Name
            _tileUIController.SetTileText(tileCharacter,tileId);
            
            childrenExist = _tileObjectController.ChildrenExist();

            tileBlocked = childrenExist;

            TileSlotUsage(false);

            _tileObjectController.BlockerActivation(childrenExist);
            
            _currentParent = this.gameObject.transform.parent;

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
            transform.SetParent(_currentParent);
        }

        private void CheckContainsChildren()
        {
            if (!childrenExist)
                return;
            
            foreach (Transform child in _currentParent)
            {
                foreach (var childId in tileData.children)
                {
                    if (childId.ToString() == child.name)
                    {
                        Debug.Log("Still contains");
                        _tileObjectController.BlockerActivation(true);
                        tileBlocked = true;
                        return;
                    }
                }
            }
            
            Debug.Log("Not contain");
            tileBlocked = false;
            _tileObjectController.BlockerActivation(false);
        }
        
        public bool IsTileBlocked()
        {
            return tileBlocked;
        }

        public void TileSlotUsage(bool value)
        {
            tileUsedInSlot = value;
        }

        public bool IsTileInSlot()
        {
            return tileUsedInSlot;
        }
        
    }
    
    
}
