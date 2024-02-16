using System.Linq;
using UnityEngine;


public static class CharactersGameObjectData
{
    public static CharactersManager CharactersManager;
    public static CharactersTreeManager CharactersTreeManager;
    public static CharactersCreationManager CharactersCreationManager;


    public static void Initialize()
    {
        CharactersManager = CharactersData.CharactersManager;
        CharactersTreeManager = CharactersManager.TreeBuilder;
        CharactersCreationManager = GameObject.Find("Canvas/Popups").GetComponent<CharactersCreationManager>();    
    }


    public static void Load()
    {
        foreach (var character in CharactersManager.CharactersCollection) CharactersCreationManager.BuildCharacterObject(character);

        CharactersTreeManager.BuildTree(CharactersManager.CharactersCollection.First(), CharactersManager.CharactersCollection.Last());
        CharactersManager.DisplayCharacter(CharactersManager.CharactersCollection.Last());
    }
}
