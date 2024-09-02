using System;
using System.Collections.Generic;
using __Scripts.Project.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace __Scripts.Project.Core
{
    public class AtlasCameraManager : MonoBehaviour
    {
        private const float TIMER_LIMIT = 0.05f;

        public UnityEvent OnCameraMovementStartEvent = new UnityEvent();

        public UnityEvent OnCameraMovementStopEvent = new UnityEvent();

        private Transform _modelRoot;

        public Transform ModelRoot;

        [SerializeField]
        public GameObject inputEventsReceiver;

        [SerializeField]
        private GameObject cameraGameObject;

        [SerializeField]
        private GameObject lights;

        private Vector3 _initialPosition;

        private Quaternion _initialRotation;

        private Vector3 _initialScale;

        public static bool canMove = true;

        private Bounds _modelBounds;

        private GameObject _cameraTarget;

        private GameObject _cameraBuiltIn;

        private CameraManipulationsHandler _cameraManipulationsHandler;

        [SerializeField]
        private Volume _postFxVolume;

        private Vector3 previousPosition;

        private Quaternion previousRotation;

        private bool previousFrameMovement;

        private float timer;

        private bool isInFPCMode;

        public bool isMoving { get; set; }

        public CameraManipulationsHandler CameraManipulationsHandler => _cameraManipulationsHandler;

        private void Awake()
        {
            _cameraManipulationsHandler = inputEventsReceiver.GetComponent<CameraManipulationsHandler>();
        }

        private void OnEnable()
        {
            CameraManipulationsHandler cameraManipulationsHandler = _cameraManipulationsHandler;
            cameraManipulationsHandler.DoubleClicked = (Action)Delegate.Combine(cameraManipulationsHandler.DoubleClicked, new Action(ResetModelTransform));
        }

        private void OnDisable()
        {
            if (_cameraManipulationsHandler.DoubleClicked != null)
            {
                CameraManipulationsHandler cameraManipulationsHandler = _cameraManipulationsHandler;
                cameraManipulationsHandler.DoubleClicked = (Action)Delegate.Remove(cameraManipulationsHandler.DoubleClicked, new Action(ResetModelTransform));
            }
        }

        public Transform GetModelRoot()
        {
            return ModelRoot;
        }

        public GameObject GetCameraGameObject()
        {
            return cameraGameObject;
        }

        public async UniTask FocusOnModel(bool isImmediate)
        {
            await _cameraManipulationsHandler.FitToTransformBounds(ModelRoot, isImmediate);
        }

        public async UniTask FocusOnObjects(List<GameObject> gameObjects)
        {
            await _cameraManipulationsHandler.FitToObjectsBounds(gameObjects);
        }

        public void StopAllCameraAnimations()
        {
            _cameraManipulationsHandler.StopAllCameraAnimations();
        }

        public void StopCameraFlyAround()
        {
            if (_cameraManipulationsHandler != null)
            {
                _cameraManipulationsHandler.StopFlyAround();
            }
        }

        public void FocusOnModelPart(GameObject go, bool shouldFlyAround = false)
        {
            _cameraManipulationsHandler.FocusOnObject(go.transform, null, shouldFlyAround);
        }

        public void ForceFocusOnModelParts(List<GameObject> goList, bool shouldFlyAround = false)
        {
            _cameraManipulationsHandler.ForceFocusOnObject(ModelRoot, goList, shouldFlyAround);
        }

        public void FocusOnModelParts(List<GameObject> goList, bool flyAround = false)
        {
            _cameraManipulationsHandler.FocusOnObject(ModelRoot, goList, flyAround);
        }

        public void InitPostProcessOnCamera(bool isCustomLocationLoaded)
        {
            bool flag = PlayerPrefs.GetInt("APP_SETTINGS_QUALITY") > 0;
            UnityEngine.Camera.main.GetComponent<UniversalAdditionalCameraData>().antialiasing = (flag ? AntialiasingMode.FastApproximateAntialiasing : AntialiasingMode.None);
            UnityEngine.Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = flag;
            if (isCustomLocationLoaded)
            {
                _postFxVolume.enabled = false;
            }
        }

        public void ResetModelTransform()
        {
            _cameraManipulationsHandler.FitToTransformBounds(ModelRoot);
        }

        public void CanMove()
        {
            canMove = true;
        }

        public void CantMove()
        {
            canMove = false;
        }

        public static void SetCanMove(bool mode)
        {
            canMove = mode;
        }

        public void LookAt(Vector3 position)
        {
            _cameraManipulationsHandler.LookAt(position);
        }

        public void SetCameraBackground(bool shouldHideFlatBackground, string mapName)
        {
            GetComponent<SpriteSwitcher>().SwitchSpriteByName(shouldHideFlatBackground ? null : mapName);
            cameraGameObject.GetComponent<UnityEngine.Camera>().clearFlags = (shouldHideFlatBackground ? CameraClearFlags.Skybox : CameraClearFlags.Color);
        }

        public void DisableCameraLight()
        {
            lights.SetActive(value: false);
        }

        private void Update()
        {
            UpdateCameraMovementIndicators();
        }

        private void UpdateCameraMovementIndicators()
        {
            if (timer < 0.05f)
            {
                timer += Time.deltaTime;
                return;
            }
            timer = 0f;
            Vector3 position = cameraGameObject.transform.position;
            bool flag = position != previousPosition;
            Quaternion rotation = cameraGameObject.transform.rotation;
            flag = flag || rotation != previousRotation;
            if (flag && !previousFrameMovement)
            {
                OnCameraMovementStartEvent.Invoke();
            }
            else if (!flag && previousFrameMovement)
            {
                OnCameraMovementStopEvent.Invoke();
            }
            previousPosition = position;
            previousRotation = rotation;
            previousFrameMovement = flag;
        }

        private void OnDestroy()
        {
            OnCameraMovementStopEvent.Invoke();
        }
    }
}
