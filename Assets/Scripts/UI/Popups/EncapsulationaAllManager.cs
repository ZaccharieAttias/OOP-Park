using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EncapsulationaAllManager : MonoBehaviour
{
    public GameObject Popup;

    public Transform SetContent;
    public Transform GetContent;

    public List<Attribute> SetCollection;
    public List<Attribute> GetCollection;

    public GameObject ButtonPrefab;
    public GameObject allButton;
    public Button CloseButton;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/EncapsulationAll");

        SetCollection = new List<Attribute>();
        GetCollection = new List<Attribute>();

        SetContent = Popup.transform.Find("Background/Foreground/Set/Back/ScrollView/Viewport/Content").GetComponent<Transform>();
        GetContent = Popup.transform.Find("Background/Foreground/Get/Back/ScrollView/Viewport/Content").GetComponent<Transform>();

        ButtonPrefab = Resources.Load<GameObject>("Buttons/Default");
        allButton = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Description/All");
        CloseButton = Popup.transform.Find("Background/Foreground/Buttons/Close").GetComponent<Button>();

        if (RestrictionManager.Instance.AllowEncapsulation)
        {
            allButton.GetComponent<Button>().onClick.AddListener(() => ToggleOn());
            allButton.SetActive(true);
            CloseButton.onClick.AddListener(() => ToggleOff());
        }
    }

    private void LoadPopup()
    {
        ClearContentPanel();

        var currentCharacter = CharactersData.CharactersManager.CurrentCharacter;
        SetCollection = currentCharacter.Attributes.Where(item => item.Setter is true).ToList();
        GetCollection = currentCharacter.Attributes.Where(item => item.Getter is true).ToList();

        Load("Set");
        Load("Get");
    }
    private void ClearContentPanel()
    {
        SetContent.Cast<Transform>().ToList().ForEach(attributeTransform => Destroy(attributeTransform.gameObject));
        GetContent.Cast<Transform>().ToList().ForEach(attributeTransform => Destroy(attributeTransform.gameObject));
    }
    public void Load(string type)
    {
        var collection = (type == "Get") ? GetCollection : SetCollection;
        var content = (type == "Get") ? GetContent : SetContent;

        foreach (var attribute in collection)
        {
            GameObject attributeGameObject = Instantiate(ButtonPrefab, content);
            attributeGameObject.name = $"{attribute.Name} {type}";

            TMP_Text ButtonText = attributeGameObject.GetComponentInChildren<TMP_Text>();
            ButtonText.text = attribute.Name;
        }
    }

    public void ToggleOn()
    {
        SceneManagement.ScenePause("EncapsulationAllManager");
        LoadPopup();
        Popup.SetActive(true);
    }
    public void ToggleOff()
    {
        SceneManagement.SceneResume("EncapsulationAllManager");
        Popup.SetActive(false);
    }
}
