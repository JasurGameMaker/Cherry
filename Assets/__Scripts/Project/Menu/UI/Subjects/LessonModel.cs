using System;
using __Scripts.Project.Data;
using __Scripts.Project.Utils;
using UnityEngine;

namespace __Scripts.Project.Menu.UI.Subjects
{
    public class LessonModel : MonoBehaviour
    {
        public ReactiveProperty<Catalog.Subject.Lesson> Lesson { get; } = new ();
        public Sprite Sprite { get; private set; }
        
        public void SetLesson(Catalog.Subject.Lesson lesson) =>
            Lesson.Value = lesson;
        
        public void SetSprite(Sprite sprite) =>
            Sprite = sprite;
    }
}
