using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;


public static class SpecialAbilitiesData
{
    public static string FilePath;
    public static SpecialAbilityManager SpecialAbilityManager;


    public static void Initialize(string folderPath)
    {
        FilePath = Path.Combine(folderPath, "SpecialAbilities.json");
        SpecialAbilityManager = GameObject.Find("Canvas/Popups").GetComponent<SpecialAbilityManager>();
    }


    public static void Save() { File.WriteAllText(FilePath, Serialize(SpecialAbilityManager.SpecialAbilitiesCollection)); }
    public static void Load() { SpecialAbilityManager.SpecialAbilitiesCollection = Deserialize(File.ReadAllText(FilePath)); }

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
        
        return SpecialAbilityManager.SpecialAbilitiesCollection[specialAbilityType].Find(ability => ability.Name == specialAbilityName);
    }
}


[System.Serializable]
public class SpecialAbilityData
{
    public string Name;
    public SpecialAbility Type;
}
