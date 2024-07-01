using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class FeedbackManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Popup;
    public TMP_Text FeedbackScore;
    public TMP_Text FeedbackText;


    [Header("Score Data")]
    public int DeathsCount;


    public void Start()
    {
        InitializeProperties();
    }
    public void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/Feedback");
        FeedbackScore = Popup.transform.Find("Background/Foreground/Percentage").GetComponent<TMP_Text>();
        FeedbackText = Popup.transform.Find("Background/Foreground/Text").GetComponent<TMP_Text>();

        DeathsCount = 0;
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            ToggleOn();
    }


    // This will be linked using add listener when we hit the door/last checkpoint
    public void LoadPopup()
    {
        var timePass = (int)Time.timeSinceLevelLoad;

        FeedbackScore.text = $"100% - {DeathsCount * 7} - TimeFactor";
        FeedbackText.text = DeathsCount == 0 ? "Congratulations! You have completed the level without dying!" : "You have died. Try again!";

        // Maybe based on the score, unlock the next level/chapter (button and current level/chapter)
        SceneManagement.UnlockNextLevel();


        // Add here logic to unlock the next level/chapter,

        // Might need to store the current level/chapter in a folder for player tracking    
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
