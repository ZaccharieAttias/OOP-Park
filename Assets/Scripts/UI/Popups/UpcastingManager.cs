using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UpcastingManager : MonoBehaviour
{
    public GameObject Popup;
    public List<(Character, List<CharacterMethod>)> UpcastableData;

    public List<int> Indices;
    public List<TextMeshProUGUI> Texts;
    public List<Button> Buttons;

    public Button CloseButton;
    public Button ConfirmButton;

    public CharacterManager CharacterManager;

    
    private void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/Upcasting");
        UpcastableData = new List<(Character, List<CharacterMethod>)>();

        Indices = new List<int>
        { 
            0, 
            0, 
            0 
        };
        Texts = new List<TextMeshProUGUI>
        {
            Popup.transform.Find("Background/Foreground/Buttons/Character/Text").GetComponent<TextMeshProUGUI>(),
            Popup.transform.Find("Background/Foreground/Buttons/Method/Text").GetComponent<TextMeshProUGUI>(),
            Popup.transform.Find("Background/Foreground/Buttons/Amount/Text").GetComponent<TextMeshProUGUI>()
        };
        Buttons = new List<Button>()
        {
            Popup.transform.Find("Background/Foreground/Buttons/Character/Right").GetComponent<Button>(),
            Popup.transform.Find("Background/Foreground/Buttons/Character/Left").GetComponent<Button>(),
            Popup.transform.Find("Background/Foreground/Buttons/Method/Right").GetComponent<Button>(),
            Popup.transform.Find("Background/Foreground/Buttons/Method/Left").GetComponent<Button>(),
            Popup.transform.Find("Background/Foreground/Buttons/Amount/Right").GetComponent<Button>(),
            Popup.transform.Find("Background/Foreground/Buttons/Amount/Left").GetComponent<Button>()

        };

        Buttons[0].onClick.AddListener(() => UpdateCharacter(1));
        Buttons[1].onClick.AddListener(() => UpdateCharacter(-1));
        Buttons[2].onClick.AddListener(() => UpdateMethod(1));
        Buttons[3].onClick.AddListener(() => UpdateMethod(-1));
        Buttons[4].onClick.AddListener(() => UpdateAmount(1));
        Buttons[5].onClick.AddListener(() => UpdateAmount(-1));

        CloseButton = Popup.transform.Find("Background/Foreground/Buttons/Close").GetComponent<Button>();
        CloseButton.onClick.AddListener(() => ToggleOff());

        ConfirmButton = Popup.transform.Find("Background/Foreground/Buttons/Confirm").GetComponent<Button>();
        ConfirmButton.onClick.AddListener(() => ApplyUpcasting());

        CharacterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
    }
    
    public void Update() { if (Input.GetKeyDown(KeyCode.U)) ToggleActivation(); }

    private void LoadPopup()
    {
        ClearContentPanel();
        
        CollectUpcastableData(CharacterManager.CurrentCharacter);
        DisplayCharacter();
        DisplayMethod();
        DisplayAmount();

        DisplayButtonsInteractable();
    }
    private void CollectUpcastableData(Character character)
    {
        foreach (Character parent in character.Parents)
        {
            CollectUpcastableData(parent);

            List<CharacterMethod> parentMethods = parent.Methods
                .Where(method => RestrictionManager.Instance.AllowAccessModifiers == false || method.AccessModifier != AccessModifier.Private)
                .ToList();
            if (parentMethods.Count > 0) UpcastableData.Add((parent, parentMethods));
        }
    }
    private void DisplayCharacter()
    { 
        Texts[0].text = UpcastableData.Count > 0 ? UpcastableData[Indices[0]].Item1.Name : ""; 
    
        Buttons[0].interactable = UpcastableData.Count > 1;
        Buttons[1].interactable = UpcastableData.Count > 1;
    }
    private void DisplayMethod()
    {
        Texts[1].text = UpcastableData.Count > 0 ? UpcastableData[Indices[0]].Item2[Indices[1]].Name : ""; 

        Buttons[2].interactable = UpcastableData.Count > 0 && UpcastableData[Indices[0]].Item2.Count > 1;
        Buttons[3].interactable = UpcastableData.Count > 0 && UpcastableData[Indices[0]].Item2.Count > 1;    
    }
    private void DisplayAmount() 
    { 
        Texts[2].text = UpcastableData.Count > 0 ? Indices[2].ToString() : ""; 

        Buttons[4].interactable = UpcastableData.Count > 0;
        Buttons[5].interactable = UpcastableData.Count > 0;

        ConfirmButton.interactable = UpcastableData.Count > 0 && Indices[2] > 0;
    }
    private void DisplayButtonsInteractable()
    {
        Buttons[0].interactable = UpcastableData.Count > 1;
        Buttons[1].interactable = UpcastableData.Count > 1;
        Buttons[2].interactable = UpcastableData.Count > 0 && UpcastableData[Indices[0]].Item2.Count > 1;
        Buttons[3].interactable = UpcastableData.Count > 0 && UpcastableData[Indices[0]].Item2.Count > 1;    
        Buttons[4].interactable = UpcastableData.Count > 0;
        Buttons[5].interactable = UpcastableData.Count > 0;

        ConfirmButton.interactable = UpcastableData.Count > 0 && Indices[2] > 0;
    }
    private void ClearContentPanel()
    {
        UpcastableData.Clear();

        Indices.ForEach(index => index = 0);
    }

    private void UpdateCharacter(int direction)
    {
        Indices[0] = (Indices[0] + direction + UpcastableData.Count) % UpcastableData.Count;
        Indices[1] = 0;
        Indices[2] = 0;

        DisplayCharacter();
        DisplayMethod();
        DisplayAmount();
    }
    private void UpdateMethod(int direction)
    {
        Indices[1] = (Indices[1] + direction + UpcastableData[Indices[0]].Item2.Count) % UpcastableData[Indices[0]].Item2.Count;
        Indices[2] = 0;

        DisplayMethod();
        DisplayAmount();
    }
    private void UpdateAmount(int direction)
    {
        Indices[2] = Mathf.Clamp(Indices[2] + direction, 0, 300);

        DisplayAmount();        
    }

    private void ApplyUpcasting()
    {
        CharacterManager.CurrentCharacter.UpcastMethod = new CharacterUpcastMethod(CharacterManager.CurrentCharacter, UpcastableData[Indices[0]].Item2[Indices[1]], Indices[2]);
        CharacterManager.CurrentCharacter.UpcastMethod.UpcastTrackerManager.ToggleOn();

        ToggleOff();
    }

    public void ToggleOn()
    { 
        LoadPopup();
        Popup.SetActive(true);    
    }
    public void ToggleOff()
    {
        CharacterManager.DisplayCharacter(CharacterManager.CurrentCharacter);
        Popup.SetActive(false); 
    }
    private void ToggleActivation() { if (Popup.activeSelf) ToggleOff(); else ToggleOn(); }
}
