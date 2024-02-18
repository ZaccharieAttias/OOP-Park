using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CharactersManager : MonoBehaviour
{
    public List<Character> CharactersCollection;
    public Character CurrentCharacter;

    public TMP_InputField NameText;
    public TMP_Text DescriptionText;

    public GameObject DefaultButton;
    public GameObject DeleteButton;

    public Transform AttributesContentPanel;
    public Transform MethodsContentPanel;
    public Transform SpecialAbilityContentPanel;

    public CharactersTreeManager CharactersTreeManager;
    public GameObject SwapScreen;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        CharactersCollection = new List<Character>();
        CurrentCharacter = null;

        NameText = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Name").GetComponent<TMP_InputField>();
        DescriptionText = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Description/Text").GetComponent<TMP_Text>();

        DefaultButton = Resources.Load<GameObject>("Buttons/Default");
        DeleteButton = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Delete");
        DeleteButton.GetComponent<Button>().onClick.AddListener(() => DeleteCharacter());

        AttributesContentPanel = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Attributes/Buttons/ScrollView/ViewPort/Content").transform;
        MethodsContentPanel = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Methods/Buttons/ScrollView/ViewPort/Content").transform;
        SpecialAbilityContentPanel = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/SpecialAbility/Buttons/ScrollView/ViewPort/Content").transform;

        CharactersTreeManager = GameObject.Find("Canvas/HTMenu").GetComponent<CharactersTreeManager>();

        SwapScreen = GameObject.Find("Canvas/HTMenu/Menu/SwapScreen");
    }

    public void AddCharacter(Character builtCharacter)
    {
        CharactersCollection.Add(builtCharacter);
        CharactersTreeManager.BuildTree(CharactersCollection.First(), CharactersCollection.Last());

        DisplayCharacter(builtCharacter);
    }
    public void DeleteCharacter()
    {
        Character parent = CurrentCharacter.Parents != null ? CurrentCharacter.Parents.First() : CharactersCollection.First();

        CurrentCharacter.Parents.ForEach(parent => parent.Childrens.Remove(CurrentCharacter));
        CharactersCollection.Remove(CurrentCharacter);
        Destroy(CurrentCharacter.CharacterButton.Button);

        CharactersTreeManager.BuildTree(CharactersCollection.First(), CharactersCollection.Last());
        DisplayCharacter(parent);
    }

    public void DisplayCharacter(Character displayCharacter)
    {
        if (CurrentCharacter != null)
        {
            GameObject previousCharacterObject = CurrentCharacter.CharacterButton.Button;
            previousCharacterObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        GameObject currentCharacterObject = displayCharacter.CharacterButton.Button;
        currentCharacterObject.GetComponent<Image>().color = new Color32(255, 165, 0, 255);

        CurrentCharacter = CharactersCollection.Find(character => character == displayCharacter);
        var CharacterPlayable = CurrentCharacter.IsAbstract == false;
        SwapScreen.GetComponent<Button>().interactable = CharacterPlayable;
        if (CurrentCharacter != null)
        {
            ClearContentPanels();

            DisplayName();
            DisplayAttributes();
            DisplayMethods();
            DisplayUpcastMethod();
            DisplaySpecialAbility();
            DisplayDelete();

            Powerup powerUp = GetComponent<Powerup>();
            powerUp.ApplyPowerup(CurrentCharacter);
        }
    }
    private void DisplayName()
    {
        NameText.interactable = CurrentCharacter.IsOriginal == false;

        string prefix = CurrentCharacter.IsAbstract ? "Abs " : "";
        NameText.text = prefix + CurrentCharacter.Name;
        DescriptionText.text = CurrentCharacter.Description;

        NameText.onEndEdit.AddListener(text =>
        {
            CurrentCharacter.Name = text;
            CurrentCharacter.CharacterButton.Button.name = text;

            DisplayCharacter(CurrentCharacter);
        });
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

            if (RestrictionManager.Instance.AllowAccessModifiers) { attributeGameObject.AddComponent<AccessModifierButton>().Attribute = attribute; }
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

            if (RestrictionManager.Instance.AllowAccessModifiers) { methodGameObject.AddComponent<AccessModifierButton>().Method = method; }
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
        specialAbilityGameObject.name = CurrentCharacter.SpecialAbility.Name;

        Image specialAbilityGameObjectImage = specialAbilityGameObject.GetComponent<Image>();
        specialAbilityGameObjectImage.color = new Color32(0, 128, 128, 255);

        TMP_Text buttonText = specialAbilityGameObject.GetComponentInChildren<TMP_Text>();
        buttonText.text = CurrentCharacter.SpecialAbility.Name;
    }
    private void DisplayDelete() { DeleteButton.SetActive(CurrentCharacter.IsOriginal == false && CurrentCharacter.IsLeaf()); }
    private void ClearContentPanels()
    {
        foreach (Transform child in AttributesContentPanel) Destroy(child.gameObject);
        foreach (Transform child in MethodsContentPanel) Destroy(child.gameObject);
        foreach (Transform child in SpecialAbilityContentPanel) Destroy(child.gameObject);
    }
}
