using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace __Scripts.Project.Menu.UI.New
{
    public class LanguageButton : MonoBehaviour
    {
        [SerializeField] private int localeIndex;

        private void Start()
        {
            OnSelectedLocaleChanged(LocalizationSettings.SelectedLocale);
            LocalizationSettings.SelectedLocaleChanged += OnSelectedLocaleChanged;
        }

        private void OnDestroy() =>
            LocalizationSettings.SelectedLocaleChanged -= OnSelectedLocaleChanged;

        private void OnSelectedLocaleChanged(Locale locale) =>
            gameObject.SetActive(localeIndex != LocalizationSettings.AvailableLocales.Locales.IndexOf(locale));

        public void OnClick() =>
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeIndex];
    }
}
