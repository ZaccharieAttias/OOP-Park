using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LootLocker.Requests;
using System.IO;

public class LootLockerManager : MonoBehaviour
{
    public GameObject buttons;
    public void Start()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {   
                if(SceneManager.GetActiveScene().name == "OnlineBuilder")
                {
                    buttons.SetActive(true);
                }
                if (SceneManager.GetActiveScene().name == "OnlinePlayground")
                {
                    if(GameObject.Find("LevelManager").GetComponent<LevelDownload>())
                    {
                        GameObject.Find("LevelManager").GetComponent<LevelDownload>().DownloadLevelData();
                    }
                }
            }
            else
            {
                Debug.Log("Login Failed");
            }
        });
    }
    public void UploadPlayerData()
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Resources", "Json", "AiModelData.json");
        LootLockerSDKManager.UploadPlayerFile(path, "save_game", response =>
        {
            if (response.success)
            {
                Debug.Log("Successfully uploaded player file, url: " + response.url);
            } 
            else
            {
                Debug.Log("Error uploading player file");
            }
        });
    }
}
