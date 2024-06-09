using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CharactersCreationManager : MonoBehaviour
{
    public GameObject Popup;

    public int SpriteIndex;
    public List<Sprite> CharacterSprites;
    public GameObject CharacterPrefab;
    public Transform CharactersContentPanel;

    public int CharacterParentsLimit;
    public List<CharacterB> SelectedCharacterParents;
    public List<GameObject> CharacterGameObjects;
    public List<GameObject> DuplicateCharacterGameObjects;

    public List<Button> ControlButtons;
    public List<Button> NotAllowedButtons;

    public SpecialAbilitiesManager SpecialAbilitiesManager;
    public Toggle CharacterToggleType;
    public SpecialAbilityManager SpecialAbilityManager;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/CharacterCreation");

        SpriteIndex = 0;
        CharacterSprites = Resources.LoadAll<Sprite>("Sprites/Characters/").ToList();
        CharacterPrefab = Resources.Load<GameObject>("Buttons/Character");
        CharactersContentPanel = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Tree/Buttons/ScrollView/ViewPort/All").transform;

        CharacterParentsLimit = RestrictionManager.Instance.AllowMultipleInheritance ? 2 : 1;
        SelectedCharacterParents = new List<CharacterB>();
        CharacterGameObjects = new List<GameObject>();
        DuplicateCharacterGameObjects = new List<GameObject>();
        
        ControlButtons = new List<Button>
        {
            Popup.transform.Find("Buttons/Add").gameObject.GetComponent<Button>(),
            Popup.transform.Find("Buttons/Cancel").gameObject.GetComponent<Button>(),
            Popup.transform.Find("Buttons/Confirm").gameObject.GetComponent<Button>(),
            Popup.transform.Find("Buttons/Reset").gameObject.GetComponent<Button>()
        };
        ControlButtons[0].GetComponent<Button>().onClick.AddListener(() => StartFactory());
        ControlButtons[1].GetComponent<Button>().onClick.AddListener(() => CancelFactory());
        ControlButtons[2].GetComponent<Button>().onClick.AddListener(() => ConfirmFactory());
        ControlButtons[3].GetComponent<Button>().onClick.AddListener(() => ResetFactory());
        
        NotAllowedButtons = new List<Button>
        {
            GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Attributes/Buttons/Edit").GetComponent<Button>(),
            GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Methods/Buttons/Edit").GetComponent<Button>(),
            GameObject.Find("Canvas/Menus/CharacterCenter/SwapScreen").GetComponent<Button>(),
            GameObject.Find("Player").GetComponent<CharactersManager>().DeleteButton.GetComponent<Button>()
        };

        SpecialAbilitiesManager = GameObject.Find("Canvas/Popups").GetComponent<SpecialAbilitiesManager>();
        SpecialAbilityManager = GameObject.Find("Canvas/Popups").GetComponent<SpecialAbilityManager>();
        CharacterToggleType = Popup.transform.Find("Buttons/CharacterType").gameObject.GetComponent<Toggle>();

        if (RestrictionManager.Instance.AllowSingleInheritance || RestrictionManager.Instance.AllowMultipleInheritance)
        {
            Button popupToggleOn = GameObject.Find("Canvas/Menus/Gameplay/SwapScreen").GetComponent<Button>();
            popupToggleOn.onClick.AddListener(() => ToggleOn());

            Button popupToggleOff = GameObject.Find("Canvas/Menus/CharacterCenter/SwapScreen").GetComponent<Button>();
            popupToggleOff.onClick.AddListener(() => ToggleOff());
        }
        
    }
    private void StartFactory()
    {
        ControlButtons[0].interactable = false;
        ControlButtons[2].interactable = false;
        ControlButtons[3].interactable = false;
        CharacterToggleType.isOn = false;

        ControlButtons[1].gameObject.SetActive(true);
        ControlButtons[2].gameObject.SetActive(true);
        ControlButtons[3].gameObject.SetActive(true);
        if (RestrictionManager.Instance.AllowAbstractClasses) CharacterToggleType.gameObject.SetActive(true);

        SelectedCharacterParents = new();
        CharacterGameObjects = new();
        DuplicateCharacterGameObjects = new();

        BuildCharacterGameObjects();
        ToggleButtonsInteractability(CharacterGameObjects.Select(item => item.GetComponent<Button>()).ToList());
        ToggleButtonsInteractability(NotAllowedButtons);
    }
    private void CancelFactory()
    {
        ControlButtons[0].interactable = true;
        ControlButtons[2].interactable = false;
        ControlButtons[3].interactable = false;

        ControlButtons[1].gameObject.SetActive(false);
        ControlButtons[2].gameObject.SetActive(false);
        ControlButtons[3].gameObject.SetActive(false);
        CharacterToggleType.gameObject.SetActive(false);

        DestroyGameObjects(DuplicateCharacterGameObjects);
        ToggleButtonsInteractability(CharacterGameObjects.Select(item => item.GetComponent<Button>()).ToList());
        ToggleButtonsInteractability(NotAllowedButtons);
    }
    private void ConfirmFactory()
    {
        ControlButtons[0].interactable = true;
        ControlButtons[2].interactable = false;
        ControlButtons[3].interactable = false;

        ControlButtons[1].gameObject.SetActive(false);
        ControlButtons[2].gameObject.SetActive(false);
        ControlButtons[3].gameObject.SetActive(false);
        CharacterToggleType.gameObject.SetActive(false);

        DestroyGameObjects(DuplicateCharacterGameObjects);
        ToggleButtonsInteractability(CharacterGameObjects.Select(item => item.GetComponent<Button>()).ToList());
        ToggleButtonsInteractability(NotAllowedButtons);

        StartCoroutine(CharacterBuildPipeline());
    }
    private void ResetFactory()
    {
        ControlButtons[2].GetComponent<Button>().interactable = false;
        ControlButtons[3].GetComponent<Button>().interactable = false;
        CharacterToggleType.isOn = false;

        SelectedCharacterParents = new();
        MarkCharacter();
    }
    private void ToggleButtonsInteractability(List<Button> buttons) { foreach (Button button in buttons) button.interactable = !button.interactable; }
    private void DestroyGameObjects(List<GameObject> gameObjects) { foreach (GameObject gameObject in gameObjects) Destroy(gameObject); }
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
            duplicateCharacterGameObjectButton.onClick.AddListener(() => CharactersData.CharactersManager.DisplayCharacter(duplicateCharacterDetails.Character));

            DuplicateCharacterGameObjects.Add(duplicateCharacterGameObject);
        }
    }
    private void MarkCharacter()
    {
        var characterGameObject = EventSystem.current.currentSelectedGameObject;
        if (characterGameObject != null)
        {
            CharacterB character = characterGameObject.GetComponent<CharacterDetails>().Character;
            bool isSelectedParentsAlready = SelectedCharacterParents.Contains(character);

            if (isSelectedParentsAlready) SelectedCharacterParents.Remove(character);
            else SelectedCharacterParents.Add(character);

            ControlButtons[2].GetComponent<Button>().interactable = SelectedCharacterParents.Count > 0;
            ControlButtons[3].GetComponent<Button>().interactable = SelectedCharacterParents.Count > 0;

            Image image = characterGameObject.GetComponent<Image>();
            image.color = isSelectedParentsAlready ? Color.white : Color.green;

            List<GameObject> currentGameObjectsFilter = DuplicateCharacterGameObjects.FindAll(obj => obj.GetComponent<Button>().interactable == (isSelectedParentsAlready is false));
            List<GameObject> ancestorGameObjectsFilter = SelectedCharacterParents is not null && SelectedCharacterParents.Any() ? FindAncestorGameObjects(SelectedCharacterParents.Last()) : new List<GameObject>();
            List<GameObject> nextGameObjectsFilter = isSelectedParentsAlready is false && SelectedCharacterParents.Count == CharacterParentsLimit ? currentGameObjectsFilter : ancestorGameObjectsFilter;
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
            SelectedCharacterParents = new();

            foreach(GameObject duplicateCharacterGameObject in DuplicateCharacterGameObjects)
            {
                duplicateCharacterGameObject.GetComponent<Button>().interactable = true;
                duplicateCharacterGameObject.GetComponent<Image>().color = Color.white;
            }
        }
    }
    private List<GameObject> FindAncestorGameObjects(CharacterB character)
    {
        List<GameObject> AncestorGameObjects = new();
        foreach (CharacterB parent in character.Parents)
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
        if (RestrictionManager.Instance.AllowSpecialAbilities)
        {
            SpecialAbilityManager.ToggleOn(SelectedCharacterParents);
            // SpecialAbilitiesManager.ToggleOn(SelectedCharacterParents);
            yield return new WaitUntil(() => SpecialAbilityManager.Popup.activeSelf is false);
        }

        CharacterB builtCharacter = BuildCharacter();
        BuildCharacterObject(builtCharacter);

        CharactersData.CharactersManager.AddCharacter(builtCharacter);
    }
    private CharacterB BuildCharacter()
    {
        SpecialAbility SpecialAbility = SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbilityType.General].First();
        if(RestrictionManager.Instance.AllowSpecialAbilities)
            SpecialAbility = SpecialAbilityManager.SelectedSpecialAbility;

        CharacterB builtCharacter = new()
        {
            IsAbstract = CharacterToggleType.isOn,

            Name = $"Character {CharacterGameObjects.Count + 1}",
            Description = $"I`m {CharacterGameObjects.Count + 1}",
            
            SpecialAbility = SpecialAbility,

            Parents = SelectedCharacterParents
        };
        builtCharacter.Parents.ForEach(parent => parent.Childrens.Add(builtCharacter));
        if (RestrictionManager.Instance.AllowBeginnerInheritance) builtCharacter.PreDetails();
        
        return builtCharacter;
    }
    public void BuildCharacterObject(CharacterB character)
    {
        GameObject characterGameObject = Instantiate(CharacterPrefab, CharactersContentPanel);

        characterGameObject.name = character.Name;
        character.CharacterButton.Button = characterGameObject;
        characterGameObject.GetComponent<CharacterDetails>().InitializeCharacter(character);

        Image image = characterGameObject.GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>("Sprites/Characters/" + character.Name);
        SpriteIndex++;

        Button button = characterGameObject.GetComponent<Button>();
        button.onClick.AddListener(() => CharactersData.CharactersManager.DisplayCharacter(character));
    }
    public void ToggleOn() { Popup.SetActive(true); }
    public void ToggleOff() { Popup.SetActive(false); }
}
