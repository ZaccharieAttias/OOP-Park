using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class CharacterAppearanceManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Popup;
    public Canvas Canvas;
    public List<GameObject> Cameras;

    [Header("Buttons")]
    public Button ConfirmButton;
    public Button CancelButton;
    public Button ResetButton;

    [Header("Character Components")]
    public Character Character;
    public Transform playerTransform;
    public Vector3 oldPosition;


    public void Start()
    {
        InitializeUIElements();
        InitializeButtons();
        InitializeCharacterComponents();
        InitializeOverride();
    }
    public void InitializeUIElements()
    {
        Popup = GameObject.Find("Canvas/Popups/CharacterAppearance");
        Canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        Cameras = new List<GameObject>
        {
            GameObject.Find("Main Camera"),
            GameObject.Find("Camera"),
        };
        Cameras[1].SetActive(false);
    }
    public void InitializeButtons()
    {
        ConfirmButton = Popup.transform.Find("Character/Buttons/Confirm").GetComponent<Button>();
        ConfirmButton.onClick.AddListener(ToggleOff);

        CancelButton = Popup.transform.Find("Character/Buttons/Cancel").GetComponent<Button>();
        CancelButton.onClick.AddListener(ToggleOff);

        ResetButton = Popup.transform.Find("Character/Buttons/Reset").GetComponent<Button>();
    }
    public void InitializeCharacterComponents()
    {
        if (GameObject.Find("Player") == null) return;
        
        Character = GameObject.Find("Player").GetComponent<Character>();
        playerTransform = GameObject.Find("Player").transform;
        oldPosition = playerTransform.position;
    }
    public void InitializeOverride()
    {
        if (RestrictionManager.Instance.AllowOverride)
        {
            var rootCharacter = CharactersData.CharactersManager.CharactersCollection.First();
            var attribute = new Attribute
            {
                Owner = rootCharacter.Name,
                Name = "appearance",
                Description = "override character appearance",
                Value = 0,
                AccessModifier = AccessModifier.Public,
                Getter = false,
                Setter = false,
            };

            var method = new Method
            {
                Owner = rootCharacter.Name,
                Name = "Appearance",
                Description = "override character appearance",
                Attribute = attribute,
                AccessModifier = AccessModifier.Public,
            };

            rootCharacter.Attributes.Add(attribute);
            rootCharacter.Methods.Add(method);
        }
    }

    public void SwitchToEditCamera()
    {
        oldPosition = playerTransform.position;
        playerTransform.localScale = new Vector3(100, 100, 100);
        playerTransform.position = new Vector3(664, 618, 0);

        Canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Canvas.worldCamera = Cameras[1].GetComponent<Camera>();

        Cameras[0].SetActive(false);
        Cameras[1].SetActive(true);
    }
    public void SwitchToMainCamera()
    {
        playerTransform.localScale = new Vector3(1, 1, 1);
        playerTransform.position = oldPosition;

        Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        Cameras[0].SetActive(true);
        Cameras[1].SetActive(false);
    }

    public void ToggleOn()
    {
        SceneManagement.ScenePause("CharacterAppearanceManager");

        SwitchToEditCamera();

        Popup.SetActive(true);
    }
    public void ToggleOff()
    {
        SceneManagement.SceneResume("CharacterAppearanceManager");

        SwitchToMainCamera();

        Popup.SetActive(false);
    }
    public void ToggleActivation()
    {
        if (Popup.activeSelf) ToggleOff();
        else ToggleOn();
    }
}