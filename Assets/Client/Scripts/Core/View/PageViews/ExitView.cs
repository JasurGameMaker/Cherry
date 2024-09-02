using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class ExitView : MonoBehaviour
    {
        [SerializeField] private Button collapseButton;
        [SerializeField] private Button minimizeButton;
        [SerializeField] private Button exitButton;

        [SerializeField] private ExitPopUpView exitPopUpView;
        
        private void OnEnable()
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnDisable()
        {
            exitButton.onClick.RemoveListener(OnExitButtonClicked);
        }

        private void OnExitButtonClicked()
        {
            exitPopUpView.gameObject.SetActive(true);
        }
    }
}