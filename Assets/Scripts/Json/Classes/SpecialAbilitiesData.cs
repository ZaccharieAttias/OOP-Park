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


    // public static void Save() { File.WriteAllText(FilePath, Serialize(SpecialAbilitiesManager.SpecialAbilitiesCollection)); }
    // public static void Load() { SpecialAbilitiesManager.SpecialAbilitiesCollection = Deserialize(File.ReadAllText(FilePath)); }
    // public static void Load(string filename) { SpecialAbilitiesManager.SpecialAbilitiesCollection = Deserialize(File.ReadAllText(filename)); }

    // public static string Serialize(Dictionary<SpecialAbilityType, List<SpecialAbility>> specialAbilities) { return JsonConvert.SerializeObject(specialAbilities, Formatting.Indented); }
    // public static Dictionary<SpecialAbilityType, List<SpecialAbility>> Deserialize(string json) { return JsonConvert.DeserializeObject<Dictionary<SpecialAbilityType, List<SpecialAbility>>>(json); }

    // public static SpecialAbilityData PackData(CharacterB character)
    // {
    //     SpecialAbilityData specialAbilityData = new()
    //     {
    //         Name = character.SpecialAbility.Name,
    //         Type = character.SpecialAbility.Type
    //     };

    //     return specialAbilityData;
    // }
    // public static SpecialAbility UnpackData(CharacterData characterData)
    // {
    //     string specialAbilityName = characterData.SpecialAbility.Name;
    //     SpecialAbilityType specialAbilityType = characterData.SpecialAbility.Type;

    //     return SpecialAbilitiesManager.SpecialAbilitiesCollection[specialAbilityType].Find(ability => ability.Name == specialAbilityName);
    // }
}


public class SpecialAbilityData
{
    public string Name;
    public SpecialAbilityType Type;
}
