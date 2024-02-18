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

    public static string Serialize(List<Method> methods) { return JsonConvert.SerializeObject(methods, Formatting.Indented); }
    public static List<Method> Deserialize(string json) { return JsonConvert.DeserializeObject<List<Method>>(json); }

    public static List<MethodData> PackData(Character character)
    {
        List<MethodData> methodsData = new();
        foreach (Method method in character.Methods)
        {
            MethodData methodData = new()
            {
                Owner = FindMethodOwner(character, method),
                Name = method.Name,
                Description = method.Description,
                Attribute = new AttributeData()
                {
                    Owner = AttributesData.FindAttributeOwner(character, method.Attribute),
                    Name = method.Attribute.Name,
                    AccessModifier = method.Attribute.AccessModifier
                },
                
                AccessModifier = method.AccessModifier
            };

            methodsData.Add(methodData);
        }

        return methodsData;
    }
    public static List<Method> UnpackData(CharacterData characterData)
    {
        List<Method> methodsCollection = new();
        foreach (MethodData methodData in characterData.Methods)
        {
            Method method = (methodData.Owner != characterData.Name)
                ? CharactersData.CharactersManager.CharactersCollection.Find(character => character.Name == methodData.Owner).Methods.Find(method => method.Name == methodData.Name)
                : new(MethodsManager.MethodsCollection.Find(method => method.Name == methodData.Name));

            method.AccessModifier = methodData.Owner != characterData.Name ? method.AccessModifier : methodData.AccessModifier;
            methodsCollection.Add(method);
        }

        return methodsCollection;
    }

    public static string FindMethodOwner(Character character, Method method)
    {
        if (character.Methods.Contains(method)) return character.Name;
        else foreach (Character parent in character.Parents) return FindMethodOwner(parent, method);

        return null;
    }
}


public class MethodData
{
    public string Owner;
    public string Name;
    public string Description;
    public AttributeData Attribute;

    public AccessModifier AccessModifier;
}
