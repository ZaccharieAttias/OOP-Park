using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTMenu : MonoBehaviour
{
    [SerializeField]  GameObject htMenu;
    
    public void ToHTMenu()
    {
        htMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ToGameplayScreen()
    {
        htMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}
