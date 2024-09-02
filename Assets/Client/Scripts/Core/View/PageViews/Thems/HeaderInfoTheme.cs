using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class HeaderInfoTheme : ToggleTheme
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image buttonImage;
        [Header("Colors Background")] 
        [SerializeField] private Color backgroundLightColor;
        [SerializeField] private Color backgroundDarkColor;
        [Header("Colors Button")] 
        [SerializeField] private Color buttonLightColor;
        [SerializeField] private Color buttonDarkColor;
        
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
            backgroundImage.color = isDark ? backgroundDarkColor : backgroundLightColor;
            buttonImage.color = isDark ? buttonDarkColor : buttonLightColor;
        }
    }
}