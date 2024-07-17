using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;


public class ChapterLoaderManager : MonoBehaviour
{
    [Header("UI Elements")]
    public List<Button> LevelButtons;
    public int ChapterIndex;

    public void Start()
    {
        var gameplayInfo = SceneManagement.GameplayInfo;
        var chapterInfos = gameplayInfo[0].ChapterInfos;
        var levelsInfo = ChapterIndex == -1 ? chapterInfos.Select(ci => ci.LevelsInfo[0]).ToList() : chapterInfos[ChapterIndex].LevelsInfo;

        for (int i = 0; i < LevelButtons.Count; i++)
        {
            var levelStatus = levelsInfo[i].Status;
            LevelButtons[i].interactable = levelStatus != -1;

            if (levelStatus == 1)
            {
                LevelButtons[i].GetComponent<Image>().color = Color.green;
            }
        }
    }

}