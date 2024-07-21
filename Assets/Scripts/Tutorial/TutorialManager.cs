using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public JsonUtilityManager jsonUtilityManager;



    public Popup CommandPopup;
    public GameObject TutorialTip;

    public GameObject MainForeground;
    public TextMeshProUGUI MainTutorialTipText;

    public GameObject RightForeground;
    public TextMeshProUGUI RightTutorialTipText;

    public GameObject LeftForeground;
    public TextMeshProUGUI LeftTutorialTipText;




    public GameObject Player;
    public Vector3 StartPosition;
    public GameObject CheckPoint;
    

    public GameObject SwapSceenToCenter;
    public GameObject SwapSceenToGameplay;


    public int check = 0;
    public bool hasMoved = false;
    public bool hasJumped = false;
    public bool hasSeenCheckpoint = false;



    public void Start()
    {
        // si la scene est la scene de tutoriel
        if (SceneManager.GetActiveScene().name == "C0L0")
            jsonUtilityManager.Load();
        CommandPopup.Show("<color=\"yellow\">[W]</color> Jump\n<color=\"yellow\">[A]</color> Left Move        <color=\"yellow\">[D]</color> Right Move", 2);
        StartPosition = Player.transform.position;
    }
    public void Update()
    {
        if (SceneManager.GetActiveScene().name == "C0L0")
        {
            if (Player.transform.position.x != StartPosition.x && check == 0 && !hasMoved)
                hasMoved = true;
            if (Player.transform.position.y > StartPosition.y && check == 0 && !hasJumped)
                hasJumped = true;

            else if (Vector3.Distance(Player.transform.position, CheckPoint.transform.position) < 3 && !hasSeenCheckpoint && check == 1)
            {
                hasSeenCheckpoint = true;
                CommandPopup.Show("The flags are checkpoints.\nThey will turn red once they have been touch.", 4);
            }
            else if (Player.GetComponent<GameController>().FeedbackManager.DeathsCount >= 3 && !TutorialTip.activeSelf && check == 2)
            {
                check++;
                TutorialTip.SetActive(true);
                MainTutorialTipText.text = "You may have noticed that it's impossible to reach the second Checkpoint.\n\nTo reach it, you'll need to use your character's hierarchical tree.\n\nClick on the button at the bottom right of your screen to access a new menu.";
                CommandPopup.gameObject.SetActive(false);
            }
            else if (GameObject.Find("Canvas/Menus/CharacterCenter").activeSelf && check == 3)
            {
                check++;
                MainTutorialTipText.text = "Welcome to your Character Control Center.\n\nHere you'll be able to access all the information you need to discover each OOP theme and its use in the world of programming in a simpler, more formal way. \n\n\n\nClick on [Enter] to continue.";
            }
            else if (Input.GetKeyDown(KeyCode.Return) && check == 4)
            {
                MainForeground.SetActive(false);
                RightForeground.SetActive(true);
                RightTutorialTipText.text = "This part of the menu contains your Class/Character hierarchical tree.\n\nIn OOP Park, a class is represented by a character. By selecting one, you'll be able to play with that character, which is like using a specific class in a code. Use its characteristics/properties to complete the level.\n\n\n\nClick on the [Enter] key to continue.";
                GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details").SetActive(false);
                check++;
            }
            else if (Input.GetKeyDown(KeyCode.Return) && check == 5)
            {
                RightForeground.SetActive(false);
                LeftForeground.SetActive(true);
                LeftTutorialTipText.text = "On the right-hand side of the menu, you'll have access to the attributes and methods of the selected class.\n\nIn OOP Park, they represent a character's characteristics and possible actions/powers. For your character to be able to perform a certain action, he must have this in his methods BUT ALSO in his attributes, because without initial values an action cannot be performed.\n\n[Right Click] on an attribute or method will display a simple description of it.\n\n\n\nClick on the [Enter] key to continue.";
                GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details").SetActive(true);
                GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Tree").SetActive(false);
                check++;
            }
            else if (Input.GetKeyDown(KeyCode.Return) && check == 6)
            {
                LeftForeground.SetActive(false);
                GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Tree").SetActive(true);
                MainForeground.SetActive(true);
                MainTutorialTipText.text = "You now have a basic understanding of how to use this menu.\n\nNow it's up to you to complete this level!\n\nClick on the button at the bottom right of your screen to return on the gameplay screen.\n\n\n\nClick on the [Enter] key to close.";
                SwapSceenToGameplay.SetActive(true);
                check++;
            }
            else if (Input.GetKeyDown(KeyCode.Return) && check == 7)
            {
                TutorialTip.SetActive(false);
                SwapSceenToGameplay.SetActive(false);
                check++;
            }
        }
    }
}
