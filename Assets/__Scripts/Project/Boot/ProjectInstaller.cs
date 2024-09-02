using __Scripts.Project.Data;
using __Scripts.Project.Scenes.SceneNavigation;
using __Scripts.Project.Services;
using UnityEngine;
using Zenject;

namespace __Scripts.Project.Boot
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private string catalogUrl;
        [SerializeField] private UIStaticData staticData;
        [SerializeField] private SceneTransitionView sceneTransitionView;
        
        
        public override void InstallBindings()
        {
            Container.Bind<IRemoteHelper>().FromInstance(new RemoteHelper(catalogUrl)).AsSingle();
            Container.Bind<UIStaticData>().FromInstance(staticData).AsSingle();
            Container.Bind<SceneLoader>().AsSingle().WithArguments(new SceneTransitionTweener(NewDontDestroy(sceneTransitionView), staticData));
        }
        
        private static T NewDontDestroy<T>(T prefab) where T : MonoBehaviour
        {
            T t = Instantiate(prefab);
            t.name = prefab.name;
            DontDestroyOnLoad(t);
            return t;
        }
    }
}
