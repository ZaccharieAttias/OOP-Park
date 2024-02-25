using UnityEngine;
using UnityEngine.UI;


public class GameplayManager : MonoBehaviour
{
    public GameObject Gameplay;
    public GameObject CharacterCenter;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        Gameplay = GameObject.Find("Canvas/Menus/Gameplay");
        CharacterCenter = GameObject.Find("Canvas/Menus/CharacterCenter");

        Button activateTree = Gameplay.transform.Find("SwapScreen").GetComponent<Button>();
        activateTree.onClick.AddListener(() => ToggleOff());

        Button activateGameplay = CharacterCenter.transform.Find("SwapScreen").GetComponent<Button>();
        activateGameplay.onClick.AddListener(() => ToggleOn());
    }


    public void ToggleOn()
    {
        SceneManagement.SceneResume();

        Gameplay.SetActive(true);
        CharacterCenter.SetActive(false);
    }
    public void ToggleOff()
    {
        SceneManagement.ScenePause();

        Gameplay.SetActive(false);
        CharacterCenter.SetActive(true);
    }
}
