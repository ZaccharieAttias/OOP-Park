using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpcastingPopupManager : MonoBehaviour
{
    private readonly string _characterTextPath =
        "Canvas/GameplayScreen/Popups/Upcasting/Background/Foreground/Buttons/Character";
    private readonly string _methodTextPath =
        "Canvas/GameplayScreen/Popups/Upcasting/Background/Foreground/Buttons/Method";
    private readonly string _quantityTextPath =
        "Canvas/GameplayScreen/Popups/Upcasting/Background/Foreground/Buttons/Quantity";
    private readonly string _popupPath = "Canvas/GameplayScreen/Popups/Upcasting";

    private GameObject _popup;
    private GameObject _characterText;
    private GameObject _methodText;
    private GameObject _quantityText;

    private GameObject _characterTextRight;
    private GameObject _methodTextRight;
    private GameObject _quantityTextRight;

    private GameObject _characterTextLeft;
    private GameObject _methodTextLeft;
    private GameObject _quantityTextLeft;


    private List<(Character, List<CharacterMethod>)> characterData;
    private int currentCharacterIndex;
    private int currentMethodIndex;
    private int currentUpcastingQuantity;

    private void Start()
    {
        _popup = GameObject.Find(_popupPath);

        _characterTextRight = GameObject.Find(_characterTextPath + "/Right");
        _characterText = GameObject.Find(_characterTextPath + "/Text");
        _characterTextLeft = GameObject.Find(_characterTextPath + "/Left");
        _characterTextRight
            .GetComponent<Button>()
            .onClick.AddListener(() => ChangeCharacterTextRight());
        _characterTextLeft
            .GetComponent<Button>()
            .onClick.AddListener(() => ChangeCharacterTextLeft());

        _methodTextRight = GameObject.Find(_methodTextPath + "/Right");
        _methodText = GameObject.Find(_methodTextPath + "/Text");
        _methodTextLeft = GameObject.Find(_methodTextPath + "/Left");
        _methodTextRight.GetComponent<Button>().onClick.AddListener(() => ChangeMethodTextRight());
        _methodTextLeft.GetComponent<Button>().onClick.AddListener(() => ChangeMethodTextLeft());

        _quantityTextRight = GameObject.Find(_quantityTextPath + "/Right");
        _quantityText = GameObject.Find(_quantityTextPath + "/Text");
        _quantityTextLeft = GameObject.Find(_quantityTextPath + "/Left");
        _quantityTextRight
            .GetComponent<Button>()
            .onClick.AddListener(() => ChangeQuantityTextRight());
        _quantityTextLeft
            .GetComponent<Button>()
            .onClick.AddListener(() => ChangeQuantityTextLeft());


        characterData = new List<(Character, List<CharacterMethod>)>();
        currentCharacterIndex = 0;
        currentMethodIndex = 0;
        currentUpcastingQuantity = 0;

        _popup.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ShowUpcastingPopup(GetComponent<CharacterManager>().currentCharacter);
        }
    }

    public void ShowUpcastingPopup(Character currentCharacter)
    {
        ClearUpcastingPopup();
        SetCharacterUpcastable(currentCharacter);

        SetCharacterText();
        SetMethodText();
        SetQuantityText();

        _popup.SetActive(true);
    }

    public void SetCharacterUpcastable(Character currentCharacter)
    {
        foreach (Character parent in currentCharacter.parents)
        {
            SetCharacterUpcastable(parent);

            List<CharacterMethod> privateMethods = new List<CharacterMethod>();
            foreach (CharacterMethod method in parent.methods)
            {
                if (method.accessModifier == AccessModifier.Private)
                {
                    privateMethods.Add(method);
                }
            }

            if (privateMethods.Count > 0) characterData.Add((parent, privateMethods));
        }
    }

    private void ClearUpcastingPopup()
    {
        characterData.Clear();
        currentCharacterIndex = 0;
        currentMethodIndex = 0;
        currentUpcastingQuantity = 0;
    }

    private void ChangeCharacterTextRight()
    {
        currentCharacterIndex = (currentCharacterIndex + 1) % characterData.Count;
        currentMethodIndex = 0;
        currentUpcastingQuantity = 0;

        SetCharacterText();
        SetMethodText();
        SetQuantityText();
    }

    private void ChangeCharacterTextLeft()
    {
        currentCharacterIndex = (currentCharacterIndex - 1 + characterData.Count) % characterData.Count;
        currentMethodIndex = 0;
        currentUpcastingQuantity = 0;

        SetCharacterText();
        SetMethodText();
        SetQuantityText();
    }

    private void SetCharacterText()
    {
        string characterName = "Unknown";

        if (currentCharacterIndex >= 0 && currentCharacterIndex < characterData.Count)
        {
            var currentCharacter = characterData[currentCharacterIndex].Item1;
            characterName = currentCharacter.name;
        }

        _characterText.GetComponent<TextMeshProUGUI>().text = characterName;
    }

    private void ChangeMethodTextRight()
    {
        currentMethodIndex = (currentMethodIndex + 1) % characterData[currentCharacterIndex].Item2.Count;
        currentUpcastingQuantity = 0;

        SetMethodText();
        SetQuantityText();
    }

    private void ChangeMethodTextLeft()
    {
        currentMethodIndex = (currentMethodIndex - 1 + characterData[currentCharacterIndex].Item2.Count) % characterData[currentCharacterIndex].Item2.Count;
        currentUpcastingQuantity = 0;

        SetMethodText();
        SetQuantityText();
    }

    private void SetMethodText()
    {
        string methodName = "Unknown";

        if (currentCharacterIndex >= 0 && currentCharacterIndex < characterData.Count)
        {
            var currentCharacter = characterData[currentCharacterIndex].Item1;

            if (currentMethodIndex >= 0 && currentMethodIndex < characterData[currentCharacterIndex].Item2.Count)
            {
                var currentMethod = characterData[currentCharacterIndex].Item2[currentMethodIndex];
                methodName = currentMethod.name;
            }
        }

        _methodText.GetComponent<TextMeshProUGUI>().text = methodName;
    }

    private void ChangeQuantityTextRight()
    {
        currentUpcastingQuantity = currentUpcastingQuantity + 1;
        SetQuantityText();
    }

    private void ChangeQuantityTextLeft()
    {
        currentUpcastingQuantity = currentUpcastingQuantity - 1 > 0 ? currentUpcastingQuantity - 1 : 0;
        SetQuantityText();
    }

    private void SetQuantityText()
    {
        _quantityText.GetComponent<TextMeshProUGUI>().text = currentUpcastingQuantity.ToString();
    }
}
