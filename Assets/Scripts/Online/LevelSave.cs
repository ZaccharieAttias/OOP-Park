using Assets.PixelFantasy.PixelTileEngine.Scripts;
using Newtonsoft.Json;
using System.IO;
using TMPro;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelSave : MonoBehaviour
{
    private TMP_InputField LevelNameInputField;
    private string LevelName;
    private GameObject LevelSaveUi;

    private TileMap _groundMap;
    private TileMap _coverMap;
    private TileMap _propsMap;
    private TileMap _wallMap;
    private TileMap _gameplayMap;
    private LevelBuilderB LevelBuilder;

    public JsonUtilityManager JsonUtilityManager;

    public void Start()
    {
        LevelSaveUi = GameObject.Find("Canvas/Menus/Gameplay/SavingScreen");
        LevelNameInputField = LevelSaveUi.transform.Find("Background/Foreground/InputField").GetComponent<TMP_InputField>();
        LevelBuilder = GameObject.Find("Grid/LevelBuilder").GetComponent<LevelBuilderB>();
        JsonUtilityManager = GameObject.Find("GameInitializer").GetComponent<JsonUtilityManager>();
    }
    public void CreateLevel()
    {
        LevelName = LevelNameInputField.text;
        _groundMap = LevelBuilder.GetGroundMap();
        _coverMap = LevelBuilder.GetCoverMap();
        _propsMap = LevelBuilder.GetPropsMap();
        _wallMap = LevelBuilder.GetWallMap();
        _gameplayMap = LevelBuilder.GetGameplayMap();
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

        // Save Level Data (the gameplay)
        SaveLevelData();

        // Save Level Data (character center)
        SaveLevelTreeData();

        LevelSaveUi.SetActive(false);
    }
    public void SaveLevelData()
    {
        string textFilePath = Directory.GetCurrentDirectory() + "/Assets/Resources/Screenshots/Saved/" + LevelName + "/Level_" + LevelName + "_Data.json";
        SaveLevel(textFilePath);

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
    public void SaveLevelTreeData()
    {
        string textFilePath = Directory.GetCurrentDirectory() + "/Assets/Resources/Screenshots/Saved/" + LevelName;
        SaveCharacter(textFilePath);

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
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

                    var wallsIndex = -1;

                    if (_wallMap[x, y, z] != null)
                    {
                        wallsIndex = level.AddTexture(_wallMap[x, y, z].SpriteRenderer.sprite.name);
                    }

                    var gameplayIndex = -1;

                    if (_gameplayMap[x, y, z] != null)
                    {
                        gameplayIndex = level.AddTexture(_gameplayMap[x, y, z].SpriteRenderer.sprite.name);
                    }

                    level.GroundMap[x, y, z] = groundIndex + 1;
                    level.CoverMap[x, y, z] = coverIndex + 1;
                    level.PropsMap[x, y, z] = propsIndex + 1;
                    level.WallMap[x, y, z] = wallsIndex + 1;
                    level.GameplayMap[x, y, z] = gameplayIndex + 1;

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
}
