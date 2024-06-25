using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


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
        // Reload level or clean the flags or something?
    }

    public void ExitFactory()
    {
        // SceneManagement.Chapter...
    }

    public void ToggleOn()
    {
        SceneManagement.ScenePause();
        Popup.SetActive(true);
    }
    public void ToggleOff()
    {
        SceneManagement.SceneResume();
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
