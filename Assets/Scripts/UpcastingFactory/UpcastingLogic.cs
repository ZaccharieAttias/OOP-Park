using System.Collections.Generic;
using UnityEngine;


public class UpcastingLogic : MonoBehaviour
{
    public CharacterManager CharacterManager;
    public UpcastingUI UpcastingUI;

    public List<(Character, List<CharacterMethod>)> CharacterData;
    public int CharacterIndex;
    public int MethodIndex;
    public int UpcastingQuantity;


    public void Start() 
    { 
        InitializeGameObject();
        InitializeProperties();     
    }

    private void InitializeGameObject() { gameObject.SetActive(false); }
    private void InitializeProperties()
    {
        CharacterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
        UpcastingUI = GameObject.Find("Canvas/GameplayScreen/Popups/UpcastingManager/UpcastingUI").GetComponent<UpcastingUI>();

        CharacterData = new List<(Character, List<CharacterMethod>)>();
        CharacterIndex = 0;
        MethodIndex = 0;
        UpcastingQuantity = 0;
    }

    public void LoadPopup(Character currentCharacter)
    {
        ClearContentPanel();
        
        SetCharacterUpcastable(currentCharacter);

        SetCharacter();
        SetMethod();
        SetQuantity();
    }
    public void SetCharacterUpcastable(Character character)
    {
        foreach (Character parent in character.Parents)
        {
            SetCharacterUpcastable(parent);

            List<CharacterMethod> parentMethods = new();
            parentMethods.AddRange(parent.Methods);
            
            if (parentMethods.Count > 0) CharacterData.Add((parent, parentMethods));
        }
    }
    public void SetCharacter()
    {
        string characterName = "None";
        if (CharacterData.Count > 0) characterName = CharacterData[CharacterIndex].Item1.Name;

        UpcastingUI.Character.text = characterName;
    }
    public void SetMethod()
    {
        string methodName = "None";
        if (CharacterData.Count > 0 ) methodName = CharacterData[CharacterIndex].Item2[MethodIndex].name;

        UpcastingUI.Method.text = methodName;
    }
    public void SetQuantity() { UpcastingUI.Quantity.text = UpcastingQuantity.ToString(); }
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
}
