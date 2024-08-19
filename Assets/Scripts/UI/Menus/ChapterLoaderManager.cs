using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class ChapterLoaderManager : MonoBehaviour
{
    [Header("UI Elements")]
    public List<Button> LevelButtons;

    [Header("Chapter Information")]
    public int ChapterIndex;
    public bool IsChapter;


    public void Start()
    {
        InitializeGameplayData();

        LoadChapterInfo();
    }
    public void InitializeGameplayData()
    {
        GameplayData.Initialize();
        GameplayData.Load();
    }

    public void LoadChapterInfo()
    {
        var gameplayInfo = SceneManagement.GameplayInfo;
        var chapterInfos = gameplayInfo[0].ChapterInfos;
        var levelsInfo = ChapterIndex == -1 ? chapterInfos.Select(ci => ci.LevelsInfo[0]).ToList() : chapterInfos[ChapterIndex].LevelsInfo;

        if (IsChapter) UpdateChapterButtons(chapterInfos);
        else UpdateLevelButtons(levelsInfo);
    }
    public void UpdateLevelButtons(List<LevelInfo> levelsInfo)
    {
        for (int i = 0; i < LevelButtons.Count; i++)
        {
            var levelStatus = levelsInfo[i].Status;
            LevelButtons[i].interactable = levelStatus != -1;

            if (levelStatus == 1) LevelButtons[i].GetComponent<Image>().color = Color.green;
        }
    }

    public void UpdateChapterButtons(List<ChapterInfo> chapterInfo)
    {
        for (int i = 0; i < LevelButtons.Count; i++)
        {
            var lastLevelStatus = chapterInfo[i].LevelsInfo.Last().Status;
            LevelButtons[i].interactable = chapterInfo[i].LevelsInfo.First().Status >= 0;

            if (lastLevelStatus == 1) LevelButtons[i].GetComponent<Image>().color = Color.green;
        }
    }
}