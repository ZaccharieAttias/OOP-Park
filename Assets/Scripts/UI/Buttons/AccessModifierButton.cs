using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AccessModifierButton : MonoBehaviour
{
    public int AccessModifierIndex;
    public int AccessModifierCount;
    public List<Color> AccessModifierColors;

    public Attribute Attribute;
    public Method Method;

    public Image ButtonImage;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        AccessModifierIndex = Attribute is not null ? Attribute.AccessModifier.GetHashCode() : Method.AccessModifier.GetHashCode();
        AccessModifierCount = System.Enum.GetNames(typeof(AccessModifier)).Length;
        AccessModifierColors = new List<Color>
        { 
            new Color32(0, 255, 0, 200),
            new Color32(255, 165, 0, 200),
            new Color32(255, 0, 0, 200) 
        };

        ButtonImage = GetComponent<Image>();
        ButtonImage.color = AccessModifierColors[AccessModifierIndex];

        GetComponent<Button>().onClick.AddListener(() => MarkAccessModifier());
    }


    private void MarkAccessModifier()
    {
        AccessModifierIndex = (AccessModifierIndex + 1) % AccessModifierCount;

        if (Attribute is not null) Attribute.AccessModifier = (AccessModifier)AccessModifierIndex;
        else Method.AccessModifier = (AccessModifier)AccessModifierIndex;

        ButtonImage.color = AccessModifierColors[AccessModifierIndex];

        if (AccessModifierIndex == 2 && RestrictionManager.Instance.AllowAccessModifiers)
        {
            Character currentCharacter = CharactersData.CharactersManager.CurrentCharacter;

            if (Attribute is not null) foreach (Character child in currentCharacter.Childrens) AttributesData.AttributesManager.CancelAttributeReferences(child, Attribute);
            else foreach (Character child in currentCharacter.Childrens) MethodsData.MethodsManager.CancelMethodReferences(child, Method);
        }

    }
}
