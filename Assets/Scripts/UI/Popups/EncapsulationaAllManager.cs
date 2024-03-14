using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;

public class EncapsulationaAllManager : MonoBehaviour
{
    public GameObject Popup;
    public GameObject Button;
    public Transform ContentGetter;
    public Transform ContentSetter;

    public void Start() { InitializeProperties(); }

    private void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/GetterSetter");
        Button = Resources.Load<GameObject>("Buttons/Default");
        ContentGetter = GameObject.Find("Canvas/Popups/GetterSetter/ALL/Background/Foreground/Buttons/ScrollViewGet/ViewPort/Content").GetComponent<Transform>();
        ContentSetter = GameObject.Find("Canvas/Popups/GetterSetter/ALL/Background/Foreground/Buttons/ScrollViewSet/ViewPort/Content").GetComponent<Transform>();

        if (RestrictionManager.Instance.AllowEncapsulation is true)
        {
            GameObject All =  GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Description/A");
            All.GetComponent<Button>().onClick.AddListener(ToggleOn);
            All.SetActive(true);
        }
    }
    private void LoadPopup()
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
    public void ToggleOn()
    {
        LoadPopup();
        Popup.SetActive(true);
    }
    public void ToggleOff()
    {
        Popup.SetActive(false);
    }
}
