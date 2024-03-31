using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.IO;



public class AbstractClassCheck : MonoBehaviour
{
    public GameObject Popup;

    public GameObject ConfirmButton;
    public GameObject MessagePopup;

    public Transform CharactersContentPanel;

    public int stage;
    public int maxStage;
    public string FolderPath;

    public Dictionary<Character, List<string>> FinalCharacterAttributesMap;
    public Dictionary<Character, List<string>> FinalCharacterMethodsMap;
    public Dictionary<Character, List<string>> ErrorsMap;
    public List<(Character, bool)> AbstractClasses;

    public GameObject ErrorObjectPrefab;
    public GameObject ErrorContentPanel;


    public void Start()
    {
        Popup = GameObject.Find("Canvas/Popups/Abstract");

        MessagePopup = Popup.transform.Find("Message").gameObject;
        ConfirmButton = Popup.transform.Find("Confirm").gameObject;
        ConfirmButton.GetComponent<Button>().onClick.AddListener(() => ConfirmStage());

        CharactersContentPanel = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Tree/Buttons/ScrollView/ViewPort/All").transform;

        stage = -1;
        maxStage = 2;
        FolderPath = Path.Combine(Application.dataPath, "Resources/Json", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);


        ErrorObjectPrefab = Resources.Load<GameObject>("ErrorObject");
        ErrorContentPanel = GameObject.Find("Canvas/Popups/Abstract/Message/ScrollView/ViewPort/Content");


        SetStage();
    }
    public void SetStage()
    {
        stage++;
        foreach (Transform child in CharactersContentPanel) Destroy(child.gameObject);

        if (stage < maxStage)
        {
            AttributesData.Load($"{FolderPath}/{stage}/Attributes.json");
            MethodsData.Load($"{FolderPath}/{stage}/Methods.json");
            SpecialAbilitiesData.Load($"{FolderPath}/{stage}/SpecialAbilities.json");
            CharactersData.Load($"{FolderPath}/{stage}/Characters.json");
            CharactersGameObjectData.Load();


            FinalCharacterAttributesMap = new();
            FinalCharacterMethodsMap = new();
            ErrorsMap = new();

            AbstractClasses = new();

            BuildFinalDataMap();
        }

        else
        {
            string text = "Well Done Bitch, I`m Batman";
            MessagePopup.GetComponentInChildren<TMP_Text>().text = text;
            MessagePopup.SetActive(true);
        }
    }
    private void BuildFinalDataMap()
    {
        List<Character> charactersCollection = CharactersData.CharactersManager.CharactersCollection;

        foreach (Character character in charactersCollection)
        {
            FinalCharacterAttributesMap.Add(character, character.Attributes.Select(attribute => attribute.Name).ToList());
            FinalCharacterMethodsMap.Add(character, character.Methods.Select(method => method.Name).ToList());

            if (character.IsAbstract) AbstractClasses.Add((character, false));
        }

        // Call the AbstractMapModifier for each of the abstract classses in AbstractClasses only if their corresponds value is False, Please
        for (int i = 0; i < AbstractClasses.Count; i++)
        {
            (Character, bool) item = AbstractClasses[i];
            AbstractMapModifier(item.Item1);
        }
    }
    
