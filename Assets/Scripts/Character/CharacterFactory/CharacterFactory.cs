using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CharacterFactory : MonoBehaviour
{
    public GameObject CharacterObjectPrefab { get; set; }

    public GameObject AddButton { get; set; }
    public GameObject ConfirmButton { get; set; }
    public GameObject CancelButton { get; set; }

    public List<GameObject> CharacterObjects { get; set; }
    public List<GameObject> DuplicateCharacterObjects { get; set; }
    public List<Character> SelectedCharacterObjects { get; set; }

    public CharacterManager CharacterManager { get; set; }

    public int ParentsLimit { get; set; }


    public void Start() 
    { 
        InitializeGameObject();
        InitializeProperties(); 
    }

    private void InitializeGameObject()
    {
        name = "CharacterFactory";

        Transform parentTransform = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons").transform;
        transform.SetParent(parentTransform);

        transform.localPosition = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(1, 1, 1);
    }

    private void InitializeProperties()
    {
        CharacterObjectPrefab = Resources.Load<GameObject>("Prefabs/Buttons/Character");

        AddButton = Instantiate(Resources.Load<GameObject>("Prefabs/Buttons/Button"));
        AddButton.AddComponent<AddFactory>();

        ConfirmButton = Instantiate(Resources.Load<GameObject>("Prefabs/Buttons/Button"));
        ConfirmButton.AddComponent<ConfirmFactory>();

        CancelButton = Instantiate(Resources.Load<GameObject>("Prefabs/Buttons/Button"));
        CancelButton.AddComponent<CancelFactory>();

        CharacterObjects = new List<GameObject>();
        DuplicateCharacterObjects = new List<GameObject>();
        SelectedCharacterObjects = new List<Character>();

        CharacterManager = GameObject.Find("Player").GetComponent<CharacterManager>();

        ParentsLimit = RestrictionManager.Instance.AllowMultipleInheritance ? 5 : 1;
    }

    public void InitializeFactory()
    {
        AddButton.GetComponent<Button>().interactable = false;
        ConfirmButton.SetActive(true);
        CancelButton.SetActive(true);

        CharacterObjects = GameObject.FindGameObjectsWithTag("CharacterButton").ToList();
        BuildDuplicateCharacterObjects();
        ToggleButtonInteractability(CharacterObjects);
    }

    public void ConfirmFactory()
    {
        ConfirmButton.SetActive(false);
        CancelButton.SetActive(false);

        AddButton.GetComponent<Button>().interactable = true;
        ConfirmButton.GetComponent<Button>().interactable = false;
        CancelButton.GetComponent<Button>().interactable = false;

        CharacterManager.AddCharacter(SelectedCharacterObjects);

        DestroyObjectsList(DuplicateCharacterObjects);
        ToggleButtonInteractability(CharacterObjects);

        CharacterObjects.Clear();
        DuplicateCharacterObjects.Clear();
        SelectedCharacterObjects.Clear();
    }

    public void CancelFactory()
    {
        ConfirmButton.GetComponent<Button>().interactable = false;
        CancelButton.GetComponent<Button>().interactable = false;

        SelectedCharacterObjects.Clear();

        UpdateCharacterObjectsUI();
    }

    private void CharacterObjectClicked()
    {
        GameObject characterObject = EventSystem.current.currentSelectedGameObject;
        Character character = characterObject.GetComponent<CharacterDetails>().GetCurrentCharacter();

        if (SelectedCharacterObjects.Contains(character)) SelectedCharacterObjects.Remove(character);
        else SelectedCharacterObjects.Add(character);

        if (SelectedCharacterObjects.Count > 0)
        {
            ConfirmButton.GetComponent<Button>().interactable = true;
            CancelButton.GetComponent<Button>().interactable = true;
        }

        else
        {
            ConfirmButton.GetComponent<Button>().interactable = false;
            CancelButton.GetComponent<Button>().interactable = false;
        }

        UpdateCharacterObjectsUI();
    }

    private void UpdateCharacterObjectsUI()
    {
        foreach (GameObject characterObject in DuplicateCharacterObjects)
        {
            Character currentCharacter = characterObject.GetComponent<CharacterDetails>().GetCurrentCharacter();

            bool isSelected = SelectedCharacterObjects.Contains(currentCharacter);
            bool isParentLimitExceeded = SelectedCharacterObjects.Count == ParentsLimit;

            characterObject.GetComponent<Button>().interactable = isSelected || isParentLimitExceeded == false;
            characterObject.GetComponent<Image>().color = isSelected ? Color.green : isParentLimitExceeded ? Color.black : Color.white;
        }

        foreach (Character character in SelectedCharacterObjects)
        {
            foreach (Character parent in character.parents)
            {
                GameObject parentObject = DuplicateCharacterObjects.Find(obj => obj.GetComponent<CharacterDetails>().GetCurrentCharacter().name == parent.name);
                parentObject.GetComponent<Button>().interactable = false;
                parentObject.GetComponent<Image>().color = Color.black;
            }

            foreach (Character child in character.childrens)
            {
                GameObject childObject = DuplicateCharacterObjects.Find(obj => obj.GetComponent<CharacterDetails>().GetCurrentCharacter().name == child.name);
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

            duplicateCharacterObject.GetComponent<RectTransform>().sizeDelta = characterObject.GetComponent<RectTransform>().sizeDelta;
            duplicateCharacterObject.transform.localScale = new Vector3(1, 1, 1);

            Button duplicateCharacterObjectButton = duplicateCharacterObject.GetComponent<Button>();
            duplicateCharacterObjectButton.onClick.RemoveAllListeners();
            duplicateCharacterObjectButton.onClick.AddListener(() => CharacterObjectClicked());

            DuplicateCharacterObjects.Add(duplicateCharacterObject);
        }
    }

    private void DestroyObjectsList(List<GameObject> gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
            Destroy(gameObject);
    }

    private void ToggleButtonInteractability(List<GameObject> gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            Button button = gameObject.GetComponent<Button>();
            button.interactable = !button.interactable;
        }
    }
}
