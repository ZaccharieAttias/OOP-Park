using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UpcastingUI : MonoBehaviour
{
    public UpcastingLogic UpcastingLogic;

    public TextMeshProUGUI Character;
    public Button CharacterRight;
    public Button CharacterLeft;
    
    public TextMeshProUGUI Method;
    public Button MethodRight;
    public Button MethodLeft;
    
    public TextMeshProUGUI Quantity;
    public Button QuantityRight;
    public Button QuantityLeft;
    
    public Button CloseButton;
    public Button ConfirmButton;

    public void Start()
    {
        InitializeGameObject();
        InitializeProperties();
    }
    public void InitializeGameObject() { gameObject.SetActive(false); }
    private void InitializeProperties()
    {
        UpcastingLogic = GameObject.Find("Canvas/GameplayScreen/Popups/UpcastingManager/UpcastingLogic").GetComponent<UpcastingLogic>();

        Transform buttonsPanel = gameObject.transform.Find("Background/Foreground/Buttons");
    
        Character = buttonsPanel.Find("Character/Text").GetComponent<TextMeshProUGUI>();
        CharacterRight = buttonsPanel.Find("Character/Right").GetComponent<Button>();
        CharacterLeft = buttonsPanel.Find("Character/Left").GetComponent<Button>();
        CharacterRight.onClick.AddListener(() => UpcastingLogic.CharacterRightArrowClicked());
        CharacterLeft.onClick.AddListener(() => UpcastingLogic.CharacterLeftArrowClicked());

        Method = buttonsPanel.Find("Method/Text").GetComponent<TextMeshProUGUI>();
        MethodRight = buttonsPanel.Find("Method/Right").GetComponent<Button>();
        MethodLeft = buttonsPanel.Find("Method/Left").GetComponent<Button>();
        MethodRight.onClick.AddListener(() => UpcastingLogic.MethodRightArrowClicked());
        MethodLeft.onClick.AddListener(() => UpcastingLogic.MethodLeftArrowClicked());

        Quantity = buttonsPanel.Find("Quantity/Text").GetComponent<TextMeshProUGUI>();
        QuantityRight = buttonsPanel.Find("Quantity/Right").GetComponent<Button>();
        QuantityLeft = buttonsPanel.Find("Quantity/Left").GetComponent<Button>();
        QuantityRight.onClick.AddListener(() => UpcastingLogic.QuantityRightArrowClicked());
        QuantityLeft.onClick.AddListener(() => UpcastingLogic.QuantityLeftArrowClicked());

        CloseButton = buttonsPanel.Find("Close").GetComponent<Button>();
        CloseButton.onClick.AddListener(() => gameObject.SetActive(false));

        ConfirmButton = buttonsPanel.Find("Confirm").GetComponent<Button>();
        ConfirmButton.onClick.AddListener(() => ConfirmButtonClicked());
    }

    public void ToggleActivation() { gameObject.SetActive(!gameObject.activeSelf); }
    public void ConfirmButtonClicked()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        UpcastingLogic.Execute();
    }
}
