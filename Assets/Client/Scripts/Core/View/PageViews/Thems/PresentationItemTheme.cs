using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Client
{
    public class PresentationItemTheme : ToggleTheme
    {
        [Header("Colors")]
        [SerializeField] private Color backgroundImageColorLight;
        [SerializeField] private Color backgroundImageColorDark;
        
        [SerializeField] private Image backgroundImage;
        
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
            backgroundImage.color = isDark ? backgroundImageColorDark : backgroundImageColorLight;
        }
    }
}