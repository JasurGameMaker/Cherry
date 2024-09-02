using System;
using __Scripts.Project.Data;
using __Scripts.Project.Menu.UI.Subjects;
using __Scripts.Project.Menu.UI.Utils;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;
using Zenject;

namespace __Scripts.Project.Menu.UI.Favorites
{
    public class FavoriteToggle : DoozyToggleListener
    {
        [SerializeField] private LessonModel lessonModel;
        
        private MenuState _menuState;

        [Inject]
        private void Construct(MenuState menuState) =>
            _menuState = menuState;

        protected override void OnAfterEnable()
        {
            if (lessonModel.Lesson.Value != null)
                OnLessonChanged(lessonModel.Lesson.Value);
            
            lessonModel.Lesson.ValueChanged += OnLessonChanged;
        }

        protected override void OnAfterDisable() =>
            lessonModel.Lesson.ValueChanged -= OnLessonChanged;

        private void OnLessonChanged(Catalog.Subject.Lesson lesson) =>
            Toggle.SetIsOn(_menuState.FavoriteLessonNames.Contains(lesson.prefabKey), false, false);

        protected override void OnToggle(bool state)
        {
            if (state)
                _menuState.FavoriteLessonNames.Add(lessonModel.Lesson.Value.prefabKey);
            else
                _menuState.FavoriteLessonNames.Remove(lessonModel.Lesson.Value.prefabKey);
            
            _menuState.Save();
        }
    }
}
