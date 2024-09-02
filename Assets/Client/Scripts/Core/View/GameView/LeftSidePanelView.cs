using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class LeftSidePanelView : MonoBehaviour
    {
        [Header("ColourPicker")]
        [SerializeField] private Button colourPickerButton;
        [SerializeField] private ColourPicker colourPicker;

        private bool _isColourPickerActive;

        private void OnEnable()
        {
            colourPickerButton.onClick.AddListener(OpenColorPicker);
        }

        private void OnDisable()
        {
            colourPickerButton.onClick.RemoveListener(OpenColorPicker);
        }

        private void OpenColorPicker()
        {
            _isColourPickerActive = !_isColourPickerActive;
            colourPicker.gameObject.SetActive(_isColourPickerActive);
        }
    }
}
