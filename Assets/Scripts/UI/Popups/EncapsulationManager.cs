using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;


public class EncapsulationManager : MonoBehaviour
{
    public GameObject Popup;

    public Transform SetContent;
    public Transform GetContent;

    public GameObject GetRowPrefab;
    public GameObject SetRowPrefab;

    public List<Attribute> SetCollection;
    public List<(Attribute, List<CharacterB>)> GetCollection;

    public TMP_InputField InputField;
    public GameObject SetButtonPrefab;

    public GameObject CurrentSet;
    public Powerup PowerUp;
    public string SelectedGetClick;
    public GameObject closeButton;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/Encapsulation");

        SetContent = Popup.transform.Find("Background/Foreground/Set/ScrollView/Viewport/Content").GetComponent<Transform>();
        GetContent = Popup.transform.Find("Background/Foreground/Get/ScrollView/Viewport/Content").GetComponent<Transform>();

        SetRowPrefab = Resources.Load<GameObject>("Buttons/SetRow");
        GetRowPrefab = Resources.Load<GameObject>("Buttons/GetRow");

        SetCollection = new List<Attribute>();
        GetCollection = new List<(Attribute, List<CharacterB>)>();

        InputField = Popup.transform.Find("Background/Foreground/Set/Input/InputField").GetComponent<TMP_InputField>();
        SetButtonPrefab = Popup.transform.Find("Background/Foreground/Set/Input/Button").gameObject;
        SetButtonPrefab.GetComponent<Button>().onClick.AddListener(SetAttribute);

        CurrentSet = null;
        SelectedGetClick = "";

