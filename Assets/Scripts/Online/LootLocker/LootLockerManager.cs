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
            }
            else
            {
                Debug.Log("Login Failed");
            }
        });
    }
}
