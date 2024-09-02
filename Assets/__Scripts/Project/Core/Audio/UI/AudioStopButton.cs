using __Scripts.Project.Menu.UI.Utils;
using Zenject;

namespace __Scripts.Project.Core.Audio.UI
{
    public class AudioStopButton : DoozyButtonListener
    {
        private IAudioPlayer _audioPlayer;

        [Inject]
        private void Construct(IAudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
        }
        
        protected override void OnClick() =>
            _audioPlayer.Stop();
    }
}
