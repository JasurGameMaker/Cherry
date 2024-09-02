using System;
using __Scripts.Project.Menu.UI.Utils;
using Doozy.Runtime.UIManager.Components;
using Zenject;

namespace __Scripts.Project.Core.Audio.UI
{
    public class AudioToggle : DoozyToggleListener
    {
        private IAudioPlayer _audioPlayer;

        [Inject]
        private void Construct(IAudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
        }

        protected override void OnAfterEnable()
        {
            _audioPlayer.Stopped += OnStop;
            _audioPlayer.Ended += OnStop;
        }

        protected override void OnAfterDisable()
        {
            _audioPlayer.Stopped -= OnStop;
            _audioPlayer.Ended -= OnStop;
        }

        private void OnStop() =>
            Toggle.SetIsOn(false, true, true);

        protected override void OnToggle(bool state)
        {
            if (state)
                _audioPlayer.Play();
            else
                _audioPlayer.Pause();
        }
    }
}
