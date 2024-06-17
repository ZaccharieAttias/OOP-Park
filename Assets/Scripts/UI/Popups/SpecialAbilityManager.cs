using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class SpecialAbilityManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Popup;
    public GameObject SpecialAbilityButton;
    public Transform SpecialAbilityContentPanel;
    public Button CancelButton;
    public Button ConfirmButton;

    [Header("Managers")]
    public CharactersCreationManager CharacterCreationManager;
    public SpecialAbilityTreeManager SpecialAbilityTreeManager;

    [Header("Special Abilities")]
    public SpecialAbility SelectedSpecialAbility;
    public List<SpecialAbilityObject> SpecialAbilitiesCollection;

    [Header("Game Objects")]
    public List<GameObject> SpecialAbilityGameObjects;


    public void Start()
    {
        InitializeProperties();
    }
    public void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/SpecialAbilityTree");
        SpecialAbilityButton = Resources.Load<GameObject>("Buttons/Ability");
        SpecialAbilityContentPanel = GameObject.Find("Canvas/Popups/SpecialAbilityTree/Buttons/ScrollView/ViewPort/All").transform;
        CancelButton = Popup.transform.Find("Buttons/Cancel").gameObject.GetComponent<Button>();
        CancelButton.onClick.AddListener(() => CancelFactory());
        ConfirmButton = Popup.transform.Find("Buttons/Confirm").gameObject.GetComponent<Button>();
        ConfirmButton.onClick.AddListener(() => ConfirmFactory());

        CharacterCreationManager = GameObject.Find("Canvas/Popups").GetComponent<CharactersCreationManager>();
        SpecialAbilityTreeManager = GameObject.Find("Canvas/Popups").GetComponent<SpecialAbilityTreeManager>();

        SelectedSpecialAbility = null;
        SpecialAbilitiesCollection = new();

        SpecialAbilityGameObjects = new();
    }

    public void StartFactory()
    {
        CancelButton.interactable = true;
        ConfirmButton.interactable = false;

        SelectedSpecialAbility = null;
        if (SpecialAbilityGameObjects.Count == 0)
        {
            BuildSpecialAbilityGameObjects();
        }

        ResetAbilitySelection();
        SpecialAbilityTreeManager.BuildTree(SpecialAbilitiesCollection.First());
    }
    public void CancelFactory()
    {
        SelectedSpecialAbility = null;

        ToggleOff();
    }
    public void ConfirmFactory()
    {
        ToggleOff();
    }

    public void MarkAbility(GameObject specialAbilityGameObject)
    {
        var specialAbilityObject = SpecialAbilitiesCollection.Find(obj => obj.Name == specialAbilityGameObject.name);
        bool isSelectedAbility = SelectedSpecialAbility is null;
        SelectedSpecialAbility = isSelectedAbility ? specialAbilityObject.SpecialAbility : null;

        ConfirmButton.interactable = isSelectedAbility;
        specialAbilityGameObject.GetComponent<Image>().color = isSelectedAbility ? Color.green : Color.white;

        UpdateDescendantButtons(specialAbilityGameObject, isSelectedAbility);
    }
    public void UpdateDescendantButtons(GameObject selectedGameObject, bool isSelectedAbility)
    {
        var selectedParent = CharacterCreationManager.SelectedParent;
        var parentAbilityObject = SpecialAbilitiesCollection.Find(obj => obj.Name == selectedParent.SpecialAbility.Name);
        var descendantGameObjects = FindDescendantGameObjects(parentAbilityObject);

        foreach (var gameObject in descendantGameObjects.Where(obj => obj.name != selectedGameObject.name))
        {
            var button = gameObject.GetComponent<Button>();
            button.interactable = !isSelectedAbility;
            gameObject.GetComponent<Image>().color = isSelectedAbility ? Color.black : Color.white;
        }
    }
    public void ResetAbilitySelection()
    {
        var selectedParent = CharacterCreationManager.SelectedParent;
        var parentAbilityObject = SpecialAbilitiesCollection.Find(obj => obj.Name == selectedParent.SpecialAbility.Name);
        var descendantGameObjects = FindDescendantGameObjects(parentAbilityObject);

        foreach (var gameObject in descendantGameObjects)
        {
            gameObject.GetComponent<Button>().interactable = true;
            gameObject.GetComponent<Image>().color = Color.white;
        }

        var nonDescendantGameObjects = SpecialAbilityGameObjects.Except(descendantGameObjects).ToList();
        foreach (var gameObject in nonDescendantGameObjects)
        {
            gameObject.GetComponent<Button>().interactable = false;
            gameObject.GetComponent<Image>().color = Color.black;
        }
    }
    public List<GameObject> FindDescendantGameObjects(SpecialAbilityObject specialAbility)
    {
        var descendantGameObjects = new List<GameObject>();

        foreach (var child in specialAbility.Childrens)
        {
            var descendantGameObject = SpecialAbilityGameObjects.Find(obj => obj.name == child.Name);
            if (descendantGameObject is not null)
            {
                descendantGameObjects.Add(descendantGameObject);
                descendantGameObjects.AddRange(FindDescendantGameObjects(child));
            }
        }

        return descendantGameObjects;
    }

    public void BuildSpecialAbilityGameObjects()
    {
        foreach (var specialAbility in SpecialAbilitiesCollection)
        {
            var specialAbilityGameObject = Instantiate(SpecialAbilityButton, SpecialAbilityContentPanel);
            specialAbilityGameObject.name = specialAbility.Name;

            var specialAbilityText = specialAbilityGameObject.GetComponentInChildren<TMP_Text>();
            specialAbilityText.text = specialAbility.Name;

            var specialAbilityButton = specialAbilityGameObject.GetComponent<Button>();
            specialAbilityButton.onClick.AddListener(() => MarkAbility(specialAbilityGameObject));

            specialAbility.Button = specialAbilityGameObject;

            SpecialAbilityGameObjects.Add(specialAbilityGameObject);
        }
    }

    public void ToggleOn()
    {
        StartFactory();
        Popup.SetActive(true);
    }
    public void ToggleOff()
    {
        Popup.SetActive(false);
    }
}