using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class AttributesManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Popup;
    public GameObject AttributeButton;
    public Transform AttributesContentPanel;

    [Header("Attribute Data")]
    public List<Attribute> AttributesCollection;
    public List<GameObject> AttributeGameObjects;


    public void Start()
    {
        InitializeProperties();
    }
    public void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/Attributes");
        AttributeButton = Resources.Load<GameObject>("Buttons/Default");
        AttributesContentPanel = Popup.transform.Find("Background/Foreground/Buttons/ScrollView/ViewPort/Content");

        AttributesCollection = new();
        AttributeGameObjects = new();

        var popupToggleOn = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Attributes/Buttons/Edit").GetComponent<Button>();
        popupToggleOn.onClick.AddListener(() => ToggleOn());

        var popupToggleOff = Popup.transform.Find("Background/Foreground/Buttons/Close").GetComponent<Button>();
        popupToggleOff.onClick.AddListener(() => ToggleOff());
    }

    public void LoadPopup()
    {
        if (AttributeGameObjects.Count == 0)
        {
            BuildAttributeGameObjects();
        }

        foreach (var attributeGameObject in AttributeGameObjects)
        {
            Image image = attributeGameObject.GetComponent<Image>();
            image.color = CharactersData.CharactersManager.CurrentCharacter.Attributes.Any(item => item.Name == attributeGameObject.name) ? Color.green : Color.white;
        }
    }
    public void MarkAttribute(GameObject attributeGameObject, Attribute attribute)
    {
        var currentCharacter = CharactersData.CharactersManager.CurrentCharacter;
        var currentAttribute = currentCharacter.Attributes.FirstOrDefault(item => item.Name == attribute.Name);

        if (currentAttribute is null)
        {
            Attribute newAttribute = new(attribute, currentCharacter.Name);
            currentCharacter.Attributes.Add(newAttribute);
        }

        else
        {
            bool isAttributeOwner = currentCharacter.Name == currentAttribute.Owner;
            if (isAttributeOwner)
            {
                CancelAttributeReferences(currentCharacter, currentAttribute);
            }

            else
            {
                currentCharacter.Attributes.Remove(currentAttribute);
            }
        }

        Image image = attributeGameObject.GetComponent<Image>();
        image.color = currentAttribute is null ? Color.green : Color.white;
    }

    public void CancelAttributeReferences(CharacterB character, Attribute attribute)
    {
        var referencedAttribute = character.Attributes.FirstOrDefault(item => item == attribute);
        if (referencedAttribute is not null)
        {
            character.Attributes.Remove(referencedAttribute);
        }

        var dependentMethod = character.Methods.FirstOrDefault(item => item.Attribute == attribute);
        if (dependentMethod is not null)
        {
            character.Methods.Remove(dependentMethod);
        }

        foreach (var child in character.Childrens)
        {
            CancelAttributeReferences(child, attribute);
        }
    }
    public void BuildAttributeGameObjects()
    {
        foreach (var attribute in AttributesCollection)
        {
            var attributeGameObject = Instantiate(AttributeButton, AttributesContentPanel);
            attributeGameObject.name = attribute.Name;

            var attributeButtonText = attributeGameObject.GetComponentInChildren<TMP_Text>();
            attributeButtonText.text = attribute.Name;

            var attributeButton = attributeGameObject.GetComponent<Button>();
            attributeButton.onClick.AddListener(() => MarkAttribute(attributeGameObject, attribute));

            AttributeGameObjects.Add(attributeGameObject);
        }
    }

    public void ToggleOn()
    {
        LoadPopup();
        Popup.SetActive(true);
    }
    public void ToggleOff()
    {
        CharactersData.CharactersManager.DisplayCharacter(CharactersData.CharactersManager.CurrentCharacter);
        Popup.SetActive(false);
    }
}
