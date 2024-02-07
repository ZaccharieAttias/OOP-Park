using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UpcastingManager : MonoBehaviour
{
    public GameObject Popup;
    public List<(Character, List<CharacterMethod>)> CharacterData;

    public int CharacterIndex;
    public TextMeshProUGUI Character;
    public Button CharacterRight;
    public Button CharacterLeft;
    
    public int MethodIndex;
    public TextMeshProUGUI Method;
    public Button MethodRight;
    public Button MethodLeft;
    
    public int UpcastingQuantity;
    public TextMeshProUGUI Quantity;
    public Button QuantityRight;
    public Button QuantityLeft;
    
    public Button CloseButton;
    public Button ConfirmButton;

    public CharacterManager CharacterManager;


    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void OnGameStart()
    {
        UpcastingManager upcastingManager = GameObject.Find("Canvas/Popups").GetComponent<UpcastingManager>();
        upcastingManager.InitializeProperties();
    }
    private void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/Upcasting");
        CharacterData = new List<(Character, List<CharacterMethod>)>();

        CharacterIndex = 0;
        Character = Popup.transform.Find("Background/Foreground/Buttons/Character/Text").GetComponent<TextMeshProUGUI>();
        CharacterRight = Popup.transform.Find("Background/Foreground/Buttons/Character/Right").GetComponent<Button>();
        CharacterLeft = Popup.transform.Find("Background/Foreground/Buttons/Character/Left").GetComponent<Button>();
        CharacterRight.onClick.AddListener(() => CharacterRightArrowClicked());
        CharacterLeft.onClick.AddListener(() => CharacterLeftArrowClicked());

        MethodIndex = 0;
        Method = Popup.transform.Find("Background/Foreground/Buttons/Method/Text").GetComponent<TextMeshProUGUI>();
        MethodRight = Popup.transform.Find("Background/Foreground/Buttons/Method/Right").GetComponent<Button>();
        MethodLeft = Popup.transform.Find("Background/Foreground/Buttons/Method/Left").GetComponent<Button>();
        MethodRight.onClick.AddListener(() => MethodRightArrowClicked());
        MethodLeft.onClick.AddListener(() => MethodLeftArrowClicked());

        UpcastingQuantity = 0;
        Quantity = Popup.transform.Find("Background/Foreground/Buttons/Quantity/Text").GetComponent<TextMeshProUGUI>();
        QuantityRight = Popup.transform.Find("Background/Foreground/Buttons/Quantity/Right").GetComponent<Button>();
        QuantityLeft = Popup.transform.Find("Background/Foreground/Buttons/Quantity/Left").GetComponent<Button>();
        QuantityRight.onClick.AddListener(() => QuantityRightArrowClicked());
        QuantityLeft.onClick.AddListener(() => QuantityLeftArrowClicked());

        CloseButton = Popup.transform.Find("Background/Foreground/Buttons/Close").GetComponent<Button>();
        CloseButton.onClick.AddListener(() => ToggleOff());

        ConfirmButton = Popup.transform.Find("Background/Foreground/Buttons/Confirm").GetComponent<Button>();
        ConfirmButton.onClick.AddListener(() => Execute());

        CharacterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
    }
    
    public void Update() { if (RestrictionManager.Instance.AllowUpcasting && Input.GetKeyDown(KeyCode.U)) ToggleActivation(); }

    private void LoadPopup()
    {
        ClearContentPanel();
        
        SetCharacterUpcastable(CharacterManager.CurrentCharacter);
        SetCharacter();
        SetMethod();
        SetQuantity();
        SetButtonsInteractable();
    }
    private void SetCharacterUpcastable(Character character)
    {
        foreach (Character parent in character.Parents)
        {
            SetCharacterUpcastable(parent);

            List<CharacterMethod> parentMethods = new();
            parentMethods.AddRange(parent.Methods);
            
            if (parentMethods.Count > 0) CharacterData.Add((parent, parentMethods));
        }
    }
    private void SetCharacter()
    {
        string characterName = "None";
        if (CharacterData.Count > 0) characterName = CharacterData[CharacterIndex].Item1.Name;

        Character.text = characterName;
    }
    private void SetMethod()
    {
        string methodName = "None";
        if (CharacterData.Count > 0 ) methodName = CharacterData[CharacterIndex].Item2[MethodIndex].Name;

        Method.text = methodName;
    }
    private void SetQuantity() { Quantity.text = UpcastingQuantity.ToString(); }
    private void SetButtonsInteractable()
    {
        CharacterRight.interactable = CharacterData.Count > 1;
        CharacterLeft.interactable = CharacterData.Count > 1;
        
        MethodRight.interactable = CharacterData.Count > 0 && CharacterData[CharacterIndex].Item2.Count > 1;
        MethodLeft.interactable = CharacterData.Count > 0 && CharacterData[CharacterIndex].Item2.Count > 1;
        
        QuantityRight.interactable = CharacterData.Count > 0 && CharacterData[CharacterIndex].Item2.Count > 0;
        QuantityLeft.interactable = CharacterData.Count > 0 && CharacterData[CharacterIndex].Item2.Count > 0;

        ConfirmButton.interactable = CharacterData.Count > 0 && CharacterData[CharacterIndex].Item2.Count > 0;
    }
    public void ClearContentPanel()
    {
        CharacterData.Clear();

        CharacterIndex = 0;
        MethodIndex = 0;
        UpcastingQuantity = 0;
    }

    public void CharacterRightArrowClicked()
    {
        CharacterIndex = (CharacterIndex + 1) % CharacterData.Count;
        MethodIndex = 0;
        UpcastingQuantity = 0;

        SetCharacter();
        SetMethod();
        SetQuantity();
    }
    public void CharacterLeftArrowClicked()
    {
        CharacterIndex = (CharacterIndex - 1 + CharacterData.Count) % CharacterData.Count;
        MethodIndex = 0;
        UpcastingQuantity = 0;

        SetCharacter();
        SetMethod();
        SetQuantity();
    }
    
    public void MethodRightArrowClicked()
    {
        MethodIndex = (MethodIndex + 1) % CharacterData[CharacterIndex].Item2.Count;
        UpcastingQuantity = 0;

        SetMethod();
        SetQuantity();
    }
    public void MethodLeftArrowClicked()
    {
        MethodIndex = (MethodIndex - 1 + CharacterData[CharacterIndex].Item2.Count) % CharacterData[CharacterIndex].Item2.Count;
        UpcastingQuantity = 0;

        SetMethod();
        SetQuantity();
    }

    public void QuantityRightArrowClicked()
    {
        UpcastingQuantity++;
        
        SetQuantity();
    }
    public void QuantityLeftArrowClicked()
    {
        UpcastingQuantity = UpcastingQuantity - 1 > 0 ? UpcastingQuantity - 1 : 0;
        
        SetQuantity();
    }

    public void Execute()
    {
        Character character = CharacterManager.CurrentCharacter;
        CharacterMethod characterMethod = CharacterData[CharacterIndex].Item2[MethodIndex];
        CharacterUpcastMethod upcastMethod = new(character, characterMethod, UpcastingQuantity);

        character.UpcastMethod = upcastMethod;

        Powerup powerUp = GameObject.Find("Player").GetComponent<Powerup>();
        powerUp.ApplyPowerup(CharacterManager.CurrentCharacter);
        
        upcastMethod.ToggleOn();

        ToggleOff();
    }

    private void ToggleOn()
    { 
        LoadPopup();
        Popup.SetActive(true);    
    }
    private void ToggleOff()
    {
        CharacterManager.DisplayCharacter(CharacterManager.CurrentCharacter);
        Popup.SetActive(false); 
    }
    private void ToggleActivation() { if (Popup.activeSelf) ToggleOff(); else ToggleOn(); }
}
