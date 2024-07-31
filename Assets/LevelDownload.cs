using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using LootLocker.Requests;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using TMPro;

public class LevelDownload : MonoBehaviour
{
    private Transform LevelDataEntryContent;
    public GameObject LevelEntryDisplayItem;
    public TMP_InputField SearchBar;
    public string LevelName1;

    public void Start()
    {
        LevelDataEntryContent = GameObject.Find("Canvas/Menus/Gameplay/DownloadScreen/Background/Foreground/ScrollView/Viewport/Content").transform;
    }

    public void DownloadLevelData()
    {
        LootLockerSDKManager.GetAssetListWithCount(1000, (response) =>
        {
            foreach (Transform child in LevelDataEntryContent)
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < response.assets.Length; i++)
            {
                GameObject displayItem = Instantiate(LevelEntryDisplayItem, transform.position, Quaternion.identity);
                displayItem.transform.SetParent(LevelDataEntryContent);
                displayItem.GetComponent<LevelInitializer>().SetInformations(i, response.assets[i].name);

                LootLockerFile[] levelFiles = response.assets[i].files;
                //image index
                int j = FindIndex(levelFiles, "PRIMARY_THUMBNAIL");
                if (j == -1)
                {
                    Debug.Log("No primary thumbnail found");
                    return;
                }
                StartCoroutine(LoadLevelIcon(levelFiles[j].url.ToString(), displayItem.GetComponent<LevelInitializer>().LevelIcon));
                //other files index
                j = FindIndex(levelFiles, "FILE");
                if (j == -1)
                {
                    Debug.Log("No file type found");
                    return;
                }
                displayItem.GetComponent<LevelInitializer>().DataFileURL = levelFiles[j].url.ToString();
            }

        }, null, true, new Dictionary<string, string>() { { "Context", "TileMap" } });
    }
    public void DownloadLevelTreeData(string levelName)
    {
        LootLockerSDKManager.GetAssetListWithCount(50, (response) =>
        {
            for (int i = 0; i < response.assets.Length; i++)
            {
                if (response.assets[i].name == levelName)
                {
                    LootLockerFile[] levelFiles = response.assets[i].files;
                    for (int j = 0; j < levelFiles.Length; j++)
                    {
                        StartCoroutine(DownloadTextFile(levelFiles[j].url.ToString(), levelName, j));
                    }
                }
            }
        }, null, true, new Dictionary<string, string>() { { "Context", "CharacterTree" } });
    }
    private IEnumerator DownloadTextFile(string url, string LevelName, int index)
    {
        LevelName1 = LevelName;
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        string type = "";
        switch (index)
        {
            case 0:
                type = "Attributes";
                break;
            case 1:
                type = "Characters";
                break;
            case 2:
                type = "Methods";
                break;
            case 3:
                type = "Restrictions";
                break;
            case 4:
                type = "SpecialAbilities";
                break;
        }

        string filePath = Directory.GetCurrentDirectory() + "/Assets/Resources/Screenshots/Download/" + LevelName + "/" + type + ".json";
        File.WriteAllText(filePath, www.downloadHandler.text);
    }
    public int FindIndex(LootLockerFile[] files, string purpose)
    {
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].tags[0].Contains(purpose))
            {
                return i;
            }
        }
        return -1;
    }
    IEnumerator LoadLevelIcon(string imageURL, Image levelImage)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageURL);
        yield return www.SendWebRequest();

        Texture2D loadedImage = DownloadHandlerTexture.GetContent(www);
        levelImage.sprite = Sprite.Create(loadedImage, new Rect(0f, 0f, loadedImage.width, loadedImage.height), Vector2.zero);
    }
    public void SearchLevel()
    {
        foreach (Transform child in LevelDataEntryContent)
        {
            if (child.GetComponent<LevelInitializer>().GetLevelName().ToLower().Contains(SearchBar.text.ToLower()))
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
        SearchBar.text = "";
    }
}
