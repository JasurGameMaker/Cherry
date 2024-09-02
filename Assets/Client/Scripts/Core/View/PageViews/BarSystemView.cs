using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class BarSystemView : MonoBehaviour
    {
        [SerializeField] private WindowsManager windowsManager;

        [SerializeField] private LeftBarTheme leftBarTheme;
        [SerializeField] private ButtonClickHandler homeButton;
        [SerializeField] private ButtonClickHandler favouritesButton;
        [SerializeField] private ButtonClickHandler downloadedButton;
        [SerializeField] private ButtonClickHandler presentationsButton;

        [SerializeField] private List<Image> _imagesList = new ();

        private void OnEnable()
        {
            homeButton.CurrentButton.onClick.AddListener(OnHomeButtonClick);
            favouritesButton.CurrentButton.onClick.AddListener(OnFavouritesButtonClick);
            downloadedButton.CurrentButton.onClick.AddListener(OnDownloadedButtonClick);
            presentationsButton.CurrentButton.onClick.AddListener(OnPresentationsButtonClick);

            windowsManager.GetWindow<MainPageView>().WindowOpened += OnMainView;
            windowsManager.GetWindow<FavouritesPageView>().WindowOpened += OnFavouriteView;
            windowsManager.GetWindow<DownloadedPageView>().WindowOpened += OnDownloadView;
            windowsManager.GetWindow<PresentationPageView>().WindowOpened += OnPresentationView;
        }

        private void OnDisable()
        {
            homeButton.CurrentButton.onClick.RemoveListener(OnHomeButtonClick);
            favouritesButton.CurrentButton.onClick.RemoveListener(OnFavouritesButtonClick);
            downloadedButton.CurrentButton.onClick.RemoveListener(OnDownloadedButtonClick);
            presentationsButton.CurrentButton.onClick.RemoveListener(OnPresentationsButtonClick);
            
            windowsManager.GetWindow<MainPageView>().WindowOpened -= OnMainView;
            windowsManager.GetWindow<FavouritesPageView>().WindowOpened -= OnFavouriteView;
            windowsManager.GetWindow<DownloadedPageView>().WindowOpened -= OnDownloadView;
            windowsManager.GetWindow<PresentationPageView>().WindowOpened -= OnPresentationView;
        }

        private void OnMainView(BaseWindowView baseWindowView)
        {
            EnableButtonOutline(homeButton.CurrentButton, true);
            leftBarTheme.UpdateButtonColor(homeButton);
            homeButton.SetButtonColor();
        }
        
        private void OnFavouriteView(BaseWindowView baseWindowView)
        {
            EnableButtonOutline(favouritesButton.CurrentButton, true);
            leftBarTheme.UpdateButtonColor(favouritesButton);
            favouritesButton.SetButtonColor();
        }
        
        private void OnDownloadView(BaseWindowView baseWindowView)
        {
            EnableButtonOutline(downloadedButton.CurrentButton, true);
            leftBarTheme.UpdateButtonColor(downloadedButton);
            downloadedButton.SetButtonColor();
        }
        
        private void OnPresentationView(BaseWindowView baseWindowView)
        {
            EnableButtonOutline(presentationsButton.CurrentButton, true);
            leftBarTheme.UpdateButtonColor(presentationsButton);
            presentationsButton.SetButtonColor();
        }

        private void OnHomeButtonClick()
        {
            EnableButtonOutline(homeButton.CurrentButton, true);
            windowsManager.OpenWindow<MainPageView>();
        }

        private void OnFavouritesButtonClick()
        {
            EnableButtonOutline(favouritesButton.CurrentButton, true);
            windowsManager.OpenWindow<FavouritesPageView>();
        }

        private void OnDownloadedButtonClick()
        {
            EnableButtonOutline(downloadedButton.CurrentButton, true);
            windowsManager.OpenWindow<DownloadedPageView>();
        }
        
        private void OnPresentationsButtonClick()
        {
            EnableButtonOutline(presentationsButton.CurrentButton, true);
            windowsManager.OpenWindow<PresentationPageView>();
        }

        private void EnableButtonOutline(Button currentButton, bool isActive)
        {
            foreach (var image in _imagesList)
            {
                var currentImage = currentButton.transform.parent.GetComponent<Image>();
                image.enabled = !isActive;
                if (image.Equals(currentImage))
                    currentImage.enabled = isActive;
            }
        }
    }
}