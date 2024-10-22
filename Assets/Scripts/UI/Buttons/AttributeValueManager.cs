using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class AttributeValueManager : MonoBehaviour
{
    [Header("Scripts")]
    public Powerup Powerup;

    [Header("UI Elements")]
    public GameObject Popup;
     public TextMeshProUGUI Value;

    [Header("Buttons")]
    public Button UpButton;
    public Button DownButton;

    [Header("Attribute Data")]
    public Attribute Attribute;


    public void Start()
    {
        InitializeScripts();
        InitializeUIElements();
        InitializeButtons();
        InitializeAttributeData();

    }
    public void InitializeScripts()
    {
        Powerup = GameObject.Find("Scripts/PowerUp").GetComponent<Powerup>();
    }
    public void InitializeUIElements()
    {
        Popup = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Value");
        Value = Popup.transform.Find("Value").GetComponent<TextMeshProUGUI>();
    }
    public void InitializeButtons()
    {
        UpButton = Popup.transform.Find("Up").GetComponent<Button>();
        UpButton.onClick.AddListener(IncrementAttributeValue);

        DownButton = Popup.transform.Find("Down").GetComponent<Button>();
        DownButton.onClick.AddListener(DecrementAttributeValue);
    }
    public void InitializeAttributeData()
    {
        Attribute = null;
    }

    public void IncrementAttributeValue()
    {
        if (IsDecimal(Attribute.Value)) Attribute.Value += (Attribute.Value.ToString().Split(',')[1].Length == 1) ? 0.1f : 0.05f;
        else Attribute.Value++;

        UpdateValueDisplay();
        ApplyPowerup();
    }
    public void DecrementAttributeValue()
    {
        if (IsDecimal(Attribute.Value)) Attribute.Value -= (Attribute.Value.ToString().Split(',')[1].Length == 1) ? 0.1f : 0.05f;
        else Attribute.Value--;

        UpdateValueDisplay();
        ApplyPowerup();
    }

    public bool IsDecimal(float value)
    {
        return value.ToString().Contains(",");
    }
    public void UpdateValueDisplay()
    {
        Value.text = Attribute.Value.ToString("0.##");
    }
    public void ApplyPowerup()
    {
        Powerup.ApplyPowerup(CharactersData.CharactersManager.CurrentCharacter);
    }
    public void SetAttribute(Attribute attribute)
    {
        if (attribute != null)
        {
            Attribute = attribute;
            Value.text = attribute.Value.ToString("0.##");
            Popup.SetActive(true);
        }
        else
        {
            Attribute = null;
            Value.text = "0";
            Popup.SetActive(false);
        }
    }
}