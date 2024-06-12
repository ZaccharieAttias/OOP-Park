using HeroEditor.Common;
using UnityEngine;
using System.IO;

/// <summary>
/// Character editor UI and behaviour.
/// </summary>
public class CharacterEditor1 : MonoBehaviour
{
    public CharacterBase Character;

    public void Start()
    {
        if (GameObject.Find("Player") != null)
        {
            Character = GameObject.Find("Player").GetComponent<CharacterBase>();
        }
    }
    public void LoadFromJson()
    {
        string path = Path.Combine(Application.dataPath, "Resources/CharactersData", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, $"{CharactersData.CharactersManager.CurrentCharacter.Name}.json");
        var json = File.ReadAllText(path);

        Character.FromJson(json);
    }
    public void LoadFromJson(string name)
    {
        string path = Path.Combine(Application.dataPath, "Resources/CharactersData", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, $"{name}.json");
        var json = File.ReadAllText(path);

        Character.FromJson(json);
    }
    public void OnlineLoadFromJson()
    {
        string path = Path.Combine(Application.dataPath, "Resources/CharactersData", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, "Character 1.json");
        var json = File.ReadAllText(path);

        Character.FromJson(json);
    }
}