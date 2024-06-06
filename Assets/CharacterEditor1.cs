using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using Assets.HeroEditor.Common.Scripts.Collections;
using Assets.HeroEditor.Common.Scripts.Common;
using Assets.HeroEditor.Common.Scripts.Data;
using Assets.HeroEditor.Common.Scripts.ExampleScripts;
using Assets.HeroEditor.InventorySystem.Scripts;
using Assets.HeroEditor.InventorySystem.Scripts.Data;
using Assets.HeroEditor.InventorySystem.Scripts.Elements;
using Assets.HeroEditor4D.SimpleColorPicker.Scripts;
using HeroEditor.Common;
using HeroEditor.Common.Data;
using HeroEditor.Common.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Character editor UI and behaviour.
/// </summary>
public class CharacterEditor1 : MonoBehaviour
{
    public CharacterBase Character;


    public void LoadFromJson()
    {
        string path = $"C:\\Users\\nitza\\Desktop\\Applications\\Projects\\School\\OOP-Park\\Assets\\Resources\\CharactersData\\json\\test\\{CharactersData.CharactersManager.CurrentCharacter.Name}.json";
        var json = System.IO.File.ReadAllText(path);

        Character.FromJson(json);
    }
}