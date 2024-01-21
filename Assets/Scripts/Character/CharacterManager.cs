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
    public GameObject CharacterDeleteButton;

    public Transform AttributesContentPanel;
    public Transform MethodsContentPanel;

    public TreeBuilder TreeBuilder;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        CharactersCollection = new List<Character>();
        CurrentCharacter = null;

        NameText = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Name").GetComponent<TMP_InputField>();
        DescriptionText = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Description/Text").GetComponent<TMP_Text>();
        
        DefaultButton = Resources.Load<GameObject>("Prefabs/Buttons/Default");
        CharacterDeleteButton = CreateDeletionButton();

        AttributesContentPanel = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Attributes/Buttons/ScrollView/ViewPort/Content").transform;
        MethodsContentPanel = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Methods/Buttons/ScrollView/ViewPort/Content").transform;

        TreeBuilder = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/ScrollView").GetComponent<TreeBuilder>();
    }
    private GameObject CreateDeletionButton()
    {
        Transform location = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details").transform;

        GameObject characterDeleteButton = Instantiate(DefaultButton, location);
        characterDeleteButton.name = "Delete";

        TMP_Text buttonText = characterDeleteButton.GetComponentInChildren<TMP_Text>();
        buttonText.text = "Delete";

        characterDeleteButton.transform.localPosition = new Vector3(235, -195, 0);

        Button button1 = characterDeleteButton.GetComponent<Button>();
        button1.onClick.AddListener(() => DeleteCharacter());

        return characterDeleteButton;
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
        CurrentCharacter = CharactersCollection.Find(character => character == displayCharacter);

        if (CurrentCharacter != null)
        {
            ClearContentPanels();

            DisplayName();
            DisplayAttributes();
            DisplayMethods();
            DisplayDelete();

            PowerUp powerUp = GetComponent<PowerUp>();
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
            GameObject attributeButton = Instantiate(DefaultButton, AttributesContentPanel);
            attributeButton.name = attribute.Name;

            TMP_Text buttonText = attributeButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = attribute.Name;

            if (RestrictionManager.Instance.AllowAccessModifiers) 
            {
                attributeButton.AddComponent<AccessModifierButton>();
                attributeButton.GetComponent<AccessModifierButton>().Attribute = attribute;
            }
        }
    }
    private void DisplayMethods()
    {
        foreach (CharacterMethod method in CurrentCharacter.Methods)
        {
            GameObject methodButton = Instantiate(DefaultButton, MethodsContentPanel);

            TMP_Text buttonText = methodButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = method.Name;

            if (RestrictionManager.Instance.AllowAccessModifiers)
            {
                methodButton.AddComponent<AccessModifierButton>();
                methodButton.GetComponent<AccessModifierButton>().Method = method;
            }
        }
    }    
    private void DisplayDelete() { CharacterDeleteButton.SetActive(CurrentCharacter.IsOriginal == false && CurrentCharacter.IsLeaf()); }
    private void ClearContentPanels()
    {
        foreach (Transform child in AttributesContentPanel) Destroy(child.gameObject);
        foreach (Transform child in MethodsContentPanel) Destroy(child.gameObject);
    }
}
