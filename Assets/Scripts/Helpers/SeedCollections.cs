using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class SeedCollections : MonoBehaviour
{
    AttributesManager AttributesManager;
    MethodsManager MethodsPopupManager;
    SpecialAbilitiesManager SpecialAbilitiesManager;
    CharactersManager CharactersManager;


    public void Start()
    {
        InitializeProperties();
        InitializeCollections();
    }
    private void InitializeProperties()
    {
        AttributesManager = GameObject.Find("Canvas/Popups").GetComponent<AttributesManager>();
        MethodsPopupManager = GameObject.Find("Canvas/Popups").GetComponent<MethodsManager>();
        SpecialAbilitiesManager = GameObject.Find("Canvas/Popups").GetComponent<SpecialAbilitiesManager>();
        CharactersManager = GameObject.Find("Player").GetComponent<CharactersManager>();
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
        attributeName = "speed";
        attributeValue = 5f;
        attributeDescription = "This is the MoveSpeed attribute";
        attributeAccessModifier = AccessModifier.Public;
        AttributesManager.AddAttribute(new CharacterAttribute(attributeName, attributeDescription, attributeValue, attributeAccessModifier));

        // Attribute 2
        attributeName = "gravity";
        attributeValue = 20f;

        attributeDescription = "This is the GravityForce attribute";
        attributeAccessModifier = AccessModifier.Protected;
        AttributesManager.AddAttribute(new CharacterAttribute(attributeName, attributeDescription, attributeValue, attributeAccessModifier));

        // Attribute 3
        attributeName = "multipleJumps";
        attributeValue = 2f;
        attributeDescription = "This is the DoubleJump attribute";
        attributeAccessModifier = AccessModifier.Private;
        AttributesManager.AddAttribute(new CharacterAttribute(attributeName, attributeDescription, attributeValue, attributeAccessModifier));
    
        // Attribute 4
        attributeName = "fireballShoot";
        attributeValue = 0.25f;
        attributeDescription = "This is the FireballShoot attribute";
        attributeAccessModifier = AccessModifier.Public;
        AttributesManager.AddAttribute(new CharacterAttribute(attributeName, attributeDescription, attributeValue, attributeAccessModifier));
    }
    private void InitializeMethodsCollection()
    {
        string methodName;
        string methodDescription;
        AccessModifier methodAccessModifier;
        CharacterMethod characterMethod = null;

        // Method 1
        methodName = "Speed";
        methodDescription = "This is the MoveSpeed method";
        methodAccessModifier = AccessModifier.Public;
        characterMethod = new(methodName, methodDescription, methodAccessModifier);
        characterMethod.Attribute = AttributesManager.AttributesCollection[0];
        MethodsPopupManager.AddMethod(characterMethod);

        // Method 2
        characterMethod = null;
        methodName = "Gravity";
        methodDescription = "This is the GravityForce method";
        methodAccessModifier = AccessModifier.Protected;
        characterMethod = new(methodName, methodDescription, methodAccessModifier);
        characterMethod.Attribute = AttributesManager.AttributesCollection[0];
        MethodsPopupManager.AddMethod(characterMethod);
        

        // Method 3
        characterMethod = null;
        methodName = "MultipleJumps";
        methodDescription = "This is the DoubleJump method";
        methodAccessModifier = AccessModifier.Private;
        characterMethod = new(methodName, methodDescription, methodAccessModifier);
        characterMethod.Attribute = AttributesManager.AttributesCollection[0];
        MethodsPopupManager.AddMethod(characterMethod);

        // Method 4
        characterMethod = null;
        methodName = "FireballShoot";
        methodDescription = "This is the FireballShoot method";
        methodAccessModifier = AccessModifier.Public;
        characterMethod = new(methodName, methodDescription, methodAccessModifier);
        characterMethod.Attribute = AttributesManager.AttributesCollection[0];
        MethodsPopupManager.AddMethod(characterMethod);
    }
    private void InitializeSpecialAbilityCollection()
    {
        string name;
        string description;
        float value;
        SpecialAbility type;
        CharacterSpecialAbility specialAbility;
        List<CharacterSpecialAbility> specialAbilities = new();

        // Special Ability 1 (General)
        name = "General";
        description = "Nothing Special";
        value = 0;
        type = SpecialAbility.General;
        specialAbility = new CharacterSpecialAbility(name, description, value, type);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(type, specialAbilities); 

        // Special Ability 2 (Automatic)
        specialAbilities = new();
        name = "Automatic";
        description = "Automatically";
        value = 0;
        type = SpecialAbility.Automatic;
        specialAbility = new CharacterSpecialAbility(name, description, value, type);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(type, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbility.General].Add(specialAbility);

        // Special Ability 3 (Manual)
        specialAbilities = new();
        name = "Manual";
        description = "Manually";
        value = 0;
        type = SpecialAbility.Manual;
        specialAbility = new CharacterSpecialAbility(name, description, value, type);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(type, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbility.General].Add(specialAbility);

        // Special Ability 4 (Jump)
        specialAbilities = new();
        name = "Jump";
        description = "Base Jump";
        value = 1;
        type = SpecialAbility.Jump;
        specialAbility = new CharacterSpecialAbility(name, description, value, type);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(type, specialAbilities);      
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbility.Automatic].Add(specialAbility);

        // Special Ability 5 (Gravity)
        specialAbilities = new();
        name = "Gravity";
        description = "Base Gravity";
        value = 1;
        type = SpecialAbility.Gravity;
        specialAbility = new CharacterSpecialAbility(name, description, value, type);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(type, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbility.Automatic].Add(specialAbility);

        // Special Ability 6 (Speed)
        specialAbilities = new();
        name = "Speed";
        description = "Base Speed";
        value = 1;
        type = SpecialAbility.Speed;
        specialAbility = new CharacterSpecialAbility(name, description, value, type);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(type, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbility.Automatic].Add(specialAbility);

        // Special Ability 7 (FireballShoot)
        specialAbilities = new();
        name = "Fireball Shoot";
        description = "Fireball Shoot";
        value = 0.25f;
        type = SpecialAbility.FireballShoot;
        specialAbility = new CharacterSpecialAbility(name, description, value, type);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(type, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbility.Manual].Add(specialAbility);

        // Special Ability 8 (DoubleJump)
        specialAbilities = new();
        name = "Double Jump";
        description = "Double Jump";
        value = 2;
        type = SpecialAbility.DoubleJump;
        specialAbility = new CharacterSpecialAbility(name, description, value, type);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(type, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbility.Jump].Add(specialAbility);

        // Special Ability 9 (WeakGravity)
        specialAbilities = new();
        name = "Weak Gravity";
        description = "Weak Gravity";
        value = 0.5f;
        type = SpecialAbility.WeakGravity;
        specialAbility = new CharacterSpecialAbility(name, description, value, type);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(type, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbility.Gravity].Add(specialAbility);

        // Special Ability 10 (FastSpeed)
        specialAbilities = new();
        name = "Fast Speed";
        description = "Fast Speed";
        value = 2;
        type = SpecialAbility.FastSpeed;
        specialAbility = new CharacterSpecialAbility(name, description, value, type);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(type, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbility.Speed].Add(specialAbility);
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
        
        characterSpecialAbility = SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbility.General][0];
        Character character1 = new(characterName, characterDescription, characterParents, characterSpecialAbility, true, true);
        InitializeCharacterObject(character1);
        CharactersManager.AddCharacter(character1);

        // Character 2 
        characterName = "Character 2";
        characterDescription = "This is the second character";
        characterParents.Add(character1);
        characterSpecialAbility = SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbility.Automatic][0];
        Character character2 = new(characterName, characterDescription, characterParents,characterSpecialAbility, true, false);
        character2.Parents.ForEach(parent => parent.Childrens.Add(character2));
        InitializeCharacterObject(character2);
        CharactersManager.AddCharacter(character2);
        characterParents.Clear();

        // Character 3 
        characterName = "Character 3";
        characterDescription = "This is the third character";
        characterParents.Add(character1);
        characterSpecialAbility = SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbility.Manual][0];
        Character character3 = new(characterName, characterDescription, characterParents, characterSpecialAbility, true, false);
        character3.Parents.ForEach(parent => parent.Childrens.Add(character3));
        InitializeCharacterObject(character3);
        CharactersManager.AddCharacter(character3);
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
        button.onClick.AddListener(() => CharactersManager.DisplayCharacter(characterNode));

        Image image = newPlayerButton.GetComponent<Image>();
        image.sprite = CharacterSprites[CharactersManager.CharactersCollection.Count % CharacterSprites.Count];
    }
}
