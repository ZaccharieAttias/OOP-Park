using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.HeroEditor.Common.Scripts.Collections;
using Assets.HeroEditor.InventorySystem.Scripts.Elements;
using Assets.HeroEditor4D.SimpleColorPicker.Scripts;
using HeroEditor.Common;
using System.IO;
using static Assets.HeroEditor.Common.Scripts.EditorScripts.CharacterEditor;


public class UpcastingManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Popup;
    public TextMeshProUGUI Text;
    public Image CharacterImage;

    [Header("Buttons")]
    public List<Button> Buttons;

    [Header("Upcasting Data")]
    public List<CharacterB> UpcastableData;
    public int Index;
    public CharacterBase Character;


    public void Start()
    {
        InitializeUIElements();
        InitializeButtons();
        InitializeUpcastingData();
        InitializeEventListeners();
    }
    public void InitializeUIElements()
    {
        Popup = GameObject.Find("Canvas/Popups/Upcasting");
        Text = Popup.transform.Find("Background/Foreground/Buttons/Character/Text").GetComponent<TextMeshProUGUI>();
        CharacterImage = Popup.transform.Find("Background/Foreground/Buttons/CharacterImage").GetComponent<Image>();
    }
    public void InitializeButtons()
    {
        Buttons = new List<Button>
        {
            Popup.transform.Find("Background/Foreground/Buttons/Character/Right").GetComponent<Button>(),
            Popup.transform.Find("Background/Foreground/Buttons/Character/Left").GetComponent<Button>(),
            Popup.transform.Find("Background/Foreground/Buttons/Confirm").GetComponent<Button>()
        };

        Buttons[0].onClick.AddListener(() => UpdateCharacter(1));
        Buttons[1].onClick.AddListener(() => UpdateCharacter(-1));
        Buttons[2].onClick.AddListener(ApplyUpcasting);
        Buttons[2].onClick.AddListener(() => GetComponent<CharacterChallengeManager>().ConfirmFactory());
    }
    public void InitializeUpcastingData()
    {
        UpcastableData = new List<CharacterB>();
        Index = 0;
        if (GameObject.Find("Player") != null)
        {
            Character = GameObject.Find("Player").GetComponent<CharacterBase>();
        }
    }
    public void InitializeEventListeners()
    {
        var closeButton = Popup.transform.Find("Background/Foreground/Buttons/Close").GetComponent<Button>();
        closeButton.onClick.AddListener(ToggleOff);
    }
    public void LoadPopup()
    {
        ClearContentPanel();

        CollectUpcastableData(CharactersData.CharactersManager.CurrentCharacter.Parent);
        DisplayCharacter();
    }
    public void CollectUpcastableData(CharacterB character)
    {
        if (character == null) return;

        CollectUpcastableData(character.Parent);
        
        UpcastableData.Add(character);
    }
    public void ApplyUpcasting()
    {
        var ReferenceCharacter = CharactersData.CharactersManager.CharactersCollection[Index].Name;
        string path = Path.Combine(Application.dataPath, "StreamingAssets", "Resources/CharactersData", "All", $"{ReferenceCharacter}.json");
        var json = File.ReadAllText(path);
        Character.FromJson(json);

        ToggleOff();
    }
    public void ClearContentPanel()
    {
        UpcastableData.Clear();
        Index = 0;
    }
    public void DisplayCharacter()
    {
        Text.text = UpcastableData[Index].Name;

        Sprite imagePath = Resources.Load<Sprite>("Sprites/Characters/" + UpcastableData[Index].Name);
        CharacterImage.sprite = imagePath;
    }
    public void UpdateCharacter(int direction)
    {
        Index = (Index + direction + UpcastableData.Count) % UpcastableData.Count;
        DisplayCharacter();
    }
    public bool Checker()
    {
        if (RestrictionManager.Instance.AllowUpcasting is false) return false;
        if (!GameObject.Find("Player")) return false;
        if (CharactersData.CharactersManager.CurrentCharacter is null) return false;

        ClearContentPanel();
        CollectUpcastableData(CharactersData.CharactersManager.CurrentCharacter.Parent);
        return UpcastableData.Count > 0;
    }
    public void ToggleOn()
    {
        SceneManagement.ScenePause("Upcasting");

        LoadPopup();
        Popup.SetActive(true);
    }
    public void ToggleOff()
    {
        SceneManagement.SceneResume("Upcasting");

        CharactersData.CharactersManager.DisplayCharacter(CharactersData.CharactersManager.CurrentCharacter);
        Popup.SetActive(false);
    }
    public void ToggleActivation()
    {
        if (Popup.activeSelf) ToggleOff();
        else ToggleOn();
    }
}