using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SpecialAbilitiesManager : MonoBehaviour
{
    public GameObject Popup;
    public List<SpecialAbilityType> AbilitiesType;
    public Dictionary<SpecialAbilityType, List<SpecialAbility>> SpecialAbilitiesCollection;

    public GameObject SpecialAbilityButton;
    public Transform SpecialAbilitiesContentPanel;

    public SpecialAbility SelectedSpecialAbility;
    public Button ConfirmButton;

    public CharactersCreationManager CharactersCreationManager;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/SpecialAbility");
        AbilitiesType = new List<SpecialAbilityType>();
        SpecialAbilitiesCollection = new Dictionary<SpecialAbilityType, List<SpecialAbility>>();
        
        SpecialAbilityButton = Resources.Load<GameObject>("Buttons/Default");
        SpecialAbilitiesContentPanel = Popup.transform.Find("Background/Foreground/Buttons/ScrollView/ViewPort/Content");

        SelectedSpecialAbility = null;
        ConfirmButton = Popup.transform.Find("Background/Foreground/Buttons/Confirm").GetComponent<Button>();
        ConfirmButton.onClick.AddListener(() => ToggleOff());
        ConfirmButton.interactable = false;

        CharactersCreationManager = GameObject.Find("Canvas/Popups").GetComponent<CharactersCreationManager>();
    }


    private void LoadPopup()
    {
        ClearContentPanel();

        AbilitiesType = CharactersCreationManager.SelectedCharacterParents.Select(item => item.SpecialAbility.Type).Distinct().ToList();        
        List<SpecialAbility> availableSpecialAbilities = AbilitiesType.SelectMany(abilityType => SpecialAbilitiesCollection[abilityType]).ToList();
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
    private void ClearContentPanel() { foreach (Transform specialAbilityTransform in SpecialAbilitiesContentPanel) Destroy(specialAbilityTransform.gameObject); }
    private void MarkSpecialAbility(GameObject specialAbilityButton, SpecialAbility specialAbility)
    {
        bool isSelectedAlready = SelectedSpecialAbility is not null;
        bool isSameAbility = SelectedSpecialAbility?.Name == specialAbility.Name;

        if (isSelectedAlready && isSameAbility == false) return;

        Image image = specialAbilityButton.GetComponent<Image>();
        image.color = isSelectedAlready || isSameAbility ? Color.white : Color.green;
        
        SelectedSpecialAbility = isSelectedAlready || isSameAbility ? null : specialAbility;
        ConfirmButton.interactable = isSelectedAlready == false && isSameAbility == false;
    }

    public void ToggleOn()
    {
        ConfirmButton.interactable = false;
        SelectedSpecialAbility = null;
        AbilitiesType.Clear();
        
        LoadPopup(); 
        Popup.SetActive(true); 
    }
    public void ToggleOff() { Popup.SetActive(false); }
}
