using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class AttributeValueManager : MonoBehaviour
{
    public Button UpButton;
    public Button DownButton;
    public TextMeshProUGUI Value;
    public Attribute Attribute;

    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        UpButton.onClick.AddListener(() => UpAttributeValue());
        DownButton.onClick.AddListener(() => DownAttributeValue());
    }

    public void UpAttributeValue()
    {
        if (Attribute.Value.ToString().Contains(","))
        {
            if (Attribute.Value.ToString().Split(',')[1].Length == 1)
            {
                Attribute.Value += 0.1f;
            }
            else
            {
                Attribute.Value += 0.05f;
            }
        }
        else
        {
            Attribute.Value++;
        }
        Value.text = Attribute.Value.ToString();
        Powerup powerUp = GameObject.Find("Scripts/PowerUp").GetComponent<Powerup>();
        powerUp.ApplyPowerup(CharactersData.CharactersManager.CurrentCharacter);
    }
    public void DownAttributeValue()
    {
        if (Attribute.Value.ToString().Contains(","))
        {
            if (Attribute.Value.ToString().Split(',')[1].Length == 1)
            {
                Attribute.Value -= 0.1f;
            }
            else
            {
                Attribute.Value -= 0.05f;
            }
        }
        else
        {
            Attribute.Value--;
        }
        Value.text = Attribute.Value.ToString();
        Powerup powerUp = GameObject.Find("Scripts/PowerUp").GetComponent<Powerup>();
        powerUp.ApplyPowerup(CharactersData.CharactersManager.CurrentCharacter);
    }
    public void SetAttribute(Attribute attribute)
    {
        if (attribute != null)
        {
            gameObject.SetActive(true);
            Value.text = attribute.Value.ToString();
            Attribute = attribute;
        }
        else
        {
            gameObject.SetActive(false);
            Value.text = 0.ToString();
            Attribute = null;
        }
    }
}