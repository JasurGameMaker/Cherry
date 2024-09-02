using __Scripts.Project.Core.Model;
using __Scripts.Project.Core.UI;
using __Scripts.Project.Menu.UI.Utils;
using UnityEngine;
using Zenject;

namespace __Scripts.Project.Core.Toggles
{
    public class PartListToggle : DoozyToggleListener
    {
        [SerializeField] private ModelPartList modelPartList;
        
        private CourseModelInitializer _modelInitializer;

        [Inject]
        private void Construct(CourseModelInitializer modelInitializer)
        {
            _modelInitializer = modelInitializer;
        }
        
        protected override void OnAfterEnable() =>
            _modelInitializer.Initialized += OnInitialize;

        protected override void OnAfterDisable() =>
            _modelInitializer.Initialized -= OnInitialize;
        
        private void OnInitialize() =>
            modelPartList.Init();

        protected override void OnToggle(bool state) =>
            modelPartList.gameObject.SetActive(state);
    }
}
