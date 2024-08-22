using LootLocker.Requests;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LootLockerManager : MonoBehaviour
{
    public GameObject buttons;
    public void Start()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                if (SceneManager.GetActiveScene().name == "OnlineBuilder")
                {
                    buttons.SetActive(true);
                }
                if (SceneManager.GetActiveScene().name == "OnlinePlayground")
                {
                    if (GameObject.Find("LevelManager").GetComponent<LevelDownload>())
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
        string path = Path.Combine(Application.dataPath, "StreamingAssets", "Resources", "Json", "AiModelData.json");
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
