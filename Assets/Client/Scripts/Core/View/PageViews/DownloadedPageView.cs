using System;
using System.Collections.Generic;
using __Scripts.Project.Data;
using __Scripts.Project.Menu.UI.New;
using __Scripts.Project.Menu.UI.Subjects;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Client
{
    public class DownloadedPageView : BaseWindowView, ILessonsProvider
    {
        [SerializeField] private Transform content;
        [SerializeField] private LessonView subjectItemPrefab;

        public IReadOnlyList<LessonView> LessonViews { get; private set; }
        
        private Catalog _catalog;

        [Inject]
        private void Construct(Catalog catalog)
        {
            _catalog = catalog;
        }

        private void Start() =>
            SpawnItems();

        private async void SpawnItems()
        {
            List<LessonView> views = new ();
            
            foreach (Catalog.Subject subject in _catalog.subjects)
            {
                foreach (Catalog.Subject.Lesson lesson in subject.lessons)
                {
                    if (!lesson.enabled)
                        continue;

                    long size = await Addressables.GetDownloadSizeAsync(lesson.prefabKey);

                    if (size > 0)
                        continue;
                    
                    LessonView subjectItem = LeanPool.Spawn(subjectItemPrefab, content);
                    views.Add(subjectItem);
                    Texture2D texture = await Addressables.LoadAssetAsync<Texture2D>(lesson.previewImageKey);
                    subjectItem
                        .SetSprite(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f))
                        .SetName(lesson.Name)
                        .SetLesson(lesson);
                }
            }
            LessonViews = views;
        }
    }
}