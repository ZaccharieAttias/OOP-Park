using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class AttributesManager : MonoBehaviour
{
    public GameObject Popup;
    public List<Attribute> AttributesCollection;
    
    public GameObject AttributeButton;
    public Transform AttributesContentPanel;

    
    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/Attributes");
        AttributesCollection = new List<Attribute>();

        AttributeButton = Resources.Load<GameObject>("Buttons/Default");
        AttributesContentPanel = Popup.transform.Find("Background/Foreground/Buttons/ScrollView/ViewPort/Content");
        
        Button PopupToggleOn = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Attributes/Buttons/Edit").GetComponent<Button>();
        PopupToggleOn.onClick.AddListener(() => ToggleOn());

        Button PopupToggleOff = Popup.transform.Find("Background/Foreground/Buttons/Close").GetComponent<Button>();
        PopupToggleOff.onClick.AddListener(() => ToggleOff());
    }


    private void LoadPopup()
    {
        ClearContentPanel();
        
        foreach (Attribute attribute in AttributesCollection)
        {
            GameObject attributeGameObject = Instantiate(AttributeButton, AttributesContentPanel);
            attributeGameObject.name = attribute.Name;

            TMP_Text attributeButtonText = attributeGameObject.GetComponentInChildren<TMP_Text>();
            attributeButtonText.text = attribute.Name;

            Image image = attributeGameObject.GetComponent<Image>();
            image.color = CharactersData.CharactersManager.CurrentCharacter.Attributes.Any(item => item.Name == attribute.Name) ? Color.green : Color.white;
            
            Button attributeButton = attributeGameObject.GetComponent<Button>();
            attributeButton.onClick.AddListener(() => MarkAttribute(attributeGameObject, attribute));
        }
    }
    private void ClearContentPanel() { foreach (Transform attributeTransform in AttributesContentPanel) Destroy(attributeTransform.gameObject); }
    private void MarkAttribute(GameObject attributeGameObject, Attribute attribute)
    {
        Character currentCharacter = CharactersData.CharactersManager.CurrentCharacter;
        var currentAttribute = currentCharacter.Attributes.FirstOrDefault(item => item.Name == attribute.Name);
    
        if (currentAttribute is null)
        {
            Attribute newAttribute = new(attribute);
            currentCharacter.Attributes.Add(newAttribute);
        }
        
        else
        {
            bool isAttributeOwner = currentCharacter.Name == AttributesData.FindAttributeOwner(currentCharacter, currentAttribute);
            if (isAttributeOwner) CancelAttributeReferences(currentCharacter, currentAttribute);
            else currentCharacter.Attributes.Remove(currentAttribute);
        }

        Image image = attributeGameObject.GetComponent<Image>();
        image.color = currentAttribute is null ? Color.green : Color.white;
    }
    public void CancelAttributeReferences(Character character, Attribute attribute)
    {
        var referencedAttribute = character.Attributes.FirstOrDefault(item => item == attribute);
        if (referencedAttribute is not null) character.Attributes.Remove(referencedAttribute);

        var dependentMethod = character.Methods.FirstOrDefault(item => item.Attribute == attribute);
        if (dependentMethod is not null) character.Methods.Remove(dependentMethod);

        foreach (Character child in character.Childrens) CancelAttributeReferences(child, attribute);
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
