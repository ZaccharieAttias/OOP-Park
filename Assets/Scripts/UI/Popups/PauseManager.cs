using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Popup;

    [Header("Buttons")]
    public Button ResumeButton;
    public Button RestartButton;
    public Button ExitButton;


    public void Start()
    {
        InitializeUIElements();
        InitializeButton();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ToggleActivation();
    }
    public void InitializeUIElements()
    {
        Popup = GameObject.Find("Canvas/Popups/Pause");
    }
    public void InitializeButton()
    {
        ResumeButton = Popup.transform.Find("Background/Foreground/Buttons/Resume").GetComponent<Button>();
        ResumeButton.onClick.AddListener(ToggleOff);

        RestartButton = Popup.transform.Find("Background/Foreground/Buttons/Restart").GetComponent<Button>();
        RestartButton.onClick.AddListener(RestartScene);

        ExitButton = Popup.transform.Find("Background/Foreground/Buttons/Exit").GetComponent<Button>();
        ExitButton.onClick.AddListener(ExitScene);
    }

    public void RestartScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManagement.SceneResume("PauseManager");
        SceneManagement.LoadScene(currentSceneName);
    }
    public void ExitScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManagement.SceneResume("PauseManager");

        string nextScene = sceneName switch
        {
            _ when sceneName.Contains("C0") => "ChapterTutorial",
            _ when sceneName.Contains("C1") => "ChapterInheritance",
            _ when sceneName.Contains("C2") => "ChapterPolymorphism",
            _ => "Playground"
        };

        SceneManagement.LoadScene(nextScene);
    }

    public void ToggleOn()
    {
        SceneManagement.ScenePause("PauseManager");

        Popup.SetActive(true);
    }
    public void ToggleOff()
    {
        SceneManagement.SceneResume("PauseManager");

        Popup.SetActive(false);
    }
    public void ToggleActivation()
    {
        if (Popup.activeSelf) ToggleOff();
        else ToggleOn();
    }
}