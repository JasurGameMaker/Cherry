using UnityEngine;
using UnityEngine.UI;

namespace __Scripts.Project.Menu.UI.Utils
{
    public class FoldoutButton : DoozyButtonListener
    {
        [SerializeField] private RectTransform objToFold;
        [SerializeField] private RectTransform rootLayout;

        private void Start()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rootLayout);
            SetState(false);
        }

        protected override void OnClick() =>
            SetState(!objToFold.gameObject.activeSelf);

        public void SetState(bool value)
        {
            objToFold.gameObject.SetActive(value);
            LayoutRebuilder.ForceRebuildLayoutImmediate(rootLayout);
        }
    }
}
