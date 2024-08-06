using UnityEngine;
using UnityEngine.EventSystems;


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
        string gameObjectDescription = null;

        gameObjectDescription ??= CharactersData.CharactersManager.CurrentCharacter.Attributes.Find(item => item.Name == gameObject.name)?.Description;
        gameObjectDescription ??= CharactersData.CharactersManager.CurrentCharacter.Methods.Find(item => item.Name == gameObject.name)?.Description;
        gameObjectDescription ??= CharactersData.CharactersManager.CurrentCharacter.SpecialAbility?.Name == gameObject.name ? CharactersData.CharactersManager.CurrentCharacter.SpecialAbility?.Description : null;

        return gameObjectDescription;
    }
    public void HandleAttributeClick(Attribute attribute)
    {
        if (attribute != null && (RestrictionManager.Instance.AllowEncapsulation || RestrictionManager.Instance.OnlineBuild))
        {
            MarkButton.ActivateButtons();
            MarkButton.AttributeClicked(attribute);

            if (IsOnlineBuilder() && !IsTestGameplay()) GameObject.Find("Canvas/Menus").GetComponent<AttributeValueManager>().SetAttribute(attribute);
        }

        else
        {
            MarkButton.DeactivateButtons();

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