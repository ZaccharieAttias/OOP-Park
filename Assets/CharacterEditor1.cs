﻿using HeroEditor.Common;
using UnityEngine;
using System.IO;

/// <summary>
/// Character editor UI and behaviour.
/// </summary>
public class CharacterEditor1 : MonoBehaviour
{
    public CharacterBase Character;

    public void LoadFromJson()
    {
        string path = Path.Combine(Application.dataPath, "Resources/CharactersData", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, $"{CharactersData.CharactersManager.CurrentCharacter.Name}.json");
        var json = File.ReadAllText(path);

        Character.FromJson(json);
    }
}