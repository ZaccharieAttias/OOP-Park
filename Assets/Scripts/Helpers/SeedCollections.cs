using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class SeedCollections : MonoBehaviour
{
    public AttributesManager AttributesManager;
    public MethodsManager MethodsManager;
    public CharactersManager CharactersManager;
    public SpecialAbilityManager SpecialAbilityManager;
    public CharacterEditor1 CharacterEditor;
    public JsonUtilityManager jsonUtilityManager;


    public void Start()
    {
        InitializeProperties();
        // InitializeCollections();
        jsonUtilityManager.Load();
    }
    private void InitializeProperties()
    {
        AttributesManager = GameObject.Find("Canvas/Popups").GetComponent<AttributesManager>();
        MethodsManager = GameObject.Find("Canvas/Popups").GetComponent<MethodsManager>();
        SpecialAbilityManager = GameObject.Find("Canvas/Popups").GetComponent<SpecialAbilityManager>();
        CharactersManager = GameObject.Find("Scripts/CharactersManager").GetComponent<CharactersManager>();
        CharacterEditor = GameObject.Find("Scripts/CharacterEditor").GetComponent<CharacterEditor1>();
        jsonUtilityManager = GameObject.Find("GameInitializer").GetComponent<JsonUtilityManager>();
    }
    private void InitializeCollections()
    {
        InitializeAttributesCollection();
        InitializeMethodsCollection();
        InitializeSpecialAbilityCollection();
        if (!RestrictionManager.Instance.OnlineBuild && !RestrictionManager.Instance.OnlineGame) InitializeCharactersCollection();

        // SceneManagement.GameplayInfo = BuildGameplayInfo();
    }

    public List<GameplayInfo> BuildGameplayInfo()
    {
        var chapterInfos = new List<ChapterInfo>();
        var chapter0 = new ChapterInfo
        {
            ChapterNumber = 0,
            Name = "Tutorial",
            LevelsInfo = new List<LevelInfo>
            {
                new() { LevelNumber = 1, Score = 0, Status = 0 },
                new() { LevelNumber = 2, Score = 0, Status = -1 },
                new() { LevelNumber = 3, Score = 0, Status = -1 },
            }
        };
        var chapter1 = new ChapterInfo
        {
            ChapterNumber = 1,
            Name = "Inheritance",
            LevelsInfo = new List<LevelInfo>
            {
                new() { LevelNumber = 1, Score = 0, Status = -1 },
                new() { LevelNumber = 2, Score = 0, Status = -1 },
                new() { LevelNumber = 3, Score = 0, Status = -1 },
                new() { LevelNumber = 4, Score = 0, Status = -1 },
            }
        };
        var chapter2 = new ChapterInfo
        {
            ChapterNumber = 2,
            Name = "Polymorphishm",
            LevelsInfo = new List<LevelInfo>
            {
                new() { LevelNumber = 1, Score = 0, Status = -1 },
                new() { LevelNumber = 2, Score = 0, Status = -1 },
                new() { LevelNumber = 3, Score = 0, Status = -1 },
                new() { LevelNumber = 4, Score = 0, Status = -1 },
                new() { LevelNumber = 5, Score = 0, Status = -1 },
            }
        };

        chapterInfos.Add(chapter0);
        chapterInfos.Add(chapter1);
        chapterInfos.Add(chapter2);

        List<GameplayInfo> gameplayInfo = new()
        {
            new GameplayInfo { ChapterInfos = chapterInfos },
            new GameplayInfo { ChapterInfos = new List<ChapterInfo>() }
        };
        return gameplayInfo;
    }
    // private void InitializeAttributesCollection()
    // {
    //     Attribute attribute;

    //     // Attribute 1
    //     attribute = new()
    //     {
    //         Owner = null,
    //         Name = "speed",
    //         Description = "Enables the character to move faster, granting the ability to react faster",
    //         Value = 2f,

    //         AccessModifier = AccessModifier.Public
    //     };
    //     AttributesManager.AttributesCollection.Add(attribute);

    //     // Attribute 2
    //     attribute = new()
    //     {
    //         Owner = null,
    //         Name = "gravity",
    //         Description = "Enables the character to jump higher, granting the ability to reach higher places",
    //         Value = 4f,

    //         AccessModifier = AccessModifier.Protected
    //     };
    //     AttributesManager.AttributesCollection.Add(attribute);

    //     // Attribute 3
    //     attribute = new()
    //     {
    //         Owner = null,
    //         Name = "multipleJumps",
    //         Description = "Enables the character to jump multiple times before landing, granting the ability to reach further places",
    //         Value = 1f,

    //         AccessModifier = AccessModifier.Private
    //     };
    //     AttributesManager.AttributesCollection.Add(attribute);

    //     // Attribute 4
    //     attribute = new()
    //     {
    //         Owner = null,
    //         Name = "fireballShoot",
    //         Description = "Enables the character to shoot fireballs, granting the ability to destory special obstacles",
    //         Value = 0.25f,

    //         AccessModifier = AccessModifier.Public
    //     };
    //     AttributesManager.AttributesCollection.Add(attribute);

    //     // Attribute 6
    //     attribute = new()
    //     {
    //         Owner = null,
    //         Name = "inverseGravity",
    //         Description = "Enables the character to inverse the gravity, granting the ability turn the game upside down",
    //         Value = -40f,

    //         AccessModifier = AccessModifier.Protected
    //     };
    //     AttributesManager.AttributesCollection.Add(attribute);

    //     // Attribute 7
    //     attribute = new()
    //     {
    //         Owner = null,
    //         Name = "wallJump",
    //         Description = "Enables the character to jump and slide from special walls, granting the ability to parkour",
    //         Value = 1f,

    //         AccessModifier = AccessModifier.Private
    //     };
    //     AttributesManager.AttributesCollection.Add(attribute);

    //     // Attribute 8
    //     attribute = new()
    //     {
    //         Owner = null,
    //         Name = "grabbing",
    //         Description = "Enables the character to grab objects, granting the ability to move objects",
    //         Value = 50f,

    //         AccessModifier = AccessModifier.Public
    //     };
    //     AttributesManager.AttributesCollection.Add(attribute);
    // }
    // private void InitializeMethodsCollection()
    // {
    //     Method method;

    //     // Method 1
    //     method = new()
    //     {
    //         Owner = null,
    //         Name = "Speed",
    //         Description = "Granting the character the ability to move faster and react faster\nThe method is manually activated by the keys [A, D]",
    //         Attribute = AttributesManager.AttributesCollection[0],

    //         AccessModifier = AccessModifier.Public
    //     };
    //     MethodsManager.MethodsCollection.Add(method);

    //     // Method 2
    //     method = new()
    //     {
    //         Owner = null,
    //         Name = "Gravity",
    //         Description = "Granting the character the ability to jump higher and reach higher places\nThe method is manually activated by the key [W]",
    //         Attribute = AttributesManager.AttributesCollection[1],

    //         AccessModifier = AccessModifier.Protected
    //     };
    //     MethodsManager.MethodsCollection.Add(method);

    //     // Method 3
    //     method = new()
    //     {
    //         Owner = null,
    //         Name = "MultipleJumps",
    //         Description = "Granting the character the ability to jump multiple times before landing and reach further places\nThe method is manually activated by the key [W]",
    //         Attribute = AttributesManager.AttributesCollection[2],

    //         AccessModifier = AccessModifier.Private
    //     };
    //     MethodsManager.MethodsCollection.Add(method);

    //     // Method 4
    //     method = new()
    //     {
    //         Owner = null,
    //         Name = "FireballShoot",
    //         Description = "Granting the character the ability to shoot fireballs and destory special obstacles\nThe method is manually activated by the key [Q]",
    //         Attribute = AttributesManager.AttributesCollection[3],

    //         AccessModifier = AccessModifier.Public
    //     };
    //     MethodsManager.MethodsCollection.Add(method);


    //     // Method 6
    //     method = new()
    //     {
    //         Owner = null,
    //         Name = "InverseGravity",
    //         Description = "Granting the character the ability to inverse the gravity and turn the game upside down\nThe method is automatically activated",
    //         Attribute = AttributesManager.AttributesCollection[4],

    //         AccessModifier = AccessModifier.Protected
    //     };
    //     MethodsManager.MethodsCollection.Add(method);

    //     // Method 7
    //     method = new()
    //     {
    //         Owner = null,
    //         Name = "WallJump",
    //         Description = "Granting the character the ability to jump and slide from special walls\nThe method is manually activated by the key [W, A, D]",
    //         Attribute = AttributesManager.AttributesCollection[5],

    //         AccessModifier = AccessModifier.Private
    //     };
    //     MethodsManager.MethodsCollection.Add(method);

    //     // Method 8
    //     method = new()
    //     {
    //         Owner = null,
    //         Name = "Grabbing",
    //         Description = "Granting the character the ability to grab objects and move them\nThe method is manually activated by the key [E]",
    //         Attribute = AttributesManager.AttributesCollection[6],

    //         AccessModifier = AccessModifier.Public
    //     };
    //     MethodsManager.MethodsCollection.Add(method);

    // }
    // private void InitializeSpecialAbilityCollection()
    // {
    //     string abilityName;
    //     string abilityDescription;
    //     float abilityValue;
    //     SpecialAbilityType abilityType;
    //     SpecialAbility specialAbility;
    //     List<SpecialAbility> specialAbilities = new();
    //     List<SpecialAbilityObject> specialAbilityObjects = new();


    //     // Special Ability 1 (General)
    //     abilityName = "General";
    //     abilityDescription = "Defining an common interface for the special abilities";
    //     abilityValue = 0;
    //     abilityType = SpecialAbilityType.General;
    //     specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
    //     specialAbilities.Add(specialAbility);
    //     SpecialAbilityManager.SpecialAbilitiesDictionary.Add(abilityType, specialAbilities);
    //     SpecialAbilityObject sp1 = new(specialAbility);
    //     sp1.Name = abilityName;
    //     specialAbilityObjects.Add(sp1);

    //     // Special Ability 2 (Automatic)
    //     specialAbilities = new();
    //     abilityName = "Automatic";
    //     abilityDescription = "Defining the automatic interface, which their special abilities will be automatically activated";
    //     abilityValue = 0;
    //     abilityType = SpecialAbilityType.Automatic;
    //     specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
    //     specialAbilities.Add(specialAbility);
    //     SpecialAbilityManager.SpecialAbilitiesDictionary.Add(abilityType, specialAbilities);
    //     SpecialAbilityManager.SpecialAbilitiesDictionary[SpecialAbilityType.General].Add(specialAbility);
    //     SpecialAbilityObject sp2 = new(specialAbility);
    //     sp2.Name = abilityName;
    //     sp1.Childrens.Add(sp2);
    //     sp2.Parent = sp1;
    //     specialAbilityObjects.Add(sp2);


    //     // Special Ability 3 (Manual)
    //     specialAbilities = new();
    //     abilityName = "Manual";
    //     abilityDescription = "Defining the manual interface, which their special abilities will be manually activated";
    //     abilityValue = 0;
    //     abilityType = SpecialAbilityType.Manual;
    //     specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
    //     specialAbilities.Add(specialAbility);
    //     SpecialAbilityManager.SpecialAbilitiesDictionary.Add(abilityType, specialAbilities);
    //     SpecialAbilityManager.SpecialAbilitiesDictionary[SpecialAbilityType.General].Add(specialAbility);
    //     SpecialAbilityObject sp3 = new(specialAbility);
    //     sp3.Name = abilityName;
    //     sp1.Childrens.Add(sp3);
    //     sp3.Parent = sp1;
    //     specialAbilityObjects.Add(sp3);


    //     // Special Ability 4 (Jump)
    //     specialAbilities = new();
    //     abilityName = "Jump";
    //     abilityDescription = "Defining the jump interface";
    //     abilityValue = 0;
    //     abilityType = SpecialAbilityType.Jump;
    //     specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
    //     specialAbilities.Add(specialAbility);
    //     SpecialAbilityManager.SpecialAbilitiesDictionary.Add(abilityType, specialAbilities);
    //     SpecialAbilityManager.SpecialAbilitiesDictionary[SpecialAbilityType.Automatic].Add(specialAbility);
    //     SpecialAbilityObject sp4 = new(specialAbility);
    //     sp4.Name = abilityName;
    //     sp2.Childrens.Add(sp4);
    //     sp4.Parent = sp2;
    //     specialAbilityObjects.Add(sp4);


    //     // Special Ability 5 (Gravity)
    //     specialAbilities = new();
    //     abilityName = "Gravity";
    //     abilityDescription = "Enhances the Gravity method by reducing further the gravity force";
    //     abilityValue = 3;
    //     abilityType = SpecialAbilityType.Gravity;
    //     specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
    //     specialAbilities.Add(specialAbility);
    //     SpecialAbilityManager.SpecialAbilitiesDictionary.Add(abilityType, specialAbilities);
    //     SpecialAbilityManager.SpecialAbilitiesDictionary[SpecialAbilityType.Automatic].Add(specialAbility);
    //     SpecialAbilityObject sp5 = new(specialAbility);
    //     sp5.Name = abilityName;
    //     sp2.Childrens.Add(sp5);
    //     sp5.Parent = sp2;
    //     specialAbilityObjects.Add(sp5);


    //     // Special Ability 6 (Speed)
    //     specialAbilities = new();
    //     abilityName = "Speed";
    //     abilityDescription = "Enhances the Speed method by increasing the speed force";
    //     abilityValue = 2;
    //     abilityType = SpecialAbilityType.Speed;
    //     specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
    //     specialAbilities.Add(specialAbility);
    //     SpecialAbilityManager.SpecialAbilitiesDictionary.Add(abilityType, specialAbilities);
    //     SpecialAbilityManager.SpecialAbilitiesDictionary[SpecialAbilityType.Automatic].Add(specialAbility);
    //     SpecialAbilityObject sp6 = new(specialAbility);
    //     sp6.Name = abilityName;
    //     sp2.Childrens.Add(sp6);
    //     sp6.Parent = sp2;
    //     specialAbilityObjects.Add(sp6);


    //     // Special Ability 7 (FireballShoot)
    //     specialAbilities = new();
    //     abilityName = "Fireball Shoot";
    //     abilityDescription = "Enhances the FireballShoot method by reducing the fireball rate of fire";
    //     abilityValue = 0.25f;
    //     abilityType = SpecialAbilityType.FireballShoot;
    //     specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
    //     specialAbilities.Add(specialAbility);
    //     SpecialAbilityManager.SpecialAbilitiesDictionary.Add(abilityType, specialAbilities);
    //     SpecialAbilityManager.SpecialAbilitiesDictionary[SpecialAbilityType.Manual].Add(specialAbility);
    //     SpecialAbilityObject sp7 = new(specialAbility);
    //     sp7.Name = abilityName;
    //     sp3.Childrens.Add(sp7);
    //     sp7.Parent = sp3;
    //     specialAbilityObjects.Add(sp7);


    //     // Special Ability 8 (DoubleJump)
    //     specialAbilities = new();
    //     abilityName = "Multiple Jumps";
    //     abilityDescription = "Enhances the MultipleJumps method by increasing the number of jumps";
    //     abilityValue = 1;
    //     abilityType = SpecialAbilityType.MultipleJumps;
    //     specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
    //     specialAbilities.Add(specialAbility);
    //     SpecialAbilityManager.SpecialAbilitiesDictionary.Add(abilityType, specialAbilities);
    //     SpecialAbilityManager.SpecialAbilitiesDictionary[SpecialAbilityType.Jump].Add(specialAbility);
    //     SpecialAbilityObject sp8 = new(specialAbility);
    //     sp8.Name = abilityName;
    //     sp4.Childrens.Add(sp8);
    //     sp8.Parent = sp4;
    //     specialAbilityObjects.Add(sp8);

    //     // Special Ability 12 (InverseGravity)
    //     specialAbilities = new();
    //     abilityName = "Inverse Gravity";
    //     abilityDescription = "Enhances the InverseGravity method by reducing the gravity force";
    //     abilityValue = -40;
    //     abilityType = SpecialAbilityType.InverseGravity;
    //     specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
    //     specialAbilities.Add(specialAbility);
    //     SpecialAbilityManager.SpecialAbilitiesDictionary.Add(abilityType, specialAbilities);
    //     SpecialAbilityManager.SpecialAbilitiesDictionary[SpecialAbilityType.Gravity].Add(specialAbility);
    //     SpecialAbilityObject sp12 = new(specialAbility);
    //     sp12.Name = abilityName;
    //     sp5.Childrens.Add(sp12);
    //     sp12.Parent = sp5;
    //     specialAbilityObjects.Add(sp12);


    //     // Special Ability 13 (WallJump)
    //     specialAbilities = new();
    //     abilityName = "Wall Jump";
    //     abilityDescription = "Enhances the WallJump method by increasing the wall jump amount";
    //     abilityValue = 1;
    //     abilityType = SpecialAbilityType.WallJump;
    //     specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
    //     specialAbilities.Add(specialAbility);
    //     SpecialAbilityManager.SpecialAbilitiesDictionary.Add(abilityType, specialAbilities);
    //     SpecialAbilityManager.SpecialAbilitiesDictionary[SpecialAbilityType.Jump].Add(specialAbility);
    //     SpecialAbilityObject sp13 = new(specialAbility);
    //     sp13.Name = abilityName;
    //     sp4.Childrens.Add(sp13);
    //     sp13.Parent = sp4;
    //     specialAbilityObjects.Add(sp13);


    //     // Special Ability 14 (Grabbing)
    //     specialAbilities = new();
    //     abilityName = "Grabbing";
    //     abilityDescription = "Enhances the Grabbing method by increasing the grabbing force";
    //     abilityValue = 10000;
    //     abilityType = SpecialAbilityType.Grabbing;
    //     specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
    //     specialAbilities.Add(specialAbility);
    //     SpecialAbilityManager.SpecialAbilitiesDictionary.Add(abilityType, specialAbilities);
    //     SpecialAbilityManager.SpecialAbilitiesDictionary[SpecialAbilityType.Manual].Add(specialAbility);
    //     SpecialAbilityObject sp14 = new(specialAbility);
    //     sp14.Name = abilityName;
    //     sp3.Childrens.Add(sp14);
    //     sp14.Parent = sp3;
    //     specialAbilityObjects.Add(sp14);


    //     SpecialAbilityManager.SpecialAbilitiesCollection = specialAbilityObjects;
    // }
    public void InitializeCharactersCollection()
    {
        string characterName;
        string characterDescription;
        CharacterB characterParent = null;
        SpecialAbility characterSpecialAbility;

        // Character 1
        characterName = "Character 1";
        characterDescription = "This is the first character";
        characterSpecialAbility = SpecialAbilityManager.SpecialAbilitiesDictionary[SpecialAbilityType.General].First();
        CharacterB character1 = new(characterName, characterDescription, characterParent, characterSpecialAbility, true, false);
        InitializeCharacterObject(character1);
        CharactersManager.AddCharacter(character1);

        // Character 2
        characterName = "Character 2";
        characterDescription = "This is the second character";
        characterParent = character1;
        characterSpecialAbility = SpecialAbilityManager.SpecialAbilitiesDictionary[SpecialAbilityType.Automatic].First();
        CharacterB character2 = new(characterName, characterDescription, characterParent, characterSpecialAbility, true, false);
        character2.Parent.Childrens.Add(character2);
        InitializeCharacterObject(character2);
        CharactersManager.AddCharacter(character2);
        characterParent = null;

        // Character 3
        characterName = "Character 3";
        characterDescription = "This is the third character";
        characterParent = character1;
        characterSpecialAbility = SpecialAbilityManager.SpecialAbilitiesDictionary[SpecialAbilityType.Manual].First();
        CharacterB character3 = new(characterName, characterDescription, characterParent, characterSpecialAbility, true, false);
        character3.Parent.Childrens.Add(character3);
        InitializeCharacterObject(character3);
        CharactersManager.AddCharacter(character3);
        characterParent = null;

        CharacterEditor.LoadFromJson();
    }
    private void InitializeCharacterObject(CharacterB characterNode)
    {
        Transform parnetTransform = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Tree/Buttons/Background/ScrollView/ViewPort/All").transform;
        GameObject characterPrefab = Resources.Load<GameObject>("Buttons/Character");

        GameObject newPlayerButton = Instantiate(characterPrefab, parnetTransform);
        newPlayerButton.name = characterNode.Name;
        characterNode.CharacterButton.Button = newPlayerButton;
        newPlayerButton.GetComponent<CharacterDetails>().InitializeCharacter(characterNode);

        Button button = newPlayerButton.GetComponent<Button>();
        button.onClick.AddListener(() => CharactersManager.DisplayCharacter(characterNode));

        Image image = newPlayerButton.GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>("Sprites/Characters/" + characterNode.Name);
    }

    private void InitializeAttributesCollection()
    {
        Attribute attribute;

        // Attribute 1
        attribute = new()
        {
            Owner = null,
            Name = "speed",
            Description = "Enables the character to move faster by 2 units, granting the ability to react faster",
            Value = 2f,

            AccessModifier = AccessModifier.Public
        };
        AttributesManager.AttributesCollection.Add(attribute);

        // Attribute 2
        attribute = new()
        {
            Owner = null,
            Name = "gravity",
            Description = "Enables the character to increase the gravity by 4 units, granting the ability to reach higher places",
            Value = 4f,

            AccessModifier = AccessModifier.Protected
        };
        AttributesManager.AttributesCollection.Add(attribute);

        // Attribute 3
        attribute = new()
        {
            Owner = null,
            Name = "multipleJumps",
            Description = "Enables the character to jump 1 more times before landing, granting the ability to reach further places",
            Value = 1f,

            AccessModifier = AccessModifier.Private
        };
        AttributesManager.AttributesCollection.Add(attribute);

        // Attribute 4
        attribute = new()
        {
            Owner = null,
            Name = "fireballShoot",
            Description = "Enables the character to shoot fireballs at rate of 0.25s, granting the ability to destory special obstacles",
            Value = 0.25f,

            AccessModifier = AccessModifier.Public
        };
        AttributesManager.AttributesCollection.Add(attribute);

        // Attribute 6
        attribute = new()
        {
            Owner = null,
            Name = "inverseGravity",
            Description = "Enables the character to inverse the gravity by -40 units, granting the ability turn the game upside down",
            Value = -40f,

            AccessModifier = AccessModifier.Protected
        };
        AttributesManager.AttributesCollection.Add(attribute);

        // Attribute 7
        attribute = new()
        {
            Owner = null,
            Name = "wallJump",
            Description = "Enables the character to jump 1 time from special walls, granting the ability to parkour",
            Value = 1f,

            AccessModifier = AccessModifier.Private
        };
        AttributesManager.AttributesCollection.Add(attribute);

        // Attribute 8
        attribute = new()
        {
            Owner = null,
            Name = "grabbing",
            Description = "Enables the character to grab objects that weight max of 50 units, granting the ability to move objects",
            Value = 50f,

            AccessModifier = AccessModifier.Public
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
            Description = "Granting the character the ability to move faster by 2 units and react faster\nThe method is manually activated by the keys [A] and [D]",
            Attribute = AttributesManager.AttributesCollection[0],

            AccessModifier = AccessModifier.Public
        };
        MethodsManager.MethodsCollection.Add(method);

        // Method 2
        method = new()
        {
            Owner = null,
            Name = "Gravity",
            Description = "Granting the character the ability to jump higher by 4 units and reach higher places\nThe method is manually activated by the key [W]",
            Attribute = AttributesManager.AttributesCollection[1],

            AccessModifier = AccessModifier.Protected
        };
        MethodsManager.MethodsCollection.Add(method);

        // Method 3
        method = new()
        {
            Owner = null,
            Name = "MultipleJumps",
            Description = "Granting the character the ability to jump 1 more times before landing and reach further places\nThe method is manually activated by the key [W]",
            Attribute = AttributesManager.AttributesCollection[2],

            AccessModifier = AccessModifier.Private
        };
        MethodsManager.MethodsCollection.Add(method);

        // Method 4
        method = new()
        {
            Owner = null,
            Name = "FireballShoot",
            Description = "Granting the character the ability to shoot fireballs at rate of 0.25s and destory special obstacles\nThe method is manually activated by the key [Q]",
            Attribute = AttributesManager.AttributesCollection[3],

            AccessModifier = AccessModifier.Public
        };
        MethodsManager.MethodsCollection.Add(method);


        // Method 6
        method = new()
        {
            Owner = null,
            Name = "InverseGravity",
            Description = "Granting the character the ability to inverse the gravity by -40 units and turn the game upside down\nThe method is automatically activated",
            Attribute = AttributesManager.AttributesCollection[4],

            AccessModifier = AccessModifier.Protected
        };
        MethodsManager.MethodsCollection.Add(method);

        // Method 7
        method = new()
        {
            Owner = null,
            Name = "WallJump",
            Description = "Granting the character the ability to jump 1 time from special walls\nThe method is manually activated by the key [W], [A], [D]",
            Attribute = AttributesManager.AttributesCollection[5],

            AccessModifier = AccessModifier.Private
        };
        MethodsManager.MethodsCollection.Add(method);

        // Method 8
        method = new()
        {
            Owner = null,
            Name = "Grabbing",
            Description = "Granting the character the ability to grab objects that weight max of 50 units and move them\nThe method is manually activated by the key [E]",
            Attribute = AttributesManager.AttributesCollection[6],

            AccessModifier = AccessModifier.Public
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
        List<SpecialAbilityObject> specialAbilityObjects = new();


        // Special Ability 1 (General)
        abilityName = "General";
        abilityDescription = "Defining an common interface for the special abilities";
        abilityValue = 0;
        abilityType = SpecialAbilityType.General;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilityManager.SpecialAbilitiesDictionary.Add(abilityType, specialAbilities);
        SpecialAbilityObject sp1 = new(specialAbility);
        sp1.Name = abilityName;
        specialAbilityObjects.Add(sp1);

        // Special Ability 2 (Automatic)
        specialAbilities = new();
        abilityName = "Automatic";
        abilityDescription = "Defining the automatic interface, which their special abilities will be automatically activated";
        abilityValue = 0;
        abilityType = SpecialAbilityType.Automatic;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilityManager.SpecialAbilitiesDictionary.Add(abilityType, specialAbilities);
        SpecialAbilityManager.SpecialAbilitiesDictionary[SpecialAbilityType.General].Add(specialAbility);
        SpecialAbilityObject sp2 = new(specialAbility);
        sp2.Name = abilityName;
        sp1.Childrens.Add(sp2);
        sp2.Parent = sp1;
        specialAbilityObjects.Add(sp2);


        // Special Ability 3 (Manual)
        specialAbilities = new();
        abilityName = "Manual";
        abilityDescription = "Defining the manual interface, which their special abilities will be manually activated";
        abilityValue = 0;
        abilityType = SpecialAbilityType.Manual;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilityManager.SpecialAbilitiesDictionary.Add(abilityType, specialAbilities);
        SpecialAbilityManager.SpecialAbilitiesDictionary[SpecialAbilityType.General].Add(specialAbility);
        SpecialAbilityObject sp3 = new(specialAbility);
        sp3.Name = abilityName;
        sp1.Childrens.Add(sp3);
        sp3.Parent = sp1;
        specialAbilityObjects.Add(sp3);


        // Special Ability 4 (Jump)
        specialAbilities = new();
        abilityName = "Jump";
        abilityDescription = "Defining the jump interface";
        abilityValue = 0;
        abilityType = SpecialAbilityType.Jump;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilityManager.SpecialAbilitiesDictionary.Add(abilityType, specialAbilities);
        SpecialAbilityManager.SpecialAbilitiesDictionary[SpecialAbilityType.Automatic].Add(specialAbility);
        SpecialAbilityObject sp4 = new(specialAbility);
        sp4.Name = abilityName;
        sp2.Childrens.Add(sp4);
        sp4.Parent = sp2;
        specialAbilityObjects.Add(sp4);


        // Special Ability 5 (Gravity)
        specialAbilities = new();
        abilityName = "Gravity";
        abilityDescription = "Enhances the Gravity method by reducing further the gravity force of 3 units";
        abilityValue = 3;
        abilityType = SpecialAbilityType.Gravity;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilityManager.SpecialAbilitiesDictionary.Add(abilityType, specialAbilities);
        SpecialAbilityManager.SpecialAbilitiesDictionary[SpecialAbilityType.Automatic].Add(specialAbility);
        SpecialAbilityObject sp5 = new(specialAbility);
        sp5.Name = abilityName;
        sp2.Childrens.Add(sp5);
        sp5.Parent = sp2;
        specialAbilityObjects.Add(sp5);


        // Special Ability 6 (Speed)
        specialAbilities = new();
        abilityName = "Speed";
        abilityDescription = "Enhances the Speed method by increasing the speed force by 2 units";
        abilityValue = 2;
        abilityType = SpecialAbilityType.Speed;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilityManager.SpecialAbilitiesDictionary.Add(abilityType, specialAbilities);
        SpecialAbilityManager.SpecialAbilitiesDictionary[SpecialAbilityType.Automatic].Add(specialAbility);
        SpecialAbilityObject sp6 = new(specialAbility);
        sp6.Name = abilityName;
        sp2.Childrens.Add(sp6);
        sp6.Parent = sp2;
        specialAbilityObjects.Add(sp6);


        // Special Ability 7 (FireballShoot)
        specialAbilities = new();
        abilityName = "Fireball Shoot";
        abilityDescription = "Enhances the FireballShoot method by reducing the fireball rate of fire by 0.25 units";
        abilityValue = 0.25f;
        abilityType = SpecialAbilityType.FireballShoot;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilityManager.SpecialAbilitiesDictionary.Add(abilityType, specialAbilities);
        SpecialAbilityManager.SpecialAbilitiesDictionary[SpecialAbilityType.Manual].Add(specialAbility);
        SpecialAbilityObject sp7 = new(specialAbility);
        sp7.Name = abilityName;
        sp3.Childrens.Add(sp7);
        sp7.Parent = sp3;
        specialAbilityObjects.Add(sp7);


        // Special Ability 8 (DoubleJump)
        specialAbilities = new();
        abilityName = "Multiple Jumps";
        abilityDescription = "Enhances the MultipleJumps method by increasing the number of jumps by 1";
        abilityValue = 1;
        abilityType = SpecialAbilityType.MultipleJumps;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilityManager.SpecialAbilitiesDictionary.Add(abilityType, specialAbilities);
        SpecialAbilityManager.SpecialAbilitiesDictionary[SpecialAbilityType.Jump].Add(specialAbility);
        SpecialAbilityObject sp8 = new(specialAbility);
        sp8.Name = abilityName;
        sp4.Childrens.Add(sp8);
        sp8.Parent = sp4;
        specialAbilityObjects.Add(sp8);

        // Special Ability 12 (InverseGravity)
        specialAbilities = new();
        abilityName = "Inverse Gravity";
        abilityDescription = "Enhances the InverseGravity method by reducing the gravity force by 40 units";
        abilityValue = -40;
        abilityType = SpecialAbilityType.InverseGravity;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilityManager.SpecialAbilitiesDictionary.Add(abilityType, specialAbilities);
        SpecialAbilityManager.SpecialAbilitiesDictionary[SpecialAbilityType.Gravity].Add(specialAbility);
        SpecialAbilityObject sp12 = new(specialAbility);
        sp12.Name = abilityName;
        sp5.Childrens.Add(sp12);
        sp12.Parent = sp5;
        specialAbilityObjects.Add(sp12);


        // Special Ability 13 (WallJump)
        specialAbilities = new();
        abilityName = "Wall Jump";
        abilityDescription = "Enhances the WallJump method by increasing the wall jump amount";
        abilityValue = 1;
        abilityType = SpecialAbilityType.WallJump;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilityManager.SpecialAbilitiesDictionary.Add(abilityType, specialAbilities);
        SpecialAbilityManager.SpecialAbilitiesDictionary[SpecialAbilityType.Jump].Add(specialAbility);
        SpecialAbilityObject sp13 = new(specialAbility);
        sp13.Name = abilityName;
        sp4.Childrens.Add(sp13);
        sp13.Parent = sp4;
        specialAbilityObjects.Add(sp13);


        // Special Ability 14 (Grabbing)
        specialAbilities = new();
        abilityName = "Grabbing";
        abilityDescription = "Enhances the Grabbing method by increasing the grabbing force by 10000 weight units";
        abilityValue = 10000;
        abilityType = SpecialAbilityType.Grabbing;
        specialAbility = new SpecialAbility(abilityName, abilityDescription, abilityValue, abilityType);
        specialAbilities.Add(specialAbility);
        SpecialAbilityManager.SpecialAbilitiesDictionary.Add(abilityType, specialAbilities);
        SpecialAbilityManager.SpecialAbilitiesDictionary[SpecialAbilityType.Manual].Add(specialAbility);
        SpecialAbilityObject sp14 = new(specialAbility);
        sp14.Name = abilityName;
        sp3.Childrens.Add(sp14);
        sp14.Parent = sp3;
        specialAbilityObjects.Add(sp14);


        SpecialAbilityManager.SpecialAbilitiesCollection = specialAbilityObjects;
    }
}
