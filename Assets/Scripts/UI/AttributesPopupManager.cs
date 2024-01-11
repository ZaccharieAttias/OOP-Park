using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class AttributesPopupManager : MonoBehaviour
{
    public List<CharacterAttribute> AttributesCollection;
    public CharacterManager CharacterManager;
    
    public Button PopupToggleOn;
    public Button PopupToggleOff;

    public GameObject AttributeButton;
    public Transform ContentPanel;


    private void Start()
    {
        InitializeGameObject();
        InitializeProperties();
    }

    private void InitializeGameObject() { gameObject.SetActive(false); }
    private void InitializeProperties()
    {
        CharacterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
        AttributesCollection = new List<CharacterAttribute>();

        PopupToggleOn = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Attributes/Buttons/Edit").GetComponent<Button>();
        PopupToggleOn.onClick.AddListener(() => ToggleOn());

        PopupToggleOff = GameObject.Find("Canvas/HTMenu/Popups/Attributes/Background/Foreground/Buttons/Close").GetComponent<Button>();
        PopupToggleOff.onClick.AddListener(() => ToggleOff());

        AttributeButton = Resources.Load<GameObject>("Prefabs/Buttons/Button");
        ContentPanel = GameObject.Find("Canvas/HTMenu/Popups/Attributes/Background/Foreground/Buttons/ScrollView/ViewPort/Content").transform;
    }

    public void AddAttribute(CharacterAttribute attribute) { AttributesCollection.Add(attribute); }

    public void LoadPopup()
    {
        ClearContentPanel();

        foreach (CharacterAttribute attribute in AttributesCollection)
        {
            GameObject attributeButton = Instantiate(AttributeButton, ContentPanel);
            attributeButton.name = attribute.name;

            TMP_Text buttonText = attributeButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = attribute.name;

            MarkAttributeInPopup(attributeButton, attribute);
        }

        gameObject.SetActive(true);
    }
    private void MarkAttributeInPopup(GameObject attributeButton, CharacterAttribute attribute)
    {
        bool hasAttribute = CharacterManager.CurrentCharacter.Attributes.Any(item => item.name == attribute.name);

        Image image = attributeButton.GetComponent<Image>();
        image.color = hasAttribute ? Color.green : Color.white;

        Button button = attributeButton.GetComponent<Button>();
        button.onClick.AddListener(() => OnClick(attribute, hasAttribute));
    }
    private void OnClick(CharacterAttribute attribute, bool hasAttribute)
    {
        if (hasAttribute) CharacterManager.CurrentCharacter.Attributes.Remove(CharacterManager.CurrentCharacter.Attributes.Find(item => item.name == attribute.name));
        else CharacterManager.CurrentCharacter.Attributes.Add(new CharacterAttribute(attribute.name, attribute.description, attribute.accessModifier));

        LoadPopup();
    }
    private void ClearContentPanel() { foreach (Transform child in ContentPanel) Destroy(child.gameObject); }

    public void ToggleOn() 
    { 
        LoadPopup(); 
        gameObject.SetActive(true);    
    }
    public void ToggleOff() 
    { 
        CharacterManager.DisplayCharacterDetails(CharacterManager.CurrentCharacter.Name);
        gameObject.SetActive(false); 
    }
}
