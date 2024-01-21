using UnityEngine;
using UnityEngine.UI;


public class AccessModifierButton : MonoBehaviour
{
    public Image ButtonImage;

    public CharacterAttribute Attribute;
    public CharacterMethod Method;

    public Color PrivateColor;
    public Color ProtectedColor;
    public Color PublicColor;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        ButtonImage = GetComponent<Image>();

        PrivateColor = new Color32(255, 0, 0, 200);
        ProtectedColor = new Color32(255, 165, 0, 200);
        PublicColor = new Color32(0, 255, 0, 200);

        GetComponent<Button>().onClick.AddListener(() => OnButtonClick());
        
        UpdateButtonVisual();
    }
    
    private void OnButtonClick()
    {
        if (Attribute != null) Attribute.AccessModifier = (AccessModifier)(((int)Attribute.AccessModifier + 1) % 3);
        if (Method != null) Method.AccessModifier = (AccessModifier)(((int)Method.AccessModifier + 1) % 3);

        UpdateButtonVisual();
    }
    private void UpdateButtonVisual()
    {
        AccessModifier accessModifier = (Attribute != null) ? Attribute.AccessModifier : Method.AccessModifier;
        switch (accessModifier)
        {
            case AccessModifier.Private:
                ButtonImage.color = PrivateColor;
                break;

            case AccessModifier.Protected:
                ButtonImage.color = ProtectedColor;
                break;

            case AccessModifier.Public:
                ButtonImage.color = PublicColor;
                break;
        }
    }
}
