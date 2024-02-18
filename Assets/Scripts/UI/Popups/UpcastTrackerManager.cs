using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UpcastTrackerManager : MonoBehaviour
{
    public GameObject Popup;
    public TextMeshProUGUI AmountDescriptionText;

    public CharactersManager CharactersManager;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    { 
        Popup = GameObject.Find("Canvas/Popups/UpcastTracker");
        AmountDescriptionText = Popup.transform.Find("Description").GetComponent<TextMeshProUGUI>();

        Button ActivateGameplayButton = GameObject.Find("Canvas/HTMenu/Menu/SwapScreen").GetComponent<Button>();
        ActivateGameplayButton.onClick.AddListener(() => ToggleOn());

        Button ActivateMenuButton = GameObject.Find("Canvas/GameplayScreen/SwapScreen").GetComponent<Button>();
        ActivateMenuButton.onClick.AddListener(() => ToggleOff());

        CharactersManager = GameObject.Find("Player").GetComponent<CharactersManager>(); 
    }


    public void UpdateUpcastingMethod(float amount)
    {
        CharactersManager.CurrentCharacter.UpcastMethod.Amount -= amount;
        amount = (int)CharactersManager.CurrentCharacter.UpcastMethod.Amount;

        if (amount > 0) AmountDescriptionText.text = amount.ToString();

        else
        {
            CharactersManager.CurrentCharacter.UpcastMethod = null;
            CharactersManager.DisplayCharacter(CharactersManager.CurrentCharacter);
            ToggleOff();
        }
    }

    public void ToggleOn() { if (CharactersManager.CurrentCharacter?.UpcastMethod is not null) Popup.SetActive(true); }
    public void ToggleOff() { Popup.SetActive(false); }
}
