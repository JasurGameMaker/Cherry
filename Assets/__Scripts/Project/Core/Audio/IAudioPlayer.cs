using System;
using UnityEngine;

namespace __Scripts.Project.Core.Audio
{
    public interface IAudioPlayer
    {
        public event Action Stopped;
        public event Action Ended;
        public bool IsPlaying { get; }
        public bool IsReady { get; }
        public void SetClip(AudioClip audioClip);
        public void Play();
        public void TriggerToggle();
        public void Pause();
        public void Stop();
        public void SetProgressNormalized(float value);
        public float GetProgressNormalized();
        public float GetTime();
    }
}
