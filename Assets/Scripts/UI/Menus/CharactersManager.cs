using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CharactersManager : MonoBehaviour
{
    [Header("Scripts")]
    public CharactersTreeManager CharactersTreeManager;
    public Powerup Powerup;

    [Header("UI Elements")]
    public GameObject Menu;
    public TMP_Text NameText;
    public TMP_Text DescriptionText;
    public Transform AttributesContentPanel;
    public Transform MethodsContentPanel;
    public Transform SpecialAbilityContentPanel;

    [Header("Buttons")]
    public GameObject DefaultButton;
    public GameObject DeleteButton;
    public GameObject GameplayButton;
    public GameObject GetButton;
    public GameObject SetButton;
    public GameObject AttributePlusButton;
    public GameObject MethodPlusButton;

    [Header("Character Data")]
    public List<CharacterB> CharactersCollection;
    public CharacterB CurrentCharacter;


    public void Start()
    {
        InitializeScripts();
        InitializeUIElements();
        InitializeButtons();
        InitializeCharacterData();
    }
    public void InitializeScripts()
    {
        CharactersTreeManager = GameObject.Find("Canvas/Menus").GetComponent<CharactersTreeManager>();
        Powerup = GameObject.Find("Scripts/PowerUp").GetComponent<Powerup>();
    }
    public void InitializeUIElements()
    {
        Menu = GameObject.Find("Canvas/Menus/CharacterCenter");
        NameText = Menu.transform.Find("Characters/Details/Name").GetComponent<TMP_Text>();
        DescriptionText = Menu.transform.Find("Characters/Details/Description/Text").GetComponent<TMP_Text>();

        AttributesContentPanel = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Attributes/Buttons/ScrollView/ViewPort/Content").transform;
        MethodsContentPanel = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Methods/Buttons/ScrollView/ViewPort/Content").transform;
        SpecialAbilityContentPanel = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/SpecialAbility/Buttons/ScrollView/ViewPort/Content").transform;
    }
    public void InitializeButtons()
    {
        DefaultButton = Resources.Load<GameObject>("Buttons/Default");

        DeleteButton = Menu.transform.Find("Characters/Details/Delete").gameObject;
        DeleteButton.GetComponent<Button>().onClick.AddListener(() => DeleteCharacter());

        GameplayButton = Menu.transform.Find("SwapScreen").gameObject;

        GetButton = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Description/G");
        SetButton = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Description/S");

        AttributePlusButton = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Attributes/Buttons/Edit");
        MethodPlusButton = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Methods/Buttons/Edit");
    }
    public void InitializeCharacterData()
    {
        CharactersCollection = new List<CharacterB>();
        CurrentCharacter = null;
    }

    public void AddCharacter(CharacterB builtCharacter)
    {
        CharactersCollection.Add(builtCharacter);
        CharactersTreeManager.BuildTree(CharactersCollection.First(), CharactersCollection.Last());

        DisplayCharacter(builtCharacter);
    }
    public void DeleteCharacter()
    {
        CurrentCharacter.Parent.Childrens.Remove(CurrentCharacter);
        CharactersCollection.Remove(CurrentCharacter);
        Destroy(CurrentCharacter.CharacterButton.Button);

        CharactersTreeManager.BuildTree(CharactersCollection.First(), CharactersCollection.Last());

        if (SceneManager.GetActiveScene().name == "OnlineBuilder") GameObject.Find("Canvas/Popups").GetComponent<CharacterSelectionManager>().CleanContent();
        DisplayCharacter(CharactersCollection.Last());
    }
    public void DisplayCharacter(CharacterB displayCharacter)
    {
        if (CurrentCharacter != null) DeselectPreviousCharacter();

        SelectCurrentCharacter(displayCharacter);
        CurrentCharacter = CharactersCollection.Find(character => character == displayCharacter);

        GameplayButton.GetComponent<Button>().interactable = !CurrentCharacter.IsAbstract;

        if (CurrentCharacter != null)
        {
            ClearContentPanels();

            DisplayName();
            DisplayAttributes();
            DisplayMethods();
            DisplayUpcastMethod();
            DisplaySpecialAbility();
            DisplayDelete();

            ApplyPowerup();

            GetButton.SetActive(false);
            SetButton.SetActive(false);

            if (SceneManager.GetActiveScene().name == "OnlineBuilder") GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Value").GetComponent<AttributeValueManager>().SetAttribute(null);
            ToggleEditButtons();
        }
    }
    public void DeselectPreviousCharacter()
    {
        GameObject previousCharacterObject = CurrentCharacter.CharacterButton.Button;
        previousCharacterObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }
    public void SelectCurrentCharacter(CharacterB displayCharacter)
    {
        GameObject currentCharacterObject = displayCharacter.CharacterButton.Button;
        currentCharacterObject.GetComponent<Image>().color = displayCharacter.IsAbstract ? new Color32(173, 216, 230, 255) : new Color32(255, 165, 0, 255);
    }
    public void ApplyPowerup()
    {
        Powerup.ApplyPowerup(CurrentCharacter);
    }
    public void ToggleEditButtons()
    {
        if (!CurrentCharacter.IsOriginal || (SceneManager.GetActiveScene().name == "OnlineBuilder" && !GameObject.Find("Scripts/PlayTestManager").GetComponent<PlayTestManager>().IsTestGameplay))
        {
            AttributePlusButton.SetActive(true);
            MethodPlusButton.SetActive(true);
        }

        else
        {
            AttributePlusButton.SetActive(false);
            MethodPlusButton.SetActive(false);
        }
    }
    public void DisplayName()
    {
        NameText.text = (CurrentCharacter.IsAbstract ? " {ABST} " : "") + CurrentCharacter.Name;
        DescriptionText.text = CurrentCharacter.Description;
    }
    public void DisplayAttributes()
    {
        foreach (Attribute attribute in CurrentCharacter.Attributes)
        {
            GameObject attributeGameObject = Instantiate(DefaultButton, AttributesContentPanel);
            attributeGameObject.AddComponent<DescriptionButton>();
            attributeGameObject.name = attribute.Name;

            TMP_Text buttonText = attributeGameObject.GetComponentInChildren<TMP_Text>();
            buttonText.text = attribute.Name;

            bool isAttributeOwner = CurrentCharacter.Name == attribute.Owner;
            if (RestrictionManager.Instance.AllowAccessModifier && isAttributeOwner) attributeGameObject.AddComponent<AccessModifierButton>().Attribute = attribute;
        }
    }
    public void DisplayMethods()
    {
        foreach (Method method in CurrentCharacter.Methods)
        {
            GameObject methodGameObject = Instantiate(DefaultButton, MethodsContentPanel);
            methodGameObject.AddComponent<DescriptionButton>();
            methodGameObject.name = method.Name;

            TMP_Text buttonText = methodGameObject.GetComponentInChildren<TMP_Text>();
            buttonText.text = method.Name;

            bool isMethodOwner = CurrentCharacter.Name == method.Owner;
            if (RestrictionManager.Instance.AllowAccessModifier && isMethodOwner) methodGameObject.AddComponent<AccessModifierButton>().Method = method;
        }
    }
    public void DisplayUpcastMethod()
    {
        if (MethodsContentPanel.childCount > CurrentCharacter.Methods.Count)
        {
            for (int i = 0; i < MethodsContentPanel.childCount; i++)
            {
                GameObject child = MethodsContentPanel.GetChild(i).gameObject;
                if (!CurrentCharacter.Methods.Any(method => method.Name == child.name))
                {
                    Destroy(child);
                    break;
                }
            }
        }

        if (CurrentCharacter.UpcastMethod != null)
        {
            GameObject upcastMethodGameObject = Instantiate(DefaultButton, MethodsContentPanel);

            upcastMethodGameObject.AddComponent<DescriptionButton>();
            upcastMethodGameObject.name = "UpcastMethod";

            Image upcastMethodGameObjectImage = upcastMethodGameObject.GetComponent<Image>();
            upcastMethodGameObjectImage.color = new Color32(128, 0, 128, 255);

            TMP_Text buttonText = upcastMethodGameObject.GetComponentInChildren<TMP_Text>();
            buttonText.text = CurrentCharacter.UpcastMethod.CharacterMethod.Name;
        }
    }
    public void DisplaySpecialAbility()
    {
        GameObject specialAbilityGameObject = Instantiate(DefaultButton, SpecialAbilityContentPanel);
        specialAbilityGameObject.AddComponent<DescriptionButton>();
        specialAbilityGameObject.name = CurrentCharacter.SpecialAbility?.Name;

        Image specialAbilityGameObjectImage = specialAbilityGameObject.GetComponent<Image>();
        specialAbilityGameObjectImage.color = new Color32(0, 255, 255, 255);

        TMP_Text buttonText = specialAbilityGameObject.GetComponentInChildren<TMP_Text>();
        buttonText.text = CurrentCharacter.SpecialAbility?.Name;
    }
    public void DisplayDelete()
    {
        DeleteButton.SetActive(!CurrentCharacter.IsOriginal && CurrentCharacter.IsLeaf());
    }
    public void ClearContentPanels()
    {
        foreach (Transform child in AttributesContentPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in MethodsContentPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in SpecialAbilityContentPanel)
        {
            Destroy(child.gameObject);
        }
    }
}