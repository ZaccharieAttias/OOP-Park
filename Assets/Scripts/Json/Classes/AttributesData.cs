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
    public static void Load(string filename) { AttributesManager.AttributesCollection = Deserialize(File.ReadAllText(filename)); }

    public static string Serialize(List<Attribute> attributes) { return JsonConvert.SerializeObject(attributes, Formatting.Indented); }
    public static List<Attribute> Deserialize(string json) { return JsonConvert.DeserializeObject<List<Attribute>>(json); }

    public static List<AttributeData> PackData(Character character)
    {
        List<AttributeData> attributesData = new();
        foreach (Attribute attribute in character.Attributes)
        {
            AttributeData attributeData = new()
            {
                Owner = attribute.Owner,
                Name = attribute.Name,
                
                AccessModifier = attribute.AccessModifier
            };

            attributesData.Add(attributeData);
        }

        return attributesData;
    }
    public static List<Attribute> UnpackData(CharacterData characterData)
    {
        List<Attribute> attributesCollection = new();
        foreach (AttributeData attributeData in characterData.Attributes)
        {
            Attribute attribute = (attributeData.Owner == characterData.Name)
               ? new(AttributesManager.AttributesCollection.Find(attribute => attribute.Name == attributeData.Name), characterData.Name)
               : CharactersData.CharactersManager.CharactersCollection.Find(character => character.Name == attributeData.Owner).Attributes.Find(attribute => attribute.Name == attributeData.Name);

            attribute.AccessModifier = attributeData.Owner == characterData.Name ? attributeData.AccessModifier: attribute.AccessModifier;
            attributesCollection.Add(attribute);
        }

        return attributesCollection;
    }
}


public class AttributeData
{
    public string Owner;
    public string Name;
    
    public AccessModifier AccessModifier;
}
