using System;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;
using Zenject;

namespace __Scripts.Project.Core.Audio.UI
{
    public class AudioPlayerSlider : MonoBehaviour
    {
        [SerializeField] private UISlider uiSlider;
        
        private IAudioPlayer _audioPlayer;
        private bool _wasPlaying;

        [Inject]
        private void Construct(IAudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
        }

        private void Update()
        {
            if (!_audioPlayer.IsReady)
                return;
            
            if (uiSlider.isSelected)
            {
                if (_audioPlayer.GetTime() == 0)
                {
                    _audioPlayer.SetProgressNormalized(uiSlider.value);
                    _audioPlayer.TriggerToggle();
                    return;
                }
                
                if (_audioPlayer.IsPlaying)
                {
                    _wasPlaying = true;
                    _audioPlayer.Pause();
                }
                
                _audioPlayer.SetProgressNormalized(uiSlider.value);
            }
            else
            {
                if (_wasPlaying)
                {
                    _audioPlayer.TriggerToggle();
                    _wasPlaying = false;
                }
                uiSlider.value = _audioPlayer.GetProgressNormalized();
            }
        }
    }
}
