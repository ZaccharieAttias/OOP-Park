using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.Networking;
using LootLocker.Requests;

public class LevelDownload : MonoBehaviour
{
    private string LevelName;
    private Transform LevelDataEntryContent;
    public GameObject LevelEntryDisplayItem;

    public void Start()
    {
        LevelDataEntryContent = GameObject.Find("Canvas/Menus/Gameplay/DownloadScreen/ScrollView/Viewport/Content").transform;
    }

    public void DownloadLevelData()
    {
        LootLockerSDKManager.GetAssetListWithCount(10, (response) =>
        {
            //supprimez les anciens éléments de la liste
            foreach (Transform child in LevelDataEntryContent)
            {
                Destroy(child.gameObject);
            }
            for(int i =0; i < response.assets.Length; i++)
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
                j = FindIndex(levelFiles,"FILE");
                if (j == -1)
                {
                    Debug.Log("No primary thumbnail found");
                    return;
                }
                displayItem.GetComponent<LevelInitializer>().TextFileURL = levelFiles[j].url.ToString();
            }

        }, null, true);
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
}
