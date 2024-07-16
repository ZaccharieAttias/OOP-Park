using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class GameplayData
{
    public static string FilePath;

    public static void Initialize()
    {
        var folderPath = Path.Combine(Application.dataPath, "Resources/Json");

        FilePath = Path.Combine(folderPath, "Gameplay.json");
    }


    public static void Save() { File.WriteAllText(FilePath, Serialize(SceneManagement.GameplayInfo)); }
    public static void Load() { SceneManagement.GameplayInfo = Deserialize(File.ReadAllText(FilePath)); }
    public static List<GameplayInfo> LoadForMenu() { return Deserialize(File.ReadAllText(Path.Combine(Application.dataPath, "Resources/Json", "Gameplay.json"))); }

    public static string Serialize(List<GameplayInfo> gameplayInfos) { return JsonConvert.SerializeObject(gameplayInfos, Formatting.Indented); }
    public static List<GameplayInfo> Deserialize(string json) { return JsonConvert.DeserializeObject<List<GameplayInfo>>(json); }
}
