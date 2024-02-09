using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AccessModifierButton : MonoBehaviour
{
    public int AccessModifierIndex;
    public int AccessModifierCount;
    public List<Color> AccessModifierColors;
    
    public CharacterAttribute Attribute;
    public CharacterMethod Method;

    public Image ButtonImage;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        AccessModifierIndex = Attribute != null ? Attribute.AccessModifier.GetHashCode() : Method.AccessModifier.GetHashCode();
        AccessModifierCount = System.Enum.GetNames(typeof(AccessModifier)).Length;;
        AccessModifierColors = new List<Color> { new Color32(0, 255, 0, 200), new Color32(255, 165, 0, 200), new Color32(255, 0, 0, 200) };
        
        ButtonImage = GetComponent<Image>();
        ButtonImage.color = AccessModifierColors[AccessModifierIndex];

        GetComponent<Button>().onClick.AddListener(() => MarkAccessModifier());
    }
    
    private void MarkAccessModifier()
    { 
        AccessModifierIndex = (AccessModifierIndex + 1) % AccessModifierCount;

        if (Attribute != null) Attribute.AccessModifier = (AccessModifier)AccessModifierIndex;
        else Method.AccessModifier = (AccessModifier)AccessModifierIndex;
        
        ButtonImage.color = AccessModifierColors[AccessModifierIndex];
    }
}
