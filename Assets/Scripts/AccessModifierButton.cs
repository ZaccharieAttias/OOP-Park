using UnityEngine;
using UnityEngine.UI;


public class AccessModifierButton : MonoBehaviour
{
    private Image buttonImage;

    public CharacterAttribute associatedAttribute;
    public CharacterMethod associatedMethod;

    public Color publicColor;
    public Color protectedColor;
    public Color privateColor;


    private void Start()
    {
        buttonImage = GetComponent<Image>();
        UpdateButtonVisual();
    }

    public void OnButtonClick()
    {
        if (associatedAttribute != null)
        {
            associatedAttribute.accessModifier = (AccessModifier)(((int)associatedAttribute.accessModifier + 1) % 3);
        }

        if (associatedMethod != null)
        {
            associatedMethod.accessModifier = (AccessModifier)(((int)associatedMethod.accessModifier + 1) % 3);
        }

        UpdateButtonVisual();
    }

    private void UpdateButtonVisual()
    {
        AccessModifier modifier = (associatedAttribute.name != "") ?
            associatedAttribute.accessModifier : associatedMethod.accessModifier;
        
        switch (modifier)
        {
            case AccessModifier.Public:
                buttonImage.color = publicColor;
                break;

            case AccessModifier.Protected:
                buttonImage.color = protectedColor;
                break;

            case AccessModifier.Private:
                buttonImage.color = privateColor;
                break;
        }
    }
}
