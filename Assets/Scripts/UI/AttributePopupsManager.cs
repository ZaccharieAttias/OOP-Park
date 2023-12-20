using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;


public class AttributesPopupManager : MonoBehaviour
{
    public List <CharacterAttribute> allAttributes = new List<CharacterAttribute>();
    private Character currentCharacter;

    public Transform attributesPanel;

    public GameObject buttonPrefab;

    public CharacterManager characterManager;


    public void ShowAttributesPopup(Character currentChar)
    {
        ClearAttributesPanel();

        currentCharacter = currentChar;

        foreach (CharacterAttribute attribute in allAttributes)
        {
            GameObject attributeButton = Instantiate(buttonPrefab, attributesPanel);
            attributeButton.name = attribute.name;

            TMP_Text buttonText = attributeButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = attribute.name;

            MarkAttributeInPopup(attributeButton, attribute);
        }

        gameObject.SetActive(true);
    }

    private void MarkAttributeInPopup(GameObject attributeButton, CharacterAttribute attribute)
    {
        bool hasAttribute = currentCharacter.attributes.Any(a => a.name == attribute.name);

        Image image = attributeButton.GetComponent<Image>();
        image.color = hasAttribute ? Color.green : Color.white;

        Button button = attributeButton.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnAttributeButtonClick(attribute, hasAttribute));
    }

    private void OnAttributeButtonClick(CharacterAttribute attribute, bool hasAttribute)
    {
        if (hasAttribute)
        {
            currentCharacter.attributes.Remove(currentCharacter.attributes.Find(item => item.name == attribute.name));
        }
        else
        {
            currentCharacter.attributes.Add(new CharacterAttribute(attribute.name, attribute.description, attribute.accessModifier));
        }

        characterManager.DisplayCharacterDetails(currentCharacter.name);

        ShowAttributesPopup(currentCharacter);
    }

    private void ClearAttributesPanel()
    {
        foreach (Transform child in attributesPanel)
        {
            Destroy(child.gameObject);
        }
    }
}
