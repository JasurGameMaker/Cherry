using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class SubjectPopUpTheme : ToggleTheme
    {
        [Header("Colors")]
        [SerializeField] private Color backgroundImageColorLight;
        [SerializeField] private Color buttonsBackgroundImageColorLight;
        [SerializeField] private Color startImageColorLight;
        [SerializeField] private Color textColorLight;
        
        [SerializeField] private Color backgroundImageColorDark;
        [SerializeField] private Color buttonsBackgroundImageColorDark;
        [SerializeField] private Color startImageColorDark;
        [SerializeField] private Color textColorDark;
        
        [Header("General Settings")]
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image closeImage;
        [SerializeField] private TextMeshProUGUI lessonText;
        [SerializeField] private TextMeshProUGUI presentationText;
        [SerializeField] private Image buttonBackgroundImage;

        [Header("Lesson Settings")]
        [SerializeField] private Image startLessonImage;
        
        [Header("Presentation Settings")]
        [SerializeField] private Image startPresentationImage;
        
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
            buttonBackgroundImage.color = isDark ? buttonsBackgroundImageColorDark : buttonsBackgroundImageColorLight;
            
            lessonText.color = isDark ? textColorDark : textColorLight;
            presentationText.color = isDark ? textColorDark : textColorLight;
            
            labelText.color = isDark ? Color.white : Color.black;
            closeImage.color = isDark ? Color.white : Color.black;

            startLessonImage.color = isDark ? startImageColorDark : startImageColorLight;
            startPresentationImage.color = isDark ? startImageColorDark : startImageColorLight;
        }
    }
}