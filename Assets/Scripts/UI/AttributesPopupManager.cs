using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;


public class AttributesPopupManager : MonoBehaviour
{
    private readonly string _buttonPrefabPath = "Prefabs/Buttons/Button";
    private readonly string _contentPanelPath = "Canvas/HTMenu/Popups/Attributes/Background/Foreground/Buttons/ScrollView/ViewPort/Content";

    private GameObject _buttonPrefab;
    private Transform _contentPanel;
    private CharacterManager _characterManager;

    private List<CharacterAttribute> _collection;
    private Character _currentCharacter;


    private void Start()
    {
        _buttonPrefab = Resources.Load<GameObject>(_buttonPrefabPath);
        _contentPanel = GameObject.Find(_contentPanelPath).transform;
        _characterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
        _collection = InitializeCollection();

        gameObject.SetActive(false);
    }

    public void ShowAttributesPopup(Character currentCharacter)
    {
        ClearContentPanel();

        _currentCharacter = currentCharacter;

        foreach (CharacterAttribute attribute in _collection)
        {
            GameObject attributeButton = Instantiate(_buttonPrefab, _contentPanel);
            attributeButton.name = attribute.name;

            TMP_Text buttonText = attributeButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = attribute.name;

            MarkAttributeInPopup(attributeButton, attribute);
        }

        gameObject.SetActive(true);
    }

    private void MarkAttributeInPopup(GameObject attributeButton, CharacterAttribute attribute)
    {
        bool hasAttribute = _currentCharacter.attributes.Any(item => item.name == attribute.name);

        Image image = attributeButton.GetComponent<Image>();
        image.color = hasAttribute ? Color.green : Color.white;

        Button button = attributeButton.GetComponent<Button>();
        button.onClick.AddListener(() => OnClick(attribute, hasAttribute));
    }

    private void OnClick(CharacterAttribute attribute, bool hasAttribute)
    {
        if (hasAttribute) _currentCharacter.attributes.Remove(_currentCharacter.attributes.Find(item => item.name == attribute.name));
        else _currentCharacter.attributes.Add(new CharacterAttribute(attribute.name, attribute.description, attribute.accessModifier));

        _characterManager.DisplayCharacterDetails(_currentCharacter.name); // Bad performances, Ideal to do that when closing the panel
        ShowAttributesPopup(_currentCharacter);
    }

    private void ClearContentPanel()
    {
        foreach (Transform child in _contentPanel) Destroy(child.gameObject);
    }

    private List<CharacterAttribute> InitializeCollection()
    {
        List<CharacterAttribute> collection = new List<CharacterAttribute>();

        string attributeName = "";
        string attributeDescription = "";
        AccessModifier attributeAccessModifier = AccessModifier.Public;

        // Attribute1 
        attributeName = "Attribute 1";
        attributeDescription = "This is the first attribute";
        attributeAccessModifier = AccessModifier.Public;
        collection.Add(new CharacterAttribute(attributeName, attributeDescription, attributeAccessModifier));

        // Attribute2
        attributeName = "Attribute 2";
        attributeDescription = "This is the second attribute";
        attributeAccessModifier = AccessModifier.Protected;
        collection.Add(new CharacterAttribute(attributeName, attributeDescription, attributeAccessModifier));

        // Attribute3
        attributeName = "Attribute 3";
        attributeDescription = "This is the third attribute";
        attributeAccessModifier = AccessModifier.Private;
        collection.Add(new CharacterAttribute(attributeName, attributeDescription, attributeAccessModifier));

        return collection;
    }
}