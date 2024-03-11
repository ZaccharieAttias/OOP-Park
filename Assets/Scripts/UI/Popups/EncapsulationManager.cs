using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncapsulationManager : MonoBehaviour
{
    public GameObject Popup;
    public GameObject AllButton;
    public GameObject Button;

    public Transform ContentGetter;
    public Transform ContentSetter;

    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/GetterSetter");
        AllButton =  GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Description/A");
        Button = Resources.Load<GameObject>("Buttons/Default");

        ContentGetter = GameObject.Find("Canvas/Popups/GetterSetter/ALL/Background/Foreground/Buttons/ScrollViewGet/ViewPort/Content").GetComponent<Transform>();
        ContentSetter = GameObject.Find("Canvas/Popups/GetterSetter/ALL/Background/Foreground/Buttons/ScrollViewSet/ViewPort/Content").GetComponent<Transform>();

        if (RestrictionManager.Instance.AllowEncapsulation is true)
        {
            AllButton.GetComponent<Button>().onClick.AddListener(ToggleOn);
            AllButton.SetActive(true);
        }
    }

    public void ToggleOn()
    {
        Popup.SetActive(true);
    }

    public void ToggleOff()
    {
        Popup.SetActive(false);
    }

}
