using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class EncapsulationManager : MonoBehaviour
{
    public GameObject AllPopup;
    public GameObject AllButton;
    public GameObject Button;
    public Transform ContentGetter;
    public Transform ContentSetter;


    public GameObject SetPopup;
    public Transform ContentSet;
    public Transform ContentGet;
    public GameObject GetterRow;
    public GameObject SetterRow;
    public List<(Attribute, List<Character>)> encapsulationGetters;
    List<Attribute> encapsulationSetters;



    public void Start() { InitializeProperties(); }

    public void Update() 
    { 
        if (Input.GetKeyDown(KeyCode.K) && RestrictionManager.Instance.AllowEncapsulation is true && GameObject.Find("Canvas/Menus/Gameplay").activeSelf is true)
            SetToggleOn();

    }
    private void InitializeProperties()
    {
        AllPopup = GameObject.Find("Canvas/Popups/GetterSetter");
        SetPopup = GameObject.Find("Canvas/Popups/Set");
        AllButton =  GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Description/A");
        Button = Resources.Load<GameObject>("Buttons/Default");

        ContentSet = GameObject.Find("Canvas/Popups/Set/Background/Foreground/ScrollViewSetters/Viewport/Content").GetComponent<Transform>();
        ContentGet = GameObject.Find("Canvas/Popups/Set/Background/Foreground/ScrollViewGetters/Viewport/Content").GetComponent<Transform>();
        GetterRow = Resources.Load<GameObject>("Buttons/GetterRow");
        SetterRow = Resources.Load<GameObject>("Buttons/SetterRow");

        ContentGetter = GameObject.Find("Canvas/Popups/GetterSetter/ALL/Background/Foreground/Buttons/ScrollViewGet/ViewPort/Content").GetComponent<Transform>();
        ContentSetter = GameObject.Find("Canvas/Popups/GetterSetter/ALL/Background/Foreground/Buttons/ScrollViewSet/ViewPort/Content").GetComponent<Transform>();

        if (RestrictionManager.Instance.AllowEncapsulation is true)
        {
            AllButton.GetComponent<Button>().onClick.AddListener(AllToggleOn);
            AllButton.SetActive(true);
        }
    }

    private void AllLoadPopup()
    {
        ClearContentPanel();
        var currentCharacter = CharactersData.CharactersManager.CurrentCharacter;
        List<Attribute> encapsulationGetters = currentCharacter.Attributes.Where(item => item.Getter is true).ToList();
        List<Attribute> encapsulationSetters = currentCharacter.Attributes.Where(item => item.Setter is true).ToList();
        
        Load(encapsulationGetters, "Getter");
        Load(encapsulationSetters, "Setter");

    }
    private void ClearContentPanel() 
    { 
        foreach (Transform attributeTransform in ContentGetter) Destroy(attributeTransform.gameObject); 
        foreach (Transform attributeTransform in ContentSetter) Destroy(attributeTransform.gameObject); 
    }
    public void Load(List<Attribute> encapsulationList, string type)
    {
        foreach (var attribute in encapsulationList)
        {
            GameObject attributeGameObject = Instantiate(Button, type == "Getter" ? ContentGetter : ContentSetter);
            attributeGameObject.name = attribute.Name + " " + type;
            
            TMP_Text ButtonText = attributeGameObject.GetComponentInChildren<TMP_Text>();
            ButtonText.text = attribute.Name;            
        }
    }




    private void SetLoadPopup()
    {
        SetClearContentPanel();
        var currentCharacter = CharactersData.CharactersManager.CurrentCharacter;
        encapsulationSetters = currentCharacter.Attributes.Where(item => item.Setter is true).ToList();
        encapsulationGetters = new ();
        
        foreach(var attribute in currentCharacter.Attributes)
            if (attribute.Setter && attribute.Getter)
                encapsulationGetters.Add((attribute, new List<Character> () {currentCharacter}));

        foreach(var parent in currentCharacter.Parents)
            if (encapsulationGetters.Count>0) GetAllGetters(parent, ref encapsulationGetters);
        
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
        var characters = encapsulationGetters.First(item => item.Item1.Name == attributeName).Item2;

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
        tmp = attribute;
        
        ContentSet.Find(attributeName + " Setter").Find("Value").GetComponent<TMP_Text>().text = attribute.Value.ToString();

        UpdateGetters(attributeName);
    }
    private void SetClearContentPanel() 
    { 
        foreach (Transform attributeTransform in ContentSet) Destroy(attributeTransform.gameObject); 
        foreach (Transform attributeTransform in ContentGet) Destroy(attributeTransform.gameObject); 
    }
    public void GetAllGetters(Character currentCharacter, ref List<(Attribute, List<Character>)> encapsulationGetters)
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
            GetAllGetters(parent, ref encapsulationGetters);
    }


    public void AllToggleOn()
    {
        AllLoadPopup();
        AllPopup.SetActive(true);
    }
    public void AllToggleOff()
    {
        AllPopup.SetActive(false);
    }
    public void SetToggleOn()
    {
        SetLoadPopup();
        SetPopup.SetActive(true);
    }
    public void SetToggleOff()
    {
        SetPopup.SetActive(false);
    }
}
