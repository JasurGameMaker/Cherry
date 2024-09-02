using System;
using __Scripts.Project.Data;
using __Scripts.Project.Menu.UI.Subjects;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace __Scripts.Project.Menu.UI.New
{
    public class LessonDownloadSize : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private LessonModel model;

        private void OnEnable() =>
            model.Lesson.ValueChanged += OnLessonChanged;

        private void OnDisable() =>
            model.Lesson.ValueChanged -= OnLessonChanged;

        private async void OnLessonChanged(Catalog.Subject.Lesson lesson)
        {
            long size = await Addressables.GetDownloadSizeAsync(lesson.prefabKey);
            text.SetText(size/(1024 * 1024) + " mb");
        }
    }
}
