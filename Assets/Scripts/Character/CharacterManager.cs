using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.EventSystems;

public class CharacterManager : MonoBehaviour
{
    [SerializeField]
    public List<Character> _charactersCollection;
    public Character currentCharacter;

    public TMP_Text characterNameText;
    public TMP_Text descriptionText;

    public Transform attributesPanel;
    public Transform methodsPanel;

    public GameObject attributesPopup;
    public GameObject methodsPopup;
    public GameObject buttonPrefab;

    public GameObject CharacterTree;
    public GameObject _deleteCharacterButton;

    public void Start()
    {
        createDeletionButton();
        

        CharacterTree = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/All");
        _charactersCollection = new List<Character>();
        CreateCharacters(); // Temporary

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

            DisplayDeletionOption();

            PowerUp powerUp = GetComponent<PowerUp>();
            powerUp.ApplyPowerup(currentCharacter);
        }
    }

    private void createDeletionButton()
    {
        GameObject button = Resources.Load<GameObject>("Prefabs/Buttons/Button");
        Transform location = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details").transform;

        GameObject deleteCharacterButton = Instantiate(button, location);
        deleteCharacterButton.name = "Delete";

        TMP_Text buttonText = deleteCharacterButton.GetComponentInChildren<TMP_Text>();
        buttonText.text = "Delete";

        deleteCharacterButton.transform.localPosition = new Vector3(235, -195, 0);

        Button button1 = deleteCharacterButton.GetComponent<Button>();
        button1.onClick.AddListener(() => DeleteCharacter());

        _deleteCharacterButton = deleteCharacterButton;
    }

    private void DisplayDeletionOption()
    {
        if (currentCharacter.isOriginal == true || currentCharacter.childrens.Count > 0)  _deleteCharacterButton.SetActive(false);
        else _deleteCharacterButton.SetActive(true);      
    }

    private void DeleteCharacter()
    {
        foreach (Character character in _charactersCollection)
        {
            if (character.name == currentCharacter.name)
            {
                currentCharacter.Dispose();
                _charactersCollection.Remove(character);
                break;
            }
        }

        Destroy(GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/" + currentCharacter.name));
        
        DisplayCharacterDetails(_charactersCollection.First().name);
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
            accessModifierButton.setAttribute(attribute);
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
            accessModifierButton.setMethod(method);
        }
    }

    private Character FindCharacterByName(string characterName)
    {
        foreach (Character character in _charactersCollection)
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
        //gameObject.name = currentCharacter.name;
    }

    public void AddCharacter(List<Character> characterNewAncestors)
    {
        int nbr = _charactersCollection.Count + 1;  
        string characterName = "Character " + nbr.ToString();
        string characterDescription = "";
        Character newCharacter = new Character(characterName, characterDescription, characterNewAncestors, false);
        _charactersCollection.Add(newCharacter);

        for (int i = 0; i < _charactersCollection.Count; i++)
        {
           if (_charactersCollection[i].name == characterNewAncestors[0].name)
           {
               _charactersCollection[i].childrens.Add(_charactersCollection.Last());
               _charactersCollection.Last().parents.Add(_charactersCollection[i]);
           }
        }

        CharacterTree.GetComponent<ButtonTreeManager>().CreateButton(newCharacter);
        DisplayCharacterDetails(newCharacter.name);
    }

    public void CreateCharacters()
    {
        string characterName = "";
        string characterDescription = "";
        List<Character> characterAncestors = new List<Character>();

        // Character1 
        characterName = "Character 1";
        characterDescription = "This is the first character";
        Character character1 = new Character(characterName, characterDescription, characterAncestors, true);
        _charactersCollection.Add(character1);

        CharacterTree.AddComponent<ButtonTreeManager>();
        CharacterTree.GetComponent<ButtonTreeManager>().InitializeButtonTreeManager(character1, this);
        CharacterTree.GetComponent<ButtonTreeManager>().CreateButton(character1);

        // Character2 
        characterName = "Character 2";
        characterDescription = "This is the second character";
        characterAncestors.Add(character1);
        Character character2 = new Character(characterName, characterDescription, characterAncestors, true);
        _charactersCollection.Add(character2);
        _charactersCollection[0].childrens.Add(character2);
        _charactersCollection[1].parents.Add(character1);

        CharacterTree.GetComponent<ButtonTreeManager>().CreateButton(character2);
        characterAncestors.Clear();
        
        DisplayCharacterDetails(character2.name);
    }

    public Character GetCurrentCharacter()
    {
        return currentCharacter;
    }

    public List<GameObject> GetCurrentCollection()
    {
        List<GameObject> characterGameObjectCollection = new List<GameObject>();
        Transform parentTransform = CharacterTree.transform;
        string siblingNameToFind;
        Transform siblingTransform;  

        foreach (Character character in _charactersCollection)
        {
            siblingNameToFind = character.name;
            siblingTransform = parentTransform.Find(siblingNameToFind);  
            GameObject foundObject = siblingTransform.gameObject;
            characterGameObjectCollection.Add(foundObject);
        }

        return characterGameObjectCollection;
    }

    public void UpdateCollection(Character character)
    {
        _charactersCollection.Add(character);
    }
}
