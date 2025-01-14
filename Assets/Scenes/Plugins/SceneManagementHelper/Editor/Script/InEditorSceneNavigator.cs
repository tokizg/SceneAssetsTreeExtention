using UnityEditor.SceneManagement;
using UnityEngine;

public static class InEditorSceneNavigator
{
    public static void RequestOpenScene(string scenePath)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            try
            {
                EditorSceneManager.OpenScene(scenePath);
            }
            catch (System.IO.FileNotFoundException)
            {
                Debug.LogError($"Scene at path {scenePath} does not exist.");
            }
        }
    }
}
