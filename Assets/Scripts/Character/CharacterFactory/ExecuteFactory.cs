using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ExecuteFactory : MonoBehaviour
{   
    public CharacterFactory CharacterFactory { get; set; }
    public CharacterManager CharacterManager { get; set; }

    public GameObject CharacterPrefab { get; set; }
    public Transform CharacterParent { get; set; }
    public Sprite[] CharacterSprites { get; set; }
    public int SpriteIndex { get; set; }


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

        CharacterPrefab = Resources.Load<GameObject>("Prefabs/Buttons/Character");
        CharacterParent = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/Tree/All").transform;
        CharacterSprites = Resources.LoadAll<Sprite>("Sprites/Characters/");
        SpriteIndex = 0;
    }

    public void Execute()
    {
        Character builtCharacter = BuildCharacter();
        BuildCharacterObject(builtCharacter);

        CharacterManager.AddCharacter(builtCharacter);
    }

    private Character BuildCharacter()
    {
        int characterIndex = CharacterFactory.CharacterObjects.Count + 1;
        string characterName = $"Character {characterIndex}";
        string characterDescription = $"I`m {characterName}";
        List<Character> parents = CharacterFactory.SelectedCharacterObjects;

        Character builtCharacter = new Character(characterName, characterDescription, parents, false);
        builtCharacter.parents.ForEach(parent => parent.childrens.Add(builtCharacter));

        return builtCharacter;
    }

    private void BuildCharacterObject(Character character)
    {
        GameObject characterObject = Instantiate(CharacterPrefab, CharacterParent);

        characterObject.name = character.name;
        character.CharacterButton = characterObject;
        characterObject.GetComponent<CharacterDetails>().InitializeCharacter(character);

        Button button = characterObject.GetComponent<Button>();
        button.onClick.AddListener(() => CharacterManager.DisplayCharacterDetails(character.name)); // Change it to set current Character and from there its somehow change the details

        Image image = characterObject.GetComponent<Image>();
        image.sprite = CharacterSprites[SpriteIndex % CharacterSprites.Length];
        SpriteIndex++;
    }
}
