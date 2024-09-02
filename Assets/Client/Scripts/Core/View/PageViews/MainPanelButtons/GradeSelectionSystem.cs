using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class GradeSelectionSystem : MonoBehaviour, IPointerClickHandler
    {
        [field: SerializeField] public Image GradeImage { get; private set; }
        [field: SerializeField] public Button GradeButton { get; private set; }
        [field: SerializeField] public TextMeshProUGUI GradeTMP { get; private set; }
        
        public int Id { get; private set; }
        
        public event Action<GradeSelectionSystem> GradeSelected;

        private void Awake()
        {
            Id = int.Parse(GradeTMP.text);
        }

        private void OnEnable()
        {
            GradeButton.onClick.AddListener(OnGradeButtonClicked);
        }

        private void OnDisable()
        {
            GradeButton.onClick.RemoveListener(OnGradeButtonClicked);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            GradeSelected?.Invoke(this);
        }
        
        private void OnGradeButtonClicked()
        {
            
        }
    }
}