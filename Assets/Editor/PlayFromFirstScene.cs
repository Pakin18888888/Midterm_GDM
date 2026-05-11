using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class PlayFromFirstScene
{
    static PlayFromFirstScene()
    {
        EditorApplication.playModeStateChanged += LoadStartScene;
    }

    static void LoadStartScene(PlayModeStateChange state)
    {
        if (state ==
            PlayModeStateChange.ExitingEditMode)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

            EditorSceneManager.OpenScene(
                "Assets/Scenes/BootScene.unity"
            );
        }
    }
}