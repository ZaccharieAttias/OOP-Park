using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class ExecuteFactory : MonoBehaviour
{   
    public CharacterFactory CharacterFactory;
    public CharacterManager CharacterManager;

    public GameObject CharacterPrefab;
    public Transform CharacterParent;
    public List<Sprite> CharacterSprites;
    public int SpriteIndex;

    public SpecialAbilityManager abilityPopup;

    public void Start()
    { 
        InitializeGameObject(); 
        InitializeProperties();    
    }
    private void InitializeGameObject()
    {
        GameObject characterFactory = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/CharacterFactory");
        transform.SetParent(characterFactory.transform);
    }
    private void InitializeProperties()
    {
        GameObject characterFactory = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/CharacterFactory");
        CharacterFactory = characterFactory.GetComponent<CharacterFactory>();

        GameObject characterManager = GameObject.Find("Player");
        CharacterManager = characterManager.GetComponent<CharacterManager>();

        CharacterPrefab = Resources.Load<GameObject>("Buttons/Character");
        CharacterParent = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/ScrollView/ViewPort/All").transform;
        CharacterSprites = Resources.LoadAll<Sprite>("Sprites/Characters/").ToList();
        SpriteIndex = 0;

        abilityPopup = GameObject.Find("Canvas/HTMenu/Popups/SpecialAbility").GetComponent<SpecialAbilityManager>();
    }

    public void ExecuteTemp()
    {
        CharacterManager.CharactersCollection.Last().SpecialAbility = abilityPopup.selectedAbility;
    }

    public void Execute()
    {
        Character builtCharacter = BuildCharacter();
        BuildCharacterObject(builtCharacter);

        CharacterManager.AddCharacter(builtCharacter);

        abilityPopup.SelectAbility(CharacterFactory.SelectedCharacterObjects);
    }

    private Character BuildCharacter()
    {
        int characterIndex = CharacterFactory.CharacterObjects.Count + 1;
        string characterName = $"Character {characterIndex}";
        string characterDescription = $"I`m {characterName}";
        List<Character> parents = CharacterFactory.SelectedCharacterObjects;

        Character builtCharacter = new(characterName, characterDescription, parents, null, false);
        builtCharacter.Parents.ForEach(parent => parent.Childrens.Add(builtCharacter));
        
        return builtCharacter;
    }

    private void BuildCharacterObject(Character character)
    {
        GameObject characterObject = Instantiate(CharacterPrefab, CharacterParent);

        characterObject.name = character.Name;
        character.CharacterButton.Button = characterObject;
        characterObject.GetComponent<CharacterDetails>().InitializeCharacter(character);

        Button button = characterObject.GetComponent<Button>();
        button.onClick.AddListener(() => CharacterManager.DisplayCharacter(character));

        Image image = characterObject.GetComponent<Image>();
        image.sprite = CharacterSprites[SpriteIndex % CharacterSprites.Count];
        SpriteIndex++;
    }
}
