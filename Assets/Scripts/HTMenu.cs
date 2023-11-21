using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTMenu : MonoBehaviour
{
    [SerializeField]  GameObject htMenu;
    
    public void ToGameplayScreen()
    {
        htMenu.SetActive(false);
    }

    public void ToHTMenu()
    {
        htMenu.SetActive(true);
    }
}
