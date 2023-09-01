using Slot;
using UnityEngine;

namespace Tile
{
    public class TileSelector : MonoBehaviour
    {

        [SerializeField] private Transform usedTileParent;
        [SerializeField] private SlotController slotController;
        void Update()
        {
        
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    WordTile tile = hit.collider.GetComponent<WordTile>();

                    if (tile != null)
                    {
                        string tileName = tile.tileCharacter;

                        slotController.AcceptLetter(tile);
                        Debug.Log("Clicked Tile Name: " + tileName);
                    }
                }
            }
        }
    }
}
