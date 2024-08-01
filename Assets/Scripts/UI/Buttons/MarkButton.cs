using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MarkButton : MonoBehaviour
{
    public GameObject GetButton;
    public GameObject SetButton;
    public GameObject AllButton;
    public Attribute Attribute;

    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        GetButton = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Description/G");
        SetButton = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Description/S");
        SetButton.GetComponent<Button>().onClick.AddListener(SetterMark);
        GetButton.GetComponent<Button>().onClick.AddListener(GetterMark);
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
        GetButton.GetComponent<Image>().color = Attribute.Getter ? Color.green : Color.white;
        SetButton.GetComponent<Image>().color = Attribute.Setter ? Color.green : Color.white;
    }

    public void ActivateButtons()
    {
        GetButton.SetActive(true);
        SetButton.SetActive(true);
        GetButton.GetComponent<Button>().interactable = !CharactersData.CharactersManager.CurrentCharacter.IsOriginal || RestrictionManager.Instance.OnlineBuild;
        SetButton.GetComponent<Button>().interactable = !CharactersData.CharactersManager.CurrentCharacter.IsOriginal || RestrictionManager.Instance.OnlineBuild;
    }

    public void DeactivateButtons()
    {
        GetButton.SetActive(false);
        SetButton.SetActive(false);
    }
}
