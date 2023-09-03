using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tile;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Slot
{
    public class SlotController : MonoBehaviour
    {
        [SerializeField] private Transform unusedParent;
        [SerializeField] private Transform usedParent;
        public List<Transform> _slotsTransform;

        public List<WordTile> wordTiles;

        public bool validWordFound;

        private DataManager _dataManager;
        
        private ScoringSystem scoringSystem;
        
        private List<string> claimedWords = new List<string>();

        private string formedWord;
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

            scoringSystem = new ScoringSystem();

            _dataManager = DataManager.Instance;
        
            foreach (Transform child in this.gameObject.transform)
            {
                _slotsTransform.Add(child);
            }
            
            //test
            CheckRemainingWord();
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

            CheckRemainingWord();
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
        
        
        public void CheckWordInDictionary()
        {
            formedWord = string.Join("", wordTiles.Select(tile => tile.tileCharacter));
            formedWord = formedWord.ToLower(); 

            if (formedWord.Length >= 2 && _dataManager.Dictionary.ContainsKey(formedWord))
            {
                _dataManager.Dictionary[formedWord] = true;
                
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

            foreach (var slot in _slotsTransform)
            {
                var wordSlot = slot.GetComponent<WordSlot>();
                wordSlot.FillTheSlot(false);
            }

            wordTiles.Clear();
            
            claimedWords.Add(formedWord);

            AcceptButton.instance.ButtonActivation(false);
            
            
        }

        private void CheckRemainingWord()
        {
            // Get the list of unused tiles from unusedParent.
            List<Data.TileData> allTiles = new List<Data.TileData>();
            foreach (Transform child in unusedParent)
            {
                WordTile wordTile = child.GetComponent<WordTile>();
                if (wordTile != null)
                {
                    allTiles.Add(wordTile.tileData);
                }
            }

            foreach (Transform child in usedParent)
            {
                WordTile wordTile = child.GetComponent<WordTile>();
                if (wordTile != null)
                {
                    allTiles.Add(wordTile.tileData);
                }
            }
            
            allTiles.OrderBy(tile => tile.children.Count).ToList();

            RemainingTiles remainingTiles = new RemainingTiles(DataManager.Instance.GetLevelData(),allTiles, DataManager.Instance.Dictionary);
            var validWords = remainingTiles.FindWords();
            Debug.Log(validWords);

        }
    }
}
