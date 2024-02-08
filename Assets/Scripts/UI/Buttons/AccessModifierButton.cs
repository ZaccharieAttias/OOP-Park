using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AccessModifierButton : MonoBehaviour
{
    public List<Color> AccessModifierColors;

    public int AccessModifierIndex;
    public int AccessModifierCount;
    public AccessModifier AccessModifier;

    public Image ButtonImage;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        Color Public = new Color32(0, 255, 0, 200);
        Color Protected = new Color32(255, 165, 0, 200);
        Color Private = new Color32(255, 0, 0, 200);
        AccessModifierColors = new List<Color> { Public, Protected, Private };
        
        AccessModifierIndex = (int)AccessModifier;
        AccessModifierCount = System.Enum.GetNames(typeof(AccessModifier)).Length;

        ButtonImage = GetComponent<Image>();
        ButtonImage.color = AccessModifierColors[AccessModifierIndex];

        GetComponent<Button>().onClick.AddListener(() => MarkAccessModifier());
    }
    
    private void MarkAccessModifier()
    { 
        AccessModifierIndex = (AccessModifierIndex + 1) % AccessModifierCount;
        AccessModifier = (AccessModifier)AccessModifierIndex;
        
        ButtonImage.color = AccessModifierColors[AccessModifierIndex];
    }
}
