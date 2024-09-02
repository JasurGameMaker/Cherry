using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class UserSettings : MonoBehaviour
    {
        [SerializeField] private LanguageSettings languageSettings;
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
            if (languageSettings.PopUp.activeInHierarchy)
                languageSettings.OnHeaderButtonClicked();

            IsClicked = !IsClicked;
            popUp.gameObject.SetActive(IsClicked);
        }
    }
}