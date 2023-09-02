using TMPro;
using UnityEngine;

namespace Tile
{
    public class TileUIController : MonoBehaviour
    {
        [SerializeField] private TextMeshPro textUI;

        public void SetTileText(string character, int tileId)
        {
            this.gameObject.name = tileId.ToString();
            textUI.text = character;
        }
    }
}
