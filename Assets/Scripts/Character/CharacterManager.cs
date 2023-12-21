using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;
using System.Collections.Generic;


public class CharacterManager : MonoBehaviour
{
    public List<Character> _charactersCollection;
    public Character currentCharacter;

    public TMP_Text characterNameText;
    public TMP_Text descriptionText;

    public Transform attributesPanel;
    public Transform methodsPanel;

    public GameObject attributesPopup;
    public GameObject methodsPopup;
    public GameObject buttonPrefab;

    public GameObject CharacterTree;
    public ButtonTreeManager buttonTreeManager;

    public void Start()
    {
        CharacterTree = GetComponent<SearchGameObject>().FindGameObject(transform.parent, "TreePanel").gameObject;

        _charactersCollection = new List<Character>();
        CreateCharacters(); // Temporary
        
    }

    public void DisplayCharacterDetails(string characterName)
    {   
        currentCharacter = FindCharacterByName(characterName);

        if (currentCharacter != null)
        {
            CleanPanel();

            characterNameText.text = currentCharacter.name;
            descriptionText.text = currentCharacter.description;
            ChangingGameObjectName();

            DisplayAttributes();
            DisplayMethods();
            PowerUp powerUp = GetComponent<PowerUp>();
            powerUp.ApplyPowerup(currentCharacter);
        }
    }

    private void DisplayAttributes()
    {
        foreach (CharacterAttribute attribute in currentCharacter.attributes)
        {
            GameObject attributeButton = Instantiate(buttonPrefab, attributesPanel);
            attributeButton.name = attribute.name;

            TMP_Text buttonText = attributeButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = attribute.name;

            AccessModifierButton accessModifierButton = attributeButton.GetComponent<AccessModifierButton>();
            accessModifierButton.associatedAttribute = attribute;
        }
    }

    private void DisplayMethods()
    {
        foreach (CharacterMethod method in currentCharacter.methods)
        {
            GameObject methodButton = Instantiate(buttonPrefab, methodsPanel);

            TMP_Text buttonText = methodButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = method.name;

            AccessModifierButton accessModifierButton = methodButton.GetComponent<AccessModifierButton>();
            accessModifierButton.associatedMethod = method;
        }
    }

    private Character FindCharacterByName(string characterName)
    {
        foreach (Character character in _charactersCollection)
        {
            if (character.name == characterName)
            {
                return character;
            }
        }

        return null;
    }

    private void CleanPanel()
    {
        foreach (Transform child in attributesPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in methodsPanel)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowAttributesPopup()
    {
        if (currentCharacter != null)
        {
            AttributesPopupManager popupManager = attributesPopup.GetComponent<AttributesPopupManager>();
            popupManager.ShowAttributesPopup(currentCharacter);
        }
    }

    public void ShowMethodsPopup()
    {
        if (currentCharacter != null)
        {
            MethodsPopupManager popupManager = methodsPopup.GetComponent<MethodsPopupManager>();
            popupManager.ShowMethodsPopup(currentCharacter);
        }
    }

    public void ChangingGameObjectName()
    {
        gameObject.name = currentCharacter.name;
    }

    public void AddCharacter()
    {
        string characterName = "";
        string characterDescription = "";
        List<CharacterAttribute> characterAttributes = new List<CharacterAttribute>();
        List<CharacterMethod> characterMethods = new List<CharacterMethod>();
        List<Character> characterAncestors = new List<Character>();
        //add the ancestors


        Character newCharacter = new Character();
        newCharacter.name = "New Character";
        newCharacter.ancestors.Add(_charactersCollection[0]);
        newCharacter.description = "New Description";

        _charactersCollection.Add(newCharacter);





        
        // Character1 
        characterName = "Character 1";
        characterDescription = "This is the first character";
        Character character1 = new Character(characterName, characterDescription, characterAttributes, characterMethods, characterAncestors);
        _charactersCollection.Add(character1);

        TreeNode treeNodeRoot = new TreeNode(character1, null, null, 0);
        buttonTreeManager = new ButtonTreeManager(treeNodeRoot, this);
        CharacterTree.GetComponent<ButtonTreeManager>().CreateButton(treeNodeRoot);
        DisplayCharacterDetails(character1.name);





        /*
        Transform myTransform = newPlayerButton.transform;
        Debug.Log(myTransform.name);
        Transform parentTransform = myTransform.parent;
        Debug.Log(parentTransform.name);

        // Remplacez "SiblingName" par le nom du frère que vous cherchez
        string siblingNameToFind = newCharacter.ancestors[0].name;

        // Cherchez le frère parmi les enfants du parent
        Transform siblingTransform = parentTransform.Find(siblingNameToFind);  
        Debug.Log(siblingTransform.name);
        
        GameObject foundObject = siblingTransform.gameObject;






        LineRenderer lineRenderer = foundObject.GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, foundObject.transform.position);
        lineRenderer.SetPosition(1, newPlayerButton.transform.position);
        lineRenderer.material.color = Color.red;
        */

        // CharacterTree.GetComponent<ButtonTreeManager>().CreateButton(newCharacter, parentofthenewCharacter);

        // PreDetails(newCharacter);



        // DisplayCharacterDetails(newCharacter.name);
    }

    public void PreDetails(Character newCharacter)
    {
        foreach (Character character in newCharacter.ancestors)
        {
            if (character.attributes != null)
            {
                foreach (CharacterAttribute attribute in character.attributes)
                {
                    CharacterAttribute newAttribute = new CharacterAttribute(attribute.name, attribute.description, attribute.accessModifier);
                    if (!(attribute.accessModifier == AccessModifier.Private))
                    {
                        if (!newCharacter.attributes.Any(a => a.name == attribute.name))
                        {
                            newCharacter.attributes.Add(newAttribute);
                        }
                    }
                }
            }

            if (character.methods != null)
            {
                foreach (CharacterMethod method in character.methods)
                {
                    CharacterMethod newMethod = new CharacterMethod(method.name, method.description, method.accessModifier);
                    if (!(method.accessModifier == AccessModifier.Private))
                    {
                        if (!newCharacter.methods.Any(m => m.name == method.name))
                        {
                            newCharacter.methods.Add(newMethod);
                        }
                    }
                }
            }
        }
    }


    public void CreateCharacters()
    {
        string characterName = "";
        string characterDescription = "";
        List<CharacterAttribute> characterAttributes = new List<CharacterAttribute>();
        List<CharacterMethod> characterMethods = new List<CharacterMethod>();
        List<Character> characterAncestors = new List<Character>();

        // Character1 
        characterName = "Character 1";
        characterDescription = "This is the first character";
        Character character1 = new Character(characterName, characterDescription, characterAttributes, characterMethods, characterAncestors);
        _charactersCollection.Add(character1);

        TreeNode treeNodeRoot = new TreeNode(character1, null, null, 0);
        buttonTreeManager = new ButtonTreeManager(treeNodeRoot, this);
        CharacterTree.GetComponent<ButtonTreeManager>().CreateButton(treeNodeRoot);
        DisplayCharacterDetails(character1.name);
    }

}
