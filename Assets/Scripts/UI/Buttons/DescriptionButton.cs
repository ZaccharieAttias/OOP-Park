using UnityEngine;
using UnityEngine.EventSystems;


public class DescriptionButton : MonoBehaviour, IPointerClickHandler
{
    public CharacterManager CharacterManager;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties() { CharacterManager = GameObject.Find("Player").GetComponent<CharacterManager>(); }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            string gameObjectDescription = null;
            
            gameObjectDescription ??= CharacterManager.CurrentCharacter.Attributes.Find(item => item.Name == gameObject.name)?.Description;
            gameObjectDescription ??= CharacterManager.CurrentCharacter.Methods.Find(item => item.Name == gameObject.name)?.Description;
            gameObjectDescription ??= CharacterManager.CurrentCharacter.SpecialAbility.Name == gameObject.name ? CharacterManager.CurrentCharacter.SpecialAbility.Description : null;

            CharacterManager.DescriptionText.text = gameObjectDescription;
        }
    }
}