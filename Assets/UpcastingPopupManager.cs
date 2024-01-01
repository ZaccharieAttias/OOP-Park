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

    private int _characterIndex;
    private int _methodIndex;
    private int _quantityIndex;

    public Dictionary<Character, List<CharacterMethod>> _characterUpcastable =
        new Dictionary<Character, List<CharacterMethod>>();

    private Character _currentCharacter;

    private void Start()
    {
        _popup = GameObject.Find(_popupPath);
        Debug.Log(_popup);

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

        _characterIndex = 0;
        _methodIndex = 0;
        _quantityIndex = 0;

        _popup.SetActive(false);
    }

    public void Update()
    {
        _currentCharacter = GetComponent<CharacterManager>().currentCharacter;
        if (Input.GetKeyDown(KeyCode.U))
        {
            ShowUpcastingPopup(_currentCharacter);
        }
    }

    public void ShowUpcastingPopup(Character currentCharacter)
    {
        ClearUpcastingPopup();
        SetCharacterUpcastable(_currentCharacter);

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

            foreach (CharacterMethod method in parent.methods)
            {
                if (method.accessModifier == AccessModifier.Private)
                {
                    if (!_characterUpcastable.ContainsKey(parent))
                    {
                        _characterUpcastable.Add(parent, new List<CharacterMethod>());
                    }
                    _characterUpcastable[parent].Add(method);
                }
            }
        }
    }

    private void ClearUpcastingPopup()
    {
        _characterUpcastable.Clear();
    }

    private void ChangeCharacterTextRight()
    {
        if (_characterUpcastable.Count > 0)
        {
            int index = (_characterIndex + 1) % _characterUpcastable.Count;
            _currentCharacter = _characterUpcastable.Keys.ElementAt(index);

            _characterIndex = index;
            _methodIndex = 0;
            _quantityIndex = 0;

            SetCharacterText();
            SetMethodText();
            SetQuantityText();
        }
    }

    private void ChangeCharacterTextLeft()
    {
        if (_characterUpcastable.Count > 0)
        {
            int index = (_characterIndex - 1) % _characterUpcastable.Count;
            _currentCharacter = _characterUpcastable.Keys.ElementAt(index);

            _characterIndex = index;
            _methodIndex = 0;
            _quantityIndex = 0;

            SetCharacterText();
            SetMethodText();
            SetQuantityText();
        }
    }

    private void SetCharacterText()
    {
        Debug.Log(_currentCharacter.name);
        _characterText.GetComponent<TextMeshProUGUI>().text = _currentCharacter.name;
    }

    private void ChangeMethodTextRight()
    {
        if (_characterUpcastable.Count > 0)
        {
            int index = (_methodIndex + 1) % _characterUpcastable[_currentCharacter].Count;
            _methodIndex = index;

            SetMethodText();
            SetQuantityText();
        }
    }

    private void ChangeMethodTextLeft()
    {
        if (_characterUpcastable.Count > 0)
        {
            int index = (_methodIndex - 1) % _characterUpcastable[_currentCharacter].Count;
            _methodIndex = index;

            SetMethodText();
            SetQuantityText();
        }
    }

    private void SetMethodText()
    {
        _methodText.GetComponent<TextMeshProUGUI>().text = _characterUpcastable[_currentCharacter][
            _methodIndex
        ].name;
    }

    private void ChangeQuantityTextRight()
    {
        if (_characterUpcastable.Count > 0)
        {
            int index = _quantityIndex + 1;
            _quantityIndex = index;

            SetQuantityText();
        }
    }

    private void ChangeQuantityTextLeft()
    {
        if (_characterUpcastable.Count > 0)
        {
            int index = _quantityIndex - 1;
            _quantityIndex = index > 0 ? index : 0;

            SetQuantityText();
        }
    }

    private void SetQuantityText()
    {
        _quantityText.GetComponent<TextMeshProUGUI>().text = _quantityIndex.ToString();
    }
}
