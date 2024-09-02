using System;
using __Scripts.Project.Data;
using __Scripts.Project.Data.Enums;
using Client;
using Client.Scripts.Core.View.PageViews;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace __Scripts.Project.Menu.UI.New
{
    public class SubjectViewSelector : MonoBehaviour
    {
        [SerializeField] private SubjectType subjectType;
        [SerializeField] private WindowsManager windowsManager;

        public Catalog.Subject Subject { get; private set; }
        
        private Button _button;
        private Catalog _catalog;

        [Inject]
        private void Construct(Catalog catalog)
        {
            _catalog = catalog;
        }
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            Subject = _catalog.FindSubject(subjectType);

            if (Subject == null)
                _button.interactable = false;
        }

        private void OnEnable() =>
            _button.onClick.AddListener(OnClick);

        private void OnDisable() =>
            _button.onClick.RemoveListener(OnClick);

        private void OnClick()
        {
            windowsManager
                .GetWindow<SubjectPageView>()
                .Init(Subject);
            
            windowsManager.OpenWindow<SubjectPageView>(overrideTable: LocalizationTables.SubjectKey);
        }
    }
}
