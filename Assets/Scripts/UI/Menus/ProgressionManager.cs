using TMPro;
using UnityEngine;


public class ProgressionManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Transform LocalProgressionContent;
    public Transform OnlineProgressionContent;

    [Header("Buttons")]
    public GameObject ProgressionPrefab;


    public void Start()
    {
        InitializeUIElements();
        InitializeButtons();

        InitializePopup();
    }
    public void InitializeUIElements()
    {
        LocalProgressionContent = GameObject.Find("Canvas/Menus/Panel/Window/Inner/ButtonsGrid/Local/ScrollView/ViewPort/Content").transform;
        OnlineProgressionContent = GameObject.Find("Canvas/Menus/Panel/Window/Inner/ButtonsGrid/Online/ScrollView/ViewPort/Content").transform;
    }
    public void InitializeButtons()
    {
        ProgressionPrefab = Resources.Load<GameObject>("Buttons/Progression");
    }
    public void InitializePopup()
    {
        GameplayData.Initialize();
        GameplayData.Load();

        LoadLocalPopup();
        LoadOnlinePopup();
    }

    public void LoadLocalPopup()
    {
        var localChaptersInfo = SceneManagement.GameplayInfo[0].ChapterInfos;
        foreach (var chapterInfo in localChaptersInfo)
        {
            var progressionInformation = Instantiate(ProgressionPrefab, LocalProgressionContent);
            progressionInformation.transform.Find("ChapterName").GetComponent<TextMeshProUGUI>().text = chapterInfo.Name;

            int completedLevels = 0;
            int totalScore = 0;

            foreach (var level in chapterInfo.LevelsInfo)
            {
                if (level.Status == 1) completedLevels++;
                totalScore += level.Score;
            }

            int progressPercentage = completedLevels / chapterInfo.LevelsInfo.Count * 100;
            int averageScore = totalScore / chapterInfo.LevelsInfo.Count;

            progressionInformation.transform.Find("ProgressionPercentage").GetComponent<TextMeshProUGUI>().text = $"{progressPercentage}%";
            progressionInformation.transform.Find("AverageScore").GetComponent<TextMeshProUGUI>().text = $"{averageScore}";
            progressionInformation.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = DescriptionSentence(progressPercentage);
        }
    }
    public void LoadOnlinePopup()
    {
        var onlineChaptersInfo = SceneManagement.GameplayInfo[1].ChapterInfos;
        foreach (var chapterInfo in onlineChaptersInfo)
        {
            var progressionInformation = Instantiate(ProgressionPrefab, OnlineProgressionContent);
            progressionInformation.transform.Find("ChapterName").GetComponent<TextMeshProUGUI>().text = chapterInfo.Name;


            int completedLevels = chapterInfo.LevelsInfo[0].Status == 1 ? 100 : 0;
            int score = chapterInfo.LevelsInfo[0].Score;

            progressionInformation.transform.Find("ProgressionPercentage").GetComponent<TextMeshProUGUI>().text = $"{completedLevels}%";
            progressionInformation.transform.Find("AverageScore").GetComponent<TextMeshProUGUI>().text = $"{score}";
            progressionInformation.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = DescriptionSentence(completedLevels);
        }
    }
    public string DescriptionSentence(int progressPercentage)
    {
        if (progressPercentage == 0) return "Just getting started!";
        else if (progressPercentage < 30) return "Keep going, you can do it!";
        else if (progressPercentage < 50) return "Making progress, nice work!";
        else if (progressPercentage < 70) return "You're doing great!";
        else if (progressPercentage < 100) return "Almost there, keep it up!";
        else return "Congratulations, you did it!";
    }
}