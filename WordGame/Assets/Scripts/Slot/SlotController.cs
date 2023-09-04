using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Data;
using Tile;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

namespace Slot
{
    public class SlotController : MonoBehaviour
    {
        [SerializeField] private Transform unusedParent;
        [SerializeField] private Transform usedParent;
        public List<Transform> _slotsTransform;

        public List<TileController> allTileControllers;

        public bool validWordFound;

        private DataManager _dataManager;
        
        private ScoringSystem scoringSystem;
        
        private List<string> claimedWords = new List<string>();

        private string formedWord;

        public static SlotController instance;
        private void Awake()
        {
            instance = this;
            TileSelector.CheckWord += CheckWordInDictionary;
            
        }

        private void OnDestroy()
        {
            TileSelector.CheckWord -= CheckWordInDictionary;
        }

        private void Start()
        {
            allTileControllers = new List<TileController>();
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

        public void TakeLetter(TileController tileController)
        {
            foreach (var slot in _slotsTransform)
            {
                var wordSlot = slot.GetComponent<WordSlot>();
            
                if(wordSlot.IsFull()) continue;
            
                wordSlot.FillTheSlot(true);
                allTileControllers.Add(tileController);
                tileController.transform.SetParent(usedParent);
                tileController.transform.position = slot.transform.position;
                
                break;
            }

            CheckRemainingWord();
        }

        public void UndoLetter()
        {
            if (allTileControllers.Count <= 0) return;
            
            int lastItemIndex = allTileControllers.Count - 1;
            
            allTileControllers.Last().TileObjectController.ReturnPreviousPosition();

            allTileControllers.Last().TileInSlot = false;
            
            _slotsTransform[lastItemIndex].GetComponent<WordSlot>().FillTheSlot(false);
            
            TileSelector.Instance.TriggerTileMovementAction(allTileControllers.Last());
            
            allTileControllers.RemoveAt(lastItemIndex);
            
        }
        
        
        public void CheckWordInDictionary()
        {
            formedWord = string.Join("", allTileControllers.Select(tile => tile.TileData.character));
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

            allTileControllers.Clear();
            
            claimedWords.Add(formedWord);

            AcceptButton.instance.ButtonActivation(false);
            
            
        }

        private void CheckRemainingWord()
        {
            // Get the list of unused tiles from unusedParent.
            List<Data.TileData> allTiles = new List<Data.TileData>();
            foreach (Transform child in unusedParent)
            {
                TileController tileController = child.GetComponent<TileController>();
                if (tileController != null)
                {
                    allTiles.Add(tileController.TileData);
                }
            }

            foreach (Transform child in usedParent)
            {
                TileController tileController = child.GetComponent<TileController>();
                if (tileController != null)
                {
                    allTiles.Add(tileController.TileData);
                }
            }
            
            allTiles.OrderBy(tile => tile.children.Count).ToList();

            RemainingTiles remainingTiles = new RemainingTiles(DataManager.Instance.GetLevelData(),allTiles, DataManager.Instance.Dictionary);
            var validWords = remainingTiles.FindWords();

            
            if (validWords.Count <= 0)
            {
                var totalScore = scoringSystem.CalculateTotalScore(claimedWords,unusedParent.transform.childCount);

                _dataManager.HighScoreManager.SetHighScore(DataManager.Instance.GetLevelIndex(), totalScore);
                
                DataManager.Instance.SetLevel(DataManager.Instance.GetLevelIndex()+1);

                UIController.instance.ShowLevelCompletePanel();

            }
            
        }
    }
}
