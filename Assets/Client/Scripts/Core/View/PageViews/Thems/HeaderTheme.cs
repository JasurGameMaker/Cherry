using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class HeaderTheme : ToggleTheme
    {
        [SerializeField] private Image backgroundImage;
        
        [SerializeField] private List<Button> buttonsList;
        
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private TextMeshProUGUI userName;
        
        [SerializeField] private TextMeshProUGUI toggleLanguage;
        
        [SerializeField] private Color lightColor;
        [SerializeField] private Color darkColor;
        
        [SerializeField] private Color firstUsernameColor;
        [SerializeField] private Color secondUsernameColor;
        
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
            var mainColor = isDark ? darkColor : lightColor;
            var userNameColor = isDark ? secondUsernameColor : firstUsernameColor;
            var headerColor = isDark ? Color.white : Color.black;
            var settingsColor = isDark ? Color.white : firstUsernameColor;
            
            foreach (var toggle in buttonsList)
            {
                toggle.image.color = mainColor;
            }
            
            userName.color = userNameColor;
            header.color = headerColor;
            toggleLanguage.color = settingsColor;

            backgroundImage.color = mainColor;
            backgroundImage.SetVerticesDirty();
        }
    }
}
