using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public abstract class BaseLessonPageView : MonoBehaviour
    {
        [SerializeField] private WindowsManager _windowsManager;

        [SerializeField] private Button _moreButton;
        [SerializeField] private TMP_InputField _searchInputField;
        [SerializeField] private TMP_Text _titleLabel;
        [SerializeField] private Transform _container;
        
        private bool _isMoreButtonClick;
    }
}