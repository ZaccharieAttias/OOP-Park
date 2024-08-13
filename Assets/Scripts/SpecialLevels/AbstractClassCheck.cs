using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class AbstractClassCheck : MonoBehaviour
{
    [Header("Scripts")]
    public AiModelData AiModelData;
    public FeedbackManager FeedbackManager;

    [Header("UI Elements")]
    public GameObject Popup;
    public GameObject MessagePopup;
    public GameObject ErrorContentPanel;
    public Transform CharactersContentPanel;

    [Header("Buttons")]
    public GameObject ConfirmButton;
    public GameObject ErrorObjectPrefab;

    [Header("Data")]
    public Dictionary<CharacterB, List<string>> FinalCharacterAttributesMap;
    public Dictionary<CharacterB, List<string>> FinalCharacterMethodsMap;
    public Dictionary<CharacterB, List<string>> ErrorsMap;
    public List<(CharacterB, int)> AbstractClasses;
    public int Stage;
    public int MaxStage;
    public string FolderPath;


    public void Start()
    {
        InitializeScripts();
        InitializeUIElements();
        InitializeButtons();
        InitializeData();

        if (RestrictionManager.Instance.AllowAbstractClass && (!RestrictionManager.Instance.OnlineGame || !RestrictionManager.Instance.OnlineBuild)) SetStage();
    }
    public void InitializeScripts()
    {
        AiModelData = GameObject.Find("Scripts/AiModelData").GetComponent<AiModelData>();
        FeedbackManager = GameObject.Find("Canvas/Popups").GetComponent<FeedbackManager>();
    }
    public void InitializeUIElements()
    {
        Popup = GameObject.Find("Canvas/Popups/Abstract");
        MessagePopup = Popup.transform.Find("Message").gameObject;
        CharactersContentPanel = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Tree/Buttons/Background/ScrollView/ViewPort/All").transform;
        ErrorContentPanel = GameObject.Find("Canvas/Popups/Abstract/Message/ScrollView/ViewPort/Content");
    }
    public void InitializeButtons()
    {
        ConfirmButton = Popup.transform.Find("Confirm").gameObject;
        ConfirmButton.GetComponent<Button>().onClick.AddListener(() => ConfirmStage());

        ErrorObjectPrefab = Resources.Load<GameObject>("ErrorObject");
    }
    public void InitializeData()
    {
        FinalCharacterAttributesMap = new();
        FinalCharacterMethodsMap = new();
        ErrorsMap = new();
        AbstractClasses = new();

        Stage = 0;
        MaxStage = 3;
        FolderPath = Path.Combine(Application.dataPath, "Resources/Json", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void SetStage()
    {
        foreach (Transform child in CharactersContentPanel) Destroy(child.gameObject);

        if (Stage < MaxStage)
        {
            AttributesData.Load($"{FolderPath}/{Stage}/Attributes.json");
            MethodsData.Load($"{FolderPath}/{Stage}/Methods.json");
            SpecialAbilitiesData.Load($"{FolderPath}/{Stage}/SpecialAbilities.json");
            CharactersData.Load($"{FolderPath}/{Stage}/Characters.json");
            CharactersGameObjectData.Load();

            FinalCharacterAttributesMap = new();
            FinalCharacterMethodsMap = new();
            ErrorsMap = new();

            AbstractClasses = new();

            BuildFinalDataMap();
            Stage++;
        }
        else
        {
            FeedbackManager.ToggleOn();
        }
    }
    public void BuildFinalDataMap()
    {
        List<CharacterB> charactersCollection = CharactersData.CharactersManager.CharactersCollection;

        foreach (CharacterB character in charactersCollection)
        {
            FinalCharacterAttributesMap.Add(character, character.Attributes.Select(attribute => attribute.Name).ToList());
            FinalCharacterMethodsMap.Add(character, character.Methods.Select(method => method.Name).ToList());
        }

        // Traverse the charactersCollection from the root to each abstarct class (using the childrens) and get its depth using BFS
        Queue<(CharacterB, int)> queue = new();
        queue.Enqueue((charactersCollection[0], 0));

        while (queue.Count > 0)
        {
            (CharacterB character, int depth) = queue.Dequeue();

            if (character.IsAbstract)
            {
                AbstractClasses.Add((character, depth));
            }

            if (character.Childrens.Count > 0)
            {
                foreach (CharacterB child in character.Childrens)
                {
                    queue.Enqueue((child, depth + 1));
                }
            }
        }

        // Sort the abstract classes by depth from higher to lower
        AbstractClasses = AbstractClasses.OrderByDescending(tuple => tuple.Item2).ToList();
        for (int i = 0; i < AbstractClasses.Count; i++)
        {
            var (character, _) = AbstractClasses[i];
            AbstractMapModifier(character);
        }
    }
    public void AbstractMapModifier(CharacterB character)
    {
        Dictionary<string, int> attributeCount = new();
        Dictionary<string, int> methodCount = new();
        int totalChildren = character.Childrens.Count;

        // Traverse all direct children of the abstract class
        foreach (CharacterB child in character.Childrens)
        {
            // Count attributes acroos all childrens
            foreach (string attribute in FinalCharacterAttributesMap[child])
            {
                if (!attributeCount.ContainsKey(attribute))
                {
                    attributeCount[attribute] = 0;
                }
                attributeCount[attribute]++;
            }

            // Count attributes acroos all childrens
            foreach (string method in FinalCharacterMethodsMap[child])
            {
                if (!methodCount.ContainsKey(method))
                {
                    methodCount[method] = 0;
                }
                methodCount[method]++;
            }
        }

        // Identify and move common attributes to the abstract parent
        foreach (var entry in attributeCount)
        {
            if (entry.Value == totalChildren) // All children share this attribute
            {
                FinalCharacterAttributesMap[character].Add(entry.Key);

                // Remove this attribute from all children
                foreach (CharacterB child in character.Childrens)
                {
                    FinalCharacterAttributesMap[child].Remove(entry.Key);
                }
            }
        }

        // Identify and move common methods to the abstract parent
        foreach (var entry in methodCount)
        {
            if (entry.Value == totalChildren) // All children share this method
            {
                FinalCharacterMethodsMap[character].Add(entry.Key);

                // Remove this method from all children
                foreach (CharacterB child in character.Childrens)
                {
                    FinalCharacterMethodsMap[child].Remove(entry.Key);
                }
            }
        }
    }



    public void ConfirmStage()
    {
        AiModelData.AbstractLevelTries++;

        ErrorsMap = new();
        List<CharacterB> CharactersCollection = CharactersData.CharactersManager.CharactersCollection;
        foreach (CharacterB character in CharactersCollection)
        {
            List<string> characterAttributes = character.Attributes.Select(attribute => attribute.Name).ToList();
            List<string> finalAttributes = FinalCharacterAttributesMap[character];

            foreach (string attribute in characterAttributes)
            {
                if (!finalAttributes.Contains(attribute))
                {
                    if (!ErrorsMap.ContainsKey(character))
                    {
                        List<string> errors = new();
                        ErrorsMap.Add(character, errors);
                    }
                    ErrorsMap[character].Add($"{attribute} is not needed.");
                }
            }
            foreach (string attribute in finalAttributes)
            {
                if (!characterAttributes.Contains(attribute))
                {
                    if (!ErrorsMap.ContainsKey(character))
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
                if (!finalMethods.Contains(method))
                {
                    if (!ErrorsMap.ContainsKey(character))
                    {
                        List<string> errors = new();
                        ErrorsMap.Add(character, errors);
                    }
                    ErrorsMap[character].Add($"{method} is not needed.");
                }
            }
            foreach (string method in finalMethods)
            {
                if (!characterMethods.Contains(method))
                {
                    if (!ErrorsMap.ContainsKey(character))
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

            foreach (KeyValuePair<CharacterB, List<string>> entry in ErrorsMap)
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