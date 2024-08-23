using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

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
    public GameObject CheckPoint1;
    public GameObject CheckPoint2;
    public GameObject Wall;
    public GameObject Brick;


    public GameObject SwapSceenToCenter;
    public GameObject SwapSceenToGameplay;
    public GameObject CharacterCreationPlus;
    public GameObject Plus;
    public GameObject Mission;
    public GameObject MissionWall;
    public GameObject MissionPrefab;
    public GameObject TreeContent;


    public int check = 0;
    public bool hasMoved = false;
    public bool hasJumped = false;
    public bool hasSeenCheckpoint = false;
    public bool hasSwapped = false;
    public int nbrOfChracaterSelected = 0;
    public string Previouscharacter;
    public bool clikedForCreation = false;
    public bool isOk = false;

    public AiModelData AiModelData;

    public void Start()
    {
        AiModelData = GameObject.Find("Scripts/AiModelData").GetComponent<AiModelData>();

        if (SceneManager.GetActiveScene().name == "C0L1")
        {
            jsonUtilityManager.Load();
            CommandPopup.Show("<color=\"yellow\">[W]</color> Jump\n<color=\"yellow\">[A]</color> Left Move        <color=\"yellow\">[D]</color> Right Move", 2, "C0L1");
        }
        else if (SceneManager.GetActiveScene().name == "C0L2")
        {
            jsonUtilityManager.Load();
            TutorialTip.SetActive(true);
        }
        else
        {
            jsonUtilityManager.Load();
            TutorialTip.SetActive(true);
        }

        StartPosition = Player.transform.position;
        Previouscharacter = CharactersData.CharactersManager.CurrentCharacter.Name;
    }
    public void Update()
    {
        if (SceneManager.GetActiveScene().name == "C0L1")
        {
            if (check == 0)
            {
                if (Player.transform.position.x != StartPosition.x && !hasMoved)
                    hasMoved = true;
                if (Player.transform.position.y > StartPosition.y && !hasJumped)
                    hasJumped = true;
            }
            else if (Vector3.Distance(Player.transform.position, CheckPoint1.transform.position) < 3 && !hasSeenCheckpoint && check == 1)
            {
                hasSeenCheckpoint = true;
                CommandPopup.Show("The flags are checkpoints.\nThey will turn red once they have been touch.", 4, "C0L1");
            }
            else if (Player.GetComponent<GameController>().AiModelData.DeathsCount >= 3 && !TutorialTip.activeSelf && check == 2)
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
            else if (check == 8)
            {
                if (Vector3.Distance(Player.transform.position, Brick.transform.position) < 3)
                {
                    CommandPopup.gameObject.SetActive(true);
                    CommandPopup.Show("These are bricks.\nYou can break them by shooting on", 4, "C0L1");
                    check++;
                }
            }

        }
        else if (SceneManager.GetActiveScene().name == "C0L2")
        {
            if (Input.GetKeyDown(KeyCode.Return) && check == 0)
            {
                TutorialTip.SetActive(false);

                CommandPopup.Show("What you see in gray are walls.\nThey can be climbed using a certain method.", 6, "C0L2");
                check++;
                RestrictionManager.Instance.AllowUpcasting = false;
                CharactersData.CharactersManager.DisplayCharacter(CharactersData.CharactersManager.CharactersCollection[0]);
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
                SwapSceenToGameplay.SetActive(false);
            }
            else if (Previouscharacter != CharactersData.CharactersManager.CurrentCharacter.Name && check == 5)
            {
                Previouscharacter = CharactersData.CharactersManager.CurrentCharacter.Name;
                nbrOfChracaterSelected++;
                if (nbrOfChracaterSelected >= 4)
                {
                    TutorialTip.SetActive(true);
                    RightForeground.SetActive(true);
                    RightTutorialTipText.text = "After inspecting the classes, you should have noticed that two classes share the wallJump attribute, with differents values, but not the method for performing the wallJump.\n\nIts time to extend your tree by creating a new class/character.\n\nThe button dedicated to this action has just been added to the menu (highlighted in red).\nBy clicking on it, you'll be able to select the parent of your new class/character by selecting it directly in the tree, and then confirm the creation.\n\n\n\nClick on the [Enter] key to close.";
                    CharacterCreationPlus.SetActive(true);
                    Plus.SetActive(true);
                    check++;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return) && check == 6)
            {
                TutorialTip.SetActive(false);
                RightForeground.SetActive(false);
                CommandPopup.Show("Create to Emily a child.", 20, "C0L2");
                check++;
            }
            else if (CharactersData.CharactersManager.CharactersCollection.Count == 5 && check == 7)
            {
                StartCoroutine(Wait(2f));
                TutorialTip.SetActive(true);
                LeftForeground.SetActive(true);
                LeftTutorialTipText.text = "You have just created a new class/character.\n\nNow it's up to you to add attributes and methods to your class, modify their accessibility by selecting them, play with your new character.\n\nIf you make a mistake, you can always delete your class. Take into consideration that the allocation of methods is restricted in your new class: you can ONLY SELECT TWO methods, from your methods available, to PLAY with.\nMoreover, attribute values are not necessarily the same for all classes.\n\nFind the right class, the right value and climb that wall to the first Checkpoint!\n\n\n\nClick on the [Enter] key to close.";
                check++;
            }
            else if (Input.GetKeyDown(KeyCode.Return) && check == 8)
            {
                CommandPopup.CanvasGroup.alpha = 0;
                TutorialTip.SetActive(false);
                LeftForeground.SetActive(false);
                CommandPopup.Show("Add to your new character the method WallJump: it will use the closest value of wallJump (From Emily).", 20, "C0L2");
                check++;
            }
            else if (CharactersData.CharactersManager.CurrentCharacter.Methods.Find(x => x.Name == "WallJump") != null && check == 9 && !isOk)
            {
                CommandPopup.CanvasGroup.alpha = 0;
                SwapSceenToGameplay.SetActive(true);
                isOk = true;
            }
            else if (CharactersData.CharactersManager.CurrentCharacter.Methods.Find(x => x.Name == "WallJump") == null && check == 9)
            {
                SwapSceenToGameplay.SetActive(false);
                isOk = false;
            }
            else if (Vector3.Distance(Player.transform.position, CheckPoint1.transform.position) < 2 && !hasSeenCheckpoint && check == 9 && isOk)
            {
                hasSeenCheckpoint = true;
                TutorialTip.SetActive(true);
                MainForeground.SetActive(true);
                MainTutorialTipText.text = "Congratulations! You've reached the first checkpoint.\n\nNow we'll explain how upcasting works.\nTo proceed an upcast, press [G] on your keyboard, this will open a contextual menu in which you can upcast your currently played class as one of it ancestors.\nWhen you upcast, your character will be treated as if it belongs to one of its parent classes. This means that while your character will keep its unique abilities forces, it will only have access to the methods defined by the selected ancestor class.\n\nChoose wisely and experiment with how the inherited abilities affect your gameplay!\nFor greater interactivity, you will need to take the correct appearance to remove the wall that blocking the access.\n\nTip: To complete the level, you'll need to select a correct child and upcast to it parent.\n\n\n\nClick on the [Enter] key to close.";
                
                
                RestrictionManager.Instance.AllowUpcasting = true;
                Mission.SetActive(true);
                MissionWall.SetActive(true);
                GameObject.Find("Canvas/Popups").GetComponent<CharacterChallengeManager>().Walls.Add(MissionWall);
                Mission.AddComponent<StageCollision>().Challenge = 1;
                GameObject MissionPop = GameObject.Find("Canvas/Popups/Mission1");
                TMP_Text txt = MissionPop.transform.Find("Background/Foreground/Mssion/Mission").GetComponent<TMP_Text>();
                txt.text = "For this masked ball, you'll need an exemplary hairstyle in addition to your magnificent cape.";
                List<string> appearancesCondition = new List<string>();
                appearancesCondition.Add("Mask");
                appearancesCondition.Add("Cape");
                appearancesCondition.Add("Hair");
                GameObject.Find("Canvas/Popups").GetComponent<CharacterChallengeManager>().InitializeUIOnlineElements(1, appearancesCondition);
                
                
                
                foreach (Transform child in TreeContent.transform)
                    Destroy(child.gameObject);

                string previouspath = CharactersData.FilePath;
                CharactersData.FilePath = Path.Combine(Application.dataPath, "StreamingAssets", "Resources/Json", "C0L2Second", "Characters.json");
                CharactersData.Load();
                CharactersGameObjectData.Load();
                CharactersData.FilePath = previouspath;
                GameObject.Find("Scripts/CharacterEditor").GetComponent<CharacterEditor1>().LoadFromJson();
                GameObject.Find("Scripts/PowerUp").GetComponent<Powerup>().ApplyPowerup(CharactersData.CharactersManager.CurrentCharacter);
                
                check++;
            }
            else if (Input.GetKeyDown(KeyCode.Return) && check == 10)
            {
                TutorialTip.SetActive(false);
                MainForeground.SetActive(false);
                check++;
            }
            else if (check == 11)
            {
                CommandPopup.Show("Select a child and Upcast in order to access to the trophy.", 20, "C0L2");
                check++;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return) && check == 0)
            {
                TutorialTip.SetActive(false);
                MainForeground.SetActive(false);
                RestrictionManager.Instance.AllowEncapsulation = false;
                RestrictionManager.Instance.AllowOverride = false;
                CommandPopup.Show("Cube objects can be carried and moved using a certain method. They don't all have the same weight! The yellow ones are EXTREMELY heavy.", 3, "C0L3");
                check++;
            }
            else if (hasSwapped && check == 1)
            {
                CommandPopup.CanvasGroup.alpha = 0;
                SwapSceenToGameplay.SetActive(false);
                CharacterCreationPlus.SetActive(false);
                check++;
                hasSwapped = false;
            }
            else if (Previouscharacter != CharactersData.CharactersManager.CurrentCharacter.Name && check == 2)
            {
                Previouscharacter = CharactersData.CharactersManager.CurrentCharacter.Name;
                nbrOfChracaterSelected++;
                if (nbrOfChracaterSelected >= 3)
                {
                    TutorialTip.SetActive(true);
                    RightForeground.SetActive(true);
                    RightTutorialTipText.text = "Once you've inspected the classes, you'll have noticed a number of new features. To begin with, let's focus on the Abstract classes. In the tree provided, Ava (tree head) is an abstract class. These classes act as templates, defining a common interface and shared functionality for other playable classes. They are not playable to support the fact that it's an abstract class. If you select it, it won't be highlighted in orange in the tree, you won't be able to return to the gameplay screen and the character name displayed will indicate that the class is abstract.\n\n\n\nClick on the [Enter] key to continue.";
                    check++;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return) && check == 3)
            {
                RightForeground.SetActive(false);
                LeftForeground.SetActive(true);
                LeftTutorialTipText.text = "Your classes now belong to families that can provide them special abilities. You'll see which family it belongs to in the “Special Ability” section. Families are becoming more and more specific to maintain the concept of polymorphism. The most accomplished (the youngest) will provide the character with a boost to the corresponding attribute. Special Abilities cannot be modified or removed, and are only defined when a new character is created.\n\n\n\nClick on the [Enter] key to continue.";
                Plus.SetActive(true);
                check++;
            }
            else if (Input.GetKeyDown(KeyCode.Return) && check == 4)
            {
                LeftForeground.SetActive(false);
                MainForeground.SetActive(true);
                Plus.SetActive(false);
                Plus = GameObject.Find("Canvas/Popups/Tutorial/Background/Foreground2/Buttons/Description");
                MainTutorialTipText.text = "To move the yellow cubes, you'll need to create a character specialized in grabbing objects. Given that “Grabbing” is a “Manual” method, we'll focus on Fiona, which is a manual character.\n\nCreate a child for Fiona, NON ABSTRACT in order to play with, specializing in the “Grabbing” family and adding it the “Grabbing” method. Thanks to this class/character, you'll be able to move any cube in the game.\n\n\n\nClick on the [Enter] key to close.";
                check++;
            }
            else if (Input.GetKeyDown(KeyCode.Return) && check == 5)
            {
                TutorialTip.SetActive(false);
                MainForeground.SetActive(false);
                CharacterCreationPlus.SetActive(true);
                CharacterCreationPlus.GetComponent<Button>().onClick.AddListener(() => hasClicked());
                check++;
            }
            else if (clikedForCreation && check == 6)
            {
                GameObject.Find("Canvas/Popups/CharacterCreation/Buttons/CharacterType/Button").GetComponent<Toggle>().interactable = false;
                check++;
            }
            else if (CharactersData.CharactersManager.CurrentCharacter.Methods.Find(x => x.Name == "Grabbing") != null && check == 7 && !isOk)
            {
                SwapSceenToGameplay.SetActive(true);
                isOk = true;
            }
            else if (CharactersData.CharactersManager.CurrentCharacter.Methods.Find(x => x.Name == "Grabbing") == null && check == 7)
            {
                SwapSceenToGameplay.SetActive(false);
                isOk = false;
            }
            else if (hasSwapped && check == 7 && isOk)
            {
                CommandPopup.Show("Press 'E' next to a cube to grab it.", 2, "C0L3");
                check++;
                hasSwapped = false;
                isOk = false;
            }
            else if (Vector3.Distance(Player.transform.position, CheckPoint1.transform.position) < 1 && !hasSeenCheckpoint && check == 8)
            {
                hasSwapped = false;
                hasSeenCheckpoint = true;
                CommandPopup.Show("Well done!!\n\nNow we will help you to reach the top by using Encaspulation.\nSwap to the Character Center.", 50, "C0L3");
                CharacterCreationPlus.SetActive(false);
                check++;
            }
            else if (hasSwapped && check == 9)
            {
                CommandPopup.CanvasGroup.alpha = 0;
                RestrictionManager.Instance.AllowEncapsulation = true;
                hasSeenCheckpoint = false;
                SwapSceenToGameplay.SetActive(false);
                TutorialTip.SetActive(true);
                LeftForeground.SetActive(true);
                LeftTutorialTipText.text = "Encapsulation allows you to decide whether each character attribute has access to a setter and a getter irrespective of its access modifier. If an attribute includes a setter, its value can be modified. If it has a getter method, its value can be accessed by descended classes, allowing controlled visibility and modification of class properties. This ensures that attributes are accessed and manipulated in a controlled manner.\n\nAs you may have noticed, next to Description there is a new button “A”, for All. By clicking on it, you'll find a list of all Get and Set methods attached to the attributes of your selected class.\n\n\n\nClick on the [Enter] key to continue.";
                check++;
                Plus.SetActive(true);
                hasSwapped = false;
            }
            else if (Input.GetKeyDown(KeyCode.Return) && check == 10)
            {
                LeftTutorialTipText.text = "By right-clicking on an attribute or method, the description section will be updated to display a description of it.\n\nFor an attribute, two buttons will be added: “G” for Get and “S” for Set. Selecting one will create a Get or Set method for the attribute.\n\n\n To continue in the level, you'll need to access a higher platform. To do this, we'll use George's MultipleJumps capability. Select George, and as you will see he already has a Get and Set method for the multipleJump attribute.\n\n\n\nClick on the [Enter] key to close.";
                check++;
            }
            else if (Input.GetKeyDown(KeyCode.Return) && check == 11)
            {
                Plus.SetActive(false);
                LeftForeground.SetActive(false);
                TutorialTip.SetActive(false);
                CommandPopup.Show("Select George.", 20, "C0L3");
                check++;
            }
            else if (CharactersData.CharactersManager.CurrentCharacter.Name == "George" && check == 12 && !isOk)
            {
                SwapSceenToGameplay.SetActive(true);
                isOk = true;
            }
            else if (CharactersData.CharactersManager.CurrentCharacter.Name != "George" && check == 12)
            {
                SwapSceenToGameplay.SetActive(false);
                isOk = false;
            }
            else if (hasSwapped && check == 12 && isOk)
            {
                CommandPopup.CanvasGroup.alpha = 0;
                TutorialTip.SetActive(true);
                MainForeground.SetActive(true);
                MainTutorialTipText.text = "Now we'll explain how utilize your Set and Get.\nTo proceed, press [G] on your keyboard, this will open a contextual menu in which you can manage your attributes.\nHere you can select the attribute you wish to work with. All attributes with Setters will be displayed in this list\n\nIn OOP Park, in order to modify a value, you need to have access to it via its Getter.\nIn the Getters section of the menu, you'll be able to choose a value for your attribute from among the existing values of that attribute, both yours and your ancestors', accessible via Getters.\nAnd in the lower part of the menu, if your attribute has a Set method, you'll be able to modify the value of your attribute as you wish.\n\nTip: To continue in the level, you'll need to change via a get or set method tha value of multupleJumps.\n\n\n\nClick on the [Enter] key to close.";
                check++;
                isOk = false;
            }
            else if (Input.GetKeyDown(KeyCode.Return) && check == 13)
            {
                MainForeground.SetActive(false);
                TutorialTip.SetActive(false);
                check++;
            }
            else if (Vector3.Distance(Player.transform.position, CheckPoint2.transform.position) < 1 && check == 14)
            {
                hasSeenCheckpoint = true;
                MainForeground.SetActive(true);
                TutorialTip.SetActive(true);
                MainTutorialTipText.text = "Congratulations! You've reached the second checkpoint.\n\nNow we'll explain how to use the Override method.\nTo proceed, press [G] on your keyboard, this will open a contextual menu in which you can override your currently played class by changing his appearance.\nYou'll be able to choose and call non-private methods from ancestor classes through overriding.\nFor greater interactivity, you must modify your appearance to meet certain conditions. These are available by touching the object ? in the level.\n\n\n\nClick on the [Enter] key to close.";
                check++;
                RestrictionManager.Instance.AllowEncapsulation = false;
                RestrictionManager.Instance.AllowOverride = true;
            }
            else if (Input.GetKeyDown(KeyCode.Return) && check == 15)
            {
                TutorialTip.SetActive(false);
                MainForeground.SetActive(false);
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
    public void hasClicked()
    {
        clikedForCreation = true;
    }
}
