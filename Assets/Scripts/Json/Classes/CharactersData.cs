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
    public static void Load() 
    { 
        CharactersManager.CharactersCollection = new();
        Deserialize(File.ReadAllText(FilePath)); 
    }
    public static void Load(string filename) 
    { 
        CharactersManager.CharactersCollection = new();
        Deserialize(File.ReadAllText(filename)); 
    }


    public static string Serialize(List<CharacterB> characters) { return JsonConvert.SerializeObject(PackData(characters), Formatting.Indented); }
    public static void Deserialize(string json) { UnpackData(JsonConvert.DeserializeObject<List<CharacterData>>(json)); }

    public static List<CharacterData> PackData(List<CharacterB> characters)
    {
        List<CharacterData> characterData = new();
        foreach (CharacterB character in characters)
        {
            CharacterData data = new()
            {
                IsOriginal = true,
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
    public static void UnpackData(List<CharacterData> characters)
    {
        foreach (CharacterData characterData in characters)
        {
            CharacterB character = new()
            {
                IsOriginal = characterData.IsOriginal,
                IsAbstract = characterData.IsAbstract,

                Name = characterData.Name,
                Description = characterData.Description,

                SpecialAbility = SpecialAbilitiesData.UnpackData(characterData),
                UpcastMethod = UpcastMethodsData.UnpackData(characterData)
            };

            CharactersManager.CharactersCollection.Add(character);
            
            character.Attributes = AttributesData.UnpackData(characterData);
            character.Methods = MethodsData.UnpackData(characterData);

            character.Parents.AddRange(CharactersManager.CharactersCollection.Where(character => characterData.Parents.Contains(character.Name)).ToList());
            character.Parents.ForEach(parent => parent.Childrens.Add(character));
        }
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
