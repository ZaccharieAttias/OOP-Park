using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoad : MonoBehaviour
{
    private Transform LevelDataEntryContent;
    public GameObject LevelEntryDisplayItem;
    public TMP_InputField LevelSaveNameText;
    public TMP_InputField LevelUploadNameText;
    public string LevelName;
    public string path;

    public void Start()
    {
        LevelDataEntryContent = GameObject.Find("Canvas/Menus/Gameplay/SavedScreen/Background/Foreground/ScrollView/Viewport/Content").transform;
        path = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Resources", "Screenshots", "Saved");
    }
    public void LoadLevelsData()
    {
        foreach (Transform child in LevelDataEntryContent)
            Destroy(child.gameObject);

        string[] files = Directory.GetFiles(path);
        for (int i = 0; i < files.Length; i++)
        {
            string fileName = Path.GetFileNameWithoutExtension(files[i]);
            GameObject displayItem = Instantiate(LevelEntryDisplayItem, transform.position, Quaternion.identity);
            displayItem.transform.SetParent(LevelDataEntryContent);
            displayItem.GetComponent<LevelInitializer>().SetInformations(i, fileName);

            displayItem.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => ToggleOff());
            displayItem.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => SetNames(fileName));
            displayItem.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => DeleteLevel(displayItem, fileName));
        }
    }
    public void SetNames(string name)
    {
        LevelName = name;
        LevelSaveNameText.text = name;
        LevelUploadNameText.text = name;
    }
    public void DeleteLevel(GameObject displayItem, string fileName)
    {
        Directory.Delete(Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Resources", "Screenshots", "Saved", fileName), true);
        File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Resources", "Screenshots", "Saved", fileName + ".meta"));
        AssetDatabase.Refresh();
        Destroy(displayItem);
    }
    public void ToggleOff()
    {
        GameObject.Find("Canvas/Menus/Gameplay/SavedScreen").SetActive(false);
        GameObject.Find("Canvas/Menus/Gameplay/LoadALevel").SetActive(false);
    }
}