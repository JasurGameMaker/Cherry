using __Scripts.Project.Menu.UI.Utils;
using __Scripts.Project.Scenes.SceneNavigation;
using UnityEngine;
using Zenject;

namespace __Scripts.Project.Menu.UI.Subjects
{
    public class StructureLoadButton : DoozyButtonListener
    {
        [SerializeField] private LessonModel lessonModel;
        
        private SceneLoader _sceneLoader;

        [Inject]
        private void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }
        
        protected override void OnClick()
        {
            if (lessonModel.Lesson.Value == null)
                return;
            
            _sceneLoader.Load(Scenes.SceneNavigation.Enums.Scenes.Game,
                fadeOutOnDemand: true,
                bindings: lessonModel.Lesson.Value);
        }
    }
}
