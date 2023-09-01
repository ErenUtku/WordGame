using System;
using System.Collections.Generic;
using System.Linq;
using Tile;
using UnityEngine;

namespace Slot
{
    public class SlotController : MonoBehaviour
    {
        [SerializeField] private Transform usedParent;
        public List<Transform> _slotsTransform;

        public List<WordTile> wordTiles;
        
        private void Start()
        {
            wordTiles = new List<WordTile>();
            _slotsTransform = new List<Transform>();
        
            foreach (Transform child in this.gameObject.transform)
            {
                _slotsTransform.Add(child);
            }
        }

        public void AcceptLetter(WordTile wordTile)
        {
            foreach (var slot in _slotsTransform)
            {
                var wordSlot = slot.GetComponent<WordSlot>();
            
                if(wordSlot.IsFull()) continue;
            
                wordSlot.FillTheSlot(true);
                wordTiles.Add(wordTile);
                //wordTile.transform.SetParent(usedParent);
                wordTile.transform.position = slot.transform.position;
                
                break;
            }
        }

        public void UndoLetter()
        {
            if (wordTiles.Count <= 0) return;
            
            int lastItemIndex = wordTiles.Count - 1;
            
            wordTiles.Last().ReturnPreviousPosition();
            
            _slotsTransform[lastItemIndex].GetComponent<WordSlot>().FillTheSlot(false);
            
            
            wordTiles.RemoveAt(lastItemIndex);
        }
    }
}
