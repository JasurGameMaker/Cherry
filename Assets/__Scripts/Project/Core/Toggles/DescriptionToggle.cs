using __Scripts.Project.Core.Model;
using __Scripts.Project.Core.UI;
using __Scripts.Project.Menu.UI.Utils;
using UnityEngine;
using Zenject;

namespace __Scripts.Project.Core.Toggles
{
    public class DescriptionToggle : DoozyToggleListener
    {
        [SerializeField] private DescriptionView description;
        
        private CoreState _coreState;

        [Inject]
        private void Construct(CoreState coreState)
        {
            _coreState = coreState;
        }
        
        protected override void OnAfterEnable() =>
            _coreState.SelectedMesh.ValueChanged += OnSelectedChanged;

        protected override void OnAfterDisable() =>
            _coreState.SelectedMesh.ValueChanged -= OnSelectedChanged;

        private void OnSelectedChanged(CourseMesh courseMesh)
        {
            if (string.IsNullOrEmpty(courseMesh.MeshData.descriptionKey))
                return;
            
            description.SetDescription(courseMesh.MeshData.tableReference, courseMesh.MeshData.descriptionKey, courseMesh.MeshData.titleKey);
        }

        protected override void OnToggle(bool state) =>
            description.gameObject.SetActive(state);
    }
}
