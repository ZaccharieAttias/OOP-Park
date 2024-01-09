using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MethodsPopupManager : MonoBehaviour
{
    public List<CharacterMethod> MethodsCollection;
    public CharacterManager CharacterManager;
    
    public Button PopupToggleOn;
    public Button PopupToggleOff;
    
    public GameObject MethodButton;
    public Transform ContentPanel;


    private void Start()
    {
        InitializeGameObject();
        InitializeProperties();
    }

    private void InitializeGameObject() { gameObject.SetActive(false); }
    private void InitializeProperties()
    {
        CharacterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
        MethodsCollection = new List<CharacterMethod>();

        PopupToggleOn = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Methods/Buttons/Edit").GetComponent<Button>();
        PopupToggleOn.onClick.AddListener(() => ToggleOn());

        PopupToggleOff = GameObject.Find("Canvas/HTMenu/Popups/Methods/Background/Foreground/Buttons/Close").GetComponent<Button>();
        PopupToggleOff.onClick.AddListener(() => ToggleOff());

        MethodButton = Resources.Load<GameObject>("Prefabs/Buttons/Button");
        ContentPanel = GameObject.Find("Canvas/HTMenu/Popups/Methods/Background/Foreground/Buttons/ScrollView/ViewPort/Content").transform;
    }

    public void AddMethod(CharacterMethod method) { MethodsCollection.Add(method); }

    public void ShowMethodsPopup()
    {
        ClearContentPanel();

        foreach (CharacterMethod method in MethodsCollection)
        {
            GameObject methodButton = Instantiate(MethodButton, ContentPanel);
            methodButton.name = method.name;

            TMP_Text buttonText = methodButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = method.name;

            MarkMethodInPopup(methodButton, method);
        }
    }
    private void MarkMethodInPopup(GameObject methodButton, CharacterMethod method)
    {
        bool hasAttribute = HasAttribute(CharacterManager.CurrentCharacter, method.name.ToLower());
        bool hasMethod = CharacterManager.CurrentCharacter.Methods.Any(item => item.name == method.name);

        Image image = methodButton.GetComponent<Image>();
        image.color = hasMethod ? Color.green : Color.white;

        Button button = methodButton.GetComponent<Button>();
        button.onClick.AddListener(() => OnClick(method, hasMethod));
        button.interactable = hasAttribute;
    }
    private void OnClick(CharacterMethod method, bool hasMethod)
    {
        if (hasMethod) CharacterManager.CurrentCharacter.Methods.Remove(CharacterManager.CurrentCharacter.Methods.Find(item => item.name == method.name));
        else CharacterManager.CurrentCharacter.Methods.Add(new CharacterMethod(method.name, method.description, method.accessModifier));

        ShowMethodsPopup();
    }
    private void ClearContentPanel() { foreach (Transform child in ContentPanel) Destroy(child.gameObject); }

    private bool HasAttribute(Character character, string methodName)
    {
        if (character.Attributes.Any(attribute => attribute.name.ToLower() == methodName)) return true;

        foreach (Character parent in character.Parents)
            if (HasAttribute(parent, methodName))
                return true;

        return false;
    }

    private void ToggleOn() 
    { 
        ShowMethodsPopup(); 
        gameObject.SetActive(true);
    }
    private void ToggleOff() 
    { 
        CharacterManager.DisplayCharacterDetails(CharacterManager.CurrentCharacter.Name);
        gameObject.SetActive(false); 
    }
}
