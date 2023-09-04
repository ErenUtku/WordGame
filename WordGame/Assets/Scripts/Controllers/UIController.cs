using UnityEngine;

namespace Controllers
{
    public class UIController : MonoBehaviour
    {
        [Header("Panels")] 
        [SerializeField] private GameObject levelCompletePanel;
        [SerializeField] private GameObject nextButton;

        public static UIController instance;
        private void Awake()
        {
            instance = this;
        }

        public void ShowLevelCompletePanel()
        {
            nextButton.SetActive(false); //Delay Button Activation

            levelCompletePanel.SetActive(true);

            Invoke(nameof(DelayNextButton), 2f);
        }
    
        private void DelayNextButton()
        {
            nextButton.SetActive(true);
        }
    }
}
