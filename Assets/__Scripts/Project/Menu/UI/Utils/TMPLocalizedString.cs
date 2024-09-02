using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace __Scripts.Project.Menu.UI.Utils
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TMPLocalizedString : LocalizeStringEvent
    {
        private TextMeshProUGUI _text;
        
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            OnUpdateString.AddListener(SetText);
        }
        
        private void SetText(string t) =>
            _text.text = t;
    }
}
