using UnityEngine;
using UnityEngine.UI;


public class GameplayManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Gameplay;
    public GameObject CharacterCenter;


    public void Start()
    {
        InitializeUIElements();
        InitializeEventListeners();
    }
    public void InitializeUIElements()
    {
        Gameplay = GameObject.Find("Canvas/Menus/Gameplay");
        CharacterCenter = GameObject.Find("Canvas/Menus/CharacterCenter");
    }
    public void InitializeEventListeners()
    {
        Button activateTree = Gameplay.transform.Find("SwapScreen").GetComponent<Button>();
        activateTree.onClick.AddListener(() => ToggleOff());

        Button activateGameplay = CharacterCenter.transform.Find("SwapScreen").GetComponent<Button>();
        activateGameplay.onClick.AddListener(() => ToggleOn());
    }

    public void ToggleOn()
    {
        if (CharactersData.CharactersManager.CurrentCharacter != null && CharactersData.CharactersManager.CurrentCharacter.IsAbstract) return;

        SceneManagement.SceneResume("GameplayManager");

        Gameplay.SetActive(true);
        CharacterCenter.SetActive(false);
    }
    public void ToggleOff()
    {
        SceneManagement.ScenePause("GameplayManager");

        Gameplay.SetActive(false);
        CharacterCenter.SetActive(true);
    }
}