using __Scripts.Project.Data;
using Zenject;

namespace __Scripts.Project.Menu
{
    public class MenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MenuState>().AsSingle();
        }
    }
}
