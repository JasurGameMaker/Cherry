using System;
using System.Collections;
using __Scripts.Project.Core.Audio.UI;
using __Scripts.Project.Core.Model;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;
using Zenject;

namespace __Scripts.Project.Core.Audio
{
    public class AudioPlayer : MonoBehaviour, IAudioPlayer
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioToggle audioToggle;
        
        public bool IsPlaying => audioSource.isPlaying;
        public bool IsReady => audioSource.clip;

        public event Action Stopped;
        public event Action Ended;

        private IEnumerator _endAwaiter;
        
        private CoreState _coreState;

        [Inject]
        private void Construct(CoreState coreState)
        {
            _coreState = coreState;
        }

        private void OnEnable() =>
            _coreState.SelectedMesh.ValueChanged += OnSelectedMeshChanged;

        private void OnDisable() =>
            _coreState.SelectedMesh.ValueChanged -= OnSelectedMeshChanged;

        private void OnSelectedMeshChanged(CourseMesh mesh)
        {
            if (!mesh.AudioClip)
                return;
            
            SetClip(mesh.AudioClip);
            SetProgressNormalized(0);
            TriggerToggle();
        }

        public void SetClip(AudioClip audioClip) =>
            audioSource.clip = audioClip;

        public void Play()
        {
            if (audioSource.isPlaying)
                StopCoroutine(_endAwaiter);
            
            audioSource.Play();
            
            _endAwaiter = WaitForEndCoroutine();
            StartCoroutine(_endAwaiter);
        }

        public void TriggerToggle() =>
            audioToggle.Toggle.SetIsOn(true);

        private IEnumerator WaitForEndCoroutine()
        {
            yield return new WaitWhile(() => audioSource.isPlaying);
            audioSource.time = 0;
            Ended?.Invoke();
        }
        
        public void Pause()
        {
            StopCoroutine(_endAwaiter);
            
            audioSource.Pause();
        }

        public void Stop()
        {
            if (_endAwaiter != null)
                StopCoroutine(_endAwaiter);
            
            audioSource.Stop();
            Stopped?.Invoke();
        }

        public void SetProgressNormalized(float value)
        {
            float newValue = value * audioSource.clip.length;
            
            if (newValue < audioSource.clip.length)
                audioSource.time = value * audioSource.clip.length;
            
        }

        public float GetProgressNormalized() =>
            audioSource.time / audioSource.clip.length;

        public float GetTime() =>
            audioSource.time;
    }
}
