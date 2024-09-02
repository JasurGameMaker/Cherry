using System;
using __Scripts.Project.Data;
using __Scripts.Project.Menu.UI.Subjects;
using UnityEngine;
using UnityEngine.UI;

namespace __Scripts.Project.Menu.UI.New
{
    public class LessonController : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private LessonModel lessonModel;
        
        public static event Action<Catalog.Subject.Lesson, Sprite> LessonSelected;
        
        private void OnEnable() =>
            button.onClick.AddListener(OnButtonClick);

        private void OnDisable() =>
            button.onClick.RemoveListener(OnButtonClick);

        private void OnButtonClick() =>
            LessonSelected?.Invoke(lessonModel.Lesson.Value, lessonModel.Sprite);
    }
}
