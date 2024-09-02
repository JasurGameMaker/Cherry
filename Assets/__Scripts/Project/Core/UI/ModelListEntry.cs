using __Scripts.Project.Core.Model;
using __Scripts.Project.Menu.UI.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace __Scripts.Project.Core.UI
{
    public class ModelListEntry : DoozyButtonListener
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private TextMeshProUGUI index;
        [SerializeField] private LocalizeStringEvent stringEvent;

        public string Text => label.text.ToLower();
        
        private AtlasCameraManager _cameraManager;
        private CourseMesh _courseMesh;
        private CoreState _coreState;

        public ModelListEntry SetCamera(AtlasCameraManager cameraManager)
        {
            _cameraManager = cameraManager;
            return this;
        }
        
        public ModelListEntry SetText(string tableReference, string entryKey)
        {
            stringEvent.SetTable(tableReference);
            stringEvent.SetEntry(entryKey);

            index.SetText((transform.GetSiblingIndex() + 1).ToString());
            return this;
        }
        
        public ModelListEntry SetTargetMesh(CourseMesh courseMesh)
        {
            _courseMesh = courseMesh;
            return this;
        }

        public ModelListEntry SetState(CoreState coreState)
        {
            _coreState = coreState;
            return this;
        }

        protected override void OnClick()
        {
            _coreState.SelectedMesh.Value = _courseMesh;
            _cameraManager.FocusOnModelPart(_courseMesh.gameObject);
        }
    }
}
