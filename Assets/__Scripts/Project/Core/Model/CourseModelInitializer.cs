using System;
using System.Threading.Tasks;
using __Scripts.Project.Core.Audio;
using __Scripts.Project.Data;
using __Scripts.Project.Scenes.SceneNavigation;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace __Scripts.Project.Core.Model
{
    public class CourseModelInitializer : MonoBehaviour
    {
        [SerializeField] private AtlasCameraManager cameraManager;
        
        [SerializeField] private Transform root;

        public CourseModel CourseModel { get; private set; }
        
        private Catalog.Subject.Lesson _lesson;
        private CoreState _coreState;
        private IAudioPlayer _audioPlayer;
        private AsyncOperationHandle<AudioClip> _audioHandle;
        private AsyncOperationHandle<GameObject> _modelHandler;
        private SceneLoader _sceneLoader;

        public event Action Initialized;

        [Inject]
        private void Construct(Catalog.Subject.Lesson lesson, CoreState coreState, IAudioPlayer audioPlayer, SceneLoader sceneLoader)
        {
            _lesson = lesson;
            _coreState = coreState;
            _audioPlayer = audioPlayer;
            _sceneLoader = sceneLoader;
        }

        private void Awake() =>
            Initialize().Forget();

        private async UniTaskVoid Initialize()
        {
            try
            {
                _modelHandler = Addressables.InstantiateAsync(_lesson.prefabKey, root);
            
                GameObject model = await _modelHandler;
            
                CourseModel = model.GetComponent<CourseModel>();
                CourseModel.Initialize(_coreState, cameraManager.CameraManipulationsHandler);
            
                if (_lesson.singleAudioClip)
                {
                    _audioHandle = Addressables.LoadAssetAsync<AudioClip>(_lesson.AudioKey);
                    AudioClip clip = await _audioHandle;
                    _audioPlayer.SetClip(clip);
                    _audioPlayer.TriggerToggle();
                }
                else
                    await CourseModel.LoadClips();
            
                cameraManager.FocusOnModel(true).Forget();
                _coreState.SelectedMesh.Value = CourseModel.CourseMeshes[0];
            
                _sceneLoader.FadeOut();
                Initialized?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                _sceneLoader.Load(Scenes.SceneNavigation.Enums.Scenes.Menu, tweenFadeIn: false);
            }
        }

        private void OnDestroy()
        {
            if (_modelHandler.IsValid())
                Addressables.Release(_modelHandler);
            
            if (_audioHandle.IsValid())
                Addressables.Release(_audioHandle);
        }
    }
}
