using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public static class CharactersGameObjectData
{
    public static CharacterCreationManager CharacterCreationManager;
    public static CharacterManager CharacterManager;
    public static CharacterTreeManager TreeBuilder;


    public static void Initialize()
    {
        CharacterCreationManager = GameObject.Find("Canvas/Popups").GetComponent<CharacterCreationManager>();
        CharacterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
        TreeBuilder = GameObject.Find("Canvas/HTMenu").GetComponent<CharacterTreeManager>();

    }


    public static void Load()
    {
        foreach (var character in CharactersData.CharacterManager.CharactersCollection)
            CharacterCreationManager.BuildCharacterObject(character);


        TreeBuilder.BuildTree(CharactersData.CharacterManager.CharactersCollection.First(), CharactersData.CharacterManager.CharactersCollection.Last());
        CharacterManager.DisplayCharacter(CharactersData.CharacterManager.CharactersCollection.Last());
    }
}
