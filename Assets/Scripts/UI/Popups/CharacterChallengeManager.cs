using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class CharacterChallengeManager : MonoBehaviour
{
    [Header("Scripts")]
    public CharacterAppearanceManager CharacterAppearanceManager;
    public CharacterEditor1 CharacterEditor1;
    public AiModelData AiModelData;

    [Header("UI Elements")]
    public GameObject Mission1Popup;
    public GameObject Mission2Popup;
    public List<GameObject> Walls;

    [Header("Challenge Attributes")]
    public int Challenge;


    public void Start()
    {
        InitializeScripts();
        InitializeUIElements();
        InitializeUniqueListeners();
        InitializeSelfProperties();
        UpdateWallsBasedOnAppearance();

    }
    public void InitializeScripts()
    {
        CharacterAppearanceManager = GameObject.Find("Canvas/Popups").GetComponent<CharacterAppearanceManager>();
        CharacterAppearanceManager.ConfirmButton.onClick.AddListener(() => ConfirmFactory());
        CharacterAppearanceManager.CancelButton.onClick.AddListener(() => CancelFactory());
        CharacterAppearanceManager.ResetButton.onClick.AddListener(() => ResetFactory());

        CharacterEditor1 = GameObject.Find("Scripts/CharacterEditor").GetComponent<CharacterEditor1>();
        AiModelData = GameObject.Find("Scripts/AiModelData").GetComponent<AiModelData>();
    }
    public void InitializeUIElements()
    {
        Mission1Popup = GameObject.Find("Canvas/Popups/Mission1");
        Mission2Popup = GameObject.Find("Canvas/Popups/Mission2");

        Walls = new List<GameObject>
        {
            GameObject.Find("Grid/Challenges/Wall1"),
            GameObject.Find("Grid/Challenges/Wall2"),
        };
    }
    public void InitializeUniqueListeners()
    {
        Transform allTransform = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Tree/Buttons/Background/ScrollView/ViewPort/All").transform;

        var CharacterGameObjects = allTransform.Cast<Transform>()
            .Where(child => child.gameObject.name != "Line")
            .Select(child => child.gameObject)
            .ToList();

        CharacterGameObjects.ForEach(item => item.GetComponent<Button>().onClick.AddListener(() => BackStage()));
    }
    public void InitializeSelfProperties()
    {
        Challenge = 1;
    }

    public void ConfirmFactory()
    {
        UpdateWallsBasedOnAppearance();
        AiModelData.AppearanceLevelTries++;
    }
    public void CancelFactory()
    {
        CharacterEditor1.LoadFromJson();
        UpdateWallsBasedOnAppearance();
    }
    public void ResetFactory()
    {
        CharacterEditor1.LoadFromJson();
        UpdateWallsBasedOnAppearance();
    }

    public void UpdateWallsBasedOnAppearance()
    {
        string json = CharacterAppearanceManager.Character.ToJson();

        string glassesValue = GetAppearanceValue(json, "Glasses");
        string beardValue = GetAppearanceValue(json, "Beard");

        Walls[0].SetActive(glassesValue.Length < 3);
        Walls[1].SetActive(beardValue.Length < 3);
    }
    public string GetAppearanceValue(string json, string appearanceType)
    {
        return json.Split(',')
            .FirstOrDefault(item => item.Contains(appearanceType))?
            .Split(':')[1] ?? "";
    }

    public void SetChallenge1()
    {
        Challenge = 1;
        Mission1Popup.SetActive(true);
    }
    public void SetChallenge2()
    {
        Challenge = 2;
        Mission2Popup.SetActive(true);
    }

    public void BackStage()
    {
        Vector3 position = Challenge switch
        {
            1 => new Vector3(-11.45f, -4.850f, 0),
            2 => new Vector3(40.745f, -4.850f, 0),
            _ => new Vector3(-11.45f, -4.850f, 0),
        };
        CharacterAppearanceManager.playerTransform.position = position;

        CharacterEditor1.LoadFromJson();
        UpdateWallsBasedOnAppearance();
    }
}