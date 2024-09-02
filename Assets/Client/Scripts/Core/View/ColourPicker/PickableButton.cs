using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class PickableButton : MonoBehaviour
    {
        [field: SerializeField] public Button pickableButton;
        [field: SerializeField] public Sprite setImage;
    }
}