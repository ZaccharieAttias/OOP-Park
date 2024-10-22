using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class AttributesData
{
    [Header("File Path")]
    public static string FilePath;

    [Header("Managers")]
    public static AttributesManager AttributesManager;


    public static void Initialize(string folderPath)
    {
        FilePath = Path.Combine(folderPath, "Attributes.json");
        AttributesManager = GameObject.Find("Canvas/Popups").GetComponent<AttributesManager>();
    }

    public static void Save()
    {
        File.WriteAllText(FilePath, Serialize(AttributesManager.AttributesCollection));
    }
    public static void Load()
    {
        AttributesManager.AttributesCollection = Deserialize(File.ReadAllText(FilePath));
    }
    public static void Load(string filename)
    {
        AttributesManager.AttributesCollection = Deserialize(File.ReadAllText(filename));
    }
    public static string Serialize(List<Attribute> attributes)
    {
        return JsonConvert.SerializeObject(attributes, Formatting.Indented);
    }
    public static List<Attribute> Deserialize(string json)
    {
        return JsonConvert.DeserializeObject<List<Attribute>>(json);
    }
    public static List<AttributeData> PackData(CharacterB character)
    {
        List<AttributeData> attributesData = new();
        foreach (Attribute attribute in character.Attributes)
        {
            if (attribute.Name == "appearance") continue;
            AttributeData attributeData = new()
            {
                Owner = attribute.Owner,
                Name = attribute.Name,
                Value = attribute.Value,
                Getter = attribute.Getter,
                Setter = attribute.Setter,

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
               ? new(AttributesManager.AttributesCollection.Find(attr => attr.Name == attributeData.Name), characterData.Name, attributeData.Value, attributeData.Getter, attributeData.Setter)
               : CharactersData.CharactersManager.CharactersCollection.Find(character => character.Name == attributeData.Owner).Attributes.Find(attr => attr.Name == attributeData.Name);

            attribute.AccessModifier = attributeData.Owner == characterData.Name ? attributeData.AccessModifier : attribute.AccessModifier;
            attributesCollection.Add(attribute);
        }

        return attributesCollection;
    }
    public static void SetPath(string path)
    {
        FilePath = path;
    }
}


public class AttributeData
{
    public string Owner;
    public string Name;
    public float Value;
    public bool Getter;
    public bool Setter;
    public AccessModifier AccessModifier;
}