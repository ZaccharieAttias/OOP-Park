using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MethodsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Popup;
    public GameObject MethodButton;
    public Transform MethodsContentPanel;

    [Header("Method Data")]
    public List<Method> MethodsCollection;
    public List<GameObject> MethodGameObjects;


    public void Start()
    {
        InitializeProperties();
    }
    public void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/Methods");
        MethodButton = Resources.Load<GameObject>("Buttons/Default");
        MethodsContentPanel = Popup.transform.Find("Background/Foreground/Buttons/ScrollView/ViewPort/Content").transform;

        MethodsCollection = new List<Method>();
        MethodGameObjects = new();

        Button popupToggleOn = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Methods/Buttons/Edit").GetComponent<Button>();
        popupToggleOn.onClick.AddListener(() => ToggleOn());

        Button popupToggleOff = Popup.transform.Find("Background/Foreground/Buttons/Close").GetComponent<Button>();
        popupToggleOff.onClick.AddListener(() => ToggleOff());
    }

    public void LoadPopup()
    {
        if (MethodGameObjects.Count == 0)
        {
            BuildMethodGameObjects();
        }

        foreach (var methodGameObject in MethodGameObjects)
        {
            Image image = methodGameObject.GetComponent<Image>();
            image.color = CharactersData.CharactersManager.CurrentCharacter.Methods.Any(item => item.Name == methodGameObject.name) ? Color.green : Color.white;

            Button methodButton = methodGameObject.GetComponent<Button>();
            methodButton.interactable = HasRequiredAttribute(CharactersData.CharactersManager.CurrentCharacter, methodGameObject.name, RestrictionManager.Instance.AllowAccessModifier);
        }
    }
    public void MarkMethod(GameObject methodGameObject, Method method)
    {
        var currentCharacter = CharactersData.CharactersManager.CurrentCharacter;
        var currentMethod = currentCharacter.Methods.FirstOrDefault(item => item.Name == method.Name);

        if (currentMethod is null)
        {
            var requiredAttribute = FindDependentAttribute(currentCharacter, method.Name, RestrictionManager.Instance.AllowAccessModifier);
            Method newMethod = new(method, currentCharacter.Name, requiredAttribute);
            currentCharacter.Methods.Add(newMethod);
        }

        else
        {
            bool isMethodOwner = currentCharacter.Name == currentMethod.Owner;
            if (isMethodOwner) CancelMethodReferences(currentCharacter, currentMethod);
            else currentCharacter.Methods.Remove(currentMethod);
        }

        Image image = methodGameObject.GetComponent<Image>();
        image.color = currentMethod is null ? Color.green : Color.white;
    }
    
    public bool HasRequiredAttribute(CharacterB character, string methodName, bool allowAccessModifier)
    {
        bool isCurrentCharacter = CharactersData.CharactersManager.CurrentCharacter == character;
        bool hasRequiredAttribute = character.Attributes.Any(attribute => attribute.Name.ToLower() == methodName.ToLower() && (isCurrentCharacter || allowAccessModifier is false || attribute.AccessModifier is not AccessModifier.Private));

        if (hasRequiredAttribute is false && character.Parent is not null)
        {
            hasRequiredAttribute = HasRequiredAttribute(character.Parent, methodName, allowAccessModifier);
        }

        return hasRequiredAttribute;
    }
    public Attribute FindDependentAttribute(CharacterB character, string methodName, bool allowAccessModifier)
    {
        bool isCurrentCharacter = CharactersData.CharactersManager.CurrentCharacter == character;
        var currentAttribute = character.Attributes.FirstOrDefault(attribute => attribute.Name.ToLower() == methodName.ToLower() && (isCurrentCharacter || allowAccessModifier is false || attribute.AccessModifier is not AccessModifier.Private));

        if (currentAttribute is null && character.Parent is not null)
        {
            currentAttribute = FindDependentAttribute(character.Parent, methodName, allowAccessModifier);
        }

        return currentAttribute;
    }
    public void CancelMethodReferences(CharacterB character, Method method)
    {
        var referencedMethod = character.Methods.FirstOrDefault(item => item == method);
        if (referencedMethod is not null) character.Methods.Remove(referencedMethod);

        foreach (CharacterB child in character.Childrens) CancelMethodReferences(child, method);
    }
    public void BuildMethodGameObjects()
    {
        foreach (var method in MethodsCollection)
        {
            var methodGameObject = Instantiate(MethodButton, MethodsContentPanel);
            methodGameObject.name = method.Name;

            var methodButtonText = methodGameObject.GetComponentInChildren<TMP_Text>();
            methodButtonText.text = method.Name;

            var methodButton = methodGameObject.GetComponent<Button>();
            methodButton.onClick.AddListener(() => MarkMethod(methodGameObject, method));

            MethodGameObjects.Add(methodGameObject);
        }
    }

    public void ToggleOn()
    {
        LoadPopup();
        Popup.SetActive(true);
    }
    public void ToggleOff()
    {
        CharactersData.CharactersManager.DisplayCharacter(CharactersData.CharactersManager.CurrentCharacter);
        Popup.SetActive(false);
    }
}
