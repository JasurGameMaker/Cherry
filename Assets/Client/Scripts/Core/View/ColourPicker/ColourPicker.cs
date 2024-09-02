using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class ColourPicker : MonoBehaviour
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private List<PickableButton> pickableButtons;

        private void OnEnable()
        {
            foreach (var pickableButton in pickableButtons)
            {
                pickableButton.pickableButton.onClick.AddListener(() => OnPickerButtonClicked(pickableButton));
            }
        }

        private void OnDisable()
        {
            foreach (var pickableButton in pickableButtons)
            {
                pickableButton.pickableButton.onClick.RemoveListener(() => OnPickerButtonClicked(pickableButton));
            }
        }

        private void OnPickerButtonClicked(PickableButton pickableButton)
        {
            backgroundImage.sprite = pickableButton.setImage;
        }
    }
}
