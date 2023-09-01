using TMPro;
using UnityEngine;

namespace Tile
{
    public class TileUIController : MonoBehaviour
    {
        [SerializeField] private TextMeshPro textUI;

        public void SetTileText(string character)
        {
            this.gameObject.name = character;
            textUI.text = character;
        }
    }
}
