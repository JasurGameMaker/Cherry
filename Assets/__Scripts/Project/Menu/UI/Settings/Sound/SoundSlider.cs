using __Scripts.Project.Data;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Zenject;

namespace __Scripts.Project.Menu.UI.Settings.Sound
{
    public class SoundSlider : MonoBehaviour
    {
        [SerializeField] private string exposedName;
        [SerializeField] private Slider slider;
        [SerializeField] private AudioMixer mixer;
        
        private MenuState _menuState;

        [Inject]
        private void Construct(MenuState menuState)
        {
            _menuState = menuState;
        }
        
        private void Start() =>
            slider.value = _menuState.VolumeNorm;

        private void OnEnable() =>
            slider.onValueChanged.AddListener(OnValueChanged);

        private void OnDisable() =>
            slider.onValueChanged.RemoveListener(OnValueChanged);

        private void OnValueChanged(float value)
        {
            _menuState.VolumeNorm = value;
            mixer.SetFloat(exposedName, Mathf.Lerp(-80, 20, value));
        }
    }
}
