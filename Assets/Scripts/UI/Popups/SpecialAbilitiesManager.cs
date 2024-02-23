using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SpecialAbilitiesManager : MonoBehaviour
{
    public GameObject Popup;
    public SpecialAbility SelectedSpecialAbility;
    public Dictionary<SpecialAbilityType, List<SpecialAbility>> SpecialAbilitiesCollection;

    public Button ConfirmButton;
    public GameObject SpecialAbilityButton;
    public Transform SpecialAbilitiesContentPanel;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/SpecialAbility");
        SelectedSpecialAbility = null;
        SpecialAbilitiesCollection = new();
        
        ConfirmButton = Popup.transform.Find("Background/Foreground/Buttons/Confirm").GetComponent<Button>();
        ConfirmButton.onClick.AddListener(() => ToggleOff());
        ConfirmButton.interactable = false;

        SpecialAbilityButton = Resources.Load<GameObject>("Buttons/Default");
        SpecialAbilitiesContentPanel = Popup.transform.Find("Background/Foreground/Buttons/ScrollView/ViewPort/Content");
    }


    private void LoadPopup(List<Character> selectedCharacterParents)
    {
        ClearContentPanel();

        List<SpecialAbilityType> availableSpecialAbilityTypes = selectedCharacterParents.Select(item => item.SpecialAbility.Type).Distinct().ToList();        
        List<SpecialAbility> availableSpecialAbilities = availableSpecialAbilityTypes.SelectMany(abilityType => SpecialAbilitiesCollection[abilityType]).ToList();
        foreach (SpecialAbility specialAbility in availableSpecialAbilities)
        {
            GameObject specialAbilityGameObject = Instantiate(SpecialAbilityButton, SpecialAbilitiesContentPanel);
            specialAbilityGameObject.name = specialAbility.Name;

            TMP_Text specialAbilityButtonText = specialAbilityGameObject.GetComponentInChildren<TMP_Text>();
            specialAbilityButtonText.text = specialAbility.Name;

            Button specialAbilityButton = specialAbilityGameObject.GetComponent<Button>();
            specialAbilityButton.onClick.AddListener(() => MarkSpecialAbility(specialAbilityGameObject, specialAbility));
        }
    }
    private void ClearContentPanel()
    { 
        SelectedSpecialAbility = null;
        ConfirmButton.interactable = false;
        
        foreach (Transform specialAbilityTransform in SpecialAbilitiesContentPanel) Destroy(specialAbilityTransform.gameObject); 
    }
    private void MarkSpecialAbility(GameObject specialAbilityButton, SpecialAbility specialAbility)
    {
        bool isSelectedAlready = SelectedSpecialAbility is not null;
        bool isSameAbility = SelectedSpecialAbility?.Name == specialAbility.Name;

        if (isSelectedAlready && isSameAbility is false) return;

        Image image = specialAbilityButton.GetComponent<Image>();
        image.color = isSelectedAlready || isSameAbility ? Color.white : Color.green;
        
        SelectedSpecialAbility = isSelectedAlready || isSameAbility ? null : specialAbility;
        ConfirmButton.interactable = isSelectedAlready is false && isSameAbility is false;
    }

    public void ToggleOn(List<Character> selectedCharacterParents)
    {
        LoadPopup(selectedCharacterParents);
        Popup.SetActive(true); 
    }
    public void ToggleOff() { Popup.SetActive(false); }
}
