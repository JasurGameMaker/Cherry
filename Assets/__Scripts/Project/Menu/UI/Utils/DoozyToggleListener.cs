using Doozy.Runtime.UIManager.Components;
using UnityEngine;

namespace __Scripts.Project.Menu.UI.Utils
{
    [RequireComponent(typeof(UIToggle))]
    public abstract class DoozyToggleListener : MonoBehaviour
    {
        public UIToggle Toggle { get; private set; }

        private void Awake() =>
            Toggle = GetComponent<UIToggle>();

        protected void OnEnable()
        {
            Toggle.OnValueChangedCallback.AddListener(OnToggle);
            OnAfterEnable();
        }

        protected void OnDisable()
        {
            Toggle.OnValueChangedCallback.RemoveListener(OnToggle);
            OnAfterDisable();
        }

        protected virtual void OnAfterEnable()
        {
        }

        protected virtual void OnAfterDisable()
        {
        }

        public void SetInteractable(bool state) =>
            Toggle.interactable = state;

        protected abstract void OnToggle(bool state);
    }
}
