using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


public static class CharactersData
{
    public static string FilePath;
    public static CharactersManager CharactersManager;


    public static void Initialize(string folderPath)
    {
        FilePath = Path.Combine(folderPath, "Characters.json");
        CharactersManager = GameObject.Find("Player").GetComponent<CharactersManager>();
    }


    public static void Save() { File.WriteAllText(FilePath, Serialize(CharactersManager.CharactersCollection)); }
    public static void Load() { CharactersManager.CharactersCollection = Deserialize(File.ReadAllText(FilePath)); }

    public static string Serialize(List<Character> characters) { return JsonConvert.SerializeObject(PackData(characters), Formatting.Indented); }
    public static List<Character> Deserialize(string json) { return UnpackData(JsonConvert.DeserializeObject<List<CharacterData>>(json)); }

    public static List<CharacterData> PackData(List<Character> characters)
    {
        List<CharacterData> characterData = new();
        foreach (Character character in characters)
        {
            CharacterData data = new()
            {
                IsOriginal = character.IsOriginal,
                IsAbstract = character.IsAbstract,

                Name = character.Name,
                Description = character.Description,

                Attributes = AttributesData.PackData(character),
                Methods = MethodsData.PackData(character),

                SpecialAbility = SpecialAbilitiesData.PackData(character),
                UpcastMethod = UpcastMethodsData.PackData(character),

                Parents = character.Parents.Select(parent => parent.Name).ToList(),
                Childrens = character.Childrens.Select(child => child.Name).ToList(),
            };

            characterData.Add(data);
        }

        return characterData;
    }
    public static List<Character> UnpackData(List<CharacterData> characters)
    {
        List<Character> charactersCollection = new();
        foreach (CharacterData characterData in characters)
        {
            Character character = new()
            {
                IsOriginal = characterData.IsOriginal,
                IsAbstract = characterData.IsAbstract,

                Name = characterData.Name,
                Description = characterData.Description,

                Attributes = AttributesData.UnpackData(characterData),
                Methods = MethodsData.UnpackData(characterData),

                SpecialAbility = SpecialAbilitiesData.UnpackData(characterData),
                UpcastMethod = UpcastMethodsData.UnpackData(characterData)
            };

            character.Parents.AddRange(charactersCollection.Where(character => characterData.Parents.Contains(character.Name)).ToList());
            character.Parents.ForEach(parent => parent.Childrens.Add(character));
            
            charactersCollection.Add(character);
        }
        
        return charactersCollection;
    }
}


public class CharacterData
{
    public bool IsOriginal;
    public bool IsAbstract;

    public string Name;
    public string Description;

    public List<AttributeData> Attributes;
    public List<MethodData> Methods;

    public SpecialAbilityData SpecialAbility;
    public UpcastMethodData UpcastMethod;

    public List<string> Parents;
    public List<string> Childrens;
}
