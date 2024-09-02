using System.Collections.Generic;
using System.Linq;
using __Scripts.Project.Data;
using Doozy.Runtime.Signals;
using TMPro;
using UnityEngine;
using Zenject;

namespace __Scripts.Project.Menu.UI
{
    public class LessonSearch : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        
        private Catalog _catalog;
        private MenuState _menuState;
        private Catalog.Subject _fallBackSubject;

        [Inject]
        private void Construct(Catalog catalog, MenuState menuState)
        {
            _menuState = menuState;
            _catalog = catalog;
        }
        
        private void OnEnable()
        {
            inputField.onValueChanged.AddListener(OnValueChanged);
            inputField.onSelect.AddListener(OnSelect);
            inputField.onDeselect.AddListener(OnDeselect);
        }

        private void OnDisable()
        {
            inputField.onValueChanged.RemoveListener(OnValueChanged);
            inputField.onSelect.RemoveListener(OnSelect);
            inputField.onDeselect.RemoveListener(OnDeselect);
        }

        private void OnSelect(string text)
        {
            if (string.IsNullOrEmpty(text))
                _fallBackSubject = _menuState.SelectedSubject.Value;
        }

        private void OnDeselect(string text)
        {
            if (string.IsNullOrEmpty(text))
                Fallback();
        }

        private void OnValueChanged(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                Fallback();
                return;
            }

            if (_menuState.SelectedSubject.Value is not { Key: "Search" })
                Signal.Send("Search", "Empty");
            
            IEnumerable<Catalog.Subject.Lesson> allLessons = _catalog.subjects.SelectMany(s => s.lessons);
            IEnumerable<Catalog.Subject.Lesson> filteredLessons = allLessons.Where(l => l.Name.ToLower().Contains(text.ToLower())).ToList();
            
            _menuState.SelectedSubject.Value = new Catalog.Subject("Search", "Поиск", "Поиск")
            {
                lessons = filteredLessons.ToArray()
            };
        }

        private void Fallback()
        {
            if (_fallBackSubject == null)
                Signal.Send("Search", "NoFallback");
            else
                _menuState.SelectedSubject.Value = _fallBackSubject;
        }
    }
}
