using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class ChapterLoaderManager : MonoBehaviour
{
    [Header("UI Elements")]
    public string ChapterName;
    public List<Button> LevelButtons;


    public void Start()
    {
        InitializeProperties();
    }
    public void InitializeProperties()
    {
        ChapterName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        char temp = ChapterName[^-1];

        LevelButtons = new List<Button>(GameObject.Find("Canvas/Menus/Menu/Levels").GetComponentsInChildren<Button>());

        foreach (var button in LevelButtons)
        {
            // button.interactable = SceneManagement.ChapterInfos[temp].LevelsInfo[button.name].Status >= 0;
        }
    }
}