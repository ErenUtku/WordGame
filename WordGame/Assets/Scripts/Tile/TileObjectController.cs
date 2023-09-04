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

        private TileController _tileController;

        private void Awake()
        {
            _tileController = GetComponentInParent<TileController>();
            TileSelector.TileMoved += CheckContainsChildren;
        }

        private void OnDestroy()
        {
            TileSelector.TileMoved -= CheckContainsChildren;
        }

        public void SetPosition(float x, float y, float z)
        {
            _xPosition = x;
            _yPosition = y;
            _zPosition = z;
            
            this.gameObject.transform.localPosition = new Vector3(x,y,z);
        }

        public void SetParent(Transform parent)
        {
            _currentParent = parent;
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
            childrenExist =  _childrenList.Count > 0;
            return childrenExist;
        } 
        
        public void BlockerActivation(bool value)
        {
            blockObject.SetActive(value);
        }
        
        private void CheckContainsChildren()
        {
            if (!childrenExist)
                return;
            
            if (_currentParent.Cast<Transform>().Any(child => _tileController.TileData.children.Any(childId => childId.ToString() == child.name)))
            {
                BlockerActivation(true);
                _tileController.TileBlocked = true;
                return;
            }
            
            _tileController.TileBlocked = false;
            BlockerActivation(false);
        }
        
    }
}
