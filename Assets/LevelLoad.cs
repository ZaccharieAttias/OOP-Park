using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using LootLocker.Requests;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class LevelLoad : MonoBehaviour
{
    private Transform LevelDataEntryContent;
    public GameObject LevelEntryDisplayItem;
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
        }
    }
    public void ToggleOff()
    {
        GameObject.Find("Canvas/Menus/Gameplay/SavedScreen").SetActive(false);
        GameObject.Find("Canvas/Menus/Gameplay/LoadALevel").SetActive(false);
    }
}
