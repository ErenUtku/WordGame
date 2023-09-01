using System.Collections.Generic;
using UnityEngine;

namespace Tile
{
    public class TileObjectController : MonoBehaviour
    {
        [SerializeField] private GameObject blockObject;
    
        //Position
        public float _xPosition;
        public float _yPosition;
        public float _zPosition;
    
        //Children
        public List<int> childrenList;

        private void Start()
        {
            this.gameObject.transform.localPosition = new Vector3(_xPosition,_yPosition,_zPosition);
        }

        public void SetPosition(float x, float y, float z)
        {
            _xPosition = x;
            _yPosition = y;
            _zPosition = z;
        }

        public void SetChildrenList(List<int> children)
        {
            childrenList = new List<int>();
            childrenList = children;
        }

        public bool ChildrenExist()
        {
            return childrenList.Count > 0;
        }

        public void BlockerActivation(bool value)
        {
            blockObject.SetActive(value);
        }
    }
}
