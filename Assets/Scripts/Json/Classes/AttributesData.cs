using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class AttributesData
{
    public static string FilePath;
    public static AttributesManager AttributesManager;


    public static void Initialize(string folderPath)
    {
        FilePath = Path.Combine(folderPath, "Attributes.json");
        AttributesManager = GameObject.Find("Canvas/Popups").GetComponent<AttributesManager>();
    }


    public static void Save() { File.WriteAllText(FilePath, Serialize(AttributesManager.AttributesCollection)); }
    public static void Load() { AttributesManager.AttributesCollection = Deserialize(File.ReadAllText(FilePath)); }

    public static string Serialize(List<CharacterAttribute> attributes) { return JsonConvert.SerializeObject(attributes, Formatting.Indented); }
    public static List<CharacterAttribute> Deserialize(string json) { return JsonConvert.DeserializeObject<List<CharacterAttribute>>(json); }

    public static List<AttributeData> PackData(Character character)
    {
        List<AttributeData> attributesData = new();
        foreach (var attribute in character.Attributes)
        {
            AttributeData attributeData = new()
            {
                Owner = FindAttributeOwner(character, attribute),
                Name = attribute.Name,
                AccessModifier = attribute.AccessModifier
            };

            attributesData.Add(attributeData);
        }

        return attributesData;
    }
    public static List<CharacterAttribute> UnpackData(CharacterData characterData)
    {
        List<CharacterAttribute> attributesCollection = new();
        foreach (var attributeData in characterData.Attributes)
        {
            CharacterAttribute attribute = (attributeData.Owner != characterData.Name)
               ? CharactersData.CharactersManager.CharactersCollection.Find(character => character.Name == attributeData.Owner).Attributes.Find(attribute => attribute.Name == attributeData.Name)
               : new(AttributesManager.AttributesCollection.Find(attribute => attribute.Name == attributeData.Name));

            attribute.AccessModifier = attributeData.Owner != characterData.Name ? attribute.AccessModifier : attributeData.AccessModifier;
            attributesCollection.Add(attribute);
        }

        return attributesCollection;
    }

    public static string FindAttributeOwner(Character character, CharacterAttribute attribute)
    {
        if (character.Attributes.Contains(attribute)) return character.Name;
        else foreach (var parent in character.Parents) return FindAttributeOwner(parent, attribute);

        return null;
    }
}


[System.Serializable]
public class AttributeData
{
    public string Owner;
    public string Name;
    public AccessModifier AccessModifier;
}