        closeButton = GameObject.Find("Canvas/Popups/Encapsulation/Background/Foreground/Close");
        closeButton.GetComponent<Button>().onClick.AddListener(ToggleOff);
    }
    public bool Checker()
    {
        if (RestrictionManager.Instance.AllowEncapsulation is false) return false;
        if (!GameObject.Find("Player")) return false;
        else PowerUp = GameObject.Find("Scripts/PowerUp").GetComponent<Powerup>();
        if (CharactersData.CharactersManager.CurrentCharacter is null) return false;
        List<Attribute> SetList = CharactersData.CharactersManager.CurrentCharacter.Attributes.Where(item => item.Setter).ToList();
        if (SetList.Count > 0)
            return true;
        return false;
    }
    private void LoadPopup()
    {
        ClearContentPanel();

        var currentCharacter = CharactersData.CharactersManager.CurrentCharacter;
        SetCollection = currentCharacter.Attributes.Where(item => item.Setter).ToList();
        GetCollection = currentCharacter.Attributes
            .Where(attribute => attribute.Setter && attribute.Getter)
            .Select(attribute => (attribute, new List<CharacterB> { currentCharacter }))
            .ToList();

        FillGetCollection(currentCharacter.Parent);

        foreach (var attribute in SetCollection)
        {
            GameObject attributeGameObject = Instantiate(SetRowPrefab, SetContent);
            attributeGameObject.name = attribute.Name + " Set";

            TMP_Text ButtonText = attributeGameObject.transform.Find("Name").GetComponent<TMP_Text>();
            ButtonText.text = attribute.Name;

            TMP_Text ButtonText2 = attributeGameObject.transform.Find("Value").GetComponent<TMP_Text>();
            ButtonText2.text = attribute.Value.ToString();
        }

        CurrentSet = SetContent.GetChild(0).gameObject;
        UpdateGetContent(CurrentSet.name.Split(' ')[0]);
        SelectedGetClick = currentCharacter.Name;
    }
    private void ClearContentPanel()
    {
        SetContent.Cast<Transform>().ToList().ForEach(attributeTransform => Destroy(attributeTransform.gameObject));
        GetContent.Cast<Transform>().ToList().ForEach(attributeTransform => Destroy(attributeTransform.gameObject));
        SetContent.GetComponent<SwipeMenu>().Scroll_pos = 1;
        SetContent.GetComponent<SwipeMenu>().Scrollposition = 1;
    }
    public void FillGetCollection(CharacterB character)
    {
        if (character is null) return;
        foreach (var attribute in character.Attributes.Where(attr => attr.Getter && SetCollection.Any(item => item.Name == attr.Name)))
        {
            var attributeList = GetCollection.FirstOrDefault(item => item.Item1.Name == attribute.Name);
            if (attributeList.Item1 is not null && attributeList.Item2.Contains(character) is false)
            {
                attributeList.Item2.Add(character);
            }
            else
            {
                GetCollection.Add((attribute, new List<CharacterB> { character }));
            }
        }

        FillGetCollection(character.Parent);
    }
    public void UpdateGetContent(string attributeName)
    {
        GetContent.Cast<Transform>().ToList().ForEach(attributeTransform => Destroy(attributeTransform.gameObject));

        var currentCharacter = CharactersData.CharactersManager.CurrentCharacter;
        if (GetCollection.Count == 0 || GetCollection.Any(item => item.Item1.Name == attributeName) is false) return;

        var characters = GetCollection.FirstOrDefault(item => item.Item1.Name == attributeName).Item2;
        foreach (var character in characters)
        {
            GameObject attributeGameObject = Instantiate(GetRowPrefab, GetContent);
            attributeGameObject.name = character.Name + " Get";

            TMP_Text ButtonText = attributeGameObject.transform.Find("Name").GetComponent<TMP_Text>();
            ButtonText.text = character.Name;

            TMP_Text ButtonText2 = attributeGameObject.transform.Find("Value").GetComponent<TMP_Text>();
            ButtonText2.text = character.Attributes.First(item => item.Name == attributeName).Value.ToString();

            Button getButton = attributeGameObject.transform.Find("Selection").GetComponent<Button>();
            getButton.onClick.AddListener(() => GetSelected(character, attributeName, attributeGameObject));

            //marquer en vert l'attribut de l'objet courant
            if (character.Name == SelectedGetClick)
            {
                attributeGameObject.transform.Find("Selection").GetComponent<Image>().color = new Color(0.5f, 1, 0.5f, 1);
            }
        }
    }
    public void GetSelected(CharacterB character, string attributeName, GameObject SelectedGet)
    {
        var currentCharacter = CharactersData.CharactersManager.CurrentCharacter;
        var oldAttribute = currentCharacter.Attributes.First(item => item.Name == attributeName);
        var newAttribute = CharactersData.CharactersManager.CharactersCollection.First(item => item.Name == character.Name).Attributes.First(item => item.Name == attributeName);

        CurrentSet.transform.Find("Value").GetComponent<TMP_Text>().text = newAttribute.Value.ToString();

        var dependentMethods = UpdateMethods(currentCharacter, oldAttribute);
        dependentMethods.ForEach(method => method.Attribute = newAttribute);

        SelectedGetClick = character.Name;
        UpdateGetContent(attributeName);
        PowerUp.ApplyPowerup(currentCharacter);
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

        CurrentSet.transform.Find("Value").GetComponent<TMP_Text>().text = modifyAttribute.Value.ToString();

        var dependentMethods = UpdateMethods(currentCharacter, modifyAttribute);
        modifyAttribute.Value = ToFloat(input);
        dependentMethods.ForEach(method => method.Attribute = modifyAttribute);

        PowerUp.ApplyPowerup(currentCharacter);
        InputField.text = "";
        LoadPopup();
    }
    public float ToFloat(string input)
    {
        float result = 0;
        int i = 0, j = 0, tmp = 0;
        for (i = 0; i < input.Length; i++)
        {
            if (input[i] == '.')
                break;
            result = result * 10 + (input[i] - '0');
        }
        for (i = i + 1; i < input.Length; i++)
        {
            tmp = tmp * 10 + (input[i] - '0');
            j++;
        }
        if (j > 0)
            result += (float)tmp / Mathf.Pow(10, j);
        return result;
    }
    public void ToggleOn()
    {
        LoadPopup();
        Popup.SetActive(true);
        SceneManagement.ScenePause("EncapsulationManager");
    }
    public void ToggleOff()
    {
        Popup.SetActive(false);
        SceneManagement.SceneResume("EncapsulationManager");
    }
    public void ToggleActivation()
    {
        if (Popup.activeSelf)
            ToggleOff();
        else
            ToggleOn();
    }
}
