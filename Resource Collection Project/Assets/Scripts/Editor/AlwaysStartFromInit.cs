using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class AlwaysStartFromInit
{
    private const string InitScenePath = "Assets/Scenes/Init.unity"; // Укажите путь к вашей Init сцене.

    static AlwaysStartFromInit()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode) // Перед запуском игры.
        {
            if (EditorSceneManager.GetActiveScene().path != InitScenePath)
            {
                bool confirm = EditorUtility.DisplayDialog(
                    "Смена сцены",
                    "Игра всегда должна запускаться с Init сцены. Переключиться на Init сцену?",
                    "Да", "Отмена");

                if (confirm)
                {
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                    EditorSceneManager.OpenScene(InitScenePath);
                }
                else
                {
                    EditorApplication.isPlaying = false;
                }
            }
        }
    }
}