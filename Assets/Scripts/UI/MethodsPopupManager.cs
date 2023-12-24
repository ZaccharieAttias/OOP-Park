using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;


public class MethodsPopupManager : MonoBehaviour
{
    private readonly string _buttonPrefabPath = "Prefabs/Buttons/Button";
    private readonly string _contentPanelPath = "Canvas/HTMenu/Popups/Methods/Background/Foreground/Buttons/ScrollView/ViewPort/Content";

    private GameObject _buttonPrefab;
    private Transform _contentPanel;
    private CharacterManager _characterManager;

    private List<CharacterMethod> _collection;
    private Character _currentCharacter;


    private void Start()
    {
        _buttonPrefab = Resources.Load<GameObject>(_buttonPrefabPath);
        _contentPanel = GameObject.Find(_contentPanelPath).transform;
        _characterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
        _collection = InitializeCollection();

        gameObject.SetActive(false);
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
        bool hasMethod = _currentCharacter.methods.Any(item => item.name == method.name);

        Image image = methodButton.GetComponent<Image>();
        image.color = hasMethod ? Color.green : Color.white;

        Button button = methodButton.GetComponent<Button>();
        button.onClick.AddListener(() => OnClick(method, hasMethod));
    }

    private void OnClick(CharacterMethod method, bool hasMethod)
    {
        if (hasMethod) _currentCharacter.methods.Remove(_currentCharacter.methods.Find(item => item.name == method.name));
        else _currentCharacter.methods.Add(new CharacterMethod(method.name, method.description, method.accessModifier));

        _characterManager.DisplayCharacterDetails(_currentCharacter.name); // Bad performances, Ideal to do that when closing the panel
        ShowMethodsPopup(_currentCharacter);
    }

    private void ClearContentPanel()
    {
        foreach (Transform child in _contentPanel) Destroy(child.gameObject);
    }

    private List<CharacterMethod> InitializeCollection()
    {
        List<CharacterMethod> collection = new List<CharacterMethod>();

        string methodName = "";
        string methodDescription = "";
        AccessModifier methodAccessModifier = AccessModifier.Public;

        // method1 
        methodName = "method 1";
        methodDescription = "This is the first method";
        methodAccessModifier = AccessModifier.Public;
        collection.Add(new CharacterMethod(methodName, methodDescription, methodAccessModifier));

        // method2
        methodName = "method 2";
        methodDescription = "This is the second method";
        methodAccessModifier = AccessModifier.Protected;
        collection.Add(new CharacterMethod(methodName, methodDescription, methodAccessModifier));

        // method3
        methodName = "method 3";
        methodDescription = "This is the third method";
        methodAccessModifier = AccessModifier.Private;
        collection.Add(new CharacterMethod(methodName, methodDescription, methodAccessModifier));

        return collection;
    }
}
