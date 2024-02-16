using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class MethodsData
{
    public static string FilePath;
    public static MethodsManager MethodsManager;


    public static void Initialize(string folderPath)
    {
        FilePath = Path.Combine(folderPath, "Methods.json");
        MethodsManager = GameObject.Find("Canvas/Popups").GetComponent<MethodsManager>();
    }


    public static void Save() { File.WriteAllText(FilePath, Serialize(MethodsManager.MethodsCollection)); }
    public static void Load() { MethodsManager.MethodsCollection = Deserialize(File.ReadAllText(FilePath)); }

    public static string Serialize(List<CharacterMethod> methods) { return JsonConvert.SerializeObject(methods, Formatting.Indented); }
    public static List<CharacterMethod> Deserialize(string json) { return JsonConvert.DeserializeObject<List<CharacterMethod>>(json); }

    public static List<MethodData> PackData(Character character)
    {
        List<MethodData> methodsData = new();
        foreach (var method in character.Methods)
        {
            MethodData methodData = new()
            {
                Owner = FindMethodOwner(character, method),
                Name = method.Name,
                Description = method.Description,
                AccessModifier = method.AccessModifier,
                Attribute = new AttributeData()
                {
                    Owner = AttributesData.FindAttributeOwner(character, method.Attribute),
                    Name = method.Attribute.Name,
                    AccessModifier = method.Attribute.AccessModifier
                }
            };

            methodsData.Add(methodData);
        }

        return methodsData;
    }
    public static List<CharacterMethod> UnpackData(CharacterData characterData)
    {
        List<CharacterMethod> methodsCollection = new();
        foreach (var methodData in characterData.Methods)
        {
            CharacterMethod method = (methodData.Owner != characterData.Name)
                ? CharactersData.CharactersManager.CharactersCollection.Find(character => character.Name == methodData.Owner).Methods.Find(method => method.Name == methodData.Name)
                : new(MethodsManager.MethodsCollection.Find(method => method.Name == methodData.Name));

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
