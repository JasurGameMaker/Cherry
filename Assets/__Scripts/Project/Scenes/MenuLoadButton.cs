using __Scripts.Project.Data;
using __Scripts.Project.Menu.UI.Utils;
using __Scripts.Project.Scenes.SceneNavigation;
using Zenject;

namespace __Scripts.Project.Scenes
{
    public class MenuLoadButton :  DoozyButtonListener
    {
        private SceneLoader _sceneLoader;
        private Catalog.Subject _subject;

        [Inject]
        private void Construct(SceneLoader sceneLoader, Catalog catalog, Catalog.Subject.Lesson lesson)
        {
            _sceneLoader = sceneLoader;
            _subject = catalog.FindSubject(lesson);
        }

        protected override void OnClick() =>
            _sceneLoader.Load(SceneNavigation.Enums.Scenes.Menu, bindings: Bindings());

        private object[] Bindings()
        {
            return _subject == null ?
                default :
                new object[]
                {
                    _subject
                };
        }
    }
}
