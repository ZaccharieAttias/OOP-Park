using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using Assets.PixelFantasy.PixelTileEngine.Scripts;
using Assets.HeroEditor.Common.Scripts.EditorScripts;
using HeroEditor.Common;
using Newtonsoft.Json;
using System.Collections;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelInitializer : MonoBehaviour
{
    private Transform Parent;
    public SpriteCollectionPF SpriteCollection;

    private int _type;
    private int _index;
    private int _layer;
    private readonly bool[] _layersEnabled = { true, true, true, true };
    private Position _positionMin;

    private TileMap _groundMap;
    private TileMap _coverMap;
    private TileMap _propsMap;
    private TileMap _wallMap;
    private TileMap _gameplayMap;

    private Transform Terrain;
    private Transform Walls;
    public GameObject PlayerPrefab;
    private CharacterEditor1 CharacterEditor1;
    private CharacterEditor Charactereditor;
    private GameObject MainCamera;
    private float x_position;
    private float y_position;

    string path;
    private LevelDownload LevelDownload;
    private LevelLoad LevelLoad;
    string MapPath;

    private int ID;
    private string LevelName;
    public TMP_Text NameText;
    public Image LevelIcon;
    public string DataFileURL;
    public string CharactersPath;
    public string AttributesPath;
    public string MethodssPath;
    public string SpecialAbilitiesPath;
    public JsonUtilityManager JsonUtilityManager;
    public bool start = true;
    public int missionNumber = 1;
    public GameObject Mission1Prefab;
    public GameObject MissionPrefab;
    public void Start()
    {
        _groundMap = new TileMap(1, 1, 4);
        _coverMap = new TileMap(1, 1, 4);
        _propsMap = new TileMap(1, 1, 4);
        _wallMap = new TileMap(1, 1, 4);
        _gameplayMap = new TileMap(1, 1, 4);

        Parent = GameObject.Find("Grid/LevelBuilder").transform;
        Terrain = Parent.Find("Terrain");
        Walls = Parent.Find("Walls");
        CharacterEditor1 = GameObject.Find("Scripts/CharacterEditor").GetComponent<CharacterEditor1>();
        Charactereditor = GameObject.Find("Scripts/CharacterEditor").GetComponent<CharacterEditor>();
        MainCamera = GameObject.Find("Main Camera");
        LevelDownload = GameObject.Find("LevelManager").GetComponent<LevelDownload>();
        if (GameObject.Find("LevelManager").GetComponent<LevelLoad>() != null)
            LevelLoad = GameObject.Find("LevelManager").GetComponent<LevelLoad>().GetComponent<LevelLoad>();
        JsonUtilityManager = GameObject.Find("GameInitializer").GetComponent<JsonUtilityManager>();

        transform.position = Vector3.zero;
        transform.localScale = Vector3.one;

        NameText.text = LevelName;
        LevelIcon.sprite = LevelIcon.sprite;
    }
    public void DownloadLevel()
    {
        StartCoroutine(DownloadTextFile(DataFileURL));
    }
    public void LoadLevel()
    {
        GameObject.Find("Grid/LevelBuilder").GetComponent<LevelBuilderB>().Draw(new Vector2(-65, 24));
        GameObject.Find("Grid/LevelBuilder").GetComponent<LevelBuilderB>().Draw(new Vector2(196, 24));
        GameObject.Find("Grid/LevelBuilder").GetComponent<LevelBuilderB>().Draw(new Vector2(-65, -25));
        GameObject.Find("Grid/LevelBuilder").GetComponent<LevelBuilderB>().Draw(new Vector2(196, -25));
        start = false;
        string path = Path.Combine(Application.dataPath, "StreamingAssets", "Resources", "Screenshots", "Saved", LevelName);
        GameObject.Find("Grid/LevelBuilder").GetComponent<LevelBuilderB>().Load(path, LevelName);
    }
    private IEnumerator DownloadTextFile(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        string path = Path.Combine(Application.dataPath, "StreamingAssets", "Resources", "Screenshots", "Download", LevelName);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        string filePath = Application.dataPath + "/StreamingAssets" + "/Resources/Screenshots/Download/" + LevelName + "/Level_" + LevelName + "_Data.json";
        File.WriteAllText(filePath, www.downloadHandler.text);

        LevelDownload.DownloadLevelTreeData(LevelName);

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
        yield return new WaitForSeconds(2f);
        Mission1Prefab = MissionPrefab;
        BuildLevel(filePath);
        SetLayers(Terrain.Find("Ground"), "Ground");
        SetPlayers();
        JsonUtilityManager.SetPath(path);
        JsonUtilityManager.Load();
        foreach (var character in CharactersData.CharactersManager.CharactersCollection)
            character.IsOriginal = true;
        JsonUtilityManager.Save();
        CharacterEditor1.LoadFromJson();
        GameObject.Find("Canvas/Popups").GetComponent<CharacterAppearanceManager>().InitializeCharacterComponents();
        GameObject.Find("Canvas/Popups").GetComponent<CharacterAppearanceManager>().InitializeOverride();
        if (RestrictionManager.Instance.AllowSingleInheritance)
        {
            GameObject.Find("Scripts/CharacterEditor").GetComponent<SwapScreen>().SwapButtonToCharacterCenter.onClick.AddListener(() => GameObject.Find("Canvas/Popups").GetComponent<CharactersCreationManager>().ToggleOn());
            GameObject.Find("Canvas/Popups/CharacterCreation/Buttons/Add").SetActive(true);
            
            Button popupToggleOn = GameObject.Find("Canvas/Menus/Gameplay/SwapScreen").GetComponent<Button>();
            popupToggleOn.onClick.AddListener(GameObject.Find("Canvas/Popups").GetComponent<CharactersCreationManager>().ToggleOn);

            Button popupToggleOff = GameObject.Find("Canvas/Menus/CharacterCenter/SwapScreen").GetComponent<Button>();
            popupToggleOff.onClick.AddListener(GameObject.Find("Canvas/Popups").GetComponent<CharactersCreationManager>().ToggleOff);
        
            GameObject.Find("Canvas/Popups").GetComponent<CharacterAppearanceManager>().InitializeOverride();
        }
        if(RestrictionManager.Instance.AllowOverride || RestrictionManager.Instance.AllowUpcasting)
            GameObject.Find("Canvas/Menus/Gameplay/SwapScreen").GetComponent<Button>().onClick.AddListener(() => GameObject.Find("Canvas/Popups").GetComponent<CharacterChallengeManager>().BackStage());
        var chapterInfo = new ChapterInfo()
        {
            ChapterNumber = 0,
            Name = LevelName,
            LevelsInfo = new System.Collections.Generic.List<LevelInfo>()
            {
                new LevelInfo()
                {
                    LevelNumber = 1,
                    Status = 0
                }
            }
        };

        bool chapterExists = SceneManagement.GameplayInfo[1].ChapterInfos.Exists(c => c.Name == LevelName);
        if (!chapterExists)
        {
            SceneManagement.GameplayInfo[1].ChapterInfos.Add(chapterInfo);
        }

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
        var level = JsonConvert.DeserializeObject<LevelB>(file);

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
                        _wallMap.Destroy(x, y, z);
                        _gameplayMap.Destroy(x, y, z);
                    }
                }
            }
        }

        var width = level.GroundMap.GetLength(0);
        var height = level.GroundMap.GetLength(1);
        var depth = level.GroundMap.GetLength(2);
        x_position = level.characterX;
        y_position = level.characterY;
        Dictionary<int, List<string>> ChallengeAppearancesConditions = new Dictionary<int, List<string>>();
        Dictionary<int, List<string>> ChallengeAppearancesTexts = new Dictionary<int, List<string>>();
        ChallengeAppearancesConditions = level.ChallengeAppearancesConditions;
        ChallengeAppearancesTexts = level.ChallengeAppearancesTexts;

        _groundMap = new TileMap(width, height, depth);
        _coverMap = new TileMap(width, height, depth);
        _propsMap = new TileMap(width, height, depth);
        _wallMap = new TileMap(width, height, depth);
        _gameplayMap = new TileMap(width, height, depth);
        _positionMin = new Position(66f - width / 2, -height / 2);

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
                    var walls = level.WallMap[x, y, z] == 0 ? null : level.TileTable[level.WallMap[x, y, z] - 1];
                    var gameplay = level.GameplayMap[x, y, z] == 0 ? null : level.TileTable[level.GameplayMap[x, y, z] - 1];

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

                    if (walls != null)
                    {
                        _type = 4;
                        _index = SpriteCollection.WallTilesets.FindIndex(i => i.name == walls);

                        if (_index != -1)
                        {
                            CreateWall(x, y, z);
                        }
                    }

                    if (props != null)
                    {
                        _type = 2;
                        _index = SpriteCollection.PropsSprites.FindIndex(i => i.name == props);

                        if (_index != -1)
                        {
                            CreateProps(x, y, z);
                        }
                    }

                    if (gameplay != null)
                    {
                        _type = 5;
                        _index = SpriteCollection.GamePlaySprite.FindIndex(i => i.name == gameplay);
                        if (_index != -1)
                        {
                            switch (_index)
                            {
                                case 1:
                                case 2:
                                case 3: CreateCheckPoint(x, y, z); break;
                                case 4: CreateEndPoint(x, y, z); break;
                                case 5:
                                case 6: CreateBrick(x, y, z); break;
                                case 7: CreateDeathZone(x, y, z); break;
                                case 8:
                                case 10: CreateReverseDeathObject(x, y, z); break;
                                case 9:
                                case 11: CreateDeathObject(x, y, z); break;
                                case 12: CreateGrabbableObject(x, y, z, 49); break;
                                case 13: CreateGrabbableObject(x, y, z, 99); break;
                                case 14: CreateGrabbableObject(x, y, z, 199); break;
                                case 15: CreateGrabbableObject(x, y, z, 499); break;
                                case 16: CreateChallengeForOverriding(x, y, z); break;
                            }
                        }
                    }
                }
            }
        }
        //set all the challenges 
        List<GameObject> missions = new List<GameObject>();
        foreach (Transform child in GameObject.Find("Canvas/Popups").transform)
        {
            if (child.name.Contains("Mission"))
                missions.Add(child.gameObject);
        }
        for (int i = 0; i < missions.Count; i++)
        {
            GameObject mission = missions[i];
            int indexxx = int.Parse(mission.name.Substring(7));
            List<string> appearancesCondition = new List<string>();
            List<string> appearancesText = new List<string>();
            appearancesCondition = ChallengeAppearancesConditions[indexxx];
            appearancesText = ChallengeAppearancesTexts[indexxx];
            TMP_Text txt = mission.transform.Find("Background/Foreground/Mssion/Mission").GetComponent<TMP_Text>();
            txt.text = level.ChallengeAppearancesTexts[indexxx][0];
            GameObject.Find("Canvas/Popups").GetComponent<CharacterChallengeManager>().InitializeUIOnlineElements(indexxx, appearancesCondition);
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
            block.GameObject.layer = 7;

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
    public void CreateWall(int x, int y, int z)
    {
        if (x < 0 || x >= _wallMap.Width || y < 0 || y >= _wallMap.Height) return;

        _wallMap.Destroy(x, y, z);

        if (_groundMap[x, y, z] != null)
        {
            Debug.LogWarning("Wall can not be placed on a ground");
            return;
        }

        if (_index != -1)
        {
            var block = new Block(SpriteCollection.WallTilesets[_index].name);

            block.Transform.SetParent(Parent.Find("Walls").transform);
            block.Transform.localPosition = new Vector3(_positionMin.X + x, _positionMin.Y + y);
            block.Transform.localScale = Vector3.one;
            block.SpriteRenderer.sprite = SpriteCollection.WallTilesets[_index];

            if (int.Parse(SpriteCollection.WallTilesets[_index].name) % 2 == 0)
                block.GameObject.AddComponent<BoxCollider2D>().offset = new Vector3(-0.3125846f, 0.5f);
            else
                block.GameObject.AddComponent<BoxCollider2D>().offset = new Vector3(0.3125846f, 0.5f);
            block.GameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.3748307f, 1f);
            block.GameObject.layer = 8;

            _wallMap[x, y, z] = block;
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

        var sprites = SpriteCollection.PropsSprites;
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
    public void CreateCheckPoint(int x, int y, int z)
    {
        if (x < 0 || x >= _gameplayMap.Width || y <= 0 || y >= _gameplayMap.Height) return;

        if (_index != -1 && _type == 2 && (_groundMap[x, y, z] != null || _groundMap[x, y - 1, z] == null))
        {
            Debug.LogWarning("Checkpoints can be placed on the ground only.");
            return;
        }

        _gameplayMap.Destroy(x, y, z);

        if (_index == -1) return;

        var block = new Block(SpriteCollection.GamePlaySprite[_index].name);

        block.Transform.SetParent(Parent.Find("Gameplay").transform);
        block.Transform.localPosition = new Vector3(_positionMin.X + x, _positionMin.Y + y);
        block.Transform.localScale = Vector3.one;
        block.SpriteRenderer.sprite = SpriteCollection.GamePlaySprite[_index];
        block.SpriteRenderer.sortingOrder = 100 * z + 30;
        block.GameObject.AddComponent<BoxCollider2D>().offset = new Vector3(0, 0.5f);
        block.GameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        block.GameObject.AddComponent<Checkpoint>();
        block.GameObject.GetComponent<Checkpoint>().spriteRenderer = block.SpriteRenderer;
        block.GameObject.GetComponent<Checkpoint>().passive = SpriteCollection.GamePlaySprite[_index];
        block.GameObject.GetComponent<Checkpoint>().active = SpriteCollection.GamePlaySprite[3];
        //create the respawn point as a child of the checkpoint
        GameObject RespawnPoint = new GameObject("RespawnPoint");
        RespawnPoint.transform.SetParent(block.Transform);
        RespawnPoint.transform.localPosition = new Vector3(0, 3f);
        block.GameObject.GetComponent<Checkpoint>().respawnPoint = RespawnPoint.transform;
        block.GameObject.GetComponent<Checkpoint>().col = block.GameObject.GetComponent<BoxCollider2D>();
        block.GameObject.tag = "Checkpoint";

        _gameplayMap[x, y, z] = block;
    }
    public void CreateEndPoint(int x, int y, int z)
    {
        if (x < 0 || x >= _gameplayMap.Width || y <= 0 || y >= _gameplayMap.Height) return;

        if (_index != -1 && _type == 4 && (_groundMap[x, y, z] != null || _groundMap[x, y - 1, z] == null))
        {
            Debug.LogWarning("Object can be placed on the ground only.");
            return;
        }

        _gameplayMap.Destroy(x, y, z);

        if (_index == -1) return;

        var block = new Block(SpriteCollection.GamePlaySprite[_index].name);

        block.Transform.SetParent(Parent.Find("Gameplay").transform);
        block.Transform.localPosition = new Vector3(_positionMin.X + x, _positionMin.Y + y);
        block.Transform.localScale = Vector3.one;
        block.SpriteRenderer.sprite = SpriteCollection.GamePlaySprite[_index];
        block.SpriteRenderer.sortingOrder = 100 * z + 30;
        block.GameObject.AddComponent<BoxCollider2D>().offset = new Vector3(0, 0.5f);
        block.GameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        if (RestrictionManager.Instance.OnlineBuild)
            block.GameObject.tag = "EndPoint";
        else if (RestrictionManager.Instance.OnlineGame)
            block.GameObject.tag = "Finish";

        _gameplayMap[x, y, z] = block;
    }
    public void CreateDeathObject(int x, int y, int z)
    {
        if (x < 0 || x >= _gameplayMap.Width || y <= 0 || y >= _gameplayMap.Height) return;

        if (_index != -1 && _type == 4 && (_groundMap[x, y, z] != null || _groundMap[x, y - 1, z] == null))
        {
            Debug.LogWarning("Object can be placed on the ground only.");
            return;
        }

        _gameplayMap.Destroy(x, y, z);

        if (_index == -1) return;

        var block = new Block(SpriteCollection.GamePlaySprite[_index].name);

        block.Transform.SetParent(Parent.Find("Gameplay").transform);
        block.Transform.localPosition = new Vector3(_positionMin.X + x, _positionMin.Y + y);
        block.Transform.localScale = Vector3.one;
        block.SpriteRenderer.sprite = SpriteCollection.GamePlaySprite[_index];
        block.SpriteRenderer.sortingOrder = 100 * z + 30;
        block.GameObject.AddComponent<EdgeCollider2D>();
        block.GameObject.GetComponent<EdgeCollider2D>().offset = new Vector2(0, 0.5f);
        block.GameObject.GetComponent<EdgeCollider2D>().isTrigger = true;
        block.GameObject.GetComponent<EdgeCollider2D>().points = new Vector2[] { new Vector2(-0.436338425f, -0.5048233f), new Vector2(-0.4363384f, 0.06517267f), new Vector2(-0.406106949f, 0.126236439f), new Vector2(-0.368604422f, 0.06568074f), new Vector2(-0.188889742f, -0.0613064766f), new Vector2(-0.12655139f, 0.122369051f), new Vector2(0.00450277328f, 0.250551224f), new Vector2(0.161319971f, 0.126229048f), new Vector2(0.246645927f, 0.1931541f), new Vector2(0.373010635f, 0.132702589f), new Vector2(0.435099125f, 0.06654525f), new Vector2(0.438595772f, -0.506190062f), new Vector2(-0.4385958f, -0.5061901f) };
        block.GameObject.tag = "Obstacle";

        _gameplayMap[x, y, z] = block;
    }
    public void CreateReverseDeathObject(int x, int y, int z)
    {
        if (x < 0 || x >= _gameplayMap.Width || y < 0 || y >= _gameplayMap.Height) return;

        if (_index != -1 && _type == 4 && (_groundMap[x, y, z] != null || _groundMap[x, y + 1, z] == null))
        {
            Debug.LogWarning("Object can be placed under the ground only.");
            return;
        }

        _gameplayMap.Destroy(x, y, z);

        if (_index == -1) return;

        var block = new Block(SpriteCollection.GamePlaySprite[_index].name);

        block.Transform.SetParent(Parent.Find("Gameplay").transform);
        block.Transform.localPosition = new Vector3(_positionMin.X + x, _positionMin.Y + y);
        block.Transform.localScale = Vector3.one;
        block.SpriteRenderer.sprite = SpriteCollection.GamePlaySprite[_index];
        block.SpriteRenderer.sortingOrder = 100 * z + 30;
        block.GameObject.AddComponent<EdgeCollider2D>();
        block.GameObject.GetComponent<EdgeCollider2D>().offset = new Vector2(0, 0.5f);
        block.GameObject.GetComponent<EdgeCollider2D>().isTrigger = true;
        block.GameObject.GetComponent<EdgeCollider2D>().points = new Vector2[] { new Vector2(0.436338425f, 0.5048233f), new Vector2(0.4363384f, -0.06517267f), new Vector2(0.406106949f, -0.126236439f), new Vector2(0.368604422f, -0.06568074f), new Vector2(0.188889742f, 0.0613064766f), new Vector2(0.12655139f, -0.122369051f), new Vector2(-0.00450277328f, -0.250551224f), new Vector2(-0.161319971f, -0.126229048f), new Vector2(-0.246645927f, -0.1931541f), new Vector2(-0.373010635f, -0.132702589f), new Vector2(-0.435099125f, -0.06654525f), new Vector2(-0.438595772f, 0.506190062f), new Vector2(0.4385958f, 0.5061901f) };
        block.GameObject.tag = "Obstacle";

        _gameplayMap[x, y, z] = block;
    }
    public void CreateBrick(int x, int y, int z)
    {
        if (x < 0 || x >= _gameplayMap.Width || y < 0 || y >= _gameplayMap.Height) return;

        //ne peut pas etre placé sur le sol ou sur un mur
        if (_index != -1 && _type == 4 && (_groundMap[x, y, z] != null || _wallMap[x, y, z] != null))
        {
            Debug.LogWarning("Brick can not be placed on the ground or on a wall.");
            return;
        }

        _gameplayMap.Destroy(x, y, z);

        if (_index == -1) return;

        var block = new Block(SpriteCollection.GamePlaySprite[_index].name);

        block.Transform.SetParent(Parent.Find("Gameplay").transform);
        block.Transform.localPosition = new Vector3(_positionMin.X + x, _positionMin.Y + y);
        block.Transform.localScale = Vector3.one;
        block.SpriteRenderer.sprite = SpriteCollection.GamePlaySprite[_index];
        block.SpriteRenderer.sortingOrder = 100 * z + 30;
        block.GameObject.AddComponent<BoxCollider2D>().offset = new Vector3(0, 0.5f);
        block.GameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        block.GameObject.tag = "Brick";
        block.GameObject.AddComponent<Animator>();
        if (_index == 5) block.GameObject.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Animations/BreakingBricks/Red/RedBrick") as RuntimeAnimatorController;
        else block.GameObject.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Animations/BreakingBricks/Grey/GreyBrick") as RuntimeAnimatorController;
        block.GameObject.AddComponent<BreakingBrick>();

        _gameplayMap[x, y, z] = block;
    }
    public void CreateDeathZone(int x, int y, int z)
    {
        if (x < 0 || x >= _groundMap.Width || y < 0 || y >= _groundMap.Height) return;

        _groundMap.Destroy(x, y, z);

        _gameplayMap.Destroy(x, y, z);

        if (_index == -1) return;

        var block = new Block(SpriteCollection.GamePlaySprite[_index].name);

        block.Transform.SetParent(Parent.Find("Gameplay").transform);
        block.Transform.localPosition = new Vector3(_positionMin.X + x, _positionMin.Y + y);
        block.Transform.localScale = Vector3.one;
        block.GameObject.GetComponent<SpriteRenderer>().sprite = SpriteCollection.GamePlaySprite[_index];
        block.GameObject.GetComponent<SpriteRenderer>().sortingOrder = 100 * z + 30;
        block.GameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        block.GameObject.AddComponent<BoxCollider2D>().offset = new Vector3(0, 0.5f);
        block.GameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        block.GameObject.tag = "DeathZone";

        _gameplayMap[x, y, z] = block;
    }
    public void CreateGrabbableObject(int x, int y, int z, float mass)
    {
        if (x < 0 || x >= _gameplayMap.Width || y < 0 || y >= _gameplayMap.Height) return;

        _gameplayMap.Destroy(x, y, z);

        if (_index == -1) return;

        var block = new Block(SpriteCollection.GamePlaySprite[_index].name);

        block.Transform.SetParent(Parent.Find("Gameplay").transform);
        block.Transform.localPosition = new Vector3(_positionMin.X + x, _positionMin.Y + y);
        block.Transform.localScale = Vector3.one;
        block.SpriteRenderer.sprite = SpriteCollection.GamePlaySprite[_index];
        block.SpriteRenderer.sortingOrder = 100 * z + 30;
        block.GameObject.AddComponent<BoxCollider2D>().offset = new Vector3(0, 0.4825f);
        block.GameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.85f, 0.85f);
        block.GameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        block.GameObject.GetComponent<Rigidbody2D>().mass = mass;
        block.GameObject.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        block.GameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        block.GameObject.AddComponent<BoxMovement>();
        block.GameObject.layer = 9;
        block.GameObject.tag = "Grab";
        block.GameObject.GetComponent<BoxMovement>().SetPosition(block.Transform.position);

        _gameplayMap[x, y, z] = block;
    }
    public void CreateChallengeForOverriding(int x, int y, int z)
    {
        if (x < 0 || x >= _gameplayMap.Width || y < 0 || y >= _gameplayMap.Height) return;

        //ne peut pas etre placé sur le sol ou sur un mur
        if (_index != -1 && _type == 4 && (_groundMap[x, y, z] != null || _wallMap[x, y, z] != null))
        {
            Debug.LogWarning("Brick can not be placed on the ground or on a wall.");
            return;
        }

        _gameplayMap.Destroy(x, y, z);

        if (_index == -1) return;

        var block = new Block(SpriteCollection.GamePlaySprite[_index].name);
        block.Transform.SetParent(Parent.Find("Gameplay").transform);
        block.Transform.localPosition = new Vector3(_positionMin.X + x, _positionMin.Y + y);
        block.Transform.localScale = Vector3.one;
        block.SpriteRenderer.sprite = SpriteCollection.GamePlaySprite[_index];
        block.SpriteRenderer.sortingOrder = 100 * z + 30;
        block.GameObject.AddComponent<BoxCollider2D>().offset = new Vector3(0, 0.5f);
        block.GameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        block.GameObject.AddComponent<StageCollision>().Challenge = missionNumber;

        GameObject wall = new GameObject("WallChallenge" + missionNumber);
        wall.transform.SetParent(Parent.Find("Gameplay").transform);
        wall.transform.localPosition = new Vector3(_positionMin.X + x + 3, _positionMin.Y + y);
        wall.transform.localScale = new Vector3(1f, 150f, 1);
        wall.AddComponent<BoxCollider2D>().offset = new Vector3(0, 0.5f);
        wall.GetComponent<BoxCollider2D>().size = new Vector2(1, 150);
        wall.AddComponent<SpriteRenderer>().color = new Color(0.466f, 0.149f, 0.235f, 1f);
        wall.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Grid/Square");

        GameObject Mission = Instantiate(Mission1Prefab, new Vector3(0,0,0), Quaternion.identity);
        Mission.name = "Mission" + missionNumber;
        Mission.transform.SetParent(GameObject.Find("Canvas/Popups").transform);
        Mission.transform.localPosition = new Vector3(0, 0, 0);
        missionNumber++;

        _gameplayMap[x, y, z] = block;
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
        Charactereditor.Character = Player.GetComponent<CharacterBase>();
        Player.SetActive(true);
        MainCamera.GetComponent<CameraFollow>().Player = Player;
        MainCamera.GetComponent<CameraFollow>().StartPosition = new Vector3(x_position, y_position, -10);

        //foreach object that have the tag "Checkpoint" in the scene set player
        GameObject[] Checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        foreach (GameObject Checkpoint in Checkpoints)
        {
            Checkpoint.GetComponent<Checkpoint>().gameController = Player.GetComponent<GameController>();
        }
        GameObject.Find("Canvas/Popups").GetComponent<CharacterChallengeManager>().GameController = Player.GetComponent<GameController>();
        GameObject.Find("Canvas/Popups").GetComponent<UpcastingManager>().Character = Player.GetComponent<CharacterBase>();
    }
    public string GetLevelName()
    {
        return LevelName;
    }
}