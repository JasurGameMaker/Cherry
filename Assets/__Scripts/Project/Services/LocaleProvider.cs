using __Scripts.Project.Localization;
using UnityEngine.Localization.Settings;

namespace __Scripts.Project.Services
{
    public static class LocaleProvider
    {
        public static Language SelectedLanguage {
            get
            {
                switch (LocalizationSettings.SelectedLocale.Identifier.Code)
                {
                    case "ru":
                        return Language.Ru;
                    case "en-US":
                        return Language.En;
                    case "uz":
                        return Language.Uzb;
                    default:
                        return Language.En;
                }
            }
        }
    }
}
