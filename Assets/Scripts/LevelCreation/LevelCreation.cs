using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelCreation : MonoBehaviour
{
    AttributesPopupManager AttributesPopupManager;
    MethodsPopupManager MethodsPopupManager;
    CharacterManager CharacterManager;
    TreeBuilder TreeBuilder;


    public void Start()
    {
        InitializeProperties();
        InitializeCollections();
    }

    private void InitializeProperties()
    {
        AttributesPopupManager = GameObject.Find("Canvas/HTMenu/Popups/Attributes").GetComponent<AttributesPopupManager>();
        MethodsPopupManager = GameObject.Find("Canvas/HTMenu/Popups/Methods").GetComponent<MethodsPopupManager>();
        CharacterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
        TreeBuilder = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/Tree").GetComponent<TreeBuilder>();
    }

    private void InitializeCollections()
    {
        InitializeAttributesCollection();
        InitializeMethodsCollection();
        InitializeCharactersCollection();
    }

    private void InitializeAttributesCollection()
    {
        string attributeName;
        string attributeDescription;
        AccessModifier attributeAccessModifier;

        // Attribute 1
        attributeName = "MoveSpeed";
        attributeDescription = "This is the MoveSpeed method";
        attributeAccessModifier = AccessModifier.Public;
        AttributesPopupManager.AddAttribute(new CharacterAttribute(attributeName, attributeDescription, attributeAccessModifier));

        // Attribute 2
        attributeName = "GravityForce";
        attributeDescription = "This is the GravityForce method";
        attributeAccessModifier = AccessModifier.Protected;
        AttributesPopupManager.AddAttribute(new CharacterAttribute(attributeName, attributeDescription, attributeAccessModifier));

        // Attribute 3
        attributeName = "DoubleJump";
        attributeDescription = "This is the DoubleJump method";
        attributeAccessModifier = AccessModifier.Private;
        AttributesPopupManager.AddAttribute(new CharacterAttribute(attributeName, attributeDescription, attributeAccessModifier));
    }
    private void InitializeMethodsCollection()
    {
        string methodName;
        string methodDescription;
        AccessModifier methodAccessModifier;

        // Method 1
        methodName = "MoveSpeed";
        methodDescription = "This is the MoveSpeed method";
        methodAccessModifier = AccessModifier.Public;
        MethodsPopupManager.AddMethod(new CharacterMethod(methodName, methodDescription, methodAccessModifier));

        // Method 2
        methodName = "GravityForce";
        methodDescription = "This is the GravityForce method";
        methodAccessModifier = AccessModifier.Protected;
        MethodsPopupManager.AddMethod(new CharacterMethod(methodName, methodDescription, methodAccessModifier));

        // Method 3
        methodName = "DoubleJump";
        methodDescription = "This is the DoubleJump method";
        methodAccessModifier = AccessModifier.Private;
        MethodsPopupManager.AddMethod(new CharacterMethod(methodName, methodDescription, methodAccessModifier));
    }
    public void InitializeCharactersCollection()
    {
        string characterName;
        string characterDescription;
        List<Character> characterParents = new();

        // Character 1 
        characterName = "Character 1";
        characterDescription = "This is the first character";
        Character character1 = new(characterName, characterDescription, characterParents, true);
        InitializeCharacterObject(character1);
        TreeBuilder.Root = character1; // Temporary
        CharacterManager.AddCharacter(character1);

        // Character 2 
        characterName = "Character 2";
        characterDescription = "This is the second character";
        characterParents.Add(character1);
        Character character2 = new(characterName, characterDescription, characterParents, true);
        character2.Parents.ForEach(parent => parent.Childrens.Add(character2));
        InitializeCharacterObject(character2);
        CharacterManager.AddCharacter(character2);
        characterParents.Clear();

        // Character 3 
        characterName = "Character 3";
        characterDescription = "This is the third character";
        characterParents.Add(character1);
        Character character3 = new(characterName, characterDescription, characterParents, true);
        character3.Parents.ForEach(parent => parent.Childrens.Add(character3));
        InitializeCharacterObject(character3);
        CharacterManager.AddCharacter(character3);
        characterParents.Clear();        
    }
    private void InitializeCharacterObject(Character characterNode)
    {
        Transform parnetTransform = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/Scroll View/Viewport/All").transform;
        GameObject characterPrefab = Resources.Load<GameObject>("Prefabs/Buttons/Character");
        
        GameObject newPlayerButton = Instantiate(characterPrefab, parnetTransform);
        newPlayerButton.name = characterNode.Name;
        characterNode.CharacterButton.Button = newPlayerButton;
        newPlayerButton.GetComponent<CharacterDetails>().InitializeCharacter(characterNode);

        Button button = newPlayerButton.GetComponent<Button>();
        button.onClick.AddListener(() => CharacterManager.DisplayCharacterDetails(characterNode.Name));
    }
}