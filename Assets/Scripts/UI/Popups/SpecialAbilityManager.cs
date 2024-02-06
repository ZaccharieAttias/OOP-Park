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

    public CharacterSpecialAbility SelectedSpecialAbility;
    public List<SpecialAbility> AbilitiesType;

    public CharacterCreationManager CharacterCreationManager;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/SpecialAbility");
        SpecialAbilitiesCollection = new Dictionary<SpecialAbility, List<CharacterSpecialAbility>>();
        
        SpecialAbilityButton = Resources.Load<GameObject>("Buttons/Default");
        ContentPanel = Popup.transform.Find("Background/Foreground/Buttons/ScrollView/ViewPort/Content");

        ConfirmButton = Popup.transform.Find("Background/Foreground/Buttons/Confirm").GetComponent<Button>();
        ConfirmButton.onClick.AddListener(() => ToggleOff());
        ConfirmButton.interactable = false;

        SelectedSpecialAbility = null;
        AbilitiesType = new List<SpecialAbility>();

        CharacterCreationManager = GameObject.Find("Canvas/Popups").GetComponent<CharacterCreationManager>();
    }

    private void LoadPopup()
    {
        ResetPopup();

        AbilitiesType = CharacterCreationManager.SelectedCharacterParents.Select(item => item.SpecialAbility.Type).Distinct().ToList();        
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
        bool isSelected = SelectedSpecialAbility != null;
        bool isSameAbility = SelectedSpecialAbility?.Name == specialAbility.Name;

        if (isSelected && isSameAbility == false) return;

        Image image = specialAbilityButton.GetComponent<Image>();
        image.color = isSelected || isSameAbility ? Color.white : Color.green;
        
        SelectedSpecialAbility = isSelected || isSameAbility ? null : specialAbility;

        ConfirmButton.interactable = isSelected == false && isSameAbility == false;
    }
    private void ResetPopup()
    {
        ClearContentPanel();
        
        ConfirmButton.interactable = false;
        SelectedSpecialAbility = null;
        AbilitiesType.Clear();
    }
    private void ClearContentPanel() { foreach (Transform child in ContentPanel) Destroy(child.gameObject); }

    public void ToggleOn()
    {
        LoadPopup(); 
        Popup.SetActive(true); 
    }
    public void ToggleOff() { Popup.SetActive(false); }
}
