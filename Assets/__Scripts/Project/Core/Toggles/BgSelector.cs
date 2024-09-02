using __Scripts.Project.Menu.UI.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace __Scripts.Project.Core.Toggles
{
    public class BgSelector : DoozyToggleListener
    {
        [SerializeField] private Image bgImage;
        [SerializeField] private Sprite bgSprite;
        
        protected override void OnToggle(bool state)
        {
            if(state)
                bgImage.sprite = bgSprite;
        }
    }
}
