using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;

public class EncapsulationManager : MonoBehaviour
{
    public GameObject SetPopup;
    public Transform ContentSet;
    public Transform ContentGet;
    public GameObject GetterRow;
    public GameObject SetterRow;
    public List<(Attribute, List<Character>)> encapsulationGetters;
    public List<Attribute> encapsulationSetters;
    public TMP_InputField inputField;
    public GameObject SetButton;
    public GameObject CurrentSetter;
    public Powerup PowerUp;

    public void Start() { InitializeProperties(); }
    public void Update() 
    { 
        if (Input.GetKeyDown(KeyCode.K) && RestrictionManager.Instance.AllowEncapsulation is true && GameObject.Find("Canvas/Menus/Gameplay").activeSelf is true)
            SetToggle();
    }
    private void InitializeProperties()
    {
        SetPopup = GameObject.Find("Canvas/Popups/Set");
        ContentSet = GameObject.Find("Canvas/Popups/Set/Background/Foreground/ScrollViewSetters/Viewport/Content").GetComponent<Transform>();
        ContentGet = GameObject.Find("Canvas/Popups/Set/Background/Foreground/ScrollViewGetters/Viewport/Content").GetComponent<Transform>();
        GetterRow = Resources.Load<GameObject>("Buttons/GetterRow");
        SetterRow = Resources.Load<GameObject>("Buttons/SetterRow");
        SetButton = GameObject.Find("Canvas/Popups/Set/Background/Foreground/SetValue/Button");
        inputField = GameObject.Find("Canvas/Popups/Set/Background/Foreground/SetValue/InputField").GetComponent<TMP_InputField>();
        PowerUp = GameObject.Find("Player").GetComponent<Powerup>();
    }
    private void LoadPopup()
    {
        ClearContentPanel();
        var currentCharacter = CharactersData.CharactersManager.CurrentCharacter;
        encapsulationSetters = currentCharacter.Attributes.Where(item => item.Setter is true).ToList();
        encapsulationGetters = new ();
        
        foreach(var attribute in currentCharacter.Attributes)
            if (attribute.Setter && attribute.Getter)
                encapsulationGetters.Add((attribute, new List<Character> () {currentCharacter}));

        foreach(var parent in currentCharacter.Parents)
            if (encapsulationGetters.Count>0) GetGetters(parent, ref encapsulationGetters);
        
        foreach(var attribute in encapsulationSetters)
        {
            GameObject attributeGameObject = Instantiate(SetterRow, ContentSet);
            attributeGameObject.name = attribute.Name + " Setter";

            TMP_Text ButtonText = attributeGameObject.transform.Find("Name").GetComponent<TMP_Text>();
            ButtonText.text = attribute.Name;

            TMP_Text ButtonText2 = attributeGameObject.transform.Find("Value").GetComponent<TMP_Text>();
            ButtonText2.text = attribute.Value.ToString();
        }
    }
    public void UpdateGetters(string attributeName)
    {
        foreach (Transform attributeTransform in ContentGet) Destroy(attributeTransform.gameObject);
        Character currentCharacter = CharactersData.CharactersManager.CurrentCharacter;
        if (encapsulationGetters.Count == 0) return;
        if (encapsulationGetters.Any(item => item.Item1.Name == attributeName) is false) return;
        var characters = encapsulationGetters.FirstOrDefault(item => item.Item1.Name == attributeName).Item2;
        
        foreach(var character in characters)
        {
            GameObject attributeGameObject = Instantiate(GetterRow, ContentGet);
            attributeGameObject.name = character.Name + " Getter";
            
            TMP_Text ButtonText = attributeGameObject.transform.Find("Name").GetComponent<TMP_Text>();
            ButtonText.text = character.Name;

            TMP_Text ButtonText2 = attributeGameObject.transform.Find("Value").GetComponent<TMP_Text>();
            ButtonText2.text = character.Attributes.First(item => item.Name == attributeName).Value.ToString();

            Button getButton = attributeGameObject.transform.Find("Selection").GetComponent<Button>();
            getButton.onClick.AddListener(() => Select(character, attributeName));
        }
    }
    public void Select(Character character, string attributeName)
    {
        var currentCharacter = CharactersData.CharactersManager.CurrentCharacter;
        var attribute = CharactersData.CharactersManager.CharactersCollection.First(item => item.Name == character.Name).Attributes.First(item => item.Name == attributeName);
        var tmp = currentCharacter.Attributes.First(item => item.Name == attributeName); 
        
        CurrentSetter.transform.Find("Value").GetComponent<TMP_Text>().text = attribute.Value.ToString();


        var updateMethod = new List<Method>();
        UpdateMethods(currentCharacter, tmp, ref updateMethod);
        foreach (var method in updateMethod)
        {
            method.Attribute = attribute;
        }
        PowerUp.ApplyPowerup(currentCharacter);
        UpdateGetters(attributeName);
    }
    private void ClearContentPanel() 
    { 
        foreach (Transform attributeTransform in ContentSet) Destroy(attributeTransform.gameObject); 
        foreach (Transform attributeTransform in ContentGet) Destroy(attributeTransform.gameObject); 
    }
    public void GetGetters(Character currentCharacter, ref List<(Attribute, List<Character>)> encapsulationGetters)
    {
        foreach(var attribute in currentCharacter.Attributes){
            if (attribute.Getter && encapsulationGetters.Any(item => item.Item1.Name == attribute.Name))
            {
                var attributeList = encapsulationGetters.First(item => item.Item1.Name == attribute.Name);
                if (!attributeList.Item2.Contains(currentCharacter))
                    attributeList.Item2.Add(currentCharacter);
            }
        }
        
        foreach(var parent in currentCharacter.Parents)
            GetGetters(parent, ref encapsulationGetters);
    }
    public void SetAttribute()
    {
        var input = inputField.text;
        string floatPattern = @"^[+-]?\d+(\.\d+)?$";
        if (!Regex.IsMatch(input, floatPattern)) return;

        var currentCharacter = CharactersData.CharactersManager.CurrentCharacter;
        var attributeSetter = encapsulationSetters.First(item => item.Name == CurrentSetter.name.Split(' ')[0]);
        var modifyAttribute = currentCharacter.Attributes.First(item => item.Name == attributeSetter.Name);

        CurrentSetter.transform.Find("Value").GetComponent<TMP_Text>().text = modifyAttribute.Value.ToString();

        //previous attribute value
        // find method and update the value of the attribute
        var updateMethod = new List<Method>();
        UpdateMethods(currentCharacter, modifyAttribute, ref updateMethod);
        modifyAttribute.Value = ToFloat(input);
        foreach (var method in updateMethod)
        {
            method.Attribute = modifyAttribute;
        }
        PowerUp.ApplyPowerup(currentCharacter);

        inputField.text = "";
    }
    public void UpdateMethods(Character character, Attribute modifyAttribute, ref List<Method> updateMethod)
    {
        foreach (var method in character.Methods)
        {
            if (method.Attribute == modifyAttribute)
            {
                updateMethod.Add(method);
            }
        }
        foreach (var child in character.Childrens)
        {
            UpdateMethods(child, modifyAttribute, ref updateMethod);
        }
    }
    public float ToFloat(string input)
    {
        float result =0;
        int i=0,j=0,tmp=0;
        for(i = 0; i < input.Length; i++)
        {
            if (input[i] == '.')
                break;
            result = result * 10 + (input[i] - '0');
        }
        for(i = i+1; i < input.Length; i++)
        {
            tmp = tmp * 10 + (input[i] - '0');
            j++;
        }
        if (j>0)
            result += (float)tmp / Mathf.Pow(10,j);
        return result;
    }
    public void ToggleOn()
    {
        LoadPopup();
        SetPopup.SetActive(true);
        SceneManagement.ScenePause();
    }
    public void ToggleOff()
    {
        SetPopup.SetActive(false);
        SceneManagement.SceneResume();
    }
    public void SetToggle()
    {
        if (SetPopup.activeSelf is true)
            ToggleOff();
        else
            ToggleOn();
    }
}
