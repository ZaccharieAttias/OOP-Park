using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MethodsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Popup;
    public Transform MethodsContentPanel;

    [Header("Buttons")]
    public GameObject MethodButton;
    public GameObject InheritanceButton;

    [Header("Method Data")]
    public List<Method> MethodsCollection;
    public List<GameObject> MethodGameObjects;
    public int MethodLimit;


    public void Start()
    {
        InitializeUIElements();
        InitializeButtons();
        InitializeMethodData();
        InitializeEventListeners();
    }
    public void InitializeUIElements()
    {
        Popup = GameObject.Find("Canvas/Popups/Methods");
        MethodsContentPanel = Popup.transform.Find("Background/Foreground/Buttons/ScrollView/ViewPort/Content");
    }
    public void InitializeButtons()
    {
        MethodButton = Resources.Load<GameObject>("Buttons/Default");
        InheritanceButton = GameObject.Find("Canvas/Popups/CharacterCreation");
    }
    public void InitializeMethodData()
    {
        MethodsCollection = new List<Method>();
        MethodGameObjects = new List<GameObject>();
        MethodLimit = 2;
    }
    public void InitializeEventListeners()
    {
        var popupToggleOn = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Methods/Buttons/Edit").GetComponent<Button>();
        popupToggleOn.onClick.AddListener(ToggleOn);

        var popupToggleOff = Popup.transform.Find("Background/Foreground/Buttons/Close").GetComponent<Button>();
        popupToggleOff.onClick.AddListener(ToggleOff);
    }

    public void LoadPopup()
    {
        if (MethodGameObjects.Count == 0) BuildMethodGameObjects();

        UpdateMethodGameObjects();
    }
    public void UpdateMethodGameObjects()
    {
        var currentCharacter = CharactersData.CharactersManager.CurrentCharacter;

        foreach (var methodGameObject in MethodGameObjects)
        {
            UpdateMethodGameObject(methodGameObject, currentCharacter);
        }
    }
    public void UpdateMethodGameObject(GameObject methodGameObject, CharacterB currentCharacter)
    {
        var methodExists = currentCharacter.Methods.Any(item => item.Name == methodGameObject.name);

        var image = methodGameObject.GetComponent<Image>();
        image.color = methodExists ? Color.green : Color.white;

        var methodButton = methodGameObject.GetComponent<Button>();
        methodButton.interactable = IsMethodSelectable(methodGameObject.name, currentCharacter, methodExists);
    }
    public bool IsMethodSelectable(string methodName, CharacterB currentCharacter, bool methodExists)
    {
        int methodCount = currentCharacter.Methods.Count;
        bool hasRequiredAttribute = HasRequiredAttribute(currentCharacter, methodName, RestrictionManager.Instance.AllowAccessModifier);

        return hasRequiredAttribute && (methodExists || methodCount < MethodLimit || RestrictionManager.Instance.OnlineBuild || currentCharacter.IsOriginal);
    }
    public void MarkMethod(GameObject methodGameObject, Method method)
    {
        var currentCharacter = CharactersData.CharactersManager.CurrentCharacter;
        var currentMethod = currentCharacter.Methods.FirstOrDefault(item => item.Name == method.Name);

        if (currentMethod == null) AddMethodToCharacter(currentCharacter, method);
        else RemoveMethodFromCharacter(currentCharacter, currentMethod);

        UpdateMethodColor(methodGameObject, currentMethod);
        LoadPopup();
    }
    public void AddMethodToCharacter(CharacterB currentCharacter, Method method)
    {
        var requiredAttribute = FindDependentAttribute(currentCharacter, method.Name, RestrictionManager.Instance.AllowAccessModifier);
        var newMethod = new Method(method, currentCharacter.Name, requiredAttribute);
        currentCharacter.Methods.Add(newMethod);
    }
    public void RemoveMethodFromCharacter(CharacterB currentCharacter, Method currentMethod)
    {
        if (currentCharacter.Name == currentMethod.Owner) CancelMethodReferences(currentCharacter, currentMethod);
        else currentCharacter.Methods.Remove(currentMethod);
    }
    public void UpdateMethodColor(GameObject methodGameObject, Method currentMethod)
    {
        var image = methodGameObject.GetComponent<Image>();
        image.color = currentMethod == null ? Color.green : Color.white;
    }
    public bool HasRequiredAttribute(CharacterB character, string methodName, bool allowAccessModifier)
    {
        var isCurrentCharacter = CharactersData.CharactersManager.CurrentCharacter == character;
        var hasRequiredAttribute = character.Attributes.Any(attribute =>
            attribute.Name.ToLower() == methodName.ToLower() &&
            (isCurrentCharacter || !allowAccessModifier || attribute.AccessModifier != AccessModifier.Private));

        if (!hasRequiredAttribute && character.Parent != null) hasRequiredAttribute = HasRequiredAttribute(character.Parent, methodName, allowAccessModifier);

        return hasRequiredAttribute;
    }
    public Attribute FindDependentAttribute(CharacterB character, string methodName, bool allowAccessModifier)
    {
        var isCurrentCharacter = CharactersData.CharactersManager.CurrentCharacter == character;
        var currentAttribute = character.Attributes.FirstOrDefault(attribute =>
            attribute.Name.ToLower() == methodName.ToLower() &&
            (isCurrentCharacter || !allowAccessModifier || attribute.AccessModifier != AccessModifier.Private));

        if (currentAttribute == null && character.Parent != null) currentAttribute = FindDependentAttribute(character.Parent, methodName, allowAccessModifier);

        return currentAttribute;
    }
    public void CancelMethodReferences(CharacterB character, Method method)
    {
        var referencedMethod = character.Methods.FirstOrDefault(item => item == method);
        if (referencedMethod != null) character.Methods.Remove(referencedMethod);

        foreach (var child in character.Childrens)
        {
            CancelMethodReferences(child, method);
        }
    }
    public void BuildMethodGameObjects()
    {
        foreach (var method in MethodsCollection)
        {
            CreateMethodGameObject(method);
        }
    }
    public void CreateMethodGameObject(Method method)
    {
        var methodGameObject = Instantiate(MethodButton, MethodsContentPanel);
        methodGameObject.name = method.Name;

        var methodButtonText = methodGameObject.GetComponentInChildren<TMP_Text>();
        methodButtonText.text = method.Name;

        var methodButton = methodGameObject.GetComponent<Button>();
        methodButton.onClick.AddListener(() => MarkMethod(methodGameObject, method));

        MethodGameObjects.Add(methodGameObject);
    }

    public void ToggleOn()
    {
        SceneManagement.ScenePause("MethodsManager");

        LoadPopup();
        Popup.SetActive(true);

        if (RestrictionManager.Instance.AllowSingleInheritance || RestrictionManager.Instance.OnlineBuild) InheritanceButton.SetActive(false);
    }
    public void ToggleOff()
    {
        SceneManagement.SceneResume("MethodsManager");

        CharactersData.CharactersManager.DisplayCharacter(CharactersData.CharactersManager.CurrentCharacter);
        Popup.SetActive(false);

        if (RestrictionManager.Instance.AllowSingleInheritance || RestrictionManager.Instance.OnlineBuild) InheritanceButton.SetActive(true);
    }
}