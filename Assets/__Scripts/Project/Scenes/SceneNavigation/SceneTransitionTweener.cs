using __Scripts.Project.Data;
using DG.Tweening;
using UnityEngine;

namespace __Scripts.Project.Scenes.SceneNavigation
{
    public class SceneTransitionTweener
    {
        private readonly SceneTransitionView _sceneTransitionView;
        private readonly UIStaticData _uiStaticData;

        private Tween _fadeOutTween;
        private Tween _fadeInTween;
        private Tween _circleTween;

        public SceneTransitionTweener(SceneTransitionView sceneTransitionView, UIStaticData uiStaticData)
        {
            _uiStaticData = uiStaticData;
            _sceneTransitionView = sceneTransitionView;
        }

        public Tween FadeIn(bool tweenFadeIn)
        {
            _sceneTransitionView.gameObject.SetActive(true);
            
            LoadingLoop();

            _fadeOutTween?.Kill();

            float fadeInSec = tweenFadeIn ? _uiStaticData.transitionDurationSec : 0;
            
            return _fadeInTween = _sceneTransitionView.fill
                .DOFade(1, fadeInSec)
                .From(0);
        }

        public Tween FadeOut()
        {
            _fadeInTween?.Kill();
            
            return _fadeOutTween = _sceneTransitionView.fill
                .DOFade(0, _uiStaticData.transitionDurationSec)
                .From(1)
                .OnComplete(() => _sceneTransitionView.gameObject.SetActive(false));
        }

        private void LoadingLoop()
        {
            if (!_sceneTransitionView.circle)
                return;
            
            if (_circleTween.IsActive())
                return;

            _circleTween = _sceneTransitionView.circle.transform
                .DORotate(new Vector3(0, 0, -360), _uiStaticData.transitionCircleAngleSec, RotateMode.FastBeyond360)
                .SetRelative()
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental)
                .SetLink(_sceneTransitionView.gameObject, LinkBehaviour.KillOnDisable);
        }
    }
}
