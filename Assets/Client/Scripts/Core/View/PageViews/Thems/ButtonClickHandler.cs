using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class ButtonClickHandler : ToggleTheme, IPointerClickHandler
    {
        public Color lightColor;
        public Color darkColor;

        [SerializeField] private Image outline;
        [SerializeField] private Button button;

        public event Action<ButtonClickHandler> ButtonClicked;
        public Image Outline => outline;
        public Button CurrentButton => button;

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
        }

        private void OnDisable()
        {
            toggleSwitch.onToggleOn.RemoveListener(() => SetTheme(true));
            toggleSwitch.onToggleOff.RemoveListener(() => SetTheme(false));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ButtonClicked?.Invoke(this);
        }

        public override void SetTheme(bool isDark)
        {
            _isDark = isDark;
            SetButtonColor();
        }

        public void SetButtonColor()
        {
            button.image.color = _isDark ? darkColor : lightColor;

            if (outline.enabled)
                button.image.color = button.image.color == lightColor ? Color.black : Color.white;
        }
    }
}