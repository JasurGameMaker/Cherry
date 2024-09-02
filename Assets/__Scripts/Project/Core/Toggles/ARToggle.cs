using __Scripts.Project.Menu.UI.Utils;
using UnityEngine;

namespace __Scripts.Project.Core.Toggles
{
    public class ARToggle : DoozyToggleListener
    {
        [SerializeField] private CameraSwitcher cameraSwitcher;
        
        protected override void OnToggle(bool state)
        {
            cameraSwitcher.ChangeCameraMode(state);
        }
    }
}
