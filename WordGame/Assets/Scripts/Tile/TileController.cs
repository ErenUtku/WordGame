using System;
using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tile
{
    public class TileController : MonoBehaviour
    {
        public TileData TileData { get; set;}
        public bool TileBlocked { get; set; }
        public bool TileInSlot { get; set; }
        public TileUIController TileUIController { get; private set; }
        public TileObjectController TileObjectController { get; private set; }
        
        private void Awake()
        {
            TileUIController = GetComponent<TileUIController>();
            TileObjectController = GetComponent<TileObjectController>();
        }
        
        public void Initialize(TileData tileData)
        {
            // Perform tile setup based on the provided TileData
            
            TileData = tileData; // Assign the TileData property
            InitializeUI();      // Initialize UI components
            InitializePosition(); // Initialize object position
            InitializeObject();  // Initialize object-related properties
        }

        private void InitializeUI()
        {
            TileUIController.SetTileText(TileData.character,TileData.id);
        }
        
        private void InitializePosition()
        {
            TileObjectController.SetPosition(TileData.position.x,TileData.position.y,TileData.position.z);
            TileObjectController.SetChildrenList(TileData.children);
        }
        private void InitializeObject()
        {
            TileBlocked = TileObjectController.ChildrenExist();
            TileObjectController.BlockerActivation(TileBlocked);
        }
    }
}
