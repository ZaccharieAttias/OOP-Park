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


    public static void Save() { File.WriteAllText(FilePath, Serialize(SceneManagement.ChapterInfos)); }
    public static void Load() { SceneManagement.ChapterInfos = Deserialize(File.ReadAllText(FilePath)); }

    public static string Serialize(List<ChapterInfo> chapterInfos) { return JsonConvert.SerializeObject(chapterInfos, Formatting.Indented); }
    public static List<ChapterInfo> Deserialize(string json) { return JsonConvert.DeserializeObject<List<ChapterInfo>>(json); }
}
