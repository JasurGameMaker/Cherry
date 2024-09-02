using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class SaveButton : MonoBehaviour
    {
        [field: SerializeField] public Button Button { get; set; }
        [field: SerializeField] public Image BackgroundImage { get; set; }
        [field: SerializeField] public Image Icon { get; set; }
        [field: SerializeField] public TextMeshProUGUI SaveTMP { get; set; }
        
        [SerializeField] private MainPanelTheme mainPanelTheme;
        
        public bool IsSaveSelected { get; private set; }

        private void OnEnable()
        {
            Button.onClick.AddListener(OnChooseButtonClicked);
        }
        
        private void OnDisable()
        {
            Button.onClick.RemoveListener(OnChooseButtonClicked);
            DisableVisual();
        }

        private void OnChooseButtonClicked()
        {
            IsSaveSelected = !IsSaveSelected;
            BackgroundImage.sprite = IsSaveSelected ? mainPanelTheme.SelectedSprite : mainPanelTheme.DefaultSprite;
            Icon.color = IsSaveSelected && !mainPanelTheme.IsDark ? Color.black : Color.white;
            SaveTMP.color = IsSaveSelected && !mainPanelTheme.IsDark ? Color.black : Color.white;
            WindowsManager.Instance.OpenWindow<DownloadedPageView>();
        }
        
        private void DisableVisual()
        {
            IsSaveSelected = false;
            BackgroundImage.sprite = mainPanelTheme.DefaultSprite;
            Icon.color = Color.white;
            SaveTMP.color = Color.white;
        }
    }
}