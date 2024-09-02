using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class LeftBarTheme : ToggleTheme
    {
        [SerializeField] private List<Image> imagesList = new ();
        [SerializeField] private List<ButtonClickHandler> buttonsList = new();

        [SerializeField] private Color lightColor;
        [SerializeField] private Color darkColor;

        private bool _isDark;

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
            SubscribeHandlers();
        }

        private void OnDisable()
        {
            toggleSwitch.onToggleOn.RemoveListener(() => SetTheme(true));
            toggleSwitch.onToggleOff.RemoveListener(() => SetTheme(false));
            UnsubscribeHandlers();
        }

        public override void SetTheme(bool isDark)
        {
            _isDark = isDark;
            SetImagesTheme(isDark);
        }

        private void SetImagesTheme(bool isDark)
        {
            var imageColor = isDark ? darkColor : lightColor;

            foreach (var image in imagesList)
            {
                image.color = imageColor;
            }
        }

        private void SubscribeHandlers()
        {
            foreach (var handler in buttonsList)
            {
                handler.ButtonClicked += SetButtonColor;
            }
        }
        
        private void UnsubscribeHandlers()
        {
            foreach (var handler in buttonsList)
            {
                handler.ButtonClicked -= SetButtonColor;
            }
        }
        
        private void SetButtonColor(ButtonClickHandler handler)
        {
            UpdateButtonColor(handler);
        }

        public void UpdateButtonColor(ButtonClickHandler handler)
        {
            handler.CurrentButton.image.color = _isDark ? handler.darkColor : handler.lightColor;

            if (_isDark && handler.Outline.enabled)
            {
                foreach (var buttonHandler in buttonsList)
                {
                    buttonHandler.CurrentButton.image.color = handler.darkColor;
                }
                handler.CurrentButton.image.color = Color.white;
            }
            else
            {
                foreach (var buttonHandler in buttonsList)
                {
                    buttonHandler.CurrentButton.image.color = handler.lightColor;
                }
                handler.CurrentButton.image.color = Color.black;
            }
        }
    }
}