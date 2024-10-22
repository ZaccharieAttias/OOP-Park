﻿using Assets.HeroEditor.Common.Scripts.Collections;
using Assets.HeroEditor.InventorySystem.Scripts.Elements;
using Assets.HeroEditor4D.SimpleColorPicker.Scripts;
using HeroEditor.Common;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static Assets.HeroEditor.Common.Scripts.EditorScripts.CharacterEditor;

public class CharacterEditor1 : MonoBehaviour
{
    public CharacterBase Character;
    public string AssetStoreLink;


    [Header("Public")]
    public SpriteCollection SpriteCollection;
    public IconCollection IconCollection;
    public Transform Tabs;
    public ScrollInventory Inventory;
    public Text ItemName;


    [Header("Other")]
    public List<Toggle> EditionToggles;
    public string EditionFilter;
    public List<string> PaintParts;
    public Button PaintButton;
    public ColorPicker ColorPicker;
    public List<string> CollectionSorting;
    public List<CollectionBackground> CollectionBackgrounds;
    public string FilePickerPath;


    public void Start()
    {
        if (GameObject.Find("Player") != null)
        {
            Character = GameObject.Find("Player").GetComponent<CharacterBase>();
        }
    }
    public void LoadFromJson()
    {
        if (GameObject.Find("Player") == null)
            return;
        string path = Path.Combine(Application.dataPath, "StreamingAssets", $"Resources/CharactersData/All/{CharactersData.CharactersManager.CurrentCharacter.Name}.json");
        var json = File.ReadAllText(path);

        Character.FromJson(json);
    }
    public void LoadFromJson(string name)
    {
        string path = Path.Combine(Application.dataPath, "StreamingAssets", "Resources/CharactersData", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, $"{name}.json");
        var json = File.ReadAllText(path);

        Character.FromJson(json);
    }
    public void OnlineLoadFromJson()
    {
        string path = Path.Combine(Application.dataPath, "StreamingAssets", "Resources/CharactersData", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, "Character 0.json");
        var json = File.ReadAllText(path);

        Character.FromJson(json);
    }
    public void SpecialLoadFromJson()
    {
        string path = Path.Combine(Application.dataPath, "StreamingAssets", "Resources/CharactersData", $"{UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}", $"{CharactersData.CharactersManager.CurrentCharacter.Name}.json");
        var json = File.ReadAllText(path);

        Character.FromJson(json);
    }
}