using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CharactersCreationManager : MonoBehaviour
{
    public GameObject Popup;

    public GameObject AddButton;
    public GameObject CancelButton;
    public GameObject ConfirmButton;
    public GameObject ResetButton;

    public int SpriteIndex;
    public List<Sprite> CharacterSprites;
    public GameObject CharacterPrefab;
    public Transform CharacterContentPanel;

    public int CharacterParentsLimit;
    public List<Character> SelectedCharacterParents;
    public List<GameObject> CharacterGameObjects;
    public List<GameObject> DuplicateCharacterGameObjects;

    public Button PopupToggleOn;
    public Button PopupToggleOff;

    public CharactersManager CharactersManager;
    public SpecialAbilitiesManager SpecialAbilitiesManager;

    public List<Button> NotAllowedButtons;

    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/CharacterCreation");

        AddButton = Popup.transform.Find("Buttons/Add").gameObject;
        AddButton.GetComponent<Button>().onClick.AddListener(() => StartFactory());

        CancelButton = Popup.transform.Find("Buttons/Cancel").gameObject;
        CancelButton.GetComponent<Button>().onClick.AddListener(() => CancelFactory());

        ConfirmButton = Popup.transform.Find("Buttons/Confirm").gameObject;
        ConfirmButton.GetComponent<Button>().onClick.AddListener(() => ConfirmFactory());

        ResetButton = Popup.transform.Find("Buttons/Reset").gameObject;
        ResetButton.GetComponent<Button>().onClick.AddListener(() => ResetFactory());

        SpriteIndex = 0;
        CharacterSprites = Resources.LoadAll<Sprite>("Sprites/Characters/").ToList();
        CharacterPrefab = Resources.Load<GameObject>("Buttons/Character");
        CharacterContentPanel = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/ScrollView/ViewPort/All").transform;

        CharacterParentsLimit = RestrictionManager.Instance.AllowMultipleInheritance ? 2 : 1;
        SelectedCharacterParents = new List<Character>();
        CharacterGameObjects = new List<GameObject>();
        DuplicateCharacterGameObjects = new List<GameObject>();

        PopupToggleOn = GameObject.Find("Canvas/GameplayScreen/SwapScreen").GetComponent<Button>();
        PopupToggleOn.onClick.AddListener(() => ToggleOn());

        PopupToggleOff = GameObject.Find("Canvas/HTMenu/Menu/SwapScreen").GetComponent<Button>();
        PopupToggleOff.onClick.AddListener(() => ToggleOff());    

        CharactersManager = GameObject.Find("Player").GetComponent<CharactersManager>();
        SpecialAbilitiesManager = GameObject.Find("Canvas/Popups").GetComponent<SpecialAbilitiesManager>();
        
        NotAllowedButtons = new List<Button>
        {
            GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Attributes/Buttons/Edit").GetComponent<Button>(),
            GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Methods/Buttons/Edit").GetComponent<Button>(),
            CharactersManager.DeleteButton.GetComponent<Button>()
        };
    }

    private void StartFactory()
    {
        AddButton.GetComponent<Button>().interactable = false;
        ConfirmButton.GetComponent<Button>().interactable = false;
        ResetButton.GetComponent<Button>().interactable = false;

        CancelButton.SetActive(true);
        ConfirmButton.SetActive(true);
        ResetButton.SetActive(true);

        SelectedCharacterParents.Clear();
        CharacterGameObjects.Clear();
        DuplicateCharacterGameObjects.Clear();

        BuildCharacterGameObjects();
        ToggleButtonsInteractability(CharacterGameObjects.Select(item => item.GetComponent<Button>()).ToList());
        ToggleButtonsInteractability(NotAllowedButtons);
    }
    private void CancelFactory()
    {
        AddButton.GetComponent<Button>().interactable = true;
        ConfirmButton.GetComponent<Button>().interactable = false;
        ResetButton.GetComponent<Button>().interactable = false;

        CancelButton.SetActive(false);
        ConfirmButton.SetActive(false);
        ResetButton.SetActive(false);

        DestroyGameObjects(DuplicateCharacterGameObjects);
        ToggleButtonsInteractability(CharacterGameObjects.Select(item => item.GetComponent<Button>()).ToList());
        ToggleButtonsInteractability(NotAllowedButtons);
    }
    private void ConfirmFactory()
    {
        AddButton.GetComponent<Button>().interactable = true;
        ConfirmButton.GetComponent<Button>().interactable = false;
        ResetButton.GetComponent<Button>().interactable = false;

        CancelButton.SetActive(false);
        ConfirmButton.SetActive(false);
        ResetButton.SetActive(false);

        DestroyGameObjects(DuplicateCharacterGameObjects);
        ToggleButtonsInteractability(CharacterGameObjects.Select(item => item.GetComponent<Button>()).ToList());
        ToggleButtonsInteractability(NotAllowedButtons);

        StartCoroutine(CharacterBuildPipeline());
    }
    private void ResetFactory()
    {
        ConfirmButton.GetComponent<Button>().interactable = false;
        ResetButton.GetComponent<Button>().interactable = false;

        SelectedCharacterParents.Clear();
        MarkCharacter();
    }

    private void BuildCharacterGameObjects()
    {
        CharacterGameObjects = GameObject.FindGameObjectsWithTag("CharacterButton").ToList();

        foreach (GameObject characterGameObject in CharacterGameObjects)
        {
            GameObject duplicateCharacterGameObject = Instantiate(characterGameObject, characterGameObject.transform.parent);

            CharacterDetails originalCharacterDetails = characterGameObject.GetComponent<CharacterDetails>();
            CharacterDetails duplicateCharacterDetails = duplicateCharacterGameObject.GetComponent<CharacterDetails>();
            duplicateCharacterDetails.InitializeCharacter(originalCharacterDetails.Character);

            Image duplicateCharacterGameObjectImage = duplicateCharacterGameObject.GetComponent<Image>();
            duplicateCharacterGameObjectImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

            Button duplicateCharacterGameObjectButton = duplicateCharacterDetails.GetComponent<Button>();
            duplicateCharacterGameObjectButton.onClick.RemoveAllListeners();
            duplicateCharacterGameObjectButton.onClick.AddListener(() => MarkCharacter());
            duplicateCharacterGameObjectButton.onClick.AddListener(() => CharactersManager.DisplayCharacter(duplicateCharacterDetails.Character));

            DuplicateCharacterGameObjects.Add(duplicateCharacterGameObject);
        }
    }
    private void ToggleButtonsInteractability(List<Button> buttons) { foreach (Button button in buttons) button.interactable = !button.interactable; }
    private void DestroyGameObjects(List<GameObject> gameObjects) { foreach (GameObject gameObject in gameObjects) Destroy(gameObject); }

    private void MarkCharacter()
    {
        var characterGameObject = EventSystem.current.currentSelectedGameObject;
        if (characterGameObject != null)
        {
            Character character = characterGameObject.GetComponent<CharacterDetails>().Character;
            bool isSelectedParentsAlready = SelectedCharacterParents.Contains(character);

            if (isSelectedParentsAlready) SelectedCharacterParents.Remove(character);
            else SelectedCharacterParents.Add(character);

            ConfirmButton.GetComponent<Button>().interactable = SelectedCharacterParents.Count > 0;
            ResetButton.GetComponent<Button>().interactable = SelectedCharacterParents.Count > 0;

            Image image = characterGameObject.GetComponent<Image>();
            image.color = isSelectedParentsAlready ? Color.white : Color.green;

            List<GameObject> currentGameObjectsFilter = DuplicateCharacterGameObjects.FindAll(obj => obj.GetComponent<Button>().interactable == (isSelectedParentsAlready == false));
            List<GameObject> ancestorGameObjectsFilter = SelectedCharacterParents != null && SelectedCharacterParents.Any() ? FindAncestorGameObjects(SelectedCharacterParents.Last()) : new List<GameObject>();
            List<GameObject> nextGameObjectsFilter = isSelectedParentsAlready == false && SelectedCharacterParents.Count == CharacterParentsLimit ? currentGameObjectsFilter : ancestorGameObjectsFilter;
            List<GameObject> finalGameObjects = isSelectedParentsAlready ? currentGameObjectsFilter.Except(nextGameObjectsFilter).ToList() : nextGameObjectsFilter.Except(DuplicateCharacterGameObjects.FindAll(obj => SelectedCharacterParents.Contains(obj.GetComponent<CharacterDetails>().Character))).ToList();                  
            foreach (GameObject gameObject in finalGameObjects)
            {
                Image gameObjectImage = gameObject.GetComponent<Image>();
                gameObjectImage.color = isSelectedParentsAlready ? Color.white : Color.black;

                Button gameObjectButton = gameObject.GetComponent<Button>();
                gameObjectButton.interactable = isSelectedParentsAlready;
            }
        }

        else 
        {
            SelectedCharacterParents.Clear();

            foreach(GameObject duplicateCharacterGameObject in DuplicateCharacterGameObjects)
            {
                duplicateCharacterGameObject.GetComponent<Button>().interactable = true;
                duplicateCharacterGameObject.GetComponent<Image>().color = Color.white;
            }
        }
    }
    private List<GameObject> FindAncestorGameObjects(Character character)
    {
        List<GameObject> AncestorGameObjects = new();

        foreach (Character parent in character.Parents)
        {
            GameObject ancestorGameObject = DuplicateCharacterGameObjects.Find(obj => obj.GetComponent<CharacterDetails>().Character == parent);
            AncestorGameObjects.Add(ancestorGameObject);
            
            List<GameObject> parentAncestors = FindAncestorGameObjects(parent);
            AncestorGameObjects.AddRange(parentAncestors);
        }

        return AncestorGameObjects;
    }

    private IEnumerator CharacterBuildPipeline()
    {
        SpecialAbilitiesManager.ToggleOn();
        yield return new WaitUntil(() => SpecialAbilitiesManager.Popup.activeSelf == false);

        Character builtCharacter = BuildCharacter();
        BuildCharacterObject(builtCharacter);

        CharactersManager.AddCharacter(builtCharacter);
    }
    private Character BuildCharacter()
    {
        int characterIndex = CharacterGameObjects.Count + 1;
        string characterName = $"Character {characterIndex}";
        string characterDescription = $"I`m {characterName}";
        List<Character> characterParents = SelectedCharacterParents;
        SpecialAbility characterSpecialAbility = SpecialAbilitiesManager.SelectedSpecialAbility;
        Character builtCharacter = new(characterName, characterDescription, characterParents, characterSpecialAbility, false, false);
        builtCharacter.Parents.ForEach(parent => parent.Childrens.Add(builtCharacter));
        
        return builtCharacter;
    }
    public void BuildCharacterObject(Character character)
    {
        GameObject characterGameObject = Instantiate(CharacterPrefab, CharacterContentPanel);

        characterGameObject.name = character.Name;
        character.CharacterButton.Button = characterGameObject;
        characterGameObject.GetComponent<CharacterDetails>().InitializeCharacter(character);

        Button button = characterGameObject.GetComponent<Button>();
        button.onClick.AddListener(() => CharactersManager.DisplayCharacter(character));

        Image image = characterGameObject.GetComponent<Image>();
        image.sprite = CharacterSprites[SpriteIndex % CharacterSprites.Count];
        SpriteIndex++;
    }

    public void ToggleOn() { Popup.SetActive(true); }
    public void ToggleOff() { Popup.SetActive(false); }
}
