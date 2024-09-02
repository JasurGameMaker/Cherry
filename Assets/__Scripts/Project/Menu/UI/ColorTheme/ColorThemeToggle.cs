using System;
using __Scripts.Project.Data;
using __Scripts.Project.Menu.UI.Utils;
using Doozy.Runtime.UIManager.Components;
using Zenject;

namespace __Scripts.Project.Menu.UI.ColorTheme
{
    public class ColorThemeToggle : DoozyToggleListener
    {
        private MenuState _menuState;

        [Inject]
        private void Construct(MenuState menuState)
        {
            _menuState = menuState;
        }

        private void Start() =>
            Toggle.SetIsOn(_menuState.GetSavedTheme() == Data.Enums.ThemeColor.Light, false, true);

        protected override void OnToggle(bool state)
        {
            _menuState.ColorTheme.Value = state ? Data.Enums.ThemeColor.Light : Data.Enums.ThemeColor.Dark;
            _menuState.Save();
        }
    }
}
