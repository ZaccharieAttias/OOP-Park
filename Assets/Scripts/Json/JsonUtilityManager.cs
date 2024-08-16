using System.IO;
using System.Linq;
using UnityEngine;


public class JsonUtilityManager : MonoBehaviour
{
    [Header("Data Paths")]
    public string FolderPath;


    public void Start()
    {
        FolderPath = Path.Combine(Application.dataPath, "Resources/Json", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        InitializeData();
    }
    public void InitializeData()
    {
        AttributesData.Initialize(FolderPath);
        MethodsData.Initialize(FolderPath);
        SpecialAbilitiesData.Initialize(FolderPath);
        CharactersData.Initialize(FolderPath);
        CharactersGameObjectData.Initialize();
        RestrictionsData.Initialize(FolderPath);
        GameplayData.Initialize();
    }

    public void Save()
    {
        if (!Directory.Exists(FolderPath)) Directory.CreateDirectory(FolderPath);

        SaveData();
    }
    public void Load()
    {
        LoadData();
        CharactersData.CharactersManager.DisplayCharacter(CharactersData.CharactersManager.CharactersCollection.Where(x => !x.IsAbstract).First());
        GameObject.Find("Scripts/CharacterEditor").GetComponent<CharacterEditor1>().LoadFromJson();
    }
    public void SetPath(string path)
    {
        FolderPath = path;

        AttributesData.SetPath(Path.Combine(FolderPath, "Attributes.json"));
        MethodsData.SetPath(Path.Combine(FolderPath, "Methods.json"));
        SpecialAbilitiesData.SetPath(Path.Combine(FolderPath, "SpecialAbilities.json"));
        CharactersData.SetPath(Path.Combine(FolderPath, "Characters.json"));
        RestrictionsData.SetPath(Path.Combine(FolderPath, "Restrictions.json"));
    }
    public void SaveData()
    {
        AttributesData.Save();
        MethodsData.Save();
        SpecialAbilitiesData.Save();
        CharactersData.Save();
        if (RestrictionManager.Instance.OnlineGame || RestrictionManager.Instance.OnlineBuild) RestrictionsData.Save();
        GameplayData.Save();
    }
    public void LoadData()
    {
        AttributesData.Load();
        MethodsData.Load();
        SpecialAbilitiesData.Load();
        CharactersData.Load();
        CharactersGameObjectData.Load();
        if (RestrictionManager.Instance.OnlineGame || RestrictionManager.Instance.OnlineBuild) RestrictionsData.Load();
        GameplayData.Load();
    }
}