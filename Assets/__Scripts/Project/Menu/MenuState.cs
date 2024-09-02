using System.Collections.Generic;
using __Scripts.Project.Data.Enums;
using __Scripts.Project.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace __Scripts.Project.Data
{
    public class MenuState
    {
        private const string FavKey = "Fav";
        private const string ThemeKey = "ThemeColor";
        private const string VolumeKey = "Volume";
        
        public ReactiveProperty<Catalog.Subject> SelectedSubject { get; private set; } = new ();
        
        public ReactiveProperty<ThemeColor> ColorTheme { get; } = new ();
        public float VolumeNorm {
            get => PlayerPrefs.GetFloat(VolumeKey, 0.5f);
            set => PlayerPrefs.SetFloat(VolumeKey, value);
        }

        public List<string> FavoriteLessonNames { get; } = 
            PlayerPrefs.HasKey(FavKey) ?
            JsonConvert.DeserializeObject<List<string>>(PlayerPrefs.GetString(FavKey)) :
            new List<string>();

        public ThemeColor GetSavedTheme() =>
            (ThemeColor)PlayerPrefs.GetInt(ThemeKey);

        public void Save()
        {
            PlayerPrefs.SetString(FavKey, JsonConvert.SerializeObject(FavoriteLessonNames));
            PlayerPrefs.SetInt(ThemeKey, (int)ColorTheme.Value);
            
            PlayerPrefs.Save();
        }
    }
}
