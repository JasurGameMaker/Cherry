using __Scripts.Project.Data;
using __Scripts.Project.Services;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Lean.Pool;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization.Components;
using UnityEngine.Video;
using Zenject;

namespace __Scripts.Project.Menu.UI.Subjects
{
    public class SubjectViewBuilder : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private RectTransform lessonsContainer;
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private UIView uiView;
        
        [SerializeField] private LessonView lessonViewPrefab;
        
        private MenuState _menuState;
        private IRemoteHelper _remoteHelper;
        private LocalizeStringEvent _labelLocalization;
        private CanvasGroup _videoCanvasGroup;

        [Inject]
        private void Construct(MenuState menuState, IRemoteHelper remoteHelper)
        {
            _remoteHelper = remoteHelper;
            _menuState = menuState;
        }

        private void Awake()
        {
            _labelLocalization = labelText.GetComponent<LocalizeStringEvent>();
            _videoCanvasGroup = videoPlayer.GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            _menuState.SelectedSubject.ValueChanged += OnNewSubject;
            uiView.OnVisibleCallback.Event.AddListener(OnVisible);
            uiView.OnHideCallback.Event.AddListener(OnHide);
        }

        private void OnDisable()
        {
            _menuState.SelectedSubject.ValueChanged -= OnNewSubject;
            uiView.OnVisibleCallback.Event.RemoveListener(OnVisible);
            uiView.OnHideCallback.Event.RemoveListener(OnHide);
        }

        private async void OnNewSubject(Catalog.Subject subject)
        {
            Clear();
            
            if (subject == null)
            {
                _labelLocalization.RefreshString();
                return;
            }
            
            labelText.SetText(subject.Name);

            if (!string.IsNullOrEmpty(subject.VideoLink))
            {
                _remoteHelper.LoadVideo(videoPlayer, subject);
                videoPlayer.Prepare();
                videoPlayer.prepareCompleted += OnPrepared;
            }
            else
                HideVideo();

            if (subject.lessons == null)
                return;
            
            foreach (Catalog.Subject.Lesson lesson in subject.lessons)
            {
                if (!lesson.enabled)
                    continue;
                
                LessonView view = LeanPool.Spawn(lessonViewPrefab, lessonsContainer);
                Texture2D texture = await Addressables.LoadAssetAsync<Texture2D>(lesson.previewImageKey);
                view
                    .SetSprite(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f))
                    .SetName(lesson.Name)
                    .SetLesson(lesson);
            }
        }

        private async void OnPrepared(VideoPlayer source)
        {
            source.prepareCompleted -= OnPrepared;
            
            source.Play();
            await UniTask.Yield();
            _videoCanvasGroup
                .DOFade(1, 0.25f)
                .SetLink(gameObject);
        }

        public void Clear()
        {
            int count = lessonsContainer.childCount;
            for (int i = count - 1; i >= 0; i--)
                LeanPool.Despawn(lessonsContainer.GetChild(i));
        }

        private void OnVisible()
        {
            //videoPlayer.Play();
        }

        private void OnHide()
        {
            HideVideo();

            _labelLocalization.RefreshString();
            _menuState.SelectedSubject.Value = null;
        }

        private void HideVideo()
        {
            videoPlayer.Stop();
            _videoCanvasGroup
                .DOFade(0, 0.25f)
                .SetLink(gameObject);
        }
    }
}
