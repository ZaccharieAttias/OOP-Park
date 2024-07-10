using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;


public class quizFeedback : MonoBehaviour
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

        if (chapterNumber + 1 < ChapterInfos.Count)
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
        FeedbackScore.text = $"{100}%";
        FeedbackText.text = "Congratulations! You have completed the quiz and the chapter!";

        SceneManagement.UnlockNextLevel();
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
