using UnityEngine;
using UnityEngine.Localization.Components;

namespace __Scripts.Project.Core.UI
{
    public class DescriptionView : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent description;
        [SerializeField] private LocalizeStringEvent title;
        
        public void SetDescription(string tableReference, string descrKey, string titleKey)
        {
            description.SetTable(tableReference);
            description.SetEntry(descrKey);
            title.SetTable(tableReference);
            title.SetEntry(titleKey);
        }
    }
}
