using UnityEngine;

namespace Slot
{
    public class WordSlot : MonoBehaviour
    {
        [SerializeField] private bool isFull;

        public void FillTheSlot(bool value)
        {
            isFull = value;
        }

        public bool IsFull()
        {
            return isFull;
        }
    }
}
