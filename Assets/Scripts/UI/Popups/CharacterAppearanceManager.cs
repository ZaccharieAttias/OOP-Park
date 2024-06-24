using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using Assets.HeroEditor.Common.Scripts.Common;
using UnityEngine;
using UnityEngine.UI;




public class CharacterAppearanceManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Popup;


    public Transform playerTransform;
    public Vector3 oldPosition;


    public List<GameObject> Cameras;
    public Canvas Canvas;




    public Button ConfirmButton;
    public Button CancelButton;
    public Button ResetButton;


    public Character Character;


    public void Start()
    {
        Popup = GameObject.Find("Canvas/Popups/CharacterAppearance");
        playerTransform = GameObject.Find("Player").transform;
        Canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        Cameras = new List<GameObject>()
        {
            GameObject.Find("Main Camera"),
            GameObject.Find("Camera"),
        };
        Character = GameObject.Find("Player").GetComponent<Character>();


        Cameras[1].SetActive(false);


        ConfirmButton = Popup.transform.Find("Character/Buttons/Confirm").gameObject.GetComponent<Button>();
        ConfirmButton.onClick.AddListener(() => ConfirmFactory());
        CancelButton = Popup.transform.Find("Character/Buttons/Cancel").gameObject.GetComponent<Button>();
        CancelButton.onClick.AddListener(() => CancelFactory());
        ResetButton = Popup.transform.Find("Character/Buttons/Reset").gameObject.GetComponent<Button>();
        ResetButton.onClick.AddListener(() => ResetFactory());
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            ToggleActivation();
        }
    }

    public void CancelFactory()
    {
        ToggleOff();
    }
    public void ConfirmFactory()
    {
        string json = Character.ToJson();

        // Save it into a file in json format
        string path = Path.Combine(Application.dataPath, "Resources/CharactersData", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, $"{CharactersData.CharactersManager.CurrentCharacter.Name}.json");


        if (File.Exists(path))
        {
            File.Delete(path);
        }
        File.WriteAllText(path, json);


        ToggleOff();
    }
    public void ResetFactory()
    {
        // CharactersData.CharactersManager.DisplayCharacter(CharactersData.CharactersManager.CurrentCharacter);
    }




    public void ToggleOn()
    {
        SceneManagement.ScenePause();


        oldPosition = playerTransform.position;
        playerTransform.localScale = new Vector3(100, 100, 100);
        playerTransform.position = new Vector3(664, 618, 0);


        // Change the canvas Render Camera to Cameras[1]
        Canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Canvas.worldCamera = Cameras[1].GetComponent<Camera>();


        Cameras[0].SetActive(false);
        Cameras[1].SetActive(true);


        Popup.SetActive(true);
    }
    public void ToggleOff()
    {
        SceneManagement.SceneResume();
        CharactersData.CharactersManager.DisplayCharacter(CharactersData.CharactersManager.CurrentCharacter);


        playerTransform.localScale = new Vector3(1, 1, 1);
        playerTransform.position = oldPosition;


        Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        // Canvas.worldCamera = Cameras[0].GetComponent<Camera>();


        Cameras[0].SetActive(true);
        Cameras[1].SetActive(false);


        Popup.SetActive(false);
    }
    public void ToggleActivation()
    {
        if (Popup.activeSelf)
        {
            ToggleOff();
        }


        else
        {
            ToggleOn();
        }
    }
}



