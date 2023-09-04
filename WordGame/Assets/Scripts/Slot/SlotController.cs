using System;
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
        private WordChecker _wordChecker;
        
        private readonly List<string> _claimedWords = new List<string>();
        private string _formedWord;
        public bool validWordFound;
        
        public int CurrentHighScore { get; set; }
        public int PreviousHighScore { get; set; }
        
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
            
                GameUIButtonController.ButtonBehavior(true,ButtonType.Undo);
                
                wordSlot.FillTheSlot(true);
                
                _allTileControllers.Add(tileController);
                
                //TilePositioning
                tileController.transform.SetParent(usedParent);
                tileController.transform.position = slot.transform.position;
                
                break;
            }

        }

        public void UndoLetter()
        {
            int lastItemIndex = _allTileControllers.Count - 1;
            
            _allTileControllers.Last().TileObjectController.ReturnPreviousPosition();

            _allTileControllers.Last().TileInSlot = false;
            
            _slotsTransform[lastItemIndex].GetComponent<WordSlot>().FillTheSlot(false);

            _allTileControllers.RemoveAt(lastItemIndex);
            
            TileSelector.Instance.TriggerTileMovementAction();

            TileSelector.Instance.TriggerWordCheckerAction();

            if (_allTileControllers.Count <= 0)
            {
                GameUIButtonController.ButtonBehavior(false,ButtonType.Undo);
            }
            
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

            GameUIButtonController.ButtonBehavior(false,ButtonType.Accept);

            CheckRemainingWord();
        }
        
        private void CheckWordInDictionary()
        {
            _formedWord = string.Join("", _allTileControllers.Select(tile => tile.TileData.character));

            if (_wordChecker.IsWordValid(_formedWord))
            {
                validWordFound = true;
                GameUIButtonController.ButtonBehavior(true,ButtonType.Accept);
            }
            else
            {
                validWordFound = false;
                GameUIButtonController.ButtonBehavior(false,ButtonType.Accept);
            }
        }

        private void CheckRemainingWord()
        {
            List<TileData> allTilesData = new List<TileData>();

            GetChildOfParent(allTilesData, WordParentPile.Unused);

            allTilesData = allTilesData.OrderBy(tileData => tileData.children.Count).ToList();

            RemainingTiles remainingTiles = new RemainingTiles(DataManager.Instance.GetLevelData(), allTilesData, DataManager.Instance.Dictionary);
            var validWords = remainingTiles.FindWords();

            if (validWords.Count == 0)
            {
                HandleLevelCompletion();
            }
        }

        public void GetChildOfParent(List<TileData> childList, WordParentPile parentType)
        {
            Transform parentTransform = null;

            switch (parentType)
            {
                case WordParentPile.Used:
                    parentTransform = usedParent;
                    break;
                case WordParentPile.Unused:
                    parentTransform = unusedParent;
                    break;
            }

            foreach (Transform child in parentTransform)
            {
                TileController tileController = child.GetComponent<TileController>();
                if (tileController != null)
                {
                    childList.Add(tileController.TileData);
                }
            }
        }
        
        private void HandleLevelCompletion()
        {
            PreviousHighScore =DataManager.Instance.HighScoreManager.GetHighScore(DataManager.Instance.GetLevelIndex());
            CurrentHighScore = ScoreController.Instance.GetLevelScore(_claimedWords, unusedParent.transform.childCount);
            LevelManager.Instance.LevelComplete();
            ScoreController.Instance.SetLevelScore(CurrentHighScore);
        }

    }
}

public enum WordParentPile
{
    Used,
    Unused
}
