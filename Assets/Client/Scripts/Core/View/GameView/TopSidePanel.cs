using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Client
{
    public class TopSidePanel : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        
        private SceneLoader _sceneLoader;
        
        [Inject]
        public void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }
        
        private void OnEnable()
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void OnDisable()
        {
            backButton.onClick.RemoveListener(OnBackButtonClicked);
        }
        
        private void OnBackButtonClicked()
        {
            _sceneLoader.LoadPreviousScene();
        }
    }
}