using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;


public class FeedbackManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Popup;
    public TMP_Text FeedbackScore;
    public TMP_Text FeedbackText;

    public Button ExitButton;
    public Button RetryButton;
    public Button NextLevelButton;


    [Header("Score Data")]
    public int DeathsCount;


    public void Start()
    {
        InitializeProperties();
    }
    public void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/Feedback");
        FeedbackScore = Popup.transform.Find("Background/Foreground/Contour/Percentage").GetComponent<TMP_Text>();
        FeedbackText = Popup.transform.Find("Background/Foreground/Contour/Text").GetComponent<TMP_Text>();

        DeathsCount = 0;

        ExitButton = Popup.transform.Find("Background/Foreground/Buttons/Exit").GetComponent<Button>();
        ExitButton.onClick.AddListener(() => ExitFactory());

        RetryButton = Popup.transform.Find("Background/Foreground/Buttons/Retry").GetComponent<Button>();
        RetryButton.onClick.AddListener(() => RetryFactory());

        NextLevelButton = Popup.transform.Find("Background/Foreground/Buttons/NextLevel").GetComponent<Button>();
        NextLevelButton.onClick.AddListener(() => NextLevelFactory());
    }

    public void ExitFactory()
    {
        var currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        var chapterNumber = int.Parse(currentSceneName[1].ToString());

        string chapterName = SceneManagement.GameplayInfo[0].ChapterInfos[chapterNumber].Name;

        SceneManagement.LoadScene(chapterName);
    }

    public void RetryFactory()
    {
        var currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        SceneManagement.LoadScene(currentSceneName);
    }

    public void NextLevelFactory()
    {
        var currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        var ChapterInfos = SceneManagement.GameplayInfo[0].ChapterInfos;

        int chapterNumber = int.Parse(currentSceneName[1].ToString());
        int levelNumber = int.Parse(currentSceneName[3].ToString());

        if (levelNumber + 1 <= ChapterInfos[chapterNumber].LevelsInfo.Count)
        {
            SceneManagement.LoadScene($"C{chapterNumber}L{levelNumber + 1}");
        }

        else if (chapterNumber + 1 < ChapterInfos.Count)
        {
            SceneManagement.LoadScene($"C{chapterNumber + 1}L1");
        }

        else
        {
            SceneManagement.LoadScene("Playground");
        }
    }

    public void LoadPopup()
    {
        GameObject.Find("Player").SetActive(false);

        string SceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        var timePass = (int)Time.timeSinceLevelLoad;

        int score = 100 - DeathsCount * 7 - timePass;
        if (SceneName == "C2L4")
        {
            int temp = GameObject.Find("Canvas/Popups").GetComponent<AbstractClassCheck>().TriesCounter;
            score = 100 - temp * 7;
        }
        FeedbackScore.text = $"{score}%";
        FeedbackText.text = DeathsCount == 0 ? "Congratulations! You have completed the level without dying!" : "You have died. Try again!";

        if (score >= 70) SceneManagement.UnlockNextLevel();
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
