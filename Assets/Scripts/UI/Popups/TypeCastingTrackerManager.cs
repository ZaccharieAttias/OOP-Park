using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TypeCastingTrackerManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject Popup;
    public TextMeshProUGUI AmountDescriptionText;


    public void Start()
    {
        InitializeUIElements();
        InitializeEventListeners();
    }
    public void InitializeUIElements()
    {
        Popup = GameObject.Find("Canvas/Popups/TypeCastingTracker");
        AmountDescriptionText = Popup.transform.Find("Description").GetComponent<TextMeshProUGUI>();
    }
    public void InitializeEventListeners()
    {
        Button activateGameplayButton = GameObject.Find("Canvas/Menus/CharacterCenter/SwapScreen").GetComponent<Button>();
        activateGameplayButton.onClick.AddListener(ToggleOn);

        Button activateMenuButton = GameObject.Find("Canvas/Menus/Gameplay/SwapScreen").GetComponent<Button>();
        activateMenuButton.onClick.AddListener(ToggleOff);
    }

    public void UpdateTypeCastingMethod(float amount)
    {
        CharactersManager charactersManager = CharactersData.CharactersManager;
        CharacterB currentCharacter = charactersManager.CurrentCharacter;

        currentCharacter.UpcastMethod.Amount -= amount;
        float updatedAmount = (int)currentCharacter.UpcastMethod.Amount;

        if (updatedAmount > 0) AmountDescriptionText.text = updatedAmount.ToString();
        else
        {
            currentCharacter.UpcastMethod = null;
            charactersManager.DisplayCharacter(currentCharacter);
            ToggleOff();
        }
    }

    public void ToggleOn()
    {
        if (CharactersData.CharactersManager.CurrentCharacter?.UpcastMethod != null) Popup.SetActive(true);
    }
    public void ToggleOff()
    {
        Popup.SetActive(false);
    }
}