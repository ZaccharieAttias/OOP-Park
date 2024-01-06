using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CharacterFactory : MonoBehaviour
{
    public ExecuteFactory ExecuteFactory { get; set; }

    public GameObject AddButton { get; set; }
    public GameObject CancelButton { get; set; }
    public GameObject ConfirmButton { get; set; }

    public List<GameObject> CharacterObjects { get; set; }
    public List<GameObject> DuplicateCharacterObjects { get; set; }
    public List<Character> SelectedCharacterObjects { get; set; }

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
        GameObject pureButtonPrefab = Resources.Load<GameObject>("Prefabs/Buttons/Pure");

        AddButton = Instantiate(pureButtonPrefab, transform.parent);
        AddButton.AddComponent<AddFactory>();

        CancelButton = Instantiate(pureButtonPrefab, transform.parent);
        CancelButton.AddComponent<CancelFactory>();

        ConfirmButton = Instantiate(pureButtonPrefab, transform.parent);
        ConfirmButton.AddComponent<ConfirmFactory>();

        GameObject executeFactoryObject = new GameObject("ExecuteFactory", typeof(ExecuteFactory));
        ExecuteFactory = executeFactoryObject.GetComponent<ExecuteFactory>();

        CharacterObjects = new List<GameObject>();
        DuplicateCharacterObjects = new List<GameObject>();
        SelectedCharacterObjects = new List<Character>();

        ParentsLimit = RestrictionManager.Instance.AllowMultipleInheritance ? 5 : 1;
    }

    public void InitializeFactory()
    {
        AddButton.GetComponent<Button>().interactable = false;
        CancelButton.SetActive(true);
        ConfirmButton.SetActive(true);

        CharacterObjects = GameObject.FindGameObjectsWithTag("CharacterButton").ToList();
        BuildDuplicateCharacterObjects();
        ToggleButtonInteractability(CharacterObjects);
    }

    public void CancelFactory()
    {
        CancelButton.GetComponent<Button>().interactable = false;
        ConfirmButton.GetComponent<Button>().interactable = false;

        SelectedCharacterObjects.Clear();

        UpdatePanelUI();
    }

    public void ConfirmFactory()
    {
        CancelButton.SetActive(false);
        ConfirmButton.SetActive(false);

        AddButton.GetComponent<Button>().interactable = true;
        CancelButton.GetComponent<Button>().interactable = false;
        ConfirmButton.GetComponent<Button>().interactable = false;

        ExecuteFactory.Execute();

        DestroyObjectsList(DuplicateCharacterObjects);
        ToggleButtonInteractability(CharacterObjects);

        CharacterObjects.Clear();
        DuplicateCharacterObjects.Clear();
        SelectedCharacterObjects.Clear();
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
        bool isSelectedEmpty = SelectedCharacterObjects.Count > 0;
        CancelButton.GetComponent<Button>().interactable = isSelectedEmpty;
        ConfirmButton.GetComponent<Button>().interactable = isSelectedEmpty;

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
            foreach (Character parent in character.parents)
            {
                GameObject parentObject = DuplicateCharacterObjects.Find(obj => obj.GetComponent<CharacterDetails>().Character.name == parent.name);
                parentObject.GetComponent<Button>().interactable = false;
                parentObject.GetComponent<Image>().color = Color.black;
            }

            foreach (Character child in character.childrens)
            {
                GameObject childObject = DuplicateCharacterObjects.Find(obj => obj.GetComponent<CharacterDetails>().Character.name == child.name);
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
