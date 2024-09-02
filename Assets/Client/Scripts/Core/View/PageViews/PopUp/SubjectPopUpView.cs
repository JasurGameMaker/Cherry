using __Scripts.Project.Data;
using __Scripts.Project.Scenes.SceneNavigation.Enums;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace Client
{
    public class SubjectPopUpView : MonoBehaviour, IPopUp
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button lessonsButton;
        [SerializeField] private Button presentationsButton;

        [SerializeField] private Button startLesson;
        [SerializeField] private Button createPresentation;
        
        [SerializeField] private GameObject lessonView;
        [SerializeField] private GameObject presentationView;
        
        [SerializeField] private Image lessonsButtonUnderline;
        [SerializeField] private Image presentationButtonUnderline;
        
        [SerializeField] private ScrollRect scrollView;
        
        public PresentationItem presentationItem;
        
        private __Scripts.Project.Scenes.SceneNavigation.SceneLoader _sceneLoader;
        
        public Catalog.Subject.Lesson Lesson { get; set; }
        
        [Inject]
        public void Construct(__Scripts.Project.Scenes.SceneNavigation.SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void OnEnable()
        {
            OnLessonButtonClicked();
            
            closeButton.onClick.AddListener(OnCloseButtonClicked);
            lessonsButton.onClick.AddListener(OnLessonButtonClicked);
            presentationsButton.onClick.AddListener(OnPresentationButtonClicked);
            startLesson.onClick.AddListener(OnStartLessonButtonClicked);
            createPresentation.onClick.AddListener(OnCreatePresentationButtonClicked);
        }

        private void OnDisable()
        {
            closeButton.onClick.RemoveListener(OnCloseButtonClicked);
            lessonsButton.onClick.RemoveListener(OnLessonButtonClicked);
            presentationsButton.onClick.RemoveListener(OnPresentationButtonClicked);
            startLesson.onClick.RemoveListener(OnStartLessonButtonClicked);
            createPresentation.onClick.RemoveListener(OnCreatePresentationButtonClicked);
        }
        
        public void Open()
        {
            gameObject.SetActive(true);
        }
        
        private void OnCloseButtonClicked()
        {
            gameObject.SetActive(false);
        }

        private void OnLessonButtonClicked()
        {
            lessonsButtonUnderline.enabled = true;
            presentationButtonUnderline.enabled = false;
            lessonView.SetActive(true);
            presentationView.SetActive(false);
        }
        
        private void OnPresentationButtonClicked()
        {
            lessonsButtonUnderline.enabled = false;
            presentationButtonUnderline.enabled = true;
            
            lessonView.SetActive(false);
            presentationView.SetActive(true);

            //LoadPresentations();
        }

        private void LoadPresentations()
        {
            for (int i = 0; i < 3; i++)
            {
                var presentation = Instantiate(presentationItem, scrollView.content);
                presentation.name = $"Presentation_{i}";
            }
        }

        private void OnStartLessonButtonClicked()
        {
            _sceneLoader.Load(Scenes.Game,
                fadeOutOnDemand: true,
                bindings: Lesson);
        }
        
        private void OnCreatePresentationButtonClicked()
        {
            //_sceneLoader.LoadSceneWithBindings("Presentation");
        }
    }
}