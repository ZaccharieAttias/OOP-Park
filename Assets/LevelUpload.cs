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

public class LevelUpload : MonoBehaviour
{
    private TMP_InputField LevelNameInputField;
    private string LevelName;
    private GameObject LevelUploadUi;

    private TileMap _groundMap;
    private TileMap _coverMap;
    private TileMap _propsMap;
    private LevelBuilderB LevelBuilder;

    public void Start()
    {
        LevelUploadUi = GameObject.Find("Canvas/Menus/Gameplay/UploadScreen");
        LevelNameInputField = LevelUploadUi.transform.Find("InputField").GetComponent<TMP_InputField>();
        LevelBuilder = GameObject.Find("Grid/LevelBuilder").GetComponent<LevelBuilderB>();

        _groundMap = LevelBuilder.GetGroundMap();
        _coverMap = LevelBuilder.GetCoverMap();
        _propsMap = LevelBuilder.GetPropsMap();

        LevelUploadUi.SetActive(false);
    }
    public void CreateLevel()
    {
        LevelName = LevelNameInputField.text;
        OpenUploadLevelUI();
        LootLockerSDKManager.CreatingAnAssetCandidate(LevelName, (response) =>
        {
            if (response.success)
            {
                UploadLevelData((int)response.asset_candidate.id);
            }
            else
            {
                Debug.Log("Level Creation Failed");
            }
        });
    }
    public void UploadLevelData(int levelID)
    {
        string screenshotFilePath = "Assets/Resources/Screenshots/Level_" + LevelName + ".png";
        LootLocker.LootLockerEnums.FilePurpose screenshotFileType = LootLocker.LootLockerEnums.FilePurpose.primary_thumbnail;

        LootLockerSDKManager.AddingFilesToAssetCandidates(levelID, screenshotFilePath, "Level_" + LevelName + ".png", screenshotFileType, (screenshotResponse) =>
        {
            if (screenshotResponse.success)
            {
                string textFilePath = Directory.GetCurrentDirectory() + "/Assets/Resources/Screenshots/" + "Level_" + LevelName + ".json";
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
    }
    public void OpenUploadLevelUI()
    {
        StartCoroutine(WaitSreenShot());
    }
    IEnumerator WaitSreenShot()
    {
        TakeScreenshot();
        yield return new WaitForSeconds(1.0f);
    }
    public void TakeScreenshot()
    {
        string filepath = Directory.GetCurrentDirectory() + "/Assets/Resources/Screenshots/";
        ScreenCapture.CaptureScreenshot(Path.Combine(filepath, "Level_" + LevelName + ".png"));
    }
    public void SaveLevel(string path)
    {
        if (_groundMap == null) return;

        var width = _groundMap.Width;
        var height = _groundMap.Height;
        var depth = _groundMap.Depth;
        var level = new Level(width, height, depth);

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

        var json = JsonConvert.SerializeObject(level);
        File.WriteAllText(path, json);
    }
}
