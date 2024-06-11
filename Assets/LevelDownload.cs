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

                LootLockerFile[] levelImageFiles = response.assets[i].files;
                StartCoroutine(LoadLevelIcon(levelImageFiles[0].url.ToString(), displayItem.GetComponent<LevelInitializer>().LevelIcon));
                displayItem.GetComponent<LevelInitializer>().TextFileURL = levelImageFiles[1].url.ToString();

            }

        }, null, true);
    }

    IEnumerator LoadLevelIcon(string imageURL, Image levelImage)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageURL);
        yield return www.SendWebRequest();

        Texture2D loadedImage = DownloadHandlerTexture.GetContent(www);
        levelImage.sprite = Sprite.Create(loadedImage, new Rect(0f, 0f, loadedImage.width, loadedImage.height), Vector2.zero);

    }
}
