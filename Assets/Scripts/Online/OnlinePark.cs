using UnityEngine;
using UnityEngine.UI;

public class OnlinePark : MonoBehaviour
{
    public Button CreateButton;
    public Button PlayButton;

    public void Start()
    {
        CreateButton = GameObject.Find("Canvas/Menus/Menu/BuildLevel").GetComponent<Button>();
        PlayButton = GameObject.Find("Canvas/Menus/Menu/BrowseLevel").GetComponent<Button>();

        CreateButton.onClick.AddListener(() => Create());
        PlayButton.onClick.AddListener(() => Play());
    }

    public void Create()
    {
        SceneManagement.RestrictionMenu();
    }

    public void Play()
    {
        Debug.Log("Playing the level...");
    }
}