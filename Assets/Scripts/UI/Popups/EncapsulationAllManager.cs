using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EncapsulationAllManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Popup;
    public Transform SetContent;
    public Transform GetContent;

    [Header("Buttons")]
    public GameObject ButtonPrefab;
    public GameObject allButton;
    public Button CloseButton;

    [Header("Attribute Collections")]
    public List<Attribute> SetCollection;
    public List<Attribute> GetCollection;


    public void Start()
    {
        InitializeUIElements();
        InitializeButtons();
        InitializeAttributeCollections();
    }
    public void InitializeUIElements()
    {
        Popup = GameObject.Find("Canvas/Popups/EncapsulationAll");
        SetContent = Popup.transform.Find("Background/Foreground/Set/Back/ScrollView/Viewport/Content").GetComponent<Transform>();
        GetContent = Popup.transform.Find("Background/Foreground/Get/Back/ScrollView/Viewport/Content").GetComponent<Transform>();
    }
    public void InitializeButtons()
    {
        ButtonPrefab = Resources.Load<GameObject>("Buttons/Default");
        allButton = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Description/All");
        allButton.GetComponent<Button>().onClick.AddListener(ToggleOn);
        allButton.SetActive(RestrictionManager.Instance.AllowEncapsulation);

        CloseButton = Popup.transform.Find("Background/Foreground/Buttons/Close").GetComponent<Button>();
        CloseButton.onClick.AddListener(ToggleOff);

    }
    public void InitializeAttributeCollections()
    {
        SetCollection = new List<Attribute>();
        GetCollection = new List<Attribute>();
    }

    public void LoadPopup()
    {
        ClearContentPanel();
        PopulateCollections();
        LoadAttributes("Set");
        LoadAttributes("Get");
    }
    public void ClearContentPanel()
    {
        foreach (Transform attributeTransform in SetContent)
        {
            Destroy(attributeTransform.gameObject);
        }
        
        foreach (Transform attributeTransform in GetContent)
        {
            Destroy(attributeTransform.gameObject);
        }
    }
    public void PopulateCollections()
    {
        var currentCharacter = CharactersData.CharactersManager.CurrentCharacter;
        SetCollection = currentCharacter.Attributes.Where(attribute => attribute.Setter).ToList();
        GetCollection = currentCharacter.Attributes.Where(attribute => attribute.Getter).ToList();
    }
    public void LoadAttributes(string type)
    {
        var collection = (type == "Get") ? GetCollection : SetCollection;
        var content = (type == "Get") ? GetContent : SetContent;

        foreach (var attribute in collection)
        {
            GameObject attributeGameObject = Instantiate(ButtonPrefab, content);
            attributeGameObject.name = $"{attribute.Name} {type}";

            TMP_Text buttonText = attributeGameObject.GetComponentInChildren<TMP_Text>();
            buttonText.text = attribute.Name;
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