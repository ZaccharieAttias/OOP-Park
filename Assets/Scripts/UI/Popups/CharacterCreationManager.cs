using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CharacterCreationManager : MonoBehaviour
{
    public GameObject Popup;

    public GameObject AddButton;
    public GameObject CancelButton;
    public GameObject ConfirmButton;
    public GameObject ResetButton;

    public List<GameObject> CharacterObjects;
    public List<GameObject> DuplicateCharacterObjects;

    public int ParentsLimit;
    public List<Character> SelectedCharacterObjects;
    public CharacterManager CharacterManager;
    public List<Button> Buttons;



    public GameObject CharacterPrefab;
    public Transform CharacterParent;
    public List<Sprite> CharacterSprites;
    public int SpriteIndex;

    public SpecialAbilityManager SpecialAbilityManager;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/CharacterCreation");

        AddButton = Popup.transform.Find("Buttons/Add").gameObject;
        AddButton.GetComponent<Button>().onClick.AddListener(() => InitializeFactory());

        CancelButton = Popup.transform.Find("Buttons/Cancel").gameObject;
        CancelButton.SetActive(false);
        CancelButton.GetComponent<Button>().onClick.AddListener(() => CancelFactory());

        ConfirmButton = Popup.transform.Find("Buttons/Confirm").gameObject;
        ConfirmButton.SetActive(false);
        ConfirmButton.GetComponent<Button>().onClick.AddListener(() => ConfirmFactory());

        ResetButton = Popup.transform.Find("Buttons/Reset").gameObject;
        ResetButton.SetActive(false);
        ResetButton.GetComponent<Button>().onClick.AddListener(() => ResetFactory());


        CharacterObjects = new List<GameObject>();
        DuplicateCharacterObjects = new List<GameObject>();
        
        SelectedCharacterObjects = new List<Character>();
        ParentsLimit = RestrictionManager.Instance.AllowMultipleInheritance ? 2 : 1;

        CharacterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
        Buttons = new List<Button>
        {
            GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Attributes/Buttons/Edit").GetComponent<Button>(),
            GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Methods/Buttons/Edit").GetComponent<Button>(),
            CharacterManager.DeleteButton.GetComponent<Button>()
        };

        CharacterPrefab = Resources.Load<GameObject>("Buttons/Character");
        CharacterParent = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/ScrollView/ViewPort/All").transform;
        CharacterSprites = Resources.LoadAll<Sprite>("Sprites/Characters/").ToList();
        SpriteIndex = 0;

        SpecialAbilityManager = GameObject.Find("Canvas/Popups").GetComponent<SpecialAbilityManager>();

        if (RestrictionManager.Instance.AllowInheritance)
        {
            Button GameplayScreenButton = GameObject.Find("Canvas/GameplayScreen/SwapScreen").GetComponent<Button>();
            GameplayScreenButton.onClick.AddListener(() => ToggleOn());

            Button MenuScreenButton = GameObject.Find("Canvas/HTMenu/Menu/SwapScreen").GetComponent<Button>();
            MenuScreenButton.onClick.AddListener(() => ToggleOff());
        }
    }

    public void InitializeFactory()
    {
        AddButton.GetComponent<Button>().interactable = false;
        ConfirmButton.GetComponent<Button>().interactable = false;
        ResetButton.GetComponent<Button>().interactable = false;

        CancelButton.SetActive(true);
        ConfirmButton.SetActive(true);
        ResetButton.SetActive(true);

        CharacterObjects = GameObject.FindGameObjectsWithTag("CharacterButton").ToList();
        BuildDuplicateCharacterObjects();
        ToggleButtonInteractability(CharacterObjects);

        SelectedCharacterObjects.Clear();
    }
    public void CancelFactory()
    {
        AddButton.GetComponent<Button>().interactable = true;
        ConfirmButton.GetComponent<Button>().interactable = false;
        ResetButton.GetComponent<Button>().interactable = false;

        CancelButton.SetActive(false);
        ConfirmButton.SetActive(false);
        ResetButton.SetActive(false);

        DestroyObjectsList(DuplicateCharacterObjects);
        ToggleButtonInteractability(CharacterObjects);

        CharacterObjects.Clear();
        DuplicateCharacterObjects.Clear();
        SelectedCharacterObjects.Clear();
    }
    public void ConfirmFactory()
    {
        AddButton.GetComponent<Button>().interactable = true;
        ConfirmButton.GetComponent<Button>().interactable = false;
        ResetButton.GetComponent<Button>().interactable = false;

        CancelButton.SetActive(false);
        ConfirmButton.SetActive(false);
        ResetButton.SetActive(false);

        StartCoroutine(CharacterBuildPipeline());

        DestroyObjectsList(DuplicateCharacterObjects);
        ToggleButtonInteractability(CharacterObjects);

        CharacterObjects.Clear();
        DuplicateCharacterObjects.Clear();
    }
    public void ResetFactory()
    {
        ConfirmButton.GetComponent<Button>().interactable = false;
        ResetButton.GetComponent<Button>().interactable = false;

        SelectedCharacterObjects.Clear();

        UpdatePanelUI();
    }

    private void CharacterObjectClicked()
    {
        GameObject characterObject = EventSystem.current.currentSelectedGameObject;
        Character character = characterObject.GetComponent<CharacterDetails>().Character;

        if (SelectedCharacterObjects.Contains(character)) SelectedCharacterObjects.Remove(character);
        else SelectedCharacterObjects.Add(character);

        UpdatePanelUI();
    }
    private void UpdatePanelUI()
    {
        bool isSelectedParents = SelectedCharacterObjects.Count > 0;
        ConfirmButton.GetComponent<Button>().interactable = isSelectedParents;
        ResetButton.GetComponent<Button>().interactable = isSelectedParents;

        foreach (GameObject characterObject in DuplicateCharacterObjects)
        {
            Character currentCharacter = characterObject.GetComponent<CharacterDetails>().Character;

            bool isSelected = SelectedCharacterObjects.Contains(currentCharacter);
            bool isParentLimitExceeded = SelectedCharacterObjects.Count == ParentsLimit;

            characterObject.GetComponent<Button>().interactable = isSelected || isParentLimitExceeded == false;
            characterObject.GetComponent<Image>().color = isSelected ? Color.green : isParentLimitExceeded ? Color.black : Color.white;
        }

        foreach (Character character in SelectedCharacterObjects)
        {
            foreach (Character parent in character.Parents)
            {
                GameObject parentObject = DuplicateCharacterObjects.Find(obj => obj.GetComponent<CharacterDetails>().Character == parent);
                parentObject.GetComponent<Button>().interactable = false;
                parentObject.GetComponent<Image>().color = Color.black;
            }

            foreach (Character child in character.Childrens)
            {
                GameObject childObject = DuplicateCharacterObjects.Find(obj => obj.GetComponent<CharacterDetails>().Character == child);
                childObject.GetComponent<Button>().interactable = false;
                childObject.GetComponent<Image>().color = Color.black;
            }
        }
    }

    private void BuildDuplicateCharacterObjects()
    {
        DuplicateCharacterObjects.Clear();

        foreach (GameObject characterObject in CharacterObjects)
        {
            GameObject duplicateCharacterObject = Instantiate(characterObject, characterObject.transform.parent);

            CharacterDetails originalDetails = characterObject.GetComponent<CharacterDetails>();
            CharacterDetails duplicateDetails = duplicateCharacterObject.GetComponent<CharacterDetails>();
            duplicateDetails.InitializeCharacter(originalDetails.Character);

            Button duplicateCharacterObjectButton = duplicateCharacterObject.GetComponent<Button>();
            duplicateCharacterObjectButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

            duplicateCharacterObjectButton.onClick.RemoveAllListeners();
            duplicateCharacterObjectButton.onClick.AddListener(() => CharacterObjectClicked());
            duplicateCharacterObjectButton.onClick.AddListener(() => CharacterManager.DisplayCharacter(duplicateDetails.Character));

            DuplicateCharacterObjects.Add(duplicateCharacterObject);
        }
    }
    private void ToggleButtonInteractability(List<GameObject> gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            Button button = gameObject.GetComponent<Button>();
            button.interactable = !button.interactable;
        }
        foreach (Button button in Buttons)
        {
            button.interactable = !button.interactable;
        }
    }
    private void DestroyObjectsList(List<GameObject> gameObjects) { foreach (GameObject gameObject in gameObjects) Destroy(gameObject); }

    private IEnumerator CharacterBuildPipeline()
    {
        SpecialAbilityManager.ToggleOn();
        yield return new WaitUntil(() => SpecialAbilityManager.Popup.activeSelf == false);

        Character builtCharacter = BuildCharacter();
        BuildCharacterObject(builtCharacter);

        CharacterManager.AddCharacter(builtCharacter);
    }
    private Character BuildCharacter()
    {
        int characterIndex = CharacterObjects.Count + 1;
        string characterName = $"Character {characterIndex}";
        string characterDescription = $"I`m {characterName}";
        List<Character> characterParents = SelectedCharacterObjects;
        CharacterSpecialAbility characterSpecialAbility = SpecialAbilityManager.SelectedSpecialAbility;
        Character builtCharacter = new(characterName, characterDescription, characterParents, characterSpecialAbility, false);
        builtCharacter.Parents.ForEach(parent => parent.Childrens.Add(builtCharacter));
        
        return builtCharacter;
    }
    private void BuildCharacterObject(Character character)
    {
        GameObject characterObject = Instantiate(CharacterPrefab, CharacterParent);

        characterObject.name = character.Name;
        character.CharacterButton.Button = characterObject;
        characterObject.GetComponent<CharacterDetails>().InitializeCharacter(character);

        Button button = characterObject.GetComponent<Button>();
        button.onClick.AddListener(() => CharacterManager.DisplayCharacter(character));

        Image image = characterObject.GetComponent<Image>();
        image.sprite = CharacterSprites[SpriteIndex % CharacterSprites.Count];
        SpriteIndex++;
    }

    public void ToggleOn() { Popup.SetActive(true); }
    public void ToggleOff() { Popup.SetActive(false); }
}
