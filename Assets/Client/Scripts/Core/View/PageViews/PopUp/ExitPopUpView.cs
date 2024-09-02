using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class ExitPopUpView : MonoBehaviour
    {
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button closeViewButton;

        private void OnEnable()
        {
            cancelButton.onClick.AddListener(OnCancelButtonClicked);
            exitButton.onClick.AddListener(OnExitButtonClicked);
            closeViewButton.onClick.AddListener(OnCancelButtonClicked);
        }

        private void OnDisable()
        {
            cancelButton.onClick.RemoveListener(OnCancelButtonClicked);
            exitButton.onClick.RemoveListener(OnExitButtonClicked);
            closeViewButton.onClick.RemoveListener(OnCancelButtonClicked);
        }

        private void OnCancelButtonClicked()
        {
            gameObject.SetActive(false);
        }
        
        private void OnExitButtonClicked()
        {
            Application.Quit();
        }
    }
}
