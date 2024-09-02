using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Zenject;

namespace Client
{
    public class SceneLoader
    {
        private readonly DiContainer _container;
        private readonly Stack<string> _sceneHistory = new Stack<string>();

        [Inject]
        public SceneLoader(DiContainer container)
        {
            _container = container;
        }

        public void LoadScene(string sceneName)
        {
            _sceneHistory.Push(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        public void LoadSceneAdditive(string sceneName)
        {
            _sceneHistory.Push(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        public void LoadSceneWithBindings(string sceneName)
        {
            _sceneHistory.Push(SceneManager.GetActiveScene().name);
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        public void LoadPreviousScene()
        {
            if (_sceneHistory.Count <= 0) return;
            string previousScene = _sceneHistory.Pop();
            SceneManager.LoadScene(previousScene, LoadSceneMode.Single);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}