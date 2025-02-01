#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityToolbarExtender;

[InitializeOnLoad]
public class PlayFromSplash : MonoBehaviour
{
    static PlayFromSplash()
    {
        ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
    }

    static void OnToolbarGUI()
    {
        GUILayout.FlexibleSpace();
        
        EditorGUIUtility.SetIconSize(new Vector2(17, 17));
        if (GUILayout.Button(new GUIContent(EditorGUIUtility.IconContent("PlayButton").image, "Play From Splash"),
                EditorStyles.toolbarButton))
        {
            PlayFromSplashScreen();
        }
        
        if (GUILayout.Button(new GUIContent("Open StockMarket Scene"),
                EditorStyles.toolbarButton))
        {
            OpenStockMarketScene();
        }
    }

    static void PlayFromSplashScreen()
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
            return;
        }

        // Debug: Check if we have any scenes in build settings
        if (EditorBuildSettings.scenes.Length == 0)
        {
            Debug.LogError("No scenes in build settings!");
            return;
        }

        // Debug: Log the path we're trying to load
        Debug.Log($"Attempting to load scene at path: {EditorBuildSettings.scenes[0].path}");
        
        // Debug: Check if the scene is enabled in build settings
        if (!EditorBuildSettings.scenes[0].enabled)
        {
            Debug.LogError("First scene in build settings is disabled!");
            return;
        }

        // Save the current scene if it has unsaved changes
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            // Store the current scene path
            string currentScene = EditorSceneManager.GetActiveScene().path;
            
            try
            {
                // Load the first scene in the build settings (splash screen)
                EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path);
                
                // Start playing
                EditorApplication.isPlaying = true;

                // Register a callback to return to the previous scene when stopping play mode
                EditorApplication.playModeStateChanged += (PlayModeStateChange state) =>
                {
                    if (state == PlayModeStateChange.EnteredEditMode)
                    {
                        EditorSceneManager.OpenScene(currentScene);
                        //EditorApplication.playModeStateChanged -= EditorApplication.playModeStateChanged;
                    }
                };
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load splash screen: {e.Message}");
            }
        }
    }

    static void OpenStockMarketScene()
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
            return;
        }

        // Debug: Check if we have any scenes in build settings
        if (EditorBuildSettings.scenes.Length == 0)
        {
            Debug.LogError("No scenes in build settings!");
            return;
        }
        
        // Save the current scene if it has unsaved changes
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            // Store the current scene path
            string currentScene = EditorSceneManager.GetActiveScene().path;
            
            try
            {
                // Load the first scene in the build settings (splash screen)
                EditorSceneManager.OpenScene(EditorBuildSettings.scenes[1].path);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load splash screen: {e.Message}");
            }
        }
    }
}
#endif
