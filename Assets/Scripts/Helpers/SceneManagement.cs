using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public static class SceneManagement
{
    public static void SceneResume() { Time.timeScale = 1f; }
    public static void ScenePause() { Time.timeScale = 0f; }

    /* Implement Scenes & Levels
    public void Presentation() { SceneManager.LoadScene("Presentation"); }
    public void Playground() { SceneManager.LoadScene("Playground"); }
    public void LocalPark() { SceneManager.LoadScene("LocalPark"); }
    public void OnlinePark() { SceneManager.LoadScene("OnlinePark"); }
    public void Progression() { SceneManager.LoadScene("Progression"); }
    public void Credits() { SceneManager.LoadScene("Credits"); }
    public void Tutorial() { SceneManager.LoadScene("Tutorial"); }
    public void Hierarchy() { SceneManager.LoadScene("Hierarchy"); }
    public void Polymorphism() { SceneManager.LoadScene("Polymorphism"); }
    */
    public static void RestrictionMenu() { UnityEngine.SceneManagement.SceneManager.LoadScene("RestrictionMenu"); }

    public static void Quit() { Application.Quit(); }

    public static List<ChapterInfo> ChapterInfos;


    public static void UnlockNextLevel()
    {
        var currentSceneName = SceneManager.GetActiveScene().name;
        int chapterNumber = currentSceneName[1];
        int levelNumber = int.Parse(currentSceneName.Substring(3));

        if (levelNumber + 1 <= ChapterInfos[chapterNumber - 1].LevelsInfo.Count)
        {
            var currentLevel = ChapterInfos[chapterNumber - 1].LevelsInfo[levelNumber - 1];
            var nextLevel = ChapterInfos[chapterNumber - 1].LevelsInfo[levelNumber];

            currentLevel.Status = 1;
            nextLevel.Status = 0;
        }

        else if (chapterNumber + 1 <= ChapterInfos.Count)
        {
            var currentLevel = ChapterInfos[chapterNumber - 1].LevelsInfo[levelNumber - 1];
            var nextLevel = ChapterInfos[chapterNumber].LevelsInfo[0];

            currentLevel.Status = 1;
            nextLevel.Status = 0;
        }

        else
        {
            var currentLevel = ChapterInfos[chapterNumber - 1].LevelsInfo[levelNumber - 1];
            currentLevel.Status = 1;

            Debug.Log("Congrats! You have completed the game!");
        }

        GameplayData.Save();
    }
}



public class ChapterInfo
{
    public int ChapterNumber;
    public List<LevelInfo> LevelsInfo;
}


public class LevelInfo
{
    public int LevelNumber;
    public int Status; // -1 locked, 0 unlocked, 1 completed
}

