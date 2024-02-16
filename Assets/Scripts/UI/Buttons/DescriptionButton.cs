using UnityEngine;
using UnityEngine.EventSystems;


public class DescriptionButton : MonoBehaviour, IPointerClickHandler
{
    public CharactersManager CharactersManager;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties() { CharactersManager = GameObject.Find("Player").GetComponent<CharactersManager>(); }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            string gameObjectDescription = null;
            
            gameObjectDescription ??= CharactersManager.CurrentCharacter.Attributes.Find(item => item.Name == gameObject.name)?.Description;
            gameObjectDescription ??= CharactersManager.CurrentCharacter.Methods.Find(item => item.Name == gameObject.name)?.Description;
            gameObjectDescription ??= CharactersManager.CurrentCharacter.SpecialAbility.Name == gameObject.name ? CharactersManager.CurrentCharacter.SpecialAbility.Description : null;

            CharactersManager.DescriptionText.text = gameObjectDescription;
        }
    }
}