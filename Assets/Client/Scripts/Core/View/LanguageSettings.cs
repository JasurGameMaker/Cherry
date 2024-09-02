using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class LanguageSettings : MonoBehaviour
    {
        [SerializeField] private UserSettings userSettings;
        [SerializeField] private Button headerButton;
        [SerializeField] private GameObject popUp;

        public GameObject PopUp => popUp;
        public bool IsClicked { get; set; }
        
        private void OnEnable()
        {
            headerButton.onClick.AddListener(OnHeaderButtonClicked);
        }

        private void OnDisable()
        {
            headerButton.onClick.RemoveListener(OnHeaderButtonClicked);
        }

        public void OnHeaderButtonClicked()
        {
            if (userSettings.PopUp.activeInHierarchy)
                userSettings.OnHeaderButtonClicked();
            
            IsClicked = !IsClicked;
            popUp.gameObject.SetActive(IsClicked);
        }
    }
}