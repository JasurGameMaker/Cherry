using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace __Scripts.Project.Core.Model.Socket
{
	[RequireComponent(typeof(SocketVisualStateController))]
	public class SocketController : MonoBehaviour, IDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler
	{
		private SocketVisualStateController _visualStateController;

		private Camera _camera;

		public Action<GameObject, bool> grabbedStateChanged;

		public UnityEvent OnMovementStartEvent = new UnityEvent();

		public UnityEvent OnMovementEndEvent = new UnityEvent();

		public Action ModelDragged;

		public bool isGrabbed;

		private bool _isInSnapDistance;

		private float _objectFollowSpeed = 1f;

		private float _rayDistance;

		private Vector3 _hitPointOffset;

		private Vector3 currentPos;

		private Quaternion currentRot;

		private bool _noHintMode;

		private Vector3 _dragBeginPosition;

		private Quaternion _dragBeginRotation;

		private Vector3 _dragBeginScale;

		private ManipulationMode _currentMode = ManipulationMode.Disabled;

		public Action processed;

		private bool isAttachedToSocket = true;

		private Vector3 initialPosition;

		private Quaternion initialRotation;

		private Shader _shader;

		private bool _isSnapTweenActive;

		private float _orbitSpeed = 0.5f;

		public event Action<bool> OnAnimRoutineLoaded;

		public bool IsDraggaable()
		{
			return _currentMode == ManipulationMode.Drag;
		}

		public void SetIsAttachedState(bool newValue)
		{
			isAttachedToSocket = newValue;
		}

		public void EnableHintMode(bool enable)
		{
			_noHintMode = !enable;
		}

		public void SwitchManipulationMode(ManipulationMode mode)
		{
			_currentMode = mode;
		}

		public bool IsInPlace()
		{
			return isAttachedToSocket;
		}

		public void Init(Shader shader, Camera cam)
		{
			_shader = shader;
			_camera = cam;
			_visualStateController = GetComponent<SocketVisualStateController>();
			initialPosition = transform.position;
			initialRotation = transform.rotation;
			isAttachedToSocket = true;
		}

		public bool CanGrab()
		{
			if (!IsDraggaable())
			{
				return false;
			}
			return !_isSnapTweenActive;
		}

		private void InvokeGrabbedStateChange(bool grabbedState)
		{
			isGrabbed = grabbedState;
			SocketController[] componentsInChildren = base.transform.GetComponentsInChildren<SocketController>();
			foreach (SocketController socketController in componentsInChildren)
			{
				socketController.grabbedStateChanged?.Invoke(socketController.gameObject, isGrabbed);
			}
		}

		private void UpdateDistanceFromSocket()
		{
			Vector3 vector = _camera.WorldToScreenPoint(initialPosition);
			if ((_camera.WorldToScreenPoint(base.gameObject.transform.position) - vector).magnitude <= 100f)
			{
				_isInSnapDistance = true;
				if (!_noHintMode)
				{
					_visualStateController?.SwitchSocketVisualState(SocketVisualState.Overlapping);
				}
			}
			else
			{
				_isInSnapDistance = false;
				if (!_noHintMode)
				{
					_visualStateController?.SwitchSocketVisualState(SocketVisualState.Visible);
				}
			}
		}

		private bool SnapObjectToSocket()
		{
			if (_isInSnapDistance)
			{
				StartSnapAnimation(goHome: true);
			}
			return _isInSnapDistance;
		}

		public void StartSnapAnimation(bool goHome)
		{
			if (!_isSnapTweenActive)
			{
				_isSnapTweenActive = true;
				this.OnAnimRoutineLoaded?.Invoke(_isSnapTweenActive);
				Sequence sequence = DOTween.Sequence();
				sequence.Append(base.gameObject.transform.DOMove(goHome ? initialPosition : currentPos, 0.5f).SetEase(Ease.OutExpo));
				sequence.Join(base.gameObject.transform.DORotateQuaternion(goHome ? initialRotation : currentRot, 0.5f).SetEase(Ease.OutExpo));
				sequence.OnComplete(ResetSocket);
			}
		}

		private void ResetSocket()
		{
			OnSnapFinish();
		}

		private void OnSnapFinish()
		{
			_isSnapTweenActive = false;
			isAttachedToSocket = true;
			this.OnAnimRoutineLoaded?.Invoke(_isSnapTweenActive);
			CheckChildrenIsAttached();
			InvokeGrabbedStateChange(grabbedState: false);
		}

		private void CheckChildrenIsAttached()
		{
			SocketController[] componentsInChildren = base.gameObject.GetComponentsInChildren<SocketController>();
			foreach (SocketController socketController in componentsInChildren)
			{
				socketController.SetIsAttachedState(Vector3.Distance(socketController.transform.position, socketController.initialPosition) < 1E-06f);
			}
		}

		public bool IsSnapTweenRunning()
		{
			return _isSnapTweenActive;
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left && !Input.GetKey(KeyCode.LeftControl))
			{
				switch (_currentMode)
				{
					case ManipulationMode.Drag:
						if (!_noHintMode)
						{
							_visualStateController?.Init(_shader, initialPosition, initialRotation);
						}
						OnMovementStartEvent.Invoke();
						break;
					case ManipulationMode.Disabled:
						return;
				}
				InitDragParameters(eventData);
			}
			if (!isAttachedToSocket)
			{
				currentPos = base.transform.position;
				currentRot = base.transform.rotation;
			}
		}

		public void OnDrag(PointerEventData data)
		{
			if (data.button != 0 || Input.GetKey(KeyCode.LeftControl))
			{
				return;
			}
			processed?.Invoke();
			switch (_currentMode)
			{
				case ManipulationMode.Drag:
					if (isGrabbed)
					{
						UpdateObjectPosition(data);
						UpdateDistanceFromSocket();
						ModelDragged?.Invoke();
					}
					break;
				case ManipulationMode.Rotation:
					Rotate(data);
					break;
				case ManipulationMode.Zoom:
					Zoom(data);
					break;
				case ManipulationMode.Disabled:
					break;
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left && !Input.GetKey(KeyCode.LeftControl))
			{
				_visualStateController?.SwitchSocketVisualState(SocketVisualState.Hidden);
				bool flag = false;
				switch (_currentMode)
				{
					case ManipulationMode.Drag:
						flag = SnapObjectToSocket();
						OnMovementEndEvent.Invoke();
						break;
					case ManipulationMode.Disabled:
						return;
				}
				isGrabbed = false;
				if (!flag)
				{
					InvokeGrabbedStateChange(grabbedState: false);
				}
			}
		}

		private void UpdateObjectPosition(PointerEventData data)
		{
			Ray ray = data.pressEventCamera.ScreenPointToRay(Input.mousePosition);
			Vector3 b = ray.origin + ray.direction * _rayDistance + _hitPointOffset;
			base.transform.position = Vector3.Lerp(base.transform.position, b, Time.time * _objectFollowSpeed);
		}

		private void InitDragParameters(PointerEventData eventData)
		{
			if (CanGrab())
			{
				_rayDistance = eventData.pointerPressRaycast.distance;
				_hitPointOffset = base.transform.position - eventData.pointerPressRaycast.worldPosition;
				isAttachedToSocket = false;
				InvokeGrabbedStateChange(grabbedState: true);
			}
		}

		private void Zoom(PointerEventData evData)
		{
			Vector3 center = base.gameObject.GetComponent<Renderer>().bounds.center;
			Vector3 vector = center - base.gameObject.transform.position;
			float num = -2f * evData.delta.y / Mathf.Abs(evData.delta.y);
			if (!float.IsNaN(num))
			{
				Vector3 position = _camera.transform.position;
				Vector3 vector2 = Vector3.MoveTowards(center, position, num * evData.delta.magnitude / new Vector2(Screen.width, Screen.height).magnitude);
				base.gameObject.transform.position = vector2 - vector;
				isAttachedToSocket = false;
			}
		}

		private void Rotate(PointerEventData evData)
		{
			Vector2 delta = evData.delta;
			Vector3 axis = _camera.transform.TransformDirection(new Vector3(delta.y, 0f - delta.x, 0f));
			Vector3 center = base.gameObject.GetComponent<Renderer>().bounds.center;
			base.gameObject.transform.RotateAround(center, axis, delta.magnitude * _orbitSpeed);
			isAttachedToSocket = false;
		}
	}
}
