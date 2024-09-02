using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Client
{
    public class WindowsManager : MonoBehaviour
    {
        public static WindowsManager Instance;
        
        [SerializeField] private List<BaseWindowView> _windowViews;

        private BaseWindowView _currentWindowView;
        private BaseWindowView _previousWindowView;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(Instance);
        }

        private void Start()
        {
            CloseAlLWindows();

            _currentWindowView = _windowViews[0];
            _currentWindowView.OpenImmediately();
        }

        public T GetWindow<T>() where T : BaseWindowView
        {
            var window = _windowViews.FirstOrDefault(w => w is T);

            if (!ReferenceEquals(window, null)) return (T)window;
            Debug.LogError("[Windows Manager] Window hasn't initialized");
            if (_windowViews.Count <= 0) return null;
            return null;
        }

        public void OpenWindow<T>(bool overlapsOpen = false, string overrideTable = null) where T : BaseWindowView
        {
            var window = _windowViews.FirstOrDefault(w => w is T);

            if (ReferenceEquals(window, null))
            {
                Debug.LogError("[Windows Manager] Window hasn't initialized " + typeof(T).Name);
                if (_windowViews.Count <= 0) return;

                _currentWindowView = _windowViews[0];
                _currentWindowView.Open();
                return;
            }

            if (!overlapsOpen)
            {
                if (!ReferenceEquals(_currentWindowView, null))
                {
                    _currentWindowView.Close();
                }
            }

            _previousWindowView = _currentWindowView;
            window.Open(overrideTable);
            _currentWindowView = window;
        }

        public void BackWindow()
        {
            if (ReferenceEquals(_previousWindowView, null)) return;

            _currentWindowView.Close();
            _currentWindowView = _windowViews[0];
            _currentWindowView.Open();
        }

        public void BackPreviewsWindow()
        {
            if (ReferenceEquals(_previousWindowView, null)) return;

            _currentWindowView.Close();
            _currentWindowView = _previousWindowView;
            _currentWindowView.Open();
        }

        public void CloseAlLWindows()
        {
            foreach (var panel in _windowViews) panel.CloseImmediately();
        }
    }
}