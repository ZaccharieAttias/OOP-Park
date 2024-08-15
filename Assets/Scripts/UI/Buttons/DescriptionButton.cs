using UnityEngine;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using System;


public class DescriptionButton : MonoBehaviour, IPointerClickHandler
{
    [Header("UI Elements")]
    public MarkButton MarkButton;


    public void Start()
    {
        InitializeUIElements();
    }
    public void InitializeUIElements()
    {
        MarkButton = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Description").GetComponent<MarkButton>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            string gameObjectDescription = GetDescription();

            CharactersData.CharactersManager.DescriptionText.text = gameObjectDescription;

            var attribute = CharactersData.CharactersManager.CurrentCharacter.Attributes.Find(item => item.Name == gameObject.name);
            if (attribute != null && attribute.Owner == CharactersData.CharactersManager.CurrentCharacter.Name) HandleAttributeClick(attribute);
            else HandleAttributeClick(null);
        }
    }
    public string GetDescription()
    {
        string description = null;

        Attribute attribute = CharactersData.CharactersManager.CurrentCharacter.Attributes.Find(item => item.Name == gameObject.name);
        Method method = CharactersData.CharactersManager.CurrentCharacter.Methods.Find(item => item.Name == gameObject.name);
        SpecialAbility specialAbility = CharactersData.CharactersManager.CurrentCharacter.SpecialAbility?.Type.ToString() == gameObject.name && method == null ? CharactersData.CharactersManager.CurrentCharacter.SpecialAbility : null;
        
        if (attribute != null) description = Regex.Replace(attribute.Description, @"(-)?\d+(\.\d+)?", attribute.Value.ToString("0.00"));
        if (method != null) description = Regex.Replace(method.Description, @"(-)?\d+(\.\d+)?", method.Attribute.Value.ToString("0.00"));
        if (specialAbility != null) description = Regex.Replace(specialAbility.Description, @"(-)?\d+(\.\d+)?", specialAbility.Value.ToString("0.00"));

        return description;
    }
    public void HandleAttributeClick(Attribute attribute)
    {
        if (attribute != null)
        {
            if (RestrictionManager.Instance.AllowEncapsulation) { MarkButton.ActivateButtons(); MarkButton.AttributeClicked(attribute);}
            if (IsOnlineBuilder() && !IsTestGameplay()) GameObject.Find("Canvas/Menus").GetComponent<AttributeValueManager>().SetAttribute(attribute);
        }

        else
        {
            if (RestrictionManager.Instance.AllowEncapsulation) MarkButton.DeactivateButtons();

            if (IsOnlineBuilder() && !IsTestGameplay()) GameObject.Find("Canvas/Menus").GetComponent<AttributeValueManager>().SetAttribute(null);
        }
    }

    public bool IsOnlineBuilder()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "OnlineBuilder";
    }
    public bool IsTestGameplay()
    {
        return GameObject.Find("Scripts/PlayTestManager").GetComponent<PlayTestManager>().IsTestGameplay;
    }
}