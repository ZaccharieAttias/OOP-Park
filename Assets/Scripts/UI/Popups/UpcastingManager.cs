using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UpcastingManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Popup;
    public List<TextMeshProUGUI> Texts;

    [Header("Buttons")]
    public List<Button> Buttons;

    [Header("Upcasting Data")]
    public List<(CharacterB, List<Method>)> UpcastableData;
    public List<int> Indices;


    public void Start()
    {
        InitializeUIElements();
        InitializeButtons();
        InitializeUpcastingData();
    }
    public void InitializeUIElements()
    {
        Popup = GameObject.Find("Canvas/Popups/Upcasting");

        Texts = new List<TextMeshProUGUI>
        {
            Popup.transform.Find("Background/Foreground/Buttons/Character/Text").GetComponent<TextMeshProUGUI>(),
            Popup.transform.Find("Background/Foreground/Buttons/Method/Text").GetComponent<TextMeshProUGUI>(),
            Popup.transform.Find("Background/Foreground/Buttons/Amount/Text").GetComponent<TextMeshProUGUI>()
        };
    }
    public void InitializeButtons()
    {
        Buttons = new List<Button>
        {
            Popup.transform.Find("Background/Foreground/Buttons/Character/Right").GetComponent<Button>(),
            Popup.transform.Find("Background/Foreground/Buttons/Character/Left").GetComponent<Button>(),
            Popup.transform.Find("Background/Foreground/Buttons/Method/Right").GetComponent<Button>(),
            Popup.transform.Find("Background/Foreground/Buttons/Method/Left").GetComponent<Button>(),
            Popup.transform.Find("Background/Foreground/Buttons/Amount/Right").GetComponent<Button>(),
            Popup.transform.Find("Background/Foreground/Buttons/Amount/Left").GetComponent<Button>(),
            Popup.transform.Find("Background/Foreground/Buttons/Confirm").GetComponent<Button>()
        };

        Buttons[0].onClick.AddListener(() => UpdateCharacter(1));
        Buttons[1].onClick.AddListener(() => UpdateCharacter(-1));
        Buttons[2].onClick.AddListener(() => UpdateMethod(1));
        Buttons[3].onClick.AddListener(() => UpdateMethod(-1));
        Buttons[4].onClick.AddListener(() => UpdateAmount(1));
        Buttons[5].onClick.AddListener(() => UpdateAmount(-1));
        Buttons[6].onClick.AddListener(ApplyUpcasting);

        var closeButton = Popup.transform.Find("Background/Foreground/Buttons/Close").GetComponent<Button>();
        closeButton.onClick.AddListener(ToggleOff);
    }
    public void InitializeUpcastingData()
    {
        UpcastableData = new List<(CharacterB, List<Method>)>();
        Indices = new List<int> { 0, 0, 0 };
    }

    public void LoadPopup()
    {
        ClearContentPanel();

        CollectUpcastableData(CharactersData.CharactersManager.CurrentCharacter);
        DisplayCharacter();
        DisplayMethod();
        DisplayAmount();

        DisplayButtonsInteractable();
    }
    public void CollectUpcastableData(CharacterB character)
    {
        if (character == null || character.Parent == null) return;

        CollectUpcastableData(character.Parent);

        var parentMethods = character.Parent.Methods
            .Where(method => !RestrictionManager.Instance.AllowAccessModifier || method.AccessModifier != AccessModifier.Private)
            .Where(method => method.Name != "Appearance")
            .ToList();

        if (parentMethods.Any()) UpcastableData.Add((character.Parent, parentMethods));
    }
    public void ApplyUpcasting()
    {
        var currentCharacter = CharactersData.CharactersManager.CurrentCharacter;
        currentCharacter.UpcastMethod = new(UpcastableData[Indices[0]].Item2[Indices[1]], Indices[2]);
        currentCharacter.UpcastMethod.UpcastingTrackerManager.ToggleOn();

        ToggleOff();
    }
    public void ClearContentPanel()
    {
        UpcastableData.Clear();
        Indices = new List<int> { 0, 0, 0 };
    }
    public void DisplayCharacter()
    {
        Texts[0].text = UpcastableData.Count > 0 ? UpcastableData[Indices[0]].Item1.Name : "";

        Buttons[0].interactable = UpcastableData.Count > 1;
        Buttons[1].interactable = UpcastableData.Count > 1;
    }
    public void DisplayMethod()
    {
        Texts[1].text = UpcastableData.Count > 0 ? UpcastableData[Indices[0]].Item2[Indices[1]].Name : "";

        Buttons[2].interactable = UpcastableData.Count > 0 && UpcastableData[Indices[0]].Item2.Count > 1;
        Buttons[3].interactable = UpcastableData.Count > 0 && UpcastableData[Indices[0]].Item2.Count > 1;
    }
    public void DisplayAmount()
    {
        Texts[2].text = UpcastableData.Count > 0 ? Indices[2].ToString() : "";

        Buttons[4].interactable = UpcastableData.Count > 0;
        Buttons[5].interactable = UpcastableData.Count > 0;
        Buttons[6].interactable = UpcastableData.Count > 0 && Indices[2] > 0;
    }
    public void DisplayButtonsInteractable()
    {
        Buttons[0].interactable = UpcastableData.Count > 1;
        Buttons[1].interactable = UpcastableData.Count > 1;
        Buttons[2].interactable = UpcastableData.Count > 0 && UpcastableData[Indices[0]].Item2.Count > 1;
        Buttons[3].interactable = UpcastableData.Count > 0 && UpcastableData[Indices[0]].Item2.Count > 1;
        Buttons[4].interactable = UpcastableData.Count > 0;
        Buttons[5].interactable = UpcastableData.Count > 0;
        Buttons[6].interactable = UpcastableData.Count > 0 && Indices[2] > 0;
    }
    public void UpdateCharacter(int direction)
    {
        Indices[0] = (Indices[0] + direction + UpcastableData.Count) % UpcastableData.Count;
        Indices[1] = 0;
        Indices[2] = 0;

        DisplayCharacter();
        DisplayMethod();
        DisplayAmount();
    }
    public void UpdateMethod(int direction)
    {
        Indices[1] = (Indices[1] + direction + UpcastableData[Indices[0]].Item2.Count) % UpcastableData[Indices[0]].Item2.Count;
        Indices[2] = 0;

        DisplayMethod();
        DisplayAmount();
    }
    public void UpdateAmount(int direction)
    {
        Indices[2] = Mathf.Clamp(Indices[2] + direction, 0, 300);

        DisplayAmount();
    }

    public bool Checker()
    {
        CollectUpcastableData(CharactersData.CharactersManager.CurrentCharacter);
        return UpcastableData.Count > 0;
    }

    public void ToggleOn()
    {
        SceneManagement.ScenePause("UpcastingManager");

        LoadPopup();
        Popup.SetActive(true);
    }
    public void ToggleOff()
    {
        SceneManagement.SceneResume("UpcastingManager");

        CharactersData.CharactersManager.DisplayCharacter(CharactersData.CharactersManager.CurrentCharacter);
        Popup.SetActive(false);
    }
    public void ToggleActivation()
    {
        if (Popup.activeSelf) ToggleOff();
        else ToggleOn();
    }
}