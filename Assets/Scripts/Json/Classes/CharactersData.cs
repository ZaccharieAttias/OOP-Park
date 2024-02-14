using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;


public static class CharactersData
{
    public static string FilePath;
    public static List<Character> CharactersCollection;


    public static void Initialize(string folderPath)
    {
        FilePath = Path.Combine(folderPath, "Characters.json");
        CharactersCollection = GameObject.Find("Player").GetComponent<CharacterManager>().CharactersCollection;
    }


    public static void Save() { File.WriteAllText(FilePath, Serialize(CharactersCollection)); }
    public static void Load() { CharactersCollection = Deserialize(File.ReadAllText(FilePath)); }

    public static string Serialize(List<Character> characters) { return JsonConvert.SerializeObject(PackData(characters), Formatting.Indented); }
    public static List<Character> Deserialize(string json) { return UnpackData(JsonConvert.DeserializeObject<List<CharacterData>>(json)); }

    public static List<CharacterData> PackData(List<Character> characters)
    {
        List<CharacterData> characterData = new();
        foreach (var character in characters)
        {
            CharacterData data = new CharacterData();
            data.IsOriginal = character.IsOriginal;
            data.Name = character.Name;
            data.Description = character.Description;

            data.Attributes = AttributesData.PackData(character);
            data.Methods = MethodsData.PackData(character);

            data.SpecialAbility = (character.SpecialAbility.Type, character.SpecialAbility.Name);

            data.Parents = character.Parents.Select(parent => parent.Name).ToList();
            data.Childrens = character.Childrens.Select(child => child.Name).ToList();

            data.IsAbstract = character.IsAbstract;
            characterData.Add(data);
        }

        return characterData;
    }
    public static List<Character> UnpackData(List<CharacterData> characters)
    {
        List<Character> charactersCollection = new();

        foreach (var characterData in characters)
        {
            Character character = new();
            character.IsOriginal = characterData.IsOriginal;
            character.Name = characterData.Name;
            character.Description = characterData.Description;
            
            character.Attributes = AttributesData.UnpackData(characterData);
            character.Methods = MethodsData.UnpackData(characterData, character);

            character.SpecialAbility = SpecialAbilitiesData.SpecialAbilitiesCollection[characterData.SpecialAbility.Item1].Find(ability => ability.Name == characterData.SpecialAbility.Item2);

            character.Parents.AddRange(charactersCollection.Where(character => characterData.Parents.Contains(character.Name)).ToList());
            character.Parents.ForEach(parent => parent.Childrens.Add(character));

            character.IsAbstract = characterData.IsAbstract;
            charactersCollection.Add(character);
        }
        
        return charactersCollection;
    }
}


[System.Serializable]
public class CharacterData
{
    public bool IsOriginal;
    public string Name;
    public string Description;

    public List<AttributeData> Attributes;
    public List<MethodData> Methods;

    public (SpecialAbility, string) SpecialAbility;

    public List<string> Parents;
    public List<string> Childrens;

    public bool IsAbstract;
}
