using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using LootLocker.Requests;
using TMPro;
using UnityEngine.Networking;

public class LevelManager : MonoBehaviour
{
    public TMP_InputField LevelNameInputField;
    public string LevelName;
    public GameObject LevelUploadUi;
    public GameObject LevelEntryDisplayItem;
    public Transform LevelDataEntryContent;

    public void CreateLevel()
    {
        LevelName = LevelNameInputField.text;
        LootLockerSDKManager.CreatingAnAssetCandidate(LevelName, (response) =>
        {
            if (response.success)
            {
                UploadLevelData((int)response.asset_candidate.id);
            }
            else
            {
                Debug.Log("Level Creation Failed");
            }
        });
    }

    public void UploadLevelData(int levelID)
    {
        string screenshotFilePath = "Assets/Screenshots/Level-Screenshot.png";
        LootLocker.LootLockerEnums.FilePurpose screenshotFileType = LootLocker.LootLockerEnums.FilePurpose.primary_thumbnail;

        LootLockerSDKManager.AddingFilesToAssetCandidates(levelID, screenshotFilePath, "Level-Screenshot.png", screenshotFileType, (screenshotResponse) =>
        {
            if (screenshotResponse.success)
            {
                string textFilePath = Directory.GetCurrentDirectory() + "/Assets/Screenshots/" + LevelName + "-Data.json";
                GetComponent<SaveHandler>().OnSave(textFilePath);

                //coverting json file to txt file
                string json = File.ReadAllText(textFilePath);
                File.WriteAllText(textFilePath, json);

                LootLocker.LootLockerEnums.FilePurpose textFileType = LootLocker.LootLockerEnums.FilePurpose.file;

                LootLockerSDKManager.AddingFilesToAssetCandidates(levelID, textFilePath, LevelName + "Data.txt", textFileType, (textResponse) =>
                {
                    if (textResponse.success)
                    {
                        LootLockerSDKManager.UpdatingAnAssetCandidate(levelID, true, (updatedResponse) =>{});
                    }
                    else
                    {
                        Debug.Log("Level Data Upload Failed");
                    }
                });
            }
            else
            {
                Debug.Log("Screenshot Upload Failed");
            }
        });
    }

    public void TakeScreenshot()
    {
        string filepath = Directory.GetCurrentDirectory() + "/Assets/Screenshots/";
        ScreenCapture.CaptureScreenshot(Path.Combine(filepath, "Level-Screenshot.png"));
    }

    IEnumerator WaitSreenShot()
    {
        TakeScreenshot();
        yield return new WaitForSeconds(1.0f);
        LevelUploadUi.SetActive(true);
    }

    public void OpenUploadLevelUI()
    {
        StartCoroutine(WaitSreenShot());
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
                displayItem.GetComponent<LevelEntryData>().ID = i;
                displayItem.GetComponent<LevelEntryData>().LevelName = response.assets[i].name;

                LootLockerFile[] levelImageFiles = response.assets[i].files;
                StartCoroutine(LoadLevelIcon(levelImageFiles[0].url.ToString(), displayItem.GetComponent<LevelEntryData>().LevelIcon));
                displayItem.GetComponent<LevelEntryData>().TextFileURL = levelImageFiles[1].url.ToString();

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
