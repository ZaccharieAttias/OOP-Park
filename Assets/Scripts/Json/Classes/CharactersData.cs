using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;


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
            
                // CharacterButton = character.CharacterButton
            };

            characterData.Add(data);
        }

        return characterData;
    }
    public static List<Character> UnpackData(List<CharacterData> characters)
    {
        List<Character> charactersCollection = new();
        foreach (var characterData in characters)
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
                UpcastMethod = characterData.UpcastMethod == null ? null : UpcastMethodsData.UnpackData(characterData)
            };

            character.Parents.AddRange(charactersCollection.Where(character => characterData.Parents.Contains(character.Name)).ToList());
            character.Parents.ForEach(parent => parent.Childrens.Add(character));

            charactersCollection.Add(character);
        }
        
        return charactersCollection;
    }
}


[System.Serializable]
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
