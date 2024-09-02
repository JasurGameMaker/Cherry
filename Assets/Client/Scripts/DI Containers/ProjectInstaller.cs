using Zenject;

namespace Client
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SceneLoader>().AsSingle().NonLazy();
        }
    }
}