using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelLoader
{
    public static string TargetScene { get; private set; } = "MainMenu";

    public static void LoadSceneWithBar(string sceneName)
    {
        Time.timeScale = 1f;
        TargetScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    public static void LoadSceneByName(string sceneName)
    {
        LoadSceneWithBar(sceneName);
    }

    public static void LoadMainMenu()
    {
        LoadSceneWithBar("MainMenu");
    }

    public static void LoadCredits()
    {
        LoadSceneWithBar("Credits");
    }

    public static void RestartCurrentLevel()
    {
        LoadSceneWithBar(SceneManager.GetActiveScene().name);
    }

    public static void LoadNextLevel()
    {
        Time.timeScale = 1f;
        string current = SceneManager.GetActiveScene().name;

        if (current == "Level01") { SaveData.UnlockLevel(2); LoadSceneWithBar("Level02"); }
        else if (current == "Level02") { SaveData.UnlockLevel(3); LoadSceneWithBar("Level03"); }
        else if (current == "Level03") { SaveData.UnlockLevel(4); LoadSceneWithBar("Level04"); }
        else if (current == "Level04") { SaveData.UnlockLevel(5); LoadSceneWithBar("Level05"); }
        else if (current == "Level05") { LoadSceneWithBar("Credits"); }
        else if (current == "Credits") { LoadSceneWithBar("MainMenu"); } // Wrap after Credits
        else { LoadSceneWithBar("MainMenu"); }
    }
}