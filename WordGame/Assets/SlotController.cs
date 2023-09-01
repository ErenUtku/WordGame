using System;
using System.Collections;
using System.Collections.Generic;
using Tile;
using UnityEngine;

public class SlotController : MonoBehaviour
{
    public List<Transform> _slotsTransform;
    
    private void Awake()
    {
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
            wordTile.transform.position = slot.transform.position;
            break;
        }
    }
}
