using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.Networking;
using LootLocker.Requests;
using Newtonsoft.Json;
using Assets.PixelFantasy.PixelTileEngine.Scripts;
using UnityEngine.Tilemaps;
using LLlibs.ZeroDepJson;
using Unity.VisualScripting;
using System;
using System.Linq;
using System.Text;
using UnityEditor;

public class LevelUpload : MonoBehaviour
{
    private TMP_InputField LevelNameInputField;
    private string LevelName;
    private GameObject LevelUploadUi;

    private TileMap _groundMap;
    private TileMap _coverMap;
    private TileMap _propsMap;
    private LevelBuilderB LevelBuilder;
    private int TileMapContextID;
    private int CharacterTreeContextID;
    public JsonUtilityManager JsonUtilityManager;

    public void Start()
    {
        LevelUploadUi = GameObject.Find("Canvas/Menus/Gameplay/UploadScreen");
        LevelNameInputField = LevelUploadUi.transform.Find("Background/Foreground/InputField").GetComponent<TMP_InputField>();
        LevelBuilder = GameObject.Find("Grid/LevelBuilder").GetComponent<LevelBuilderB>();
        JsonUtilityManager = GameObject.Find("GameInitializer").GetComponent<JsonUtilityManager>();

        _groundMap = LevelBuilder.GetGroundMap();
        _coverMap = LevelBuilder.GetCoverMap();
        _propsMap = LevelBuilder.GetPropsMap();

        TileMapContextID = 235247;
        CharacterTreeContextID = 235353;

        LevelUploadUi.SetActive(false);
    }
    public void CreateLevel()
    {
        LevelName = LevelNameInputField.text;
        LevelUploadUi.SetActive(false);
        string path = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Resources", "Screenshots", "Saved", LevelName);
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
                return;
            }
        }, filters: new Dictionary<string, string>() { { "Context", "TileMap" } } , context_id: TileMapContextID);

        // Upload Level Data (character center)
        LootLockerSDKManager.CreatingAnAssetCandidate(LevelName , (response) =>
        {
            if (response.success)
            {
                UploadLevelTreeData((int)response.asset_candidate.id);
            }
            else
            {
                Debug.Log("Character Center Creation Failed");
                return;
            }
        }, filters: new Dictionary<string, string>() { { "Context", "CharacterTree" } } , context_id: CharacterTreeContextID);
    }
    public void UploadLevelData(int levelID)
    {
        string screenshotFilePath = "Assets/Resources/Screenshots/Saved/" + LevelName + "/Level_" + LevelName + ".png";
        LootLocker.LootLockerEnums.FilePurpose screenshotFileType = LootLocker.LootLockerEnums.FilePurpose.primary_thumbnail;

        LootLockerSDKManager.AddingFilesToAssetCandidates(levelID, screenshotFilePath, "Level_" + LevelName + ".png", screenshotFileType, (screenshotResponse) =>
        {
            if (screenshotResponse.success)
            {
                string textFilePath = Directory.GetCurrentDirectory() + "/Assets/Resources/Screenshots/Saved/" + LevelName + "/Level_" + LevelName + "_Data.json";
                SaveLevel(textFilePath);

                //coverting json file to txt file
                string json = File.ReadAllText(textFilePath);
                File.WriteAllText(textFilePath, json);

                LootLocker.LootLockerEnums.FilePurpose textFileType = LootLocker.LootLockerEnums.FilePurpose.file;

                LootLockerSDKManager.AddingFilesToAssetCandidates(levelID, textFilePath, "Level_" + LevelName + "_Data.txt", textFileType, (textResponse) =>
                {
                    if (textResponse.success)
                    {
                        LootLockerSDKManager.UpdatingAnAssetCandidate(levelID, true, (updatedResponse) =>{});
                    }
                    else
                    {
                        Debug.Log("Level Data Upload Failed");
                    }
                });
            }
            else
            {
                Debug.Log("Screenshot Upload Failed");
            }
        });

        AssetDatabase.Refresh();
        string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Resources", "Screenshots", "Saved", LevelName));
        foreach (string file in files)
        {
            if (file.Contains(".txt"))
            {
                File.Delete(file);
            }
        }
        AssetDatabase.Refresh();
    }
    public void UploadLevelTreeData(int levelID)
    {
        string textFilePath = Directory.GetCurrentDirectory() + "/Assets/Resources/Screenshots/Saved/" + LevelName;
        SaveCharacter(textFilePath);

        // string json;
        LootLocker.LootLockerEnums.FilePurpose textFileType = LootLocker.LootLockerEnums.FilePurpose.file;

        string[] files = Directory.GetFiles(textFilePath);

        AddingFilesToAsset(levelID, files, 0, textFileType);

        AssetDatabase.Refresh();
        string[] filess = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Resources", "Screenshots", "Saved", LevelName));
        foreach (string file in filess)
        {
            if (file.Contains(".txt"))
            {
                File.Delete(file);
            }
        }
        AssetDatabase.Refresh();
    }
    public void OpenUploadLevelUI()
    {
        StartCoroutine(WaitScreenShot());
    }
    IEnumerator WaitScreenShot()
    {
        TakeScreenshot();
        yield return new WaitForSeconds(1.0f);
    }
    public void TakeScreenshot()
    {
        string filepath = Directory.GetCurrentDirectory() + "/Assets/Resources/Screenshots/Saved/" + LevelName + "/";
        ScreenCapture.CaptureScreenshot(Path.Combine(filepath, "Level_" + LevelName + ".png"));
    }
    public void SaveLevel(string path)
    {
        if (_groundMap == null) return;

        var width = _groundMap.Width;
        var height = _groundMap.Height;
        var depth = _groundMap.Depth;
        var level = new LevelB(width, height, depth, 0, 0);

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

                    level.GroundMap[x, y, z] = groundIndex + 1;
                    level.CoverMap[x, y, z] = coverIndex + 1;
                    level.PropsMap[x, y, z] = propsIndex + 1;
                }
            }
        }

        var player = GameObject.Find("Player");
        Transform playerTransform = player.transform;

        level.characterX = playerTransform.localPosition.x;
        level.characterY = playerTransform.localPosition.y;

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
            LootLockerSDKManager.UpdatingAnAssetCandidate(levelID, true, (updatedResponse) =>{});
            return true;
        }
        while (!files[i].Contains(".json") || files[i].Contains("_Data"))
        {
            i++;
            if (i >= files.Length)
            {
            LootLockerSDKManager.UpdatingAnAssetCandidate(levelID, true, (updatedResponse) =>{});
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
            }
            else
            {
                Debug.Log("Level Data Upload Failed");
            }
        });
        return false;
    }
}
