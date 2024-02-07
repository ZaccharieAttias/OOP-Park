using TMPro;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class CharacterUpcastMethod
{
    public Character Character;
    public CharacterMethod CharacterMethod;
    public float Quantity;

    public CharacterManager CharacterManager;
    public GameObject ContentPanel;
    public TextMeshProUGUI QuantityDescriptionText;

    public void Start()
    {
        CharacterManager = GameObject.Find("Player").GetComponent<CharacterManager>();

        ContentPanel = GameObject.Find("Canvas/Popups/SpecialAbilityCoolDown");
        QuantityDescriptionText = GameObject.Find("Canvas/Popups/SpecialAbilityCoolDown/Description").GetComponent<TextMeshProUGUI>();

        Button activateGameplayButton = GameObject.Find("Canvas/HTMenu/Menu/SwapScreen").GetComponent<Button>();
        activateGameplayButton.onClick.AddListener(() => ToggleOn());

        Button activateMenuButton = GameObject.Find("Canvas/GameplayScreen/SwapScreen").GetComponent<Button>();
        activateMenuButton.onClick.AddListener(() => ToggleOff());
    }

    public CharacterUpcastMethod(Character character, CharacterMethod characterMethod, float quantity)
    {
        Character = character;
        CharacterMethod = characterMethod;
        Quantity = quantity;

        Start();
    }

    public void UpdateUpcast(float quantity) 
    { 
        Quantity -= quantity;
        
        if (Quantity <= 0) 
        {
            Character.UpcastMethod = null;
            GameObject.Find("Player").GetComponent<CharacterManager>().DisplayCharacter(Character);
            ToggleOff();
        }

        else { QuantityDescriptionText.text = ((int)Quantity).ToString(); }
   }

   public void ToggleOn() { if (Character.UpcastMethod != null && Quantity > 0) { QuantityDescriptionText.text = ((int)Quantity).ToString(); ContentPanel.SetActive(true); }}
   private void ToggleOff() { ContentPanel.SetActive(false); }
}