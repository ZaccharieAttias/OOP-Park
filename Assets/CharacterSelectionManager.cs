using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;

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
    public bool RootCreated = false;
    public GameObject TreeContent;
    public GameObject Abstract;
    private bool isAbstract;


    public void Start()
    {
        charactersCreationManager = GameObject.Find("Canvas/Popups").GetComponent<CharactersCreationManager>();


        SelectionMenu = GameObject.Find("Canvas/Popups/Selection");
        CharactersManager = GameObject.Find("Scripts/CharactersManager").GetComponent<CharactersManager>();
        CharacterEditor = GameObject.Find("Scripts/CharacterEditor").GetComponent<CharacterEditor1>();
        Content = SelectionMenu.transform.Find("Background/Foreground/Buttons/Background/ScrollView/ViewPort/Content");
        ButtonPrefab = Resources.Load<GameObject>("Buttons/Character");
        ConfirmButton = SelectionMenu.transform.Find("Background/Foreground/Buttons/Confirm").gameObject;
        TreeContent = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Tree/Buttons/Background/ScrollView/ViewPort/All");
        ConfirmButton.GetComponent<Button>().onClick.AddListener(() => ConfirmRootCharacter());
        ConfirmButton.GetComponent<Button>().onClick.AddListener(() => ConfirmButton.GetComponent<Button>().interactable = false);
        
        CharacterName = "";
    }
    public void DisplayCharacters()
    {
        ToggleOn();
        if(!RootCreated)
        {
            MenuInitialization();
            if(RestrictionManager.Instance.AllowAbstractClass)
                Abstract.gameObject.SetActive(true);
        }
        else Abstract.gameObject.SetActive(false);
            
    }
    public void MenuInitialization()
    {
        foreach (string file in Directory.GetFiles(Application.dataPath + "/Resources/Sprites/Characters"))
        {
            if (file.EndsWith(".png"))
            {
                string name = Path.GetFileNameWithoutExtension(file);
                if (TreeContent.transform.Find(name) != null)
                    continue;
                GameObject button = Instantiate(ButtonPrefab, Content);
                button.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/Characters/{name}");
                button.name = name;
                button.GetComponent<Button>().onClick.AddListener(() => MarkCharacter());
            }
        }
        if (CharactersData.CharactersManager.CurrentCharacter != null)
            RootCreated = true;
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
            if (RestrictionManager.Instance.AllowAbstractClass)
                isAbstract = Abstract.transform.Find("Button").GetComponent<Toggle>().isOn;
            else
                isAbstract = false;
            CharacterB character = new(CharacterName, RootDescription, null, RootSpecialAbility, true, isAbstract);
            InitializeCharacterObject(character);
            CharactersManager.AddCharacter(character);
            CharactersManager.CurrentCharacter = character;
            CharactersManager.DisplayCharacter(character);
        }

        ToggleOff();
    }
    private void InitializeCharacterObject(CharacterB characterNode)
    {
        Transform parentTransform = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Tree/Buttons/Background/ScrollView/ViewPort/All").transform;
        GameObject characterPrefab = Resources.Load<GameObject>("Buttons/Character");

        GameObject newPlayerButton = Instantiate(characterPrefab, parentTransform);
        newPlayerButton.name = characterNode.Name;
        characterNode.CharacterButton.Button = newPlayerButton;
        newPlayerButton.GetComponent<CharacterDetails>().InitializeCharacter(characterNode);

        Button button = newPlayerButton.GetComponent<Button>();
        button.onClick.AddListener(() => CharactersManager.DisplayCharacter(characterNode));

        Image image = newPlayerButton.GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>("Sprites/Characters/" + characterNode.Name);
    }
    public void CleanContent()
    {
        foreach (Transform button in Content)
            Destroy(button.gameObject);
    }
    public void ToggleOn()
    {
        SelectionMenu.SetActive(true);
        charactersCreationManager.AddButton.gameObject.SetActive(false);
        if (!RootCreated)
            GameObject.Find("Canvas/Menus/CharacterCenter/SwapScreen").GetComponent<Button>().interactable = false;
    }
    public void ToggleOff()
    {
        SelectionMenu.SetActive(false);
        charactersCreationManager.AddButton.gameObject.SetActive(true);
        if (!RootCreated)
        {
            if (!CharactersData.CharactersManager.CurrentCharacter.IsAbstract)
                GameObject.Find("Canvas/Menus/CharacterCenter/SwapScreen").GetComponent<Button>().interactable = true;
        }
        RootCreated = true;
    }
}
