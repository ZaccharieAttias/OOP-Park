using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class AttributesManager : MonoBehaviour
{
    public GameObject Popup;
    public List<CharacterAttribute> AttributesCollection;
    
    public GameObject AttributeButton;
    public Transform ContentPanel;

    public Button PopupToggleOn;
    public Button PopupToggleOff;

    public CharacterManager CharacterManager;

    
    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/Attributes");
        AttributesCollection = new List<CharacterAttribute>();

        AttributeButton = Resources.Load<GameObject>("Buttons/Default");
        ContentPanel = Popup.transform.Find("Background/Foreground/Buttons/ScrollView/ViewPort/Content");
        
        PopupToggleOn = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Attributes/Buttons/Edit").GetComponent<Button>();
        PopupToggleOn.onClick.AddListener(() => ToggleOn());

        PopupToggleOff = Popup.transform.Find("Background/Foreground/Buttons/Close").GetComponent<Button>();
        PopupToggleOff.onClick.AddListener(() => ToggleOff());
        
        CharacterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
    }

    public void AddAttribute(CharacterAttribute attribute) { AttributesCollection.Add(attribute); }

    private void LoadPopup()
    {
        ClearContentPanel();

        foreach (CharacterAttribute attribute in AttributesCollection)
        {
            GameObject attributeGameObject = Instantiate(AttributeButton, ContentPanel);
            attributeGameObject.name = attribute.Name;

            TMP_Text buttonText = attributeGameObject.GetComponentInChildren<TMP_Text>();
            buttonText.text = attribute.Name;

            Button attributeButton = attributeGameObject.GetComponent<Button>();
            attributeButton.onClick.AddListener(() => MarkAttribute(attributeGameObject, attribute));
            
            Image image = attributeGameObject.GetComponent<Image>();
            image.color = CharacterManager.CurrentCharacter.Attributes.Any(item => item.Name == attribute.Name) ? Color.green : Color.white;
        }
    }
    private void MarkAttribute(GameObject attributeGameObject, CharacterAttribute attribute)
    {
        var currentCharacter = CharacterManager.CurrentCharacter;
        var currentAttribute = currentCharacter.Attributes.Find(item => item.Name == attribute.Name);   
        
        if (currentAttribute != null)
        {
            currentCharacter.Attributes.Remove(currentAttribute);
            CancelDependentMethods(currentCharacter ,currentAttribute);
        }

        else
        {
            CharacterAttribute deepCopyAttribute = new(attribute.Name, attribute.Description, attribute.Value, attribute.AccessModifier);
            currentCharacter.Attributes.Add(deepCopyAttribute);
        }

        Image image = attributeGameObject.GetComponent<Image>();
        image.color = currentAttribute == null ? Color.green : Color.white;
    }
    private void CancelDependentMethods(Character character, CharacterAttribute deletedAttribute)
    {
        if (RestrictionManager.Instance.AllowBeginnerInheritance) character.Attributes.Remove(deletedAttribute);
        
        var dependentMethodToRemove = character.Methods.Find(method => method.Attribute == deletedAttribute);
        if (dependentMethodToRemove != null) character.Methods.Remove(dependentMethodToRemove);
        
        foreach (Character child in character.Childrens) CancelDependentMethods(child, deletedAttribute);
    }
    private void ClearContentPanel() { foreach (Transform child in ContentPanel) Destroy(child.gameObject); }

    public void ToggleOn()
    { 
        LoadPopup(); 
        Popup.SetActive(true);    
    }
    public void ToggleOff()
    {
        CharacterManager.DisplayCharacter(CharacterManager.CurrentCharacter);
        Popup.SetActive(false); 
    }
}
