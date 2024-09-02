using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Client
{
    public class PresentationItem : MonoBehaviour
    {
        [SerializeField] private Button startPresentation;
        [SerializeField] private Button deletePresentation;

        /*private SceneLoader _sceneLoader;
        
        [Inject]
        public void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }*/
        
        private void OnEnable()
        {
            startPresentation.onClick.AddListener(OnCreatePresentationButtonClicked);
            deletePresentation.onClick.AddListener(DeleteItem);
        }

        private void OnDisable()
        {
            startPresentation.onClick.RemoveListener(OnCreatePresentationButtonClicked);
            deletePresentation.onClick.RemoveListener(DeleteItem);
        }
        
        private void OnCreatePresentationButtonClicked()
        {
            //_sceneLoader.LoadSceneWithBindings("Presentation");
        }

        private void DeleteItem()
        {
            Destroy(gameObject);
        }
    }
}
