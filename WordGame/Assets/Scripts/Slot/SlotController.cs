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
        
        private Dictionary<string, bool> _dictionary;

        public List<WordTile> wordTiles;

        public bool validWordFound;

        private void Awake()
        {
            TileSelector.CheckWord += CheckWordInDictionary;
        }

        private void OnDestroy()
        {
            TileSelector.CheckWord -= CheckWordInDictionary;
        }

        private void Start()
        {
            wordTiles = new List<WordTile>();
            _slotsTransform = new List<Transform>();
            
            // Load the dictionary from the "dictionary.txt" file.
            LoadDictionary();
        
            foreach (Transform child in this.gameObject.transform)
            {
                _slotsTransform.Add(child);
            }
        }

        public void TakeLetter(WordTile wordTile)
        {
            foreach (var slot in _slotsTransform)
            {
                var wordSlot = slot.GetComponent<WordSlot>();
            
                if(wordSlot.IsFull()) continue;
            
                wordSlot.FillTheSlot(true);
                wordTiles.Add(wordTile);
                wordTile.transform.SetParent(usedParent);
                wordTile.transform.position = slot.transform.position;
                
                break;
            }
        }

        public void UndoLetter()
        {
            if (wordTiles.Count <= 0) return;
            
            int lastItemIndex = wordTiles.Count - 1;
            
            wordTiles.Last().ReturnPreviousPosition();

            wordTiles.Last().TileSlotUsage(false);
            
            _slotsTransform[lastItemIndex].GetComponent<WordSlot>().FillTheSlot(false);
            
            wordTiles.RemoveAt(lastItemIndex);
            
            

            TileSelector.Instance.TriggerTileMovementAction();
            
            Debug.Log("Finish");
        }
        
        private void LoadDictionary()
        {
            TextAsset dictionaryFile = Resources.Load<TextAsset>("dictionary/dictionary");
            
            if (dictionaryFile != null)
            {
                string[] words = dictionaryFile.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                _dictionary = words.ToDictionary(word => word.ToLower(), _ => true);
            }
            else
            {
                Debug.LogError("Dictionary file not found!");
                _dictionary = new Dictionary<string, bool>();
            }
        }
        
        public void CheckWordInDictionary()
        {
            string formedWord = string.Join("", wordTiles.Select(tile => tile.tileCharacter));
            formedWord = formedWord.ToLower(); 

            if (formedWord.Length >= 2 && _dictionary.ContainsKey(formedWord))
            {
                _dictionary[formedWord] = true;
                validWordFound = true;

                AcceptButton.instance.ButtonActivation(true);
                Debug.Log("Valid word found: " + formedWord);
                return;
            }

            AcceptButton.instance.ButtonActivation(false);
            validWordFound = false;
        }

        public void ClaimWord()
        {
            if (!validWordFound) return;

            Debug.Log("SCORE HERE");
            
            
            foreach (Transform child in usedParent)
            {
                Destroy(child.gameObject);
            }
            
            AcceptButton.instance.ButtonActivation(false);
        }
        
    }
}
