using UnityEngine;
using UnityEngine.UI;

public class AccessModifierButton : MonoBehaviour
{
    private Image _buttonImage;

    private CharacterAttribute _attribute;
    private CharacterMethod _method;

    private Color _privateColor;
    private Color _protectedColor;
    private Color _publicColor;

    private void Start()
    {
        _buttonImage = GetComponent<Image>();

        _privateColor = new Color32(255, 0, 0, 200);
        _protectedColor = new Color32(255, 165, 0, 200);
        _publicColor = new Color32(0, 255, 0, 200);

        GetComponent<Button>().onClick.AddListener(() => OnButtonClick());
        
        UpdateButtonVisual();
    }

    private void OnButtonClick()
    {
        if (_attribute != null) _attribute.accessModifier = (AccessModifier)(((int)_attribute.accessModifier + 1) % 3);
        else if (_method != null) _method.accessModifier = (AccessModifier)(((int)_method.accessModifier + 1) % 3);

        UpdateButtonVisual();
    }

    private void UpdateButtonVisual()
    {
        AccessModifier modifier = (_attribute.name != null) ? _attribute.accessModifier : _method.accessModifier;
        
        switch (modifier)
        {
            case AccessModifier.Private:
                _buttonImage.color = _privateColor;
                break;

            case AccessModifier.Protected:
                _buttonImage.color = _protectedColor;
                break;

            case AccessModifier.Public:
                _buttonImage.color = _publicColor;
                break;
        }
    }

    public void setAttribute(CharacterAttribute attribute) { _attribute = attribute; }
    public void setMehod(CharacterMethod method) { _method = method; }
}
