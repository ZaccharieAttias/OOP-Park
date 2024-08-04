using UnityEngine;
using UnityEngine.UI;


public class MarkButton : MonoBehaviour
{
    [Header("Buttons")]
    public GameObject GetButton;
    public GameObject SetButton;
    public GameObject AllButton;

    [Header("Attribute")]
    public Attribute Attribute;


    public void Start()
    {
        InitializeButtons();
    }
    public void InitializeButtons()
    {
        GetButton = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Description/G");
        GetButton.GetComponent<Button>().onClick.AddListener(GetterMark);

        SetButton = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Description/S");
        SetButton.GetComponent<Button>().onClick.AddListener(SetterMark);
    }

    public void SetterMark()
    {
        Attribute.Setter = !Attribute.Setter;

        SetButton.GetComponent<Image>().color = Attribute.Setter ? Color.green : Color.white;
    }
    public void GetterMark()
    {
        Attribute.Getter = !Attribute.Getter;

        GetButton.GetComponent<Image>().color = Attribute.Getter ? Color.green : Color.white;
    }
    public void AttributeClicked(Attribute attribute)
    {
        Attribute = attribute;

        UpdateButtonColors();
    }
    public void UpdateButtonColors()
    {
        GetButton.GetComponent<Image>().color = Attribute.Getter ? Color.green : Color.white;
        SetButton.GetComponent<Image>().color = Attribute.Setter ? Color.green : Color.white;
    }
    public void ActivateButtons()
    {
        SetButtonsActive(true);
        UpdateButtonInteractivity();
    }
    public void DeactivateButtons()
    {
        SetButtonsActive(false);
    }
    public void SetButtonsActive(bool isActive)
    {
        GetButton.SetActive(isActive);
        SetButton.SetActive(isActive);
    }
    public void UpdateButtonInteractivity()
    {
        bool isInteractable = !CharactersData.CharactersManager.CurrentCharacter.IsOriginal || RestrictionManager.Instance.OnlineBuild;

        GetButton.GetComponent<Button>().interactable = isInteractable;
        SetButton.GetComponent<Button>().interactable = isInteractable;
    }
}