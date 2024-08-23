using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class PopupKeyManager : MonoBehaviour
{
    [Header("Scripts")]
    public CharacterAppearanceManager CharacterAppearanceManager;
    public EncapsulationManager EncapsulationManager;
    public TypeCastingManager TypeCastingManager;
    public UpcastingManager UpcastingManager;

    [Header("UI Elements")]
    public Transform Content;
    public GameObject PopupMenu;

    [Header("Buttons")]
    public GameObject ButtonPrefab;
    private bool canActivate;


    public void Start()
    {
        InitializeScripts();
        InitializeUIElements();
        InitializeButtons();
    }
    public void Update()
    {
        canActivate = true;
        if (GameObject.Find("Scripts/PlayTestManager") != null)
        {
            if (!GameObject.Find("Scripts/PlayTestManager").GetComponent<PlayTestManager>().IsTestGameplay)
                canActivate = false;
        }
        if (Input.GetKeyDown(KeyCode.G) && canActivate) ToggleActivation();
    }
    public void InitializeScripts()
    {
        CharacterAppearanceManager = GameObject.Find("Canvas/Popups").GetComponent<CharacterAppearanceManager>();
        EncapsulationManager = GameObject.Find("Canvas/Popups").GetComponent<EncapsulationManager>();
        TypeCastingManager = GameObject.Find("Canvas/Popups").GetComponent<TypeCastingManager>();
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

        if (restrictionManager.AllowTypeCasting && TypeCastingManager.Checker())
        {
            CreateButton("TypeCasting", () => TypeCastingManager.ToggleActivation());
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
    public void CreateButton(string buttonText, UnityAction onClickAction)
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
    public void ToggleOn()
    {
        if (!GameObject.Find("Canvas/Menus/Gameplay").activeSelf) return;
        if (!TypeCastingManager.Checker() && !UpcastingManager.Checker() && !EncapsulationManager.Checker() && !RestrictionManager.Instance.AllowOverride) return;

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