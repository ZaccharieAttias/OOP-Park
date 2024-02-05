using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SpecialAbilityManager : MonoBehaviour
{
    public GameObject Popup;
    public Dictionary<SpecialAbility, List<CharacterSpecialAbility>> SpecialAbilitiesCollection;

    public GameObject SpecialAbilityButton;
    public Transform ContentPanel;

    public Button ConfirmButton;

    public CharacterSpecialAbility selectedAbility;
    public List<SpecialAbility> AbilitiesType;

    public CharacterManager CharacterManager;
    public CharacterCreationManager CharacterCreationManager;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/SpecialAbility");
        SpecialAbilitiesCollection = new Dictionary<SpecialAbility, List<CharacterSpecialAbility>>();
        
        SpecialAbilityButton = Resources.Load<GameObject>("Buttons/Default");
        ContentPanel = Popup.transform.Find("Background/Foreground/Buttons/ScrollView/ViewPort/Content");

        ConfirmButton = Popup.transform.Find("Background/Foreground/Buttons/Confirm").GetComponent<Button>();
        ConfirmButton.onClick.AddListener(() => Execute());
        ConfirmButton.interactable = false;

        selectedAbility = null;
        AbilitiesType = new List<SpecialAbility>();

        CharacterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
        CharacterCreationManager = GameObject.Find("Canvas/Popups").GetComponent<CharacterCreationManager>();
    }

    private void LoadPopup()
    {
        ClearContentPanel();

        AbilitiesType = CharacterCreationManager.SelectedCharacterObjects.Select(item => item.SpecialAbility.Type).Distinct().ToList();        
        List<CharacterSpecialAbility> availableSpecialAbilities = AbilitiesType.SelectMany(abilityType => SpecialAbilitiesCollection[abilityType]).ToList();
        foreach (CharacterSpecialAbility specialAbility in availableSpecialAbilities)
        {
            GameObject specialAbilityGameObject = Instantiate(SpecialAbilityButton, ContentPanel);
            specialAbilityGameObject.name = specialAbility.Name;

            TMP_Text buttonText = specialAbilityGameObject.GetComponentInChildren<TMP_Text>();
            buttonText.text = specialAbility.Name;

            Button specialAbilityButton = specialAbilityGameObject.GetComponent<Button>();
            specialAbilityButton.onClick.AddListener(() => MarkSpecialAbility(specialAbilityGameObject, specialAbility));
        }
    }
    private void MarkSpecialAbility(GameObject specialAbilityButton, CharacterSpecialAbility specialAbility)
    {
        bool isSelected = selectedAbility != null;
        bool isSameAbility = selectedAbility?.Name == specialAbility.Name;

        if (isSelected && isSameAbility == false) return;

        Image image = specialAbilityButton.GetComponent<Image>();
        image.color = isSelected || isSameAbility ? Color.white : Color.green;
        
        selectedAbility = isSelected || isSameAbility ? null : specialAbility;

        ConfirmButton.interactable = isSelected == false && isSameAbility == false;
    }
    private void ClearContentPanel() { foreach (Transform child in ContentPanel) Destroy(child.gameObject); }

    private void Execute()
    {
        CharacterManager.CurrentCharacter.SpecialAbility = selectedAbility;
        
        selectedAbility = null;
        ConfirmButton.interactable = false;

        ToggleOff();
    }

    public void ToggleOn()
    { 
        LoadPopup(); 
        Popup.SetActive(true); 
    }
    public void ToggleOff() { Popup.SetActive(false); }
}
