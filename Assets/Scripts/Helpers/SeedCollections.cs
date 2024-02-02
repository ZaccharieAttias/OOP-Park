using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class SeedCollections : MonoBehaviour
{
    AttributesPopupManager AttributesPopupManager;
    MethodsPopupManager MethodsPopupManager;
    SpecialAbilityManager SpecialAbilityManager;
    CharacterManager CharacterManager;


    public void Start()
    {
        InitializeProperties();
        InitializeCollections();
    }
    private void InitializeProperties()
    {
        AttributesPopupManager = GameObject.Find("Canvas/HTMenu/Popups/Attributes").GetComponent<AttributesPopupManager>();
        MethodsPopupManager = GameObject.Find("Canvas/HTMenu/Popups/Methods").GetComponent<MethodsPopupManager>();
        SpecialAbilityManager = GameObject.Find("Canvas/HTMenu/Popups/SpecialAbility").GetComponent<SpecialAbilityManager>();
        CharacterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
    }
    private void InitializeCollections()
    {
        InitializeAttributesCollection();
        InitializeMethodsCollection();
        InitializeSpecialAbilityCollection();
        InitializeCharactersCollection();
    }
    private void InitializeAttributesCollection()
    {
        string attributeName;
        string attributeDescription;
        float attributeValue;
        AccessModifier attributeAccessModifier;

        // Attribute 1
        attributeName = "speedBuff";
        attributeValue = 5f;
        attributeDescription = "This is the MoveSpeed method";
        attributeAccessModifier = AccessModifier.Public;
        AttributesPopupManager.AddAttribute(new CharacterAttribute(attributeName, attributeDescription, attributeValue, attributeAccessModifier));

        // Attribute 2
        attributeName = "gravityBuff";
        attributeValue = 20f;

        attributeDescription = "This is the GravityForce method";
        attributeAccessModifier = AccessModifier.Protected;
        AttributesPopupManager.AddAttribute(new CharacterAttribute(attributeName, attributeDescription, attributeValue, attributeAccessModifier));

        // Attribute 3
        attributeName = "doubleJumpBuff";
        attributeValue = 2f;
        attributeDescription = "This is the DoubleJump method";
        attributeAccessModifier = AccessModifier.Private;
        AttributesPopupManager.AddAttribute(new CharacterAttribute(attributeName, attributeDescription, attributeValue, attributeAccessModifier));
    }
    private void InitializeMethodsCollection()
    {
        string methodName;
        string methodDescription;
        AccessModifier methodAccessModifier;

        // Method 1
        methodName = "SpeedBuff";
        methodDescription = "This is the MoveSpeed method";
        methodAccessModifier = AccessModifier.Public;
        MethodsPopupManager.AddMethod(new CharacterMethod(methodName, methodDescription, methodAccessModifier));

        // Method 2
        methodName = "GravityBuff";
        methodDescription = "This is the GravityForce method";
        methodAccessModifier = AccessModifier.Protected;
        MethodsPopupManager.AddMethod(new CharacterMethod(methodName, methodDescription, methodAccessModifier));

        // Method 3
        methodName = "DoubleJumpBuff";
        methodDescription = "This is the DoubleJump method";
        methodAccessModifier = AccessModifier.Private;
        MethodsPopupManager.AddMethod(new CharacterMethod(methodName, methodDescription, methodAccessModifier));
    }
    private void InitializeSpecialAbilityCollection()
    {
        string name;
        string description;
        float value;
        AbilityType type;
        CharacterSpecialAbility specialAbility;
        List<CharacterSpecialAbility> specialAbilities = new();

        // Special Ability 1 (General)
        name = "General";
        description = "Nothing Special";
        value = 0;
        type = AbilityType.General;
        specialAbility = new CharacterSpecialAbility(name, description, value, type);
        specialAbilities.Add(specialAbility);
        SpecialAbilityManager.SpecialAbilitiesCollection.Add(type, specialAbilities); 

        // Special Ability 2 (Automatic)
        specialAbilities = new();
        name = "Automatic";
        description = "Automatically";
        value = 0;
        type = AbilityType.Automatic;
        specialAbility = new CharacterSpecialAbility(name, description, value, type);
        specialAbilities.Add(specialAbility);
        SpecialAbilityManager.SpecialAbilitiesCollection.Add(type, specialAbilities);

        // Special Ability 4 (Jump)
        specialAbilities = new();
        name = "Jump";
        description = "Base Jump";
        value = 1;
        type = AbilityType.Jump;
        specialAbility = new CharacterSpecialAbility(name, description, value, type);
        specialAbilities.Add(specialAbility);
        SpecialAbilityManager.SpecialAbilitiesCollection.Add(type, specialAbilities);      
        SpecialAbilityManager.SpecialAbilitiesCollection[AbilityType.Automatic].Add(specialAbility);

        // Special Ability 5 (Gravity)
        specialAbilities = new();
        name = "Gravity";
        description = "Base Gravity";
        value = 1;
        type = AbilityType.Gravity;
        specialAbility = new CharacterSpecialAbility(name, description, value, type);
        specialAbilities.Add(specialAbility);
        SpecialAbilityManager.SpecialAbilitiesCollection.Add(type, specialAbilities);
        SpecialAbilityManager.SpecialAbilitiesCollection[AbilityType.Automatic].Add(specialAbility);

        // Special Ability 6 (Speed)
        specialAbilities = new();
        name = "Speed";
        description = "Base Speed";
        value = 1;
        type = AbilityType.Speed;
        specialAbility = new CharacterSpecialAbility(name, description, value, type);
        specialAbilities.Add(specialAbility);
        SpecialAbilityManager.SpecialAbilitiesCollection.Add(type, specialAbilities);
        SpecialAbilityManager.SpecialAbilitiesCollection[AbilityType.Automatic].Add(specialAbility);

        // Special Ability 7 (DoubleJump)
        specialAbilities = new();
        name = "Double Jump";
        description = "Double Jump";
        value = 2;
        type = AbilityType.DoubleJump;
        specialAbility = new CharacterSpecialAbility(name, description, value, type);
        specialAbilities.Add(specialAbility);
        SpecialAbilityManager.SpecialAbilitiesCollection.Add(type, specialAbilities);
        SpecialAbilityManager.SpecialAbilitiesCollection[AbilityType.Jump].Add(specialAbility);

        // Special Ability 8 (WeakGravity)
        specialAbilities = new();
        name = "Weak Gravity";
        description = "Weak Gravity";
        value = 0.5f;
        type = AbilityType.WeakGravity;
        specialAbility = new CharacterSpecialAbility(name, description, value, type);
        specialAbilities.Add(specialAbility);
        SpecialAbilityManager.SpecialAbilitiesCollection.Add(type, specialAbilities);
        SpecialAbilityManager.SpecialAbilitiesCollection[AbilityType.Gravity].Add(specialAbility);

        // Special Ability 9 (FastSpeed)
        specialAbilities = new();
        name = "Fast Speed";
        description = "Fast Speed";
        value = 2;
        type = AbilityType.FastSpeed;
        specialAbility = new CharacterSpecialAbility(name, description, value, type);
        specialAbilities.Add(specialAbility);
        SpecialAbilityManager.SpecialAbilitiesCollection.Add(type, specialAbilities);
        SpecialAbilityManager.SpecialAbilitiesCollection[AbilityType.Speed].Add(specialAbility);
    }
    public void InitializeCharactersCollection()
    {      
        string characterName;
        string characterDescription;
        List<Character> characterParents = new();
        CharacterSpecialAbility characterSpecialAbility;

        // Character 1 
        characterName = "Character 1";
        characterDescription = "This is the first character";
        
        characterSpecialAbility = SpecialAbilityManager.SpecialAbilitiesCollection[AbilityType.General][0];
        Character character1 = new(characterName, characterDescription, characterParents, characterSpecialAbility, true);
        InitializeCharacterObject(character1);
        CharacterManager.AddCharacter(character1);

        // Character 2 
        characterName = "Character 2";
        characterDescription = "This is the second character";
        characterParents.Add(character1);
        characterSpecialAbility = SpecialAbilityManager.SpecialAbilitiesCollection[AbilityType.Automatic][0];
        Character character2 = new(characterName, characterDescription, characterParents,characterSpecialAbility, true);
        character2.Parents.ForEach(parent => parent.Childrens.Add(character2));
        InitializeCharacterObject(character2);
        CharacterManager.AddCharacter(character2);
        characterParents.Clear();

        // Character 3 
        characterName = "Character 3";
        characterDescription = "This is the third character";
        characterParents.Add(character1);
        characterSpecialAbility = SpecialAbilityManager.SpecialAbilitiesCollection[AbilityType.Automatic][0];
        Character character3 = new(characterName, characterDescription, characterParents, characterSpecialAbility, true);
        character3.Parents.ForEach(parent => parent.Childrens.Add(character3));
        InitializeCharacterObject(character3);
        CharacterManager.AddCharacter(character3);
        characterParents.Clear();
    }
    private void InitializeCharacterObject(Character characterNode)
    {
        Transform parnetTransform = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/ScrollView/ViewPort/All").transform;
        GameObject characterPrefab = Resources.Load<GameObject>("Buttons/Character");
        List<Sprite> CharacterSprites = Resources.LoadAll<Sprite>("Sprites/Characters/").ToList();

        GameObject newPlayerButton = Instantiate(characterPrefab, parnetTransform);
        newPlayerButton.name = characterNode.Name;
        characterNode.CharacterButton.Button = newPlayerButton;
        newPlayerButton.GetComponent<CharacterDetails>().InitializeCharacter(characterNode);

        Button button = newPlayerButton.GetComponent<Button>();
        button.onClick.AddListener(() => CharacterManager.DisplayCharacter(characterNode));

        Image image = newPlayerButton.GetComponent<Image>();
        image.sprite = CharacterSprites[CharacterManager.CharactersCollection.Count % CharacterSprites.Count];
    }
}
