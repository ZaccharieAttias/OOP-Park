using System.IO;
using UnityEngine;


public class JsonUtilityManager : MonoBehaviour
{
    public string FolderPath;


    public void Start()
    {
        FolderPath = Path.Combine(Application.dataPath, "Resources/Json", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        AttributesData.Initialize(FolderPath);
        MethodsData.Initialize(FolderPath);
        SpecialAbilitiesData.Initialize(FolderPath);
        CharactersData.Initialize(FolderPath);
        CharactersGameObjectData.Initialize();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) Save();
        if (Input.GetKeyDown(KeyCode.L)) Load();
    }


    public void Save()
    {
        if (!Directory.Exists(FolderPath))
        {
            Directory.CreateDirectory(FolderPath);
        }
        AttributesData.Save();
        MethodsData.Save();
        // SpecialAbilitiesData.Save();
        CharactersData.Save();
    }
    public void Load()
    {
        AttributesData.Load();
        MethodsData.Load();
        // SpecialAbilitiesData.Load();
        CharactersData.Load();
        CharactersGameObjectData.Load();
    }
}
