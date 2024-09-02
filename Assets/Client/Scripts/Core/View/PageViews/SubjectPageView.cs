using System.Collections.Generic;
using __Scripts.Project.Data;
using __Scripts.Project.Menu.UI.New;
using __Scripts.Project.Menu.UI.Subjects;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Client.Scripts.Core.View.PageViews
{
    public class SubjectPageView : BaseWindowView, ILessonsProvider
    {
        [SerializeField] private Transform content;
        [SerializeField] private LessonView lessonView;

        public IReadOnlyList<LessonView> LessonViews { get; private set; }

        public async void Init(Catalog.Subject subject)
        {
            Clear();
            
            ViewLabelText = subject.Key;

            List<LessonView> views = new ();
            
            foreach (Catalog.Subject.Lesson lesson in subject.lessons)
            {
                if (!lesson.enabled)
                    continue;
                
                LessonView view = LeanPool.Spawn(lessonView, content);
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
