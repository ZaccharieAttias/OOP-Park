using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PopupKeyMenu : MonoBehaviour
{
    [Header("Scripts")]
    public CharacterAppearanceManager CharacterAppearanceManager;
    public EncapsulationManager EncapsulationManager;
    public UpcastingManager UpcastingManager;


    [Header("UI Elements")]
    public Transform Content;
    public GameObject PopupMenu;

    [Header("Buttons")]
    public GameObject ButtonPrefab;


    public void Start()
    {
        InitializeScripts();
        InitializeUIElements();
        InitializeButtons();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && ShouldTogglePopup()) ToggleActivation();
    }
    public void InitializeScripts()
    {
        CharacterAppearanceManager = GameObject.Find("Canvas/Popups").GetComponent<CharacterAppearanceManager>();
        EncapsulationManager = GameObject.Find("Canvas/Popups").GetComponent<EncapsulationManager>();
        UpcastingManager = GameObject.Find("Canvas/Popups").GetComponent<UpcastingManager>();
    }
    public void InitializeUIElements()
    {
        Content = GameObject.Find("Canvas/Popups/KeyMenu/Background/Foreground/Buttons/Content").transform;
        PopupMenu = GameObject.Find("Canvas/Popups/KeyMenu");
    }
    public void InitializeButtons()
    {
        ButtonPrefab = Resources.Load<GameObject>("Buttons/Default");
    }

    public void LoadPopupButtons()
    {
        ClearExistingButtons();

        var restrictionManager = RestrictionManager.Instance;

        if (restrictionManager.AllowUpcasting && UpcastingManager.Checker())
        {
            CreateButton("Upcasting", () => UpcastingManager.ToggleActivation());
        }

        if (restrictionManager.AllowEncapsulation && EncapsulationManager.Checker())
        {
            CreateButton("Encapsulation", () => EncapsulationManager.ToggleActivation());
        }

        if (restrictionManager.AllowOverride)
        {
            CreateButton("Override", () => CharacterAppearanceManager.ToggleActivation());
        }
    }
    public void ClearExistingButtons()
    {
        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }
    }
    public void CreateButton(string buttonText, UnityEngine.Events.UnityAction onClickAction)
    {
        GameObject button = Instantiate(ButtonPrefab, Content);
        var textComponent = button.GetComponentInChildren<TMP_Text>();
        textComponent.text = buttonText;
        textComponent.fontSize = 40;
        textComponent.fontStyle = FontStyles.Bold;

        var buttonComponent = button.GetComponent<Button>();
        buttonComponent.onClick.AddListener(onClickAction);
        buttonComponent.onClick.AddListener(ToggleActivation);
    }
    public bool ShouldTogglePopup()
    {
        var restrictionManager = RestrictionManager.Instance;
        return restrictionManager.AllowUpcasting || restrictionManager.AllowEncapsulation || restrictionManager.AllowOverride;
    }
    public bool HandleSpecialCases()
    {
        var restrictionManager = RestrictionManager.Instance;

        if (restrictionManager.AllowUpcasting && !restrictionManager.AllowEncapsulation && !restrictionManager.AllowOverride)
        {
            UpcastingManager.ToggleActivation();
            return true;
        }

        if (!restrictionManager.AllowUpcasting && restrictionManager.AllowEncapsulation && !restrictionManager.AllowOverride)
        {
            EncapsulationManager.ToggleActivation();
            return true;
        }

        if (!restrictionManager.AllowUpcasting && !restrictionManager.AllowEncapsulation && restrictionManager.AllowOverride)
        {
            CharacterAppearanceManager.ToggleActivation();
            return true;
        }

        return false;
    }

    public void ToggleOn()
    {
        if (HandleSpecialCases()) return;

        SceneManagement.ScenePause("KeyMenu");

        PopupMenu.SetActive(true);
        LoadPopupButtons();
    }
    public void ToggleOff()
    {
        SceneManagement.SceneResume("KeyMenu");

        PopupMenu.SetActive(false);
    }
    public void ToggleActivation()
    {
        if (PopupMenu.activeSelf) ToggleOff();
        else ToggleOn();
    }
}