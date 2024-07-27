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


    public void Start()
    {
        InitializeProperties();
        InitializeCollections();
    }
    private void InitializeProperties()
    {
        AttributesManager = GameObject.Find("Canvas/Popups").GetComponent<AttributesManager>();
        MethodsManager = GameObject.Find("Canvas/Popups").GetComponent<MethodsManager>();
        SpecialAbilityManager = GameObject.Find("Canvas/Popups").GetComponent<SpecialAbilityManager>();
        CharactersManager = GameObject.Find("Scripts/CharactersManager").GetComponent<CharactersManager>();
        CharacterEditor = GameObject.Find("Scripts/CharacterEditor").GetComponent<CharacterEditor1>();
    }
    private void InitializeCollections()
    {
        InitializeAttributesCollection();
        InitializeMethodsCollection();
        InitializeSpecialAbilityCollection();
        if (!RestrictionManager.Instance.OnlineBuild && !RestrictionManager.Instance.OnlineGame) InitializeCharactersCollection();

        SceneManagement.GameplayInfo = BuildGameplayInfo();
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
                new LevelInfo { LevelNumber = 1, Status = 1 },
                new LevelInfo { LevelNumber = 2, Status = 1 },
                new LevelInfo { LevelNumber = 3, Status = 1 },
                new LevelInfo { LevelNumber = 4, Status = 1 },
                new LevelInfo { LevelNumber = 5, Status = 1 },
            }
        };
        var chapter1 = new ChapterInfo
        {
            ChapterNumber = 1,
            Name = "Inheritance",
            LevelsInfo = new List<LevelInfo>
            {
                new LevelInfo { LevelNumber = 1, Status = 1 },
                new LevelInfo { LevelNumber = 2, Status = 1 },
                new LevelInfo { LevelNumber = 3, Status = 0 },
                new LevelInfo { LevelNumber = 4, Status = -1 },
                new LevelInfo { LevelNumber = 5, Status = -1 },
            }
        };
        var chapter2 = new ChapterInfo
        {
            ChapterNumber = 2,
            Name = "Polymorphishm",
            LevelsInfo = new List<LevelInfo>
            {
                new LevelInfo { LevelNumber = 1, Status = -1 },
                new LevelInfo { LevelNumber = 2, Status = -1 },
                new LevelInfo { LevelNumber = 3, Status = -1 },
                new LevelInfo { LevelNumber = 4, Status = -1 },
                new LevelInfo { LevelNumber = 5, Status = -1 },
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


    private void InitializeAttributesCollection()
    {
        Attribute attribute;

        // Attribute 1
        attribute = new()
        {
            Owner = null,
            Name = "speed",
            Description = "This is the MoveSpeed attribute",
            Value = 2f,

            AccessModifier = AccessModifier.Public
        };
        AttributesManager.AttributesCollection.Add(attribute);

        // Attribute 2
        attribute = new()
        {
            Owner = null,
            Name = "gravity",
            Description = "This is the GravityForce attribute",
            Value = 4f,

            AccessModifier = AccessModifier.Protected
        };
        AttributesManager.AttributesCollection.Add(attribute);

        // Attribute 3
        attribute = new()
        {
            Owner = null,
            Name = "multipleJumps",
            Description = "This is the DoubleJump attribute",
            Value = 1f,

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

        // Attribute 8
        attribute = new()
        {
            Owner = null,
            Name = "grabbing",
            Description = "This is the grabbing attribute",
            Value = 20f,

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


        // Method 6
        method = new()
        {
            Owner = null,
            Name = "InverseGravity",
            Description = "This is the InverseGravity method",
            Attribute = AttributesManager.AttributesCollection[4],

            AccessModifier = AccessModifier.Protected
        };
        MethodsManager.MethodsCollection.Add(method);

        // Method 7
        method = new()
        {
            Owner = null,
            Name = "WallJump",
            Description = "This is the WallJump method",
            Attribute = AttributesManager.AttributesCollection[5],

            AccessModifier = AccessModifier.Private
        };
        MethodsManager.MethodsCollection.Add(method);

        // Method 8
        method = new()
        {
            Owner = null,
            Name = "Grabbing",
            Description = "This is the grabbing method",
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
        abilityDescription = "Nothing Special";
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
        abilityDescription = "Automatically";
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
        abilityDescription = "Manually";
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
        abilityDescription = "Base Jump";
        abilityValue = 1;
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
        abilityDescription = "Base Gravity";
        abilityValue = 1;
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
        abilityDescription = "Base Speed";
        abilityValue = 1;
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
        abilityDescription = "Fireball Shoot";
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
        abilityDescription = "Multiple Jump";
        abilityValue = 2;
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
        abilityDescription = "Inverse Gravity";
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
        abilityDescription = "Wall Jump";
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
        abilityDescription = "Grabbing";
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
}
