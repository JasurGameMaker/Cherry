using System;
using __Scripts.Project.Core.Model.Socket;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace __Scripts.Project.Core.Model
{
    [RequireComponent(typeof(SocketController))]
    [RequireComponent(typeof(Renderer))]
    public class CourseMesh : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private MeshData meshData;

        public MeshData MeshData => meshData;
        public SocketController SocketController => _socketController;
        
        [CanBeNull]
        public AudioClip AudioClip => _audioClip;
        
        private SocketController _socketController;

        private Vector3 _origin;
        private CoreState _coreState;
        private AtlasCameraManager _cameraManager;
        private AudioClip _audioClip;
        private AsyncOperationHandle<AudioClip> _audioClipHandler;
        private CameraManipulationsHandler _cameraManipulationsHandler;

        private void Awake()
        {
            _socketController = GetComponent<SocketController>();
            _socketController.Init(GetComponent<Renderer>().material.shader, Camera.main);
            _origin = transform.position;
        }

        private void OnDestroy()
        {
            if (_audioClipHandler.IsValid())
                Addressables.Release(_audioClipHandler);
        }

        public void Initialize(CoreState coreState, CameraManipulationsHandler cameraManipulationsHandler)
        {
            _coreState = coreState;
            _cameraManipulationsHandler = cameraManipulationsHandler;
        }

        public Tween ResetPosition() =>
            transform
                .DOMove(_origin, 0.7f);

        public void OnPointerClick(PointerEventData eventData)
        {
            _coreState.SelectedMesh.Value = this;

            if (!IsDraggable())
                _cameraManipulationsHandler.FocusOnObject(transform);
        }

        public bool IsDraggable() =>
            _socketController.IsDraggaable();
        
        public async UniTask LoadClip()
        {
            if (string.IsNullOrEmpty(meshData.audioKey))
                return;
            
            string audioClipKey = LocalizationSettings.StringDatabase.GetLocalizedString(meshData.tableReference, meshData.audioKey);
            _audioClipHandler = Addressables.LoadAssetAsync<AudioClip>(audioClipKey);
            _audioClip = await _audioClipHandler;
        }
    }

    [Serializable]
    public struct MeshData
    {
        public string tableReference;
        public string titleKey;
        public string descriptionKey;
        [InfoBox("Used only if singleAudioClip is set to false in config")]
        public string audioKey;
    }
}
