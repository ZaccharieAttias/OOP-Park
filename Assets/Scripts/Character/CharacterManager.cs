using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CharacterManager : MonoBehaviour
{
    public List<Character> CharactersCollection;
    public Character CurrentCharacter;

    public TMP_Text CharacterNameText;
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

        CharacterNameText = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Title").GetComponent<TMP_Text>();
        DescriptionText = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Description/Text").GetComponent<TMP_Text>();
        DefaultButton = Resources.Load<GameObject>("Prefabs/Buttons/Button");
        CreateDeletionButton(); // CharacterDeleteButton

        AttributesContentPanel = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Attributes/Buttons/ScrollView/ViewPort/Content").transform;
        MethodsContentPanel = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details/Methods/Buttons/ScrollView/ViewPort/Content").transform;

        TreeBuilder = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/Scroll View").GetComponent<TreeBuilder>();
    }
    private void CreateDeletionButton()
    {
        Transform location = GameObject.Find("Canvas/HTMenu/Menu/Characters/Details").transform;

        CharacterDeleteButton = Instantiate(DefaultButton, location);
        CharacterDeleteButton.name = "Delete";

        TMP_Text buttonText = CharacterDeleteButton.GetComponentInChildren<TMP_Text>();
        buttonText.text = "Delete";

        CharacterDeleteButton.transform.localPosition = new Vector3(235, -195, 0);

        Button button1 = CharacterDeleteButton.GetComponent<Button>();
        button1.onClick.AddListener(() => DeleteCharacter());
    }
   
    public void AddCharacter(Character builtCharacter)
    {
        CharactersCollection.Add(builtCharacter);
        TreeBuilder.BuildTree();
        TreeBuilder.ScrollView.FocusOnItem(builtCharacter.CharacterButton.Button.GetComponent<RectTransform>());
        StartCoroutine(TreeBuilder.ScrollView.FocusOnItemCoroutine(builtCharacter.CharacterButton.Button.GetComponent<RectTransform>(), 1.0f));

        DisplayCharacterDetails(builtCharacter.Name);
    }
    public void DeleteCharacter()
    {
        CurrentCharacter.Parents.ForEach(parent => parent.Childrens.Remove(CurrentCharacter));
        CharactersCollection.Remove(CurrentCharacter);
        Character parent = null;
        if (CurrentCharacter.Parents != null) parent = CurrentCharacter.Parents.First();
        else parent = CharactersCollection.First();
        Destroy(CurrentCharacter.CharacterButton.Button);
        
        TreeBuilder.BuildTree();
        TreeBuilder.ScrollView.FocusOnItem(parent.CharacterButton.Button.GetComponent<RectTransform>());
        StartCoroutine(TreeBuilder.ScrollView.FocusOnItemCoroutine(parent.CharacterButton.Button.GetComponent<RectTransform>(), 1.0f));
        DisplayCharacterDetails(CharactersCollection.First().Name);
    }

    public void DisplayCharacterDetails(string characterName)
    {
        CurrentCharacter = CharactersCollection.Find(character => character.Name == characterName);

        if (CurrentCharacter != null)
        {
            ClearContentPanels();

            CharacterNameText.text = CurrentCharacter.Name;
            DescriptionText.text = CurrentCharacter.Description;

            DisplayAttributes();
            DisplayMethods();
            DisplayDelete();

            PowerUp powerUp = GetComponent<PowerUp>();
            powerUp.ApplyPowerup(CurrentCharacter);
        }
    }
    private void DisplayAttributes()
    {
        foreach (CharacterAttribute attribute in CurrentCharacter.Attributes)
        {
            GameObject attributeButton = Instantiate(DefaultButton, AttributesContentPanel);
            attributeButton.name = attribute.name;

            TMP_Text buttonText = attributeButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = attribute.name;

            if (RestrictionManager.Instance.AllowAccessModifiers) 
            {
                attributeButton.AddComponent<AccessModifierButton>();
                attributeButton.GetComponent<AccessModifierButton>().setAttribute(attribute);
            }
        }
    }
    private void DisplayMethods()
    {
        foreach (CharacterMethod method in CurrentCharacter.Methods)
        {
            GameObject methodButton = Instantiate(DefaultButton, MethodsContentPanel);

            TMP_Text buttonText = methodButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = method.name;

            if (RestrictionManager.Instance.AllowAccessModifiers)
            {
                methodButton.AddComponent<AccessModifierButton>();
                methodButton.GetComponent<AccessModifierButton>().setMethod(method);
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
