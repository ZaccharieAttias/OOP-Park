using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;


public static class MethodsData
{
    public static string FilePath;
    public static List<CharacterMethod> MethodsCollection;


    public static void Initialize(string folderPath)
    {
        FilePath = Path.Combine(folderPath, "Methods.json");
        MethodsCollection = GameObject.Find("Canvas/Popups").GetComponent<MethodsManager>().MethodsCollection;
    }


    public static void Save() { File.WriteAllText(FilePath, Serialize(MethodsCollection)); }
    public static void Load() { MethodsCollection = Deserialize(File.ReadAllText(FilePath)); }

    public static string Serialize(List<CharacterMethod> methods) { return JsonConvert.SerializeObject(methods, Formatting.Indented); }
    public static List<CharacterMethod> Deserialize(string json) { return JsonConvert.DeserializeObject<List<CharacterMethod>>(json); }

    public static List<MethodData> PackData(Character character)
    {
        List<MethodData> methodsData = new();
        foreach (var method in character.Methods)
        {
            MethodData methodData = new MethodData();
            methodData.Owner = FindMethodOwner(character, method);
            methodData.Name = method.Name;
            methodData.Description = method.Description;
            methodData.AccessModifier = method.AccessModifier;

            AttributeData attributeData = new AttributeData();
            attributeData.Owner = AttributesData.FindAttributeOwner(character, method.Attribute);
            attributeData.Name = method.Attribute.Name;
            attributeData.AccessModifier = method.Attribute.AccessModifier;

            methodData.Attribute = attributeData;
            methodsData.Add(methodData);
        }

        return methodsData;
    }
    public static List<CharacterMethod> UnpackData(CharacterData characterData, Character character)
    {
        List<CharacterMethod> methodsCollection = new();
        foreach (var methodData in characterData.Methods)
        {
            CharacterMethod method = (methodData.Owner != characterData.Name)
                ? CharactersData.CharactersCollection.Find(character => character.Name == methodData.Owner).Methods.Find(method => method.Name == methodData.Name)
                : new(MethodsData.MethodsCollection.Find(method => method.Name == methodData.Name));

            method.AccessModifier = methodData.Owner != characterData.Name ? method.AccessModifier : methodData.AccessModifier;
            methodsCollection.Add(method);
        }

        return methodsCollection;
    }

    public static string FindMethodOwner(Character character, CharacterMethod method)
    {
        if (character.Methods.Contains(method)) return character.Name;
        else foreach (var parent in character.Parents) return FindMethodOwner(parent, method);

        return null;
    }
}


[System.Serializable]
public class MethodData
{
    public string Owner;
    public string Name;
    public string Description;
    public AccessModifier AccessModifier;
    public AttributeData Attribute;
}
