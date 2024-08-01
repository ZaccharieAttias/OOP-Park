using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FeedbackManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Popup;
    public TMP_Text FeedbackScore;
    public TMP_Text FeedbackText;

    public Button ExitButton;
    public Button RetryButton;
    public Button NextLevelButton;


    public AiModelData AiModelData;

    public void Start()
    {
        InitializeProperties();
    }
    public void InitializeProperties()
    {
        AiModelData = GameObject.Find("Scripts/AiModelData").GetComponent<AiModelData>();

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
        var chapterNumber = currentSceneName[1].ToString();

        switch (chapterNumber)
        {
            case "0":
                SceneManager.LoadScene("ChapterTutorial");
                break;
            case "1":
                SceneManager.LoadScene("ChapterIneritance");
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
        string sceneName = SceneManager.GetActiveScene().name;
        int score = AiModelData.CalculateScore();

        FeedbackScore.text = $"{score}%";
        if (score < 50) FeedbackText.text = "Keep practicing! You can do better!";
        else if (score < 70) FeedbackText.text = "You're close!, dont give up!";
        else if (score < 85) FeedbackText.text = "Great job! You passed the level, but still need work!";
        else if (score < 95) FeedbackText.text = "Excellent job! You're amazing!";
        else FeedbackText.text = "Perfect! You're a master!";

        if (sceneName == "OnlinePlayground") SceneManagement.CompleteOnline();
        else if (score >= 70) SceneManagement.UnlockNextLevel();

        NextLevelButton.interactable = score >= 70;
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
