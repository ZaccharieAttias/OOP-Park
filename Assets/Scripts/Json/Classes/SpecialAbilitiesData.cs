using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;




public static class SpecialAbilitiesData
{
    public static string FilePath;
    public static SpecialAbilityManager SpecialAbilityManager;




    public static void Initialize(string folderPath)
    {
        FilePath = Path.Combine(folderPath, "SpecialAbilities.json");
        SpecialAbilityManager = GameObject.Find("Canvas/Popups").GetComponent<SpecialAbilityManager>();
    }




    public static void Save() { File.WriteAllText(FilePath, Serialize(SpecialAbilityManager.SpecialAbilitiesDictionary)); }
    public static void Load() { SpecialAbilityManager.SpecialAbilitiesCollection = Deserialize(File.ReadAllText(FilePath)); }
    public static void Load(string filename) { SpecialAbilityManager.SpecialAbilitiesCollection = Deserialize(File.ReadAllText(filename)); }


    public static string Serialize(Dictionary<SpecialAbilityType, List<SpecialAbility>> specialAbilities) { return JsonConvert.SerializeObject(specialAbilities, Formatting.Indented); }
    public static List<SpecialAbilityObject> Deserialize(string json) { return temp(json); }


    public static SpecialAbilityData PackData(CharacterB character)
    {
        if (character.SpecialAbility == null) return null;
        
        SpecialAbilityData specialAbilityData = new()
        {
            Name = character.SpecialAbility.Name,
            Type = character.SpecialAbility.Type,
        };


        return specialAbilityData;
    }
    public static SpecialAbility UnpackData(CharacterData characterData)
    {
        string specialAbilityName = characterData.SpecialAbility?.Name;
        if (specialAbilityName == null) return null;
        SpecialAbilityType specialAbilityType = characterData.SpecialAbility.Type;


        return SpecialAbilityManager.SpecialAbilitiesCollection.Find(ability => ability.SpecialAbility.Name == specialAbilityName).SpecialAbility;
    }




    public static List<SpecialAbilityObject> temp(string json)
    {
        Dictionary<SpecialAbilityType, List<SpecialAbility>> specialAbilities = JsonConvert.DeserializeObject<Dictionary<SpecialAbilityType, List<SpecialAbility>>>(json);


        List<SpecialAbilityObject> specialAbilityObjects = new();
        foreach (KeyValuePair<SpecialAbilityType, List<SpecialAbility>> specialAbilityType in specialAbilities)
        {
            foreach (SpecialAbility specialAbility in specialAbilityType.Value)
            {
                SpecialAbilityObject specialAbilityObject = new(specialAbility);


                if (specialAbility.Name.Replace(" ", "") == specialAbilityType.Key.ToString().Replace(" ", ""))
                {
                    if (specialAbility.Name == "General") specialAbilityObject.Parent = null;
                    else continue;
                }
                else
                {
                    specialAbilityObject.Parent = specialAbilityObjects.Find(ability => ability.SpecialAbility.Name.Replace(" ", "") == specialAbilityType.Key.ToString().Replace(" ", ""));
                    specialAbilityObject.Parent.Childrens.Add(specialAbilityObject);
                }
                specialAbilityObject.Name = specialAbility.Name;
                specialAbilityObjects.Add(specialAbilityObject);
            }
        }
        return specialAbilityObjects;
    }
    public static void SetPath(string path) { FilePath = path; }
}


public class SpecialAbilityData
{
    public string Name;
    public SpecialAbilityType Type;
}