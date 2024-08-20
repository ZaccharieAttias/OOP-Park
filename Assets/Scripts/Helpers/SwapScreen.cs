using UnityEngine;
using UnityEngine.UI;


public class SwapScreen : MonoBehaviour
{
    [Header("Scripts")]
    public AiModelData AiModelData;
    public CharacterEditor1 CharacterEditor;

    [Header("Buttons")]
    public Button SwapButtonToGameplay;
    public Button SwapButtonToCharacterCenter;

    [Header("Settings")]
    public bool firstTime;


    public void Start()
    {
        InitializeScripts();
        InitializeButtons();
        InitializeSettings();
    }
    public void InitializeScripts()
    {
        AiModelData = GameObject.Find("Scripts/AiModelData").GetComponent<AiModelData>();
        CharacterEditor = GameObject.Find("Scripts/CharacterEditor").GetComponent<CharacterEditor1>();
    }
    public void InitializeButtons()
    {
        SwapButtonToGameplay = GameObject.Find("Canvas/Menus/CharacterCenter/SwapScreen").GetComponent<Button>();
        SwapButtonToGameplay.onClick.AddListener(() => CharacterEditor.LoadFromJson());
        SwapButtonToGameplay.onClick.AddListener(() => AiModelData.AddCharacterData());

        SwapButtonToCharacterCenter = GameObject.Find("Canvas/Menus/Gameplay/SwapScreen").GetComponent<Button>();
    }
    public void InitializeSettings()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "C2L4") SwapButtonToGameplay.gameObject.SetActive(false);
        firstTime = true;

    }

    public void FisrtSwap()
    {
        if (firstTime)
        {
            firstTime = false;
            SwapButtonToCharacterCenter.onClick.RemoveAllListeners();

            SwapButtonToCharacterCenter.onClick.AddListener(() => GameObject.Find("Canvas/Popups").GetComponent<CharactersCreationManager>().ToggleOn());

            SwapButtonToCharacterCenter.onClick.AddListener(() => GameObject.Find("Canvas/Menus").GetComponent<GameplayManager>().ToggleOff());
            GameObject.Find("Grid/LevelBuilder").GetComponent<LevelBuilderB>().SetUI();
        }
    }
}