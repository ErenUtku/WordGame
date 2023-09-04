using System;
using Controllers;
using Data;
using Slot;
using UnityEngine;

namespace Tile
{
    public class TileSelector : MonoBehaviour
    {
        private bool _stopTileClicking;

        public static Action TileMoved;
        public static Action CheckWord;
        
        public static TileSelector Instance;
        private void Awake()
        {
            Instance = this;

            LevelManager.OnLevelComplete += StopTileClicking;
        }

        private void OnDestroy()
        {
            LevelManager.OnLevelComplete -= StopTileClicking;
        }

        void Update()
        {
            if (_stopTileClicking) return;
            
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    TileController tileController = hit.collider.GetComponent<TileController>();

                    if (tileController.TileBlocked || tileController.TileInSlot) return;

                    if (tileController != null)
                    {
                        
                        SlotController.instance.TakeLetter(tileController);
                        
                        tileController.TileInSlot = true;
                        
                        TriggerTileMovementAction();
                        
                        CheckWord?.Invoke();
                    }
                }
            }
        }

        public void TriggerTileMovementAction()
        {
            TileMoved?.Invoke();
        }

        private void StopTileClicking(LevelData levelData)
        {
            _stopTileClicking = true;
        }

       
    }
}
