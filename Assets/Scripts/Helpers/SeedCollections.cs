using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class SeedCollections : MonoBehaviour
{
    AttributesManager AttributesManager;
    MethodsManager MethodsManager;
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
        MethodsManager = GameObject.Find("Canvas/Popups").GetComponent<MethodsManager>();
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
        CharacterAttribute attribute;

        // Attribute 1
        attributeName = "speed";
        attributeValue = 5f;
        attributeDescription = "This is the MoveSpeed attribute";
        attributeAccessModifier = AccessModifier.Public;
        attribute = new CharacterAttribute(attributeName, attributeDescription, attributeValue, attributeAccessModifier);
        AttributesManager.AttributesCollection.Add(attribute);

        // Attribute 2
        attributeName = "gravity";
        attributeValue = 20f;
        attributeDescription = "This is the GravityForce attribute";
        attributeAccessModifier = AccessModifier.Protected;
        attribute = new CharacterAttribute(attributeName, attributeDescription, attributeValue, attributeAccessModifier);
        AttributesManager.AttributesCollection.Add(attribute);

        // Attribute 3
        attributeName = "multipleJumps";
        attributeValue = 2f;
        attributeDescription = "This is the DoubleJump attribute";
        attributeAccessModifier = AccessModifier.Private;
        attribute = new CharacterAttribute(attributeName, attributeDescription, attributeValue, attributeAccessModifier);
        AttributesManager.AttributesCollection.Add(attribute);

        // Attribute 4
        attributeName = "fireballShoot";
        attributeValue = 0.25f;
        attributeDescription = "This is the FireballShoot attribute";
        attributeAccessModifier = AccessModifier.Public;
        attribute = new CharacterAttribute(attributeName, attributeDescription, attributeValue, attributeAccessModifier);
        AttributesManager.AttributesCollection.Add(attribute);
    }
    private void InitializeMethodsCollection()
    {
        string methodName;
        string methodDescription;
        CharacterAttribute methodAttribute;
        AccessModifier methodAccessModifier;
        CharacterMethod method;

        // Method 1
        methodName = "Speed";
        methodDescription = "This is the MoveSpeed method";
        methodAttribute = AttributesManager.AttributesCollection[0];
        methodAccessModifier = AccessModifier.Public;
        method = new CharacterMethod(methodName, methodDescription, methodAttribute, methodAccessModifier);
        MethodsManager.MethodsCollection.Add(method);

        // Method 2
        methodName = "Gravity";
        methodDescription = "This is the GravityForce method";
        methodAttribute = AttributesManager.AttributesCollection[1];
        methodAccessModifier = AccessModifier.Protected;
        method = new CharacterMethod(methodName, methodDescription, methodAttribute, methodAccessModifier);
        MethodsManager.MethodsCollection.Add(method);

        // Method 3
        methodName = "MultipleJumps";
        methodDescription = "This is the DoubleJump method";
        methodAttribute = AttributesManager.AttributesCollection[2];
        methodAccessModifier = AccessModifier.Private;
        method = new CharacterMethod(methodName, methodDescription, methodAttribute, methodAccessModifier);
        MethodsManager.MethodsCollection.Add(method);

        // Method 4
        methodName = "FireballShoot";
        methodDescription = "This is the FireballShoot method";
        methodAttribute = AttributesManager.AttributesCollection[3];
        methodAccessModifier = AccessModifier.Public;
        method = new CharacterMethod(methodName, methodDescription, methodAttribute, methodAccessModifier);
        MethodsManager.MethodsCollection.Add(method);
    }
    private void InitializeSpecialAbilityCollection()
    {
        string abilityName;
        string abilityDescription;
        float abilityValue;
        SpecialAbility abilityType;
        CharacterSpecialAbility specialAbility;
        List<CharacterSpecialAbility> specialAbilities = new();

        // Special Ability 1 (General)
        abilityName = "General";
        abilityDescription = "Nothing Special";
        abilityValue = 0;
        abilityType = SpecialAbility.General;
        specialAbility = new CharacterSpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(abilityType, specialAbilities);

        // Special Ability 2 (Automatic)
        specialAbilities = new();
        abilityName = "Automatic";
        abilityDescription = "Automatically";
        abilityValue = 0;
        abilityType = SpecialAbility.Automatic;
        specialAbility = new CharacterSpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(abilityType, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbility.General].Add(specialAbility);

        // Special Ability 3 (Manual)
        specialAbilities = new();
        abilityName = "Manual";
        abilityDescription = "Manually";
        abilityValue = 0;
        abilityType = SpecialAbility.Manual;
        specialAbility = new CharacterSpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(abilityType, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbility.General].Add(specialAbility);

        // Special Ability 4 (Jump)
        specialAbilities = new();
        abilityName = "Jump";
        abilityDescription = "Base Jump";
        abilityValue = 1;
        abilityType = SpecialAbility.Jump;
        specialAbility = new CharacterSpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(abilityType, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbility.Automatic].Add(specialAbility);

        // Special Ability 5 (Gravity)
        specialAbilities = new();
        abilityName = "Gravity";
        abilityDescription = "Base Gravity";
        abilityValue = 1;
        abilityType = SpecialAbility.Gravity;
        specialAbility = new CharacterSpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(abilityType, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbility.Automatic].Add(specialAbility);

        // Special Ability 6 (Speed)
        specialAbilities = new();
        abilityName = "Speed";
        abilityDescription = "Base Speed";
        abilityValue = 1;
        abilityType = SpecialAbility.Speed;
        specialAbility = new CharacterSpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(abilityType, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbility.Automatic].Add(specialAbility);

        // Special Ability 7 (FireballShoot)
        specialAbilities = new();
        abilityName = "Fireball Shoot";
        abilityDescription = "Fireball Shoot";
        abilityValue = 0.25f;
        abilityType = SpecialAbility.FireballShoot;
        specialAbility = new CharacterSpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(abilityType, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbility.Manual].Add(specialAbility);

        // Special Ability 8 (DoubleJump)
        specialAbilities = new();
        abilityName = "Double Jump";
        abilityDescription = "Double Jump";
        abilityValue = 2;
        abilityType = SpecialAbility.DoubleJump;
        specialAbility = new CharacterSpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(abilityType, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbility.Jump].Add(specialAbility);

        // Special Ability 9 (WeakGravity)
        specialAbilities = new();
        abilityName = "Weak Gravity";
        abilityDescription = "Weak Gravity";
        abilityValue = 0.5f;
        abilityType = SpecialAbility.WeakGravity;
        specialAbility = new CharacterSpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(abilityType, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbility.Gravity].Add(specialAbility);

        // Special Ability 10 (FastSpeed)
        specialAbilities = new();
        abilityName = "Fast Speed";
        abilityDescription = "Fast Speed";
        abilityValue = 2;
        abilityType = SpecialAbility.FastSpeed;
        specialAbility = new CharacterSpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(abilityType, specialAbilities);
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
        Character character2 = new(characterName, characterDescription, characterParents, characterSpecialAbility, true, false);
        character2.Parents.ForEach(parent => parent.Childrens.Add(character2));
        InitializeCharacterObject(character2);
        CharactersManager.AddCharacter(character2);
        characterParents = new();

        // Character 3 
        characterName = "Character 3";
        characterDescription = "This is the third character";
        characterParents.Add(character1);
        characterSpecialAbility = SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbility.Manual][0];
        Character character3 = new(characterName, characterDescription, characterParents, characterSpecialAbility, true, false);
        character3.Parents.ForEach(parent => parent.Childrens.Add(character3));
        InitializeCharacterObject(character3);
        CharactersManager.AddCharacter(character3);
        characterParents = new();
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
