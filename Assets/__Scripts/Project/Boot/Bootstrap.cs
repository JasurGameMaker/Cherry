using __Scripts.Project.Data;
using __Scripts.Project.Scenes.SceneNavigation;
using __Scripts.Project.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization.Settings;
using Zenject;

namespace __Scripts.Project.Boot
{
    public class Bootstrap : MonoBehaviour
    {
        //TEMP
        [SerializeField] private GameObject loginPopup;
        [SerializeField] private RectTransform popupRoot;
        
        private IRemoteHelper _remoteHelper;
        private SceneLoader _sceneLoader;

        [Inject]
        private void Construct(SceneLoader sceneLoader, IRemoteHelper remoteHelper)
        {
            _sceneLoader = sceneLoader;
            _remoteHelper = remoteHelper;
        }

        private async void Awake()
        {
            Catalog catalog = await _remoteHelper.LoadCatalog();
            ProjectContext.Instance.Container.Bind<Catalog>().FromInstance(catalog).AsSingle();
            
            await LocalizationSettings.InitializationOperation;

            ProjectContext.Instance.Container.InstantiatePrefab(loginPopup, popupRoot);
            //_sceneLoader.Load(Utils.SceneNavigation.Enums.Scenes.Menu);
        }
    }
}
