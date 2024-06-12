using System.Collections;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using Assets.PixelFantasy.PixelTileEngine.Scripts;
using HeroEditor.Common;
using LootLocker.Requests;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEditor;

public class LevelInitializer : MonoBehaviour
{
    private Transform Parent;
    public SpriteCollectionPF SpriteCollection;
    //public SpriteRenderer Cursor;

    private int _type;
    private int _index;
    private int _layer;
    private readonly bool[] _layersEnabled = { true, true, true, true };
    private Position _positionMin;

    private TileMap _groundMap;
    private TileMap _coverMap;
    private TileMap _propsMap;

    private Transform Terrain;
    private Transform Walls;
    public GameObject PlayerPrefab;
    private CharacterEditor1 CharacterEditor1;
    private GameObject MainCamera;
    private float x_position = -4;
    private float y_position = -1;

    string path;
    private LevelUpload LevelUpload;
    string MapPath;

    private int ID;
    private string LevelName;
    public TMP_Text NameText;
    public Image LevelIcon;
    public string TextFileURL;
    
    public void Start()
    {
        _groundMap = new TileMap(1, 1, 4);
        _coverMap = new TileMap(1, 1, 4);
        _propsMap = new TileMap(1, 1, 4);
        
        Parent = GameObject.Find("Grid/LevelBuilder").transform;
        Terrain = Parent.Find("Terrain");
        Walls = Parent.Find("Walls");
        CharacterEditor1 = GameObject.Find("Scripts/CharacterEditor").GetComponent<CharacterEditor1>();
        MainCamera = GameObject.Find("Main Camera");
        LevelUpload = GameObject.Find("LevelManager").GetComponent<LevelUpload>();
        
        transform.position = Vector3.zero;
        transform.localScale = Vector3.one;

        NameText.text = LevelName;
        LevelIcon.sprite = LevelIcon.sprite;
    }
//temporarily load the level for testing
    public void LoadLevelForTesting()
    {
        MapPath = Path.Combine(Application.dataPath, "Resources/Json", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, "Level.json");
        BuildLevel(MapPath);
        SetLayers(Terrain, "Ground");
        SetPlayers();
    }
    public void LoadLevel()
    {
        StartCoroutine(DownloadTextFile(TextFileURL));
    }
    private IEnumerator DownloadTextFile(string textfileURL)
    {
        UnityWebRequest www = UnityWebRequest.Get(textfileURL);
        yield return www.SendWebRequest();

        string filePath = Directory.GetCurrentDirectory() + "/Assets/Resources/Screenshots/Level_" + LevelName + "_Data.json";
        File.WriteAllText(filePath, www.downloadHandler.text);

        AssetDatabase.Refresh();
        yield return new WaitForSeconds(1f);
        BuildLevel(filePath);
        SetLayers(Terrain, "Ground");
        SetPlayers();
        yield return new WaitForSeconds(2f);
        GameObject.Find("Canvas/Menus/Gameplay/DownloadScreen").SetActive(false);
    }
    public void SetInformations(int id, string name)
    {
        ID = id;
        LevelName = name;
    }
    private void BuildLevel(string json)
    {   
        var file = File.ReadAllText(json);
        var level = JsonConvert.DeserializeObject<Level>(file);

        if (_groundMap != null)
        {
            for (var x = 0; x < _groundMap.Width; x++)
            {
                for (var y = 0; y < _groundMap.Height; y++)
                {
                    for (var z = 0; z < _groundMap.Depth; z++)
                    {
                        _groundMap.Destroy(x, y, z);
                        _coverMap.Destroy(x, y, z);
                        _propsMap.Destroy(x, y, z);
                    }
                }
            }
        }

        var width = level.GroundMap.GetLength(0);
        var height = level.GroundMap.GetLength(1);
        var depth = level.GroundMap.GetLength(2);

        _groundMap = new TileMap(width, height, depth);
        _coverMap = new TileMap(width, height, depth);
        _propsMap = new TileMap(width, height, depth);
        _positionMin = new Position(-width / 2, -height / 2);

        var index = _index;

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                for (var z = 0; z < depth; z++)
                {
                    var ground = level.GroundMap[x, y, z] == 0 ? null : level.TileTable[level.GroundMap[x, y, z] - 1];
                    var cover = level.CoverMap[x, y, z] == 0 ? null : level.TileTable[level.CoverMap[x, y, z] - 1];
                    var props = level.PropsMap[x, y, z] == 0 ? null : level.TileTable[level.PropsMap[x, y, z] - 1];

                    if (ground != null)
                    {
                        _type = 0;
                        _index = SpriteCollection.GroundTilesets.FindIndex(i => i.Name == ground);

                        if (_index != -1)
                        {
                            CreateGround(x, y, z);
                        }
                    }

                    if (cover != null)
                    {
                        _type = 1;
                        _index = SpriteCollection.CoverTilesets.FindIndex(i => i.Name == cover);

                        if (_index != -1)
                        {
                            CreateCover(x, y, z);
                        }
                    }

                    if (props != null)
                    {
                        _type = 2;
                        _index = SpriteCollection.PropsSprites.FindIndex(i => i.name == props);

                        if (_index == -1)
                        {
                            _type = 3;
                            _index = SpriteCollection.OtherSprites.FindIndex(i => i.name == props);
                        }

                        if (_index != -1)
                        {
                            CreateProps(x, y, z);
                        }
                    }
                }
            }
        }

        _index = index;
    }
    private void CreateGround(int x, int y, int z)
    {
        if (x < 0 || x >= _groundMap.Width || y < 0 || y >= _groundMap.Height) return;

        _groundMap.Destroy(x, y, z);

        if (_index != -1)
        {
            var block = new Block(SpriteCollection.GroundTilesets[_index].Name);

            block.Transform.SetParent(Terrain.Find("Ground").transform);
            block.Transform.localPosition = new Vector3(_positionMin.X + x, _positionMin.Y + y);
            block.Transform.localScale = Vector3.one;
            block.SpriteRenderer.sprite = SpriteCollection.GroundTilesets[_index].Sprites[0];
            block.GameObject.AddComponent<BoxCollider2D>().offset = new Vector3(0, 0.5f);

            _groundMap[x, y, z] = block;
        }

        for (var dx = -1; dx <= 1; dx++)
        {
            for (var dy = -1; dy <= 1; dy++)
            {
                SetGround(x + dx, y + dy, z);
            }
        }

        for (var dx = -1; dx <= 1; dx++)
        {
            for (var dy = -1; dy <= 1; dy++)
            {
                SetCover(x + dx, y + dy, z);
            }
        }
    }
    private void CreateCover(int x, int y, int z)
    {
        if (x < 0 || x >= _coverMap.Width || y < 0 || y >= _coverMap.Height) return;

        _coverMap.Destroy(x, y, z);

        if (_groundMap[x, y, z] == null || _groundMap[x, y + 1, z] != null)
        {
            Debug.LogWarning("Covers can be placed over the ground only.");
            return;
        }

        if (_index != -1)
        {
            var block = new Block(SpriteCollection.CoverTilesets[_index].Name);

            block.Transform.SetParent(Terrain.Find("Cover").transform);
            block.Transform.localPosition = new Vector3(_positionMin.X + x, _positionMin.Y + y);
            block.Transform.localScale = Vector3.one;
            block.SpriteRenderer.sprite = SpriteCollection.CoverTilesets[_index].Sprites[0];

            _coverMap[x, y, z] = block;
        }
        
        for (var i = -1; i <= 1; i++)
        {
            for (var j = -1; j <= 1; j++)
            {
                SetCover(x + i, y + j, z);
            }
        }
    }
    private void CreateProps(int x, int y, int z)
    {
        if (x < 0 || x >= _propsMap.Width || y <= 0 || y >= _propsMap.Height) return;

        if (_index != -1 && _type == 2 && (_groundMap[x, y, z] != null || _groundMap[x, y - 1, z] == null))
        {
            Debug.LogWarning("Props can be placed on the ground only.");
            return;
        }

        _propsMap.Destroy(x, y, z);

        if (_index == -1) return;

        var sprites = _type == 2 ? SpriteCollection.PropsSprites : SpriteCollection.OtherSprites;
        var block = new Block(sprites[_index].name);

        block.Transform.SetParent(Terrain.Find("Props").transform);
        block.Transform.localPosition = new Vector3(_positionMin.X + x, _positionMin.Y + y);
        block.Transform.localScale = Vector3.one;
        block.SpriteRenderer.sprite = sprites[_index];
        block.SpriteRenderer.sortingOrder = 100 * z + 30;

        if (_type == 2 && _index != -1)
        {
            block.OffsetY = -1;
            block.Transform.localPosition -= new Vector3(0, 1f / 16f);
        }

        _propsMap[x, y, z] = block;
    }
    private void SetGround(int x, int y, int z)
    {
        if (x < 0 || x >= _groundMap.Width || y < 0 || y >= _groundMap.Height) return;
        if (_groundMap[x, y, z] == null) return;

        var tileset = SpriteCollection.GroundTilesets.Single(i => i.Sprites.Contains(_groundMap[x, y, z].SpriteRenderer.sprite));
        var map = _groundMap.GetBitmap(x, y, z);
        var mask = GetMask(x, y, map);
        var sprite = tileset.ResolveSprite(mask, out var flipX);

        if (sprite == null)
        {
            _groundMap.Destroy(x, y, z);
        }
        else
        {
            _groundMap[x, y, z].SpriteRenderer.sprite = sprite;
            _groundMap[x, y, z].SpriteRenderer.flipX = flipX;
            _groundMap[x, y, z].SpriteRenderer.sortingOrder = 100 * z + 10;
        }
    }  
    private void SetCover(int x, int y, int z)
    {
        if (x < 0 || x >= _groundMap.Width || y < 0 || y >= _groundMap.Height) return;
        if (_coverMap[x, y, z] == null) return;

        if (_groundMap[x, y, z] == null || _groundMap[x, y + 1, z] != null)
        {
            _coverMap.Destroy(x, y, z);
            return;
        }

        _coverMap[x, y, z].SpriteRenderer.flipX = false;
        _coverMap[x, y, z].SpriteRenderer.sortingOrder = 100 * z + 20;

        var tileset = SpriteCollection.CoverTilesets.Single(i => i.Sprites.Contains(_coverMap[x, y, z].SpriteRenderer.sprite));
        var map = _coverMap.GetBitmap(x, y, z);
        var mask = GetMask(x, y, map);

        if (_groundMap[x - 1, y + 1, z] != null && _groundMap[x - 1, y, z] != null) mask[1, 0] = 2;
        if (_groundMap[x + 1, y + 1, z] != null && _groundMap[x + 1, y, z] != null) mask[1, 2] = 2;

        var sprite = tileset.ResolveSprite(mask, out var flipX);

        if (sprite == null)
        {
            _coverMap.Destroy(x, y, z);
        }
        else
        {
            _coverMap[x, y, z].SpriteRenderer.sprite = sprite;
            _coverMap[x, y, z].SpriteRenderer.flipX = flipX;
        }
    }
    private static int[,] GetMask(int x, int y, int[,] map)
    {
        return new[,]
        {
            {
                x > 0 && y < map.GetLength(1) - 1 && map[x - 1, y + 1] == 1 && map[x - 1, y] == 1 && map[x, y + 1] == 1 ? 1 : 0,
                y < map.GetLength(1) - 1 && map[x, y + 1] == 1 ? 1 : 0,
                x < map.GetLength(0) - 1 && y < map.GetLength(1) - 1 && map[x + 1, y + 1] == 1 && map[x + 1, y] == 1 && map[x, y + 1] == 1 ? 1 : 0
            },
            {
                x > 0 && map[x - 1, y] == 1 ? 1 : 0,
                1,
                x < map.GetLength(0) - 1 && map[x + 1, y] == 1 ? 1 : 0 },
            {
                x > 0 && y > 0 && map[x - 1, y - 1] == 1 && map[x - 1, y] == 1 && map[x, y - 1] == 1 ? 1 : 0,
                y > 0 && map[x, y - 1] == 1 ? 1 : 0,
                x < map.GetLength(0) - 1 && y > 0 && map[x + 1, y - 1] == 1 && map[x + 1, y] == 1 && map[x, y - 1] == 1 ? 1 : 0
            }
        };
    }
    private void SetLayers(Transform LocalParent, string name)
    {
        foreach (Transform child in LocalParent)
        {
            if (child.gameObject.layer != LayerMask.NameToLayer(name))
                child.gameObject.layer = LayerMask.NameToLayer(name);
            else
                SetLayers(child, name);
        }
    }
    private void SetPlayers()
    {
        GameObject Player = Instantiate(PlayerPrefab, new Vector3(x_position, y_position, 0), Quaternion.identity);
        Player.name = "Player";
        CharacterEditor1.Character = Player.GetComponent<CharacterBase>();
        CharacterEditor1.OnlineLoadFromJson();
        Player.SetActive(true);
        MainCamera.GetComponent<CameraFollow>().Player = Player;
    }
}