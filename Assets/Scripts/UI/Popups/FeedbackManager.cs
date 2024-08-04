using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class FeedbackManager : MonoBehaviour
{
    [Header("Scripts")]
    public AiModelData AiModelData;

    [Header("UI Elements")]
    public GameObject Popup;
    public TMP_Text FeedbackScore;
    public TMP_Text FeedbackText;

    [Header("Buttons")]
    public Button ExitButton;
    public Button RetryButton;
    public Button NextLevelButton;


    public void Start()
    {
        GameplayData.Initialize();
        InitializeScripts();
        InitializeUIElements();
        InitializeButtons();
    }
    public void InitializeScripts()
    {
        AiModelData = GameObject.Find("Scripts/AiModelData").GetComponent<AiModelData>();
    }
    public void InitializeUIElements()
    {
        Popup = GameObject.Find("Canvas/Popups/Feedback");
        FeedbackScore = Popup.transform.Find("Background/Foreground/Contour/Percentage").GetComponent<TMP_Text>();
        FeedbackText = Popup.transform.Find("Background/Foreground/Contour/Text").GetComponent<TMP_Text>();
    }
    public void InitializeButtons()
    {
        ExitButton = Popup.transform.Find("Background/Foreground/Buttons/Exit").GetComponent<Button>();
        ExitButton.onClick.AddListener(() => ExitFactory());

        RetryButton = Popup.transform.Find("Background/Foreground/Buttons/Retry").GetComponent<Button>();
        RetryButton.onClick.AddListener(() => RetryFactory());

        NextLevelButton = Popup.transform.Find("Background/Foreground/Buttons/NextLevel").GetComponent<Button>();
        NextLevelButton.onClick.AddListener(() => NextLevelFactory());
    }

    public void ExitFactory()
    {
        var currentSceneName = SceneManager.GetActiveScene().name;
        var chapterNumber = currentSceneName[1].ToString();

        switch (chapterNumber)
        {
            case "0":
                SceneManager.LoadScene("ChapterTutorial");
                break;
            case "1":
                SceneManager.LoadScene("ChapterInheritance");
                break;
            case "2":
                SceneManager.LoadScene("ChapterPolymorphism");
                break;
            default:
                SceneManager.LoadScene("OnlinePark");
                break;
        }
    }
    public void RetryFactory()
    {
        var currentSceneName = SceneManager.GetActiveScene().name;
        SceneManagement.LoadScene(currentSceneName);
    }
    public void NextLevelFactory()
    {
        var currentSceneName = SceneManager.GetActiveScene().name;
        var chapterInfos = SceneManagement.GameplayInfo[0].ChapterInfos;

        int chapterNumber = int.Parse(currentSceneName[1].ToString());
        int levelNumber = int.Parse(currentSceneName[3].ToString());

        if (levelNumber + 1 <= chapterInfos[chapterNumber].LevelsInfo.Count) SceneManagement.LoadScene($"C{chapterNumber}L{levelNumber + 1}");
        else if (chapterNumber + 1 < chapterInfos.Count) SceneManagement.LoadScene($"C{chapterNumber + 1}L1");
        else SceneManagement.LoadScene("Playground");
    }

    public void LoadPopup()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null) player.SetActive(false);

        string sceneName = SceneManager.GetActiveScene().name;
        int score = AiModelData.CalculateScore();

        FeedbackScore.text = $"{score}%";
        FeedbackText.text = GetFeedbackText(score);

        bool isNextChallengePlayable;
        if (sceneName == "OnlinePlayground") isNextChallengePlayable = UpdateOnlineProgression(score);
        else isNextChallengePlayable = UpdateLocalProgression(score);

        NextLevelButton.interactable = isNextChallengePlayable;
    }
    public string GetFeedbackText(int score)
    {
        if (score < 50) return "Keep practicing! You can do better!";
        if (score < 70) return "You're close! Don't give up!";
        if (score < 85) return "Great job! You passed the level, but still need work!";
        if (score < 95) return "Excellent job! You're amazing!";

        return "Perfect! You're a master!";
    }

    public bool UpdateLocalProgression(int score)
    {
        const int MinScoreToUnlockNextLevel = 70;
        bool isNextChallengePlayable = false;

        var chapterInfos = SceneManagement.GameplayInfo[0].ChapterInfos;
        var currentSceneName = SceneManager.GetActiveScene().name;

        int chapterNumber = int.Parse(currentSceneName[1].ToString());
        int levelNumber = int.Parse(currentSceneName[3].ToString());

        var currentLevel = chapterInfos[chapterNumber].LevelsInfo[levelNumber - 1];
        currentLevel.Score = Math.Max(currentLevel.Score, score);
        currentLevel.Status = currentLevel.Score >= MinScoreToUnlockNextLevel ? 1 : 0;

        if (TryGetNextLevel(chapterInfos, chapterNumber, levelNumber, out var nextLevel))
        {
            if (nextLevel.Status != 1) nextLevel.Status = currentLevel.Score >= MinScoreToUnlockNextLevel ? 0 : -1;
            isNextChallengePlayable = currentLevel.Score >= MinScoreToUnlockNextLevel || nextLevel.Status == 1;
        }
        else
        {
            SceneManagement.LoadScene("Finish");
        }

        GameplayData.Save();
        return isNextChallengePlayable;
    }
    public bool UpdateOnlineProgression(int score)
    {
        const int MinScoreToUnlockNextLevel = 70;
        bool isNextChallengePlayable = false;

        string LevelName = GameObject.Find("LevelManager").GetComponent<LevelDownload>().LevelName1;
        var ChapterInfos = SceneManagement.GameplayInfo[1].ChapterInfos;
        var currentChapter = ChapterInfos.Find(chapter => chapter.Name == LevelName);

        if (currentChapter != null)
        {
            var currentLevel = currentChapter.LevelsInfo[0];

            currentLevel.Score = Math.Max(currentLevel.Score, score);
            currentLevel.Status = currentLevel.Score >= MinScoreToUnlockNextLevel ? 1 : 0;
        }

        else
        {
            ChapterInfos.Add(new ChapterInfo
            {
                ChapterNumber = ChapterInfos.Count,
                Name = LevelName,
                LevelsInfo = new List<LevelInfo> { new() { LevelNumber = 1, Score = score, Status = score >= MinScoreToUnlockNextLevel ? 1 : 0 } }
            });
        }

        GameplayData.Save();
        return isNextChallengePlayable;
    }
    public bool TryGetNextLevel(IList<ChapterInfo> chapterInfos, int chapterNumber, int levelNumber, out LevelInfo nextLevel)
    {
        if (levelNumber < chapterInfos[chapterNumber].LevelsInfo.Count)
        {
            nextLevel = chapterInfos[chapterNumber].LevelsInfo[levelNumber];
            return true;
        }
        else if (chapterNumber + 1 < chapterInfos.Count)
        {
            nextLevel = chapterInfos[chapterNumber + 1].LevelsInfo[0];
            return true;
        }

        nextLevel = null;
        return false;
    }


    public void ToggleOn()
    {
        LoadPopup();
        Popup.SetActive(true);
    }
    public void ToggleOff()
    {
        CharactersData.CharactersManager.DisplayCharacter(CharactersData.CharactersManager.CurrentCharacter);
        Popup.SetActive(false);
    }
}