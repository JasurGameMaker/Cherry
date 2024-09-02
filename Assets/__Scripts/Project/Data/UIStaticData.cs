using UnityEngine;

namespace __Scripts.Project.Data
{
    [CreateAssetMenu(fileName = "UIStaticData", menuName = "Data/UIStaticData")]
    public class UIStaticData : ScriptableObject
    {
        public float transitionCircleAngleSec;
        public float transitionDurationSec;
    }
}
