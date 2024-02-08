using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UpcastTrackerManager : MonoBehaviour
{
    public GameObject Popup;
    public TextMeshProUGUI AmountDescriptionText;

    public Button ActivateGameplayButton;
    public Button ActivateMenuButton;

    public CharacterManager CharacterManager;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    { 
        Popup = GameObject.Find("Canvas/Popups/UpcastTracker");
        AmountDescriptionText = Popup.transform.Find("Description").GetComponent<TextMeshProUGUI>();

        ActivateGameplayButton = GameObject.Find("Canvas/HTMenu/Menu/SwapScreen").GetComponent<Button>();
        ActivateGameplayButton.onClick.AddListener(() => ToggleOn());

        ActivateMenuButton = GameObject.Find("Canvas/GameplayScreen/SwapScreen").GetComponent<Button>();
        ActivateMenuButton.onClick.AddListener(() => ToggleOff());

        CharacterManager = GameObject.Find("Player").GetComponent<CharacterManager>(); 
    }

    public void UpdateUpcastingMethod(float amount)
    {
        CharacterManager.CurrentCharacter.UpcastMethod.Amount -= amount;
        amount = (int)CharacterManager.CurrentCharacter.UpcastMethod.Amount;
        if (amount > 0) { AmountDescriptionText.text = amount.ToString(); }

        else
        {
            CharacterManager.CurrentCharacter.UpcastMethod = null;
            CharacterManager.DisplayCharacter(CharacterManager.CurrentCharacter);
            ToggleOff();
        }
    }

    public void ToggleOn() { if (CharacterManager.CurrentCharacter.UpcastMethod != null) Popup.SetActive(true);}
    public void ToggleOff() { Popup.SetActive(false); }
}