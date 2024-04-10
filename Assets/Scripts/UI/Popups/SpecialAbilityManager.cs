using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class SpecialAbilityManager : MonoBehaviour
{
    public GameObject Popup;
    public GameObject ButtonPrefab;
    public Transform SpecialAbilityContentPanel;

    public SpecialAbility SelectedSpecialAbility;
    public List<GameObject> SpecialAbilityGameObjects;
    public List<SpecialAbilityObject> SpecialAbilitiesCollection;
    public SpecialAbilityTreeManager SpecialAbilityTreeManager;
    public SpecialAbilitiesManager SpecialAbilitiesManager;


    public List<Button> ControlButtons;
    public List<Button> NotAllowedButtons;
    public CharactersCreationManager CharacterCreationManager;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/SpecialAbilityTree");

        ButtonPrefab = Resources.Load<GameObject>("Buttons/Ability");
        SpecialAbilityContentPanel = GameObject.Find("Canvas/Popups/SpecialAbilityTree/Buttons/ScrollView/ViewPort/All").transform;
        CharacterCreationManager = GameObject.Find("Canvas/Popups").GetComponent<CharactersCreationManager>();
        SpecialAbilitiesManager = GameObject.Find("Canvas/Popups").GetComponent<SpecialAbilitiesManager>();
        SpecialAbilityTreeManager = GameObject.Find("Canvas/Popups").GetComponent<SpecialAbilityTreeManager>();

        ControlButtons = new List<Button>
        {
            Popup.transform.Find("Buttons/Confirm").gameObject.GetComponent<Button>()
        };
        ControlButtons[0].GetComponent<Button>().onClick.AddListener(() => ConfirmFactory());
        
        NotAllowedButtons = new List<Button>
        {
            GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Attributes/Buttons/Edit").GetComponent<Button>(),
            GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Methods/Buttons/Edit").GetComponent<Button>(),
            GameObject.Find("Canvas/Menus/CharacterCenter/SwapScreen").GetComponent<Button>(),
            GameObject.Find("Player").GetComponent<CharactersManager>().DeleteButton.GetComponent<Button>()
        };        
    }
    private void StartFactory()
    {
        ControlButtons[0].interactable = false;
        ControlButtons[0].gameObject.SetActive(true);

        SelectedSpecialAbility = null;
        SpecialAbilityGameObjects = new();

        BuildSpecialAbilityGameObjects();
        ToggleButtonsInteractability(NotAllowedButtons);
        SpecialAbilityTreeManager.BuildTree(SpecialAbilitiesCollection.First());
    }
    private void ConfirmFactory()
    {
        ControlButtons[0].interactable = false;

        ControlButtons[0].gameObject.SetActive(false);

        ToggleButtonsInteractability(SpecialAbilityGameObjects.Select(item => item.GetComponent<Button>()).ToList());
        ToggleButtonsInteractability(NotAllowedButtons);
        ToggleOff();
    }
    private void ToggleButtonsInteractability(List<Button> buttons) { foreach (Button button in buttons) button.interactable = !button.interactable; }
    private void BuildSpecialAbilityGameObjects()
    {
        foreach (SpecialAbilityObject SpeAbiGameObject in SpecialAbilitiesCollection)
        {
            GameObject SpeAbilityGameObject = Instantiate(ButtonPrefab, SpecialAbilityContentPanel);
            SpeAbilityGameObject.name = SpeAbiGameObject.name;
            SpeAbilityGameObject.GetComponentInChildren<TMP_Text>().text = SpeAbiGameObject.name;
            SpeAbiGameObject.Button = SpeAbilityGameObject;

            Button SpeAbiGameObjectButton = SpeAbilityGameObject.GetComponent<Button>();
            SpeAbiGameObjectButton.onClick.AddListener(() => MarkAbility());

            SpeAbiGameObjectButton.interactable = false;
            SpeAbiGameObjectButton.GetComponent<Image>().color = Color.black;

            SpecialAbilityGameObjects.Add(SpeAbilityGameObject);
        }
        
        List <Character> selectedParents = CharacterCreationManager.SelectedCharacterParents;
        List <SpecialAbilityObject> parentSpeAbiObject = new();
        foreach (Character selectedParent in selectedParents)
        {
            SpecialAbility specialAbility = selectedParent.SpecialAbility;
            parentSpeAbiObject.Add(SpecialAbilitiesCollection.Find(obj => obj.name == specialAbility.Name));
        }

        foreach (SpecialAbilityObject parentSpeAbi in parentSpeAbiObject)
        {
            List<GameObject> temp = FindDescendantGameObjects(parentSpeAbi);
            foreach (GameObject gameObject in temp)
            {
                gameObject.GetComponent<Button>().interactable = true;
                gameObject.GetComponent<Image>().color = Color.white;
            }
        }

    }
    private void MarkAbility()
    {
        var speAbiGameObject = EventSystem.current.currentSelectedGameObject;
        if (speAbiGameObject != null)
        {
            Debug.Log(speAbiGameObject.GetComponent<Button>().interactable);
            SpecialAbilityObject specialAbilityObject = SpecialAbilitiesCollection.Find(obj => obj.name == speAbiGameObject.name);
            bool isSelectedParentsAlready = SelectedSpecialAbility is not null && SelectedSpecialAbility.Name == specialAbilityObject.name;

            if (isSelectedParentsAlready) SelectedSpecialAbility = null;
            else SelectedSpecialAbility = specialAbilityObject.SpecialAbility;

            ControlButtons[0].GetComponent<Button>().interactable = SelectedSpecialAbility is not null;

            Image image = speAbiGameObject.GetComponent<Image>();
            image.color = isSelectedParentsAlready ? Color.white : Color.green;
        }
    }
    private List<GameObject> FindAncestorGameObjects(SpecialAbilityObject SpeAbility)
    {
        List<GameObject> AncestorGameObjects = new();
        SpecialAbilityObject parent  = SpeAbility.Parent;

        GameObject ancestorGameObject = SpecialAbilityGameObjects.Find(obj => obj.GetComponent<SpecialAbilityObject>().name == parent.name);
        AncestorGameObjects.Add(ancestorGameObject);
            
        List<GameObject> parentAncestors = FindAncestorGameObjects(parent);
        return AncestorGameObjects;
    }
    private List<GameObject> FindDescendantGameObjects(SpecialAbilityObject SpeAbility)
    {
        List<GameObject> DescendantGameObjects = new();
        List<SpecialAbilityObject> children = SpeAbility.Childrens;

        foreach (SpecialAbilityObject child in children)
        {
            GameObject descendantGameObject = SpecialAbilityGameObjects.Find(obj => obj.name == child.name);
            DescendantGameObjects.Add(descendantGameObject);

            List<GameObject> childDescendants = FindDescendantGameObjects(child);
            DescendantGameObjects.AddRange(childDescendants);
        }

        return DescendantGameObjects;
    }
    private void LoadPopup(List<Character> selectedCharacterParents)
    {
        SelectedSpecialAbility = null;
        ControlButtons[0].interactable = false;
        StartFactory();
    }
    public void ToggleOn(List<Character> selectedCharacterParents) 
    { 
        LoadPopup(CharacterCreationManager.SelectedCharacterParents); 
        Popup.SetActive(true);
    }
    public void ToggleOff() { Popup.SetActive(false); }
}
