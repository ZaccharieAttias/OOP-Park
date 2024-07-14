using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LootLocker.Requests;

public class LootLockerManager : MonoBehaviour
{
    public GameObject buttons;
    public void Start()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {   
                buttons.SetActive(true);
                
                // si GameObject.Find("LevelManager") a le composant LevelDownload alors on appelle la méthode DownloadLevelData
                if (GameObject.Find("LevelManager").GetComponent<LevelDownload>())
                {
                    GameObject.Find("LevelManager").GetComponent<LevelDownload>().DownloadLevelData();
                }
            }
            else
            {
                Debug.Log("Login Failed");
            }
        });
    }
}
