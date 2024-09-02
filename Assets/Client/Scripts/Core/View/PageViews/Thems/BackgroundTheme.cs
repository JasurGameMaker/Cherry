using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class BackgroundTheme : ToggleTheme
    {
        [SerializeField] private UIGradient gradientColor;
        [SerializeField] private Image backgroundImage;
        
        [SerializeField] private Color firstLightThemeGradient;
        [SerializeField] private Color secondLightThemeGradient;
        
        [SerializeField] private Color firstDarkThemeGradient;
        [SerializeField] private Color secondDarkThemeGradient;
        
        private void Start()
        {
            if (toggleSwitch == null) 
                return;
            
            SetTheme(toggleSwitch.CurrentValue);
        }
        
        private void OnEnable()
        {
            toggleSwitch.onToggleOn.AddListener(() => SetTheme(true));
            toggleSwitch.onToggleOff.AddListener(() => SetTheme(false));
        }

        private void OnDisable()
        {
            toggleSwitch.onToggleOn.RemoveListener(() => SetTheme(true));
            toggleSwitch.onToggleOff.RemoveListener(() => SetTheme(false));
        }

        public override void SetTheme(bool isDark)
        {
            var topGradientColor = isDark ? firstDarkThemeGradient : firstLightThemeGradient;
            var bottomGradientColor = isDark ? secondDarkThemeGradient : secondLightThemeGradient;
            
            gradientColor.m_color1 = topGradientColor;
            gradientColor.m_color2 = bottomGradientColor;
            
            backgroundImage.SetVerticesDirty();
        }
    }
}