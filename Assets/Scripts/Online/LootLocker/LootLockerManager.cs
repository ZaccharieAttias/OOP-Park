using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LootLocker.Requests;

public class LootLockerManager : MonoBehaviour
{
    public void Login()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Login Success");
                //SceneManager.LoadScene("OnlineLoadLevel");
                
                //set the object to active
                GameObject.Find("Canvas/GameplayScreen/Login").SetActive(false);
                
            }
            else
            {
                Debug.Log("Login Failed");
            }
        });
    }
}
