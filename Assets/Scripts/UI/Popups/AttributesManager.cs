using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class AttributesManager : MonoBehaviour
{
    public GameObject Popup;
    public List<CharacterAttribute> AttributesCollection;
    
    public GameObject AttributeButton;
    public Transform ContentPanel;

    public Button PopupToggleOn;
    public Button PopupToggleOff;

    public CharacterManager CharacterManager;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void OnGameStart()
    {
        AttributesManager attributesManager = GameObject.Find("Canvas/Popups").GetComponent<AttributesManager>();
        attributesManager.InitializeProperties();
    }
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

    public void LoadPopup()
    {
        ClearContentPanel();

        foreach (CharacterAttribute attribute in AttributesCollection)
        {
            GameObject attributeButton = Instantiate(AttributeButton, ContentPanel);
            attributeButton.name = attribute.Name;

            TMP_Text buttonText = attributeButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = attribute.Name;

            MarkAttributeInPopup(attributeButton, attribute);
        }
    }
    private void MarkAttributeInPopup(GameObject attributeButton, CharacterAttribute attribute)
    {
        bool hasAttribute = CharacterManager.CurrentCharacter.Attributes.Any(item => item.Name == attribute.Name);

        Image image = attributeButton.GetComponent<Image>();
        image.color = hasAttribute ? Color.green : Color.white;

        Button button = attributeButton.GetComponent<Button>();
        button.onClick.AddListener(() => OnClick(attribute, hasAttribute));
    }
    private void OnClick(CharacterAttribute attribute, bool hasAttribute)
    {
        if (hasAttribute) 
        {
            var currentCharacter = CharacterManager.CurrentCharacter;
            var currentAttribute = currentCharacter.Attributes.Find(item => item.Name == attribute.Name);

            currentCharacter.Attributes.Remove(currentAttribute);
            CancelDependentMethods(currentCharacter ,currentAttribute);
        }

        else 
            CharacterManager.CurrentCharacter.Attributes.Add(new CharacterAttribute(attribute.Name, attribute.Description, attribute.Value, attribute.AccessModifier));

        LoadPopup();
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
