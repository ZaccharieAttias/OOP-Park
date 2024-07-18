using System.Collections;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using Assets.PixelFantasy.PixelTileEngine.Scripts;
using HeroEditor.Common;
using TMPro;
using UnityEngine.UI;
using UnityEditor;
using LootLocker.Extension.DataTypes;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using System.Collections.Generic;
using Assets.HeroEditor.Common.Scripts.Common;


public class LevelBuilderB : MonoBehaviour
{
    private Transform Parent;
    public SpriteCollectionPF SpriteCollection;
    public SpriteRenderer Cursor;

    private int _type;
    private int _index;
    private int _layer;
    private readonly bool[] _layersEnabled = { true, true, true, true };
    private Position _positionMin;

    private TileMap _groundMap;
    private TileMap _coverMap;
    private TileMap _propsMap;

    private string _hash;

    private Transform Terrain;
    private Transform Walls;
    private CharacterEditor1 CharacterEditor;
    public GameObject PlayerPrefab;
    public GameObject Player;
    public SwapScreen swapScreen;
    public int check = 0;
    public GameObject CheckPointPrefab;
    public Dictionary<string, bool> MinimumObjectsCreated = new Dictionary<string, bool>();


    public void Start()
    {
        _groundMap = new TileMap(1, 1, 4);
        _coverMap = new TileMap(1, 1, 4);
        _propsMap = new TileMap(1, 1, 4);

        Parent = GameObject.Find("Grid/LevelBuilder").transform;
        Terrain = Parent.Find("Terrain");
        Walls = Parent.Find("Walls");
        CharacterEditor = GameObject.Find("Scripts/CharacterEditor").GetComponent<CharacterEditor1>();
        swapScreen = GameObject.Find("Scripts/CharacterEditor").GetComponent<SwapScreen>();

        MinimumObjectsCreated.Add("Player", false);
        MinimumObjectsCreated.Add("Ground", false);
    }
    public void SwitchTile(int type, int index)
    {
        _type = type;
        _index = index;

        if (index == -1)
        {
            Cursor.sprite = SpriteCollection.DeleteSprite;
        }
        else {
            switch (type)
            {
                case 0:
                    Cursor.sprite = SpriteCollection.GroundTilesets[index].Sprites[20];
                    break;
                case 1:
                    Cursor.sprite = SpriteCollection.CoverTilesets[index].Sprites[0];
                    break;
                case 2:
                    Cursor.sprite = SpriteCollection.PropsSprites[index];
                    break;
                case 3:
                    Cursor.sprite = SpriteCollection.OtherSprites[index];
                    break;
                case 4:
                    Cursor.sprite = SpriteCollection.GamePlaySprite[index];
                    break;
            }
            if (Cursor.sprite.name == "skeleton") Cursor.transform.localScale = new Vector3(0.85f, 0.85f, 1);
            else Cursor.transform.localScale = Vector3.one;
        }
    }
    public void EnableCursor(bool value)
    {
        Cursor.enabled = value;
    }
    public void MoveCursor(Vector2 pointer)
    {
        if (!Cursor.enabled) return;

        var position = Position.FromPointer(pointer);

        Cursor.transform.position = new Vector3(position.X, position.Y);

        if (_type == 2 && _index != -1)
        {
            Cursor.transform.position -= new Vector3(0, 1f / 16f);
        }
    }
    public void Draw(Vector2 pointer)
    {
        if (!_layersEnabled[_layer]) return;

        var position = Position.FromPointer(pointer);
        var p = new Position(position.X - _positionMin.X, position.Y - _positionMin.Y);

        if (p.X < 0 || p.X >= _groundMap.Width || p.Y < 0 || p.Y >= _groundMap.Height)
        {
            _groundMap.ExtendMap(p);
            _coverMap.ExtendMap(p);
            _propsMap.ExtendMap(p);
            _positionMin = new Position(Mathf.Min(_positionMin.X, position.X), Mathf.Min(_positionMin.Y, position.Y));
            p = position - _positionMin;
            _hash = null;
        }

        var hash = $"{_type}.{_index}.{p}";

        if (hash == _hash) return;

        _hash = hash;

        switch (_type)
        {
            case 0: CreateGround(p.X, p.Y, _layer); break;
            case 1: CreateCover(p.X, p.Y, _layer); break;
            case 2:
            case 3: CreateProps(p.X, p.Y, _layer); break;
            case 4: 
                switch (_index)
                {
                    case -1: 
                        if (p.X < 0 || p.X >= _propsMap.Width || p.Y <= 0 || p.Y >= _propsMap.Height) return;
                        _propsMap.Destroy(p.X, p.Y, _layer);
                        break;
                    case 0: CreatePlayer(p.X, p.Y, _layer); break;
                    case 1: CreateCheckPoint(p.X, p.Y, _layer); break;
                    case 2: CreateCheckPoint(p.X, p.Y, _layer); break;
                    case 3: CreateCheckPoint(p.X, p.Y, _layer); break;
                    case 4: CreacteDeathObject(p.X, p.Y, _layer); break;
                    case 5: CreateDeathZone(p.X, p.Y, _layer); break;
                }
                break;
        }
    }
    public void SwitchLayer(int layer)
    {
        _layer = layer;
    }
    public void EnableLayer(int layer)
    {
        _layersEnabled[layer] = !_layersEnabled[layer];

        for (var x = 0; x < _groundMap.Width; x++)
        {
            for (var y = 0; y < _groundMap.Height; y++)
            {
                if (_groundMap[x, y, layer] != null) _groundMap[x, y, layer].SpriteRenderer.enabled = _layersEnabled[layer];
                if (_coverMap[x, y, layer] != null) _coverMap[x, y, layer].SpriteRenderer.enabled = _layersEnabled[layer];
                if (_propsMap[x, y, layer] != null) _propsMap[x, y, layer].SpriteRenderer.enabled = _layersEnabled[layer];
            }
        }
    }
    public void CreateGround(int x, int y, int z)
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
            MinimumObjectsCreated["Ground"] = true;
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
    public void CreateCover(int x, int y, int z)
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
    public void CreateProps(int x, int y, int z)
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
    public void CreateCheckPoint(int x, int y, int z)
    {
        if (x < 0 || x >= _propsMap.Width || y <= 0 || y >= _propsMap.Height) return;

        if (_index != -1 && _type == 4 && (_groundMap[x, y, z] != null || _groundMap[x, y - 1, z] == null))
        {
            Debug.LogWarning("Checkpoints can be placed on the ground only.");
            return;
        }

        _propsMap.Destroy(x, y, z);

        if (_index == -1) return;

        var block = new Block(SpriteCollection.GamePlaySprite[_index].name);

        block.Transform.SetParent(Terrain.Find("Props").transform);
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

        if (_type == 4 && _index != -1)
        {
            block.OffsetY = -1;
            block.Transform.localPosition -= new Vector3(0, 1f / 16f);
        }

        _propsMap[x, y, z] = block;
    }
    public void CreacteDeathObject(int x, int y, int z)
    {
        if (x < 0 || x >= _propsMap.Width || y <= 0 || y >= _propsMap.Height) return;

        if (_index != -1 && _type == 4 && (_groundMap[x, y, z] != null || _groundMap[x, y - 1, z] == null))
        {
            Debug.LogWarning("Object can be placed on the ground only.");
            return;
        }

        _propsMap.Destroy(x, y, z);

        if (_index == -1) return;

        var block = new Block(SpriteCollection.GamePlaySprite[_index].name);

        block.Transform.SetParent(Terrain.Find("Props").transform);
        block.Transform.localPosition = new Vector3(_positionMin.X + x, _positionMin.Y + y);
        block.Transform.localScale = Vector3.one;
        block.SpriteRenderer.sprite = SpriteCollection.GamePlaySprite[_index];
        block.SpriteRenderer.sortingOrder = 100 * z + 30;
        block.GameObject.AddComponent<EdgeCollider2D>();
        block.GameObject.GetComponent<EdgeCollider2D>().offset = new Vector2(0, 0.5f);
        block.GameObject.GetComponent<EdgeCollider2D>().isTrigger = true;
        block.GameObject.tag = "Obstacle";

        if (_type == 4 && _index != -1)
        {
            block.OffsetY = -1;
            block.Transform.localPosition -= new Vector3(0, 1f / 16f);
        }

        _propsMap[x, y, z] = block;
    }
    public void CreateDeathZone(int x, int y, int z)
    {
        if (x < 0 || x >= _groundMap.Width || y < 0 || y >= _groundMap.Height) return;

        _propsMap.Destroy(x, y, z);

        if (_index == -1) return;

        var block = new Block(SpriteCollection.GamePlaySprite[_index].name);

        block.Transform.SetParent(Terrain.Find("Props").transform);
        block.Transform.localPosition = new Vector3(_positionMin.X + x, _positionMin.Y + y);
        block.Transform.localScale = new Vector3(0.85f, 0.85f, 1);
        block.SpriteRenderer.sprite = SpriteCollection.GamePlaySprite[_index];
        block.SpriteRenderer.sortingOrder = 100 * z + 30;
        block.GameObject.AddComponent<BoxCollider2D>().offset = new Vector3(0, 0.5f);
        block.GameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        block.GameObject.tag = "DeathZone";

        if (_type == 4 && _index != -1)
        {
            block.OffsetY = -1;
            block.Transform.localPosition -= new Vector3(0, 1f / 16f);
        }

        _propsMap[x, y, z] = block;
    }
    public void CreatePlayer(int x, int y, int z)
    {
        if (x < 0 || x >= _propsMap.Width || y <= 0 || y >= _propsMap.Height) return;

        if (_index != -1 && _type == 2 && (_groundMap[x, y, z] != null || _groundMap[x, y - 1, z] == null))
        {
            Debug.LogWarning("Players can be placed on the ground only.");
            return;
        }

        if (_index == -1) return;

        string CharacterName = SpriteCollection.GamePlaySprite[_index].name;
        if (GameObject.Find("Player") == null)
        {
            Player = Instantiate(PlayerPrefab, new Vector3(_positionMin.X + x, _positionMin.Y + y), Quaternion.identity);
            Player.name = "Player";
            Player.transform.localScale = Vector3.one;
            Player.transform.localPosition = new Vector3(_positionMin.X + x, _positionMin.Y + y);

            CharacterEditor.Character = Player.GetComponent<CharacterBase>();
            CharacterEditor.LoadFromJson(CharacterName);
            GameObject.Find("Canvas/Popups").GetComponent<CharacterAppearanceManager>().playerTransform = Player.transform;
            GameObject.Find("Canvas/Popups").GetComponent<CharacterAppearanceManager>().Character = Player.GetComponent<Character>();
            MinimumObjectsCreated["Player"] = true;
        }
        else
        {
            if (CharactersData.CharactersManager.CurrentCharacter == null)
                CharacterEditor.LoadFromJson(CharacterName);
            else
                CharacterEditor.LoadFromJson(CharactersData.CharactersManager.CurrentCharacter.Name);
            Player.transform.localPosition = new Vector3(_positionMin.X + x, _positionMin.Y + y);
        }
        SetPlayer();
        if (swapScreen.firstTime && check == 0) 
        {
            CharactersCreationManager CharactersCreationManager = GameObject.Find("Canvas/Popups").GetComponent<CharactersCreationManager>();
            swapScreen.SwapButtonToCharacterCenter.onClick.AddListener(() => CharactersCreationManager.RootCreation());
            swapScreen.SwapButtonToCharacterCenter.onClick.AddListener(() => swapScreen.FisrtSwap());
            check = 1;
        }
    }
    public void SetPlayer()
    {
        GameObject Player = GameObject.Find("Player");
        Player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        Player.GetComponent<Movement>().enabled = false;
        Player.GetComponent<GameController>().enabled = false;
        Player.GetComponent<GrabObject>().enabled = false;

        //foreach object that have the tag "Checkpoint" in the scene set player
        GameObject[] Checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        PlayTestManager playTestManager = GameObject.Find("Scripts/PlayTestManager").GetComponent<PlayTestManager>();

        foreach (GameObject Checkpoint in Checkpoints)
        {
            Checkpoint.GetComponent<Checkpoint>().gameController = Player.GetComponent<GameController>();
        }

        playTestManager.Player = Player;
        playTestManager.x_start_pos = Player.transform.position.x;
        playTestManager.y_start_pos = Player.transform.position.y;
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
    public void SwitchCover()
    {
        for (var x = 0; x < _coverMap.Width; x++)
        {
            for (var y = 0; y < _coverMap.Height; y++)
            {
                if (_coverMap[x, y, _layer] != null)
                {
                    _coverMap[x, y, _layer].SpriteRenderer.enabled = !_coverMap[x, y, _layer].SpriteRenderer.enabled;
                } 
            }
        }
    }
    public TileMap GetGroundMap()
    {
        return _groundMap;
    }
    public TileMap GetCoverMap()
    {
        return _coverMap;
    }
    public TileMap GetPropsMap()
    {
        return _propsMap;
    }
    public void SetUI()
    {
        if (Terrain.Find("Ground").childCount == 0) MinimumObjectsCreated["Ground"] = false;
        if (MinimumObjectsCreated["Player"])
        {
            GameObject.Find("Canvas/Menus/Gameplay/SwapScreen").SetActive(true);
            if(MinimumObjectsCreated["Ground"])
            {
                if (!swapScreen.firstTime){
                    GameObject.Find("Canvas/Menus/Gameplay/Buttons/Upload").SetActive(true);
                    GameObject.Find("Canvas/Menus/Gameplay/Buttons/PlayTest").SetActive(true);
                }
            }
            else
            {
                GameObject.Find("Canvas/Menus/Gameplay/Buttons/Upload").SetActive(false);
                GameObject.Find("Canvas/Menus/Gameplay/Buttons/PlayTest").SetActive(false);
            }
        }
        else
        {
            GameObject.Find("Canvas/Menus/Gameplay/Buttons/PlayTest").SetActive(false);
            GameObject.Find("Canvas/Menus/Gameplay/Buttons/Upload").SetActive(false);
            GameObject.Find("Canvas/Menus/Gameplay/SwapScreen").SetActive(false);
        }
    }
}