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
    public Transform ContentPanel;

    public CharactersManager CharactersManager;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/Methods");
        MethodsCollection = new List<Method>();

        MethodButton = Resources.Load<GameObject>("Buttons/Default");
        ContentPanel = Popup.transform.Find("Background/Foreground/Buttons/ScrollView/ViewPort/Content").transform;

        Button PopupToggleOn = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Methods/Buttons/Edit").GetComponent<Button>();
        PopupToggleOn.onClick.AddListener(() => ToggleOn());

        Button PopupToggleOff = Popup.transform.Find("Background/Foreground/Buttons/Close").GetComponent<Button>();
        PopupToggleOff.onClick.AddListener(() => ToggleOff());
        
        CharactersManager = GameObject.Find("Player").GetComponent<CharactersManager>();
    }


    private void LoadPopup()
    {
        ClearContentPanel();

        foreach (Method method in MethodsCollection)
        {
            GameObject methodGameObject = Instantiate(MethodButton, ContentPanel);
            methodGameObject.name = method.Name;

            TMP_Text methodButtonText = methodGameObject.GetComponentInChildren<TMP_Text>();
            methodButtonText.text = method.Name;

            Button methodButton = methodGameObject.GetComponent<Button>();
            methodButton.onClick.AddListener(() => MarkMethod(methodGameObject, method));
            methodButton.interactable = HasRequiredAttribute(CharactersManager.CurrentCharacter, method.Name.ToLower(), RestrictionManager.Instance.AllowAccessModifiers);
            
            Image image = methodGameObject.GetComponent<Image>();
            image.color = CharactersManager.CurrentCharacter.Methods.Any(item => item.Name == method.Name) ? Color.green : Color.white;
        }
    }
    private void ClearContentPanel() { foreach (Transform child in ContentPanel) Destroy(child.gameObject); }
    private void MarkMethod(GameObject methodGameObject, Method method)
    {
        var currentCharacter = CharactersManager.CurrentCharacter;
        var currentMethod = currentCharacter.Methods.Find(item => item.Name == method.Name); 

        if (currentMethod is not null)
            currentCharacter.Methods.Remove(currentMethod);

        else
        {
            Attribute requiredAttribute = FindDependentAttribute(currentCharacter, method.Name, RestrictionManager.Instance.AllowAccessModifiers);
            Method newMethod = new(method.Name, method.Description, requiredAttribute, method.AccessModifier);
            currentCharacter.Methods.Add(newMethod);
        }

        Image image = methodGameObject.GetComponent<Image>();
        image.color = currentMethod is null ? Color.green : Color.white;
    }
    private bool HasRequiredAttribute(Character character, string methodName, bool allowAccessModifiers)
    {
        if (character.Attributes.Any(attribute => attribute.Name.ToLower() == methodName && (CharactersManager.CurrentCharacter == character || allowAccessModifiers == false || attribute.AccessModifier != AccessModifier.Private))) return true;
        
        return character.Parents.Any(parent => HasRequiredAttribute(parent, methodName, allowAccessModifiers));
    }
    private Attribute FindDependentAttribute(Character character, string methodName, bool allowAccessModifiers)
    {
        var currentAttribute = character.Attributes.Find(attribute => attribute.Name.ToLower() == methodName.ToLower() && (CharactersManager.CurrentCharacter == character || allowAccessModifiers == false || attribute.AccessModifier != AccessModifier.Private));
        if (currentAttribute is not null) return currentAttribute;
        
        return character.Parents.Select(parent => FindDependentAttribute(parent, methodName, allowAccessModifiers)).FirstOrDefault(parentAttribute => parentAttribute is not null);
    }   

    private void ToggleOn()
    { 
        LoadPopup(); 
        Popup.SetActive(true);
    }
    private void ToggleOff()
    { 
        CharactersManager.DisplayCharacter(CharactersManager.CurrentCharacter);
        Popup.SetActive(false); 
    }
}
