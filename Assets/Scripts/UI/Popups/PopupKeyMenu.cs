using System.Collections;
using System.Collections.Generic;
using LootLocker.Extension.DataTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupKeyMenu : MonoBehaviour
{
    public CharacterChallangeManager characterChallangeManager;
    public CharacterAppearanceManager characterAppearanceManager;
    public EncapsulationManager encapsulationManager;
    public UpcastingManager upcastingManager;
    public Transform Content;
    public GameObject ButtonPrefab;
    public GameObject PopupMenu;


    public void Start()
    {
        characterChallangeManager = GameObject.Find("Canvas/Popups").GetComponent<CharacterChallangeManager>();
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
        if (RestrictionManager.Instance.AllowUpcasting && !RestrictionManager.Instance.AllowEncapsulation && !RestrictionManager.Instance.AllowOverride)
        { upcastingManager.ToggleActivation(); return; }
        else if (!RestrictionManager.Instance.AllowUpcasting && RestrictionManager.Instance.AllowEncapsulation && !RestrictionManager.Instance.AllowOverride)
        { encapsulationManager.ToggleActivation(); return; }
        else if (!RestrictionManager.Instance.AllowUpcasting && !RestrictionManager.Instance.AllowEncapsulation && RestrictionManager.Instance.AllowOverride)
        { characterAppearanceManager.ToggleActivation(); return; }

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

        if (RestrictionManager.Instance.AllowUpcasting)
        {
            GameObject button = Instantiate(ButtonPrefab, Content);
            button.GetComponentInChildren<TMP_Text>().text = "Upcasting";
            button.GetComponentInChildren<TMP_Text>().fontSize = 40;
            button.GetComponentInChildren<TMP_Text>().fontStyle = FontStyles.Bold;
            button.GetComponent<Button>().onClick.AddListener(() => upcastingManager.ToggleActivation());
            button.GetComponent<Button>().onClick.AddListener(() => ToggleActivation());
        }
        if (RestrictionManager.Instance.AllowEncapsulation && encapsulationManager.Checker())
        {
            GameObject button = Instantiate(ButtonPrefab, Content);
            button.GetComponentInChildren<TMP_Text>().text = "Encapsulation";
            button.GetComponentInChildren<TMP_Text>().fontSize = 40;
            button.GetComponentInChildren<TMP_Text>().fontStyle = FontStyles.Bold;
            button.GetComponent<Button>().onClick.AddListener(() => encapsulationManager.ToggleActivation());
            button.GetComponent<Button>().onClick.AddListener(() => ToggleActivation());
        }
        if (RestrictionManager.Instance.AllowOverride)
        {
            if (RestrictionManager.Instance.AllowOverride)
            {
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "C2L3")
                {
                    GameObject button = Instantiate(ButtonPrefab, Content);
                    button.GetComponentInChildren<TMP_Text>().text = "Override";
                    button.GetComponentInChildren<TMP_Text>().fontSize = 40;
                    button.GetComponentInChildren<TMP_Text>().fontStyle = FontStyles.Bold;
                    button.GetComponent<Button>().onClick.AddListener(() => characterChallangeManager.ToggleActivation());
                    button.GetComponent<Button>().onClick.AddListener(() => ToggleActivation());
                }
                else
                {
                    GameObject button = Instantiate(ButtonPrefab, Content);
                    button.GetComponentInChildren<TMP_Text>().text = "Override";
                    button.GetComponentInChildren<TMP_Text>().fontSize = 40;
                    button.GetComponentInChildren<TMP_Text>().fontStyle = FontStyles.Bold;
                    button.GetComponent<Button>().onClick.AddListener(() => characterAppearanceManager.ToggleActivation());
                    button.GetComponent<Button>().onClick.AddListener(() => ToggleActivation());
                }
            }
        }
    }
}