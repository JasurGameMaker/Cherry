using __Scripts.Project.Menu.UI.Utils;
using __Scripts.Project.Scenes.SceneNavigation;
using UnityEngine;
using Zenject;

namespace __Scripts.Project.Scenes
{
    public class SceneLoadButton : DoozyButtonListener
    {
        [SerializeField] private SceneNavigation.Enums.Scenes scene;
        
        private SceneLoader _sceneLoader;

        [Inject]
        private void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }
        
        protected override void OnClick() =>
            _sceneLoader.Load(scene);
    }
}
