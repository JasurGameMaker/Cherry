using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class ForegroundTheme : ToggleTheme
    {
        [SerializeField] private Image backgroundImage;
        
        [SerializeField] private Color lightThemeColor;
        [SerializeField] private Color darkThemeColor;
        
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
            var backgroundColor = isDark ? darkThemeColor : lightThemeColor;
            backgroundImage.color = backgroundColor;
            backgroundImage.SetVerticesDirty();
        }
    }
}