using System;
using System.Linq;
using __Scripts.Project.Data.Enums;
using __Scripts.Project.Localization;
using __Scripts.Project.Services;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace __Scripts.Project.Data
{
    [JsonObject(MemberSerialization.Fields)]
    [Serializable]
    public class Catalog
    {
        public int version;
        public Subject[] subjects;

        [CanBeNull]
        public Subject FindSubject(SubjectType subjectType) =>
            subjects.FirstOrDefault(s => s.Key == subjectType.ToString());

        public Subject FindSubject(Subject.Lesson lesson) =>
            subjects.FirstOrDefault(s => s.lessons.Contains(lesson));

        [JsonObject(MemberSerialization.Fields)]
        [Serializable]
        public class Subject
        {
            [OnValueChanged("OnSubjectTypeChanged")]
            [JsonIgnore]
            [SerializeField] private SubjectType subjectType;
            
            [SerializeField, ReadOnly] private string name_en;
            [SerializeField] private string name_ru;
            [SerializeField] private string name_uzb;

            public Subject(string nameEn, string nameRu, string nameUzb)
            {
                name_en = nameEn;
                name_ru = nameRu;
                name_uzb = nameUzb;
            }
            
            private void OnSubjectTypeChanged() =>
                name_en = subjectType.ToString();

            public string Key => name_en;
            public string Name {
                get
                {
                    switch (LocaleProvider.SelectedLanguage)
                    {
                        case Language.En:
                            return Key;
                        case Language.Ru:
                            return name_ru;
                        case Language.Uzb:
                            return name_uzb;
                        default:
                            return Key;
                    }
                }
            }

            [SerializeField] private string videoLink_en;
            [SerializeField] private string videoLink_ru;
            [SerializeField] private string videoLink_uzb;

            public string VideoLink {
                get
                {
                    switch (LocaleProvider.SelectedLanguage)
                    {
                        case Language.En:
                            return videoLink_en;
                        case Language.Ru:
                            return videoLink_ru;
                        case Language.Uzb:
                            return videoLink_uzb;
                        default:
                            return videoLink_en;
                    }
                }
            }
            
            public Lesson[] lessons;
            
            [JsonObject(MemberSerialization.Fields)]
            [Serializable]
            public class Lesson
            {
                public int version;
                public bool enabled = true;
                public bool paid;

                [SerializeField] private string name_en;
                [SerializeField] private string name_ru;
                [SerializeField] private string name_uzb;

                public string Name {
                    get
                    {
                        switch (LocaleProvider.SelectedLanguage)
                        {
                            case Language.En:
                                return name_en;
                            case Language.Ru:
                                return name_ru;
                            case Language.Uzb:
                                return name_uzb;
                            default:
                                return name_en;
                        }
                    }
                }
                public bool singleAudioClip = true;

                [SerializeField, ShowIf("singleAudioClip")] private string audioKey_en;
                [SerializeField, ShowIf("singleAudioClip")] private string audioKey_ru;
                [SerializeField, ShowIf("singleAudioClip")] private string audioKey_uzb;

                public string AudioKey {
                    get
                    {
                        switch (LocaleProvider.SelectedLanguage)
                        {
                            case Language.En:
                                return audioKey_en;
                            case Language.Ru:
                                return audioKey_ru;
                            case Language.Uzb:
                                return audioKey_uzb;
                            default:
                                return audioKey_en;
                        }
                    }
                }
                [Space]
                public string previewImageKey;

                public string prefabKey;
                public string preloadLabel;
            }
        }
    }
}
