using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PauseManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Popup;
    public Button ResumeButton;
    public Button RestartButton;
    public Button ExitButton;

    public void Start()
    {
        InitializeProperties();
    }
    public void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/Pause");

        ResumeButton = Popup.transform.Find("Background/Foreground/Buttons/Resume").transform.GetComponent<Button>();
        ResumeButton.onClick.AddListener(() => ToggleOff());

        RestartButton = Popup.transform.Find("Background/Foreground/Buttons/Restart").transform.GetComponent<Button>();
        RestartButton.onClick.AddListener(() => RestartFactory());

        ExitButton = Popup.transform.Find("Background/Foreground/Buttons/Exit").transform.GetComponent<Button>();
        ExitButton.onClick.AddListener(() => ExitFactory());
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleActivation();
        }
    }

    public void RestartFactory()
    {
        var currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        SceneManagement.SceneResume("PauseManager");
        SceneManagement.LoadScene(currentSceneName);
    }

    public void ExitFactory()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManagement.SceneResume("PauseManager");
        if (sceneName.Contains("C0"))
        {
            SceneManagement.LoadScene("ChapterTutorial");
        }
        else if (sceneName.Contains("C1"))
        {
            SceneManagement.LoadScene("ChapterIneritance");
        }
        else if (sceneName.Contains("C2"))
        {
            SceneManagement.LoadScene("ChapterPolymorphism");
        }
        else
        {
            SceneManagement.LoadScene("Playground");
        }
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
        if (Popup.activeSelf)
        {
            ToggleOff();
        }
        else
        {
            ToggleOn();
        }
    }
}
