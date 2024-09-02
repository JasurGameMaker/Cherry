using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using __Scripts.Project.Data;
using __Scripts.Project.Data.Enums;
using __Scripts.Project.Menu.UI.New;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Zenject;

namespace __Scripts.Project.Menu.UI.Subjects
{
    public class AllLessonsMapper : SerializedMonoBehaviour, ILessonsProvider
    {
        [OdinSerialize] private Dictionary<SubjectType, RectTransform> map;
        [SerializeField] private LessonView lessonViewPrefab;
        
        private Catalog _catalog;

        public IReadOnlyList<LessonView> LessonViews { get; private set; }

        [Inject]
        private void Construct(Catalog catalog)
        {
            _catalog = catalog;
        }

        private void Start() =>
            Map();

        private void OnEnable()
        {
            if (LessonViews != null)
                OnLocaleChanged(LocalizationSettings.SelectedLocale);
            
            LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
        }
        
        private void OnDisable() =>
            LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;

        private void OnLocaleChanged(Locale _)
        {
            foreach (LessonView lessonView in LessonViews)
                lessonView.SetName(lessonView.LessonModel.Lesson.Value.Name);
        }

        private async void Map()
        {
            List<LessonView> views = new ();
            
            foreach (KeyValuePair<SubjectType, RectTransform> pair in map)
            {
                Catalog.Subject subject = _catalog.FindSubject(pair.Key);

                if (subject == null)
                    continue;
                
                foreach (Catalog.Subject.Lesson lesson in subject.lessons)
                {
                    if (!lesson.enabled)
                        continue;
                
                    LessonView view = LeanPool.Spawn(lessonViewPrefab, pair.Value);
                    views.Add(view);
                    Texture2D texture = await Addressables.LoadAssetAsync<Texture2D>(lesson.previewImageKey);
                    view
                        .SetSprite(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f))
                        .SetName(lesson.Name)
                        .SetLesson(lesson);
                }
            }
            LessonViews = views;
        }
    }
}
