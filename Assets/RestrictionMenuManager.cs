using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestrictionMenuManager : MonoBehaviour
{
    public void Awake()
    {   
        GameObject manager = GameObject.Find("Manager");
        if (manager.GetComponent<RestrictionMenu>() == null)
        {
            manager.AddComponent<RestrictionMenu>();
        }       
    }
}
