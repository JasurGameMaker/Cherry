using System;
using System.Collections.Generic;
using System.Linq;
using __Scripts.Project.Core.Model;
using __Scripts.Project.Utils;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace __Scripts.Project.Core
{
    [RequireComponent(typeof(TouchGesturesTracker))]
    public class CameraManipulationsHandler : MonoBehaviour
    {
        [SerializeField]
        public Transform modelTransform;

        [SerializeField]
        private Transform cameraTransform;

        private Vector3 center;

        [SerializeField]
        private ModelManipulationSettings zoomSettings;

        [SerializeField]
        private ModelManipulationSettings rotateSettings;

        [SerializeField]
        private ModelManipulationSettings panSettings;

        [SerializeField]
        private ModelManipulationSettings pinchZoomSettings;

        [SerializeField]
        [Range(0.1f, 0.9f)]
        private float bounceDistance = 0.15f;

        [SerializeField]
        private float bounceSpeed = 1f;

        private BoxCollider bounds;

        private Vector3 boundsDefaultSize;

        private float wasdqeSpeed = 5f;

        private float zoomSpeed = 25f;

        public float perspectiveZoomSpeed = 3f;

        public float orthoZoomSpeed = 3f;

        private TouchGesturesTracker _touchGesturesTracker;

        public Action DoubleClicked;

        public Action BackgroundClicked;

        private Vector3 _targetPosition;

        private Vector3 _panOffset;

        private float _currentDistance;

        private Quaternion _cameraRotation;

        private Transform _target;

        private Bounds _targetBounds;

        public float _initialAngleY = 5f;

        public float _initialAngleX = 30f;

        private Plane _movePlane;

        private bool _isBuilInCameraExists;

        private bool _isBuilInCameraAnimationEnabled;

        private bool isMinDist;

        private Transform _cameraBuiltIn;

        private Transform _cameraTarget;

        public bool shouldFlyAroundTarget;

        public bool automaticCameraMovementEnabled;

        private float _cameraFollowSpeed = 9f;

        private float minScrollDist = 0.2f;

        private bool shoudUpdateCameraPosition = true;

        private bool _cameraRotationIsLimited;

        private float _cameraRotationAxisAMinLimit;

        private float _cameraRotationAxisAMaxLimit;

        private float _cameraRotationAxisBMinLimit;

        private float _cameraRotationAxisBMaxLimit;

        private float timer;

        private Quaternion lookAtRot;

        private bool firstCameraAnimationFrame = true;

        private Vector3 _cameraRotationEuler;

        private bool _isCameraTransformTweenActive;

        private string _currentFocusTargetName;
        private CourseModelInitializer _modelInitializer;

        public static event Action CameraManuallyMoved;

        [Inject]
        private void Construct(CourseModelInitializer modelInitializer)
        {
            _modelInitializer = modelInitializer;
        }

        private void OnEnable()
        {
            _touchGesturesTracker = GetComponent<TouchGesturesTracker>();
            
            TouchGesturesTracker touchGesturesTracker = _touchGesturesTracker;
            touchGesturesTracker.ScrollZoomed = (Action<float, Vector3>)Delegate.Combine(touchGesturesTracker.ScrollZoomed, new Action<float, Vector3>(OnScrollZoom));
            TouchGesturesTracker touchGesturesTracker2 = _touchGesturesTracker;
            touchGesturesTracker2.Pinched = (Action<float, Vector3>)Delegate.Combine(touchGesturesTracker2.Pinched, new Action<float, Vector3>(OnPinchZoom));
            TouchGesturesTracker touchGesturesTracker3 = _touchGesturesTracker;
            touchGesturesTracker3.Swiped = (Action<Vector2>)Delegate.Combine(touchGesturesTracker3.Swiped, new Action<Vector2>(OnSwipe));
            TouchGesturesTracker touchGesturesTracker4 = _touchGesturesTracker;
            touchGesturesTracker4.Moved = (Action<Vector3, Vector3>)Delegate.Combine(touchGesturesTracker4.Moved, new Action<Vector3, Vector3>(OnMove));
            TouchGesturesTracker touchGesturesTracker5 = _touchGesturesTracker;
            touchGesturesTracker5.BeganMove = (Action<Vector3>)Delegate.Combine(touchGesturesTracker5.BeganMove, new Action<Vector3>(OnBeganMove));
            TouchGesturesTracker touchGesturesTracker6 = _touchGesturesTracker;
            touchGesturesTracker6.EndedDrag = (Action)Delegate.Combine(touchGesturesTracker6.EndedDrag, new Action(OnDragEnded));
            TouchGesturesTracker touchGesturesTracker7 = _touchGesturesTracker;
            touchGesturesTracker7.DoubleClicked = (Action)Delegate.Combine(touchGesturesTracker7.DoubleClicked, new Action(OnDoubleClick));
            TouchGesturesTracker touchGesturesTracker8 = _touchGesturesTracker;
            touchGesturesTracker8.BackgroundClicked = (Action)Delegate.Combine(touchGesturesTracker8.BackgroundClicked, new Action(OnBackgroundClicked));
            
            _modelInitializer.Initialized += AdjustSpeedByMapSize;
        }

        private void OnDisable()
        {
            _touchGesturesTracker.Pinched = null;
            _touchGesturesTracker.ScrollZoomed = null;
            _touchGesturesTracker.Swiped = null;
            _touchGesturesTracker.Moved = null;
            _touchGesturesTracker.BeganMove = null;
            _touchGesturesTracker.EndedDrag = null;
            _touchGesturesTracker.DoubleClicked = null;
            _touchGesturesTracker.BackgroundClicked = null;
            
            _modelInitializer.Initialized -= AdjustSpeedByMapSize;
        }

        private void AdjustSpeedByMapSize()
        {
            float num = 200f;
            float num2 = 5f;
            float num3 = 25f;
            float num4 = GetMapSize() / num;
            wasdqeSpeed = num2 * num4 * 100f;
            zoomSpeed = num3 * num4 * 300f;
            print("Controll speed adjustments: " + $"map size: x{num4: 0.00}, " + $"new wasd speed: {wasdqeSpeed: 0.00}, " + $"new zoom speed: {zoomSpeed: 0.00}");
        }

        private float GetMapSize()
        {
            List<Renderer> allPhysicalObjects = GetAllPhysicalObjects();
            Vector3 max = allPhysicalObjects.First().bounds.max;
            Vector3 min = allPhysicalObjects.First().bounds.min;
            foreach (Renderer item in allPhysicalObjects)
            {
                Bounds bounds = item.bounds;
                if (bounds.max.x > max.x)
                {
                    max.x = bounds.max.x;
                }
                if (bounds.max.y > max.y)
                {
                    max.y = bounds.max.y;
                }
                if (bounds.max.z > max.z)
                {
                    max.z = bounds.max.z;
                }
                if (bounds.min.x < min.x)
                {
                    min.x = bounds.min.x;
                }
                if (bounds.min.y < min.y)
                {
                    min.y = bounds.min.y;
                }
                if (bounds.min.z < min.z)
                {
                    min.z = bounds.min.z;
                }
            }
            return (max - min).magnitude;
        }

        private List<Renderer> GetAllPhysicalObjects()
        {
            return FindObjectsOfType<Renderer>().ToList();
        }

        private void OnBackgroundClicked()
        {
            BackgroundClicked?.Invoke();
        }

        private void OverviewViewShow()
        {
            AdjustSpeedByMapSize();
        }

        private void OnDragEnded()
        {
            shoudUpdateCameraPosition = true;
        }

        public void LookAt(Vector3 position)
        {
            automaticCameraMovementEnabled = true;
            cameraTransform.DOLookAt(position, 0.25f).OnComplete(delegate
            {
                _targetPosition = position;
                _currentDistance = Vector3.Distance(cameraTransform.position, _targetPosition);
                GetRotationFromCameraTransform();
                automaticCameraMovementEnabled = false;
            });
        }

        private void OnPinchZoom(float zoomValue, Vector3 screenPosition)
        {
            OnCameraManualMove();
            if (!automaticCameraMovementEnabled)
            {
                minScrollDist = _targetBounds.size.magnitude * 0.1f;
                float num = ((!isMinDist) ? Vector3.Distance(cameraTransform.position, _targetPosition) : minScrollDist);
                float num2 = pinchZoomSettings.InverseValue.x * num * zoomValue;
                if (num <= minScrollDist && zoomValue > 0f)
                {
                    Vector3 vector = cameraTransform.position - _targetPosition;
                    _targetPosition -= vector * 0.1f;
                    isMinDist = true;
                }
                else
                {
                    _currentDistance = Mathf.Clamp(_currentDistance - num2, 0.01f, 80000f);
                    isMinDist = false;
                }
            }
        }

        private void OnAnimationModeSwitch(bool isActive)
        {
            if (isActive)
            {
                _touchGesturesTracker.DoubleClicked = null;
                EnableCameraAnimation();
                return;
            }
            _touchGesturesTracker.DoubleClicked = null;
            TouchGesturesTracker touchGesturesTracker = _touchGesturesTracker;
            touchGesturesTracker.DoubleClicked = (Action)Delegate.Combine(touchGesturesTracker.DoubleClicked, new Action(OnDoubleClick));
            DisableCameraAnimation();
            _cameraRotationIsLimited = false;
        }

        public void SetCameraRotationLimited(bool shouldLimitCameraRotation)
        {
            _cameraRotationIsLimited = shouldLimitCameraRotation;
        }

        public void OnAnimationPlaybackChange(bool isPlaying, bool autoplay)
        {
            firstCameraAnimationFrame = !autoplay;
            if (isPlaying)
            {
                EnableCameraAnimation();
            }
            else
            {
                CalcCameraRotationLimits();
            }
        }

        private void CalcCameraRotationLimits()
        {
            _cameraRotationAxisAMaxLimit = _cameraRotationEuler.y + 45f;
            _cameraRotationAxisBMaxLimit = _cameraRotationEuler.x + 45f;
            _cameraRotationAxisAMinLimit = _cameraRotationEuler.y - 45f;
            _cameraRotationAxisBMinLimit = _cameraRotationEuler.x - 45f;
        }

        private void DisableCameraAnimation()
        {
            if (_isBuilInCameraAnimationEnabled && _isBuilInCameraExists && !(_cameraTarget == null) && !(cameraTransform == null))
            {
                _isBuilInCameraAnimationEnabled = false;
                _targetPosition = _cameraTarget.position;
                _currentDistance = Vector3.Distance(cameraTransform.position, _targetPosition);
                InitZoomParameters();
                ReturnControlToManualMovementHandlers();
            }
        }

        public void ResetTargetPosition()
        {
            _targetPosition = cameraTransform.position + cameraTransform.forward * _currentDistance;
        }

        public void ReturnControlToManualMovementHandlers()
        {
            automaticCameraMovementEnabled = false;
            GetRotationFromCameraTransform();
        }

        private void GetRotationFromCameraTransform()
        {
            _cameraRotation = cameraTransform.rotation;
            _cameraRotationEuler = _cameraRotation.eulerAngles;
        }

        public void EnableCameraAnimation()
        {
            _isBuilInCameraAnimationEnabled = true;
            SwitchToAutomaticCameraMovement();
        }

        public void SwitchToAutomaticCameraMovement()
        {
            automaticCameraMovementEnabled = true;
        }

        private void LateUpdate()
        {
            FlyAroundTarget();
            WASDQEMove();
            if (!automaticCameraMovementEnabled)
            {
                UpdateCameraTransform();
            }
            FollowSceneBuiltInCamera();
        }

        private void WASDQEMove()
        {
            bool flag = false;
            GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
            if (currentSelectedGameObject != null)
            {
                flag = currentSelectedGameObject.TryGetComponent<TMP_InputField>(out var _);
            }
            if (!(!AtlasCameraManager.canMove || flag))
            {
                Vector3 zero = Vector3.zero;
                if (Input.GetKey(KeyCode.W))
                {
                    float b = _currentDistance - wasdqeSpeed * Time.deltaTime;
                    _currentDistance = Mathf.Max(0.01f, b);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    zero += -cameraTransform.right;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    float b2 = _currentDistance + wasdqeSpeed * Time.deltaTime;
                    _currentDistance = Mathf.Max(0.01f, b2);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    zero += cameraTransform.right;
                }
                if (Input.GetKey(KeyCode.E))
                {
                    zero += cameraTransform.up;
                }
                if (Input.GetKey(KeyCode.Q))
                {
                    zero += -cameraTransform.up;
                }
                if (zero.magnitude > 0f)
                {
                    OnCameraManualMove();
                    _targetPosition += zero * wasdqeSpeed * Time.deltaTime;
                }
            }
        }

        private void OnCameraManualMove()
        {
            StopAllCameraAnimations();
            CameraManuallyMoved?.Invoke();
        }

        public void StopAllCameraAnimations()
        {
            DisableCameraAnimation();
            shouldFlyAroundTarget = false;
            cameraTransform.DOKill();
        }

        private void FollowSceneBuiltInCamera()
        {
            if (_isBuilInCameraExists && !(_cameraBuiltIn == null) && !(_cameraTarget == null) && _isBuilInCameraAnimationEnabled)
            {
                if (firstCameraAnimationFrame)
                {
                    cameraTransform.position = _cameraBuiltIn.position;
                    cameraTransform.LookAt(_cameraTarget.position);
                    firstCameraAnimationFrame = false;
                }
                else
                {
                    cameraTransform.position = Vector3.Lerp(cameraTransform.position, _cameraBuiltIn.position, Time.deltaTime * _cameraFollowSpeed);
                    Vector3 normalized = (_cameraTarget.position - cameraTransform.position).normalized;
                    lookAtRot = Quaternion.LookRotation(normalized);
                    cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, lookAtRot, Time.deltaTime * _cameraFollowSpeed);
                }
            }
        }

        private void FlyAroundTarget()
        {
            if (shouldFlyAroundTarget)
            {
                timer += Time.deltaTime * 0.2f;
                float num = 2f / (20f - Mathf.Cos(2f * timer));
                float deltaA = num * Mathf.Cos(timer);
                float num2 = num * Mathf.Sin(3f * timer) / 2f;
                Vector3 eulerAngles = cameraTransform.eulerAngles;
                float num3 = eulerAngles.x % 360f;
                if (num3 < 0f)
                {
                    num3 += 360f;
                }
                float num4 = -1f;
                if ((num3 < 180f) ^ (Mathf.Sign(num2) > 0f))
                {
                    num4 = 1f;
                }
                float num5 = -1f * (num3 - 180f) * (num3 - 180f) / 32400f + 1f;
                num2 *= 1f + num4 * num5;
                float deltaZ = (0f - eulerAngles.z) * Time.deltaTime;
                RotateAngleAxis(deltaA, num2, deltaZ);
            }
        }

        private void OnDoubleClick()
        {
            OnCameraManualMove();
            DoubleClicked?.Invoke();
        }

        public void SetTarget(Transform target)
        {
            InitializeBuiltInCamera(target);
            if (_isBuilInCameraExists)
            {
                SetTargetToCam(target);
                Debug.Log("SetTargetToCam");
            }
            else
            {
                SetInitialCameraRotation();
                FitToTransformBounds(target.transform);
                Debug.Log("FitToBounds");
                GetRotationFromCameraTransform();
            }
            InitZoomParameters();
        }

        public void InitializeBuiltInCamera(Transform target)
        {
            _target = target;
            _isBuilInCameraExists = FindBuiltInCamera();
            if (_isBuilInCameraExists)
            {
                _cameraBuiltIn.gameObject.SetActive(value: true);
            }
        }

        private void InitZoomParameters()
        {
            _targetBounds = _target.EncapsulateBounds();
            float magnitude = _targetBounds.extents.magnitude;
            center = _targetBounds.center;
            zoomSettings.BaseSpeed = magnitude / 5f;
            zoomSettings.Acceleration = zoomSettings.BaseSpeed / 5f;
            zoomSettings.Deceleration = zoomSettings.BaseSpeed / 10f;
            pinchZoomSettings.BaseSpeed = magnitude;
            pinchZoomSettings.CurrentSpeed = pinchZoomSettings.BaseSpeed;
        }

        private void SetTargetToCam(Transform target)
        {
            if (_isBuilInCameraExists && !(_cameraBuiltIn == null) && !(_cameraTarget == null))
            {
                automaticCameraMovementEnabled = true;
                cameraTransform.position = _cameraBuiltIn.position;
                cameraTransform.LookAt(_cameraTarget.position);
                GetRotationFromCameraTransform();
                _targetPosition = _cameraTarget.position;
                _currentDistance = Vector3.Distance(_cameraBuiltIn.position, _cameraTarget.position);
                automaticCameraMovementEnabled = false;
                isMinDist = false;
            }
        }

        private bool FindBuiltInCamera()
        {
            _cameraBuiltIn = null;
            _cameraTarget = null;
            Transform[] componentsInChildren = _target.GetComponentsInChildren<Transform>(includeInactive: true);
            foreach (Transform transform in componentsInChildren)
            {
                if (transform.name.Contains("Camera") && !transform.name.Contains("CameraTarget") && !transform.name.Contains("CameraPath"))
                {
                    _cameraBuiltIn = transform.transform;
                }
                if (transform.name.Contains("CameraTarget"))
                {
                    _cameraTarget = transform.transform;
                }
            }
            if (_cameraBuiltIn != null)
            {
                return _cameraTarget != null;
            }
            return false;
        }

        private void OnScrollZoom(float zoomValue, Vector3 screenTargetPoint)
        {
            OnCameraManualMove();
            float b = _currentDistance - zoomValue * Time.deltaTime * zoomSpeed;
            _currentDistance = Mathf.Max(0.01f, b);
        }

        private void OnSwipe(Vector2 delta)
        {
            OnCameraManualMove();
            if (!automaticCameraMovementEnabled)
            {
                float deltaA = delta.x / (float)Screen.width * 360f * rotateSettings.InverseValue.y;
                float deltaX = delta.y / (float)Screen.height * 180f * rotateSettings.InverseValue.x;
                _currentDistance = Vector3.Distance(cameraTransform.position, _targetPosition);
                RotateAngleAxis(deltaA, deltaX);
            }
        }

        private void RotateAngleAxis(float deltaA, float deltaX, float deltaZ = 0f)
        {
            _cameraRotationEuler += new Vector3(deltaX, deltaA, deltaZ);
            if (_cameraRotationIsLimited)
            {
                _cameraRotationEuler.x = Mathf.Clamp(_cameraRotationEuler.x, _cameraRotationAxisBMinLimit, _cameraRotationAxisBMaxLimit);
                _cameraRotationEuler.y = Mathf.Clamp(_cameraRotationEuler.y, _cameraRotationAxisAMinLimit, _cameraRotationAxisAMaxLimit);
            }
            _cameraRotation = Quaternion.Euler(_cameraRotationEuler);
            cameraTransform.rotation = _cameraRotation;
            cameraTransform.position = cameraTransform.rotation * new Vector3(0f, 0f, 0f - _currentDistance) + _targetPosition;
        }

        private void Start()
        {
            CameraBounds cameraBounds = FindObjectOfType<CameraBounds>();
            if (!(cameraBounds == null))
            {
                bounds = cameraBounds.Bounds;
                boundsDefaultSize = bounds.size;
            }
        }

        private void UpdateCameraTransform()
        {
            if (!shoudUpdateCameraPosition)
            {
                return;
            }
            if (bounds != null)
            {
                if (!IsInBounds())
                {
                    ShrinkBounds(value: true);
                    ReturnToBounds();
                }
                else
                {
                    ShrinkBounds(value: false);
                    cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraTransform.rotation * new Vector3(0f, 0f, 0f - _currentDistance) + _targetPosition, Time.deltaTime * _cameraFollowSpeed);
                }
            }
            else
            {
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraTransform.rotation * new Vector3(0f, 0f, 0f - _currentDistance) + _targetPosition, Time.deltaTime * _cameraFollowSpeed);
            }
        }

        private bool IsInBounds()
        {
            return bounds.bounds.Contains(cameraTransform.position);
        }

        private void ShrinkBounds(bool value)
        {
            if (value)
            {
                bounds.size = boundsDefaultSize * (1f - bounceDistance);
            }
            else
            {
                bounds.size = boundsDefaultSize;
            }
        }

        private void ReturnToBounds()
        {
            ResetTargetPosition();
            Vector3 vector = bounds.bounds.center;
            Vector3 b = (bounds.bounds.ClosestPoint(cameraTransform.position) - vector) * bounceDistance + vector;
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, b, Time.deltaTime * bounceSpeed);
        }

        private void OnMove(Vector3 screenPosition, Vector3 screenDelta)
        {
            OnCameraManualMove();
            if (!automaticCameraMovementEnabled)
            {
                cameraTransform.Translate(CalcMoveDelta(screenPosition, screenDelta), Space.World);
                _targetPosition = cameraTransform.position + cameraTransform.forward * _currentDistance;
            }
        }

        private void SetInitialCameraRotation()
        {
            _cameraRotation = Quaternion.identity;
            _cameraRotationEuler = _cameraRotation.eulerAngles;
            RotateAngleAxis(_initialAngleX, _initialAngleY);
        }

        public void StopFlyAround()
        {
            if (cameraTransform != null)
            {
                cameraTransform.DOKill();
            }
            StopAllCoroutines();
            shouldFlyAroundTarget = false;
        }

        public async UniTask FitToObjectsBounds(List<GameObject> goList, bool isInstant = false)
        {
            if (goList.Count != 0)
            {
                Bounds bounds = EncapsulateBounds(goList[0].transform);
                for (int i = 1; i < goList.Count; i++)
                {
                    bounds.Encapsulate(EncapsulateBounds(goList[i].transform));
                }
                await FitToBounds(bounds, isInstant);
            }
        }

        public async UniTask FitToTransformBounds(Transform t, bool isInstant = false)
        {
            if (t == null)
            {
                Debug.Log("FitToBounds: transform is null");
                return;
            }
            Bounds bounds = EncapsulateBounds(t);
            if (bounds.size.magnitude <= 0f)
            {
                bounds = _target.EncapsulateBounds();
            }
            await FitToBounds(bounds, isInstant);
        }

        private async UniTask FitToBounds(Bounds bounds, bool isInstant)
        {
            DisableCameraAnimation();
            if ((_isBuilInCameraExists && _isBuilInCameraAnimationEnabled) || _isCameraTransformTweenActive)
            {
                return;
            }
            shouldFlyAroundTarget = false;
            isMinDist = false;
            _targetBounds = bounds;
            _targetPosition = _targetBounds.center;
            float num = CalcFinalDistance(isModel: false);
            if (float.IsNaN(num))
            {
                return;
            }
            center = _targetBounds.center;
            _currentDistance = num;
            Vector3 vector = cameraTransform.rotation * new Vector3(0f, 0f, 0f - _currentDistance) + _targetPosition + _panOffset;
            if (isInstant)
            {
                cameraTransform.localPosition = vector;
                return;
            }
            _isCameraTransformTweenActive = true;
            automaticCameraMovementEnabled = true;
            await cameraTransform.DOMove(vector, 0.5f).SetEase(Ease.OutExpo).OnKill(delegate
                {
                    _isCameraTransformTweenActive = false;
                    automaticCameraMovementEnabled = false;
                })
                .AsyncWaitForCompletion();
        }

        private float CalcFinalDistance(bool isModel)
        {
            UnityEngine.Camera componentInChildren = cameraTransform.GetComponentInChildren<UnityEngine.Camera>();
            float magnitude = _targetBounds.extents.magnitude;
            float num = 2f * Mathf.Tan(0.5f * componentInChildren.fieldOfView * (MathF.PI / 180f));
            float num2 = ((!isModel) ? 2f : 3f) * magnitude / num;
            float num3 = num2 - magnitude;
            if (num3 < 0.01f)
            {
                num2 += 0.01f - num3;
            }
            return num2;
        }

        public void FocusOnObject(Transform t, List<GameObject> goList = null, bool shouldFlyAround = false)
        {
            DisableCameraAnimation();
            string text = ((goList == null) ? t.name : goList[0].name);
            if (!_isCameraTransformTweenActive || !(_currentFocusTargetName == text))
            {
                _currentFocusTargetName = text;
                FocusOnObjectInternal(t, goList, shouldFlyAround);
            }
        }

        public void ForceFocusOnObject(Transform t, List<GameObject> goList = null, bool shouldFlyAround = false)
        {
            DisableCameraAnimation();
            FocusOnObjectInternal(t, goList, shouldFlyAround);
        }

        private void FocusOnObjectInternal(Transform t, List<GameObject> goList, bool shouldFlyAround)
        {
            cameraTransform.DOKill();
            if (t == null)
            {
                Debug.Log("FitToBounds: transform is null");
                return;
            }
            shouldFlyAroundTarget = false;
            automaticCameraMovementEnabled = true;
            Bounds targetBounds = ((goList == null) ? EncapsulateBounds(t) : EncapsulateBounds(goList));
            _targetBounds = targetBounds;
            _targetPosition = _targetBounds.center;
            center = _targetBounds.center;
            float num = CalcFinalDistance(isModel: false);
            if (!float.IsNaN(num))
            {
                _currentDistance = num;
                _isCameraTransformTweenActive = true;
                Vector3 endValue = cameraTransform.rotation * new Vector3(0f, 0f, 0f - _currentDistance) + _targetPosition + _panOffset;
                cameraTransform.DOMove(endValue, 2f).SetEase(Ease.InOutCubic).OnKill(delegate
                {
                    _currentFocusTargetName = string.Empty;
                    _isCameraTransformTweenActive = false;
                    automaticCameraMovementEnabled = false;
                    shouldFlyAroundTarget = shouldFlyAround;
                });
            }
        }

        public static Bounds EncapsulateBounds(Transform t)
        {
            Collider[] componentsInChildren = t.GetComponentsInChildren<Collider>();
            Bounds result;
            if (componentsInChildren != null && componentsInChildren.Length != 0)
            {
                result = componentsInChildren[0].bounds;
                for (int i = 1; i < componentsInChildren.Length; i++)
                {
                    Collider collider = componentsInChildren[i];
                    if (!(collider.bounds.size.magnitude <= 0.01f))
                    {
                        result.Encapsulate(collider.bounds);
                    }
                }
            }
            else
            {
                result = default(Bounds);
            }
            return result;
        }

        public static Bounds EncapsulateBounds(List<GameObject> focusList)
        {
            Bounds bounds = default(Bounds);
            List<Collider> list = focusList.SelectMany((GameObject o) => o.GetComponentsInChildren<Collider>()).ToList();
            bounds = list[0].bounds;
            for (int i = 1; i < list.Count; i++)
            {
                Collider collider = list[i];
                bounds.Encapsulate(collider.bounds);
            }
            return bounds;
        }

        private Vector3 CalcMoveDelta(Vector3 screenPosition, Vector3 screenDelta)
        {
            UnityEngine.Camera component = cameraTransform.GetComponent<UnityEngine.Camera>();
            Ray ray = component.ScreenPointToRay(screenPosition - screenDelta);
            Ray ray2 = component.ScreenPointToRay(screenPosition);
            if (_movePlane.Raycast(ray2, out var enter) && _movePlane.Raycast(ray, out var enter2))
            {
                return ray.GetPoint(Mathf.Abs(enter2)) - ray2.GetPoint(Mathf.Abs(enter));
            }
            return Vector3.zero;
        }

        private void OnBeganMove(Vector3 screenPosition)
        {
            if (Vector3.Distance(cameraTransform.position, _targetPosition) < 0.1f)
            {
                Vector3 inPoint = cameraTransform.position + cameraTransform.forward;
                _movePlane = new Plane(cameraTransform.forward, inPoint);
            }
            else
            {
                _movePlane = new Plane(cameraTransform.forward, _targetPosition);
            }
        }
    }
}
