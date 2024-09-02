using TMPro;
using UnityEngine;

namespace __Scripts.Project.Core.UI
{
    public class ModelSearchField : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private RectTransform root;
        
        private void OnEnable() =>
            inputField.onValueChanged.AddListener(OnValueChanged);

        private void OnDisable() =>
            inputField.onValueChanged.RemoveListener(OnValueChanged);

        private void OnValueChanged(string text)
        {
            var textLower = text.ToLower();
            foreach (RectTransform child in root)
            {
                var entry = child.GetComponent<ModelListEntry>();
                entry.gameObject.SetActive(text == string.Empty || entry.Text.Contains(textLower));
            }
        }
    }
}
