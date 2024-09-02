using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class ChooseButton : MonoBehaviour
    {
        [field: SerializeField] public Button Button { get; set; }
        [field: SerializeField] public Image Image { get; set; }
        [field: SerializeField] public TextMeshProUGUI ChooseTMP { get; set; }
        [field: SerializeField] public Image ChooseImage { get; set; }
        [field: SerializeField] public TextMeshProUGUI ChooseGradeTMP { get; set; }
        [field: SerializeField] public Image ScrollImage { get; set; }
        [field: SerializeField] public List<GradeSelectionSystem> GradeSelectionList { get; set; }
        
        [SerializeField] private MainPanelTheme mainPanelTheme;
        [SerializeField] private GameObject popUp;
        
        public bool IsChooseSelected { get; private set; }

        private void OnEnable()
        {
            Button.onClick.AddListener(OnChooseButtonClicked);
            foreach (var gradeSelection in GradeSelectionList)
            {
                gradeSelection.GradeSelected += OnGradeSelected;
            }
        }
        
        private void OnDisable()
        {
            Button.onClick.RemoveListener(OnChooseButtonClicked);
            foreach (var gradeSelection in GradeSelectionList)
            {
                gradeSelection.GradeSelected -= OnGradeSelected;
            }            
            DisableVisual();
        }

        private void OnChooseButtonClicked()
        {
            IsChooseSelected = !IsChooseSelected;
            popUp.SetActive(IsChooseSelected);
            Image.sprite = IsChooseSelected ? mainPanelTheme.SelectedSprite : mainPanelTheme.DefaultSprite;
            ChooseTMP.color = IsChooseSelected && !mainPanelTheme.IsDark ? Color.black : Color.white;
        }

        private void OnGradeSelected(GradeSelectionSystem grade)
        {
            foreach (var gradeSelection in GradeSelectionList)
            {
                gradeSelection.GradeImage.enabled = false;
                gradeSelection.GradeTMP.color = Color.white;
                gradeSelection.GradeImage.color = mainPanelTheme.GradeSelectionLight;
                
                if (gradeSelection.Id == grade.Id)
                {
                    grade.GradeImage.enabled = true;
            
                    grade.GradeImage.color = mainPanelTheme.IsDark
                        ? mainPanelTheme.GradeSelectionDark
                        : mainPanelTheme.GradeSelectionLight;
            
                    grade.GradeTMP.color = mainPanelTheme.IsDark
                        ? Color.white
                        : Color.black;
                }
            }
        }

        private void DisableVisual()
        {
            IsChooseSelected = false;
            Image.sprite = mainPanelTheme.DefaultSprite;
            ChooseTMP.color = Color.white;
            popUp.SetActive(false);
        }
    }
}