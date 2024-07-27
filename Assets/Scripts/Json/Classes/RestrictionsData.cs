using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class RestrictionsData
{
    public static string FilePath;

    public static void Initialize(string folderPath)
    {
        FilePath = Path.Combine(folderPath, "Restrictions.json");
    }

    public static void Save() 
    { 
        List<RestrictionData> restrictions = new()
        {
            new()
            {
                AllowSingleInheritance = RestrictionManager.Instance.AllowSingleInheritance,
                AllowBeginnerInheritance = RestrictionManager.Instance.AllowBeginnerInheritance,
                AllowSpecialAbility = RestrictionManager.Instance.AllowSpecialAbility,
                AllowAccessModifier = RestrictionManager.Instance.AllowAccessModifier,
                AllowOverride = RestrictionManager.Instance.AllowOverride,
                AllowUpcasting = RestrictionManager.Instance.AllowUpcasting,
                AllowAbstractClass = RestrictionManager.Instance.AllowAbstractClass,
                AllowEncapsulation = RestrictionManager.Instance.AllowEncapsulation,
                OnlineGame = RestrictionManager.Instance.OnlineGame,
                OnlineBuild = RestrictionManager.Instance.OnlineBuild
            }
        };
        if (FilePath.Contains("Saved") || FilePath.Contains("Temp")) 
        {
            restrictions[0].OnlineGame = false;
            restrictions[0].OnlineBuild = true;
        }
        else if (FilePath.Contains("Download")) 
        {
            restrictions[0].OnlineGame = true;
            restrictions[0].OnlineBuild = false;
        }
        File.WriteAllText(FilePath, Serialize(restrictions)); 
    }
    public static void Load() 
    { 
        List<RestrictionData> restrictions = Deserialize(File.ReadAllText(FilePath));
        RestrictionManager.Instance.AllowSingleInheritance = restrictions[0].AllowSingleInheritance;
        RestrictionManager.Instance.AllowBeginnerInheritance = restrictions[0].AllowBeginnerInheritance;
        RestrictionManager.Instance.AllowSpecialAbility = restrictions[0].AllowSpecialAbility;
        RestrictionManager.Instance.AllowAccessModifier = restrictions[0].AllowAccessModifier;
        RestrictionManager.Instance.AllowOverride = restrictions[0].AllowOverride;
        RestrictionManager.Instance.AllowUpcasting = restrictions[0].AllowUpcasting;
        RestrictionManager.Instance.AllowAbstractClass = restrictions[0].AllowAbstractClass;
        RestrictionManager.Instance.AllowEncapsulation = restrictions[0].AllowEncapsulation;
        if (FilePath.Contains("Saved") || FilePath.Contains("Temp")) 
        {
            RestrictionManager.Instance.OnlineGame = false;
            RestrictionManager.Instance.OnlineBuild = true;
        }
        else if (FilePath.Contains("Download")) 
        {
            RestrictionManager.Instance.OnlineGame = true;
            RestrictionManager.Instance.OnlineBuild = false;
        }
    }
    public static void Load(string filename) 
    {
        List<RestrictionData> restrictions = Deserialize(File.ReadAllText(filename));
        RestrictionManager.Instance.AllowSingleInheritance = restrictions[0].AllowSingleInheritance;
        RestrictionManager.Instance.AllowBeginnerInheritance = restrictions[0].AllowBeginnerInheritance;
        RestrictionManager.Instance.AllowSpecialAbility = restrictions[0].AllowSpecialAbility;
        RestrictionManager.Instance.AllowAccessModifier = restrictions[0].AllowAccessModifier;
        RestrictionManager.Instance.AllowOverride = restrictions[0].AllowOverride;
        RestrictionManager.Instance.AllowUpcasting = restrictions[0].AllowUpcasting;
        RestrictionManager.Instance.AllowAbstractClass = restrictions[0].AllowAbstractClass;
        RestrictionManager.Instance.AllowEncapsulation = restrictions[0].AllowEncapsulation;
        RestrictionManager.Instance.OnlineGame = restrictions[0].OnlineGame;
        RestrictionManager.Instance.OnlineBuild = restrictions[0].OnlineBuild;
    }

    public static string Serialize(List<RestrictionData> restrictions) { return JsonConvert.SerializeObject(restrictions, Formatting.Indented); }
    public static List<RestrictionData> Deserialize(string json) { return JsonConvert.DeserializeObject<List<RestrictionData>>(json); }
    public static void SetPath(string path) { FilePath = path; }
}


public class RestrictionData
{
    public bool AllowSingleInheritance;
    public bool AllowBeginnerInheritance;
    public bool AllowSpecialAbility;
    public bool AllowAccessModifier;
    public bool AllowOverride;
    public bool AllowUpcasting;
    public bool AllowAbstractClass;
    public bool AllowEncapsulation;
    public bool OnlineGame;
    public bool OnlineBuild;
}
