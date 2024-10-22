using Assets.PixelFantasy.PixelTileEngine.Scripts;
using LootLocker.Requests;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelUpload : MonoBehaviour
{
    private TMP_InputField LevelNameInputField;
    private string LevelName;
    private GameObject LevelUploadUi;

    public TileMap _groundMap;
    public TileMap _coverMap;
    public TileMap _propsMap;
    public TileMap _wallMap;
    public TileMap _gameplayMap;
    private LevelBuilderB LevelBuilder;
    private int TileMapContextID;
    private int CharacterTreeContextID;
    public JsonUtilityManager JsonUtilityManager;
    public GameObject ErrorText;

    public void Start()
    {
        LevelUploadUi = GameObject.Find("Canvas/Menus/Gameplay/UploadScreen");
        LevelNameInputField = LevelUploadUi.transform.Find("Background/Foreground/InputField").GetComponent<TMP_InputField>();
        LevelBuilder = GameObject.Find("Grid/LevelBuilder").GetComponent<LevelBuilderB>();
        JsonUtilityManager = GameObject.Find("GameInitializer").GetComponent<JsonUtilityManager>();

        _groundMap = LevelBuilder.GetGroundMap();
        _coverMap = LevelBuilder.GetCoverMap();
        _propsMap = LevelBuilder.GetPropsMap();
        _gameplayMap = LevelBuilder.GetGameplayMap();
        _wallMap = LevelBuilder.GetWallMap();

        TileMapContextID = 235247;
        CharacterTreeContextID = 235353;

        LevelUploadUi.SetActive(false);
    }
    public void CreateLevel()
    {
        LevelName = LevelNameInputField.text;
        string path = Path.Combine(Application.dataPath, "StreamingAssets", "Resources", "Screenshots", "Saved", LevelName);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        else
        {
            string[] filesDic = Directory.GetFiles(path);
            foreach (string file in filesDic)
            {
                if (file.Contains(".png") || file.Contains(".txt"))
                {
                    File.Delete(file);
                }
            }
        }

        OpenUploadLevelUI();

        // Upload Level Data (the gameplay)
        LootLockerSDKManager.CreatingAnAssetCandidate(LevelName, (response) =>
        {
            if (response.success)
            {
                UploadLevelData((int)response.asset_candidate.id);
            }
            else
            {
                Debug.Log("Level Creation Failed");
                ErrorText.GetComponent<TMP_Text>().text = "Level Creation Failed";
                return;
            }
        }, filters: new Dictionary<string, string>() { { "Context", "TileMap" } }, context_id: TileMapContextID);

        // Upload Level Data (character center)
        LootLockerSDKManager.CreatingAnAssetCandidate(LevelName, (response) =>
        {
            if (response.success)
            {
                UploadLevelTreeData((int)response.asset_candidate.id);
                GameObject.Find("Canvas/Menus/Gameplay").SetActive(false);
            }
            else
            {
                Debug.Log("Character Center Creation Failed");
                ErrorText.GetComponent<TMP_Text>().text = "Level Creation Failed";
                return;
            }
        }, filters: new Dictionary<string, string>() { { "Context", "CharacterTree" } }, context_id: CharacterTreeContextID);
    }
    public void UploadLevelData(int levelID)
    {
        string screenshotFilePath = Application.dataPath + "/StreamingAssets" + "/Resources/Screenshots/Saved/" + LevelName + "/Level_" + LevelName + ".png";
        LootLocker.LootLockerEnums.FilePurpose screenshotFileType = LootLocker.LootLockerEnums.FilePurpose.primary_thumbnail;

        LootLockerSDKManager.AddingFilesToAssetCandidates(levelID, screenshotFilePath, "Level_" + LevelName + ".png", screenshotFileType, (screenshotResponse) =>
        {
            if (screenshotResponse.success)
            {
                string textFilePath = Application.dataPath + "/StreamingAssets" + "/Resources/Screenshots/Saved/" + LevelName + "/Level_" + LevelName + "_Data.json";
                SaveLevel(textFilePath);

                //coverting json file to txt file
                string json = File.ReadAllText(textFilePath);
                File.WriteAllText(textFilePath, json);

                LootLocker.LootLockerEnums.FilePurpose textFileType = LootLocker.LootLockerEnums.FilePurpose.file;

                LootLockerSDKManager.AddingFilesToAssetCandidates(levelID, textFilePath, "Level_" + LevelName + "_Data.txt", textFileType, (textResponse) =>
                {
                    if (textResponse.success)
                    {
                        LootLockerSDKManager.UpdatingAnAssetCandidate(levelID, true, (updatedResponse) => { });
                    }
                    else
                    {
                        ErrorText.GetComponent<TMP_Text>().text = "Level Creation Failed";
                        Debug.Log("Level Data Upload Failed");
                    }
                });
            }
            else
            {
                ErrorText.GetComponent<TMP_Text>().text = "Level Creation Failed";
                Debug.Log("Screenshot Upload Failed");
            }
        });

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
        string[] files = Directory.GetFiles(Path.Combine(Application.dataPath, "StreamingAssets", "Resources", "Screenshots", "Saved", LevelName));
        foreach (string file in files)
        {
            if (file.Contains(".txt"))
            {
                File.Delete(file);
            }
        }
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
    public void UploadLevelTreeData(int levelID)
    {
        string textFilePath = Path.Combine(Application.dataPath, "StreamingAssets", "Resources", "Screenshots", "Saved", LevelName);
        SaveCharacter(textFilePath);

        LootLocker.LootLockerEnums.FilePurpose textFileType = LootLocker.LootLockerEnums.FilePurpose.file;

        string[] files = Directory.GetFiles(textFilePath);
        AddingFilesToAsset(levelID, files, 0, textFileType);

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
        string[] filess = Directory.GetFiles(textFilePath);
        foreach (string file in filess)
        {
            if (file.Contains(".txt"))
            {
                File.Delete(file);
            }
        }
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
    public void OpenUploadLevelUI()
    {
        StartCoroutine(WaitScreenShot());
    }
    IEnumerator WaitScreenShot()
    {
        TakeScreenshot();
        yield return new WaitForSeconds(1.0f);
        GameObject.Find("Canvas/Menus/Gameplay").SetActive(true);
    }
    public void TakeScreenshot()
    {
        GameObject.Find("Canvas/Menus/Gameplay").SetActive(false);
        string filepath = Application.dataPath + "/StreamingAssets" + "/Resources/Screenshots/Saved/" + LevelName + "/";
        ScreenCapture.CaptureScreenshot(Path.Combine(filepath, "Level_" + LevelName + ".png"));
    }
    public void SaveLevel(string path)
    {
        if (_groundMap == null) return;

        var width = _groundMap.Width;
        var height = _groundMap.Height;
        var depth = _groundMap.Depth;
        Dictionary<int, List<string>> ChallengeAppearancesConditions = new Dictionary<int, List<string>>();
        Dictionary<int, List<string>> ChallengeAppearancesTexts = new Dictionary<int, List<string>>();
        var level = new LevelB(width, height, depth, 0, 0, ChallengeAppearancesConditions, ChallengeAppearancesTexts);

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                for (var z = 0; z < depth; z++)
                {
                    var groundIndex = -1;

                    if (_groundMap[x, y, z] != null)
                    {
                        groundIndex = level.AddTexture(_groundMap[x, y, z].SpriteRenderer.sprite.texture.name);
                    }

                    var coverIndex = -1;

                    if (_coverMap[x, y, z] != null)
                    {
                        coverIndex = level.AddTexture(_coverMap[x, y, z].SpriteRenderer.sprite.texture.name);
                    }

                    var propsIndex = -1;

                    if (_propsMap[x, y, z] != null)
                    {
                        propsIndex = level.AddTexture(_propsMap[x, y, z].SpriteRenderer.sprite.name);
                    }

                    var wallIndex = -1;

                    if (_wallMap[x, y, z] != null)
                    {
                        wallIndex = level.AddTexture(_wallMap[x, y, z].SpriteRenderer.sprite.name);
                    }

                    var gameplayIndex = -1;

                    if (_gameplayMap[x, y, z] != null)
                    {
                        gameplayIndex = level.AddTexture(_gameplayMap[x, y, z].SpriteRenderer.sprite.name);
                    }

                    level.GroundMap[x, y, z] = groundIndex + 1;
                    level.CoverMap[x, y, z] = coverIndex + 1;
                    level.PropsMap[x, y, z] = propsIndex + 1;
                    level.WallMap[x, y, z] = wallIndex + 1;
                    level.GameplayMap[x, y, z] = gameplayIndex + 1;
                }
            }
        }

        var player = GameObject.Find("Player");
        Transform playerTransform = player.transform;

        level.characterX = playerTransform.localPosition.x;
        level.characterY = playerTransform.localPosition.y;

        List<GameObject> missions = new List<GameObject>();
        foreach (Transform child in GameObject.Find("Canvas/Popups").transform)
        {
            if (child.name.Contains("Mission"))
                missions.Add(child.gameObject);
        }

        int i = 1;
        foreach (GameObject mission in missions)
        {
            int index = int.Parse(mission.name.Substring(7));
            List<string> appearancesCondition = new List<string>();
            List<string> appearancesText = new List<string>();
            appearancesCondition = GameObject.Find("Canvas/Popups").GetComponent<CharacterChallengeManager>().ChallengeAppearancesConditions[index];
            string texts = "";
            if (mission.transform.Find("Background/Foreground/Mssion/InputField") != null)
                texts = mission.transform.Find("Background/Foreground/Mssion/InputField").GetComponent<TMP_InputField>().text;
            else
                texts =  mission.transform.Find("Background/Foreground/Mssion/Mission").GetComponent<TMP_Text>().text;
            appearancesText.Add(texts);
            level.ChallengeAppearancesConditions.Add(i, appearancesCondition);
            level.ChallengeAppearancesTexts.Add(i, appearancesText);
            i++;
        }


        var json = JsonConvert.SerializeObject(level);
        File.WriteAllText(path, json);
    }
    public void SaveCharacter(string path)
    {
        JsonUtilityManager.SetPath(path);
        JsonUtilityManager.Save();
    }
    private bool AddingFilesToAsset(int levelID, string[] files, int i, LootLocker.LootLockerEnums.FilePurpose filePurpose)
    {
        if (i >= files.Length)
        {
            LootLockerSDKManager.UpdatingAnAssetCandidate(levelID, true, (updatedResponse) => { });
            LevelUploadUi.SetActive(false);
            LevelUploadUi.transform.Find("Background/Foreground/Buttons/Button").GetComponent<Button>().interactable = true;
            LevelUploadUi.transform.Find("Background/Foreground/Buttons/Close").GetComponent<Button>().interactable = true;
            return true;
        }
        while (true)
        {
            if (i<files.Length && files[i].EndsWith("json") && !files[i].EndsWith("Data.json"))
                break;
            i++;
            if (i >= files.Length)
            {
                LootLockerSDKManager.UpdatingAnAssetCandidate(levelID, true, (updatedResponse) => { });
                LevelUploadUi.SetActive(false);
                LevelUploadUi.transform.Find("Background/Foreground/Buttons/Button").GetComponent<Button>().interactable = true;
                LevelUploadUi.transform.Find("Background/Foreground/Buttons/Close").GetComponent<Button>().interactable = true;
                return true;
            }
        }

        string json;
        json = File.ReadAllText(files[i]);
        File.WriteAllText(files[i].Replace(".json", ".txt"), json);

        LootLockerSDKManager.AddingFilesToAssetCandidates(levelID, files[i].Replace(".json", ".txt"), Path.GetFileName(files[i].Replace(".json", ".txt")), filePurpose, (textResponse) =>
        {
            if (textResponse.success)
            {
                i++;
                AddingFilesToAsset(levelID, files, i, filePurpose);
                // if (i == files.Length)
                // {
                    // LevelUploadUi.SetActive(false);
                    // LevelUploadUi.transform.Find("Background/Foreground/Buttons/Button").GetComponent<Button>().interactable = true;
                    // LevelUploadUi.transform.Find("Background/Foreground/Buttons/Close").GetComponent<Button>().interactable = true;
                // }
            }
            else
            {
                ErrorText.GetComponent<TMP_Text>().text = "Level Creation Failed";
                Debug.Log("Level Data Upload Failed");
            }
        });
        return false;
    }
    public void CheckName()
    {
        Button UploadButton = LevelUploadUi.transform.Find("Background/Foreground/Buttons/Button").GetComponent<Button>();
        Button CloseButton = LevelUploadUi.transform.Find("Background/Foreground/Buttons/Close").GetComponent<Button>();
        UploadButton.interactable = false;
        CloseButton.interactable = false;
        LootLockerSDKManager.GetAssetListWithCount(1000, (response) =>
        {
            for (int i = 0; i < response.assets.Length; i++)
            {
                if (response.assets[i].name == LevelNameInputField.text)
                {
                    ErrorText.GetComponent<TMP_Text>().text = "A level with this name already exists. Please modify it.";
                    ErrorText.SetActive(true);
                    UploadButton.interactable = true;
                    CloseButton.interactable = true;
                    return;
                }
            }
            ErrorText.SetActive(false);
            CreateLevel();
        }, null, true, new Dictionary<string, string>() { { "Context", "TileMap" } });
    }
}
