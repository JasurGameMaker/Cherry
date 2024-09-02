using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
    public class FavouritesPageView : BaseWindowView, ILessonsProvider
    {
        [SerializeField] private Transform content;
        [SerializeField] private LessonView subjectItemPrefab;
        
        private Catalog _catalog;
        private MenuState _menuState;
        public IReadOnlyList<LessonView> LessonViews { get; private set; }


        [Inject]
        private void Construct(MenuState menuState, Catalog catalog)
        {
            _menuState = menuState;
            _catalog = catalog;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SpawnItems();
        }

        private async void SpawnItems()
        {
            Clear();
            
            List<LessonView> views = new ();
            
            IEnumerable<Catalog.Subject.Lesson> allLessons = _catalog.subjects.SelectMany(s => s.lessons);
            IEnumerable<Catalog.Subject.Lesson> filteredLessons = allLessons
                .Where(x => _menuState.FavoriteLessonNames.Any(y => y == x.prefabKey));
                
            foreach (Catalog.Subject.Lesson lesson in filteredLessons)
            {
                if (!lesson.enabled)
                    continue;
                
                LessonView view = LeanPool.Spawn(subjectItemPrefab, content);
                views.Add(view);
                Texture2D texture = await Addressables.LoadAssetAsync<Texture2D>(lesson.previewImageKey);
                view
                    .SetSprite(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f))
                    .SetName(lesson.Name)
                    .SetLesson(lesson);
            }
            LessonViews = views;
        }

        public void Clear()
        {
            int count = content.childCount;
            for (int i = count - 1; i >= 0; i--)
                LeanPool.Despawn(content.GetChild(i));
        }
    }
}