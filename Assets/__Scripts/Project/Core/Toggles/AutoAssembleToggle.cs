using System.Linq;
using __Scripts.Project.Core.Model;
using __Scripts.Project.Menu.UI.Utils;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Doozy.Runtime.UIManager.Components;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Playables;
using Zenject;

namespace __Scripts.Project.Core.Toggles
{
    public class AutoAssembleToggle : DoozyToggleListener
    {
        [SerializeField] private DrawingToggle drawingToggle;
        
        
        private CourseModelInitializer _modelInitializer;
        private AtlasCameraManager _cameraManager;

        [Inject]
        private void Construct(AtlasCameraManager cameraManager, CourseModelInitializer modelInitializer)
        {
            _cameraManager = cameraManager;
            _modelInitializer = modelInitializer;
        }

        protected override void OnAfterEnable() =>
            _modelInitializer.Initialized += OnInitialize;

        protected override void OnAfterDisable()
        {
            _modelInitializer.Initialized -= OnInitialize;
            
            foreach (var socketController in _modelInitializer.CourseModel.SocketControllers)
            {
                socketController.OnMovementStartEvent.RemoveListener(OnMovementStart);
                socketController.OnAnimRoutineLoaded -= OnAnimRoutineLoaded;
            }
        }

        private void OnInitialize()
        {
            SetInteractable(_modelInitializer.CourseModel.PlayableDirector);
            
            foreach (var socketController in _modelInitializer.CourseModel.SocketControllers)
            {
                socketController.OnMovementStartEvent.AddListener(OnMovementStart);
                socketController.OnAnimRoutineLoaded += OnAnimRoutineLoaded;
            }
        }

        private void OnAnimRoutineLoaded(bool isTweenActive)
        {
            if (_modelInitializer.CourseModel.SocketControllers.All(c => c.IsInPlace()))
            {
                Toggle.SetIsOn(false, true, false);
                SetInteractable(_modelInitializer.CourseModel.PlayableDirector);
            }
            else
                Toggle.SetIsOn(true, true, false);
        }

        private void OnMovementStart()
        {
            SetInteractable(true);
            Toggle.SetIsOn(true, true, false);
        }

        protected override void OnToggle(bool state)
        {
            if (state)
            {
                if (_modelInitializer.CourseModel.PlayableDirector == null)
                    return;
                
                _modelInitializer.CourseModel.PlayableDirector.Play();
                AssemblyCoroutine();
            }
            else
                _modelInitializer.CourseModel
                    .Assemble()
                    .OnStart(() =>
                    {
                        SetInteractable(false);
                        drawingToggle.SetInteractable(false);
                    })
                    .OnComplete(() =>
                    {
                        _cameraManager.FocusOnModel(false).Forget();
                        
                        if (_modelInitializer.CourseModel.PlayableDirector != null)
                            SetInteractable(true);
                        
                        drawingToggle.SetInteractable(true);
                    });
        }

        private async void AssemblyCoroutine()
        {
            SetInteractable(false);
            drawingToggle.SetInteractable(false);
            
            await _cameraManager.FocusOnModel(false);
            await UniTask.WaitWhile(() => _modelInitializer.CourseModel.PlayableDirector.state == PlayState.Playing);
            await _cameraManager.FocusOnModel(false);

            _modelInitializer.CourseModel.SocketControllers.ForEach(s => s.SetIsAttachedState(false));
            SetInteractable(true);
            drawingToggle.SetInteractable(true);
        }
    }
}
