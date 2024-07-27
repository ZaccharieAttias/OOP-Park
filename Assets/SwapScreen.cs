using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapScreen : MonoBehaviour
{
    public Button SwapButtonToGameplay;
    public Button SwapButtonToCharacterCenter;
    public CharacterEditor1 CharacterEditor;
    public bool firstTime = true;

    public void Start()
    {
        CharacterEditor = GameObject.Find("Scripts/CharacterEditor").GetComponent<CharacterEditor1>();
        SwapButtonToGameplay = GameObject.Find("Canvas/Menus/CharacterCenter/SwapScreen").GetComponent<Button>();
        SwapButtonToGameplay.onClick.AddListener(() => CharacterEditor.LoadFromJson());

        SwapButtonToCharacterCenter = GameObject.Find("Canvas/Menus/Gameplay/SwapScreen").GetComponent<Button>();

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "C2L4") SwapButtonToGameplay.gameObject.SetActive(false);
    }
    public void FisrtSwap()
    {
        if (firstTime)
        {
            firstTime = false;
            SwapButtonToCharacterCenter.onClick.RemoveAllListeners();
            if (RestrictionManager.Instance.AllowSingleInheritance)
                SwapButtonToCharacterCenter.onClick.AddListener(() => GameObject.Find("Canvas/Popups").GetComponent<CharactersCreationManager>().ToggleOn());
            SwapButtonToCharacterCenter.onClick.AddListener(() => GameObject.Find("Canvas/Menus").GetComponent<GameplayManager>().ToggleOff());
            GameObject.Find("Grid/LevelBuilder").GetComponent<LevelBuilderB>().SetUI();
        }
    }
}
