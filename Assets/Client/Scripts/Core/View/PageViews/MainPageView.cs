using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class MainPageView : BaseWindowView
    {
        [SerializeField] private Button mySubjectsButton;
        [SerializeField] private Button downloadedButton;
        [SerializeField] private Button allSubjectsButton;
        [SerializeField] private Button presentationsButton;
        [SerializeField] private Button favouritesButton;
        
        protected override void OnEnable()
        {
            mySubjectsButton.onClick.AddListener(OnMySubjectsButtonClick);
            downloadedButton.onClick.AddListener(OnDownloadedSubjectsButtonClick);
            allSubjectsButton.onClick.AddListener(OnAllSubjectsButtonClick);
            presentationsButton.onClick.AddListener(OnPresentationsButtonClick);
            favouritesButton.onClick.AddListener(OnFavouritesButtonClick);
        }

        protected override void OnDisable()
        {
            mySubjectsButton.onClick.RemoveListener(OnMySubjectsButtonClick);
            downloadedButton.onClick.RemoveListener(OnDownloadedSubjectsButtonClick);
            allSubjectsButton.onClick.RemoveListener(OnAllSubjectsButtonClick);
            presentationsButton.onClick.RemoveListener(OnPresentationsButtonClick);
            favouritesButton.onClick.RemoveListener(OnFavouritesButtonClick);
        }

        private void OnMySubjectsButtonClick()
        {
            WindowsManager.OpenWindow<AllSubjectsView>();
        }

        private void OnDownloadedSubjectsButtonClick()
        {
            WindowsManager.OpenWindow<DownloadedPageView>();
        }

        private void OnAllSubjectsButtonClick()
        {
            WindowsManager.OpenWindow<ComposedSubjectsView>();
        }
        
        private void OnPresentationsButtonClick()
        {
            WindowsManager.OpenWindow<PresentationPageView>();
        }
        
        private void OnFavouritesButtonClick()
        {
            WindowsManager.OpenWindow<FavouritesPageView>();
        }
    }
}