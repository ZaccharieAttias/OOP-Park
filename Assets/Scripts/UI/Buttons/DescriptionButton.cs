using UnityEngine;
using UnityEngine.EventSystems;


public class DescriptionButton : MonoBehaviour, IPointerClickHandler
{
    public MarkButton MarkButton;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        MarkButton = GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Description").GetComponent<MarkButton>();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            string gameObjectDescription = null;
            
            gameObjectDescription ??= CharactersData.CharactersManager.CurrentCharacter.Attributes.Find(item => item.Name == gameObject.name)?.Description;
            gameObjectDescription ??= CharactersData.CharactersManager.CurrentCharacter.Methods.Find(item => item.Name == gameObject.name)?.Description;
            gameObjectDescription ??= CharactersData.CharactersManager.CurrentCharacter.SpecialAbility?.Name == gameObject.name ? CharactersData.CharactersManager.CurrentCharacter.SpecialAbility?.Description : null;

            CharactersData.CharactersManager.DescriptionText.text = gameObjectDescription;
            

            var attribute = CharactersData.CharactersManager.CurrentCharacter.Attributes.Find(item => item.Name == gameObject.name);
            if (attribute != null && attribute.Owner == CharactersData.CharactersManager.CurrentCharacter.Name && RestrictionManager.Instance.AllowEncapsulation)
            {
                MarkButton.ActivateButtons();
                MarkButton.AttributeClicked(attribute);
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "OnlineBuilder")
                    GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Value").GetComponent<AttributeValueManager>().SetAttribute(attribute);
            }
            else
            {
                MarkButton.DeactivateButtons();
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "OnlineBuilder")
                    if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "OnlineBuilder")GameObject.Find("Canvas/Menus/CharacterCenter/Characters/Details/Value").GetComponent<AttributeValueManager>().SetAttribute(null);
            }
        }
    }
}