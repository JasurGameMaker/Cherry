using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneNavigation
{
    [InitializeOnLoad]
    public class SceneNavigationGUI
    {
        static SceneNavigationGUI()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        }

        private static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            HandleNavigation();
            HandleRedirect();
        }

        private static void HandleNavigation()
        {
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                string path = scene.path;
                string name = Path.GetFileNameWithoutExtension(path);

                if (GUILayout.Button(new GUIContent(name, $"Open {name} Scene")))
                {
                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
                    EditorSceneManager.OpenScene(path);
                }
            }
        }

        private static void HandleRedirect()
        {
            SceneRedirect.Redirect = GUILayout.Toggle(SceneRedirect.Redirect, new GUIContent("Redirect", "Redirect to first scene on enter play mode"));

            if (SceneRedirect.Redirect)
                SceneRedirect.SetStartSceneToFirstInBuild();
            else
                EditorSceneManager.playModeStartScene = null;
        }
    }
}