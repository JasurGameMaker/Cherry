using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class UserSettingsTheme : ToggleTheme
    {
        [SerializeField] private List<TextMeshProUGUI> bodyTextList = new();
        [SerializeField] private Image backgroundImage;
        [Header("Colors")] 
        [SerializeField] private Color lightColor;
        [SerializeField] private Color darkColor;
        
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
            backgroundImage.color = isDark ? darkColor : lightColor;
            foreach (var text in bodyTextList)
            {
                text.color = isDark ? Color.white : Color.black;
            }
        }
    }
}
