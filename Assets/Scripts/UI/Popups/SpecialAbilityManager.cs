using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SpecialAbilityManager : MonoBehaviour
{
    [Header("Scripts")]
    public CharactersCreationManager CharacterCreationManager;
    public SpecialAbilityTreeManager SpecialAbilityTreeManager;

    [Header("UI Elements")]
    public GameObject Popup;
    public Transform SpecialAbilityContentPanel;

    [Header("Buttons")]
    public GameObject SpecialAbilityButtonPrefab;
    public Button CancelButton;
    public Button ConfirmButton;

    [Header("Special Abilities Data")]
    public SpecialAbility SelectedSpecialAbility;
    public List<SpecialAbilityObject> SpecialAbilitiesCollection;
    public List<GameObject> SpecialAbilityGameObjects;
    public Dictionary<SpecialAbilityType, List<SpecialAbility>> SpecialAbilitiesDictionary;


    public void Start()
    {
        InitializeScripts();
        InitializeUIElements();
        InitializeButtons();
        InitializeSepcialAbilitiesData();
    }
    public void InitializeScripts()
    {
        CharacterCreationManager = GameObject.Find("Canvas/Popups").GetComponent<CharactersCreationManager>();
        SpecialAbilityTreeManager = GameObject.Find("Canvas/Popups").GetComponent<SpecialAbilityTreeManager>();
    }
    public void InitializeUIElements()
    {
        Popup = GameObject.Find("Canvas/Popups/SpecialAbilityTree");
        SpecialAbilityContentPanel = GameObject.Find("Canvas/Popups/SpecialAbilityTree/Background/Foreground/Buttons/ScrollView/ViewPort/All").transform;
    }
    public void InitializeButtons()
    {
        SpecialAbilityButtonPrefab = Resources.Load<GameObject>("Buttons/Ability");

        CancelButton = Popup.transform.Find("Background/Foreground/Buttons/Cancel").GetComponent<Button>();
        CancelButton.onClick.AddListener(CancelFactory);

        ConfirmButton = Popup.transform.Find("Background/Foreground/Buttons/Confirm").GetComponent<Button>();
        ConfirmButton.onClick.AddListener(ConfirmFactory);
    }
    public void InitializeSepcialAbilitiesData()
    {
        SelectedSpecialAbility = null;
        SpecialAbilitiesCollection = new List<SpecialAbilityObject>();
        SpecialAbilityGameObjects = new List<GameObject>();
        SpecialAbilitiesDictionary = new Dictionary<SpecialAbilityType, List<SpecialAbility>>();
    }

    public void StartFactory()
    {
        CancelButton.interactable = true;
        ConfirmButton.interactable = false;

        SelectedSpecialAbility = null;

        if (SpecialAbilityGameObjects.Count == 0) BuildSpecialAbilityGameObjects();


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
        bool isSelectedAbility = SelectedSpecialAbility == null;

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
            if (descendantGameObject != null)
            {
                descendantGameObjects.Add(descendantGameObject);
                descendantGameObjects.AddRange(FindDescendantGameObjects(child));
            }
        }

        return descendantGameObjects;
    }
    public void BuildSpecialAbilityGameObjects()
    {
        if (SpecialAbilityContentPanel.childCount > 0)
        {
            foreach (Transform child in SpecialAbilityContentPanel)
            {
                Destroy(child.gameObject);
            }

            SpecialAbilityGameObjects.Clear();
        }

        foreach (var specialAbility in SpecialAbilitiesCollection)
        {
            var specialAbilityGameObject = Instantiate(SpecialAbilityButtonPrefab, SpecialAbilityContentPanel);
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
        SceneManagement.ScenePause("SpecialAbilityManager");

        StartFactory();
        Popup.SetActive(true);
    }
    public void ToggleOff()
    {
        SceneManagement.SceneResume("SpecialAbilityManager");

        Popup.SetActive(false);
    }
}