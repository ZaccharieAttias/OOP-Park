using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class SpecialAbilityManager : MonoBehaviour
{
    public Dictionary<AbilityType, List<CharacterSpecialAbility>> SpecialAbilitiesCollection;

    public GameObject Popup;
    public Transform ContentPanel;
    public GameObject ButtonPrefab;

    public CharacterSpecialAbility selectedAbility;
    public AbilityType TempType;
    public Button ConfirmButton;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void OnGameStart()
    {
        SpecialAbilityManager specialAbilityManager = GameObject.Find("Canvas/HTMenu/Popups/SpecialAbility").GetComponent<SpecialAbilityManager>();
        specialAbilityManager.InitializeProperties();
    }
    private void InitializeProperties()
    {
        Popup = GameObject.Find("Canvas/HTMenu/Popups/SpecialAbility");
        ContentPanel = GameObject.Find("Canvas/HTMenu/Popups/SpecialAbility/Background/Foreground/Buttons/ScrollView/ViewPort/Content").transform;
        ButtonPrefab = Resources.Load<GameObject>("Prefabs/Buttons/Default");

        ConfirmButton = GameObject.Find("Canvas/HTMenu/Popups/SpecialAbility/Background/Foreground/Buttons/Confirm").GetComponent<Button>();
        ConfirmButton.onClick.AddListener(() => onConfirmation());
        ConfirmButton.interactable = false;

        SpecialAbilitiesCollection = new Dictionary<AbilityType, List<CharacterSpecialAbility>>();
    }
    
    
    public void SelectAbility(List<Character> parents)
    {
        gameObject.SetActive(true);
        TempType = parents[0].SpecialAbility.Type;
        LoadPopup(TempType);
    }

    public void LoadPopup(AbilityType type)
    {
        ClearContentPanel();

        foreach (CharacterSpecialAbility specialAbility in SpecialAbilitiesCollection[type])
        {
            if (specialAbility.Type == type) continue;

            GameObject button = Instantiate(ButtonPrefab, ContentPanel);
            button.name = specialAbility.Name;

            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            buttonText.text = specialAbility.Name;

            MarkAbilityInPopup(button, specialAbility);
        }
    }
    private void MarkAbilityInPopup(GameObject specialAbilityButton, CharacterSpecialAbility specialAbility)
    {
        bool isAbilitySelected = selectedAbility != null;
        bool isSelected = isAbilitySelected && selectedAbility.Name == specialAbility.Name;

        Image image = specialAbilityButton.GetComponent<Image>();
        image.color = isSelected ? Color.green : Color.white;

        Button button = specialAbilityButton.GetComponent<Button>();
        button.onClick.AddListener(() => OnClick(specialAbility, isSelected));
    }
    private void OnClick(CharacterSpecialAbility specialAbility, bool isSelected)
    {
        if (isSelected) selectedAbility = null;
        else selectedAbility = specialAbility;

        ConfirmButton.interactable = selectedAbility != null;
        LoadPopup(TempType);
    }

    private void ClearContentPanel() { foreach (Transform child in ContentPanel) Destroy(child.gameObject); }


    private void onConfirmation()
    {
        gameObject.SetActive(false);

        ExecuteFactory executeFactory = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons/CharacterFactory/ExecuteFactory").GetComponent<ExecuteFactory>();
        executeFactory.ExecuteTemp();
    }
}