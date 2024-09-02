using System.Collections.Generic;
using System.Linq;
using __Scripts.Project.Menu.UI.Subjects;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;

namespace __Scripts.Project.Menu.UI.New
{
    public class LessonsSearch : SerializedMonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [OdinSerialize] private ILessonsProvider lessonsProvider;
        
        private void OnEnable()
        {
            inputField.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnDisable()
        {
            inputField.onValueChanged.RemoveListener(OnValueChanged);
        }
        
        private void OnValueChanged(string text)
        {
            IEnumerable<LessonView> filteredViews = lessonsProvider.LessonViews
                .Where(l => l.LessonModel.Lesson.Value.Name.ToLower().Contains(text.ToLower()))
                .ToList();

            foreach (LessonView lesson in lessonsProvider.LessonViews)
                lesson.gameObject.SetActive(filteredViews.Contains(lesson));
        }
    }
}
