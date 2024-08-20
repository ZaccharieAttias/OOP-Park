using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class AttributesManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Popup;
    public Transform AttributesContentPanel;

    [Header("Buttons")]
    public GameObject AttributeButton;
    public GameObject InheritanceButton;

    [Header("Attribute Data")]
    public List<Attribute> AttributesCollection;
    public List<GameObject> AttributeGameObjects;


    public void Start()
    {
        InitializeUIElements();
        InitializeButtons();
        InitializeAttributeData();
        InitializeEventListeners();
    }
    public void InitializeUIElements()
    {
        Popup = GameObject.Find("Canvas/Popups/Attributes");
        AttributesContentPanel = Popup.transform.Find("Background/Foreground/Buttons/ScrollView/ViewPort/Content");
    }
    public void InitializeButtons()
    {
        AttributeButton = Resources.Load<GameObject>("Buttons/Default");
        InheritanceButton = GameObject.Find("Canvas/Popups/CharacterCreation");
    }
    public void InitializeAttributeData()
    {
        AttributesCollection = new List<Attribute>();
        AttributeGameObjects = new List<GameObject>();
    }
    public void InitializeEventListeners()
    {
        var popupToggleOn = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Attributes/Buttons/Edit").GetComponent<Button>();
        popupToggleOn.onClick.AddListener(ToggleOn);

        var popupToggleOff = Popup.transform.Find("Background/Foreground/Buttons/Close").GetComponent<Button>();
        popupToggleOff.onClick.AddListener(ToggleOff);
    }

    public void LoadPopup()
    {
        if (AttributeGameObjects.Count == 0) BuildAttributeGameObjects();

        AttributeGameObjects.ForEach(attributeGameObject => UpdateAttributeGameObject(attributeGameObject));
    }
    public void UpdateAttributeGameObject(GameObject attributeGameObject)
    {
        var currentCharacter = CharactersData.CharactersManager.CurrentCharacter;
        bool hasAttribute = currentCharacter.Attributes.Any(item => item.Name == attributeGameObject.name);

        Image image = attributeGameObject.GetComponent<Image>();
        image.color = hasAttribute ? Color.green : Color.white;
    }

    public void MarkAttribute(GameObject attributeGameObject, Attribute attribute)
    {
        var currentCharacter = CharactersData.CharactersManager.CurrentCharacter;
        var currentAttribute = currentCharacter.Attributes.FirstOrDefault(item => item.Name == attribute.Name);

        if (currentAttribute == null) AddAttributeToCharacter(currentCharacter, attribute);
        else RemoveAttributeFromCharacter(currentCharacter, currentAttribute);

        UpdateAttributeColor(attributeGameObject, currentAttribute);
    }
    public void AddAttributeToCharacter(CharacterB currentCharacter, Attribute attribute)
    {
        Attribute newAttribute = new(attribute, currentCharacter.Name);
        currentCharacter.Attributes.Add(newAttribute);
    }
    public void RemoveAttributeFromCharacter(CharacterB currentCharacter, Attribute currentAttribute)
    {
        bool isAttributeOwner = currentCharacter.Name == currentAttribute.Owner;

        if (isAttributeOwner) CancelAttributeReferences(currentCharacter, currentAttribute);
        else currentCharacter.Attributes.Remove(currentAttribute);
    }
    public void UpdateAttributeColor(GameObject attributeGameObject, Attribute currentAttribute)
    {
        Image image = attributeGameObject.GetComponent<Image>();
        image.color = currentAttribute == null ? Color.green : Color.white;
    }

    public bool IsAttributeExist(CharacterB character, string attributeName)
    {
        if (character == null) return false;

        bool isAttributeExist = character.Attributes.Any(attribute => attribute.Name.ToLower() == attributeName.ToLower());
        if (isAttributeExist == false && character.Parent != null) isAttributeExist = IsAttributeExist(character.Parent, attributeName);

        return isAttributeExist;
    }
    public bool IsAttributeGetterExist(CharacterB character, string attributeName)
    {
        if (character == null) return false;

        bool isAttributeGetterExist = character.Attributes.Any(attribute => attribute.Name.ToLower() == attributeName.ToLower() && attribute.Getter);
        if (isAttributeGetterExist == false && character.Parent != null) isAttributeGetterExist = IsAttributeGetterExist(character.Parent, attributeName);

        return isAttributeGetterExist;
    }
    public void CancelAttributeReferences(CharacterB character, Attribute attribute)
    {
        RemoveAttributeFromCharacterAndMethods(character, attribute);
        character.Childrens.ForEach(child => CancelAttributeReferences(child, attribute));
    }
    public void RemoveAttributeFromCharacterAndMethods(CharacterB character, Attribute attribute)
    {
        var referencedAttribute = character.Attributes.FirstOrDefault(item => item == attribute);
        if (referencedAttribute != null) character.Attributes.Remove(referencedAttribute);

        var dependentMethod = character.Methods.FirstOrDefault(item => item.Attribute == attribute);
        if (dependentMethod != null) character.Methods.Remove(dependentMethod);
    }

    public void BuildAttributeGameObjects()
    {
        foreach (var attribute in AttributesCollection)
        {
            CreateAttributeGameObject(attribute);
        }
    }
    public void CreateAttributeGameObject(Attribute attribute)
    {
        var attributeGameObject = Instantiate(AttributeButton, AttributesContentPanel);
        attributeGameObject.name = attribute.Name;

        var attributeButtonText = attributeGameObject.GetComponentInChildren<TMP_Text>();
        attributeButtonText.text = attribute.Name;

        var attributeButton = attributeGameObject.GetComponent<Button>();
        attributeButton.onClick.AddListener(() => MarkAttribute(attributeGameObject, attribute));

        AttributeGameObjects.Add(attributeGameObject);
    }

    public void ToggleOn()
    {
        SceneManagement.ScenePause("AttributesManager");

        LoadPopup();
        Popup.SetActive(true);

        if (RestrictionManager.Instance.AllowSingleInheritance || RestrictionManager.Instance.OnlineBuild) InheritanceButton.SetActive(false);
    }
    public void ToggleOff()
    {
        SceneManagement.SceneResume("AttributesManager");

        CharactersData.CharactersManager.DisplayCharacter(CharactersData.CharactersManager.CurrentCharacter);
        Popup.SetActive(false);

        if (RestrictionManager.Instance.AllowSingleInheritance || RestrictionManager.Instance.OnlineBuild) InheritanceButton.SetActive(true);
    }
}