using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UpcastingTrackerManager : MonoBehaviour
{
    public GameObject Popup;
    public TextMeshProUGUI AmountDescriptionText;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    { 
        Popup = GameObject.Find("Canvas/Popups/UpcastingTracker");
        AmountDescriptionText = Popup.transform.Find("Description").GetComponent<TextMeshProUGUI>();

        Button ActivateGameplayButton = GameObject.Find("Canvas/Menus/CharacterCenter/SwapScreen").GetComponent<Button>();
        ActivateGameplayButton.onClick.AddListener(() => ToggleOn());

        Button ActivateMenuButton = GameObject.Find("Canvas/Menus/Gameplay/SwapScreen").GetComponent<Button>();
        ActivateMenuButton.onClick.AddListener(() => ToggleOff());
    }


    public void UpdateUpcastingMethod(float amount)
    {
        CharactersManager charactersManager = CharactersData.CharactersManager;
        Character currentCharacter = charactersManager.CurrentCharacter;

        currentCharacter.UpcastMethod.Amount -= amount;
        amount = (int)currentCharacter.UpcastMethod.Amount;

        if (amount > 0) AmountDescriptionText.text = amount.ToString();

        else
        {
            currentCharacter.UpcastMethod = null;
            charactersManager.DisplayCharacter(currentCharacter);
            ToggleOff();
        }
    }

    public void ToggleOn() { if (CharactersData.CharactersManager.CurrentCharacter?.UpcastMethod is not null) Popup.SetActive(true); }
    public void ToggleOff() { Popup.SetActive(false); }
}
