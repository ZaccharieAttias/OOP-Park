using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class CharacterChallengeManager : MonoBehaviour
{
    [Header("Scripts")]
    public CharacterAppearanceManager CharacterAppearanceManager;
    public CharacterEditor1 CharacterEditor1;
    public AiModelData AiModelData;
    public GameController GameController;

    [Header("UI Elements")]
    public GameObject Mission1Popup;
    public GameObject Mission2Popup;
    public List<GameObject> MissionPopup;
    public List<GameObject> Walls;

    [Header("Challenge Attributes")]
    public int Challenge;
    public List<int> ChallengeNumberList;
    public Dictionary<int, List<string>> ChallengeAppearancesConditions;

    public void Start()
    {
        InitializeScripts();
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "OnlineBuilder" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "OnlinePlayground" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "C0L2" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "C1L3")
        {
            InitializeUIElements();
            InitializeUniqueListeners();
            InitializeSelfProperties();
            UpdateWallsBasedOnAppearance();
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "C1L3")
        {
            InitializeUniqueListeners();
            InitializeSelfProperties();
            ChallengeAppearancesConditions = new Dictionary<int, List<string>>();
            InitializeUIOnlineElements(1, new List<string> { "FullHair", "WeaponType", "Cape" });
            InitializeUIOnlineElements(2, new List<string> { "WeaponType", "Mask", "Hair"});
            InitializeUIOnlineElements(3, new List<string> { "FullHair", "WeaponType", "Cape" });
            Walls.Clear();
            Walls.Add(GameObject.Find("Grid/Gameplay/WallChallenge" + 1));
            Walls.Add(GameObject.Find("Grid/Gameplay/WallChallenge" + 2));
            Walls.Add(GameObject.Find("Grid/Gameplay/WallChallenge" + 3));
            Debug.Log("ChallengeAppearancesConditions");
        }
        else
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "OnlineBuilder" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "OnlinePlayground")
                InitializeUniqueListeners();
            InitializeSelfProperties();
            ChallengeAppearancesConditions = new Dictionary<int, List<string>>();
        }

    }
    public void InitializeScripts()
    {
        CharacterAppearanceManager = GameObject.Find("Canvas/Popups").GetComponent<CharacterAppearanceManager>();
        CharacterAppearanceManager.ConfirmButton.onClick.AddListener(() => ConfirmFactory());
        CharacterAppearanceManager.CancelButton.onClick.AddListener(() => CancelFactory());
        CharacterAppearanceManager.ResetButton.onClick.AddListener(() => ResetFactory());

        CharacterEditor1 = GameObject.Find("Scripts/CharacterEditor").GetComponent<CharacterEditor1>();
        AiModelData = GameObject.Find("Scripts/AiModelData").GetComponent<AiModelData>();
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "OnlineBuilder" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "OnlinePlayground")
            GameController = GameObject.Find("Player").GetComponent<GameController>();
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
    public void InitializeUIOnlineElements(int index, List<string> appearancesCondition)
    {
        ChallengeNumberList.Add(index);
        ChallengeAppearancesConditions.Add(index, appearancesCondition);
        MissionPopup.Add(GameObject.Find("Canvas/Popups/Mission" + index));
        Walls.Add(GameObject.Find("Grid/LevelBuilder/Gameplay/WallChallenge" + index));
    }
    public void InitializeUniqueListeners()
    {
        Transform allTransform = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Tree/Buttons/Background/ScrollView/ViewPort/All").transform;

        var CharacterGameObjects = allTransform.Cast<Transform>()
            .Where(child => child.gameObject.name != "Line")
            .Select(child => child.gameObject)
            .ToList();

        CharacterGameObjects.ForEach(item => item.GetComponent<Button>().onClick.AddListener(() => BackStage()));
        if (RestrictionManager.Instance.AllowUpcasting || RestrictionManager.Instance.AllowOverride)
            GameObject.Find("Canvas/Menus/Gameplay/SwapScreen").GetComponent<Button>().onClick.AddListener(() => BackStage());
    }
    public void InitializeSelfProperties()
    {
        Challenge = 1;
    }

    public void ConfirmFactory()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "OnlineBuilder" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "OnlinePlayground" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "C0L2" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "C1L3")
            UpdateWallsBasedOnAppearance();
        else if (ChallengeAppearancesConditions.Count > 0)
            UpdateWallsBasedOnAppearance(Challenge, ChallengeAppearancesConditions.FirstOrDefault(item => item.Key == Challenge).Value);
        AiModelData.AppearanceLevelTries++;
    }
    public void CancelFactory()
    {
        CharacterEditor1.LoadFromJson();
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "OnlineBuilder" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "OnlinePlayground" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "C0L2" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "C1L3")
            UpdateWallsBasedOnAppearance();
        else if (ChallengeAppearancesConditions.Count > 0)
            UpdateWallsBasedOnAppearance(Challenge, ChallengeAppearancesConditions.FirstOrDefault(item => item.Key == Challenge).Value);
    }
    public void ResetFactory()
    {
        CharacterEditor1.LoadFromJson();
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "OnlineBuilder" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "OnlinePlayground" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "C0L2" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "C1L3")
            UpdateWallsBasedOnAppearance();
        else if (ChallengeAppearancesConditions.Count > 0)
            UpdateWallsBasedOnAppearance(Challenge, ChallengeAppearancesConditions.FirstOrDefault(item => item.Key == Challenge).Value);
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
        json =json.Trim ('{','}');
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        string[] pairs = json.Split(',');
        foreach (string pair in pairs)
        {
            string[] keyValue = pair.Split(new[] {':'}, 2);
            string key = keyValue[0].Trim('"');
            string value = keyValue[1].Trim('"');
            dictionary.Add(key, value);
        }
        if (appearanceType == "MeleePaired" || appearanceType == "Melee1H" || appearanceType == "Melee2H" || appearanceType == "Bow")
            return dictionary["WeaponType"] == appearanceType ? appearanceType : "";
        else if (appearanceType == "Armor" || appearanceType == "Vest" || appearanceType == "Gloves" || appearanceType == "Belt" || appearanceType == "Boots" || appearanceType == "Pauldrons")
        {
            if (appearanceType == "Armor")
                return dictionary["Armor[0]"] != "" ? appearanceType : "";
            else if (appearanceType == "Vest")
                return dictionary["Armor[25]"] != "" ? appearanceType : "";
            else if (appearanceType == "Gloves")
                return dictionary["Armor[4]"] != "" ? appearanceType : "";
            else if (appearanceType == "Belt")
                return dictionary["Armor[20]"] != "" ? appearanceType : "";
            else if (appearanceType == "Boots")
                return dictionary["Armor[18]"] != "" ? appearanceType : "";
            else if (appearanceType == "Pauldrons")
                return dictionary["Armor[1]"] != "" ? appearanceType : "";
        }
        else if(!dictionary.ContainsKey(appearanceType))
        {
            foreach (var key in dictionary.Keys)
            {
                if (key.Contains(appearanceType))
                    return dictionary[key];
            }
            return "";
        }
        return dictionary[appearanceType];
    }
    public void SetChallenge1()
    {
        Mission1Popup.SetActive(true);
    }
    public void SetChallenge2()
    {
        Mission2Popup.SetActive(true);
    }
    public void SetChallenge(int challenge)
    {
        MissionPopup.FirstOrDefault(item => item.name == "Mission" + challenge)?.SetActive(true);
        Challenge = challenge;
    }
    public void BackStage()
    {
        if ((!RestrictionManager.Instance.AllowUpcasting && !RestrictionManager.Instance.AllowOverride) || (RestrictionManager.Instance.OnlineBuild && GameObject.Find("Scripts/PlayTestManager") != null && !GameObject.Find("Scripts/PlayTestManager").GetComponent<PlayTestManager>().IsTestGameplay)) return;
        if (ChallengeAppearancesConditions == null) return;
        if (RestrictionManager.Instance.AllowUpcasting)
        {
            UpcastingManager upcastingManager = GameObject.Find("Canvas/Popups").GetComponent<UpcastingManager>();
            if (upcastingManager.IsUpcasting)
            {
                //giving the player the option to upcast and the removed methods
                foreach (var method in upcastingManager.noneSharedMethods)
                {
                    CharactersData.CharactersManager.CurrentCharacter.Methods.Add(method);
                    CharactersData.CharactersManager.DisplayCharacter(CharactersData.CharactersManager.CurrentCharacter);
                }
                upcastingManager.IsUpcasting = false;
                CharacterEditor1.LoadFromJson();
                if (ChallengeAppearancesConditions.Count > 0)
                {
                    UpdateWallsBasedOnAppearance(Challenge, ChallengeAppearancesConditions.FirstOrDefault(item => item.Key == Challenge).Value);
                    GameController.returnLastPosition();
                }
            }
            CharacterEditor1.LoadFromJson();
            if (!RestrictionManager.Instance.AllowOverride || !upcastingManager.IsUpcasting) return;
        }

        CharacterEditor1.LoadFromJson();
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "OnlineBuilder" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "OnlinePlayground" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "C0L2" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "C1L3")
            UpdateWallsBasedOnAppearance();
        else if (ChallengeAppearancesConditions.Count > 0)
        {   
            UpdateWallsBasedOnAppearance(Challenge, ChallengeAppearancesConditions.FirstOrDefault(item => item.Key == Challenge).Value);
            GameController.returnLastPosition();
        }

    }
    public void UpdateWallsBasedOnAppearance(int index, List<string> appearancesCondition)
    {
        string json = CharacterAppearanceManager.Character.ToJson();

        // Check if all conditions are met
        bool allConditionsMet = true;
        foreach (string appearance in appearancesCondition)
        {
            if (GetAppearanceValue(json, appearance) == "" || GetAppearanceValue(json, appearance) == "False" || GetAppearanceValue(json, appearance) == "0")
            {
                allConditionsMet = false;
                break;
            }
        }

        // If all conditions are met, set wall to inactive
        if (allConditionsMet)
        {
            Walls.FirstOrDefault(item => item.name == "WallChallenge" + index)?.SetActive(false);
        }
        else
        {
            Walls.FirstOrDefault(item => item.name == "WallChallenge" + index)?.SetActive(true);
        }
    }
    public void ResetWalls()
    {
        Walls.ForEach(wall => wall.SetActive(true));

    }
    public void DestroyWallAndMission(int missionnumber)
    {
        GameObject wall = GameObject.Find("Grid/LevelBuilder/Gameplay/WallChallenge" + missionnumber);
        GameObject mission = GameObject.Find("Canvas/Popups/Mission" + missionnumber);
        Walls.Remove(wall);
        MissionPopup.Remove(mission);
        ChallengeAppearancesConditions.Remove(missionnumber);
        ChallengeNumberList.Remove(missionnumber);
        Destroy(mission);
        Destroy(wall);
    }
}