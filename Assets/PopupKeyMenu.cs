using System.Collections;
using System.Collections.Generic;
using LootLocker.Extension.DataTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupKeyMenu : MonoBehaviour
{
    public CharacterAppearanceManager characterAppearanceManager;
    public EncapsulationManager encapsulationManager;
    public UpcastingManager upcastingManager;
    public Transform Content;
    public GameObject ButtonPrefab;
    public GameObject PopupMenu;
    public void Start()
    {
        characterAppearanceManager = GameObject.Find("Canvas/Popups").GetComponent<CharacterAppearanceManager>();
        encapsulationManager = GameObject.Find("Canvas/Popups").GetComponent<EncapsulationManager>();
        upcastingManager = GameObject.Find("Canvas/Popups").GetComponent<UpcastingManager>();
        Content = GameObject.Find("Canvas/Popups/KeyMenu/Background/Foreground/Buttons/Content").transform;
        ButtonPrefab = Resources.Load<GameObject>("Buttons/Default");
        PopupMenu = GameObject.Find("Canvas/Popups/KeyMenu");
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && (RestrictionManager.Instance.AllowUpcasting || RestrictionManager.Instance.AllowEncapsulation || RestrictionManager.Instance.AllowOverride))
        {
            ToggleActivation();
        }
    }
    public void ToggleOn()
    {
        SceneManagement.ScenePause("KeyMenu");
        PopupMenu.SetActive(true);
        LoadPopupButtons();
    }
    public void ToggleOff()
    {
        PopupMenu.SetActive(false);
        SceneManagement.SceneResume("KeyMenu");
    }
    public void ToggleActivation()
    {
        if (PopupMenu.activeSelf)
        {
            ToggleOff();
        }
        else
        {
            ToggleOn();
        }
    }
    public void LoadPopupButtons()
    {
        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }

        if(RestrictionManager.Instance.AllowUpcasting)
        {
            GameObject button = Instantiate(ButtonPrefab, Content);
            button.GetComponentInChildren<TMP_Text>().text = "Upcasting";
            button.GetComponent<Button>().onClick.AddListener(() => upcastingManager.ToggleActivation());
            button.GetComponent<Button>().onClick.AddListener(() => ToggleActivation());
        }
        if(RestrictionManager.Instance.AllowEncapsulation && encapsulationManager.Checker())
        {
            GameObject button = Instantiate(ButtonPrefab, Content);
            button.GetComponentInChildren<TMP_Text>().text = "Encapsulation";
            button.GetComponent<Button>().onClick.AddListener(() => encapsulationManager.ToggleActivation());
            button.GetComponent<Button>().onClick.AddListener(() => ToggleActivation());
        }
        if(RestrictionManager.Instance.AllowOverride)
        {
            GameObject button = Instantiate(ButtonPrefab, Content);
            button.GetComponentInChildren<TMP_Text>().text = "Override";
            button.GetComponent<Button>().onClick.AddListener(() => characterAppearanceManager.ToggleActivation());
            button.GetComponent<Button>().onClick.AddListener(() => ToggleActivation());
        }
    }
}