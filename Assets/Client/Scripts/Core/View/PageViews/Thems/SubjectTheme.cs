using TMPro;
using UnityEngine;

namespace Client
{
    public class SubjectTheme : ToggleTheme
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private TextMeshProUGUI description;
        
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
            Color color = isDark ? darkColor : lightColor;
            label.color = color;
            description.color = color;
        }
    }
}
