using __Scripts.Project.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace __Scripts.Project.Menu.UI.Subjects
{
    public class LessonView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI lessonNameTxt;
        [SerializeField] private Image image;
        [SerializeField] private LessonModel lessonModel;
        
        public LessonModel LessonModel => lessonModel;
        
        public LessonView SetSprite(Sprite sprite)
        {
            image.overrideSprite = sprite;
            lessonModel.SetSprite(sprite);
            return this;
        }

        public LessonView SetName(string lessonName)
        {
            lessonNameTxt.SetText(lessonName);
            return this;
        }

        public LessonView SetLesson(Catalog.Subject.Lesson lesson)
        {
            lessonModel.SetLesson(lesson);
            return this;
        }
    }
}
