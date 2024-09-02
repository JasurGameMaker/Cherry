using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace __Scripts.Project.Menu.UI.Settings
{
    public class LanguageDropdown : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropdown;

        private void Awake() =>
            dropdown.value = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);

        private void OnEnable() =>
            dropdown.onValueChanged.AddListener(OnValueChanged);

        private void OnDisable() =>
            dropdown.onValueChanged.RemoveListener(OnValueChanged);

        private void OnValueChanged(int value) =>
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[value];
    }
}
