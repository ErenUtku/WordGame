using System;
using Slot;
using UnityEngine;

namespace Tile
{
    public class TileSelector : MonoBehaviour
    {

        [SerializeField] private Transform usedTileParent;
        [SerializeField] private SlotController slotController;

        public static Action TileMoved;
        public static TileSelector Instance;
        public static Action CheckWord;
        private void Awake()
        {
            Instance = this;
        }

        void Update()
        {
        
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    WordTile tile = hit.collider.GetComponent<WordTile>();

                    if (tile.IsTileBlocked() || tile.IsTileInSlot()) return;

                    if (tile != null)
                    {
                        string tileName = tile.tileCharacter;

                        slotController.TakeLetter(tile);

                        TriggerTileMovementAction();
                        
                        tile.TileSlotUsage(true);
                        
                        CheckWord?.Invoke();
                        
                        Debug.Log("Clicked Tile Name: " + tileName);
                    }
                }
            }
        }

        public void TriggerTileMovementAction()
        {
            TileMoved?.Invoke();
        }

       
    }
}
