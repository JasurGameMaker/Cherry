using Doozy.Runtime.UIManager.Components;
using UnityEngine;

namespace __Scripts.Project.Menu.UI.Utils
{
    [RequireComponent(typeof(UIButton))]
    public abstract class DoozyButtonListener : MonoBehaviour
    {
        private UIButton _button;

        private void Awake() =>
            _button = GetComponent<UIButton>();

        private void OnEnable() =>
            _button.onClickEvent.AddListener(OnClick);

        private void OnDisable() =>
            _button.onClickEvent.RemoveListener(OnClick);

        protected abstract void OnClick();
    }
}
