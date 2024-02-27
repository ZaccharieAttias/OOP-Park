using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MethodsManager : MonoBehaviour
{
    public GameObject Popup;
    public List<Method> MethodsCollection;
    
    public GameObject MethodButton;
    public Transform MethodsContentPanel;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/Methods");
        MethodsCollection = new List<Method>();

        MethodButton = Resources.Load<GameObject>("Buttons/Default");
        MethodsContentPanel = Popup.transform.Find("Background/Foreground/Buttons/ScrollView/ViewPort/Content").transform;

        Button popupToggleOn = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Methods/Buttons/Edit").GetComponent<Button>();
        popupToggleOn.onClick.AddListener(() => ToggleOn());

        Button popupToggleOff = Popup.transform.Find("Background/Foreground/Buttons/Close").GetComponent<Button>();
        popupToggleOff.onClick.AddListener(() => ToggleOff());
    }


    private void LoadPopup()
    {
        ClearContentPanel();

        foreach (Method method in MethodsCollection)
        {
            GameObject methodGameObject = Instantiate(MethodButton, MethodsContentPanel);
            methodGameObject.name = method.Name;

            TMP_Text methodButtonText = methodGameObject.GetComponentInChildren<TMP_Text>();
            methodButtonText.text = method.Name;

            Image image = methodGameObject.GetComponent<Image>();
            image.color = CharactersData.CharactersManager.CurrentCharacter.Methods.Any(item => item.Name == method.Name) ? Color.green : Color.white;

            Button methodButton = methodGameObject.GetComponent<Button>();
            methodButton.onClick.AddListener(() => MarkMethod(methodGameObject, method));
            methodButton.interactable = HasRequiredAttribute(CharactersData.CharactersManager.CurrentCharacter, method.Name, RestrictionManager.Instance.AllowAccessModifiers);
        }
    }
    private void ClearContentPanel() { foreach (Transform methodTransform in MethodsContentPanel) Destroy(methodTransform.gameObject); }
    private void MarkMethod(GameObject methodGameObject, Method method)
    {
        Character currentCharacter = CharactersData.CharactersManager.CurrentCharacter;
        var currentMethod = currentCharacter.Methods.FirstOrDefault(item => item.Name == method.Name); 

        if (currentMethod is null)
        {
            Attribute requiredAttribute = FindDependentAttribute(currentCharacter, method.Name, RestrictionManager.Instance.AllowAccessModifiers);
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
    private bool HasRequiredAttribute(Character character, string methodName, bool allowAccessModifiers)
    {
        bool isCurrentCharacter = CharactersData.CharactersManager.CurrentCharacter == character;
        bool hasRequiredAttribute = character.Attributes.Any(attribute => attribute.Name.ToLower() == methodName.ToLower() && (isCurrentCharacter || allowAccessModifiers is false || attribute.AccessModifier is not AccessModifier.Private));
        
        if (hasRequiredAttribute is false && character.Parents.Count > 0)
        {
            foreach (Character parent in character.Parents)
            {
                hasRequiredAttribute = HasRequiredAttribute(parent, methodName, allowAccessModifiers);
                if (hasRequiredAttribute) break;
            }
        }

        return hasRequiredAttribute;
    }
    private Attribute FindDependentAttribute(Character character, string methodName, bool allowAccessModifiers)
    {
        bool isCurrentCharacter = CharactersData.CharactersManager.CurrentCharacter == character;
        var currentAttribute = character.Attributes.FirstOrDefault(attribute => attribute.Name.ToLower() == methodName.ToLower() && (isCurrentCharacter || allowAccessModifiers is false || attribute.AccessModifier is not AccessModifier.Private));

        if (currentAttribute is null && character.Parents.Count > 0)
        {
            foreach (Character parent in character.Parents)
            {
                currentAttribute = FindDependentAttribute(parent, methodName, allowAccessModifiers);
                if (currentAttribute is not null) break;
            }
        }
        return currentAttribute;
    }
    public void CancelMethodReferences(Character character, Method method)
    {
        var referencedMethod = character.Methods.FirstOrDefault(item => item == method);
        if (referencedMethod is not null) character.Methods.Remove(referencedMethod);

        foreach (Character child in character.Childrens) CancelMethodReferences(child, method);
    }

    private void ToggleOn()
    { 
        LoadPopup(); 
        Popup.SetActive(true);
    }
    private void ToggleOff()
    { 
        CharactersData.CharactersManager.DisplayCharacter(CharactersData.CharactersManager.CurrentCharacter);
        Popup.SetActive(false); 
    }
}
