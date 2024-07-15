using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ProgressionMenu : MonoBehaviour
{

    public GameObject ProgressionPrefab;
    public Transform LocalProgressionContent;
    public Transform OnlineProgressionContent;
    void Start()
    {
        GameplayData.Initialize();
        GameplayData.Load();
        InitializePopup();
    }

    public void InitializePopup()
    {
        var localChaptersInfo = SceneManagement.GameplayInfo[0].ChapterInfos;
        foreach (var chapterInfo in localChaptersInfo)
        {
            var progression = Instantiate(ProgressionPrefab, LocalProgressionContent);
            progression.transform.Find("Chapter").GetComponent<TextMeshProUGUI>().text = chapterInfo.Name;

            float progress = 0;
            foreach (var level in chapterInfo.LevelsInfo)
            {
                if (level.Status == 1)
                {
                    progress++;
                }
            }
            progress = progress / chapterInfo.LevelsInfo.Count * 100;
            progression.transform.Find("Progression").GetComponent<TextMeshProUGUI>().text = $"{progress}%";
            progression.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = DescriptionSentence(progress);
        }

        var onlineChaptersInfo = SceneManagement.GameplayInfo[1].ChapterInfos;
        foreach (var chapterInfo in onlineChaptersInfo)
        {
            var progression = Instantiate(ProgressionPrefab, OnlineProgressionContent);
            progression.transform.Find("Chapter").GetComponent<TextMeshProUGUI>().text = chapterInfo.Name;

            float progress = 0;
            foreach (var level in chapterInfo.LevelsInfo)
            {
                if (level.Status == 1)
                {
                    progress++;
                }
            }
            progress = progress / chapterInfo.LevelsInfo.Count * 100;
            progression.transform.Find("Progression").GetComponent<TextMeshProUGUI>().text = $"{progress}%";
            progression.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = DescriptionSentence(progress);
        }
    }

    public string DescriptionSentence(float progress)
    {
        if (progress == 0) return "Fucking Noob";
        else if (progress < 30) return "Pussy ass bitch";
        else if (progress < 50) return "Half there mate!";
        else if (progress < 70) return "You are okey";
        else if (progress < 100) return "Almost There!!!";
        else return "Congrats!!!";
    }
}
