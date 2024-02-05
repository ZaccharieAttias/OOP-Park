using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CharacterManager : MonoBehaviour
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

    public CharacterTreeManager TreeBuilder;


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

        TreeBuilder = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/ScrollView").GetComponent<CharacterTreeManager>();
    }
   
    public void AddCharacter(Character builtCharacter)
    {
        CharactersCollection.Add(builtCharacter);
        TreeBuilder.BuildTree(CharactersCollection.First(), CharactersCollection.Last());

        DisplayCharacter(builtCharacter);
    }
    public void DeleteCharacter()
    {
        Character parent = CurrentCharacter.Parents != null ? CurrentCharacter.Parents.First() : CharactersCollection.First();
        
        CurrentCharacter.Parents.ForEach(parent => parent.Childrens.Remove(CurrentCharacter));
        CharactersCollection.Remove(CurrentCharacter);
        Destroy(CurrentCharacter.CharacterButton.Button);
        
        TreeBuilder.BuildTree(CharactersCollection.First(), CharactersCollection.Last());
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
        
        if (CurrentCharacter != null)
        {
            ClearContentPanels();

            DisplayName();
            DisplayAttributes();
            DisplayMethods();
            DisplaySpecialAbility();
            DisplayDelete();

            Powerup powerUp = GetComponent<Powerup>();
            powerUp.ApplyPowerup(CurrentCharacter);
        }
    }
    private void DisplayName()
    { 
        NameText.interactable = CurrentCharacter.IsOriginal == false;

        NameText.text = CurrentCharacter.Name;
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
        foreach (CharacterAttribute attribute in CurrentCharacter.Attributes)
        {
            GameObject attributeGameObject = Instantiate(DefaultButton, AttributesContentPanel);
            attributeGameObject.AddComponent<RightClickButton>();

            attributeGameObject.name = attribute.Name;

            TMP_Text buttonText = attributeGameObject.GetComponentInChildren<TMP_Text>();
            buttonText.text = attribute.Name;

            if (RestrictionManager.Instance.AllowAccessModifiers) 
            {
                attributeGameObject.AddComponent<AccessModifierManager>();
                attributeGameObject.GetComponent<AccessModifierManager>().Attribute = attribute;
            }
        }
    }
    private void DisplayMethods()
    {
        foreach (CharacterMethod method in CurrentCharacter.Methods)
        {
            GameObject methodGameObject = Instantiate(DefaultButton, MethodsContentPanel);
            methodGameObject.AddComponent<RightClickButton>();
            methodGameObject.name = method.Name;

            TMP_Text buttonText = methodGameObject.GetComponentInChildren<TMP_Text>();
            buttonText.text = method.Name;

            if (RestrictionManager.Instance.AllowAccessModifiers)
            {
                methodGameObject.AddComponent<AccessModifierManager>();
                methodGameObject.GetComponent<AccessModifierManager>().Method = method;
            }
        }
    }    
    private void DisplaySpecialAbility()
    {
        // Bad case: Creating a character, we initialized the special ability after...

        if (CurrentCharacter.SpecialAbility == null) return;
        
        GameObject specialAbilityGameObject = Instantiate(DefaultButton, SpecialAbilityContentPanel);
        specialAbilityGameObject.AddComponent<RightClickButton>();
        specialAbilityGameObject.name = CurrentCharacter.SpecialAbility.Name;
        
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
