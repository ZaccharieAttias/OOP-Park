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
    public Transform ContentPanel;

    public CharactersManager CharactersManager;

    
    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/Attributes");
        AttributesCollection = new List<Attribute>();

        AttributeButton = Resources.Load<GameObject>("Buttons/Default");
        ContentPanel = Popup.transform.Find("Background/Foreground/Buttons/ScrollView/ViewPort/Content");
        
        Button PopupToggleOn = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Attributes/Buttons/Edit").GetComponent<Button>();
        PopupToggleOn.onClick.AddListener(() => ToggleOn());

        Button PopupToggleOff = Popup.transform.Find("Background/Foreground/Buttons/Close").GetComponent<Button>();
        PopupToggleOff.onClick.AddListener(() => ToggleOff());
        
        CharactersManager = GameObject.Find("Player").GetComponent<CharactersManager>();
    }


    private void LoadPopup()
    {
        ClearContentPanel();

        foreach (Attribute attribute in AttributesCollection)
        {
            GameObject attributeGameObject = Instantiate(AttributeButton, ContentPanel);
            attributeGameObject.name = attribute.Name;

            TMP_Text attributeButtonText = attributeGameObject.GetComponentInChildren<TMP_Text>();
            attributeButtonText.text = attribute.Name;

            Button attributeButton = attributeGameObject.GetComponent<Button>();
            attributeButton.onClick.AddListener(() => MarkAttribute(attributeGameObject, attribute));
            
            Image image = attributeGameObject.GetComponent<Image>();
            image.color = CharactersManager.CurrentCharacter.Attributes.Any(item => item.Name == attribute.Name) ? Color.green : Color.white;
        }
    }
    private void ClearContentPanel() { foreach (Transform child in ContentPanel) Destroy(child.gameObject); }
    private void MarkAttribute(GameObject attributeGameObject, Attribute attribute)
    {
        var currentCharacter = CharactersManager.CurrentCharacter;
        var currentAttribute = currentCharacter.Attributes.Find(item => item.Name == attribute.Name);   
        
        if (currentAttribute is not null)
        {
            currentCharacter.Attributes.Remove(currentAttribute);
            CancelDependentMethods(currentCharacter ,currentAttribute);
        }

        else
        {
            Attribute newAttribute = new(attribute.Name, attribute.Description, attribute.Value, attribute.AccessModifier);
            currentCharacter.Attributes.Add(newAttribute);
        }

        Image image = attributeGameObject.GetComponent<Image>();
        image.color = currentAttribute is null ? Color.green : Color.white;
    }
    private void CancelDependentMethods(Character character, Attribute attribute)
    {
        if (RestrictionManager.Instance.AllowBeginnerInheritance) character.Attributes.Remove(attribute);
        
        var dependentMethodToRemove = character.Methods.Find(method => method.Attribute == attribute);
        if (dependentMethodToRemove is not null) character.Methods.Remove(dependentMethodToRemove);
        
        foreach (Character child in character.Childrens) CancelDependentMethods(child, attribute);
    }

    public void ToggleOn()
    { 
        LoadPopup();
        Popup.SetActive(true);    
    }
    public void ToggleOff()
    {
        CharactersManager.DisplayCharacter(CharactersManager.CurrentCharacter);
        Popup.SetActive(false); 
    }
}
