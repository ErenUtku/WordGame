using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tile
{
    public class TileObjectController : MonoBehaviour
    {
        [SerializeField] private GameObject blockObject;

        private List<int> _childrenList;

        private Transform _currentParent;
        private float _xPosition;
        private float _yPosition;
        private float _zPosition;
        
        [SerializeField] private bool childrenExist;

        private void Awake()
        {
            TileSelector.TileMoved += CheckContainsChildren;
        }

        public void SetPosition(float x, float y, float z)
        {
            _xPosition = x;
            _yPosition = y;
            _zPosition = z;
            
            this.gameObject.transform.localPosition = new Vector3(x,y,z);
            _currentParent = this.transform.parent;
        }
        
        public void ReturnPreviousPosition()
        {
            this.gameObject.transform.localPosition = new Vector3(_xPosition,_yPosition,_zPosition);
            transform.SetParent(_currentParent);
        }
        
        public void SetChildrenList(List<int> children)
        {
            _childrenList = new List<int>();
            _childrenList = children;
        }
        
        public bool ChildrenExist()
        {
            return _childrenList.Count > 0;
        } 
        
        public void BlockerActivation(bool value)
        {
            blockObject.SetActive(value);
        }
        
        private void CheckContainsChildren(TileController tileController)
        {
            if (!childrenExist)
                return;
            
            if (_currentParent.Cast<Transform>().Any(child => tileController.TileData.children.Any(childId => childId.ToString() == child.name)))
            {
                BlockerActivation(true);
                tileController.TileBlocked = true;
            }
            
            tileController.TileBlocked = false;
            BlockerActivation(false);
        }
        
    }
}
