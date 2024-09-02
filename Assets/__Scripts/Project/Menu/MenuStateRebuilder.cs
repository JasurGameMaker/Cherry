using System;
using __Scripts.Project.Data;
using Client;
using Client.Scripts.Core.View.PageViews;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization.Settings;
using Zenject;

namespace __Scripts.Project.Menu
{
    public class MenuStateRebuilder : MonoBehaviour
    {
        [SerializeField] private WindowsManager windowsManager;
        
        private Catalog.Subject _lastSubject;

        [Inject]
        private void Construct([InjectOptional]Catalog.Subject subject)
        {
            _lastSubject = subject;
        }

        private void Start()
        {
            if (_lastSubject != null)
                RestoreWindow();
        }

        private void RestoreWindow()
        {
            windowsManager
                .GetWindow<SubjectPageView>()
                .Init(_lastSubject);
            
            windowsManager.OpenWindow<SubjectPageView>(overrideTable: LocalizationTables.SubjectKey);
        }
    }
}
