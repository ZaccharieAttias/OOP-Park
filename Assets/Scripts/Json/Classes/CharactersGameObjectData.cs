using System.Collections.Generic;
using UnityEngine;



public static class CharactersGameObjectData
{
    public static CharacterCreationManager CharacterCreationManager;
    public static CharacterManager CharacterManager;


    public static void Initialize()
    {
        CharacterCreationManager = GameObject.Find("Canvas/Popups").GetComponent<CharacterCreationManager>();
        CharacterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
    }


    public static void Load()
    {
        List<Character> characters = CharactersData.CharactersCollection;
        CharactersData.CharactersCollection.Clear();

        foreach (var character in characters)
        {
            CharacterCreationManager.BuildCharacterObject(character);
            CharacterManager.AddCharacter(character);
        }
    }
}
