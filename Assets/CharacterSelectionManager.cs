using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class CharacterSelectionManager : MonoBehaviour
{
    public CharactersCreationManager charactersCreationManager;
    public GameObject SelectionMenu;
    public CharacterEditor1 CharacterEditor;
    public GameObject ButtonPrefab;
    public Transform Content;
    public GameObject ConfirmButton;
    public CharactersManager CharactersManager;
    public string CharacterName;
    private bool RootCreated = false;


    public void Start()
    {
        charactersCreationManager = GameObject.Find("Canvas/Popups").GetComponent<CharactersCreationManager>();
        charactersCreationManager.ConfirmButton.onClick.AddListener(() => DisplayCharacters());

        SelectionMenu = GameObject.Find("Canvas/Popups/Selection");
        CharactersManager = GameObject.Find("Scripts/CharactersManager").GetComponent<CharactersManager>();
        CharacterEditor = GameObject.Find("Scripts/CharacterEditor").GetComponent<CharacterEditor1>();
        Content = SelectionMenu.transform.Find("Background/Foreground/Buttons/ScrollView/ViewPort/Content");
        ButtonPrefab = Resources.Load<GameObject>("Buttons/Character");
        ConfirmButton = SelectionMenu.transform.Find("Background/Foreground/Buttons/Confirm").gameObject;
        ConfirmButton.GetComponent<Button>().onClick.AddListener(() => ConfirmRootCharacter());
        CharacterName = "";
    }
    public void DisplayCharacters()
    {
        ToggleOn();
        if(!RootCreated)
            MenuInitialization();
    }
    public void MenuInitialization()
    {
        foreach (string file in Directory.GetFiles(Application.dataPath + "/Resources/Sprites/Characters"))
        {
            if (file.EndsWith(".png"))
            {
                string name = Path.GetFileNameWithoutExtension(file);
                GameObject button = Instantiate(ButtonPrefab, Content);
                button.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/Characters/{name}");
                button.name = name;
                button.GetComponent<Button>().onClick.AddListener(() => MarkCharacter());
            }
        }
    }
    public void MarkCharacter()
    {
        var selectedGameObject = EventSystem.current.currentSelectedGameObject;
        bool isSelected = selectedGameObject.GetComponent<Image>().color == Color.green;
        
        if (isSelected)
        {
            foreach (Transform button in Content)
            {
                button.GetComponent<Image>().color = Color.white;
                button.GetComponent<Button>().interactable = true;
                ConfirmButton.GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            foreach (Transform button in Content)
            {
                button.GetComponent<Image>().color = Color.white;
                button.GetComponent<Button>().interactable = false;
            }
            selectedGameObject.GetComponent<Image>().color = Color.green;
            selectedGameObject.GetComponent<Button>().interactable = true;
            ConfirmButton.GetComponent<Button>().interactable = true;
            CharacterName = selectedGameObject.name;
        }
    }
    public void ConfirmRootCharacter()
    {
        foreach (Transform button in Content)
        {
            if (button.GetComponent<Image>().color == Color.green)
                Destroy(button.gameObject);
            else
            {
                button.GetComponent<Button>().interactable = true;
                button.GetComponent<Image>().color = Color.white;
            }
        }

        string RootDescription = "This is " + CharacterName + ".";

        SpecialAbility RootSpecialAbility;
        if (!RootCreated){
            if (RestrictionManager.Instance.AllowSpecialAbility)
                RootSpecialAbility = GameObject.Find("Canvas/Popups").GetComponent<SpecialAbilityManager>().SpecialAbilitiesCollection.Find(ability => ability.Name == "General").SpecialAbility;
            else 
                RootSpecialAbility = null;
            CharacterB character = new(CharacterName, RootDescription, null, RootSpecialAbility, true, false);
            InitializeCharacterObject(character);
            CharactersManager.AddCharacter(character);
        }

        ToggleOff();
    }
    private void InitializeCharacterObject(CharacterB characterNode)
    {
        Transform parnetTransform = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Tree/Buttons/ScrollView/ViewPort/All").transform;
        GameObject characterPrefab = Resources.Load<GameObject>("Buttons/Character");

        GameObject newPlayerButton = Instantiate(characterPrefab, parnetTransform);
        newPlayerButton.name = characterNode.Name;
        characterNode.CharacterButton.Button = newPlayerButton;
        newPlayerButton.GetComponent<CharacterDetails>().InitializeCharacter(characterNode);

        Button button = newPlayerButton.GetComponent<Button>();
        button.onClick.AddListener(() => CharactersManager.DisplayCharacter(characterNode));

        Image image = newPlayerButton.GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>("Sprites/Characters/" + characterNode.Name);
    }
    public void ToggleOn()
    {
        SelectionMenu.SetActive(true);
        charactersCreationManager.AddButton.gameObject.SetActive(false);
    }
    public void ToggleOff()
    {
        SelectionMenu.SetActive(false);
        charactersCreationManager.AddButton.gameObject.SetActive(true);
        RootCreated = true;
    }
}
