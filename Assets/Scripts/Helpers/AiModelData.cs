using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AiModelData : MonoBehaviour
{
    public int Score = 0;
    public int DeathsCount = 0;
    public int TimeTook = 0;
    public int AbstractLevelTries = 0;
    public int AppearanceLevelTries = 0;
    public List<CharacterData> CharactersOrder = new();

    public LevelDownload LevelDownload;

    private string FilePath;

    public void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        var folderPath = Path.Combine(Application.dataPath, "Resources/Json");

        FilePath = Path.Combine(folderPath, "AiModelData.json");
    }

    public void AddCharacterData()
    {
        var currentCharacter = CharactersData.CharactersManager.CurrentCharacter;

        CharactersOrder.Add(new CharacterData
        {
            IsOriginal = currentCharacter.IsOriginal,
            IsAbstract = currentCharacter.IsAbstract,

            Name = currentCharacter.Name,
            Description = currentCharacter.Description,

            Attributes = AttributesData.PackData(currentCharacter),
            Methods = MethodsData.PackData(currentCharacter),

            SpecialAbility = SpecialAbilitiesData.PackData(currentCharacter),
            UpcastMethod = UpcastMethodsData.PackData(currentCharacter),

            Parent = currentCharacter.Parent?.Name,
            Childrens = currentCharacter.Childrens.Select(child => child.Name).ToList()
        });
    }

    public void SaveJson()
    {
        GameData gameData = new()
        {
            LevelName = RestrictionManager.Instance.OnlineGame ? LevelDownload.LevelName1 : SceneManager.GetActiveScene().name,
            Score = Score,
            DeathsCount = DeathsCount,
            TimeTook = TimeTook,
            AbstractLevelTries = AbstractLevelTries,
            AppearanceLevelTries = AppearanceLevelTries,
            CharactersOrder = CharactersOrder
        };

        string json = JsonConvert.SerializeObject(gameData, Formatting.Indented);
        File.WriteAllText(FilePath, json);

        GameObject.Find("LootLockerManager").GetComponent<LootLockerManager>().UploadPlayerData();
    }

    public int CalculateScore()
    {
        TimeTook = (int)Time.timeSinceLevelLoad;

        int timeVariable = TimeTook / 90;
        int deathsVariable = DeathsCount > 2 ? DeathsCount - 2 : 0;
        int abstractLevelTriesVariable = AbstractLevelTries > 5 ? AbstractLevelTries - 5 : 0;
        int appearanceLevelTriesVariable = AppearanceLevelTries > 5 ? AppearanceLevelTries - 5 : 0;

        Score = 100 - (deathsVariable * 5) - (abstractLevelTriesVariable * 5) - (appearanceLevelTriesVariable * 5) - (timeVariable * 5);
        Score = Score < 0 ? 0 : Score;

        SaveJson();
        return Score;
    }
}


public class GameData
{
    public string LevelName;
    public int Score;
    public int DeathsCount;
    public int TimeTook;
    public int AbstractLevelTries;
    public int AppearanceLevelTries;
    public List<CharacterData> CharactersOrder;
}
