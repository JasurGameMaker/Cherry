using UnityEngine;

namespace Client
{
    public abstract class ToggleTheme : MonoBehaviour
    {
        [field : SerializeField] public ToggleSwitch toggleSwitch;

        public abstract void SetTheme(bool isDark);
    }
}