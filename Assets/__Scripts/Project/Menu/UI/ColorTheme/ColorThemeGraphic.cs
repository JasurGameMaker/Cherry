using System;
using __Scripts.Project.Data;
using Doozy.Runtime.UIManager.Animators;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace __Scripts.Project.Menu.UI.ColorTheme
{
    public class ColorThemeGraphic : MonoBehaviour
    {
        [SerializeField] private Graphic graphic;
        [Space]
        [SerializeField] private Data.Enums.ThemeColor overrideWithDefault;

        [SerializeField] private UIToggleColorAnimator toggleColorAnimator;
        [SerializeField] private UISelectableColorAnimator colorAnimator;
        
        [SerializeField] private Color lightColor;
        [SerializeField] private Color darkColor;
        
        private MenuState _menuState;

        [Inject]
        private void Construct(MenuState menuState)
        {
            _menuState = menuState;
        }

        private void Awake()
        {
            switch (overrideWithDefault)
            {
                case Data.Enums.ThemeColor.Light:
                    lightColor = graphic.color;
                    break;
                case Data.Enums.ThemeColor.Dark:
                    darkColor = graphic.color;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _menuState.ColorTheme.ValueChanged += OnColorThemeChanged;
        }
        
        private void OnDestroy() =>
            _menuState.ColorTheme.ValueChanged -= OnColorThemeChanged;
        
        private void OnColorThemeChanged(Data.Enums.ThemeColor theme)
        {
            switch (theme)
            {
                case Data.Enums.ThemeColor.Light:
                    graphic.color = lightColor;
                    break;
                case Data.Enums.ThemeColor.Dark:
                    graphic.color = darkColor;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(theme), theme, null);
            }
            
            colorAnimator?.SetStartColor(graphic.color);
            toggleColorAnimator?.SetStartColor(graphic.color);
        }
    }
}
