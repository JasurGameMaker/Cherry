using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using __Scripts.Project.Core.Model.Socket;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;

namespace __Scripts.Project.Core
{
	public class TouchGesturesTracker : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler, IScrollHandler, IPointerUpHandler, IPointerDownHandler
	{
		private bool isPinching;

		private int PinchPointerId = -1;

		private float previousPinchDistance;

		private bool isMoving;

		private int PanPointerId = -1;

		private Vector2 previousPanPosition;

		private bool isSwiping;

		private int SwipePointerId = -1;

		public Action<Vector2> Swiped;

		public Action<float, Vector3> Pinched;

		public Action<float, Vector3> ScrollZoomed;

		public Action<Vector3, Vector3> Moved;

		public Action BeganDrag;

		public Action EndedDrag;

		public Action<Vector3> BeganMove;

		public Action DoubleClicked;

		public Action BackgroundClicked;

		private bool isScrollProcessed;

		private bool isMoveProcessed;

		private bool isPanProcessed;

		private Vector3 lastMousePosition;

		private bool BeginDragInvoked;

		private Vector3 touchBeginPosition;

		private Vector3 touchEndPosition;

		private bool gestureStartedOverModel;

		private bool gestureStartedOverUI;

		private bool touchMoved;

		private Vector3 lastPointerClickPosition;

		private double lastPointerClickTime;

		private double pointerClickDelay = 0.3499999940395355;

		private double singleClickTime = 0.10000000149011612;

		private float clickRadius = 60f;

		private bool hasClickProcessed;

		private Stopwatch pointerStopWatch = new Stopwatch();

		private void LateUpdate()
		{
			UpdateMouseInput();
			UpdateTouchInput();
		}

