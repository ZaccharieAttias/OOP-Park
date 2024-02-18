using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SpecialAbilitiesManager : MonoBehaviour
{
    public GameObject Popup;
    public Dictionary<SpecialAbilityType, List<SpecialAbility>> SpecialAbilitiesCollection;

    public GameObject SpecialAbilityButton;
    public Transform ContentPanel;

    public Button ConfirmButton;

    public SpecialAbility SelectedSpecialAbility;
    public List<SpecialAbilityType> AbilitiesType;

    public CharactersCreationManager CharactersCreationManager;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/Popups/SpecialAbility");
        SpecialAbilitiesCollection = new Dictionary<SpecialAbilityType, List<SpecialAbility>>();
        
        SpecialAbilityButton = Resources.Load<GameObject>("Buttons/Default");
        ContentPanel = Popup.transform.Find("Background/Foreground/Buttons/ScrollView/ViewPort/Content");

        ConfirmButton = Popup.transform.Find("Background/Foreground/Buttons/Confirm").GetComponent<Button>();
        ConfirmButton.onClick.AddListener(() => ToggleOff());
        ConfirmButton.interactable = false;

        SelectedSpecialAbility = null;
        AbilitiesType = new List<SpecialAbilityType>();

        CharactersCreationManager = GameObject.Find("Canvas/Popups").GetComponent<CharactersCreationManager>();
    }

    private void LoadPopup()
    {
        ResetPopup();

        AbilitiesType = CharactersCreationManager.SelectedCharacterParents.Select(item => item.SpecialAbility.Type).Distinct().ToList();        
        List<SpecialAbility> availableSpecialAbilities = AbilitiesType.SelectMany(abilityType => SpecialAbilitiesCollection[abilityType]).ToList();
        foreach (SpecialAbility specialAbility in availableSpecialAbilities)
        {
            GameObject specialAbilityGameObject = Instantiate(SpecialAbilityButton, ContentPanel);
            specialAbilityGameObject.name = specialAbility.Name;

            TMP_Text buttonText = specialAbilityGameObject.GetComponentInChildren<TMP_Text>();
            buttonText.text = specialAbility.Name;

            Button specialAbilityButton = specialAbilityGameObject.GetComponent<Button>();
            specialAbilityButton.onClick.AddListener(() => MarkSpecialAbility(specialAbilityGameObject, specialAbility));
        }
    }
    private void MarkSpecialAbility(GameObject specialAbilityButton, SpecialAbility specialAbility)
    {
        bool isSelectedAlready = SelectedSpecialAbility != null;
        bool isSameAbility = SelectedSpecialAbility?.Name == specialAbility.Name;

        if (isSelectedAlready && isSameAbility == false) return;

        Image image = specialAbilityButton.GetComponent<Image>();
        image.color = isSelectedAlready || isSameAbility ? Color.white : Color.green;
        
        SelectedSpecialAbility = isSelectedAlready || isSameAbility ? null : specialAbility;

        ConfirmButton.interactable = isSelectedAlready == false && isSameAbility == false;
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
