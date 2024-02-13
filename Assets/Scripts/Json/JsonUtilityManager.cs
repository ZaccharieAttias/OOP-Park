using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


public class JsonUtilityManager : MonoBehaviour
{
    public string FolderPath;

    public CharacterManager CharacterManager;
    public AttributesManager AttributesManager;
    public MethodsManager MethodsManager;
    public SpecialAbilityManager SpecialAbilityManager;


    public void Start()
    {
        FolderPath = Path.Combine(Application.dataPath, "Resources/Json", SceneManager.GetActiveScene().name);
    
        CharacterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
        AttributesManager = GameObject.Find("Canvas/Popups").GetComponent<AttributesManager>();
        MethodsManager = GameObject.Find("Canvas/Popups").GetComponent<MethodsManager>();
        SpecialAbilityManager = GameObject.Find("Canvas/Popups").GetComponent<SpecialAbilityManager>();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) Save();
        // if (Input.GetKeyDown(KeyCode.L)) Load();
    }
    
    public void Save()
    {
        SaveScriptCollection(AttributesManager, "Attributes.json");
        SaveScriptCollection(MethodsManager, "Methods.json");
        SaveScriptCollection(CharacterManager, "Characters.json");
    }


    private void SaveScriptCollection<T>(T script, string filename)
    {
        string json = Serialize(script);
        File.WriteAllText(Path.Combine(FolderPath, filename), Serialize(Deserialize<CollectionWrapper>(json)));
    }
    
    public static string Serialize<T>(T data) { return JsonUtility.ToJson(data, true); }
    public static T Deserialize<T>(string json) { return JsonUtility.FromJson<T>(json); }

    public static string ExtractJsonObjectByKey(string json, string key)
    {
        string quote = "\"";
        int startIndex = json.IndexOf($"{quote}{key}{quote}");
        if (startIndex == -1) return null;

        int endIndex = FindEndIndex(json, startIndex + key.Length + 4);

        return endIndex == -1 ? null : json.Substring(startIndex, endIndex);
    }
    private static int FindEndIndex(string json, int endIndex)
    {
        Stack<char> jsonMarkStack = new();
        Dictionary<char, char> jsonMarkDict = new()
        {
            { '}', '{' },
            { ']', '[' },
            // { '"', '"' }, // Doesnt work for now, find a way to distinguish between key and value
        };

        jsonMarkStack.Push(json[endIndex++]);
        while (endIndex < json.Length && jsonMarkStack.Count > 0)
        {
            char currentChar = json[endIndex];
            if (jsonMarkDict.ContainsValue(currentChar)) jsonMarkStack.Push(currentChar);
            
            else if (jsonMarkDict.ContainsKey(currentChar))
                if (jsonMarkStack.Pop() != jsonMarkDict[currentChar]) return -1;
            
            endIndex++;
        }

        return jsonMarkStack.Count == 0 ? endIndex : -1;
    }
}