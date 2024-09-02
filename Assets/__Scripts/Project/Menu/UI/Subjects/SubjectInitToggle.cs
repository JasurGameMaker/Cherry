using __Scripts.Project.Data;
using __Scripts.Project.Data.Enums;
using __Scripts.Project.Menu.UI.Utils;
using UnityEngine;
using Zenject;

namespace __Scripts.Project.Menu.UI.Subjects
{
    public class SubjectInitToggle : DoozyToggleListener
    {
        [SerializeField] private SubjectType subjectType;
        
        private Catalog _catalog;
        private MenuState _menuState;

        [Inject]
        private void Construct(Catalog catalog, MenuState menuState)
        {
            _menuState = menuState;
            _catalog = catalog;
        }
        
        protected override void OnToggle(bool state)
        {
            if (state)
                _menuState.SelectedSubject.Value = _catalog.FindSubject(subjectType);
        }
    }
}
