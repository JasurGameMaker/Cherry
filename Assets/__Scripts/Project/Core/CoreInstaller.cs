using __Scripts.Project.Core.Audio;
using __Scripts.Project.Core.Model;
using UnityEngine;
using Zenject;

namespace __Scripts.Project.Core
{
    public class CoreInstaller : MonoInstaller
    {
        [SerializeField] private CourseModelInitializer courseModelInitializer;
        [SerializeField] private AtlasCameraManager cameraManager;
        [SerializeField] private AudioPlayer audioPlayer;
        
        public override void InstallBindings()
        {
            Container.Bind<CourseModelInitializer>().FromInstance(courseModelInitializer).AsSingle();
            Container.Bind<AtlasCameraManager>().FromInstance(cameraManager).AsSingle();
            Container.Bind<IAudioPlayer>().FromInstance(audioPlayer).AsSingle();
            Container.Bind<CoreState>().AsSingle();
        }
    }
}
