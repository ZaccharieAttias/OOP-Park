using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using LLlibs.ZeroDepJson;
using LootLocker.Extension.DataTypes;


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
    public static void RestrictionMenu() { UnityEngine.SceneManagement.SceneManager.LoadScene("RestrictionMenu"); }

    public static void Quit() { Application.Quit(); }

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }




    public static List<GameplayInfo> GameplayInfo = GameplayData.LoadForMenu();

    public static void UnlockNextLevel()
    {

        var ChapterInfos = GameplayInfo[0].ChapterInfos;
        var currentSceneName = SceneManager.GetActiveScene().name;

        int chapterNumber = int.Parse(currentSceneName[1].ToString());
        int levelNumber = int.Parse(currentSceneName[3].ToString());

        if (levelNumber + 1 <= ChapterInfos[chapterNumber].LevelsInfo.Count)
        {
            var currentLevel = ChapterInfos[chapterNumber].LevelsInfo[levelNumber - 1];
            var nextLevel = ChapterInfos[chapterNumber].LevelsInfo[levelNumber];

            currentLevel.Status = 1;
            nextLevel.Status = 0;
        }

        else if (chapterNumber + 1 < ChapterInfos.Count)
        {
            var currentLevel = ChapterInfos[chapterNumber].LevelsInfo[levelNumber - 1];
            var nextLevel = ChapterInfos[chapterNumber + 1].LevelsInfo[0];

            currentLevel.Status = 1;
            nextLevel.Status = 0;
        }

        else
        {
            var currentLevel = ChapterInfos[chapterNumber].LevelsInfo[levelNumber - 1];
            currentLevel.Status = 1;
        }

        GameplayData.Save();
    }

    public static void CompleteOnline()
    {
        string LevelName = GameObject.Find("LevelManager").GetComponent<LevelDownload>().LevelName1;
        var ChapterInfos = GameplayInfo[1].ChapterInfos;
        var currentChapter = ChapterInfos.Find(chapter => chapter.Name == LevelName);
        if (currentChapter != null)
            currentChapter.LevelsInfo[0].Status = 1;
        else 
            {
                ChapterInfos.Add(new ChapterInfo
                {
                    ChapterNumber = ChapterInfos.Count,
                    Name = LevelName,
                    LevelsInfo = new List<LevelInfo> { new LevelInfo { LevelNumber = 1, Status = 1 } }
                });
            }
        
        GameplayData.Save();
    }
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
    public int Status; // -1 locked, 0 unlocked, 1 completed
}

