using System;
using UnityEngine.UI;
using UnityEngine;

public class AcceptButton : MonoBehaviour
{
    private Button _button;
    public static AcceptButton instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _button = this.GetComponent<Button>();

        //First Time
        ButtonActivation(false);

    }

    public void ButtonActivation(bool value)
    {
        _button.interactable = value;
    }
}
