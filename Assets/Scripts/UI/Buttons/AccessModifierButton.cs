using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AccessModifierButton : MonoBehaviour
{
    [Header("Attributes")]
    public int AccessModifierIndex;
    public int AccessModifierCount;
    public List<Color> AccessModifierColors;

    [Header("Associated Data")]
    public Attribute Attribute;
    public Method Method;

    [Header("UI Elements")]
    public Image ButtonImage;


    public void Start()
    {
        InitializeAttributes();
        InitializeUIElements();
    }
    public void InitializeAttributes()
    {
        AccessModifierIndex = Attribute != null ? Attribute.AccessModifier.GetHashCode() : Method.AccessModifier.GetHashCode();
        AccessModifierCount = System.Enum.GetNames(typeof(AccessModifier)).Length;
        AccessModifierColors = new List<Color>
        {
            new Color32(0, 255, 0, 200),
            new Color32(255, 165, 0, 200),
            new Color32(255, 0, 0, 200)
        };
    }
    public void InitializeUIElements()
    {
        ButtonImage = GetComponent<Image>();
        ButtonImage.color = AccessModifierColors[AccessModifierIndex];

        GetComponent<Button>().onClick.AddListener(() => MarkAccessModifier());
    }

    public void MarkAccessModifier()
    {
        // If the character is original and the scene is not Online Builder in play test mode, return
        if (CharactersData.CharactersManager.CurrentCharacter.IsOriginal && (!IsOnlineBuilder() || IsTestGameplay())) return;

        AccessModifierIndex = (AccessModifierIndex + 1) % AccessModifierCount;

        if (Attribute != null) Attribute.AccessModifier = (AccessModifier)AccessModifierIndex;
        else Method.AccessModifier = (AccessModifier)AccessModifierIndex;

        ButtonImage.color = AccessModifierColors[AccessModifierIndex];

        if (AccessModifierIndex == 2 && RestrictionManager.Instance.AllowAccessModifier)
        {
            CharacterB currentCharacter = CharactersData.CharactersManager.CurrentCharacter;

            if (Attribute != null)
            {
                foreach (CharacterB child in currentCharacter.Childrens)
                {
                    AttributesData.AttributesManager.CancelAttributeReferences(child, Attribute);
                }
            }

            else
            {
                foreach (CharacterB child in currentCharacter.Childrens)
                {
                    MethodsData.MethodsManager.CancelMethodReferences(child, Method);
                }
            }
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