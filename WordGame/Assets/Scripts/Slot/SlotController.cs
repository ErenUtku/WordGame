using System.Collections.Generic;
using System.Linq;
using Controllers;
using Data;
using Tile;
using UnityEngine;
using Utils;

namespace Slot
{
    public class SlotController : MonoBehaviour
    {
        [Header("Parents")] 
        [SerializeField] private Transform slotParent;
        [SerializeField] private Transform unusedParent;
        [SerializeField] private Transform usedParent;
        
        private List<Transform> _slotsTransform;
        private List<TileController> _allTileControllers;
        
        private DataManager _dataManager;
        private ScoringSystem _scoringSystem;
        private WordChecker _wordChecker;
        
        private readonly List<string> _claimedWords = new List<string>();
        private string _formedWord;
        public bool validWordFound;
        
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
            _allTileControllers = new List<TileController>();
            _slotsTransform = new List<Transform>();
            _scoringSystem = new ScoringSystem();
            _dataManager = DataManager.Instance;

            _wordChecker = new WordChecker(_dataManager.Dictionary);
        
            foreach (Transform child in slotParent.transform)
            {
                _slotsTransform.Add(child);
            }
        }

        public void TakeLetter(TileController tileController)
        {
            foreach (var slot in _slotsTransform)
            {
                var wordSlot = slot.GetComponent<WordSlot>();
            
                if(wordSlot.IsFull()) continue;
            
                wordSlot.FillTheSlot(true);
                _allTileControllers.Add(tileController);
                tileController.transform.SetParent(usedParent);
                tileController.transform.position = slot.transform.position;
                
                break;
            }

            CheckRemainingWord();
        }

        public void UndoLetter()
        {
            if (_allTileControllers.Count <= 0) return;
            
            int lastItemIndex = _allTileControllers.Count - 1;
            
            _allTileControllers.Last().TileObjectController.ReturnPreviousPosition();

            _allTileControllers.Last().TileInSlot = false;
            
            _slotsTransform[lastItemIndex].GetComponent<WordSlot>().FillTheSlot(false);
            
            TileSelector.Instance.TriggerTileMovementAction(_allTileControllers.Last());
            
            _allTileControllers.RemoveAt(lastItemIndex);
            
        }
        
        public void ClaimWord()
        {
            if (!validWordFound) return;

            foreach (Transform child in usedParent)
            {
                Destroy(child.gameObject);
            }

            foreach (var slot in _slotsTransform)
            {
                var wordSlot = slot.GetComponent<WordSlot>();
                wordSlot.FillTheSlot(false);
            }

            _allTileControllers.Clear();
            
            _claimedWords.Add(_formedWord);

            GameUIButtonController.instance.ButtonActivation(false,ButtonType.Accept);
            
            
        }
        
        private void CheckWordInDictionary()
        {
            _formedWord = string.Join("", _allTileControllers.Select(tile => tile.TileData.character));

            if (_wordChecker.IsWordValid(_formedWord))
            {
                validWordFound = true;
                GameUIButtonController.instance.ButtonActivation(true, ButtonType.Accept);
            }
            else
            {
                validWordFound = false;
                GameUIButtonController.instance.ButtonActivation(false, ButtonType.Accept);
            }
        }

        private void CheckRemainingWord()
        {
            List<Data.TileData> allTiles = GetAllTiles();

            allTiles.OrderBy(tile => tile.children.Count).ToList();

            RemainingTiles remainingTiles = new RemainingTiles(DataManager.Instance.GetLevelData(), allTiles, DataManager.Instance.Dictionary);
            var validWords = remainingTiles.FindWords();

            if (validWords.Count == 0)
            {
                HandleLevelCompletion();
            }
        }

        private List<Data.TileData> GetAllTiles()
        {
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

            return allTiles;
        }
        
        private void HandleLevelCompletion()
        {
            ScoreController.Instance.SetLevelScore(_claimedWords, unusedParent.transform.childCount);
            
            LevelManager.Instance.LevelComplete();
            
        }

    }
}
