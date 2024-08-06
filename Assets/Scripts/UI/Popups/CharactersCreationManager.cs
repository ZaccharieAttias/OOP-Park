using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CharactersCreationManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Popup;
    public GameObject SelectionMenu;
    public Transform CharactersContentPanel;

    [Header("Buttons")]
    public GameObject CharacterPrefab;
    public Button AddButton;
    public Button CancelButton;
    public Button ConfirmButton;
    public List<Button> NotAllowedButtons;

    [Header("Character Data")]
    public CharacterB SelectedParent;
    public List<GameObject> CharacterGameObjects;
    public List<GameObject> DuplicateCharacterGameObjects;

    [Header("Managers")]
    public Toggle CharacterToggleType;


    public void Start()
    {
        InitializeUIElements();
        InitializeButtons();
        InitializeNotAllowedButtons();
        InitializeCharacterToggleType();
    }
    public void InitializeUIElements()
    {
        Popup = GameObject.Find("Canvas/Popups/CharacterCreation");
        CharactersContentPanel = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Tree/Buttons/Background/ScrollView/ViewPort/All").transform;
        SelectionMenu = GameObject.Find("Canvas/Popups/Selection");
    }
    public void InitializeButtons()
    {
        CharacterPrefab = Resources.Load<GameObject>("Buttons/Character");

        AddButton = Popup.transform.Find("Buttons/Add").GetComponent<Button>();
        AddButton.onClick.AddListener(StartFactory);

        CancelButton = Popup.transform.Find("Buttons/Cancel").GetComponent<Button>();
        CancelButton.onClick.AddListener(CancelFactory);

        ConfirmButton = Popup.transform.Find("Buttons/Confirm").GetComponent<Button>();
        ConfirmButton.onClick.AddListener(ConfirmFactory);
    }
    public void InitializeNotAllowedButtons()
    {
        NotAllowedButtons = new List<Button>
        {
            GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Attributes/Buttons/Edit").GetComponent<Button>(),
            GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Methods/Buttons/Edit").GetComponent<Button>(),
            GameObject.Find("Canvas/Menus/CharacterCenter/SwapScreen").GetComponent<Button>(),
            GameObject.Find("Scripts/CharactersManager").GetComponent<CharactersManager>().DeleteButton.GetComponent<Button>()
        };
    }
    public void InitializeCharacterToggleType()
    {
        CharacterToggleType = Popup.transform.Find("Buttons/CharacterType/Button").GetComponent<Toggle>();

        if (RestrictionManager.Instance.AllowSingleInheritance)
        {
            Button popupToggleOn = GameObject.Find("Canvas/Menus/Gameplay/SwapScreen").GetComponent<Button>();
            popupToggleOn.onClick.AddListener(ToggleOn);

            Button popupToggleOff = GameObject.Find("Canvas/Menus/CharacterCenter/SwapScreen").GetComponent<Button>();
            popupToggleOff.onClick.AddListener(ToggleOff);
        }

        if (RestrictionManager.Instance.OnlineGame) AddButton.gameObject.SetActive(false);
    }

    public void StartFactory()
    {
        SetControlButtonsState(false, true, false);
        SetActiveControlButtons(true, true, true, RestrictionManager.Instance.AllowAbstractClass);

        CharacterToggleType.isOn = false;

        SelectedParent = null;
        CharacterGameObjects = new List<GameObject>();
        DuplicateCharacterGameObjects = new List<GameObject>();

        BuildCharacterGameObjects();
        ToggleButtonsInteractability(CharacterGameObjects.Select(item => item.GetComponent<Button>()).ToList());
        ToggleButtonsInteractability(NotAllowedButtons);
    }
    public void CancelFactory()
    {
        SetControlButtonsState(true, false, false);
        SetActiveControlButtons(true, false, false, false);

        DestroyGameObjects(DuplicateCharacterGameObjects);
        ToggleButtonsInteractability(CharacterGameObjects.Select(item => item.GetComponent<Button>()).ToList());
        ToggleButtonsInteractability(NotAllowedButtons);
    }
    public void ConfirmFactory()
    {
        SetControlButtonsState(true, false, false);
        SetActiveControlButtons(true, false, false, false);

        DestroyGameObjects(DuplicateCharacterGameObjects);
        ToggleButtonsInteractability(CharacterGameObjects.Select(item => item.GetComponent<Button>()).ToList());
        ToggleButtonsInteractability(NotAllowedButtons);

        StartCoroutine(CharacterBuildPipeline());
    }

    public void MarkCharacter()
    {
        var selectedGameObject = EventSystem.current.currentSelectedGameObject;
        bool isSelectedParent = SelectedParent == null;
        SelectedParent = isSelectedParent ? selectedGameObject.GetComponent<CharacterDetails>().Character : null;

        selectedGameObject.GetComponent<Image>().color = isSelectedParent ? Color.green : Color.white;
        selectedGameObject.GetComponent<Button>().interactable = true;

        ConfirmButton.GetComponent<Button>().interactable = isSelectedParent;

        foreach (GameObject gameObject in DuplicateCharacterGameObjects.Where(item => item != selectedGameObject))
        {
            gameObject.GetComponent<Image>().color = isSelectedParent ? Color.black : Color.white;
            gameObject.GetComponent<Button>().interactable = !isSelectedParent;
        }
    }
    public void SetControlButtonsState(bool addInteractable, bool cancelInteractable, bool confirmInteractable)
    {
        AddButton.interactable = addInteractable;
        CancelButton.interactable = cancelInteractable;
        ConfirmButton.interactable = confirmInteractable;
    }
    public void SetActiveControlButtons(bool addActive, bool cancelActive, bool confirmActive, bool toggleActive)
    {
        AddButton.gameObject.SetActive(addActive);
        CancelButton.gameObject.SetActive(cancelActive);
        ConfirmButton.gameObject.SetActive(confirmActive);
        CharacterToggleType.gameObject.transform.parent.gameObject.SetActive(toggleActive);
    }
    public void ToggleButtonsInteractability(List<Button> buttons)
    {
        foreach (Button button in buttons)
        {
            button.interactable = !button.interactable;
        }
    }

    public void BuildCharacterGameObjects()
    {
        CharacterGameObjects = GameObject.FindGameObjectsWithTag("CharacterButton").ToList();
        foreach (GameObject characterGameObject in CharacterGameObjects)
        {
            GameObject duplicateCharacterGameObject = Instantiate(characterGameObject, characterGameObject.transform.parent);

            CharacterDetails originalCharacterDetails = characterGameObject.GetComponent<CharacterDetails>();
            CharacterDetails duplicateCharacterDetails = duplicateCharacterGameObject.GetComponent<CharacterDetails>();
            duplicateCharacterDetails.InitializeCharacter(originalCharacterDetails.Character);

            Image duplicateCharacterGameObjectImage = duplicateCharacterGameObject.GetComponent<Image>();
            duplicateCharacterGameObjectImage.color = new Color32(255, 255, 255, 255);

            Button duplicateCharacterGameObjectButton = duplicateCharacterDetails.GetComponent<Button>();
            duplicateCharacterGameObjectButton.onClick.RemoveAllListeners();
            duplicateCharacterGameObjectButton.onClick.AddListener(MarkCharacter);
            duplicateCharacterGameObjectButton.onClick.AddListener(() => CharactersData.CharactersManager.DisplayCharacter(duplicateCharacterDetails.Character));

            DuplicateCharacterGameObjects.Add(duplicateCharacterGameObject);
        }
    }
    public void DestroyGameObjects(List<GameObject> gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator CharacterBuildPipeline()
    {
        if (RestrictionManager.Instance.OnlineBuild)
        {
            if (GetComponent<CharacterSelectionManager>().Content.childCount == 0) GetComponent<CharacterSelectionManager>().MenuInitialization();
            GetComponent<CharacterSelectionManager>().DisplayCharacters();

            yield return new WaitUntil(() => SelectionMenu.activeSelf);
            yield return new WaitUntil(() => !SelectionMenu.activeSelf);
        }

        if (RestrictionManager.Instance.AllowSpecialAbility)
        {
            AddButton.gameObject.SetActive(false);
            SpecialAbilitiesData.SpecialAbilityManager.ToggleOn();

            yield return new WaitUntil(() => !SpecialAbilitiesData.SpecialAbilityManager.Popup.activeSelf);

            AddButton.gameObject.SetActive(true);
        }

        var builtCharacter = BuildCharacter();
        if (builtCharacter == null) yield break;

        BuildCharacterObject(builtCharacter);
        CharactersData.CharactersManager.AddCharacter(builtCharacter);
    }
    public CharacterB BuildCharacter()
    {
        if (RestrictionManager.Instance.AllowSpecialAbility && SpecialAbilitiesData.SpecialAbilityManager.SelectedSpecialAbility == null) return null;

        CharacterB builtCharacter = RestrictionManager.Instance.OnlineBuild
            ? new CharacterB
            {
                IsAbstract = CharacterToggleType.isOn,
                Name = GetComponent<CharacterSelectionManager>().CharacterName,
                Description = $"I'm {GetComponent<CharacterSelectionManager>().CharacterName}",
                SpecialAbility = RestrictionManager.Instance.AllowSpecialAbility ? SpecialAbilitiesData.SpecialAbilityManager.SelectedSpecialAbility : null,
                Parent = SelectedParent
            }
            : new CharacterB
            {
                IsAbstract = CharacterToggleType.isOn,
                Name = $"Character {CharacterGameObjects.Count + 1}",
                Description = $"I'm {CharacterGameObjects.Count + 1}",
                SpecialAbility = RestrictionManager.Instance.AllowSpecialAbility ? SpecialAbilitiesData.SpecialAbilityManager.SelectedSpecialAbility : null,
                Parent = SelectedParent
            };

        builtCharacter.Parent?.Childrens.Add(builtCharacter);

        return builtCharacter;
    }
    public void BuildCharacterObject(CharacterB character)
    {
        var characterGameObject = Instantiate(CharacterPrefab, CharactersContentPanel);
        characterGameObject.name = character.Name;
        character.CharacterButton.Button = characterGameObject;
        characterGameObject.GetComponent<CharacterDetails>().InitializeCharacter(character);

        var image = characterGameObject.GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>($"Sprites/Characters/{character.Name}");

        var button = characterGameObject.GetComponent<Button>();
        button.onClick.AddListener(() => CharactersData.CharactersManager.DisplayCharacter(character));
    }
    public void RootCreation()
    {
        GetComponent<CharacterSelectionManager>().DisplayCharacters();
    }

    public void ToggleOn()
    {
        Popup.SetActive(true);
    }
    public void ToggleOff()
    {
        Popup.SetActive(false);
    }
}