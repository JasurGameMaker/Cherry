using __Scripts.Project.Data;
using __Scripts.Project.Menu.UI.New;
using Client;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace __Scripts.Project.Menu.UI.Subjects
{
    public class LessonPopupListener : MonoBehaviour
    {
        [SerializeField] private SubjectPopUpView popup;
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private Image preview;
        
        private void OnEnable() =>
            LessonController.LessonSelected += OnLessonSelected;

        private void OnDisable() =>
            LessonController.LessonSelected -= OnLessonSelected;

        private void OnLessonSelected(Catalog.Subject.Lesson lesson, Sprite sprite)
        {
            popup.Lesson = lesson;
            labelText.SetText(lesson.Name);
            preview.sprite = sprite;
            popup.gameObject.SetActive(true);
        }
    }
}
