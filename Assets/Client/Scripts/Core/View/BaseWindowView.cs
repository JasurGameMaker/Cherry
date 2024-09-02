using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace Client
{
    public abstract class BaseWindowView : MonoBehaviour
    {
        [field: SerializeField] protected string ViewLabelText;
        [field: SerializeField] public SubjectPopUpView subjectPopUpView;
        
        [SerializeField] private TextMeshProUGUI labelTMP;
        [SerializeField] private Button _backButton;
        
        public event Action<BaseWindowView> WindowOpened;
        public event Action<BaseWindowView> WindowClosed;

        public event Action BackButtonPressed;

        protected Button BackButton => _backButton;

        public WindowsManager WindowsManager { get; private set; }
        
        private LocalizeStringEvent _viewLabelText;

        protected virtual void Awake()
        {
            WindowsManager = GetComponentInParent<WindowsManager>();
            _viewLabelText = labelTMP.GetComponent<LocalizeStringEvent>();
        }

        protected virtual void OnEnable()
        {
            if (ReferenceEquals(_backButton, null))
                return;

            _backButton.onClick.AddListener(Back);
        }

        protected virtual void OnDisable()
        {
            if (ReferenceEquals(_backButton, null))
                return;

            _backButton.onClick.RemoveListener(Back);
        }

        public void Open(string overrideTable = null)
        {
            gameObject.SetActive(true);
            _viewLabelText.SetTable(overrideTable ?? "Main Menu");
            _viewLabelText.SetEntry(ViewLabelText);
            WindowOpened?.Invoke(this);
        }

        public void Close()
        {
            WindowClosed?.Invoke(this);
            gameObject.SetActive(false);
        }

        private void Back()
        {
            WindowsManager.BackPreviewsWindow();
            BackButtonPressed?.Invoke();
        }

        public void OpenImmediately()
        {
            gameObject.SetActive(true);
            WindowOpened?.Invoke(this);
        }

        public void CloseImmediately()
        {
            WindowClosed?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}