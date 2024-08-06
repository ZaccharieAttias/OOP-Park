using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class SceneManagement
{
    public static List<string> ActivePopups = new List<string>();
    public static void SceneResume(string popupName)
    {
        ActivePopups.Remove(popupName);

        if (ActivePopups.Count == 0) Time.timeScale = 1f;
    }
    public static void ScenePause(string popupName)
    {
        ActivePopups.Add(popupName);

        Time.timeScale = 0f;
    }

    public static void Hierarchy() { SceneManager.LoadScene("Hierarchy"); }
    public static void Polymorphism() { SceneManager.LoadScene("Polymorphism"); }
    public static void RestrictionMenu() { SceneManager.LoadScene("RestrictionMenu"); }

    public static void Quit() { Application.Quit(); }

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }




    public static List<GameplayInfo> GameplayInfo = GameplayData.LoadForMenu();
}


public class GameplayInfo
{
    public List<ChapterInfo> ChapterInfos;
}
public class ChapterInfo
{
    public int ChapterNumber;
    public string Name;
    public List<LevelInfo> LevelsInfo;
}


public class LevelInfo
{
    public int LevelNumber;
    public int Score;
    public int Status;
}

