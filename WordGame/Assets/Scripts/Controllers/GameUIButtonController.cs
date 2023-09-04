using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Controllers
{
    public class GameUIButtonController : MonoBehaviour
    {
        [SerializeField] private Button acceptButton;
        [SerializeField] private Button undoButton;

        public static Action<bool, ButtonType> ButtonBehavior;

        private void Awake()
        {
            ButtonBehavior += ButtonActivation;
        }
        
        private void OnDestroy()
        {
            ButtonBehavior -= ButtonActivation;
        }

        private void Start()
        {
            //First Time
            ButtonActivation(false,ButtonType.Accept);
            ButtonActivation(false,ButtonType.Undo);
        }

        private void ButtonActivation(bool value,ButtonType buttonType)
        {
            switch (buttonType)
            {
                case ButtonType.Accept:
                    acceptButton.interactable = value;
                    break;
                case ButtonType.Undo:
                    undoButton.interactable = value;
                    break;
            }
        }
    }

    public enum ButtonType
    {
        Accept,
        Undo
    }
}