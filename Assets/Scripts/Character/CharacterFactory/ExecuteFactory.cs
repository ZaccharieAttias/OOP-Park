using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


[SerializeField]
public class ExecuteFactory : MonoBehaviour
{   
    public CharacterFactory CharacterFactory;
    public CharacterManager CharacterManager;

    public GameObject CharacterPrefab;
    public Transform CharacterParent;
    public List<Sprite> CharacterSprites;
    public int SpriteIndex;


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
        CharacterSprites = Resources.LoadAll<Sprite>("Sprites/Characters/").ToList();
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

        Character builtCharacter = new(characterName, characterDescription, parents, false);
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
        button.onClick.AddListener(() => CharacterManager.DisplayCharacterDetails(character.Name)); // Change it to set current Character and from there its somehow change the details

        Image image = characterObject.GetComponent<Image>();
        image.sprite = CharacterSprites[SpriteIndex % CharacterSprites.Count];
        SpriteIndex++;
    }
}
