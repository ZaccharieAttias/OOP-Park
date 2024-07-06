using System.Collections.Generic;
using System.Linq;
using LootLocker.Extension.DataTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CharactersManager : MonoBehaviour
{
    public GameObject Menu;

    public List<CharacterB> CharactersCollection;
    public CharacterB CurrentCharacter;

    public TMP_Text NameText;
    public TMP_Text DescriptionText;

    public GameObject DefaultButton;
    public GameObject DeleteButton;
    public GameObject GameplayButton;

    public Transform AttributesContentPanel;
    public Transform MethodsContentPanel;
    public Transform SpecialAbilityContentPanel;

    public CharactersTreeManager CharactersTreeManager;

    public GameObject GetButton;
    public GameObject SetButton;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        Menu = GameObject.Find("Canvas/Menus/CharacterCenter");

        CharactersCollection = new List<CharacterB>();
        CurrentCharacter = null;

        NameText = Menu.transform.Find("Characters/Details/Name").GetComponent<TMP_Text>();
        DescriptionText = Menu.transform.Find("Characters/Details/Description/Text").GetComponent<TMP_Text>();

        DefaultButton = Resources.Load<GameObject>("Buttons/Default");
        DeleteButton = Menu.transform.Find("Characters/Details/Delete").gameObject;
        DeleteButton.GetComponent<Button>().onClick.AddListener(() => DeleteCharacter());
        GameplayButton = Menu.transform.Find("SwapScreen").gameObject;

        AttributesContentPanel = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Attributes/Buttons/ScrollView/ViewPort/Content").transform;
        MethodsContentPanel = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Methods/Buttons/ScrollView/ViewPort/Content").transform;
        SpecialAbilityContentPanel = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/SpecialAbility/Buttons/ScrollView/ViewPort/Content").transform;

        CharactersTreeManager = GameObject.Find("Canvas/Menus").GetComponent<CharactersTreeManager>();

        GetButton = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Description/G");
        SetButton = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Description/S");
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
        DisplayCharacter(CharactersCollection.Last());
    }

    public void DisplayCharacter(CharacterB displayCharacter)
    {
        if (CurrentCharacter is not null)
        {
            GameObject previousCharacterObject = CurrentCharacter.CharacterButton.Button;
            previousCharacterObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        GameObject currentCharacterObject = displayCharacter.CharacterButton.Button;
        currentCharacterObject.GetComponent<Image>().color = displayCharacter.IsAbstract ? new Color32(173, 216, 230, 255) : new Color32(255, 165, 0, 255);

        CurrentCharacter = CharactersCollection.Find(character => character == displayCharacter);
        GameplayButton.GetComponent<Button>().interactable = CurrentCharacter.IsAbstract is false;
        if (CurrentCharacter is not null)
        {
            ClearContentPanels();

            DisplayName();
            DisplayAttributes();
            DisplayMethods();
            DisplayUpcastMethod();
            DisplaySpecialAbility();
            DisplayDelete();

            Powerup powerUp = GameObject.Find("Scripts/PowerUp").GetComponent<Powerup>();
            powerUp.ApplyPowerup(CurrentCharacter);

            GetButton.SetActive(false);
            SetButton.SetActive(false);
        }
    }
    private void DisplayName()
    {
        NameText.text = (CurrentCharacter.IsAbstract ? " {ABST} " : "") + CurrentCharacter.Name;
        DescriptionText.text = CurrentCharacter.Description;
    }
    private void DisplayAttributes()
    {
        foreach (Attribute attribute in CurrentCharacter.Attributes)
        {
            GameObject attributeGameObject = Instantiate(DefaultButton, AttributesContentPanel);
            attributeGameObject.AddComponent<DescriptionButton>();

            attributeGameObject.name = attribute.Name;

            TMP_Text buttonText = attributeGameObject.GetComponentInChildren<TMP_Text>();
            buttonText.text = attribute.Name;

            bool isAttributeOwner = CurrentCharacter.Name == attribute.Owner;
            if (RestrictionManager.Instance.AllowAccessModifier && isAttributeOwner) { attributeGameObject.AddComponent<AccessModifierButton>().Attribute = attribute; }
        }
    }
    private void DisplayMethods()
    {
        foreach (Method method in CurrentCharacter.Methods)
        {
            GameObject methodGameObject = Instantiate(DefaultButton, MethodsContentPanel);
            methodGameObject.AddComponent<DescriptionButton>();
            methodGameObject.name = method.Name;

            TMP_Text buttonText = methodGameObject.GetComponentInChildren<TMP_Text>();
            buttonText.text = method.Name;

            bool isMethodOwner = CurrentCharacter.Name == method.Owner;
            if (RestrictionManager.Instance.AllowAccessModifier && isMethodOwner) { methodGameObject.AddComponent<AccessModifierButton>().Method = method; }
        }
    }
    public void DisplayUpcastMethod()
    {
        if (MethodsContentPanel.childCount > CurrentCharacter.Methods.Count)
        {
            for (int i = 0; i < MethodsContentPanel.childCount; i++)
            {
                GameObject child = MethodsContentPanel.GetChild(i).gameObject;
                if (CurrentCharacter.Methods.Any(method => method.Name == child.name) == false) { Destroy(child); break; }
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
    private void DisplaySpecialAbility()
    {
        GameObject specialAbilityGameObject = Instantiate(DefaultButton, SpecialAbilityContentPanel);
        specialAbilityGameObject.AddComponent<DescriptionButton>();
        specialAbilityGameObject.name = CurrentCharacter.SpecialAbility?.Name;

        Image specialAbilityGameObjectImage = specialAbilityGameObject.GetComponent<Image>();
        specialAbilityGameObjectImage.color = new Color32(0, 128, 128, 255);

        TMP_Text buttonText = specialAbilityGameObject.GetComponentInChildren<TMP_Text>();
        buttonText.text = CurrentCharacter.SpecialAbility?.Name;
    }
    private void DisplayDelete() { DeleteButton.SetActive(CurrentCharacter.IsOriginal == false && CurrentCharacter.IsLeaf()); }
    private void ClearContentPanels()
    {
        foreach (Transform child in AttributesContentPanel) Destroy(child.gameObject);
        foreach (Transform child in MethodsContentPanel) Destroy(child.gameObject);
        foreach (Transform child in SpecialAbilityContentPanel) Destroy(child.gameObject);
    }
}
