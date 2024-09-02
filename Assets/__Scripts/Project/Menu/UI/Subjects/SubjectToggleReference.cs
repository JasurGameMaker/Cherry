using __Scripts.Project.Menu.UI.Utils;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;

namespace __Scripts.Project.Menu.UI.Subjects
{
    public class SubjectToggleReference : DoozyButtonListener
    {
        [SerializeField] private UIToggle referencedToggle;
        [SerializeField] private FoldoutButton foldoutButton;

        protected override void OnClick()
        {
            foldoutButton.SetState(true);
            referencedToggle.isOn = true;
        }
    }
}
