using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SceneNavigation
{
    public static class SceneRedirect
    {
        public static bool Redirect {
            get => EditorPrefs.GetBool(nameof(Redirect), true);
            set => EditorPrefs.SetBool(nameof(Redirect), value);
        }

        public static void SetStartSceneToFirstInBuild()
        {
            string path = EditorBuildSettings.scenes[0].path;
            SceneAsset startScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);

            if (startScene != null)
                EditorSceneManager.playModeStartScene = startScene;
            else
                Debug.LogWarning("Could not find Scene " + path);
        }
    }
}
