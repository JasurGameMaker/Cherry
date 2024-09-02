using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace __Scripts.Project.Scenes.SceneNavigation
{
    public class SceneLoader
    {
        private readonly ZenjectSceneLoader _zenSceneLoader;
        private readonly SceneTransitionTweener _sceneTransitionTweener;

        private SceneLoader(ZenjectSceneLoader zenSceneLoader, SceneTransitionTweener sceneTransitionTweener)
        {
            _zenSceneLoader = zenSceneLoader;
            _sceneTransitionTweener = sceneTransitionTweener;
        }

        public void Load(Enums.Scenes scene, bool tweenFadeIn = true, bool fadeOutOnDemand = false, params object[] bindings)
        {
            DOTween.Validate();

            DOTween.Sequence()
                .Append(_sceneTransitionTweener.FadeIn(tweenFadeIn))
                .AppendCallback(AfterFade);

            void AfterFade()
            {
                var loadingOperation = _zenSceneLoader.LoadSceneAsync((int)scene, LoadSceneMode.Single, container =>
                {
                    foreach (object binding in bindings)
                        container.Bind(binding.GetType())
                            .FromInstance(binding)
                            .AsSingle()
                            .NonLazy();
                });
                
                if (!fadeOutOnDemand)
                    loadingOperation.completed += _ => _sceneTransitionTweener.FadeOut();
            }
        }

        public void FadeOut() =>
            _sceneTransitionTweener.FadeOut();
    }
}
