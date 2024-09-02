using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class LessonsPageView : BaseWindowView
    {
        [SerializeField] private List<BaseLessonPageView> _pageViews;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _previousButton;

        [SerializeField] private Image _nextButtonImage;
        [SerializeField] private Image _previousButtonImage;
        
        private BaseLessonPageView _currentPageView;
        private BaseLessonPageView _previousPageView;

        private Color _enabledColor = new(0.1411765f, 0.1529412f, 0.1882353f, 1f);
        private Color _disabledColor = new(0.5843138f, 0.6078432f, 0.6470588f, 1f);

        private int _currentPageIndex;

        protected override void OnEnable()
        {
            base.OnEnable();
            _nextButton.onClick.AddListener(OnNextButtonClick);
            _previousButton.onClick.AddListener(OnPreviousButtonClick);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _nextButton.onClick.RemoveListener(OnNextButtonClick);
            _previousButton.onClick.RemoveListener(OnPreviousButtonClick);
        }

        private void Start()
        {
            OpenWindow<LessonPageItemView>();
            _currentPageIndex = 0;
        }

        private void OnNextButtonClick()
        {
            if (_currentPageIndex == _pageViews.Count - 1)
            {
                return;
            }

            OpenWindow(++_currentPageIndex);
            UpdateButtonView();
        }

        private void OnPreviousButtonClick()
        {
            if (_currentPageIndex == 0)
                return;

            OpenWindow(--_currentPageIndex);
            UpdateButtonView();
        }

        private void UpdateButtonView()
        {
            switch (_currentPageIndex)
            {
                case 0:
                    _nextButtonImage.color = _enabledColor;
                    _previousButtonImage.color = _disabledColor;

                    _previousButton.interactable = false;
                    break;
                case > 0:
                    _nextButtonImage.color = _enabledColor;
                    _previousButtonImage.color = _enabledColor;

                    _previousButton.interactable = true;
                    break;
            }


            if (_currentPageIndex != _pageViews.Count - 1) return;
            
            _nextButtonImage.color = _disabledColor;
            _previousButtonImage.color = _enabledColor;

            _nextButton.interactable = true;
        }

        public T GetWindow<T>() where T : BaseLessonPageView
        {
            var window = _pageViews.FirstOrDefault(w => w is T);

            if (!ReferenceEquals(window, null)) return (T)window;
            Debug.LogError("[Windows Manager] Window hasn't initialized");
            if (_pageViews.Count <= 0) return null;
            return null;
        }

        public void OpenWindow<T>() where T : BaseLessonPageView
        {
            var window = _pageViews.FirstOrDefault(w => w is T);

            if (ReferenceEquals(window, null))
            {
                Debug.LogError("[Windows Manager] Window hasn't initialized " + typeof(T).Name);
                if (_pageViews.Count <= 0) return;

                _currentPageView = _pageViews[0];
                _currentPageView.gameObject.SetActive(true);
                return;
            }

            if (!ReferenceEquals(_currentPageView, null))
            {
                _previousPageView = _currentPageView;
                _currentPageView.gameObject.SetActive(true);
            }

            CloseAlLWindows();
            window.gameObject.SetActive(true);
            _currentPageView = window;
        }

        public void OpenWindow(int index)
        {
            var window = _pageViews[index];

            if (ReferenceEquals(window, null))
            {
                Debug.LogError("[Windows Manager] Window hasn't initialized " + index);
                if (_pageViews.Count <= 0) return;

                _currentPageView = _pageViews[0];
                _currentPageView.gameObject.SetActive(true);
                return;
            }

            if (!ReferenceEquals(_currentPageView, null))
            {
                _previousPageView = _currentPageView;
                _currentPageView.gameObject.SetActive(true);
            }

            CloseAlLWindows();
            window.gameObject.SetActive(true);
            _currentPageView = window;
        }

        public void CloseAlLWindows()
        {
            foreach (var panel in _pageViews) panel.gameObject.SetActive(false);
        }
    }
}