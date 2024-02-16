using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class SpecialAbilitiesData
{
    public static string FilePath;
    public static SpecialAbilitiesManager SpecialAbilitiesManager;


    public static void Initialize(string folderPath)
    {
        FilePath = Path.Combine(folderPath, "SpecialAbilities.json");
        SpecialAbilitiesManager = GameObject.Find("Canvas/Popups").GetComponent<SpecialAbilitiesManager>();
    }


    public static void Save() { File.WriteAllText(FilePath, Serialize(SpecialAbilitiesManager.SpecialAbilitiesCollection)); }
    public static void Load() { SpecialAbilitiesManager.SpecialAbilitiesCollection = Deserialize(File.ReadAllText(FilePath)); }

    public static string Serialize(Dictionary<SpecialAbility, List<CharacterSpecialAbility>> specialAbilities) { return JsonConvert.SerializeObject(specialAbilities, Formatting.Indented); }
    public static Dictionary<SpecialAbility, List<CharacterSpecialAbility>> Deserialize(string json) { return JsonConvert.DeserializeObject<Dictionary<SpecialAbility, List<CharacterSpecialAbility>>>(json); }

    public static SpecialAbilityData PackData(Character character)
    {
        SpecialAbilityData specialAbilityData = new()
        {
            Name = character.SpecialAbility.Name,
            Type = character.SpecialAbility.Type
        };

        return specialAbilityData;
    }
    public static CharacterSpecialAbility UnpackData(CharacterData characterData)
    {
        string specialAbilityName = characterData.SpecialAbility.Name;
        SpecialAbility specialAbilityType = characterData.SpecialAbility.Type;
        
        return SpecialAbilitiesManager.SpecialAbilitiesCollection[specialAbilityType].Find(ability => ability.Name == specialAbilityName);
    }
}


[System.Serializable]
public class SpecialAbilityData
{
    public string Name;
    public SpecialAbility Type;
}
