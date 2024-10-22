using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EncapsulationManager : MonoBehaviour
{
    [Header("Scripts")]
    public Powerup Powerup;
    public SwipeMenu SwipeMenu;

    [Header("UI Elements")]
    public GameObject Popup;
    public Transform SetContent;
    public Transform GetContent;
    public GameObject CurrentSet;
    public TMP_InputField InputField;
    public string SelectedGetClick;

    [Header("Buttons")]
    public GameObject GetRowPrefab;
    public GameObject SetRowPrefab;
    public GameObject SetButtonPrefab;
    public GameObject CloseButton;

    [Header("Attribute Collections")]
    public List<Attribute> SetCollection;
    public List<(Attribute, List<CharacterB>)> GetCollection;


    public void Start()
    {
        InitializeScripts();
        InitializeUIElements();
        InitializeButtons();
        InitializeAttributeCollections();
    }
    public void InitializeScripts()
    {
        Powerup = GameObject.Find("Scripts/PowerUp").GetComponent<Powerup>();
        SwipeMenu = GameObject.Find("Canvas/Popups/Encapsulation/Background/Foreground/Set/Back/ScrollView/Viewport/Content").GetComponent<SwipeMenu>();
    }
    public void InitializeUIElements()
    {
        Popup = GameObject.Find("Canvas/Popups/Encapsulation");
        SetContent = Popup.transform.Find("Background/Foreground/Set/Back/ScrollView/Viewport/Content").GetComponent<Transform>();
        GetContent = Popup.transform.Find("Background/Foreground/Get/Back/ScrollView/Viewport/Content").GetComponent<Transform>();

        CurrentSet = null;

        InputField = Popup.transform.Find("Background/Foreground/Set/Input/InputField").GetComponent<TMP_InputField>();
        SelectedGetClick = "";
    }
    public void InitializeButtons()
    {
        SetRowPrefab = Resources.Load<GameObject>("Buttons/SetRow");
        GetRowPrefab = Resources.Load<GameObject>("Buttons/GetRow");

        SetButtonPrefab = Popup.transform.Find("Background/Foreground/Set/Input/Button").gameObject;
        SetButtonPrefab.GetComponent<Button>().onClick.AddListener(SetAttribute);

        CloseButton = GameObject.Find("Canvas/Popups/Encapsulation/Background/Foreground/Close");
        CloseButton.GetComponent<Button>().onClick.AddListener(ToggleOff);

    }
    public void InitializeAttributeCollections()
    {
        SetCollection = new List<Attribute>();
        GetCollection = new List<(Attribute, List<CharacterB>)>();
    }

    public void LoadPopup()
    {
        ClearContentPanel();
        PopulateCollections();

        SetCollection.ForEach(attribute => CreateSetAttributeUI(attribute));
        CurrentSet = SetContent.GetChild(0).gameObject;

        UpdateGetContent(CurrentSet.name.Split(' ')[0]);
        SelectedGetClick = CharactersData.CharactersManager.CurrentCharacter.Name;
        if (GetContent.childCount > 0)
            GetContent.GetChild(0).transform.Find("Selection").GetComponent<Image>().color = new Color(0.5f, 1, 0.5f, 1);
        if(SetContent.childCount > 0)
        {
            SetContent.GetChild(0).transform.Find("BackName").GetComponent<Image>().color = new Color(0.5f, 1, 0.5f, 1);
            SetContent.GetChild(0).transform.Find("BackValue").GetComponent<Image>().color = new Color(0.5f, 1, 0.5f, 1);
        }
    }
    public void ClearContentPanel()
    {
        SetContent.Cast<Transform>().ToList().ForEach(attributeTransform => Destroy(attributeTransform.gameObject));
        GetContent.Cast<Transform>().ToList().ForEach(attributeTransform => Destroy(attributeTransform.gameObject));

        SwipeMenu.CurrentScrollPosition = 1;
        SwipeMenu.PreviousScrollPosition = -1;
    }
    public void PopulateCollections()
    {
        var currentCharacter = CharactersData.CharactersManager.CurrentCharacter;
        SetCollection = currentCharacter.Attributes.Where(item => item.Setter).ToList();
        GetCollection = currentCharacter.Attributes
            .Where(attribute => attribute.Setter && attribute.Getter)
            .Select(attribute => (attribute, new List<CharacterB> { currentCharacter }))
            .ToList();

        FillGetCollection(currentCharacter.Parent);
    }
    public void FillGetCollection(CharacterB character)
    {
        if (character is null) return;

        foreach (var attribute in character.Attributes.Where(attr => attr.Getter && SetCollection.Any(item => item.Name == attr.Name)))
        {
            var attributeList = GetCollection.FirstOrDefault(item => item.Item1.Name == attribute.Name);
            if (attributeList.Item1 is not null && attributeList.Item2.Contains(character) is false) attributeList.Item2.Add(character);
            else GetCollection.Add((attribute, new List<CharacterB> { character }));
        }

        FillGetCollection(character.Parent);
    }
    public void UpdateGetContent(string attributeName)
    {
        GetContent.Cast<Transform>().ToList().ForEach(attributeTransform => Destroy(attributeTransform.gameObject));

        var currentCharacter = CharactersData.CharactersManager.CurrentCharacter;
        if (!GetCollection.Any(item => item.Item1.Name == attributeName)) return;

        var characters = GetCollection.FirstOrDefault(item => item.Item1.Name == attributeName).Item2;
        foreach (var character in characters)
        {
            CreateGetAttributeUI(character, attributeName);
        }
    }
    public void CreateSetAttributeUI(Attribute attribute)
    {
        GameObject attributeGameObject = Instantiate(SetRowPrefab, SetContent);
        attributeGameObject.name = attribute.Name + " Set";

        TMP_Text ButtonText = attributeGameObject.transform.Find("BackName").transform.Find("Name").GetComponent<TMP_Text>();
        ButtonText.text = attribute.Name;

        TMP_Text ButtonText2 = attributeGameObject.transform.Find("BackValue").transform.Find("Value").GetComponent<TMP_Text>();
        ButtonText2.text = attribute.Value.ToString();
    }
    public void CreateGetAttributeUI(CharacterB character, string attributeName)
    {
        GameObject attributeGameObject = Instantiate(GetRowPrefab, GetContent);
        attributeGameObject.name = $"{character.Name} Get";

        TMP_Text buttonText = attributeGameObject.transform.Find("BackName/Name").GetComponent<TMP_Text>();
        buttonText.text = character.Name;

        TMP_Text buttonValueText = attributeGameObject.transform.Find("BackValue/Value").GetComponent<TMP_Text>();
        buttonValueText.text = character.Attributes.First(item => item.Name == attributeName).Value.ToString();

        Button getButton = attributeGameObject.transform.Find("Selection").GetComponent<Button>();
        getButton.onClick.AddListener(() => GetSelected(character, attributeName));

        if (character.Name == SelectedGetClick)
        {
            attributeGameObject.transform.Find("Selection").GetComponent<Image>().color = new Color(0.5f, 1, 0.5f, 1);
        }
    }
    public void GetSelected(CharacterB character, string attributeName)
    {
        var currentCharacter = CharactersData.CharactersManager.CurrentCharacter;
        var oldAttribute = currentCharacter.Attributes.First(item => item.Name == attributeName);
        var newAttribute = CharactersData.CharactersManager.CharactersCollection.First(item => item.Name == character.Name).Attributes.First(item => item.Name == attributeName);

        CurrentSet.transform.Find("BackValue/Value").GetComponent<TMP_Text>().text = newAttribute.Value.ToString();

        var dependentMethods = UpdateMethods(currentCharacter, oldAttribute);
        dependentMethods.ForEach(method => method.Attribute = newAttribute);

        SelectedGetClick = character.Name;
        UpdateGetContent(attributeName);
        Powerup.ApplyPowerup(currentCharacter);
    }
    public List<Method> UpdateMethods(CharacterB character, Attribute modifyAttribute)
    {
        var dependentMethods = new List<Method>();
        dependentMethods.AddRange(character.Methods.Where(method => method.Attribute == modifyAttribute));
        dependentMethods.AddRange(character.Childrens.SelectMany(child => UpdateMethods(child, modifyAttribute)));

        return dependentMethods;
    }
    public void SetAttribute()
    {
        var input = InputField.text;
        string floatPattern = @"^[+-]?\d+(\.\d+)?$";
        if (!Regex.IsMatch(input, floatPattern)) return;

        var currentCharacter = CharactersData.CharactersManager.CurrentCharacter;
        var attributeSetter = SetCollection.First(item => item.Name == CurrentSet.name.Split(' ')[0]);
        var modifyAttribute = currentCharacter.Attributes.First(item => item.Name == attributeSetter.Name);

        modifyAttribute.Value = ToFloat(input);
        CurrentSet.transform.Find("BackValue/Value").GetComponent<TMP_Text>().text = modifyAttribute.Value.ToString();

        var dependentMethods = UpdateMethods(currentCharacter, modifyAttribute);
        dependentMethods.ForEach(method => method.Attribute = modifyAttribute);

        Powerup.ApplyPowerup(currentCharacter);
        InputField.text = "";

        UpdateGetContent(attributeSetter.Name);
    }
    public float ToFloat(string input)
    {
        int sign = input[0] == '-' ? -1 : 1;
        float result = 0;
        int tmp = 0;
        int j = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == '.')
                break;
            if (input[i] == '-')
                continue;
            result = result * 10 + (input[i] - '0');
        }
        if (input.IndexOf('.') == -1)
            return result *sign;

        for (int i = input.IndexOf('.') + 1; i < input.Length; i++)
        {
            tmp = tmp * 10 + (input[i] - '0');
            j++;
        }
        if (j > 0)
            result += (float)tmp / Mathf.Pow(10, j);
        return result * sign;
    }

    public bool Checker()
    {
        if (RestrictionManager.Instance.AllowEncapsulation is false) return false;
        if (!GameObject.Find("Player")) return false;
        if (CharactersData.CharactersManager.CurrentCharacter is null) return false;

        List<Attribute> setList = CharactersData.CharactersManager.CurrentCharacter.Attributes.Where(item => item.Setter).ToList();
        return setList.Count > 0;
    }

    public void ToggleOn()
    {
        SceneManagement.ScenePause("EncapsulationManager");

        LoadPopup();
        Popup.SetActive(true);
    }
    public void ToggleOff()
    {
        SceneManagement.SceneResume("EncapsulationManager");

        Popup.SetActive(false);
    }
    public void ToggleActivation()
    {
        if (Popup.activeSelf) ToggleOff();
        else ToggleOn();
    }
}