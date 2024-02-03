using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MethodsManager : MonoBehaviour
{
    public GameObject Popup;
    public List<CharacterMethod> MethodsCollection;
    
    public GameObject MethodButton;
    public Transform ContentPanel;

    public Button PopupToggleOn;
    public Button PopupToggleOff;

    public CharacterManager CharacterManager;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void OnGameStart()
    {
        MethodsManager methodsManager = GameObject.Find("Canvas/Popups").GetComponent<MethodsManager>();
        methodsManager.InitializeProperties();
    }
    private void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/Methods");
        MethodsCollection = new List<CharacterMethod>();

        MethodButton = Resources.Load<GameObject>("Buttons/Default");
        ContentPanel = Popup.transform.Find("Background/Foreground/Buttons/ScrollView/ViewPort/Content").transform;

        PopupToggleOn = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Methods/Buttons/Edit").GetComponent<Button>();
        PopupToggleOn.onClick.AddListener(() => ToggleOn());

        PopupToggleOff = Popup.transform.Find("Background/Foreground/Buttons/Close").GetComponent<Button>();
        PopupToggleOff.onClick.AddListener(() => ToggleOff());
        
        CharacterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
    }

    public void AddMethod(CharacterMethod method) { MethodsCollection.Add(method); }

    public void LoadPopup()
    {
        ClearContentPanel();

        foreach (CharacterMethod method in MethodsCollection)
        {
            GameObject methodButton = Instantiate(MethodButton, ContentPanel);
            methodButton.name = method.Name;

            TMP_Text buttonText = methodButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = method.Name;

            MarkMethodInPopup(methodButton, method);
        }
    }
    private void MarkMethodInPopup(GameObject methodButton, CharacterMethod method)
    {
        bool hasAttribute = CheckAttribute(CharacterManager.CurrentCharacter, method.Name.ToLower(), RestrictionManager.Instance.AllowAccessModifiers);
        bool hasMethod = CharacterManager.CurrentCharacter.Methods.Any(item => item.Name == method.Name);

        Image image = methodButton.GetComponent<Image>();
        image.color = hasMethod ? Color.green : Color.white;

        Button button = methodButton.GetComponent<Button>();
        button.onClick.AddListener(() => OnClick(method, hasMethod));
        button.interactable = hasAttribute;
    }
    private bool CheckAttribute(Character character, string methodName, bool allowAccessModifiers)
    {
        if (character.Attributes.Any(attribute => attribute.Name.ToLower() == methodName && (CharacterManager.CurrentCharacter == character || allowAccessModifiers == false || attribute.AccessModifier != AccessModifier.Private))) return true;
        return character.Parents.Any(parent => CheckAttribute(parent, methodName, allowAccessModifiers));
    }
    private void OnClick(CharacterMethod method, bool hasMethod)
    {
        Character currentCharacter = CharacterManager.CurrentCharacter;

        if (hasMethod) 
            currentCharacter.Methods.Remove(currentCharacter.Methods.Find(item => item.Name.ToLower() == method.Name.ToLower()));
        
        else
        {
            currentCharacter.Methods.Add(new CharacterMethod(method.Name, method.Description, method.AccessModifier));            
            currentCharacter.Methods.Last().Attribute = FindDependentAttribute(CharacterManager.CurrentCharacter, method.Name, RestrictionManager.Instance.AllowAccessModifiers);
        }

        LoadPopup();
    }
    private CharacterAttribute FindDependentAttribute(Character character, string methodName, bool allowAccessModifiers)
    {
        var currentAttribute = character.Attributes.Find(attribute => attribute.Name.ToLower() == methodName.ToLower() && (CharacterManager.CurrentCharacter == character || allowAccessModifiers == false || attribute.AccessModifier != AccessModifier.Private));
        if (currentAttribute != null) return currentAttribute;
        return character.Parents.Select(parent => FindDependentAttribute(parent, methodName, allowAccessModifiers)).FirstOrDefault(parentAttribute => parentAttribute != null);
    }   
    private void ClearContentPanel() { foreach (Transform child in ContentPanel) Destroy(child.gameObject); }

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
}
