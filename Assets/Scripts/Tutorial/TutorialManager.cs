using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.EventSystems;

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
    public GameObject Wall;
    

    public GameObject SwapSceenToCenter;
    public GameObject SwapSceenToGameplay;
    public GameObject CharacterCreationPlus;
    public GameObject Plus;


    public int check = 0;
    public bool hasMoved = false;
    public bool hasJumped = false;
    public bool hasSeenCheckpoint = false;
    public bool hasSwapped = false;
    public int nbrOfChracaterSelected = 0;
    public string Previouscharacter;



    public void Start()
    {
        // si la scene est la scene de tutoriel
        if (SceneManager.GetActiveScene().name == "C0L0")
        {
            jsonUtilityManager.Load();
            CommandPopup.Show("<color=\"yellow\">[W]</color> Jump\n<color=\"yellow\">[A]</color> Left Move        <color=\"yellow\">[D]</color> Right Move", 2);
        }
        else if (SceneManager.GetActiveScene().name == "C0L1")
        {
            jsonUtilityManager.Load();
            TutorialTip.SetActive(true);
        }
        
        StartPosition = Player.transform.position;
        Previouscharacter = CharactersData.CharactersManager.CurrentCharacter.Name;
    }
    public void Update()
    {
        if (SceneManager.GetActiveScene().name == "C0L0")
        {
            if (check == 0)
            {
                if (Player.transform.position.x != StartPosition.x && !hasMoved)
                hasMoved = true;
                if (Player.transform.position.y > StartPosition.y && !hasJumped)
                hasJumped = true;
            }
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
                MainTutorialTipText.text = "Welcome to your Character Control Center.\n\nHere you'll be able to access all the information you need to discover each OOP theme and its use in the world of programming in a simpler, more formal way.\n\n\n\nClick on [Enter] to continue.";
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
        else if (SceneManager.GetActiveScene().name == "C0L1")
        {
            if (Input.GetKeyDown(KeyCode.Return) && check == 0)
            {
                TutorialTip.SetActive(false);

                CommandPopup.Show("What you see in gray are walls.\nThey can be climbed using a certain method.", 6);
                check++;
            }
            else if (hasSwapped && check == 1)
            {
                CharacterCreationPlus.SetActive(false);
                CommandPopup.CanvasGroup.alpha = 0;
                TutorialTip.SetActive(true);
                MainTutorialTipText.text = "New level, new tree.\n\nEach level in OOP Park has its own tree to diversify the exercises.\nIt's up to you to pay attention to every detail of each class, to correctly choose the class that will allow you to continue and complete the level.\n\n\n\nClick on [Enter] to continue.";
                check++;
            }
            else if (Input.GetKeyDown(KeyCode.Return) && check == 2)
            {
                MainForeground.SetActive(false);
                RightForeground.SetActive(true);
                RightTutorialTipText.text = "For climbing walls, you'll have to use the 'WallJump' method to reach the first checkpoint.\n\nTo do this, you'll need to search for a class that contains the corresponding attribute AND method.\n\n\n\nClick on the [Enter] key to continue.";
                check++;
            }
            else if (Input.GetKeyDown(KeyCode.Return) && check == 3)
            {
                RightForeground.SetActive(false);
                LeftForeground.SetActive(true);
                LeftTutorialTipText.text = "As you can see, attributes and methods are distinguished by three colors, each representing the type of accessibility: green is 'public', orange is 'protected', and red is 'private'.\n\nAccording on the logic of each, it's up to you to understand when and who can access them. Their logic is the same as in the coding world.\n\n\n\nClick on the [Enter] key to continue.";
                check++;
            }
            else if (Input.GetKeyDown(KeyCode.Return) && check == 4)
            {
                LeftForeground.SetActive(false);
                TutorialTip.SetActive(false);
                check++;
            }
            else if (Previouscharacter != CharactersData.CharactersManager.CurrentCharacter.Name && check == 5)
            {
                Previouscharacter = CharactersData.CharactersManager.CurrentCharacter.Name;
                nbrOfChracaterSelected++;
                if (nbrOfChracaterSelected >= 5)
                {
                    TutorialTip.SetActive(true);
                    RightForeground.SetActive(true);
                    RightTutorialTipText.text = "After inspecting the classes, you should have noticed that two classes share the wallJump attribute, but not the method for performing the wallJump.\n\nIts time to extend your tree by creating a new class/character.\n\nThe button dedicated to this action has just been added to the menu (highlighted in red).\nBy clicking on it, you'll be able to select the parent of your new class/character by selecting it directly in the tree, and then confirm the creation.\n\n\n\nClick on the [Enter] key to close.";
                    CharacterCreationPlus.SetActive(true);
                    Plus.SetActive(true);
                    check++;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return) && check == 6)
            {
                TutorialTip.SetActive(false);
                RightForeground.SetActive(false);
                check++;
            }
            else if (CharactersData.CharactersManager.CharactersCollection.Count == 5 && check == 7)
            {
                StartCoroutine(Wait(2f));
                TutorialTip.SetActive(true);
                LeftForeground.SetActive(true);
                LeftTutorialTipText.text = "You have just created a new class/character.\n\nNow it's up to you to add attributes and methods to your class, modify their accessibility by selecting them, play with your new character.\n\nIf you make a mistake, you can always delete your class. Take into consideration that the allocation of attributes and methods is restricted according to what already exists in your class's parents.\nMoreover, attribute values are not necessarily the same for all classes.\n\nFind the right class and climb that wall to the first Checkpoint!\n\n\n\nClick on the [Enter] key to close.";
                check++;
            }
            else if (Input.GetKeyDown(KeyCode.Return) && check == 8)
            {
                TutorialTip.SetActive(false);
                LeftForeground.SetActive(false);
                check++;
            }
            else if (Vector3.Distance(Player.transform.position, CheckPoint.transform.position) < 3 && !hasSeenCheckpoint && check == 9)
            {
                hasSeenCheckpoint = true;
                TutorialTip.SetActive(true);
                MainTutorialTipText.text = "Congratulations! You've reached the first checkpoint.\n\nNow we'll explain how upcasting works.\nTo proceed an upcast, press [G] on your keyboard, this will open a contextual menu in which you can upcast your currently played class.\nYou'll be able to choose which class to upcast and which method to call.\nFor greater interactivity, you'll also be able to choose the time or number of times the method will be used.\n\nTip: To complete the level, you'll need to inverse the gravity and break walls with fireballs.\n\n\n\nClick on the [Enter] key to close.";
                check++;
            }
            else if (Input.GetKeyDown(KeyCode.Return) && check == 10)
            {
                TutorialTip.SetActive(false);
                check++;
            }
        }
    }
    private IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
    }
    public void Swapp()
    {
        hasSwapped = true;
    }
}
