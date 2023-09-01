using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
