using System.Linq;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;

namespace __Scripts.Project.Menu.UI.New
{
    public class SubjectSearch : MonoBehaviour
    {
        [SerializeField] private SubjectViewSelector[] selectors;
        [SerializeField] private TMP_InputField inputField;

        private void OnEnable() =>
            inputField.onValueChanged.AddListener(OnValueChanged);

        private void OnDisable() =>
            inputField.onValueChanged.RemoveListener(OnValueChanged);

        private void OnValueChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                selectors.ForEach(s => s.gameObject.SetActive(true));
                return;
            }
            
            foreach (SubjectViewSelector selector in selectors)
            {
                if (selector.Subject == null)
                {
                    selector.gameObject.SetActive(false);
                    continue;
                }
                selector.gameObject.SetActive(selector.Subject.Name.ToLower().Contains(value.ToLower()));
            }
        }
    }
}
