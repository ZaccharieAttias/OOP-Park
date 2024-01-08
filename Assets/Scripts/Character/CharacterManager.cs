using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


[Serializable]
public class CharacterManager : MonoBehaviour
{
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

    public TreeBuilder TreeBuilder;


    public void Start()
    {
        createDeletionButton();
        
        TreeBuilder = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/Tree").GetComponent<TreeBuilder>();

        CharacterTree = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/Tree/All");
        _charactersCollection = new List<Character>();
        CreateCharacters(); // Temporary

    }

    public void DisplayCharacterDetails(string characterName)
    {
        currentCharacter = FindCharacterByName(characterName);

        if (currentCharacter != null)
        {
            CleanPanel();

            characterNameText.text = currentCharacter.Name;
            descriptionText.text = currentCharacter.Description;
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
        if (currentCharacter.IsOriginal == true || currentCharacter.Childrens.Count > 0)  _deleteCharacterButton.SetActive(false);
        else _deleteCharacterButton.SetActive(true);      
    }

    private void DeleteCharacter()
    {

        Destroy(GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/" + currentCharacter.Name));
        
        DisplayCharacterDetails(_charactersCollection.First().Name);
    }

    private void DisplayAttributes()
    {
        foreach (CharacterAttribute attribute in currentCharacter.Attributes)
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
        foreach (CharacterMethod method in currentCharacter.Methods)
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
            if (character.Name == characterName)
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

    public void AddCharacter(Character builtCharacter)
    {
        _charactersCollection.Add(builtCharacter);
        TreeBuilder.BuildTree();

        DisplayCharacterDetails(builtCharacter.Name);
    }

    public void CreateCharacters()
    {
        string characterName;
        string characterDescription;
        List<Character> characterAncestors = new();

        // Character1 
        characterName = "Character 1";
        characterDescription = "This is the first character";
        Character character1 = new(characterName, characterDescription, characterAncestors, true);
        TempForCreateCharacterButton(character1);
        TreeBuilder.Root = character1; // Temporary
        AddCharacter(character1);

        // Character2 
        characterName = "Character 2";
        characterDescription = "This is the second character";
        characterAncestors.Add(character1);
        Character character2 = new(characterName, characterDescription, characterAncestors, true);
        character2.Parents.ForEach(parent => parent.Childrens.Add(character2));
        TempForCreateCharacterButton(character2);
        AddCharacter(character2);
        characterAncestors.Clear();

        // Character3 
        characterName = "Character 3";
        characterDescription = "This is the third character";
        characterAncestors.Add(character1);
        Character character3 = new(characterName, characterDescription, characterAncestors, true);
        character3.Parents.ForEach(parent => parent.Childrens.Add(character3));
        TempForCreateCharacterButton(character3);
        AddCharacter(character3);
        characterAncestors.Clear();
        
        DisplayCharacterDetails(character1.Name);
    }

    private void TempForCreateCharacterButton(Character characterNode)
    {
        Transform parnetTransform = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/Tree/All").transform;

        GameObject characterPrefab = Resources.Load<GameObject>("Prefabs/Buttons/Character");
        GameObject newPlayerButton = Instantiate(characterPrefab, parnetTransform);
        newPlayerButton.name = characterNode.Name;
        characterNode.CharacterButton.Button = newPlayerButton;
        newPlayerButton.GetComponent<CharacterDetails>().InitializeCharacter(characterNode);
        Button button = newPlayerButton.GetComponent<Button>();
        button.onClick.AddListener(() => DisplayCharacterDetails(characterNode.Name)); // Change it to set current Character and from there its somehow change the details
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
            siblingNameToFind = character.Name;
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
