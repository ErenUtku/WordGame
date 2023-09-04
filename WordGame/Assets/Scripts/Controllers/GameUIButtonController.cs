using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class GameUIButtonController : MonoBehaviour
    {
        [SerializeField] private Button acceptButton;
        [SerializeField] private Button reverseButton;
        public static GameUIButtonController instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            //First Time
            ButtonActivation(false,ButtonType.Accept);
        }

        public void ButtonActivation(bool value,ButtonType buttonType)
        {
            switch (buttonType)
            {
                case ButtonType.Accept:
                    acceptButton.interactable = value;
                    break;
                case ButtonType.Reverse:
                    //TODO
                    break;
            }
        
        }
    }

    public enum ButtonType
    {
        Accept,
        Reverse
    }
}