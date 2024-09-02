using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class MainPanelTheme : ToggleTheme
    {
        [field: SerializeField] public Sprite DefaultSprite { get; private set; }
        [field: SerializeField] public Sprite SelectedSprite { get; private set; }

        [field: SerializeField] public Color GradeSelectionLight { get; private set; }
        [field: SerializeField] public Color GradeSelectionDark { get; private set; }

        [Header("Scripts")] 
        [SerializeField] private List<ChooseButton> chooseButtonList = new();
        [SerializeField] private List<SaveButton> saveButtonList = new();

        [SerializeField] private Color lightColor;
        [SerializeField] private Color darkColor;
        
        public bool IsDark { get; private set; }
        
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
            IsDark = isDark;
            SetChooseButtonsTheme(isDark);
            SetSaveButtonsTheme(isDark);
        }

        private void SetChooseButtonsTheme(bool isDark)
        {
            foreach (var chooseButton in chooseButtonList)
            {
                chooseButton.Image.color = isDark ? darkColor : lightColor;
                chooseButton.ChooseTMP.color = Color.white;
                chooseButton.ChooseImage.color = isDark ? darkColor : lightColor;
                
                foreach (var grade in chooseButton.GradeSelectionList)
                {
                    if (isDark && grade.GradeImage.enabled)
                    {
                        Debug.Log("dark");
                        grade.GradeImage.color = GradeSelectionDark;
                        grade.GradeTMP.color = Color.white;
                    }

                    if (!isDark && grade.GradeImage.enabled)
                    {
                        Debug.Log("light");
                        grade.GradeImage.color = GradeSelectionLight;
                        grade.GradeTMP.color = Color.black;
                    }
                }
                
                chooseButton.ChooseGradeTMP.color = Color.white;
                chooseButton.ScrollImage.color = isDark ? darkColor : lightColor;

                if (isDark && chooseButton.IsChooseSelected)
                {
                    chooseButton.ChooseTMP.color = Color.white;
                }
                if (!isDark && chooseButton.IsChooseSelected)
                {
                    chooseButton.ChooseTMP.color = Color.black;
                }
            }
        }
        
        private void SetSaveButtonsTheme(bool isDark)
        {
            foreach (var saveButton in saveButtonList)
            {
                saveButton.BackgroundImage.color = isDark ? darkColor : lightColor;
                saveButton.SaveTMP.color = Color.white;
                saveButton.Icon.color = Color.white;

                if (isDark && saveButton.IsSaveSelected)
                {
                    saveButton.Icon.color = Color.white;
                    saveButton.SaveTMP.color = Color.white;
                }
                if (!isDark && saveButton.IsSaveSelected)
                {
                    saveButton.Icon.color = Color.black;
                    saveButton.SaveTMP.color = Color.black;
                }
            }
        }
    }
}