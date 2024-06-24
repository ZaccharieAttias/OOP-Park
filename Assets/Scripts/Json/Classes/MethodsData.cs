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
    public static void Load(string filename) { MethodsManager.MethodsCollection = Deserialize(File.ReadAllText(filename)); }

    public static string Serialize(List<Method> methods) { return JsonConvert.SerializeObject(methods, Formatting.Indented); }
    public static List<Method> Deserialize(string json) { return JsonConvert.DeserializeObject<List<Method>>(json); }

    public static List<MethodData> PackData(CharacterB character)
    {
        List<MethodData> methodsData = new();
        foreach (Method method in character.Methods)
        {
            MethodData methodData = new()
            {
                Owner = method.Owner,
                Name = method.Name,
                Attribute = new AttributeData()
                {
                    Owner = method.Attribute.Owner,
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
            Method method = (methodData.Owner == characterData.Name)
                ? new(MethodsManager.MethodsCollection.Find(item => item.Name == methodData.Name), characterData.Name, CharactersData.CharactersManager.CharactersCollection.Find(character => character.Name == methodData.Attribute.Owner).Attributes.Find(attribute => attribute.Name == methodData.Attribute.Name))
                : CharactersData.CharactersManager.CharactersCollection.Find(character => character.Name == methodData.Owner).Methods.Find(method => method.Name == methodData.Name);

            method.AccessModifier = methodData.Owner == characterData.Name ? methodData.AccessModifier : method.AccessModifier;
            methodsCollection.Add(method);
        }

        return methodsCollection;
    }
    public static void SetPath(string path) { FilePath = path; }
}


public class MethodData
{
    public string Owner;
    public string Name;
    public AttributeData Attribute;

    public AccessModifier AccessModifier;
}