		private void UpdateMouseInput()
		{
			if (Input.touchSupported && Input.touchCount > 0)
			{
				return;
			}
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
			{
				List<RaycastResult> raycastResults = GetRaycastResults(Input.mousePosition);
				gestureStartedOverModel = IsPointingOverModel(raycastResults);
				gestureStartedOverUI = IsPointingOverUI(raycastResults);
			}
			if (!gestureStartedOverModel && !gestureStartedOverUI)
			{
				if (Input.GetMouseButtonDown(0) && !BeginDragInvoked)
				{
					BeganDrag?.Invoke();
				}
				if (Input.GetMouseButtonUp(0))
				{
					BeginDragInvoked = false;
				}
				if (Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftControl) && !isMoveProcessed)
				{
					Vector2 obj = Input.mousePosition - lastMousePosition;
					if (obj.magnitude > 0f)
					{
						Swiped?.Invoke(obj);
					}
				}
			}
			if (!gestureStartedOverUI)
			{
				if (Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(1) || (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl)))
				{
					BeganMove?.Invoke(Input.mousePosition);
				}
				if ((Input.GetMouseButton(2) || Input.GetMouseButton(1) || (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl))) && !isPanProcessed)
				{
					Vector2 vector = Input.mousePosition - lastMousePosition;
					if (vector.magnitude > 0f)
					{
						Moved?.Invoke(Input.mousePosition, vector);
					}
				}
			}
			if (Math.Abs(0f - Input.GetAxis("Mouse ScrollWheel")) > 0f)
			{
				List<RaycastResult> raycastResults2 = GetRaycastResults(Input.mousePosition);
				if (!IsPointingOverUI(raycastResults2))
				{
					float num = Input.mouseScrollDelta.y;
					if (Input.GetKey(KeyCode.LeftControl))
					{
						num /= 5f;
					}
					if (!isScrollProcessed)
					{
						ScrollZoomed?.Invoke(num, Input.mousePosition);
					}
				}
			}
			lastMousePosition = Input.mousePosition;
			isScrollProcessed = false;
			hasClickProcessed = false;
		}

		private void UpdateTouchInput()
		{
			if (!Input.touchSupported)
			{
				return;
			}
			if (Input.touchCount == 0)
			{
				isPinching = false;
				return;
			}
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began)
			{
				List<RaycastResult> raycastResults = GetRaycastResults(Input.mousePosition);
				gestureStartedOverModel = IsPointingOverModel(raycastResults);
				gestureStartedOverUI = IsPointingOverUI(raycastResults);
			}
			if (gestureStartedOverModel || gestureStartedOverUI)
			{
				return;
			}
			if (Input.touchCount >= 2)
			{
				if (!isPinching)
				{
					isPinching = CheckIsPinching();
					if (isPinching)
					{
						previousPinchDistance = GetCurrentPinchDistance();
					}
				}
				if (!isPinching && !isMoveProcessed)
				{
					switch (touch.phase)
					{
						case TouchPhase.Began:
							BeganMove?.Invoke(touch.position);
							break;
						case TouchPhase.Moved:
							Moved?.Invoke(touch.position, touch.deltaPosition);
							break;
					}
				}
			}
			if (isPinching && !isMoving && Input.touchCount >= 2)
			{
				float arg = CalcPinchDelta();
				Touch touch2 = Input.GetTouch(1);
				Pinched?.Invoke(arg, (touch.position + touch2.position) / 2f);
			}
			if (Input.touchCount != 1 || isMoving)
			{
				return;
			}
			switch (touch.phase)
			{
				case TouchPhase.Began:
					touchMoved = false;
					break;
				case TouchPhase.Moved:
					touchMoved = true;
					Swiped?.Invoke(touch.deltaPosition);
					break;
				case TouchPhase.Ended:
					if (touchMoved)
					{
						EndedDrag?.Invoke();
					}
					break;
				case TouchPhase.Stationary:
					break;
			}
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			BeganDrag?.Invoke();
			BeginDragInvoked = true;
			if (Input.touchSupported)
			{
				if (Input.touchCount >= 2 && !isPinching)
				{
					isPinching = CheckIsPinching();
					if (isPinching)
					{
						PinchPointerId = eventData.pointerId;
						previousPinchDistance = GetCurrentPinchDistance();
					}
				}
				if (Input.touchCount == 2 && !isMoving)
				{
					isMoving = true;
					PanPointerId = eventData.pointerId;
					BeganMove?.Invoke(eventData.position);
				}
				if (Input.touchCount >= 2 && !isMoving && !isPinching)
				{
					SwipePointerId = eventData.pointerId;
				}
			}
			if (eventData.button == PointerEventData.InputButton.Right || eventData.button == PointerEventData.InputButton.Middle || (eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftControl)))
			{
				BeganMove?.Invoke(eventData.position);
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (Input.touchSupported)
			{
				if (Input.touchCount >= 2)
				{
					if (!isPinching)
					{
						isPinching = CheckIsPinching();
						if (isPinching)
						{
							PinchPointerId = eventData.pointerId;
							previousPinchDistance = GetCurrentPinchDistance();
						}
					}
					isSwiping = CheckIsMoving();
				}
				if (isPinching && eventData.pointerId == PinchPointerId && Input.touchCount >= 2)
				{
					float arg = CalcPinchDelta();
					Pinched?.Invoke(arg, eventData.pressPosition);
				}
				if (Input.touchCount == 1 && eventData.pointerId == PanPointerId && !isPinching)
				{
					Swiped?.Invoke(eventData.delta);
				}
				if (isSwiping && SwipePointerId == eventData.pointerId && eventData.dragging)
				{
					Moved?.Invoke(eventData.position, eventData.delta);
					isMoveProcessed = true;
				}
			}
			if (eventData.button == PointerEventData.InputButton.Right || eventData.button == PointerEventData.InputButton.Middle || (eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftControl)))
			{
				Moved?.Invoke(eventData.position, eventData.delta);
				isPanProcessed = true;
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			isMoving = false;
			isPinching = false;
			PanPointerId = -1;
			PinchPointerId = -1;
			SwipePointerId = -1;
			BeginDragInvoked = false;
			isPanProcessed = false;
			isMoveProcessed = false;
			EndedDrag?.Invoke();
		}

		public void OnScroll(PointerEventData eventData)
		{
			float num = eventData.scrollDelta.y;
			if (Input.GetKey(KeyCode.LeftControl))
			{
				num /= 5f;
			}
			ScrollZoomed?.Invoke(num, Input.mousePosition);
			isScrollProcessed = true;
		}

		private float CalcPinchDelta()
		{
			float currentPinchDistance = GetCurrentPinchDistance();
			float value = (currentPinchDistance - previousPinchDistance) / previousPinchDistance;
			previousPinchDistance = currentPinchDistance;
			return Mathf.Clamp(value, -1f, 1f);
		}

		private static float GetCurrentPinchDistance()
		{
			return (Input.touches[0].position - Input.touches[1].position).magnitude;
		}

		private bool CheckIsPinching()
		{
			Vector2 deltaPosition = Input.touches[0].deltaPosition;
			Vector2 deltaPosition2 = Input.touches[1].deltaPosition;
			return Vector2.Dot(deltaPosition, deltaPosition2) < 0f;
		}

		private bool CheckIsMoving()
		{
			Vector2 deltaPosition = Input.touches[0].deltaPosition;
			Vector2 deltaPosition2 = Input.touches[1].deltaPosition;
			return Vector2.Dot(deltaPosition, deltaPosition2) > 0f;
		}

		private bool IsPointingOverModel(List<RaycastResult> raycastResults)
		{
			return (from result in raycastResults
				select result.gameObject.GetComponent<SocketController>() into controller
				where controller != null
				where controller.enabled
				select controller).Any((SocketController controller) => controller.IsDraggaable());
		}

		private static List<RaycastResult> GetRaycastResults(Vector3 pointerPosition)
		{
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = pointerPosition;
			List<RaycastResult> list = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerEventData, list);
			return list;
		}

		private bool IsPointingOverUI(List<RaycastResult> raycastResults)
		{
			return raycastResults.Any((RaycastResult result) => result.gameObject.layer == LayerMask.NameToLayer("UI"));
		}

		private void CheckDoubleClick(Vector3 position)
		{
			float num = Vector3.Distance(lastPointerClickPosition, position);
			bool num2 = num < clickRadius;
			double num3 = (double)Time.time - lastPointerClickTime;
			bool flag = num3 < pointerClickDelay;
			if (num2 && flag)
			{
				Debug.Log("Distance = " + num + " Clicks delta time = " + num3);
				DoubleClicked?.Invoke();
			}
			else
			{
				pointerStopWatch.Stop();
				if (pointerStopWatch.Elapsed.TotalSeconds < singleClickTime)
				{
					BackgroundClicked?.Invoke();
				}
			}
			lastPointerClickTime = Time.time;
			lastPointerClickPosition = position;
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (isPinching || isMoving || isMoveProcessed)
			{
				return;
			}
			if (eventData.clickCount == 2 && eventData.button == PointerEventData.InputButton.Left)
			{
				DoubleClicked?.Invoke();
				return;
			}
			if (Input.touchSupported && Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Ended && touch.deltaPosition.magnitude > 0f)
				{
					return;
				}
			}
			CheckDoubleClick(eventData.position);
			hasClickProcessed = true;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			pointerStopWatch.Reset();
			pointerStopWatch.Start();
		}
	}
}
