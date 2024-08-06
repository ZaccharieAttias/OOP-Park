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
    public List<(CharacterB, bool)> AbstractClasses;
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

        Stage = -1;
        MaxStage = 2;
        FolderPath = Path.Combine(Application.dataPath, "Resources/Json", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void SetStage()
    {
        Stage++;
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

            if (character.IsAbstract) AbstractClasses.Add((character, false));
        }

        for (int i = 0; i < AbstractClasses.Count; i++)
        {
            (CharacterB, bool) item = AbstractClasses[i];
            AbstractMapModifier(item.Item1);
        }
    }
    public void AbstractMapModifier(CharacterB character)
    {
        if (AbstractClasses[AbstractClasses.FindIndex(item => item.Item1 == character)].Item2)
        {
            return;
        }
        AbstractClasses[AbstractClasses.FindIndex(item => item.Item1 == character)] = (character, true);

        Dictionary<string, List<CharacterB>> commonAttributes = new();
        Dictionary<string, List<CharacterB>> commonMethods = new();

        foreach (CharacterB child in character.Childrens)
        {
            if (child.IsAbstract) AbstractMapModifier(child);

            foreach (Attribute attribute in child.Attributes)
            {
                if (!commonAttributes.TryGetValue(attribute.Name, out List<CharacterB> characters))
                {
                    characters = new List<CharacterB>();
                    commonAttributes.Add(attribute.Name, characters);
                }
                commonAttributes[attribute.Name].Add(child);
            }

            foreach (Method method in child.Methods)
            {
                if (!commonMethods.TryGetValue(method.Name, out List<CharacterB> characters))
                {
                    characters = new List<CharacterB>();
                    commonMethods.Add(method.Name, characters);
                }
                commonMethods[method.Name].Add(child);
            }
        }

        foreach (KeyValuePair<string, List<CharacterB>> entry in commonAttributes)
        {
            if (entry.Value.Count > 1)
            {
                FinalCharacterAttributesMap[character].Add(entry.Key);

                foreach (CharacterB characterFix in entry.Value)
                {
                    FinalCharacterAttributesMap[characterFix].Remove(entry.Key);
                }
            }
        }

        foreach (KeyValuePair<string, List<CharacterB>> entry in commonMethods)
        {
            if (entry.Value.Count > 1)
            {
                FinalCharacterMethodsMap[character].Add(entry.Key);

                foreach (CharacterB characterFix in entry.Value)
                {
                    FinalCharacterMethodsMap[characterFix].Remove(entry.Key);
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