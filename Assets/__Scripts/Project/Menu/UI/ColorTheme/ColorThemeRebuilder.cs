using __Scripts.Project.Data;
using __Scripts.Project.Data.Enums;
using Client;
using UnityEngine;
using Zenject;

namespace __Scripts.Project.Menu.UI.ColorTheme
{
    public class ColorThemeRebuilder : MonoBehaviour
    {
        [SerializeField] private ToggleSwitch mainToggleSwitche;
        [SerializeField] private ToggleSwitchMaterialChange toggleSwitch;
        
        private MenuState _menuState;

        [Inject]
        private void Construct(MenuState menuState)
        {
            _menuState = menuState;
        }

        private void Start()
        {
            mainToggleSwitche.ToggleByGroupManager(_menuState.GetSavedTheme() == ThemeColor.Dark);
            toggleSwitch.ToggleByGroupManager(_menuState.GetSavedTheme() == ThemeColor.Dark);
        }

        private void OnEnable()
        {
            mainToggleSwitche.onToggleOn.AddListener(OnToggleOn);
            mainToggleSwitche.onToggleOff.AddListener(OnToggleOff);
        }

        private void OnDisable()
        {
            mainToggleSwitche.onToggleOn.RemoveListener(OnToggleOn);
            mainToggleSwitche.onToggleOff.RemoveListener(OnToggleOff);
        }

        private void OnToggleOn()
        {
            _menuState.ColorTheme.Value = ThemeColor.Dark;
            _menuState.Save();
        }

        private void OnToggleOff()
        {
            _menuState.ColorTheme.Value = ThemeColor.Light;
            _menuState.Save();
        }
    }
}
