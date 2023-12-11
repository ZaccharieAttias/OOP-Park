using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;


public class AttributesPopupManager : MonoBehaviour
{
    public CharacterAttribute[] allAttributes;
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
            currentCharacter.attributes = currentCharacter.attributes.Where(a => a.name != attribute.name).ToArray();
        }

        else
        {
            CharacterAttribute newAttribute = new CharacterAttribute(attribute.name, attribute.description, attribute.accessModifier);
            currentCharacter.attributes = currentCharacter.attributes.Append(newAttribute).ToArray();

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
