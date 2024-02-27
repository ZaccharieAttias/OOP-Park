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
        Attribute attribute;

        // Attribute 1
        attribute = new()
        {
           Owner = null,
            Name = "speed",
            Description = "This is the MoveSpeed attribute",
            Value = 5f,

            AccessModifier = AccessModifier.Public
        };
        AttributesManager.AttributesCollection.Add(attribute);

        // Attribute 2
        attribute = new()
        {
            Owner = null,
            Name = "gravity",
            Description = "This is the GravityForce attribute",
            Value = 20f,

            AccessModifier = AccessModifier.Protected
        };
        AttributesManager.AttributesCollection.Add(attribute);

        // Attribute 3
        attribute = new()
        {
            Owner = null,
            Name = "multipleJumps",
            Description = "This is the DoubleJump attribute",
            Value = 2f,

            AccessModifier = AccessModifier.Private
        };
        AttributesManager.AttributesCollection.Add(attribute);

        // Attribute 4
        attribute = new()
        {
            Owner = null,
            Name = "fireballShoot",
            Description = "This is the FireballShoot attribute",
            Value = 0.25f,

            AccessModifier = AccessModifier.Public
        };
        AttributesManager.AttributesCollection.Add(attribute);

        // Attribute 5
        attribute = new()
        {
            Owner = null,
            Name = "grapplingGun",
            Description = "This is the GrapplingGun attribute",
            Value = 10f,

            AccessModifier = AccessModifier.Public
        };
        AttributesManager.AttributesCollection.Add(attribute);

        // Attribute 6
        attribute = new()
        {
            Owner = null,
            Name = "inverseGravity",
            Description = "This is the InverseGravity attribute",
            Value = -40f,

            AccessModifier = AccessModifier.Protected
        };
        AttributesManager.AttributesCollection.Add(attribute);

        // Attribute 7
        attribute = new()
        {
            Owner = null,
            Name = "wallJump",
            Description = "This is the WallJump attribute",
            Value = 1f,

            AccessModifier = AccessModifier.Private
        };
        AttributesManager.AttributesCollection.Add(attribute); 
    }
    private void InitializeMethodsCollection()
    {
        Method method;

        // Method 1
        method = new()
        {
            Owner = null,
            Name = "Speed",
            Description = "This is the MoveSpeed method",
            Attribute = AttributesManager.AttributesCollection[0],

            AccessModifier = AccessModifier.Public
        };
        MethodsManager.MethodsCollection.Add(method);
        
        // Method 2
        method = new()
        {
            Owner = null,
            Name = "Gravity",
            Description = "This is the GravityForce method",
            Attribute = AttributesManager.AttributesCollection[1],

            AccessModifier = AccessModifier.Protected
        };
        MethodsManager.MethodsCollection.Add(method);

        // Method 3
        method = new()
        {
            Owner = null,
            Name = "MultipleJumps",
            Description = "This is the DoubleJump method",
            Attribute = AttributesManager.AttributesCollection[2],

            AccessModifier = AccessModifier.Private
        };
        MethodsManager.MethodsCollection.Add(method);

        // Method 4
        method = new()
        {
            Owner = null,
            Name = "FireballShoot",
            Description = "This is the FireballShoot method",
            Attribute = AttributesManager.AttributesCollection[3],

            AccessModifier = AccessModifier.Public
        };
        MethodsManager.MethodsCollection.Add(method);

        // Method 5
        method = new()
        {
            Owner = null,
            Name = "GrapplingGun",
            Description = "This is the GrapplingGun method",
            Attribute = AttributesManager.AttributesCollection[4],

            AccessModifier = AccessModifier.Public
        };
        MethodsManager.MethodsCollection.Add(method);

        // Method 6
        method = new()
        {
            Owner = null,
            Name = "InverseGravity",
            Description = "This is the InverseGravity method",
            Attribute = AttributesManager.AttributesCollection[5],

            AccessModifier = AccessModifier.Protected
        };
        MethodsManager.MethodsCollection.Add(method);

        // Method 7
        method = new()
        {
            Owner = null,
            Name = "WallJump",
            Description = "This is the WallJump method",
            Attribute = AttributesManager.AttributesCollection[6],

            AccessModifier = AccessModifier.Private
        };
        MethodsManager.MethodsCollection.Add(method);
    }
    private void InitializeSpecialAbilityCollection()
    {
        string abilityName;
        string abilityDescription;
        float abilityValue;
        SpecialAbilityType abilityType;
        SpecialAbility specialAbility;
        List<SpecialAbility> specialAbilities = new();

        // Special Ability 1 (General)
        abilityName = "General";
        abilityDescription = "Nothing Special";
        abilityValue = 0;
        abilityType = SpecialAbilityType.General;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(abilityType, specialAbilities);

        // Special Ability 2 (Automatic)
        specialAbilities = new();
        abilityName = "Automatic";
        abilityDescription = "Automatically";
        abilityValue = 0;
        abilityType = SpecialAbilityType.Automatic;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(abilityType, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbilityType.General].Add(specialAbility);

        // Special Ability 3 (Manual)
        specialAbilities = new();
        abilityName = "Manual";
        abilityDescription = "Manually";
        abilityValue = 0;
        abilityType = SpecialAbilityType.Manual;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(abilityType, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbilityType.General].Add(specialAbility);

        // Special Ability 4 (Jump)
        specialAbilities = new();
        abilityName = "Jump";
        abilityDescription = "Base Jump";
        abilityValue = 1;
        abilityType = SpecialAbilityType.Jump;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(abilityType, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbilityType.Automatic].Add(specialAbility);

        // Special Ability 5 (Gravity)
        specialAbilities = new();
        abilityName = "Gravity";
        abilityDescription = "Base Gravity";
        abilityValue = 1;
        abilityType = SpecialAbilityType.Gravity;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(abilityType, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbilityType.Automatic].Add(specialAbility);

        // Special Ability 6 (Speed)
        specialAbilities = new();
        abilityName = "Speed";
        abilityDescription = "Base Speed";
        abilityValue = 1;
        abilityType = SpecialAbilityType.Speed;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(abilityType, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbilityType.Automatic].Add(specialAbility);

        // Special Ability 7 (FireballShoot)
        specialAbilities = new();
        abilityName = "Fireball Shoot";
        abilityDescription = "Fireball Shoot";
        abilityValue = 0.25f;
        abilityType = SpecialAbilityType.FireballShoot;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(abilityType, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbilityType.Manual].Add(specialAbility);

        // Special Ability 8 (DoubleJump)
        specialAbilities = new();
        abilityName = "Double Jump";
        abilityDescription = "Double Jump";
        abilityValue = 2;
        abilityType = SpecialAbilityType.DoubleJump;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(abilityType, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbilityType.Jump].Add(specialAbility);

        // Special Ability 9 (WeakGravity)
        specialAbilities = new();
        abilityName = "Weak Gravity";
        abilityDescription = "Weak Gravity";
        abilityValue = 0.5f;
        abilityType = SpecialAbilityType.WeakGravity;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(abilityType, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbilityType.Gravity].Add(specialAbility);

        // Special Ability 10 (FastSpeed)
        specialAbilities = new();
        abilityName = "Fast Speed";
        abilityDescription = "Fast Speed";
        abilityValue = 2;
        abilityType = SpecialAbilityType.FastSpeed;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(abilityType, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbilityType.Speed].Add(specialAbility);

        // Special Ability 11 (GrapplingGun)
        specialAbilities = new();
        abilityName = "Grappling Gun";
        abilityDescription = "Grappling Gun";
        abilityValue = 10;
        abilityType = SpecialAbilityType.GrapplingGun;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(abilityType, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbilityType.Automatic].Add(specialAbility);

        // Special Ability 12 (InverseGravity)
        specialAbilities = new();
        abilityName = "Inverse Gravity";
        abilityDescription = "Inverse Gravity";
        abilityValue = -40;
        abilityType = SpecialAbilityType.InverseGravity;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(abilityType, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbilityType.Gravity].Add(specialAbility);

        // Special Ability 13 (WallJump)
        specialAbilities = new();
        abilityName = "Wall Jump";
        abilityDescription = "Wall Jump";
        abilityValue = 1;
        abilityType = SpecialAbilityType.WallJump;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilitiesManager.SpecialAbilitiesCollection.Add(abilityType, specialAbilities);
        SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbilityType.Jump].Add(specialAbility);

    }
    public void InitializeCharactersCollection()
    {
        string characterName;
        string characterDescription;
        List<Character> characterParents = new();
        SpecialAbility characterSpecialAbility;

        // Character 1 
        characterName = "Character 1";
        characterDescription = "This is the first character";
        characterSpecialAbility = SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbilityType.General][0];
        Character character1 = new(characterName, characterDescription, characterParents, characterSpecialAbility, true, false);
        InitializeCharacterObject(character1);
        CharactersManager.AddCharacter(character1);

        // Character 2 
        characterName = "Character 2";
        characterDescription = "This is the second character";
        characterParents.Add(character1);
        characterSpecialAbility = SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbilityType.Automatic][0];
        Character character2 = new(characterName, characterDescription, characterParents, characterSpecialAbility, true, false);
        character2.Parents.ForEach(parent => parent.Childrens.Add(character2));
        InitializeCharacterObject(character2);
        CharactersManager.AddCharacter(character2);
        characterParents = new();

        // Character 3 
        characterName = "Character 3";
        characterDescription = "This is the third character";
        characterParents.Add(character1);
        characterSpecialAbility = SpecialAbilitiesManager.SpecialAbilitiesCollection[SpecialAbilityType.Manual][0];
        Character character3 = new(characterName, characterDescription, characterParents, characterSpecialAbility, true, false);
        character3.Parents.ForEach(parent => parent.Childrens.Add(character3));
        InitializeCharacterObject(character3);
        CharactersManager.AddCharacter(character3);
        characterParents = new();
    }
    private void InitializeCharacterObject(Character characterNode)
    {
        Transform parnetTransform = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Tree/Buttons/ScrollView/ViewPort/All").transform;
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