    private void AbstractMapModifier(Character character)
    {
        if (AbstractClasses[AbstractClasses.FindIndex(item => item.Item1 == character)].Item2)
        {
            return;
        }
        AbstractClasses[AbstractClasses.FindIndex(item => item.Item1 == character)] = (character, true);
    
        Dictionary<string, List<Character>> commonAttributes = new();
        Dictionary<string, List<Character>> commonMethods = new();

        foreach (Character child in character.Childrens)
        {
            if (child.IsAbstract) AbstractMapModifier(child);
            
            foreach (Attribute attribute in child.Attributes)
            {
                if (commonAttributes.TryGetValue(attribute.Name, out List<Character> characters) == false)
                {
                    characters = new List<Character>();
                    commonAttributes.Add(attribute.Name, characters);
                }
                commonAttributes[attribute.Name].Add(child);
            }

            foreach (Method method in child.Methods)
            {
                if (commonMethods.TryGetValue(method.Name, out List<Character> characters) == false)
                {
                    characters = new List<Character>();
                    commonMethods.Add(method.Name, characters);
                }
                commonMethods[method.Name].Add(child);
            }
        }

        foreach (KeyValuePair<string, List<Character>> entry in commonAttributes)
        {
            if (entry.Value.Count > 1)
            {
                FinalCharacterAttributesMap[character].Add(entry.Key);

                foreach (Character characterFix in entry.Value)
                {
                    FinalCharacterAttributesMap[characterFix].Remove(entry.Key);
                }
            }
        }
        foreach (KeyValuePair<string, List<Character>> entry in commonMethods)
        {
            if (entry.Value.Count > 1)
            {
                FinalCharacterMethodsMap[character].Add(entry.Key);

                foreach (Character characterFix in entry.Value)
                {
                    FinalCharacterMethodsMap[characterFix].Remove(entry.Key);
                }
            }
        }
    }
    public void ConfirmStage()
    {
        ErrorsMap = new();
        List<Character> CharactersCollection = CharactersData.CharactersManager.CharactersCollection;
        foreach (Character character in CharactersCollection)
        {
            List<string> characterAttributes = character.Attributes.Select(attribute => attribute.Name).ToList();
            List<string> finalAttributes = FinalCharacterAttributesMap[character];

            foreach (string attribute in characterAttributes)
            {
                if (finalAttributes.Contains(attribute) == false)
                {
                    if (ErrorsMap.ContainsKey(character) == false)
                    {
                        List<string> errors = new();
                        ErrorsMap.Add(character, errors);
                    }
                    ErrorsMap[character].Add($"{attribute} is not needed.");
                }
            }
            foreach (string attribute in finalAttributes)
            {
                if (characterAttributes.Contains(attribute) == false)
                {
                    if (ErrorsMap.ContainsKey(character) == false)
                    {
                        List<string> errors = new();
                        ErrorsMap.Add(character, errors);
                    }
                    ErrorsMap[character].Add($"{attribute} is missing.");
                }
            }

            List<string> characterMethods = character.Methods.Select(method => method.Name).ToList();
            List<string> finalMethods = FinalCharacterMethodsMap[character];

            foreach (string method in characterMethods)
            {
                if (finalMethods.Contains(method) == false)
                {
                    if (ErrorsMap.ContainsKey(character) == false)
                    {
                        List<string> errors = new();
                        ErrorsMap.Add(character, errors);
                    }
                    ErrorsMap[character].Add($"{method} is not needed.");
                }
            }
            foreach (string method in finalMethods)
            {
                if (characterMethods.Contains(method) == false)
                {
                    if (ErrorsMap.ContainsKey(character) == false)
                    {
                        List<string> errors = new();
                        ErrorsMap.Add(character, errors);
                    }
                    ErrorsMap[character].Add($"{method} is missing.");
                }
            }
            
        }
    
        if (ErrorsMap.Count > 0)
        {
            foreach (Transform child in ErrorContentPanel.transform) Destroy(child.gameObject);

            foreach (KeyValuePair<Character, List<string>> entry in ErrorsMap)
            {
                foreach (string error in entry.Value)
                {
                    GameObject errorObject = Instantiate(ErrorObjectPrefab, ErrorContentPanel.transform);
                    errorObject.transform.Find("Sprite").GetComponent<Image>().sprite = entry.Key.CharacterButton.Button.GetComponent<Image>().sprite;

                    List<string> errorWords = error.Split(' ').ToList();
                    errorObject.transform.Find("Error/Title").GetComponent<TMP_Text>().text = errorWords[0];
                    errorWords.RemoveAt(0);
                    errorObject.transform.Find("Error/Description").GetComponent<TMP_Text>().text = string.Join(" ", errorWords);

                    errorObject.GetComponent<Image>().color = errorWords[^1] == "missing." ? Color.red : Color.yellow;
                }
            }

            MessagePopup.SetActive(true);
        }
        
        else
        {
            SetStage();
        }
    }
}
