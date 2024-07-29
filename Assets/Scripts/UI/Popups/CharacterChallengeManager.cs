using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using Assets.HeroEditor.Common.Scripts.Common;
using UnityEngine;
using UnityEngine.UI;




public class CharacterChallangeManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Popup;
    public Transform playerTransform;
    public Vector3 oldPosition;
    public List<GameObject> Cameras;
    public Canvas Canvas;
    public Character Character;
    public Button ConfirmButton;
    public Button CancelButton;
    public Button ResetButton;

    public GameObject Mission1Popup;
    public GameObject Mission2Popup;
    public GameObject Mission3Popup;

    public List<GameObject> Walls;

    public int challenge = 1;

    public Transform Player;
    public List<GameObject> CharacterGameObjects;
    public CharacterEditor1 CharacterEditor1;

    public void Start()
    {
        Player = GameObject.Find("Player").transform;

        Mission1Popup = GameObject.Find("Canvas/Popups/Mission1");
        Mission2Popup = GameObject.Find("Canvas/Popups/Mission2");
        Mission3Popup = GameObject.Find("Canvas/Popups/Mission3");

        Popup = GameObject.Find("Canvas/Popups/CharacterAppearance");
        if (GameObject.Find("Player") != null)
        {
            playerTransform = GameObject.Find("Player").transform;
            Character = GameObject.Find("Player").GetComponent<Character>();
        }
        Canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        Cameras = new List<GameObject>()
        {
            GameObject.Find("Main Camera"),
            GameObject.Find("Camera"),
        };

        Cameras[1].SetActive(false);


        CharacterGameObjects = new List<GameObject>();
        Walls = new List<GameObject>()
        {
            GameObject.Find("Grid/Challenges/Wall1"),
            GameObject.Find("Grid/Challenges/Wall2"),
            GameObject.Find("Grid/Challenges/Wall3"),
        };

        CharacterEditor1 = GameObject.Find("Scripts/CharacterEditor").GetComponent<CharacterEditor1>();

        ConfirmButton = Popup.transform.Find("Character/Buttons/Confirm").GetComponent<Button>();
        CancelButton = Popup.transform.Find("Character/Buttons/Cancel").GetComponent<Button>();
        ResetButton = Popup.transform.Find("Character/Buttons/Reset").GetComponent<Button>();

        ConfirmButton.onClick.AddListener(() => ConfirmFactory());
        CancelButton.onClick.AddListener(() => CancelFactory());
        ResetButton.onClick.AddListener(() => ResetFactory());
    }


    public void ConfirmFactory()
    {
        ChallengeCheck();
        ToggleOff();
    }

    public void CancelFactory()
    {
        ChallengeCheck();
        CharacterEditor1.LoadFromJson();
        ToggleOff();
    }
    public void ResetFactory()
    {
        CharacterEditor1.LoadFromJson();
    }
    public void ChallengeCheck()
    {
        if (CharacterGameObjects.Count == 0)
        {
            Transform allTransform = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Tree/Buttons/Background/ScrollView/ViewPort/All").transform;
            foreach (Transform child in allTransform)
            {
                if (child.gameObject.name != "Line")
                {
                    CharacterGameObjects.Add(child.gameObject);
                }
            }

            foreach (var item in CharacterGameObjects)
            {
                item.GetComponent<Button>().onClick.AddListener(() => BackStage());
            }
        }


        string json = Character.ToJson();
        string glassesValue = "";
        string helmetValue = "";
        string beardValue = "";

        foreach (var item in json.Split(','))
        {
            if (item.Contains("Glasses"))
            {
                glassesValue = item.Split(':')[1];
            }

            if (item.Contains("Helmet"))
            {
                helmetValue = item.Split(':')[1];
            }

            if (item.Contains("Beard"))
            {
                beardValue = item.Split(':')[1];
            }
        }


        Walls[0].SetActive(glassesValue.Length < 3);
        Walls[1].SetActive(helmetValue.Length < 3);
        Walls[2].SetActive(beardValue.Length < 3);
    }
    public void BackStage()
    {
        if (challenge == 1)
        {
            Player.position = new Vector3(12, 0, 0);
        }
        else if (challenge == 2)
        {
            Player.position = new Vector3(22, 0, 0);
        }
        else if (challenge == 3)
        {
            Player.position = new Vector3(48, 0, 0);
        }
    }

    public void SetChallenge1()
    {
        challenge = 1;

        Mission1Popup.SetActive(true);
    }

    public void SetChallenge2()
    {
        challenge = 2;
        Mission2Popup.SetActive(true);
    }

    public void SetChallenge3()
    {
        challenge = 3;
        Mission3Popup.SetActive(true);
    }

    public void ToggleOn()
    {
        SceneManagement.ScenePause("CharacterChallengeManager");

        oldPosition = playerTransform.position;
        playerTransform.localScale = new Vector3(100, 100, 100);
        playerTransform.position = new Vector3(664, 618, 0);

        Canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Canvas.worldCamera = Cameras[1].GetComponent<Camera>();

        Cameras[0].SetActive(false);
        Cameras[1].SetActive(true);

        Popup.SetActive(true);
    }
    public void ToggleOff()
    {
        SceneManagement.SceneResume("CharacterChallengeManager");
        CharactersData.CharactersManager.DisplayCharacter(CharactersData.CharactersManager.CurrentCharacter);

        playerTransform.localScale = new Vector3(1, 1, 1);
        playerTransform.position = oldPosition;

        Canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        Cameras[0].SetActive(true);
        Cameras[1].SetActive(false);
        Popup.SetActive(false);
    }
    public void ToggleActivation()
    {
        if (Popup.activeSelf)
            ToggleOff();
        else
            ToggleOn();
    }
}



