using System.IO;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace __Scripts.Project.Data
{
    [CreateAssetMenu(fileName = "CatalogCreator", menuName = "Data/CatalogCreator")]
    public class CatalogCreator : ScriptableObject
    {
        [SerializeField] private Catalog catalog;
        
#if UNITY_EDITOR
        [Button]
        public void Save()
        {
            string path = Application.dataPath + "/Data" + "/catalog.json";

            Debug.Log("Saved to: " + path);

            File.WriteAllText(path, JsonConvert.SerializeObject(catalog));
            AssetDatabase.Refresh();
        }
#endif
    }
}
