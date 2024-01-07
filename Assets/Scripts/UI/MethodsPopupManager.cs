using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MethodsPopupManager : MonoBehaviour
{
    private readonly string _buttonPrefabPath = "Prefabs/Buttons/Button";
    private readonly string _closeButtonPath = "Canvas/HTMenu/Popups/Methods/Background/Foreground/Buttons/Close";
    private readonly string _contentPanelPath = "Canvas/HTMenu/Popups/Methods/Background/Foreground/Buttons/ScrollView/ViewPort/Content";

    private CharacterManager _characterManager;
    private GameObject _buttonPrefab;
    private Transform _contentPanel;

    private List<CharacterMethod> _collection;
    private Character _currentCharacter;


    private void Start()
    {
        _characterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
        _buttonPrefab = Resources.Load<GameObject>(_buttonPrefabPath);
        _contentPanel = GameObject.Find(_contentPanelPath).transform;

        Button closeButton = GameObject.Find(_closeButtonPath).GetComponent<Button>();
        closeButton.onClick.AddListener(() => _characterManager.DisplayCharacterDetails(_currentCharacter.Name));

        _collection = InitializeCollection();

        gameObject.SetActive(false);
    }

    private List<CharacterMethod> InitializeCollection()
    {
        List<CharacterMethod> collection = new List<CharacterMethod>();

        string methodName = "";
        string methodDescription = "";
        AccessModifier methodAccessModifier = AccessModifier.Public;

        // method1
        methodName = "MoveSpeed";
        methodDescription = "This is the MoveSpeed method";
        methodAccessModifier = AccessModifier.Public;
        collection.Add(new CharacterMethod(methodName, methodDescription, methodAccessModifier));

        // method2
        methodName = "GravityForce";
        methodDescription = "This is the GravityForce method";
        methodAccessModifier = AccessModifier.Protected;
        collection.Add(new CharacterMethod(methodName, methodDescription, methodAccessModifier));

        // method3
        methodName = "DoubleJump";
        methodDescription = "This is the DoubleJump method";
        methodAccessModifier = AccessModifier.Private;
        collection.Add(new CharacterMethod(methodName, methodDescription, methodAccessModifier));

        return collection;
    }

    public void ShowMethodsPopup(Character currentCharacter)
    {
        ClearContentPanel();

        _currentCharacter = currentCharacter;

        foreach (CharacterMethod method in _collection)
        {
            GameObject methodButton = Instantiate(_buttonPrefab, _contentPanel);
            methodButton.name = method.name;

            TMP_Text buttonText = methodButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = method.name;

            MarkMethodInPopup(methodButton, method);
        }

        gameObject.SetActive(true);
    }

    private void MarkMethodInPopup(GameObject methodButton, CharacterMethod method)
    {
        bool hasAttribute = HasAttributeRecursively(_currentCharacter, method.name.ToLower());
        bool hasMethod = _currentCharacter.Methods.Any(item => item.name == method.name);

        if (hasAttribute == false)
        {
            methodButton.GetComponent<Button>().interactable = false;
            return;
        }

        Image image = methodButton.GetComponent<Image>();
        image.color = hasMethod ? Color.green : Color.white;

        Button button = methodButton.GetComponent<Button>();
        button.onClick.AddListener(() => OnClick(method, hasMethod));
    }

    private void OnClick(CharacterMethod method, bool hasMethod)
    {
        if (hasMethod) _currentCharacter.Methods.Remove(_currentCharacter.Methods.Find(item => item.name == method.name));
        else _currentCharacter.Methods.Add(new CharacterMethod(method.name, method.description, method.accessModifier));

        ShowMethodsPopup(_currentCharacter);
    }

    private void ClearContentPanel()
    {
        foreach (Transform child in _contentPanel)
            Destroy(child.gameObject);
    }

    private bool HasAttributeRecursively(Character character, string methodName)
    {
        if (character.Attributes.Any(attribute => attribute.name.ToLower() == methodName)) return true;

        foreach (Character parent in character.Parents)
            if (HasAttributeRecursively(parent, methodName))
                return true;

        return false;
    }
}
