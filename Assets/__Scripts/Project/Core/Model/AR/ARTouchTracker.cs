using UnityEngine;

namespace __Scripts.Project.Core.Model.AR
{
    public class ARTouchTracker : MonoBehaviour
    {
        [SerializeField] private Transform modelRoot;
        
        private float _initialDistance;
        private Vector3 _initialScale;
        private Vector3 _resetScale;

        private void OnEnable() =>
            _resetScale = modelRoot.transform.localScale;

        private void OnDisable() =>
            modelRoot.transform.localScale = _resetScale;

        private void Update()
        {
            if (Input.touchCount != 2)
                return;

            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            if (touch0.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Ended ||
                touch0.phase == TouchPhase.Canceled || touch1.phase == TouchPhase.Canceled)
                return;

            if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
            {
                _initialDistance = Vector2.Distance(touch0.position, touch1.position);
                _initialScale = modelRoot.transform.localScale;
            }
            else
            {
                if (Mathf.Approximately(_initialDistance, 0))
                    return;
                
                float currentDistance = Vector2.Distance(touch0.position, touch1.position);
                modelRoot.transform.localScale = _initialScale * (currentDistance / _initialDistance);
            }
        }
    }
}
