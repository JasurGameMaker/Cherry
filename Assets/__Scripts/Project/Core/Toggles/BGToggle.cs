using __Scripts.Project.Menu.UI.Utils;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

namespace __Scripts.Project.Core.Toggles
{
    public class BGToggle : DoozyToggleListener
    {
        [SerializeField] private UIView toggleView;
        
        private Transform _layout;

        private void Start() =>
            _layout = toggleView.transform.GetChild(0);
        
        protected override void OnToggle(bool state)
        {
            if (state)
            {
                UpdatePosition();
                toggleView.Show();
            }
            else
                toggleView.Hide();
        }

        private void UpdatePosition() =>
            _layout.position = new Vector3(_layout.position.x, transform.position.y);
    }
}
