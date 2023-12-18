using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;


public class CharacterManager : MonoBehaviour
{
    public Character[] characters;
    public Character currentCharacter;

    public TMP_Text characterNameText;
    public TMP_Text descriptionText;
    
    public Transform attributesPanel;
    public Transform methodsPanel;
    
    public GameObject attributesPopup;
    public GameObject methodsPopup;
    public GameObject buttonPrefab;
    public GameObject simpleButton;

    // public GameObject PlayerPrefab;
    public GameObject CharacterTree;

    public string imagePath  = "Imports/Characters/3/Idle/Idle (1).png";



    public void Start()
    {
        currentCharacter = characters[0];
        DisplayCharacterDetails(currentCharacter.name);
    }



    public void DisplayCharacterDetails(string characterName)
    {
        currentCharacter = FindCharacterByName(characterName);

        if (currentCharacter != null)
        {
            CleanPanel();

            characterNameText.text = currentCharacter.name;
            descriptionText.text = currentCharacter.description;
            ChangingGameObjectName();

            DisplayAttributes();
            DisplayMethods();
            PowerUp powerUp = GetComponent<PowerUp>();
            powerUp.ApplyPowerup(currentCharacter);
        }
    }

    private void DisplayAttributes()
    {
        foreach (CharacterAttribute attribute in currentCharacter.attributes)
        {
            GameObject attributeButton = Instantiate(buttonPrefab, attributesPanel);
            attributeButton.name = attribute.name;
            
            TMP_Text buttonText = attributeButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = attribute.name;

            AccessModifierButton accessModifierButton = attributeButton.GetComponent<AccessModifierButton>();
            accessModifierButton.associatedAttribute = attribute;
        }
    }

    private void DisplayMethods()
    {
        foreach (CharacterMethod method in currentCharacter.methods)
        {
            GameObject methodButton = Instantiate(buttonPrefab, methodsPanel);

            TMP_Text buttonText = methodButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = method.name;

            AccessModifierButton accessModifierButton = methodButton.GetComponent<AccessModifierButton>();
            accessModifierButton.associatedMethod = method;
        }
    }

    private Character FindCharacterByName(string characterName)
    {
        foreach (Character character in characters)
        {
            if (character.name == characterName)
            {
                return character;
            }
        }

        return null;
    }

    private void CleanPanel()
    {
        foreach (Transform child in attributesPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in methodsPanel)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowAttributesPopup()
    {
        if (currentCharacter != null)
        {
            AttributesPopupManager popupManager = attributesPopup.GetComponent<AttributesPopupManager>();
            popupManager.ShowAttributesPopup(currentCharacter);
        }
    }

    public void ShowMethodsPopup()
    {
        if (currentCharacter != null)
        {
            MethodsPopupManager popupManager = methodsPopup.GetComponent<MethodsPopupManager>();
            popupManager.ShowMethodsPopup(currentCharacter);
        }
    }

    public void ChangingGameObjectName()
    {
        gameObject.name = currentCharacter.name;
    }

    public void AddCharacter()
    {
        Character newCharacter = new Character();
        newCharacter.name = "New Character";
        newCharacter.description = "New Description";


        newCharacter.attributes = new CharacterAttribute[0];
        newCharacter.methods = new CharacterMethod[0];

        Character[] newCharacters = new Character[characters.Length + 1];
        for (int i = 0; i < characters.Length; i++)
        {
            newCharacters[i] = characters[i];
        }

        newCharacters[newCharacters.Length - 1] = newCharacter;
        characters = newCharacters;

    
        GameObject newPlayerButton = Instantiate(simpleButton, transform);
        newPlayerButton.transform.SetParent(CharacterTree.transform);

        newPlayerButton.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
        newPlayerButton.transform.localPosition = new Vector3(-147, -100, 0);
        newPlayerButton.transform.localScale = new Vector3(1, 1, 1);
        newPlayerButton.name = newCharacter.name;

        newPlayerButton.GetComponent<Button>().onClick.RemoveAllListeners();
        newPlayerButton.GetComponent<Button>().onClick.AddListener(() => DisplayCharacterDetails(newCharacter.name));
        newPlayerButton.GetComponent<Button>().onClick.AddListener(() => GetComponent<ChangingSkin>().BlackAndWhiteSkin());


        Debug.Log("Image Path: " + imagePath);
        Debug.Log("Application Path: " + Application.dataPath);
        string filePath = Path.Combine(Application.dataPath, imagePath);
        
        if (File.Exists(filePath))
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData); // Load image data into the texture
            newPlayerButton.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        }
        DisplayCharacterDetails(newCharacter.name);
    }

}
